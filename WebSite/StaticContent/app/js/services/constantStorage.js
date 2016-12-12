"use strict";
var ConstantStorage = (function () {
    function ConstantStorage() {
    }
    ConstantStorage.getWordsLevelUpdaterController = function () {
        return this.wordsLevelUpdaterController;
    };
    ConstantStorage.getPickerTestsController = function () {
        return this.pickerTestsController;
    };
    ConstantStorage.getDeleteWordController = function () {
        return this.deleteWordController;
    };
    ConstantStorage.getUserStatController = function () {
        return this.userStatController;
    };
    ConstantStorage.getApplicationMessageController = function () {
        return this.applicationMessageController;
    };
    ConstantStorage.getUserInfoController = function () {
        return this.userInfoController;
    };
    ConstantStorage.getWordTranslaterController = function () {
        return this.wordTranslaterController;
    };
    ConstantStorage.getWordsManagerController = function () {
        return this.wordsManagerController;
    };
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
// urls
ConstantStorage.wordTranslaterController = '/api/WordTranslater';
ConstantStorage.wordsManagerController = '/api/WordsManager';
ConstantStorage.userInfoController = '/api/UserInfo';
ConstantStorage.applicationMessageController = '/api/ApplicationMessage';
ConstantStorage.userStatController = '/api/userStat';
ConstantStorage.deleteWordController = '/api/DeleteWord';
ConstantStorage.pickerTestsController = '/api/PickerTests';
ConstantStorage.wordsLevelUpdaterController = 'api/WordsLevelUpdater';
exports.ConstantStorage = ConstantStorage;
