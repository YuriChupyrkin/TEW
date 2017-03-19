"use strict";
var modalWindowModel_1 = require("../models/modalWindowModel");
var CommonHelper = (function () {
    function CommonHelper() {
    }
    CommonHelper.logOff = function () {
        window.location.href = '/account/SignOff';
    };
    CommonHelper.buildOkCancelModalConfig = function (header, text, applyCallback) {
        var modalWindowModel = new modalWindowModel_1.ModalWindowModel();
        modalWindowModel.HeaderText = header;
        modalWindowModel.BodyText = text;
        modalWindowModel.IsApplyButton = true;
        modalWindowModel.IsCancelButton = true;
        modalWindowModel.ApplyButtonText = 'OK';
        modalWindowModel.CancelButtonText = 'Cancel';
        modalWindowModel.ApplyCallback = function () { return applyCallback(); };
        return modalWindowModel;
    };
    return CommonHelper;
}());
exports.CommonHelper = CommonHelper;
