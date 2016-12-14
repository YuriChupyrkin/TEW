import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { UserStatModel } from './models/userStatModel';
import { HttpService } from './services/httpService';

@Component({
    selector: 'user-stat',
    templateUrl: '../StaticContent/app/templates/userStat.html'
})

export class UserStat implements OnInit  {
    private userStatModel: UserStatModel;

    constructor(private httpService: HttpService){
    }
    
    ngOnInit() {
        var userId = ConstantStorage.getUserId();
        if (userId != 0 && userId != undefined) {
            this.httpService.processGet<UserStatModel>(`${ConstantStorage.getUserStatController()}?userId=${userId}`)
                .then(result => this.userStatModel = result);
        }
    }
}