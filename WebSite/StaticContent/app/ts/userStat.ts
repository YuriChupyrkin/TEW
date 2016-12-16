import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { UserStatModel } from './models/userStatModel';
import { HttpService } from './services/httpService';
import { ModalWindowServise } from './services/modalWindowServise';
import { User } from './models/user';

@Component({
    selector: 'user-stat',
    templateUrl: '../StaticContent/app/templates/userStat.html'
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
         let modalWindowConfig = {
             headerText: 'Reset',
             bodyText: 'Do you really want reset level of your words?',
             isApplyButton: true,
             isCancelButton: true,
             applyButtonText: 'Yes',
             cancelButtonText: 'No',
             applyCallback: () => this.resetLevel()
        }

        ModalWindowServise.showModalWindow(modalWindowConfig);
    }

    private resetLevel(){
        var userId = ConstantStorage.getUserId();
        var user = new User();
        user.Id = userId;

        if (userId != 0 && userId != undefined) {
            this.httpService.processPost<User>(user, ConstantStorage.getResetWordsLevelController())
                .then(r => this.updateUserStat());
        } 
    }
}