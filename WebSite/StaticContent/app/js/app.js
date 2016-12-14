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
var httpService_1 = require("./services/httpService");
var router_1 = require("@angular/router");
var pubSub_1 = require("./services/pubSub");
var commonHelper_1 = require("./services/commonHelper");
var modalWindowServise_1 = require("./services/modalWindowServise");
var AppComponent = (function () {
    function AppComponent(httpService, router) {
        var _this = this;
        this.httpService = httpService;
        this.router = router;
        this.showLoadingArr = [];
        constantStorage_1.ConstantStorage.setYandexTranslaterApiKey('dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f');
        this.userName = '';
        this.httpService.processGet(constantStorage_1.ConstantStorage.getUserInfoController())
            .then(function (response) { return _this.setUserInfo(response); });
        this.httpService.processGet(constantStorage_1.ConstantStorage.getApplicationMessageController())
            .then(function (response) { return _this.applicationMessage = response; });
    }
    AppComponent.prototype.ngOnInit = function () {
        var _this = this;
        var self = this;
        modalWindowServise_1.ModalWindowServise.initModalWindowService(this);
        pubSub_1.PubSub.Sub('loading', function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            if (args && args.length) {
                args.forEach(function (x) {
                    if (x === true) {
                        _this.showLoadingArr.push(x);
                    }
                    else if (_this.showLoadingArr.length > 0) {
                        _this.showLoadingArr.pop();
                    }
                });
            }
        });
    };
    AppComponent.prototype.setModalConfig = function (config) {
        this.modalConfig = config;
    };
    AppComponent.prototype.setUserInfo = function (user) {
        constantStorage_1.ConstantStorage.setUserName(user.Email);
        constantStorage_1.ConstantStorage.setUserId(user.Id);
        this.userName = user.Email;
        this.router.navigate(['/home']);
    };
    AppComponent.prototype.logOut = function () {
        var modalConfig = {
            headerText: 'Sign out',
            bodyText: "Do you want sign out?",
            isApplyButton: true,
            isCancelButton: true,
            applyButtonText: 'Yes',
            cancelButtonText: 'No',
            applyCallback: function () { return commonHelper_1.CommonHelper.logOff(); }
        };
        modalWindowServise_1.ModalWindowServise.showModalWindow(modalConfig);
    };
    return AppComponent;
}());
AppComponent = __decorate([
    core_1.Component({
        selector: 'my-app',
        templateUrl: '../StaticContent/app/templates/app.html'
    }),
    __metadata("design:paramtypes", [httpService_1.HttpService, router_1.Router])
], AppComponent);
exports.AppComponent = AppComponent;
