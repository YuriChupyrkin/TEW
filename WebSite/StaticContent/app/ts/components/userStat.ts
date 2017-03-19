import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from '../helpers/constantStorage';
import { UserStatModel } from '../models/userStatModel';
import { HttpService } from '../services/httpService';
import { ModalWindowServise } from '../services/modalWindowServise';
import { User } from '../models/user';
import { ModalWindowModel } from '../models/modalWindowModel';
import { CommonHelper } from '../helpers/commonHelper';

@Component({
    selector: 'user-stat',
    templateUrl: '../StaticContent/app/templates/components/userStat.html'
})

export class UserStat implements OnInit  {
    private userStatModel: UserStatModel;

    constructor(private httpService: HttpService) {
    }

    ngOnInit() {
       this.updateUserStat();
    }

    private updateUserStat() {
        let userId = ConstantStorage.getUserId();
        if (userId !== 0 && userId !== undefined) {
            this.httpService.processGet<UserStatModel>(`${ConstantStorage.getUserStatController()}?userId=${userId}`)
                .then(result => this.userStatModel = result);
        }
    }

    private resetUserWords() {
        let modalWindowModel = CommonHelper.buildOkCancelModalConfig(
            `Reset`,
            `Do you really want reset level of your words?`,
            this.resetLevel.bind(this));

        ModalWindowServise.showModalWindow(modalWindowModel);
    }

    private resetLevel() {
        let userId = ConstantStorage.getUserId();
        let user = new User();
        user.Id = userId;

        if (userId !== 0 && userId !== undefined) {
            this.httpService.processPost(user, ConstantStorage.getResetWordsLevelController())
                .then(r => this.updateUserStat());
        }
    }
}