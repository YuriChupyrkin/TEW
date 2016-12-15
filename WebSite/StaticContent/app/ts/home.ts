import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { CommonHelper } from './services/commonHelper';
import { UserStatModel } from './models/userStatModel';
import { HttpService } from './services/httpService';
import { ModalWindowServise } from './services/modalWindowServise';

@Component({
    selector: 'home',
    templateUrl: '../StaticContent/app/templates/home.html'
})

export class Home implements OnInit  {
    private usersCount: number;
    private wordsCount: number;
    private userStatModel: UserStatModel;

    constructor(private httpService: HttpService){
    }

    ngOnInit() {
        this.httpService.processGet<any>(ConstantStorage.getTewInfoContoller())
            .then(response => { 
                this.usersCount = response.Users; 
                this.wordsCount = response.Words;
            });
    }
}
