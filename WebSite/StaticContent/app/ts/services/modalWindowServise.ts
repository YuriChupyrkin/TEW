import { AppComponent } from '../app';
import { ModalWindow } from '../helpComponents/ModalWindow';

export class ModalWindowServise {
    private static appComponent: AppComponent 

    public static initModalWindowService(app: AppComponent){
        if(!this.appComponent) {
            this.appComponent = app;
        }
    }

    public static showModalWindow(config: any) {
        if (!config || !this.appComponent){
            throw new Error("invalid config");
        }

        this.appComponent.setModalConfig(config);
        ModalWindow.showWindow();
    }
}