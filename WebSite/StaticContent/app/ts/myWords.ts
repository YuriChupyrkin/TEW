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
    private readonly wordsPerPage = 50;
    private wordsCount: number;
    private words: Array<Word>;
    private isLoading = false;

    constructor(private httpService: HttpService) {
        this.words = new Array<Word>();
        this.wordsCount = 999999;

        this.loadWords();
    }

    private removeWord(word: Word) {
        this.startLoading();

        // hidden
        word.Hidden = true;

        var wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.UserName = ConstantStorage.getUserName();
        wordsCloudModel.Words = [word];

        var result = this.httpService.processPost(wordsCloudModel, ConstantStorage.getDeleteWordController());

        result.then(
            response => { this.removedWord(word); this.endLoading(); },
            error => word.Hidden = false);
    }

    private setUserWords(userWords: UserWords) {
        if (userWords.Words) { 
            this.wordsCount = userWords.TotalWords;
            this.words.push.apply(this.words, userWords.Words);
        }
    }

    private removedWord(word: Word) {
        var wordIndex = this.words.indexOf(word);

        if (wordIndex == -1) {
            return
        }

        this.words.splice(wordIndex, 1);
        this.wordsCount--;
    }

    // ************* PAGING LOGIC (START START REGION) ****************
    @HostListener('window:scroll', ['$event']) 
    private scrollEvent (event) {
        var scrollTop = document.body.scrollTop;
        var scrollHeight = document.body.scrollHeight;
        var clientHeight = document.documentElement.clientHeight;
        
        if (scrollTop + clientHeight + 50 >= scrollHeight && this.canLoadPage()) {
            this.loadWords();
        }
    }

    @HostListener('window: resize', ['$event'])
    private resuzeEvent (event) {
        this.isNeedMoreWords();
    }

    private loadWords () {
        this.startLoading();

        var url = `${ConstantStorage.getWordsManagerController()}`;
        url += `?UserId=${ConstantStorage.getUserId()}`;
        url += `&CurrentWordsCount=${this.words.length}`;
        url += `&WordsPerPage=${this.wordsPerPage}`;

        var result = this.httpService.processGet<UserWords>(url);
        result.then(
            json => { this.setUserWords(json); this.endLoading(); this.isNeedMoreWords(); },
            error => { this.endLoading(); alert('ERROR of loading words!!!'); });
    }

    // when page is loaded but scroll is hidden
    private isNeedMoreWords () {
        var self = this;

        // wait for render
        setTimeout(function () {
            var scrollHeight = document.body.scrollHeight;
            var clientHeight = document.documentElement.clientHeight;

            if (scrollHeight == clientHeight && this.canLoadPage()) {
                this.loadWords();
            }
        }, 300);
    }

    private startLoading () {
        // disable scroll
        var x=window.scrollX;
        var y=window.scrollY;
        window.onscroll = function () { window.scrollTo(x, y); };

        this.isLoading = true;
    }

    private endLoading () {
        window.onscroll = function() {};
        this.isLoading = false;
    }

    private fakeAddWords () {
        if (this.words.length < 1000) {
            this.words.push.apply(this.words, this.words);
        }
    }

    private canLoadPage () {
        return this.wordsCount > this.words.length && this.isLoading == false;
    }

    // ************* PAGING LOGIC (END OF REGION) ****************
}