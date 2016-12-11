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
var constantStorage_1 = require("./services/constantStorage");
var commonHelper_1 = require("./services/commonHelper");
var httpService_1 = require("./services/httpService");
var Home = (function () {
    function Home(httpService) {
        this.httpService = httpService;
    }
    Home.prototype.logOff = function () {
        commonHelper_1.CommonHelper.logOff();
    };
    Home.prototype.ngOnInit = function () {
        var _this = this;
        this.userName = constantStorage_1.ConstantStorage.getUserName();
        var userId = constantStorage_1.ConstantStorage.getUserId();
        if (userId != 0 && userId != undefined) {
            this.httpService.processGet(constantStorage_1.ConstantStorage.getUserStatController() + "?userId=" + userId)
                .subscribe(function (result) { return _this.userStatModel = result; });
        }
    };
    return Home;
}());
Home = __decorate([
    core_1.Component({
        selector: 'home',
        templateUrl: '../../scripts/angularjs/app/templates/home.html'
    }),
    __metadata("design:paramtypes", [httpService_1.HttpService])
], Home);
exports.Home = Home;
