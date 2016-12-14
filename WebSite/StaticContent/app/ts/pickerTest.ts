import { Component } from '@angular/core';
import { PickerTestModel } from './models/pickerTestModel';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';
import { Word } from './models/word';
import { WordsCloudModel } from './models/wordsCloudModel';
import { ModalWindowServise } from './services/modalWindowServise';

@Component({
    selector: 'picker-test',
    templateUrl: '../StaticContent/app/templates/pickerTest.html'
})

export class PickerTest {
    private readonly EnRuTest: string = "EnRuTest";
    private readonly RuEnTest: string = "RuEnTest";

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
        this.progress = 5;

        this.firstTestNOTloaded = false;
        this.initEmptyCurrentTest();
        this.testName = testName == this.EnRuTest ? this.EnRuTest : this.RuEnTest;

        var url = `${ConstantStorage.getPickerTestsController()}?userId=${ConstantStorage.getUserId()}&testType=${this.testName}`;
        this.httpService.processGet<Array<PickerTestModel>>(url).then(
            response => this.startTest(response),
            error => {
                this.showError('"Your words" should be have than 4 words');
                console.dir(error);
            });
    }

    private startTest(tests: Array<PickerTestModel>) {
        console.dir(tests);
        if (!tests || tests.length == 0) {
            this.showError('"Your words" should be have than 4 words');
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
        var isTrueAnswer = false;

        if (this.trueAnswer != answer) {
            var message = `"${this.currentTest.Word}" = "${this.trueAnswer}"`;
            this.showFailedAnswerInModal(message);
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
        var postObject = {
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

        if (this.testIsFinished) {
            this.testIsFinished = false;
            this.initEmptyCurrentTest();
            this.prepareTest(this.testName);
            return;
        }

        if (this.testIndex >= this.testCount) {
            let message = `Errors!!: ${this.failedCount}`;
            this.testIsFinished = true;
            this.trueAnswer = 'disable pick button';
            this.showResultsInModal(message);
            return;
        }

        this.currentTest = this.testSet[this.testIndex];
    }

    private helpPick() {
        if (0 != this.currentTest.AnswerId) {
            this.currentTest.Answers.splice(0, 1);
            this.currentTest.AnswerId--;
        } else {
            this.currentTest.Answers.splice(1, 1);
        }
    }

    private deleteWord(pickerTestModel: PickerTestModel) {
        var wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.UserName = ConstantStorage.getUserName();
        var word = new Word();
        word.Id = pickerTestModel.WordId;
        word.English = pickerTestModel.Word;
        wordsCloudModel.Words = [word];

        var result = this.httpService.processPost<WordsCloudModel>(wordsCloudModel, ConstantStorage.getDeleteWordController());

        result.then(response => console.dir(response));
        this.setNextTest();
    }
     
    private showError(message: string) {
        let modalWindowConfig = {
             headerText: 'PAGE ERROR',
             bodyText: message,
             isCancelButton: true,
             cancelButtonText: 'Cancel'
        };

       ModalWindowServise.showModalWindow(modalWindowConfig);
    }

    private showIsDeleteModal(pickerTestModel: PickerTestModel) {
        let modalWindowConfig = {
             headerText: 'Delete',
             bodyText: `Delete word: "${pickerTestModel.Word}"?`,
             isApplyButton: true,
             isCancelButton: true,
             applyButtonText: 'Yes',
             cancelButtonText: 'No',
             applyCallback: () => this.deleteWord(pickerTestModel)
        };

        ModalWindowServise.showModalWindow(modalWindowConfig);
    }

    private showFailedAnswerInModal(message: string) {
        let modalWindowConfig = {
             headerText: 'Error',
             bodyText: message,
             isApplyButton: true,
             isCancelButton: false,
             applyButtonText: 'ok',
             applyCallback: () => this.setNextTest()
        }

        ModalWindowServise.showModalWindow(modalWindowConfig);
    }

    private showResultsInModal(message: string) {
        let modalWindowConfig = {
             headerText: 'Done',
             bodyText: message,
             isApplyButton: true,
             isCancelButton: false,
             applyButtonText: 'ok',
             applyCallback: () => this.setNextTest()
        }

        ModalWindowServise.showModalWindow(modalWindowConfig);
    }
}