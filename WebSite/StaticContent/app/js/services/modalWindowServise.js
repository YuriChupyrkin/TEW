"use strict";
var ModalWindow_1 = require("../helpComponents/ModalWindow");
var ModalWindowServise = (function () {
    function ModalWindowServise() {
    }
    ModalWindowServise.initModalWindowService = function (app) {
        if (!this.appComponent) {
            this.appComponent = app;
        }
    };
    ModalWindowServise.showModalWindow = function (config) {
        if (!config || !this.appComponent) {
            throw new Error("invalid config");
        }
        this.appComponent.setModalConfig(config);
        ModalWindow_1.ModalWindow.showWindow();
    };
    ModalWindowServise.hideModalWindow = function () {
        ModalWindow_1.ModalWindow.closeWindow();
    };
    return ModalWindowServise;
}());
exports.ModalWindowServise = ModalWindowServise;
