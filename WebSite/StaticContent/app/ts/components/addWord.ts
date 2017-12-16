import { Component, OnInit  } from '@angular/core';
import { ConstantStorage } from '../helpers/constantStorage';
import { HttpService } from '../services/httpService';
import { WordsCloudModel } from '../models/wordsCloudModel';
import { Word } from '../models/word';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TranslateModel } from '../models/translateModel';
import { CommonHelper } from '../helpers/commonHelper';
import { SelectableListItemModel } from '../models/selectableListItemModel';
import '../../scss/components/addWord.scss';
import '../../scss/components/common.scss';

@Component({
    selector: 'add-word',
    templateUrl: '../StaticContent/app/templates/components/addWord.html',
})

export class AddWord implements OnInit {
    private translates: Array<SelectableListItemModel>;
    private addWordform: FormGroup;
    private translateFor: string;
    private testTrasnlates: Array<SelectableListItemModel>;

    constructor(private formBuilder: FormBuilder, private httpService: HttpService) {
        this.translates = new Array<SelectableListItemModel>();
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

        if (this.translateFor !== englishWord) {
            this.translate();
        }
    }

    private translate() {
        let englishWord = this.addWordform.controls['english'].value;

        if (!englishWord || this.translateFor === englishWord) {
            return;
        }

        this.translateFor = englishWord;

        let englishWordWithoutSpaces = englishWord.replace(' ', '%20');
        this.clearTranslateResults(false);

        this.translateByYandex(englishWordWithoutSpaces);
        this.translateByExistsWords(englishWord);
    }

    private translateByExistsWords(englishWord: string) {
        let url = `${ConstantStorage.getWordTranslaterController()}?word=${englishWord}`;
        this.httpService.processGet<Array<string>>(url)
            .then(response => this.addInternalTranslate(response));
    }

    private translateByYandex(englishWord: string) {
        let url = 'https://dictionary.yandex.net/api/v1/dicservice.json/lookup';
        let translateLang = 'en-ru';
        let apiKey = ConstantStorage.getYandexTranslaterApiKey();

        let resultUri = `${url}?key=${apiKey}&lang=${translateLang}&text=${englishWord}`;
        this.httpService.processGet<JSON>(resultUri, true)
            .then(response => this.parseTranslate(response));
    }

    private parseTranslate(response: JSON) {
        let def = response['def'];
        if (def && def['0']) {
            let defZero = def['0'];

            if (defZero && defZero['tr'] && defZero['tr']['0']) {
                let translate = defZero['tr']['0'];

                if (translate['text']) {
                    this.addYandexTranslate([translate['text']]);
                }

                if (translate['syn'] && translate['syn']['length']) {
                    for (let i = 0; i < translate['syn']['length']; i++) {
                        let syn = translate['syn'][i];
                        this.addYandexTranslate([syn['text']]);
                    }
                }

                if (translate && translate['ex'] && translate['ex'][0] && translate['ex'][0]['text']) {
                    let example = translate['ex'][0]['text'];
                    this.addWordform.controls['example'].setValue(example.toString());
                }
            }
        }
    }

    private chooseTranslate(item: SelectableListItemModel) {
        this.addWordform.controls['russian'].setValue(item.Value);
    }

    private clearTranslateResults(isClearEnglishWord: boolean) {
        if (isClearEnglishWord) {
            this.addWordform.controls['english'].reset();
        }

        this.addWordform.controls['russian'].reset();
        this.addWordform.controls['example'].reset();
        this.translates = new Array<SelectableListItemModel>();
    }

    private addYandexTranslate(translates: Array<string>) {
        let self = this;

        translates.forEach(translate => {
            if (!self.translates.find(item => CommonHelper.isStringEqualInLower(item.Value, translate))) {
                self.translates.push(<SelectableListItemModel>
                    {
                        Value: translate,
                        IsHighlighted: false
                    }
                );
            }
        });
    }

    private addInternalTranslate(internalTranslates: Array<any>) {
        let self = this;

        internalTranslates.forEach(internalTranslate => {
            let existedTranslate =
                self.translates
                    .find(item => CommonHelper.isStringEqualInLower(item.Value, internalTranslate.Russian));
            let isUserWord = internalTranslate.UserId === ConstantStorage.getUserId();

            if (existedTranslate) {
                if (!existedTranslate.IsHighlighted) {
                    existedTranslate.IsHighlighted = isUserWord;
                    existedTranslate.SecondaryValue = isUserWord ? `Level: ${internalTranslate.Level}` : '';
                }
            } else {
                self.translates.push(<SelectableListItemModel>
                    {
                        Value: internalTranslate.Russian,
                        IsHighlighted: isUserWord,
                        SecondaryValue: isUserWord ? `Level: ${internalTranslate.Level}` : ''
                    }
                );
            }
        });
    }

    private save(englishWord: string, russianWord: string, example: string) {
        if (!englishWord || !russianWord) {
            console.log('English and Translate are required!');
            return;
        }

        if (englishWord !== this.translateFor && this.translateFor !== undefined) {
            this.clearTranslateResults(false);
            return;
        }

        let wordCloudModel = new WordsCloudModel();
        wordCloudModel.UserName = ConstantStorage.getUserName();

        let word = new Word();
        word.English = englishWord;
        word.Russian = russianWord;
        word.UpdateDate = new Date();
        word.Example = example;

        wordCloudModel.Words = [word];

        this.httpService.processPost(wordCloudModel, ConstantStorage.getWordsManagerController())
            .then(response => console.dir(response), error => alert('error'));

        this.clearTranslateResults(true);
    }

    private translateChange(value: any) {
        console.log('translateChange');
        console.log(value);

        let translate = this.addWordform.controls['russian'].value;
        this.translates.forEach(item => {
            item.IsActive = item.Value === translate;
        });
    }
}
