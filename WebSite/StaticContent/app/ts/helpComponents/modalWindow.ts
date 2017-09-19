import { Component, Input, Output, EventEmitter,
    ViewChild, ViewContainerRef, ComponentFactoryResolver } from '@angular/core';
import { JQueryHelper } from '../helpers/jqueryHelper';
import { ModalWindowModel } from '../models/modalWindowModel';
import { PubSub } from '../services/pubSub';

@Component({
    selector: 'modal-window',
    templateUrl: '../../StaticContent/app/templates/helpComponents/modalWindow.html',
    styleUrls: [
        '../StaticContent/app/css/components/modalWindow.css',
        '../StaticContent/app/css/components/common.css'
    ]
})

export class ModalWindow {
    private windowSizeClass: string;

    @Output() public windowApplied = new EventEmitter();
    @Output() public windowCanceled = new EventEmitter();
    @ViewChild('innerComponent', {read: ViewContainerRef}) innerComponent: ViewContainerRef;

    private config: ModalWindowModel;
    private dismissed: boolean;

    @Input() set windowConfig(value: ModalWindowModel) {
        this.dismissed = false;
        if (value) {
            this.config = value;

            if (value.InnerComponent) {
                 let factory = this.resolver.resolveComponentFactory(value.InnerComponentType);
                 let component = this.innerComponent.createComponent(factory);
                 component.instance.options = value.InnerComponentOptions;
            }

            this.windowSizeClass = this.config.MediumWindowSize ? 'modal-md' : 'modal-sm';

            if (this.config.IsApplyButton === undefined) {
                this.config.IsApplyButton = false;
            }

            if (this.config.IsCancelButton === undefined) {
                this.config.IsCancelButton = true;
            }

            if (!this.config.CancelButtonText) {
                this.config.CancelButtonText = 'Cancel';
            }
        } else {
            // default config
            this.config = this.buildDefaultConfig();
        }
    }

    constructor(private viewContainerRef: ViewContainerRef, private resolver: ComponentFactoryResolver ) {
        let self = this;
        this.windowSizeClass = 'modal-sm';
        this.dismissed = false;
        if (!this.config) {
            this.windowConfig = undefined;
        }
    }

    private applyWindow() {
        this.dismissed = true;
        if (this.config.ApplyCallback) {
            this.config.ApplyCallback();
        }
        else {
            this.windowApplied.emit();
        }

        this.dismiss();
    }

    private cancelWindow() {
        this.dismissed = true;
        if (this.config.CancelCallback) {
            this.config.CancelCallback();
        }
        else {
            this.windowCanceled.emit();
        }

        this.dismiss();
    }

    // set event for X (close)
    private closeWindow() {
        if (this.config.IsCancelButton) {
            this.cancelWindow();
        }
        else if (this.config.IsApplyButton) {
            this.applyWindow();
        }
        else {
            this.dismiss();
        }
    }

    private dismiss() {
        if (this.innerComponent) {
            this.innerComponent.clear();
        }

        this.triggerCloseEvent();
    }

    private triggerCloseEvent() {
        PubSub.Pub('modalClose');
    }

    private buildDefaultConfig() {
        let modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = 'header';
        modalWindowModel.BodyText = 'body';
        modalWindowModel.IsApplyButton = false;
        modalWindowModel.IsCancelButton = true;
        modalWindowModel.ApplyButtonText = 'apply';
        modalWindowModel.CancelButtonText = 'cancel';
        modalWindowModel.ApplyCallback = () => console.log('apply callback');
        modalWindowModel.CancelCallback = () => console.log('cancel callback');
        modalWindowModel.CloseCallback = () => console.log('close callback');

        return modalWindowModel;
    }
}