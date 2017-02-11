import { AppComponent } from '../components/app';
import { ModalWindow } from '../helpComponents/ModalWindow';
import { ModalWindowModel } from '../models/modalWindowModel';

export class ModalWindowServise {
    private static appComponent: AppComponent 

    public static initModalWindowService(app: AppComponent){
        if(!this.appComponent) {
            this.appComponent = app;
        }
    }

    public static showModalWindow(config: ModalWindowModel) {
        if (!config || !this.appComponent){
            throw new Error("invalid config");
        }

        this.appComponent.setModalConfig(config);
        ModalWindow.showWindow();
    }
}