import { Component } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';

@Component({
    selector: 'add-word',
    templateUrl: '../../scripts/angularjs/app/templates/addWord.html'
})

export class AddWord {
    private englishWord: string;
    private translates: Array<string>;
    private exampleOfUser: string;
    private chosenTranslate: string;

    constructor(private httpService: HttpService) {
        this.translates = new Array<string>();
        this.englishWord = 'cat';
    }

    private translate() {
        if (!this.englishWord) {
            return;
        }

        console.log('translate....');
        this.clearTranslateResults();
        this.translateByYandex();
        this.translateByExistsWords();
    }

    private translateByExistsWords() {
        console.log("translateByExistsWords");
    }

    private translateByYandex() {
        let url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup";
        let translateLang = "en-ru";
        let apiKey = ConstantStorage.getYandexTranslaterApiKey();

        var resultUri = `${url}?key=${apiKey}&lang=${translateLang}&text=${this.englishWord}`;
        this.httpService.processGet<JSON>(resultUri)
            .subscribe(response => this.parseTranslate(response));
    }

    private parseTranslate(response: JSON) {
        var def = response['def'];
        if (def && def['0']) {
            var defZero = def['0'];

            if (defZero && defZero['tr'] && defZero['tr']['0']) {
                var translate = defZero['tr']['0'];
                console.log(translate);

                if (translate['text']) {
                    this.translates.push(translate['text'].toString());
                }

                if (translate['syn'] && translate['syn']['length']) {
                    for (var i = 0; i < translate['syn']['length']; i++) {
                        var syn = translate['syn'][i];
                        this.translates.push(syn['text']);
                    }
                }

                if (translate && translate['ex'] && translate['ex'][0] && translate['ex'][0]['text']) {
                    this.exampleOfUser = translate['ex'][0]['text'];
                }
            }
        }
    }

    private chooseTranslate(translate: string) {
        console.log("choose: " + translate);
        this.chosenTranslate = translate;
    }

    private clearTranslateResults() {
        this.chosenTranslate = '';
        this.exampleOfUser = '';
        this.translates = new Array<string>();
    }

    private save() {
        console.log("SAVE");
    }



        //if (data && data.def) {
        //    for (var i = 0; i < data.def.length; i++) {
        //        console.log("DEF");
        //    }
        //}
}