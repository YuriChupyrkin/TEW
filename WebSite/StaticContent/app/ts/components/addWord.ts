import { Component, OnInit  } from '@angular/core';
import { ConstantStorage } from '../helpers/constantStorage';
import { HttpService } from '../services/httpService';
import { WordsCloudModel } from '../models/wordsCloudModel';
import { Word } from '../models/word';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'add-word',
    templateUrl: '../StaticContent/app/templates/components/addWord.html'
})

export class AddWord implements OnInit {
    private translates: Array<string>;
    private addWordform: FormGroup;
    private translateFor: string;

    constructor(private formBuilder: FormBuilder, private httpService: HttpService) {
        this.translates = new Array<string>();
    }

    ngOnInit() {
        this.addWordform = this.formBuilder.group({
            english: ['', Validators.compose([Validators.required, Validators.maxLength(25)])],
            example: ['', Validators.maxLength(50)],
            russian: ['', Validators.compose([Validators.required, Validators.maxLength(25)])]
        });
    }

    private submitForm(value: any): void {
        this.save(value['english'], value['russian'], value['example']);
    }

    private focusOut() {
        let englishWord = this.addWordform.controls['english'].value;

        if (this.translateFor != englishWord) {
            this.translate();
        }
    }

    private translate() {
        let englishWord = this.addWordform.controls['english'].value;

        if (!englishWord || this.translateFor == englishWord) {
            return;
        }

        this.translateFor = englishWord;

        var englishWordWithoutSpaces = englishWord.replace(' ', '%20');
        this.clearTranslateResults(false);

        this.translateByYandex(englishWordWithoutSpaces);
        this.translateByExistsWords(englishWord);
    }

    private translateByExistsWords(englishWord: string) {
        var url = `${ConstantStorage.getWordTranslaterController()}?word=${englishWord}`;
        this.httpService.processGet<Array<string>>(url).then(response => this.addTranslate(response));
    }

    private translateByYandex(englishWord: string) {
        let url = "https://dictionary.yandex.net/api/v1/dicservice.json/lookup";
        let translateLang = "en-ru";
        let apiKey = ConstantStorage.getYandexTranslaterApiKey();

        var resultUri = `${url}?key=${apiKey}&lang=${translateLang}&text=${englishWord}`;
        this.httpService.processGet<JSON>(resultUri, true)
            .then(response => this.parseTranslate(response));
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
                    var example = translate['ex'][0]['text'];
                    this.addWordform.controls['example'].setValue(example.toString());
                }
            }
        }
    }

    private chooseTranslate(translate: string) {
        this.addWordform.controls['russian'].setValue(translate);
    }

    private clearTranslateResults(isClearEnglishWord: boolean) {
        if (isClearEnglishWord) {
            this.addWordform.controls['english'].reset();
        }

        this.addWordform.controls['russian'].reset();
        this.addWordform.controls['example'].reset();
        this.translates = new Array<string>();
    }

    private addTranslate(translates: Array<string>) {
        var self = this;

        translates.forEach(translate => {
            if (self.translates.indexOf(translate) == -1) {
                self.translates.push(translate);
            }
        });
    }

    private save(englishWord: string, russianWord: string, example: string) {
        if (!englishWord || !russianWord) {
            console.log("English and Translate are required!");
            return;
        }

        if (englishWord != this.translateFor && this.translateFor != undefined){
            this.clearTranslateResults(false);
            return;
        }

        var wordCloudModel = new WordsCloudModel();
        wordCloudModel.UserName = ConstantStorage.getUserName();

        var word = new Word();
        word.English = englishWord;
        word.Russian = russianWord;
        word.UpdateDate = new Date();
        word.Example = example;

        wordCloudModel.Words = [word];

        this.httpService.processPost(wordCloudModel, ConstantStorage.getWordsManagerController())
            .then(response => console.dir(response), error => alert("error"));

        this.clearTranslateResults(true);
    }
}