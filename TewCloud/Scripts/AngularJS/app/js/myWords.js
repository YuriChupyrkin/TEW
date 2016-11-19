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
var httpService_1 = require('./services/httpService');
var userWords_1 = require('./models/userWords');
var wordsCloudModel_1 = require('./models/wordsCloudModel');
var constantStorage_1 = require('./services/constantStorage');
var MyWords = (function () {
    function MyWords(httpService) {
        this.httpService = httpService;
        this.wordsManagerController = '/api/WordsManager';
        this.deleteWordController = '/api/DeleteWord';
        this.userWords = new userWords_1.UserWords();
        this.loaded = false;
        this.getWords();
    }
    MyWords.prototype.getWords = function () {
        var _this = this;
        var url = this.wordsManagerController + '?userName=' + constantStorage_1.ConstantStorage.getUserName();
        var result = this.httpService.processGet(url);
        result.subscribe(function (json) { return _this.setUserWords(json); });
    };
    MyWords.prototype.removeWord = function (word) {
        var _this = this;
        console.dir(word);
        // hidden
        word.Hidden = true;
        var wordsCloudModel = new wordsCloudModel_1.WordsCloudModel();
        wordsCloudModel.UserName = constantStorage_1.ConstantStorage.getUserName();
        wordsCloudModel.Words = [word];
        var result = this.httpService.processPost(wordsCloudModel, this.deleteWordController);
        result.subscribe(function (response) { return _this.removedWord(word); }, function (error) { return word.Hidden = false; });
    };
    MyWords.prototype.setUserWords = function (userWords) {
        this.loaded = true;
        this.userWords = userWords;
        this.wordsCount = userWords.Words.length;
    };
    MyWords.prototype.removedWord = function (word) {
        var wordIndex = this.userWords.Words.indexOf(word);
        if (wordIndex == -1) {
            return;
        }
        this.userWords.Words.splice(wordIndex, 1);
        this.wordsCount--;
    };
    MyWords = __decorate([
        core_1.Component({
            selector: 'my-words',
            templateUrl: '../../scripts/angularjs/app/templates/myWords.html'
        }), 
        __metadata('design:paramtypes', [httpService_1.HttpService])
    ], MyWords);
    return MyWords;
}());
exports.MyWords = MyWords;
