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
var httpService_1 = require("./services/httpService");
var wordsCloudModel_1 = require("./models/wordsCloudModel");
var constantStorage_1 = require("./services/constantStorage");
var MyWords = (function () {
    function MyWords(httpService) {
        this.httpService = httpService;
        this.wordsPerPage = 100;
        this.words = new Array();
        this.currentPage = 1;
        this.getWords();
    }
    MyWords.prototype.doSomething = function (event) {
        var scrollTop = document.body.scrollTop;
        var scrollHeight = document.body.scrollHeight;
        var clientHeight = document.documentElement.clientHeight;
        console.log('---------------------------');
        console.debug("Scroll scrollTop", scrollTop);
        console.debug("Scroll scrollHeight", scrollHeight);
        console.debug("Scroll clientHeight", clientHeight);
        console.log('---------------------------');
        if (scrollTop + clientHeight + 50 >= scrollHeight) {
            console.log('!!!!!!!! LOAD !!!!!!!!!!!!!!');
            this.fakeAddWords();
        }
    };
    MyWords.prototype.fakeAddWords = function () {
        this.words.push.apply(this.words, this.words);
    };
    MyWords.prototype.getWords = function () {
        var _this = this;
        var url = constantStorage_1.ConstantStorage.getWordsManagerController() + "?userName=" + constantStorage_1.ConstantStorage.getUserName();
        var result = this.httpService.processGet(url);
        result.then(function (json) { return _this.setUserWords(json); });
    };
    MyWords.prototype.removeWord = function (word) {
        var _this = this;
        console.dir(word);
        // hidden
        word.Hidden = true;
        var wordsCloudModel = new wordsCloudModel_1.WordsCloudModel();
        wordsCloudModel.UserName = constantStorage_1.ConstantStorage.getUserName();
        wordsCloudModel.Words = [word];
        var result = this.httpService.processPost(wordsCloudModel, constantStorage_1.ConstantStorage.getDeleteWordController());
        result.then(function (response) { return _this.removedWord(word); }, function (error) { return word.Hidden = false; });
    };
    MyWords.prototype.setUserWords = function (userWords) {
        if (userWords.Words) {
            this.wordsCount = userWords.TotalWords;
            this.words = userWords.Words;
        }
    };
    MyWords.prototype.removedWord = function (word) {
        var wordIndex = this.words.indexOf(word);
        if (wordIndex == -1) {
            return;
        }
        this.words.splice(wordIndex, 1);
        this.wordsCount--;
    };
    return MyWords;
}());
__decorate([
    core_1.HostListener('window:scroll', ['$event']),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", void 0)
], MyWords.prototype, "doSomething", null);
MyWords = __decorate([
    core_1.Component({
        selector: 'my-words',
        templateUrl: '../StaticContent/app/templates/myWords.html'
    }),
    __metadata("design:paramtypes", [httpService_1.HttpService])
], MyWords);
exports.MyWords = MyWords;
