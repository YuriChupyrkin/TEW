import { Component } from '@angular/core';
import { PickerTestModel } from './models/pickerTestModel';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';
import { Word } from './models/word';
import { WordsCloudModel } from './models/wordsCloudModel';

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

        var url = `${this.PickerTestsController}?userId=${ConstantStorage.getUserId()}&testType=${this.testName}`;
        this.httpService.processGet<Array<PickerTestModel>>(url).subscribe(
            response => this.startTest(response),
            error => {
                alert(`"Your words" should be have than 3 words`);
                console.dir(error);
            });
    }

    private startTest(tests: Array<PickerTestModel>) {
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

        this.choosenAnswer = answer;
    }

    private pickAnswer(answer: string) {
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
    }

    private deleteWord(pickerTestModel: PickerTestModel) {
        if (confirm(`Delete word: "${pickerTestModel.Word}"?`) == false) {
            return;
        }

        var wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.UserName = ConstantStorage.getUserName();
        var word = new Word();
        word.Id = pickerTestModel.WordId;
        word.English = pickerTestModel.Word;
        wordsCloudModel.Words = [word];

        var result = this.httpService.processPost<WordsCloudModel>(wordsCloudModel, '/api/DeleteWord');

        result.subscribe(response => console.dir(response));
        this.setNextTest();
    }

    private helpPick() {
        if (0 != this.currentTest.AnswerId) {
            this.currentTest.Answers.splice(0, 1);
            this.currentTest.AnswerId--;
        } else {
            this.currentTest.Answers.splice(1, 1);
        }
    }
}