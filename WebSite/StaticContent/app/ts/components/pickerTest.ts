import { Component } from '@angular/core';
import { PickerTestModel } from '../models/pickerTestModel';
import { ConstantStorage } from '../helpers/constantStorage';
import { HttpService } from '../services/httpService';
import { Word } from '../models/word';
import { WordsCloudModel } from '../models/wordsCloudModel';
import { ModalWindowServise } from '../services/modalWindowServise';
import { ModalWindowModel } from '../models/modalWindowModel';
import { CommonHelper } from '../helpers/commonHelper';
import { SelectableListItemModel } from '../models/selectableListItemModel';

@Component({
    selector: 'picker-test',
    templateUrl: '../StaticContent/app/templates/components/pickerTest.html',
    styleUrls: [
        '../StaticContent/app/css/components/common.css',
        '../StaticContent/app/css/components/tewTest.css'
    ]
})

export class PickerTest {
    private readonly EnRuTest: string = 'EnRuTest';
    private readonly RuEnTest: string = 'RuEnTest';

    private testSet: Array<PickerTestModel>;
    private testName: string;
    private testIndex: number;
    private testCount: number;
    private failedCount: number;
    private currentTest: PickerTestModel;
    private choosenAnswer: SelectableListItemModel;
    private testIsFinished: boolean;
    private firstTestNOTloaded: boolean;
    private progress: string;
    private selectableListItems: Array<SelectableListItemModel>;

    constructor(private httpService: HttpService) {
        this.testSet = new Array<PickerTestModel>();
        this.selectableListItems = new Array<SelectableListItemModel>();
        this.progress = '';

        this.firstTestNOTloaded = true;
        this.initEmptyCurrentTest();
    }

    private prepareTest(testName: string) {
        this.progress = '';

        this.firstTestNOTloaded = false;
        this.initEmptyCurrentTest();
        this.testName = testName === this.EnRuTest ? this.EnRuTest : this.RuEnTest;

        let url = `${ConstantStorage.getPickerTestsController()}?` +
            `userId=${ConstantStorage.getUserId()}&testType=${this.testName}`;
        this.httpService.processGet<Array<PickerTestModel>>(url).then(
            response => this.startTest(response),
            error => {
                CommonHelper.showError('"Your words" should be have than 4 words');
                console.dir(error);
            });
    }

    private startTest(tests: Array<PickerTestModel>) {
        console.dir(tests);
        if (!tests || tests.length === 0) {
            CommonHelper.showError('"Your words" should be have than 4 words');
            return;
        }

        this.testSet = tests;
        this.testIndex = 0;
        this.testCount = tests.length;
        this.failedCount = 0;
        this.currentTest = tests[this.testIndex];
        this.setSelectableListItems(this.currentTest);

        this.progress = `${this.testIndex + 1}/${this.testCount}`;
    }

    private initEmptyCurrentTest() {
        this.currentTest = new PickerTestModel();
        this.selectableListItems = new Array<SelectableListItemModel>();
        this.currentTest.Answers = [];
        this.currentTest.Word = '';

        this.choosenAnswer = new SelectableListItemModel();
    }

    private setAnswer(answer: SelectableListItemModel) {
        if (!answer.Value) {
            return;
        }

        this.choosenAnswer = answer;
    }

    private pickAnswer(answer: SelectableListItemModel) {
        if (!answer.Value) {
            return;
        }

        let trueAnswer = this.currentTest.Answers[this.currentTest.AnswerId];
        let isTrueAnswer = false;

        if (trueAnswer !== answer.Value) {
            let message = `"${this.currentTest.Word}" = "${trueAnswer}"`;

            // highlight true answer
            this.selectableListItems.forEach(item => {
                item.IsHighlighted = item.Value === trueAnswer;
            });

            this.showMessageAndNext(message, true);
            this.failedCount++;
        } else {
            isTrueAnswer = true;
        }

        this.sendTestResult(this.currentTest.WordId, isTrueAnswer);

        if (isTrueAnswer) {
            this.setNextTest();
        }
    }

    private sendTestResult(wordId: number, isTrueAnswer: boolean) {
        let postObject = {
            WordId: wordId,
            IsTrueAnswer: isTrueAnswer,
            TestType: this.testName
        };

        this.httpService
            .processPost(postObject, ConstantStorage.getWordsLevelUpdaterController())
            .then();
    }

    private setNextTest() {
        this.choosenAnswer = new SelectableListItemModel();
        this.testIndex++;

        // set progress
        this.progress = `${this.testIndex + 1}/${this.testCount}`;

        if (this.testIndex >= this.testCount) {
            let message = `Errors count: ${this.failedCount}`;
            this.testIsFinished = true;
            this.showMessageAndNext(message, false);

            // reset progress
            this.progress = '';
            this.firstTestNOTloaded = true;
            this.initEmptyCurrentTest();

            return;
        }

        this.currentTest = this.testSet[this.testIndex];
        this.setSelectableListItems(this.currentTest);
    }

    private helpPick() {
        if (this.currentTest.AnswerId !== 0) {
            this.currentTest.Answers.splice(0, 1);
            this.currentTest.AnswerId--;
        } else {
            this.currentTest.Answers.splice(1, 1);
        }

        this.choosenAnswer = new SelectableListItemModel();
        this.setSelectableListItems(this.currentTest);
    }

    private setSelectableListItems(pickerTestModel: PickerTestModel) {
        this.selectableListItems = pickerTestModel.Answers.map(answer => {
            return <SelectableListItemModel> { Value: answer, IsHighlighted: false };
        });
    }

    private deleteWord(pickerTestModel: PickerTestModel) {
        let wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.UserName = ConstantStorage.getUserName();
        let word = new Word();
        word.Id = pickerTestModel.WordId;
        word.English = pickerTestModel.Word;
        wordsCloudModel.Words = [word];

        let result = this.httpService.processPost(wordsCloudModel, ConstantStorage.getDeleteWordController());

        result.then(response => console.dir(response));
        this.setNextTest();
    }

    private showIsDeleteModal(pickerTestModel: PickerTestModel) {
        let modalWindowModel = CommonHelper.buildOkCancelModalConfig(
            `Remove word`,
            `Do you really want remove ${pickerTestModel.Word}`,
            this.deleteWord.bind(this, pickerTestModel));

        ModalWindowServise.showModalWindow(modalWindowModel);
    }

    private showMessageAndNext (message: string, isError: boolean) {
        let modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = isError ? 'Error' : 'Done';
        modalWindowModel.BodyText = message;
        modalWindowModel.IsApplyButton = true;
        modalWindowModel.IsCancelButton = false;
        modalWindowModel.ApplyButtonText = 'OK';

        if (isError) {
            modalWindowModel.ApplyCallback = () => this.setNextTest();
        }

        ModalWindowServise.showModalWindow(modalWindowModel);
    }
}