"use strict";
var CommonHelper = (function () {
    function CommonHelper() {
    }
    CommonHelper.logOff = function () {
        window.location.href = '/account/SignOff';
    };
    return CommonHelper;
}());
exports.CommonHelper = CommonHelper;
