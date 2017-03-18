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
var httpService_1 = require("../services/httpService");
var wordsCloudModel_1 = require("../models/wordsCloudModel");
var constantStorage_1 = require("../helpers/constantStorage");
var MyWords = (function () {
    function MyWords(httpService) {
        this.httpService = httpService;
        this.wordsPerPage = 50;
        this.isLoading = false;
        this.words = new Array();
        this.wordsCount = 999999;
        this.sortKey = 'level';
        this.sortAsc = true;
        this.loadWords();
    }
    MyWords.prototype.removeWord = function (word) {
        var _this = this;
        this.startLoading();
        // hidden
        word.Hidden = true;
        var wordsCloudModel = new wordsCloudModel_1.WordsCloudModel();
        wordsCloudModel.UserName = constantStorage_1.ConstantStorage.getUserName();
        wordsCloudModel.Words = [word];
        var result = this.httpService.processPost(wordsCloudModel, constantStorage_1.ConstantStorage.getDeleteWordController());
        result.then(function (response) { _this.removedWord(word); _this.endLoading(); }, function (error) { return word.Hidden = false; });
    };
    MyWords.prototype.setUserWords = function (userWords) {
        if (userWords.Words) {
            this.wordsCount = userWords.TotalWords;
            this.words.push.apply(this.words, userWords.Words);
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
    // ************* SORT LOGIC *********************
    MyWords.prototype.headerClick = function (sortKey) {
        if (sortKey == this.sortKey) {
            this.sortAsc = !this.sortAsc;
        }
        else {
            this.sortKey = sortKey;
            this.sortAsc = true;
        }
        console.group('sorting');
        console.log("sort type: " + this.sortKey);
        console.log("sort asc: " + this.sortAsc);
        console.groupEnd();
        // reset words
        this.words = [];
        this.loadWords();
    };
    // ************* END OF SORT LOGIC **************
    // ************* PAGING LOGIC (START START REGION) ****************
    MyWords.prototype.scrollEvent = function (event) {
        var scrollTop = (document.documentElement && document.documentElement.scrollTop)
            || document.body.scrollTop;
        var scrollHeight = document.body.scrollHeight;
        var clientHeight = document.documentElement.clientHeight;
        var tenPercents = (scrollTop + clientHeight) / 100 * 10;
        if (scrollTop + clientHeight + tenPercents >= scrollHeight && this.canLoadPage()) {
            this.loadWords();
        }
    };
    MyWords.prototype.resuzeEvent = function (event) {
        this.isNeedMoreWords();
    };
    MyWords.prototype.loadWords = function () {
        var _this = this;
        this.startLoading();
        var url = "" + constantStorage_1.ConstantStorage.getWordsManagerController();
        url += "?UserId=" + constantStorage_1.ConstantStorage.getUserId();
        url += "&CurrentWordsCount=" + this.words.length;
        url += "&WordsPerPage=" + this.wordsPerPage;
        url += "&sortKey=" + this.sortKey;
        url += "&sortAsc=" + this.sortAsc;
        var result = this.httpService.processGet(url);
        result.then(function (json) { _this.setUserWords(json); _this.endLoading(); _this.isNeedMoreWords(); }, function (error) { _this.endLoading(); alert('ERROR of loading words!!!'); });
    };
    // when page is loaded but scroll is hidden
    MyWords.prototype.isNeedMoreWords = function () {
        var self = this;
        // wait for render
        setTimeout(function () {
            var scrollHeight = document.body.scrollHeight;
            var clientHeight = document.documentElement.clientHeight;
            if (scrollHeight == clientHeight && self.canLoadPage()) {
                self.loadWords();
            }
        }, 300);
    };
    MyWords.prototype.startLoading = function () {
        // disable scroll
        var x = window.scrollX;
        var y = window.scrollY;
        window.onscroll = function () { window.scrollTo(x, y); };
        this.isLoading = true;
    };
    MyWords.prototype.endLoading = function () {
        window.onscroll = function () { };
        this.isLoading = false;
    };
    MyWords.prototype.fakeAddWords = function () {
        if (this.words.length < 1000) {
            this.words.push.apply(this.words, this.words);
        }
    };
    MyWords.prototype.canLoadPage = function () {
        return this.wordsCount > this.words.length && this.isLoading == false;
    };
    return MyWords;
}());
__decorate([
    core_1.HostListener('window:scroll', ['$event']),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", void 0)
], MyWords.prototype, "scrollEvent", null);
__decorate([
    core_1.HostListener('window: resize', ['$event']),
    __metadata("design:type", Function),
    __metadata("design:paramtypes", [Object]),
    __metadata("design:returntype", void 0)
], MyWords.prototype, "resuzeEvent", null);
MyWords = __decorate([
    core_1.Component({
        selector: 'my-words',
        templateUrl: '../StaticContent/app/templates/components/myWords.html'
    }),
    __metadata("design:paramtypes", [httpService_1.HttpService])
], MyWords);
exports.MyWords = MyWords;
