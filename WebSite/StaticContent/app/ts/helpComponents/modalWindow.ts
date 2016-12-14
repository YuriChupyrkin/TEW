import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'modal-window',
    templateUrl: '../../StaticContent/app/templates/helpComponents/modalWindow.html'
})

export class ModalWindow {
    private config: any;

    @Input() set windowConfig(value: any){
        if (value) {
            this.config = value;
        } else {
            // default config
            this.config = {
                headerText: 'header',
                bodyText: 'body',
                isApplyButton: false,
                isCancelButton: true,
                applyButtonText: 'apply'
            };
        }

        if (this.config.isApplyButton === undefined){
            this.config.isApplyButton = false;
        }

         if (this.config.isCancelButton === undefined){
            this.config.isCancelButton = true;
        }
    }

    @Output() public windowApplied = new EventEmitter();

    constructor(){
        if (!this.config) {
            this.windowConfig = undefined;
        }
    }

    public showWindow() {
        
    }

    private applyWindow(){
        this.windowApplied.emit();
    }
}