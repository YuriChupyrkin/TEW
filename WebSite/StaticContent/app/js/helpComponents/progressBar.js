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
var ProgressBar = (function () {
    function ProgressBar() {
        this.setProgress(5);
    }
    Object.defineProperty(ProgressBar.prototype, "progress", {
        set: function (value) {
            this.setProgress(value);
        },
        enumerable: true,
        configurable: true
    });
    ProgressBar.prototype.setProgress = function (progress) {
        progress = this.validateProgress(progress);
        this.currentProgress = progress;
        console.log("setProgress: " + progress);
    };
    ProgressBar.prototype.validateProgress = function (progress) {
        if (progress > 100) {
            return 100;
        }
        if (progress < 0) {
            return 0;
        }
        return progress;
    };
    return ProgressBar;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Object])
], ProgressBar.prototype, "progress", null);
ProgressBar = __decorate([
    core_1.Component({
        selector: 'progress-bar',
        templateUrl: '../../StaticContent/app/templates/helpComponents/progressBar.html'
    }),
    __metadata("design:paramtypes", [])
], ProgressBar);
exports.ProgressBar = ProgressBar;
