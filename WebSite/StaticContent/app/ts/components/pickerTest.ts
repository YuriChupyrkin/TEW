import { Component } from '@angular/core';
import { PickerTestModel } from '../models/pickerTestModel';
import { ConstantStorage } from '../helpers/constantStorage';
import { HttpService } from '../services/httpService';
import { Word } from '../models/word';
import { WordsCloudModel } from '../models/wordsCloudModel';
import { ModalWindowServise } from '../services/modalWindowServise';
import { ModalWindowModel } from '../models/modalWindowModel';
import { CommonHelper } from '../helpers/commonHelper';

@Component({
    selector: 'picker-test',
    templateUrl: '../StaticContent/app/templates/components/pickerTest.html'
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
    private choosenAnswer: string;
    private trueAnswer: string;
    private testIsFinished: boolean;
    private firstTestNOTloaded: boolean;
    private progress: number;

    constructor(private httpService: HttpService) {
        this.testSet = new Array<PickerTestModel>();
        this.progress = 3;

        this.firstTestNOTloaded = true;
        this.initEmptyCurrentTest();
    }

    private prepareTest(testName: string) {
        this.progress = 3;

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
    }

    private initEmptyCurrentTest() {
        this.currentTest = new PickerTestModel();
        this.currentTest.Answers = [];
        this.currentTest.Word = '';

        this.choosenAnswer = '';
        this.trueAnswer = '';
    }

    private setAnswer(answer: string) {
        if (!answer || this.trueAnswer) {
            return;
        }

        this.choosenAnswer = answer;
    }

    private pickAnswer(answer: string) {
        if (!answer || this.trueAnswer) {
            return;
        }

        this.trueAnswer = this.currentTest.Answers[this.currentTest.AnswerId];
        let isTrueAnswer = false;

        if (this.trueAnswer !== answer) {
            let message = `"${this.currentTest.Word}" = "${this.trueAnswer}"`;
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
        this.choosenAnswer = '';
        this.trueAnswer = '';

        this.testIndex++;

        // set progress
        this.progress = Math.round(this.testIndex / this.testCount * 100);

        if (this.testIndex >= this.testCount) {
            let message = `Errors count: ${this.failedCount}`;
            this.testIsFinished = true;
            this.trueAnswer = 'disable pick button';
            this.showMessageAndNext(message, false);

            // reset progress
            this.progress = 0;
            this.firstTestNOTloaded = true;
            this.initEmptyCurrentTest();

            return;
        }

        this.currentTest = this.testSet[this.testIndex];
    }

    private helpPick() {
        if (0 !== this.currentTest.AnswerId) {
            this.currentTest.Answers.splice(0, 1);
            this.currentTest.AnswerId--;
        } else {
            this.currentTest.Answers.splice(1, 1);
        }
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

    // private showError(message: string) {
    //     let modalWindowModel = new ModalWindowModel();
    //     modalWindowModel.HeaderText = 'PAGE ERROR';
    //     modalWindowModel.BodyText = message;
    //     modalWindowModel.IsCancelButton = true;
    //     modalWindowModel.CancelButtonText = 'Cancel';

    //    ModalWindowServise.showModalWindow(modalWindowModel);
    // }

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