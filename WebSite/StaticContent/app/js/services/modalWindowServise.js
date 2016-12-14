"use strict";
var ModalWindow_1 = require("../helpComponents/ModalWindow");
var ModalWindowServise = (function () {
    function ModalWindowServise() {
    }
    ModalWindowServise.initModalWindowService = function (app) {
        this.appComponent = app;
    };
    ModalWindowServise.showModalWindow = function (config) {
        if (!config || !this.appComponent) {
            throw new Error("invalid config");
        }
        this.appComponent.setModalConfig(config);
        ModalWindow_1.ModalWindow.showWindow();
    };
    return ModalWindowServise;
}());
exports.ModalWindowServise = ModalWindowServise;
