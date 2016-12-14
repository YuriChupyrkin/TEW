"use strict";
var JQueryHelper = (function () {
    function JQueryHelper() {
    }
    JQueryHelper.getElement = function (expression) {
        return jQuery(expression);
    };
    JQueryHelper.getElementById = function (id) {
        return this.getElement("#" + id);
    };
    return JQueryHelper;
}());
exports.JQueryHelper = JQueryHelper;
