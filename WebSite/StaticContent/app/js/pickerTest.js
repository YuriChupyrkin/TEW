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
var core_1 = require("@angular/core");
var pickerTestModel_1 = require("./models/pickerTestModel");
var constantStorage_1 = require("./services/constantStorage");
var httpService_1 = require("./services/httpService");
var word_1 = require("./models/word");
var wordsCloudModel_1 = require("./models/wordsCloudModel");
var ModalWindow_1 = require("./helpComponents/ModalWindow");
var PickerTest = (function () {
    function PickerTest(httpService) {
        this.httpService = httpService;
        this.EnRuTest = "EnRuTest";
        this.RuEnTest = "RuEnTest";
        this.testSet = new Array();
        this.progress = 3;
        this.firstTestNOTloaded = true;
        this.initEmptyCurrentTest();
    }
    PickerTest.prototype.prepareTest = function (testName) {
        var _this = this;
        this.progress = 5;
        this.firstTestNOTloaded = false;
        this.initEmptyCurrentTest();
        this.testName = testName == this.EnRuTest ? this.EnRuTest : this.RuEnTest;
        var url = constantStorage_1.ConstantStorage.getPickerTestsController() + "?userId=" + constantStorage_1.ConstantStorage.getUserId() + "&testType=" + this.testName;
        this.httpService.processGet(url).subscribe(function (response) { return _this.startTest(response); }, function (error) {
            _this.showError('"Your words" should be have than 4 words');
            console.dir(error);
        });
    };
    PickerTest.prototype.startTest = function (tests) {
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
    };
    PickerTest.prototype.initEmptyCurrentTest = function () {
        this.currentTest = new pickerTestModel_1.PickerTestModel();
        this.currentTest.Answers = [];
        this.currentTest.Word = '';
        this.choosenAnswer = '';
        this.trueAnswer = '';
        this.resultMessage = '';
    };
    PickerTest.prototype.setAnswer = function (answer) {
        if (!answer || this.trueAnswer) {
            return;
        }
        this.choosenAnswer = answer;
    };
    PickerTest.prototype.pickAnswer = function (answer) {
        if (!answer || this.trueAnswer) {
            return;
        }
        this.trueAnswer = this.currentTest.Answers[this.currentTest.AnswerId];
        var isTrueAnswer = false;
        if (this.trueAnswer != answer) {
            //this.resultMessage = `Error! "${this.currentTest.Word}" = "${this.trueAnswer}"`;
            var message = "\"" + this.currentTest.Word + "\" = \"" + this.trueAnswer + "\"";
            this.showFailedAnswerInModal(message);
            this.failedCount++;
        }
        else {
            isTrueAnswer = true;
        }
        this.sendTestResult(this.currentTest.WordId, isTrueAnswer);
        if (isTrueAnswer) {
            this.setNextTest();
        }
    };
    PickerTest.prototype.sendTestResult = function (wordId, isTrueAnswer) {
        var postObject = {
            WordId: wordId,
            IsTrueAnswer: isTrueAnswer,
            TestType: this.testName
        };
        this.httpService
            .processPost(postObject, constantStorage_1.ConstantStorage.getWordsLevelUpdaterController())
            .subscribe();
    };
    PickerTest.prototype.setNextTest = function () {
        this.choosenAnswer = '';
        this.trueAnswer = '';
        this.resultMessage = '';
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
            var message = "Errors!!: " + this.failedCount;
            this.resultMessage = message;
            this.testIsFinished = true;
            this.trueAnswer = 'disable pick button';
            this.showResultsInModal(message);
            return;
        }
        this.currentTest = this.testSet[this.testIndex];
    };
    PickerTest.prototype.helpPick = function () {
        if (0 != this.currentTest.AnswerId) {
            this.currentTest.Answers.splice(0, 1);
            this.currentTest.AnswerId--;
        }
        else {
            this.currentTest.Answers.splice(1, 1);
        }
    };
    PickerTest.prototype.deleteWord = function (pickerTestModel) {
        var wordsCloudModel = new wordsCloudModel_1.WordsCloudModel();
        wordsCloudModel.UserName = constantStorage_1.ConstantStorage.getUserName();
        var word = new word_1.Word();
        word.Id = pickerTestModel.WordId;
        word.English = pickerTestModel.Word;
        wordsCloudModel.Words = [word];
        var result = this.httpService.processPost(wordsCloudModel, constantStorage_1.ConstantStorage.getDeleteWordController());
        result.subscribe(function (response) { return console.dir(response); });
        this.setNextTest();
    };
    PickerTest.prototype.showError = function (message) {
        this.modalWindowConfig = {
            headerText: 'PAGE ERROR',
            bodyText: message,
            isCancelButton: true,
            cancelButtonText: 'Cancel'
        };
        ModalWindow_1.ModalWindow.showWindow();
    };
    PickerTest.prototype.showIsDeleteModal = function (pickerTestModel) {
        var _this = this;
        this.modalWindowConfig = {
            headerText: 'Delete',
            bodyText: "Delete word: \"" + pickerTestModel.Word + "\"?",
            isApplyButton: true,
            isCancelButton: true,
            applyButtonText: 'Yes',
            cancelButtonText: 'No',
            applyCallback: function () { return _this.deleteWord(pickerTestModel); }
        };
        ModalWindow_1.ModalWindow.showWindow();
    };
    PickerTest.prototype.showFailedAnswerInModal = function (message) {
        var _this = this;
        this.modalWindowConfig = {
            headerText: 'Error',
            bodyText: message,
            isApplyButton: true,
            isCancelButton: false,
            applyButtonText: 'ok',
            applyCallback: function () { return _this.setNextTest(); }
        };
        ModalWindow_1.ModalWindow.showWindow();
    };
    PickerTest.prototype.showResultsInModal = function (message) {
        var _this = this;
        this.modalWindowConfig = {
            headerText: 'Done',
            bodyText: message,
            isApplyButton: true,
            isCancelButton: false,
            applyButtonText: 'ok',
            applyCallback: function () { return _this.setNextTest(); }
        };
        ModalWindow_1.ModalWindow.showWindow();
    };
    PickerTest.prototype.modalApplied = function () {
        console.log('ModalApplied');
        //this.setNextTest();
    };
    return PickerTest;
}());
PickerTest = __decorate([
    core_1.Component({
        selector: 'picker-test',
        templateUrl: '../StaticContent/app/templates/pickerTest.html'
    }),
    __metadata("design:paramtypes", [httpService_1.HttpService])
], PickerTest);
exports.PickerTest = PickerTest;
