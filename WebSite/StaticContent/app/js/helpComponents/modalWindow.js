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
var jqueryHelper_1 = require("../services/jqueryHelper");
var ModalWindow = (function () {
    function ModalWindow() {
        this.modal_window_id = "tew-modal-window";
        this.windowApplied = new core_1.EventEmitter();
        this.windowCanceled = new core_1.EventEmitter();
        if (!this.config) {
            this.windowConfig = undefined;
        }
    }
    Object.defineProperty(ModalWindow.prototype, "windowConfig", {
        set: function (value) {
            if (value) {
                this.config = value;
            }
            else {
                // default config
                this.config = this.buildDefaultConfig();
            }
            if (this.config.isApplyButton === undefined) {
                this.config.isApplyButton = false;
            }
            if (this.config.isCancelButton === undefined) {
                this.config.isCancelButton = true;
            }
            if (!this.config.cancelButtonText) {
                this.config.cancelButtonText = 'Cancel';
            }
        },
        enumerable: true,
        configurable: true
    });
    ModalWindow.showWindow = function () {
        var element = jqueryHelper_1.JQueryHelper.getElementById(this.MODAL_WINDOW_ID);
        if (element && element.modal) {
            // wait for previous modal if they were...
            setTimeout(function () { return element.modal(); }, 550);
        }
    };
    ModalWindow.prototype.applyWindow = function () {
        if (this.config.applyCallback) {
            this.config.applyCallback();
        }
        else {
            this.windowApplied.emit();
        }
    };
    ModalWindow.prototype.cancelWindow = function () {
        if (this.config.cancelCallback) {
            this.config.cancelCallback();
        }
        else {
            this.windowCanceled.emit();
        }
    };
    ModalWindow.prototype.buildDefaultConfig = function () {
        var config = {
            headerText: 'header',
            bodyText: 'body',
            isApplyButton: false,
            isCancelButton: true,
            applyButtonText: 'apply',
            cancelButtonText: 'cancel',
            applyCallback: function () { return console.log('apply callback'); },
            cancelCallback: function () { return console.log('cancel callback'); }
        };
        return config;
    };
    return ModalWindow;
}());
ModalWindow.MODAL_WINDOW_ID = "tew-modal-window";
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], ModalWindow.prototype, "windowApplied", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], ModalWindow.prototype, "windowCanceled", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Object])
], ModalWindow.prototype, "windowConfig", null);
ModalWindow = __decorate([
    core_1.Component({
        selector: 'modal-window',
        templateUrl: '../../StaticContent/app/templates/helpComponents/modalWindow.html'
    }),
    __metadata("design:paramtypes", [])
], ModalWindow);
exports.ModalWindow = ModalWindow;