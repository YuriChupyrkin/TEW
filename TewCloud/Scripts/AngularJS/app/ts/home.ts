﻿import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { CommonHelper } from './services/commonHelper';
import { UserStatModel } from './models/userStatModel';
import { HttpService } from './services/httpService';

@Component({
    selector: 'home',
    templateUrl: '../../scripts/angularjs/app/templates/home.html'
})

export class Home implements OnInit  {
    private userName: string;
    private userStatModel: UserStatModel;

    constructor(private httpService: HttpService){

    }

    private logOff() {
        CommonHelper.logOff();
    }

    ngOnInit() {
        this.userName = ConstantStorage.getUserName();

        var userId = ConstantStorage.getUserId();
        if (userId != 0 && userId != undefined) {
            this.httpService.processGet<UserStatModel>(`${ConstantStorage.getUserStatController()}?userId=${userId}`)
                .subscribe(result => this.userStatModel = result);
        }
    }
}
