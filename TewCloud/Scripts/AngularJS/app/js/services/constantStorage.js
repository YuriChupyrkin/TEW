"use strict";
var ConstantStorage = (function () {
    function ConstantStorage() {
    }
    ConstantStorage.setUserName = function (name) {
        this.userName = name;
    };
    ConstantStorage.getUserName = function () {
        return this.userName;
    };
    ConstantStorage.setYandexTranslaterApiKey = function (apiKey) {
        this.yandexTranslaterApiKey = apiKey;
    };
    ConstantStorage.getYandexTranslaterApiKey = function () {
        return this.yandexTranslaterApiKey;
    };
    ConstantStorage.setUserId = function (id) {
        this.userId = id;
    };
    ConstantStorage.getUserId = function () {
        return this.userId;
    };
    return ConstantStorage;
}());
exports.ConstantStorage = ConstantStorage;
