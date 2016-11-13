import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { CommonHelper } from './services/commonHelper';

@Component({
    selector: 'home',
    templateUrl: '../../scripts/angularjs/app/templates/home.html'
})

export class Home implements OnInit  {
    private userName: string;

    private logOff() {
        CommonHelper.logOff();
    }

    ngOnInit() {
        this.userName = ConstantStorage.getUserName();
    }
}
