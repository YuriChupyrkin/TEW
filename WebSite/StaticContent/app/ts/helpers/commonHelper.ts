import { ModalWindowModel } from '../models/modalWindowModel';
import { ModalWindowServise } from '../services/modalWindowServise';
import { ConstantStorage } from '../helpers/constantStorage';

export class CommonHelper {
    public static logOff() {
        window.location.href = '/account/SignOff';
    }

    public static buildOkCancelModalConfig(
        header: string,
        text: string,
        applyCallback: any,
        cancelCallback?: any): ModalWindowModel {

        let modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = header;
        modalWindowModel.BodyText = text;
        modalWindowModel.IsApplyButton = true;
        modalWindowModel.IsCancelButton = true;
        modalWindowModel.ApplyButtonText = 'OK';
        modalWindowModel.CancelButtonText = 'Cancel';
        modalWindowModel.ApplyCallback = () => applyCallback();

        if (cancelCallback) {
            modalWindowModel.CancelCallback = () => cancelCallback();
        }

        return modalWindowModel;
    }

    public static showError(message: string) {
        let modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = 'PAGE ERROR';
        modalWindowModel.BodyText = message;
        modalWindowModel.IsCancelButton = true;
        modalWindowModel.CancelButtonText = 'Cancel';

        ModalWindowServise.showModalWindow(modalWindowModel);
    }

    public static isApplicationReady() {
        return !!ConstantStorage.getUserId();
    }
}