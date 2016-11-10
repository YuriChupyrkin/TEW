import { Component, OnInit  } from '@angular/core';
import { ConstantStorage } from './services/constantStorage';
import { HttpService } from './services/httpService';
import { WordsCloudModel } from './models/wordsCloudModel';
import { Word } from './models/word';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Inject } from '@angular/core';

@Component({
    selector: 'add-word',
    templateUrl: '../../scripts/angularjs/app/templates/addWord.html'
})

export class AddWord implements OnInit {
    private wordTranslaterController: string = '/api/WordTranslater';
    private wordsManagerController = '/api/WordsManager';

    private englishWord: string;
    private translates: Array<string>;
    private exampleOfUser: string;
    private chosenTranslate: string;

    private addWordform: FormGroup;

    constructor(private formBuilder: FormBuilder, private httpService: HttpService) {
        this.translates = new Array<string>();
    }

    ngOnInit() {
        console.log('on init');
        this.addWordform = this.formBuilder.group({
            english: ['', Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(15)])],
           // example: ['', Validators.minLength(3)],
           // russian: ['', Validators.maxLength(10)],
        });
    }


    submitForm(value: any): void {
        console.log('Reactive Form Data: ')
        console.dir(value);
    }

    private translate() {
        if (!this.englishWord) {
            return;
        }

        this.clearTranslateResults(false);
        this.translateByYandex();
        this.translateByExistsWords();
    }

    private translateByExistsWords() {
        var url = `${this.wordTranslaterController}?word=${this.englishWord}`;
        this.httpService.processGet<Array<string>>(url).subscribe(response => this.addTranslate(response));
    }

    private translateByYandex() {
        let url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup";
        let translateLang = "en-ru";
        let apiKey = ConstantStorage.getYandexTranslaterApiKey();
        let text = this.englishWord.replace(' ', '%20');

        var resultUri = `${url}?key=${apiKey}&lang=${translateLang}&text=${text}`;
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
                    this.addTranslate([translate['text']]);
                }

                if (translate['syn'] && translate['syn']['length']) {
                    for (var i = 0; i < translate['syn']['length']; i++) {
                        var syn = translate['syn'][i];
                        this.addTranslate([syn['text']]);
                    }
                }

                if (translate && translate['ex'] && translate['ex'][0] && translate['ex'][0]['text']) {
                    this.exampleOfUser = translate['ex'][0]['text'];
                }
            }
        }
    }

    private chooseTranslate(translate: string) {
        this.chosenTranslate = translate;
    }

    private clearTranslateResults(isClearEnglishWord: boolean) {
        if (isClearEnglishWord) {
            this.englishWord = '';
        }

        this.chosenTranslate = '';
        this.exampleOfUser = '';
        this.translates = new Array<string>();
    }

    private addTranslate(translates: Array<string>) {
        var self = this;
        //translates.forEach(function (translate: string) {
        //    if (self.translates.indexOf(translate) == -1) {
        //        self.translates.push(translate);
        //    }
        //});

        translates.forEach(translate => {
            if (self.translates.indexOf(translate) == -1) {
                self.translates.push(translate);
            }
        });
    }

    private save() {
        if (!this.englishWord || !this.chosenTranslate) {
            console.log("English and Translate are required!");
            return;
        }

        var wordCloudModel = new WordsCloudModel();
        wordCloudModel.UserName = ConstantStorage.getUserName();

        var word = new Word();
        word.English = this.englishWord;
        word.Russian = this.chosenTranslate;
        word.UpdateDate = new Date();
        word.Example = this.exampleOfUser;

        wordCloudModel.Words = [word];

        this.httpService.processPost(wordCloudModel, this.wordsManagerController)
            .subscribe(response => this.clearTranslateResults(true), error => alert("error"));
    }
}