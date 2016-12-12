import { Component, OnInit } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';
import { User } from './models/user';
import { Router} from '@angular/router';

@Component({
    selector: 'my-app',
    templateUrl: '../StaticContent/app/templates/appNext.html'
})

export class AppComponent implements OnInit {
    private userName: string;
    private applicationMessage: string;
    private desktopMode: boolean = true;

    constructor(private httpService: HttpService, private router: Router) {
        ConstantStorage.setYandexTranslaterApiKey('dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f');

        this.userName = '';
        this.httpService.processGet<User>(ConstantStorage.getUserInfoController()).subscribe(response => this.setUserInfo(response));

        this.httpService.processGet<string>(ConstantStorage.getApplicationMessageController())
            .subscribe(response => this.applicationMessage = response);
    }

    ngOnInit() {
        this.resized(null);
    }

    private setUserInfo(user: User) {
        ConstantStorage.setUserName(user.Email);
        ConstantStorage.setUserId(user.Id);

        this.userName = user.Email;
        this.router.navigate(['/home']);
    }

    private resized(event: any) {
        var width = window.innerWidth;

        if (width < 980) {
            this.desktopMode = false;
        } else {
            this.desktopMode = true;
        }

        console.log("resized: " + this.desktopMode);
    }
}