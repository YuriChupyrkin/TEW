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
var AddWord = (function () {
    function AddWord(httpService) {
        this.httpService = httpService;
        this.translates = new Array();
        this.englishWord = 'cat';
    }
    AddWord.prototype.translate = function () {
        if (!this.englishWord) {
            return;
        }
        console.log('translate....');
        this.clearTranslateResults();
        this.translateByYandex();
        this.translateByExistsWords();
    };
    AddWord.prototype.translateByExistsWords = function () {
        console.log("translateByExistsWords");
    };
    AddWord.prototype.translateByYandex = function () {
        var _this = this;
        var url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup";
        var translateLang = "en-ru";
        var apiKey = constantStorage_1.ConstantStorage.getYandexTranslaterApiKey();
        var resultUri = url + "?key=" + apiKey + "&lang=" + translateLang + "&text=" + this.englishWord;
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
                    this.translates.push(translate['text'].toString());
                }
                if (translate['syn'] && translate['syn']['length']) {
                    for (var i = 0; i < translate['syn']['length']; i++) {
                        var syn = translate['syn'][i];
                        this.translates.push(syn['text']);
                    }
                }
                if (translate && translate['ex'] && translate['ex'][0] && translate['ex'][0]['text']) {
                    this.exampleOfUser = translate['ex'][0]['text'];
                }
            }
        }
    };
    AddWord.prototype.chooseTranslate = function (translate) {
        console.log("choose: " + translate);
        this.chosenTranslate = translate;
    };
    AddWord.prototype.clearTranslateResults = function () {
        this.chosenTranslate = '';
        this.exampleOfUser = '';
        this.translates = new Array();
    };
    AddWord.prototype.save = function () {
        console.log("SAVE");
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
