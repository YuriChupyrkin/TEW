"use strict";
var PubSub = (function () {
    function PubSub() {
    }
    PubSub.Pub = function (name) {
        var args = [];
        for (var _i = 1; _i < arguments.length; _i++) {
            args[_i - 1] = arguments[_i];
        }
        if (!this.registry[name]) {
            return;
        }
        this.registry[name].forEach(function (x) {
            x.apply(null, args);
        });
    };
    PubSub.Sub = function (name, fn) {
        if (this.registry && name && fn) {
            if (!this.registry[name]) {
                this.registry[name] = [fn];
            }
            else if (this.registry[name].length) {
                this.registry[name].push(fn);
            }
        }
    };
    PubSub.Clear = function (name) {
        if (this.registry && name) {
            this.registry[name] = null;
        }
    };
    return PubSub;
}());
PubSub.registry = {};
exports.PubSub = PubSub;
