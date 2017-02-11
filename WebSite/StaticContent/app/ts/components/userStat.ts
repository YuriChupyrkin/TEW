import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from '../helpers/constantStorage';
import { UserStatModel } from '../models/userStatModel';
import { HttpService } from '../services/httpService';
import { ModalWindowServise } from '../services/modalWindowServise';
import { User } from '../models/user';
import { ModalWindowModel } from '../models/modalWindowModel';

@Component({
    selector: 'user-stat',
    templateUrl: '../StaticContent/app/templates/components/userStat.html'
})

export class UserStat implements OnInit  {
    private userStatModel: UserStatModel;

    constructor(private httpService: HttpService){
    }
    
    ngOnInit() {
       this.updateUserStat();
    }

    private updateUserStat() {
        var userId = ConstantStorage.getUserId();
        if (userId != 0 && userId != undefined) {
            this.httpService.processGet<UserStatModel>(`${ConstantStorage.getUserStatController()}?userId=${userId}`)
                .then(result => this.userStatModel = result);
        }
    }

    private resetUserWords() {
        var modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = 'Reset';
        modalWindowModel.BodyText = `Do you really want reset level of your words?`;
        modalWindowModel.IsApplyButton = true;
        modalWindowModel.IsCancelButton = true;
        modalWindowModel.ApplyButtonText = 'Yes';
        modalWindowModel.CancelButtonText = 'No';
        modalWindowModel.ApplyCallback = () => this.resetLevel();

        ModalWindowServise.showModalWindow(modalWindowModel);
    }

    private resetLevel(){
        var userId = ConstantStorage.getUserId();
        var user = new User();
        user.Id = userId;

        if (userId != 0 && userId != undefined) {
            this.httpService.processPost(user, ConstantStorage.getResetWordsLevelController())
                .then(r => this.updateUserStat());
        } 
    }
}