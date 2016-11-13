"use strict";
var CommonHelper = (function () {
    function CommonHelper() {
    }
    CommonHelper.logOff = function () {
        if (confirm("log out?")) {
            window.location.href = '/account/SignOff';
        }
    };
    return CommonHelper;
}());
exports.CommonHelper = CommonHelper;
