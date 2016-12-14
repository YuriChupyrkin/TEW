import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';
import { User } from './models/user';
import { Router} from '@angular/router';
import { PubSub } from './services/pubSub';
import { CommonHelper } from './services/commonHelper';
import { ModalWindow } from './helpComponents/modalWindow';
import { ModalWindowServise } from './services/modalWindowServise';

@Component({
    selector: 'my-app',
    templateUrl: '../StaticContent/app/templates/app.html'
})

export class AppComponent implements OnInit {
    private userName: string;
    private applicationMessage: string;
    private showLoading: boolean;
    private modalConfig: any;

    constructor(private httpService: HttpService, private router: Router) {
        this.showLoading = false;
        ConstantStorage.setYandexTranslaterApiKey('dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f');

        this.userName = '';
        this.httpService.processGet<User>(ConstantStorage.getUserInfoController()).then(response => this.setUserInfo(response));

        this.httpService.processGet<string>(ConstantStorage.getApplicationMessageController())
            .then(response => this.applicationMessage = response);
    }

    ngOnInit() {
        var self = this;
        ModalWindowServise.initModalWindowService(this);

        PubSub.Sub('loading', (...args: Array<any>) => {
            if(args && args.length) {
                args.forEach(x => {   
                    self.showLoading = x === true ? true : false
                });
            }
        }); 
    }

    public setModalConfig(config: any){
        this.modalConfig = config;
    }

    private setUserInfo(user: User) {
        ConstantStorage.setUserName(user.Email);
        ConstantStorage.setUserId(user.Id);

        this.userName = user.Email;
        this.router.navigate(['/home']);
    }

    private logOut(){
        let modalConfig = {
             headerText: 'Sign out',
             bodyText: `Do you want sign out?`,
             isApplyButton: true,
             isCancelButton: true,
             applyButtonText: 'Yes',
             cancelButtonText: 'No',
             applyCallback: () => CommonHelper.logOff()
        };

        ModalWindowServise.showModalWindow(modalConfig);

        // var modal = ConstantStorage.getModalWindow();
        // modal.windowConfig = this.modalConfig;
        // modal.showWindow();
    }
}