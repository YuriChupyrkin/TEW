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
var jqueryHelper_1 = require("../helpers/jqueryHelper");
var modalWindowModel_1 = require("../models/modalWindowModel");
var ModalWindow = ModalWindow_1 = (function () {
    function ModalWindow() {
        this.modal_window_id = "tew-modal-window";
        this.windowApplied = new core_1.EventEmitter();
        this.windowCanceled = new core_1.EventEmitter();
        var self = this;
        this.dismissed = false;
        if (!this.config) {
            this.windowConfig = undefined;
        }
        jqueryHelper_1.JQueryHelper.getElement(document).on("hide.bs.modal", "#" + ModalWindow_1.MODAL_WINDOW_ID, function () {
            if (self.dismissed == false) {
                self.closeWindow();
            }
        });
    }
    Object.defineProperty(ModalWindow.prototype, "windowConfig", {
        set: function (value) {
            this.dismissed = false;
            if (value) {
                this.config = value;
                if (this.config.IsApplyButton === undefined) {
                    this.config.IsApplyButton = false;
                }
                if (this.config.IsCancelButton === undefined) {
                    this.config.IsCancelButton = true;
                }
                if (!this.config.CancelButtonText) {
                    this.config.CancelButtonText = 'Cancel';
                }
            }
            else {
                // default config
                this.config = this.buildDefaultConfig();
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
        this.dismissed = true;
        if (this.config.ApplyCallback) {
            this.config.ApplyCallback();
        }
        else {
            this.windowApplied.emit();
        }
    };
    ModalWindow.prototype.cancelWindow = function () {
        this.dismissed = true;
        if (this.config.CancelCallback) {
            this.config.CancelCallback();
        }
        else {
            this.windowCanceled.emit();
        }
    };
    // set event for X (close)
    ModalWindow.prototype.closeWindow = function () {
        if (this.config.IsCancelButton) {
            this.cancelWindow();
        }
        else if (this.config.IsApplyButton) {
            this.applyWindow();
        }
    };
    ModalWindow.prototype.buildDefaultConfig = function () {
        var modalWindowModel = new modalWindowModel_1.ModalWindowModel();
        modalWindowModel.HeaderText = 'header';
        modalWindowModel.BodyText = 'body';
        modalWindowModel.IsApplyButton = false;
        modalWindowModel.IsCancelButton = true;
        modalWindowModel.ApplyButtonText = 'apply';
        modalWindowModel.CancelButtonText = 'cancel';
        modalWindowModel.ApplyCallback = function () { return console.log('apply callback'); };
        modalWindowModel.CancelCallback = function () { return console.log('cancel callback'); };
        modalWindowModel.CloseCallback = function () { return console.log('close callback'); };
        return modalWindowModel;
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
    __metadata("design:type", modalWindowModel_1.ModalWindowModel),
    __metadata("design:paramtypes", [modalWindowModel_1.ModalWindowModel])
], ModalWindow.prototype, "windowConfig", null);
ModalWindow = ModalWindow_1 = __decorate([
    core_1.Component({
        selector: 'modal-window',
        templateUrl: '../../StaticContent/app/templates/helpComponents/modalWindow.html'
    }),
    __metadata("design:paramtypes", [])
], ModalWindow);
exports.ModalWindow = ModalWindow;
var ModalWindow_1;
