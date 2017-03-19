import { Type } from '@angular/core';

export class ModalWindowModel {
    HeaderText: string;
    BodyText: string;
    IsApplyButton: boolean;
    IsCancelButton: boolean;
    ApplyButtonText: string;
    CancelButtonText: string;
    ApplyCallback: any;
    CancelCallback: any;
    CloseCallback: any;
    InnerComponent: boolean;
    InnerComponentType: Type<any>;
    InnerComponentOptions: any;
    MediumWindowSize: boolean;
}