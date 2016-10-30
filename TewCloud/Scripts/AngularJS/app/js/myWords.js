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
var getPostService_1 = require('./services/getPostService');
var userWords_1 = require('./models/userWords');
var MyWords = (function () {
    function MyWords(getPostService) {
        this.getPostService = getPostService;
        this.description = "myWords";
        this.userWords = new userWords_1.UserWords();
        this.userWords.UserName = 'hello';
    }
    MyWords.prototype.getWords = function () {
        var _this = this;
        var url = '/api/WordsManager?userName=yurec37@yandex.ru';
        var result = this.getPostService.getRequest(url);
        var js = 'json';
        result.subscribe(function (json) { return _this.userWords = json; });
    };
    MyWords.prototype.showWords = function () {
        console.dir(this.userWords);
    };
    MyWords = __decorate([
        core_1.Component({
            selector: 'my-words',
            templateUrl: '../../scripts/angularjs/app/templates/myWords.html'
        }), 
        __metadata('design:paramtypes', [getPostService_1.GetPostService])
    ], MyWords);
    return MyWords;
}());
exports.MyWords = MyWords;
