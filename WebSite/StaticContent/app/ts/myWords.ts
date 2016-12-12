import { Component } from '@angular/core';
import { HttpService } from './services/httpService';
import { UserWords } from './models/userWords';
import { Word } from './models/word';
import { WordsCloudModel } from './models/wordsCloudModel';
import { ConstantStorage } from './services/constantStorage';

@Component({
    selector: 'my-words',
    templateUrl: '../StaticContent/app/templates/myWords.html'
})

export class MyWords {
    private loaded: boolean;
    private userWords: UserWords;
    private wordsCount: number;

    constructor(private httpService: HttpService) {
        this.userWords = new UserWords();
        this.loaded = false;

        this.getWords();
    }

    private getWords() {
        var url = `${ConstantStorage.getWordsManagerController()}?userName=${ConstantStorage.getUserName()}`;

        var result = this.httpService.processGet<UserWords>(url);
        result.subscribe(json => this.setUserWords(json));
    }

    private removeWord(word: Word) {
        console.dir(word);

        // hidden
        word.Hidden = true;

        var wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.UserName = ConstantStorage.getUserName();
        wordsCloudModel.Words = [word];

        var result = this.httpService.processPost<WordsCloudModel>(wordsCloudModel, ConstantStorage.getDeleteWordController());

        result.subscribe(
            response => this.removedWord(word),
            error => word.Hidden = false);
    }

    private setUserWords(userWords: UserWords) {
        this.loaded = true;
        this.userWords = userWords;
        this.wordsCount = userWords.Words.length;
    }

    private removedWord(word: Word) {
        var wordIndex = this.userWords.Words.indexOf(word);

        if (wordIndex == -1) {
            return
        }

        this.userWords.Words.splice(wordIndex, 1);
        this.wordsCount--;
    }
}