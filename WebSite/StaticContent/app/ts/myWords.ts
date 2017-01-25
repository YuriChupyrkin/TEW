import { Component, HostListener } from '@angular/core';
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
    private readonly wordsPerPage = 100;
    private currentPage: number;
    private userWords: UserWords;
    private wordsCount: number;

    constructor(private httpService: HttpService) {
        this.userWords = new UserWords();
        this.currentPage = 1;

        this.getWords();
    }
    
    @HostListener('window:scroll', ['$event']) 
    private doSomething(event) {
        var scrollTop = document.body.scrollTop;
        var scrollHeight = document.body.scrollHeight;
        var clientHeight = document.documentElement.clientHeight;
        
        console.log('---------------------------');
        console.debug("Scroll scrollTop", scrollTop);
        console.debug("Scroll scrollHeight", scrollHeight);
        console.debug("Scroll clientHeight", clientHeight);
        console.log('---------------------------');

        if (scrollTop + clientHeight + 50 >= scrollHeight) {
            console.log('!!!!!!!! LOAD !!!!!!!!!!!!!!');
            this.fakeAddWords();
        }
    }

    private fakeAddWords () {
        this.userWords.Words.push.apply(this.userWords.Words, this.userWords.Words);
    }

    private getWords() {
        var url = `${ConstantStorage.getWordsManagerController()}?userName=${ConstantStorage.getUserName()}`;

        var result = this.httpService.processGet<UserWords>(url);
        result.then(json => this.setUserWords(json));
    }

    private removeWord(word: Word) {
        console.dir(word);

        // hidden
        word.Hidden = true;

        var wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.UserName = ConstantStorage.getUserName();
        wordsCloudModel.Words = [word];

        var result = this.httpService.processPost<WordsCloudModel>(wordsCloudModel, ConstantStorage.getDeleteWordController());

        result.then(
            response => this.removedWord(word),
            error => word.Hidden = false);
    }

    private setUserWords(userWords: UserWords) {
        this.userWords = userWords;

        if(this.userWords.Words) { 
            // this.wordsCount = userWords.Words.length;
            this.wordsCount = userWords.TotalWords;
        }
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