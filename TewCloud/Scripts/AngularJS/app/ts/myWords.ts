import { Component } from '@angular/core';
import { HttpService } from './services/httpService';
import { UserWords } from './models/userWords';
import { Word } from './models/word';
import { WordsCloudModel } from './models/wordsCloudModel';
import { ConstantStorage } from './services/constantStorage';

@Component({
    selector: 'my-words',
    templateUrl: '../../scripts/angularjs/app/templates/myWords.html'
})

export class MyWords {
    private loaded: boolean;
    private userWords: UserWords;
    //testInput: string;

    constructor(private httpService: HttpService) {
        this.userWords = new UserWords();
        //this.testInput = 'test string';
        this.loaded = false;

        this.getWords();
    }

    private getWords() {
        var url = '/api/WordsManager?userName=' + ConstantStorage.getUserName();

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

        var url = 'api/DeleteWord';
        var result = this.httpService.processPost<WordsCloudModel>(wordsCloudModel, url);

        result.subscribe(
            response => this.removedWord(word),
            error => word.Hidden = false);
    }

    private setUserWords(userWords: UserWords) {
        this.loaded = true;
        this.userWords = userWords;

        for (let i = 0; i < this.userWords.Words.length; i++) {
            //this.userWords.Words[i].UpdateDateString = this.userWords.Words[i].UpdateDate.toISOString();
            this.userWords.Words[i].UpdateDateString = this.userWords.Words[i].UpdateDate.toString();
        }
    }

    private removedWord(word: Word) {
        var wordIndex = this.userWords.Words.indexOf(word);

        if (wordIndex == -1) {
            return
        }

        this.userWords.Words.splice(wordIndex, 1);
    }


    //public showWords() {
    //    console.dir(this.userWords);
    //    console.log('testinput: ' + this.testInput);
    //}
}