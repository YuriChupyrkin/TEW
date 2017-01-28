import { Component, Input, Output, EventEmitter } from '@angular/core';
import { JQueryHelper } from '../helpers/jqueryHelper';

@Component({
    selector: 'modal-window',
    templateUrl: '../../StaticContent/app/templates/helpComponents/modalWindow.html'
})

export class ModalWindow {
    public static readonly MODAL_WINDOW_ID = "tew-modal-window";
    private readonly modal_window_id = "tew-modal-window";
    
    @Output() public windowApplied = new EventEmitter();
    @Output() public windowCanceled = new EventEmitter();

    private config: any;
    private dismissed: boolean;

    @Input() set windowConfig(value: any) {
        this.dismissed = false;
        if (value) {
            this.config = value;
            
            if (this.config.isApplyButton === undefined) {
                this.config.isApplyButton = false;
            }

            if (this.config.isCancelButton === undefined) {
                this.config.isCancelButton = true;
            }

            if (!this.config.cancelButtonText) {
                this.config.cancelButtonText = 'Cancel';
            }
        } else {
            // default config
            this.config = this.buildDefaultConfig();
        }
    }

    constructor() {
        var self = this;
        this.dismissed = false;
        if (!this.config) {
            this.windowConfig = undefined;
        }

        JQueryHelper.getElement(document).on(`hide.bs.modal`, `#${ModalWindow.MODAL_WINDOW_ID}`, function () {
            if (self.dismissed == false) {
                self.closeWindow();
            }
        });
    }

    public static showWindow() {
        let element = JQueryHelper.getElementById(this.MODAL_WINDOW_ID);

        if (element && element.modal) {
            // wait for previous modal if they were...
            setTimeout(() => element.modal(), 550);
        }
    }

    private applyWindow() {
        this.dismissed = true;
        if (this.config.applyCallback) {
            this.config.applyCallback();
        }
        else {
            this.windowApplied.emit();
        }
    }

    private cancelWindow() {
        this.dismissed = true;
        if (this.config.cancelCallback) {
            this.config.cancelCallback();
        }
        else {
            this.windowCanceled.emit();
        }
    }

    // set event for X (close)
    private closeWindow() {
        if (this.config.isCancelButton) {
            this.cancelWindow();
        }
        else if (this.config.isApplyButton) {
            this.applyWindow();
        }
    }

    private buildDefaultConfig() {
        let config = {
            headerText: 'header',
            bodyText: 'body',
            isApplyButton: false,
            isCancelButton: true,
            applyButtonText: 'apply',
            cancelButtonText: 'cancel',
            applyCallback: () => console.log('apply callback'),
            cancelCallback: () => console.log('cancel callback'),
            closeCallback: () => console.log('close callback')
        };

        return config;
    }
}