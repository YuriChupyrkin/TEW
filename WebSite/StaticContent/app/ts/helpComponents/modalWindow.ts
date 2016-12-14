import { Component, Input, Output, EventEmitter } from '@angular/core';
import { JQueryHelper } from '../services/jqueryHelper';

@Component({
    selector: 'modal-window',
    templateUrl: '../../StaticContent/app/templates/helpComponents/modalWindow.html'
})

export class ModalWindow {
    public static readonly MODAL_WINDOW_ID = "tew-modal-window";
    private readonly modal_window_id = "tew-modal-window";
    
    private config: any;
    @Output() public windowApplied = new EventEmitter();
    @Output() public windowCanceled = new EventEmitter();

    @Input() set windowConfig(value: any){
        if (value) {
            this.config = value;
        } else {
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
    }

    constructor(){
        if (!this.config) {
            this.windowConfig = undefined;
        }
    }

    public static showWindow() {
        let element = JQueryHelper.getElementById(this.MODAL_WINDOW_ID);

        if (element && element.modal) {
            // wait for previous modal if they were...
            setTimeout(() => element.modal(), 550);
        }
    }

    private applyWindow() {
        if (this.config.applyCallback) {
            this.config.applyCallback();
        }
        else {
            this.windowApplied.emit();
        }
    }

    private cancelWindow() {
        if (this.config.cancelCallback) {
            this.config.cancelCallback();
        }
        else {
            this.windowCanceled.emit();
        }
    }

    private buildDefaultConfig(){
        let config = {
            headerText: 'header',
            bodyText: 'body',
            isApplyButton: false,
            isCancelButton: true,
            applyButtonText: 'apply',
            cancelButtonText: 'cancel',
            applyCallback: () => console.log('apply callback'),
            cancelCallback: () => console.log('cancel callback')
        };

        return config;
    }
}