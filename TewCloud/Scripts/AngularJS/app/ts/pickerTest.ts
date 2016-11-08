import { Component } from '@angular/core';
import { PickerTestModel } from './models/pickerTestModel';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';

@Component({
    selector: 'picker-test',
    templateUrl: '../../scripts/angularjs/app/templates/pickerTest.html'
})

export class PickerTest {
    private EnRuTest: string = "EnRuTest";
    private RuEnTest: string = "RuEnTest";
    private PickerTestsController: string = '/api/PickerTests';
    private WordsLevelUpdaterController = 'api/WordsLevelUpdater';

    private testSet: Array<PickerTestModel>;
    private testName: string;
    private testIndex: number;
    private testCount: number;
    private failedCount: number;
    private currentTest: PickerTestModel;
    private choosenAnswer: string;
    private trueAnswer: string;
    private resultMessage: string;
    private testIsFinished: boolean;

    constructor(private httpService: HttpService) {
        this.testSet = new Array<PickerTestModel>();

        this.initEmptyCurrentTest();
    }

    private prepareTest(testName: string) {
        this.initEmptyCurrentTest();
        this.testName = testName == this.EnRuTest ? this.EnRuTest : this.RuEnTest;
        console.log("prepareTest: " + this.testName);

        var url = `${this.PickerTestsController}?userId=${ConstantStorage.getUserId()}&testType=${this.testName}`;
        this.httpService.processGet<Array<PickerTestModel>>(url).subscribe(response => this.startTest(response));
    }

    private startTest(tests: Array<PickerTestModel>) {
        if (tests.length < 4) {
            alert("you must add words for test");
            return;
        }

        console.dir(tests);
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
        this.resultMessage = '';
    }

    private setAnswer(answer: string) {
        if (!answer || this.trueAnswer) {
            return;
        }

        console.log("set: " + answer);
        this.choosenAnswer = answer;
    }

    private pickAnswer(answer: string) {
        console.log("choosen: " + answer);

        if (!answer || this.trueAnswer) {
            return;
        }

        this.trueAnswer = this.currentTest.Answers[this.currentTest.AnswerId];
        var isTrueAnswer = false;

        if (this.trueAnswer != answer) {
            this.resultMessage = `Error! "${this.currentTest.Word}" = "${this.trueAnswer}"`;
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
            .processPost(postObject, this.WordsLevelUpdaterController)
            .subscribe(r => console.dir(r));
    }

    private setNextTest() {
        this.choosenAnswer = '';
        this.trueAnswer = '';
        this.resultMessage = '';

        this.testIndex++;

        if (this.testIsFinished) {
            this.testIsFinished = false;
            this.initEmptyCurrentTest();
            this.prepareTest(this.testName);
            return;
        }

        if (this.testIndex >= this.testCount) {
            this.resultMessage = `Finish! Errors: ${this.failedCount}`;
            this.testIsFinished = true;
            this.trueAnswer = 'disable pick button';
            return;
        }

        this.currentTest = this.testSet[this.testIndex];
        console.dir(this.currentTest);
    }

    private deleteWord(pickerTestModel: PickerTestModel) {
        if (confirm(`Delete word: ${pickerTestModel.Word}`)) {
            alert(`deleted.... id: ${pickerTestModel.WordId}`);
        }
    }
}