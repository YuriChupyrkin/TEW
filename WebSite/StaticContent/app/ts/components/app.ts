import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from '../helpers/constantStorage';
import { HttpService } from '../services/httpService';
import { Router} from '@angular/router';
import { PubSub } from '../services/pubSub';
import { CommonHelper } from '../helpers/commonHelper';
import { ModalWindowServise } from '../services/modalWindowServise';
import { JQueryHelper } from '../helpers/jQueryHelper';
import { ModalWindowModel } from '../models/modalWindowModel';
import { UserStatModel } from '../models/userStatModel';
import '../../scss/components/common.scss';

@Component({
    selector: 'my-app',
    templateUrl: '../StaticContent/app/templates/components/app.html',
})

export class AppComponent implements OnInit {
    private userName: string;
    private modalConfig: ModalWindowModel;
    private showLoadingArr = [];
    private isLoading = false;
    private isModalOpen = false;
    private JQueryModalExpression = '.modal-container';

    constructor(private httpService: HttpService, private router: Router) {
        let url = `${ConstantStorage.getUserInfoController()}?userId=0`;
        this.isLoading = true;
        ConstantStorage.setYandexTranslaterApiKey
            ('dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f');

        this.userName = '';
        this.httpService.processGet<UserStatModel>(url)
            .then(
                response => this.setUserInfo(response),
                error => this.isLoading = false
            );
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

        PubSub.Sub('modalClose', () => {
            this.hideModal();
        });
    }

    public hideModal() {
        let modal = JQueryHelper.getElement(this.JQueryModalExpression);
        modal.removeClass('modal-container-visible');
        setTimeout(() => this.isModalOpen = false, 200);
    }

    public showModal(config: ModalWindowModel) {
        let modal;
        this.modalConfig = config;

        // wait for closing of prev window
        setTimeout(() =>  {
            this.isModalOpen = true;

            setTimeout(() => {
                modal = JQueryHelper.getElement(this.JQueryModalExpression);
                modal.addClass('modal-container-visible');
            }, 200);
        }, 500);
    }

    public setModalConfig(config: ModalWindowModel) {
        this.modalConfig = config;
    }

    private setUserInfo(userStatModel: UserStatModel) {
        ConstantStorage.setUserStatModel(userStatModel);

        this.userName = userStatModel.Email.split('@')[0];
        this.router.navigate(['/user-stat']);
        this.isLoading = false;
    }

    private logOut() {
        let modalWindowModel = CommonHelper.buildOkCancelModalConfig(
            `Sign out`,
            `Do you want sign out?`,
            CommonHelper.logOff);

        ModalWindowServise.showModalWindow(modalWindowModel);
    }
}
