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
var http_1 = require("@angular/http");
var constantStorage_1 = require("./constantStorage");
var pubSub_1 = require("./pubSub");
require("rxjs/add/operator/map");
var HttpService = (function () {
    function HttpService(http) {
        this.http = http;
    }
    HttpService.prototype.processGet = function (url, isExternalRequest) {
        if (isExternalRequest === void 0) { isExternalRequest = false; }
        var headers = new http_1.Headers();
        var userId = constantStorage_1.ConstantStorage.getUserId();
        if (isExternalRequest == false && userId) {
            headers.append('Authorization', userId.toString());
        }
        // start loading...
        pubSub_1.PubSub.Pub(constantStorage_1.ConstantStorage.getLoadingEvent(), true);
        var getRequest = this.http.get(url, { headers: headers }).map(function (response) { return response.json(); });
        //getRequest.subscribe(r => this.requestFinished(), e => this.requestFinishedWithError(url, 'get', e));
        return getRequest;
    };
    HttpService.prototype.processPost = function (object, url) {
        var headers = new http_1.Headers();
        var userId = constantStorage_1.ConstantStorage.getUserId();
        if (userId) {
            headers.append('Authorization', userId.toString());
        }
        // start loading...
        pubSub_1.PubSub.Pub(constantStorage_1.ConstantStorage.getLoadingEvent(), true);
        var postRequest = this.http.post(url, object, { headers: headers });
        //postRequest.subscribe(r => this.requestFinished(), e => this.requestFinishedWithError(url, 'post', e));
        return postRequest;
    };
    HttpService.prototype.requestFinished = function () {
        // end loading... 
        pubSub_1.PubSub.Pub(constantStorage_1.ConstantStorage.getLoadingEvent(), false);
    };
    HttpService.prototype.requestFinishedWithError = function (url, method, error) {
        console.log(url + " (" + method + "): request finished with error:");
        console.log(error);
        // end loading...
        pubSub_1.PubSub.Pub(constantStorage_1.ConstantStorage.getLoadingEvent(), false);
    };
    return HttpService;
}());
HttpService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], HttpService);
exports.HttpService = HttpService;
