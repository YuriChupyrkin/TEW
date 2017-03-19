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
var constantStorage_1 = require("../helpers/constantStorage");
var httpService_1 = require("../services/httpService");
var modalWindowServise_1 = require("../services/modalWindowServise");
var user_1 = require("../models/user");
var commonHelper_1 = require("../helpers/commonHelper");
var UserStat = (function () {
    function UserStat(httpService) {
        this.httpService = httpService;
    }
    UserStat.prototype.ngOnInit = function () {
        this.updateUserStat();
    };
    UserStat.prototype.updateUserStat = function () {
        var _this = this;
        var userId = constantStorage_1.ConstantStorage.getUserId();
        if (userId !== 0 && userId !== undefined) {
            this.httpService.processGet(constantStorage_1.ConstantStorage.getUserStatController() + "?userId=" + userId)
                .then(function (result) { return _this.userStatModel = result; });
        }
    };
    UserStat.prototype.resetUserWords = function () {
        var modalWindowModel = commonHelper_1.CommonHelper.buildOkCancelModalConfig("Reset", "Do you really want reset level of your words?", this.resetLevel.bind(this));
        modalWindowServise_1.ModalWindowServise.showModalWindow(modalWindowModel);
    };
    UserStat.prototype.resetLevel = function () {
        var _this = this;
        var userId = constantStorage_1.ConstantStorage.getUserId();
        var user = new user_1.User();
        user.Id = userId;
        if (userId !== 0 && userId !== undefined) {
            this.httpService.processPost(user, constantStorage_1.ConstantStorage.getResetWordsLevelController())
                .then(function (r) { return _this.updateUserStat(); });
        }
    };
    return UserStat;
}());
UserStat = __decorate([
    core_1.Component({
        selector: 'user-stat',
        templateUrl: '../StaticContent/app/templates/components/userStat.html'
    }),
    __metadata("design:paramtypes", [httpService_1.HttpService])
], UserStat);
exports.UserStat = UserStat;
