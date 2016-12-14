import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { CommonHelper } from './services/commonHelper';
import { UserStatModel } from './models/userStatModel';
import { HttpService } from './services/httpService';
import { ModalWindow } from './helpComponents/ModalWindow';

@Component({
    selector: 'home',
    templateUrl: '../StaticContent/app/templates/home.html'
})

export class Home implements OnInit  {
    private userName: string;
    private userStatModel: UserStatModel;
    private modalConfig: any;

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

        this.modalConfig = {
            headerText: 'Hello',
            bodyText: 'My name is....',
            isApplyButton: true,
            applyButtonText: 'apply'
        }
    }

    private modalApplied(){
        console.log('home applied');
    }

    public setModalWindow1(){
         this.modalConfig = {
            headerText: 'Hello 1',
            bodyText: 'My name is Yuri',
            isApplyButton: true,
            applyButtonText: 'ok'
        }
    }

    public setModalWindow2(){
         this.modalConfig = {
            headerText: 'Header for 2',
            bodyText: 'text text text',
            isApplyButton: true,
            applyButtonText: 'da!'
        }

        ModalWindow.showWindow();
    }
}
