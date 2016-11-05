"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var pickerTestModel_1 = require('./models/pickerTestModel');
var constantStorage_1 = require('./services/constantStorage');
var httpService_1 = require('./services/httpService');
var PickerTest = (function () {
    function PickerTest(httpService) {
        this.httpService = httpService;
        this.EnRuTest = "EnRuTest";
        this.RuEnTest = "RuEnTest";
        this.PickerTestsController = '/api/PickerTests';
        this.WordsLevelUpdaterController = 'api/WordsLevelUpdater';
        this.testSet = new Array();
        this.initEmptyCurrentTest();
    }
    PickerTest.prototype.prepareTest = function (testName) {
        var _this = this;
        this.testName = testName == this.EnRuTest ? this.EnRuTest : this.RuEnTest;
        console.log("prepareTest: " + this.testName);
        var url = this.PickerTestsController + "?userId=" + constantStorage_1.ConstantStorage.getUserId() + "&testType=" + this.testName;
        this.httpService.processGet(url).subscribe(function (response) { return _this.startTest(response); });
    };
    PickerTest.prototype.startTest = function (tests) {
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
    };
    PickerTest.prototype.initEmptyCurrentTest = function () {
        this.currentTest = new pickerTestModel_1.PickerTestModel();
        this.currentTest.Answers = [];
        this.currentTest.Word = '';
    };
    PickerTest.prototype.setAnswer = function (answer) {
        if (!answer || this.trueAnswer) {
            return;
        }
        console.log("set: " + answer);
        this.choosenAnswer = answer;
    };
    PickerTest.prototype.pickAnswer = function (answer) {
        console.log("choosen: " + answer);
        if (!answer || this.trueAnswer) {
            return;
        }
        this.trueAnswer = this.currentTest.Answers[this.currentTest.AnswerId];
        var isTrueAnswer = false;
        if (this.trueAnswer != answer) {
            this.resultMessage = "Error!<br/>\"" + this.currentTest.Word + "\" = \"" + this.trueAnswer + "\"";
            this.failedCount++;
        }
        else {
            isTrueAnswer = true;
            this.setNextTest();
        }
        this.sendTestResult(this.currentTest.WordId, isTrueAnswer);
    };
    PickerTest.prototype.sendTestResult = function (wordId, isTrueAnswer) {
        var postObject = {
            WordId: wordId,
            IsTrueAnswer: isTrueAnswer,
            TestType: this.testName
        };
        this.httpService
            .processPost(postObject, this.WordsLevelUpdaterController)
            .subscribe();
    };
    PickerTest.prototype.setNextTest = function () {
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
            this.resultMessage = "Finish! Errors: " + this.failedCount;
            this.testIsFinished = true;
            this.trueAnswer = 'disable pick button';
            return;
        }
        this.currentTest = this.testSet[this.testIndex];
        console.dir(this.currentTest);
    };
    PickerTest = __decorate([
        core_1.Component({
            selector: 'picker-test',
            templateUrl: '../../scripts/angularjs/app/templates/pickerTest.html'
        }), 
        __metadata('design:paramtypes', [httpService_1.HttpService])
    ], PickerTest);
    return PickerTest;
}());
exports.PickerTest = PickerTest;
