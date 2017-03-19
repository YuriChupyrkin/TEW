import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from '../helpers/constantStorage';
import { HttpService } from '../services/httpService';
import { User } from '../models/user';
import { Router} from '@angular/router';
import { PubSub } from '../services/pubSub';
import { CommonHelper } from '../helpers/commonHelper';
import { ModalWindow } from '../helpComponents/modalWindow';
import { ModalWindowServise } from '../services/modalWindowServise';
import { JQueryHelper } from '../helpers/jQueryHelper';
import { ModalWindowModel } from '../models/modalWindowModel';

@Component({
    selector: 'my-app',
    templateUrl: '../StaticContent/app/templates/components/app.html'
})

export class AppComponent implements OnInit {
    private userName: string;
    private modalConfig: ModalWindowModel;
    private showLoadingArr = [];
    private isLoading = false;

    constructor(private httpService: HttpService, private router: Router) {
        this.isLoading = true;
        ConstantStorage.setYandexTranslaterApiKey
            ('dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f');

        this.userName = '';
        this.httpService.processGet<User>(ConstantStorage.getUserInfoController())
            .then(
                response => this.setUserInfo(response),
                error => this.isLoading = false);
    }

    ngOnInit() {
        let self = this;
        ModalWindowServise.initModalWindowService(this);

        PubSub.Sub('loading', (...args: Array<any>) => {
            if (args && args.length) {
                args.forEach(x => {
                    if (x === true) {
                        this.showLoadingArr.push(x);
                    }
                    else if (this.showLoadingArr.length > 0) {
                        this.showLoadingArr.pop();
                    }
                });
            }
        });

        // auto toggle
        let windowWidth = JQueryHelper.getElement(window).width();

        JQueryHelper.getElement('.navbar-collapse a:not(.dropdown-toggle)').click(function(){
            if (windowWidth < 768 ) {
                JQueryHelper.getElement('.navbar-collapse').collapse('hide');
            }
        });
    }

    public setModalConfig(config: ModalWindowModel) {
        this.modalConfig = config;
    }

    private setUserInfo(user: User) {
        ConstantStorage.setUserName(user.Email);
        ConstantStorage.setUserId(user.Id);

        this.userName = user.Email;
        this.router.navigate(['/user-stat']);
        this.isLoading = false
    }

    private logOut() {
        let modalWindowModel = CommonHelper.buildOkCancelModalConfig(
            `Sign out`,
            `Do you want sign out?`,
            CommonHelper.logOff);

        ModalWindowServise.showModalWindow(modalWindowModel);
    }
}