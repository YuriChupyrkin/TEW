import { Component, OnInit } from '@angular/core';
import { Router} from '@angular/router';
import { HttpService } from '../services/httpService';
import { ConstantStorage } from '../helpers/constantStorage';
import { WriteTestModel } from '../models/writeTestModel';
import { CommonHelper } from '../helpers/commonHelper';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ModalWindowServise } from '../services/modalWindowServise';

@Component({
    selector: 'write-test',
    templateUrl: '../StaticContent/app/templates/components/writeTest.html'
})

export class WriteTest implements OnInit {

    private testCount: number;
    private testIndex: number;
    private failedCount: number;
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
            answer: ['', Validators.compose([Validators.required, Validators.maxLength(25)])],
        });
    }

    private prepareTest() {
        this.currentTestModel = new WriteTestModel();
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
        console.dir(tests);

        this.testSet = tests;
        this.testCount = tests.length;
        this.testIndex = 0;
        this.failedCount = 0;
        this.currentTestModel = tests[0];
    }

    private checkAnswer(value: any) {
        let answer = value['answer'];
        this.answerForm.controls['answer'].setValue('');
        if (!answer) {
            return;
        }

        console.log(answer);

        let isTrueAnser = this.currentTestModel.TrueAnswer.toLowerCase() === answer.toLowerCase();

        if (!isTrueAnser) {
            this.failedCount++;
            console.log('failCount: ' + this.failedCount);
        }

        let postObject = {
            WordId: this.currentTestModel.EnRuWordId,
            IsTrueAnswer: isTrueAnser,
            TestType: this.isHelpUsed ? 'SpellingWithHelpTest' : 'SpellingTest'
        };

        this.httpService
            .processPost(postObject, ConstantStorage.getWordsLevelUpdaterController())
            .then();

        this.setNextTestModel();
    }

    private useHelp() {
        this.isHelpUsed = true;
        this.helpWord = 'I am help world';
    }

    private setNextTestModel() {
        this.testIndex++;

        if (this.testIndex >= this.testCount) {
            let modalWindowModel = CommonHelper.buildOkCancelModalConfig(
                `Test done (${this.testCount - this.failedCount}/${this.testCount})`,
                'Do you want pass test again?',
                this.prepareTest.bind(this),
                () => this.router.navigate(['/user-stat']));

            ModalWindowServise.showModalWindow(modalWindowModel);
            return;
        }

        this.currentTestModel = this.testSet[this.testIndex];
        this.isHelpUsed = false;
    }
}