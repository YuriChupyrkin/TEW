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
var constantStorage_1 = require('./services/constantStorage');
var httpService_1 = require('./services/httpService');
var wordsCloudModel_1 = require('./models/wordsCloudModel');
var word_1 = require('./models/word');
var AddWord = (function () {
    function AddWord(httpService) {
        this.httpService = httpService;
        this.wordTranslaterController = '/api/WordTranslater';
        this.wordsManagerController = '/api/WordsManager';
        this.translates = new Array();
    }
    AddWord.prototype.translate = function () {
        if (!this.englishWord) {
            return;
        }
        this.clearTranslateResults(false);
        this.translateByYandex();
        this.translateByExistsWords();
    };
    AddWord.prototype.translateByExistsWords = function () {
        var _this = this;
        var url = this.wordTranslaterController + "?word=" + this.englishWord;
        this.httpService.processGet(url).subscribe(function (response) { return _this.addTranslate(response); });
    };
    AddWord.prototype.translateByYandex = function () {
        var _this = this;
        var url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup";
        var translateLang = "en-ru";
        var apiKey = constantStorage_1.ConstantStorage.getYandexTranslaterApiKey();
        var text = this.englishWord.replace(' ', '%20');
        var resultUri = url + "?key=" + apiKey + "&lang=" + translateLang + "&text=" + text;
        this.httpService.processGet(resultUri)
            .subscribe(function (response) { return _this.parseTranslate(response); });
    };
    AddWord.prototype.parseTranslate = function (response) {
        var def = response['def'];
        if (def && def['0']) {
            var defZero = def['0'];
            if (defZero && defZero['tr'] && defZero['tr']['0']) {
                var translate = defZero['tr']['0'];
                console.log(translate);
                if (translate['text']) {
                    this.addTranslate([translate['text']]);
                }
                if (translate['syn'] && translate['syn']['length']) {
                    for (var i = 0; i < translate['syn']['length']; i++) {
                        var syn = translate['syn'][i];
                        this.addTranslate([syn['text']]);
                    }
                }
                if (translate && translate['ex'] && translate['ex'][0] && translate['ex'][0]['text']) {
                    this.exampleOfUser = translate['ex'][0]['text'];
                }
            }
        }
    };
    AddWord.prototype.chooseTranslate = function (translate) {
        this.chosenTranslate = translate;
    };
    AddWord.prototype.clearTranslateResults = function (isClearEnglishWord) {
        if (isClearEnglishWord) {
            this.englishWord = '';
        }
        this.chosenTranslate = '';
        this.exampleOfUser = '';
        this.translates = new Array();
    };
    AddWord.prototype.addTranslate = function (translates) {
        var self = this;
        translates.forEach(function (translate) {
            if (self.translates.indexOf(translate) == -1) {
                self.translates.push(translate);
            }
        });
    };
    AddWord.prototype.save = function () {
        var _this = this;
        if (!this.englishWord || !this.chosenTranslate) {
            console.log("English and Translate are required!");
            return;
        }
        var wordCloudModel = new wordsCloudModel_1.WordsCloudModel();
        wordCloudModel.UserName = constantStorage_1.ConstantStorage.getUserName();
        var word = new word_1.Word();
        word.English = this.englishWord;
        word.Russian = this.chosenTranslate;
        word.UpdateDate = new Date();
        word.Example = this.exampleOfUser;
        wordCloudModel.Words = [word];
        this.httpService.processPost(wordCloudModel, this.wordsManagerController)
            .subscribe(function (response) { return _this.clearTranslateResults(true); }, function (error) { return alert("error"); });
    };
    AddWord = __decorate([
        core_1.Component({
            selector: 'add-word',
            templateUrl: '../../scripts/angularjs/app/templates/addWord.html'
        }), 
        __metadata('design:paramtypes', [httpService_1.HttpService])
    ], AddWord);
    return AddWord;
}());
exports.AddWord = AddWord;
