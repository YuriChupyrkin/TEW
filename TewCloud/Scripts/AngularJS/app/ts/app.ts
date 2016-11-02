import { Component } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
declare var $: any;

@Component({
    selector: 'my-app',
    templateUrl: '../../scripts/angularjs/app/templates/app.html'
})

export class AppComponent {
    constructor() {
        // TODO: fix it
        ConstantStorage.setUserName('yurec37@yandex.ru');
        ConstantStorage.setYandexTranslaterApiKey('dict.1.1.20160904T125311Z.5e2c6c9dfb5cd3c3.71b0d5220878e340d60dcfa0faf7f649af59c65f');
    }
}