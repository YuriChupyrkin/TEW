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
///<reference path="./../../typings/main.d.ts"/>
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var platform_browser_1 = require("@angular/platform-browser");
var app_1 = require("./components/app");
var home_1 = require("./components/home");
var addWord_1 = require("./components/addWord");
var myWords_1 = require("./components/myWords");
var pickerTest_1 = require("./components/pickerTest");
var app_routes_1 = require("./app.routes");
var common_1 = require("@angular/common");
var httpService_1 = require("./services/httpService");
var loadingAnimation_1 = require("./helpComponents/loadingAnimation");
var loadingLockDisplay_1 = require("./helpComponents/loadingLockDisplay");
var progressBar_1 = require("./helpComponents/progressBar");
var modalWindow_1 = require("./helpComponents/modalWindow");
var userStat_1 = require("./components/userStat");
var irregularVerbs_1 = require("./components/irregularVerbs");
var myWordsHeader_1 = require("./helpComponents/myWordsHeader");
// For textbox binding
var forms_1 = require("@angular/forms");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [platform_browser_1.BrowserModule, app_routes_1.routing, http_1.HttpModule, forms_1.FormsModule, forms_1.ReactiveFormsModule],
        declarations: [
            app_1.AppComponent,
            home_1.Home,
            addWord_1.AddWord,
            myWords_1.MyWords,
            pickerTest_1.PickerTest,
            loadingAnimation_1.LoadingAnimation,
            progressBar_1.ProgressBar,
            modalWindow_1.ModalWindow,
            userStat_1.UserStat,
            irregularVerbs_1.IrregularVerbs,
            loadingLockDisplay_1.LoadingLockDisplay,
            myWordsHeader_1.MyWordsHeader
        ],
        bootstrap: [app_1.AppComponent],
        providers: [
            { provide: common_1.APP_BASE_HREF, useValue: '/#' },
            app_routes_1.appRoutingProviders,
            httpService_1.HttpService
        ]
    }),
    __metadata("design:paramtypes", [])
], AppModule);
exports.AppModule = AppModule;
