import { ModalWindowModel } from '../models/modalWindowModel';

export class CommonHelper {
    public static logOff() {
        window.location.href = '/account/SignOff';
    }

    public static buildOkCancelModalConfig(
        header: string, text: string, applyCallback: any): ModalWindowModel {

        let modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = header;
        modalWindowModel.BodyText = text;
        modalWindowModel.IsApplyButton = true;
        modalWindowModel.IsCancelButton = true;
        modalWindowModel.ApplyButtonText = 'OK';
        modalWindowModel.CancelButtonText = 'Cancel';
        modalWindowModel.ApplyCallback = () => applyCallback();

        return modalWindowModel;
    }
}