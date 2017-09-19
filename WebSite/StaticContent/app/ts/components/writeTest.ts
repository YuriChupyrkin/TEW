import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import { HttpService } from '../services/httpService';
import { ConstantStorage } from '../helpers/constantStorage';
import { WriteTestModel } from '../models/writeTestModel';
import { CommonHelper } from '../helpers/commonHelper';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ModalWindowServise } from '../services/modalWindowServise';
import { ModalWindowModel } from '../models/modalWindowModel';

@Component({
    selector: 'write-test',
    templateUrl: '../StaticContent/app/templates/components/writeTest.html',
    styleUrls: [
        '../StaticContent/app/css/components/common.css',
        '../StaticContent/app/css/components/tewTest.css'
    ]
})

export class WriteTest implements OnInit {

    private testCount: number;
    private testIndex: number;
    private failedCount: number;
    private progress: string;
    private isHelpUsed: boolean;
    private helpWord: string;
    private currentTestModel: WriteTestModel;
    private testSet: Array<WriteTestModel>;
    private answerForm: FormGroup;

    constructor(
        private httpService: HttpService,
        private formBuilder: FormBuilder,
        private router: Router) {
        this.prepareTest();
    }

    ngOnInit() {
        this.answerForm = this.formBuilder.group({
            answer: ['', Validators.compose([Validators.required])],
        });
    }

    private prepareTest() {
        this.progress = '';
        this.isHelpUsed = false;
        this.currentTestModel = new WriteTestModel();

        if (!CommonHelper.isApplicationReady()) {
            return;
        }

        let url = `${ConstantStorage.getWriteTestsController()}?` +
            `userId=${ConstantStorage.getUserId()}`;

        this.httpService.processGet<Array<WriteTestModel>>(url).then(
            response => this.startTest(response),
            error => {
                CommonHelper.showError('"Your words" should be have than 4 words');
                console.dir(error);
            });
    }

    private startTest(tests: Array<WriteTestModel>) {
        this.testSet = tests;
        this.testCount = tests.length;
        this.testIndex = 0;
        this.failedCount = 0;
        this.currentTestModel = tests[0];
        this.currentTestModel.Example
            = this.hideWordInExample(this.currentTestModel.TrueAnswer, this.currentTestModel.Example);

        this.progress = `${this.testIndex + 1}/${this.testCount}`;
    }

    private checkAnswer(value: any) {
        let answer = value['answer'];
        this.answerForm.controls['answer'].setValue('');
        if (!answer) {
            return;
        }

        let isTrueAnser = this.currentTestModel.TrueAnswer.toLowerCase() === answer.toLowerCase();

        let postObject = {
            WordId: this.currentTestModel.EnRuWordId,
            IsTrueAnswer: isTrueAnser,
            TestType: this.isHelpUsed ? 'SpellingWithHelpTest' : 'SpellingTest'
        };

        this.httpService
            .processPost(postObject, ConstantStorage.getWordsLevelUpdaterController())
            .then();

         if (!isTrueAnser) {
            this.failedCount++;
            this.showTrueAnswer(`"${this.currentTestModel.Word}" === "${this.currentTestModel.TrueAnswer}"`);
        } else {
            this.setNextTestModel();
        }
    }

    private useHelp() {
        this.isHelpUsed = true;
        let answer = this.currentTestModel.TrueAnswer;

        if (answer.length < 3) {
            this.helpWord = '';
            return;
        }

        let helpWord = answer;
        let maxIteration = 30;
        let iterationCount = 0;

        while (helpWord === answer && maxIteration > iterationCount) {
            helpWord = answer.split('').sort(() => 0.5 - Math.random()).join('');
            iterationCount++;
        }

        this.helpWord = helpWord;
    }

    private setNextTestModel() {
        this.testIndex++;

        if (this.testIndex >= this.testCount) {
            let modalWindowModel = CommonHelper.buildOkCancelModalConfig(
                `Test done (${this.testCount - this.failedCount}/${this.testCount})`,
                'Do you want pass test again?',
                this.prepareTest.bind(this),
                () => this.router.navigate(['/user-stat']));

            setTimeout(() => {
                ModalWindowServise.showModalWindow(modalWindowModel);
            }, 300);
            return;
        }

        this.progress = `${this.testIndex + 1}/${this.testCount}`;
        this.currentTestModel = this.testSet[this.testIndex];
        this.currentTestModel.Example
            = this.hideWordInExample(this.currentTestModel.TrueAnswer, this.currentTestModel.Example);
        this.isHelpUsed = false;
    }

    private hideWordInExample(word: string, example: string) {
        if (!example || !word) {
            return '';
        }

        // 70% length
        let searchCriteriaLength = Math.round(word.length / 100 * 70);
        let searchCriteria = word.substring(0, searchCriteriaLength).toLowerCase();
        let exampleItems = example.split(' ');

        let matches = exampleItems.filter((item) => {
            return item.toLowerCase().indexOf(searchCriteria) === 0;
        });

        matches.forEach((match) => example = example.replace(new RegExp(match, 'g'), '***'));
        return example;
    }

    private showTrueAnswer (message: string) {
        let modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = 'Error';
        modalWindowModel.BodyText = message;
        modalWindowModel.IsApplyButton = true;
        modalWindowModel.IsCancelButton = false;
        modalWindowModel.ApplyButtonText = 'OK';
        modalWindowModel.ApplyCallback = () => this.setNextTestModel();

        ModalWindowServise.showModalWindow(modalWindowModel);
    }
}
