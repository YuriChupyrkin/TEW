import { Component, HostListener } from '@angular/core';
import { HttpService } from '../services/httpService';
import { UserWords } from '../models/userWords';
import { Word } from '../models/word';
import { WordsCloudModel } from '../models/wordsCloudModel';
import { ConstantStorage } from '../helpers/constantStorage';
import { ModalWindowModel } from '../models/modalWindowModel';
import { ModalWindowServise } from '../services/modalWindowServise';
import { EditMyWord } from '../helpComponents/editMyWord';
import { PubSub } from '../services/pubSub';
import { CommonHelper } from '../helpers/commonHelper';

@Component({
    selector: 'my-words',
    templateUrl: '../StaticContent/app/templates/components/myWords.html'
})

export class MyWords {
    private readonly wordsPerPage = 50;
    private wordsCount: number;
    private words: Array<Word>;
    private isLoading = false;
    private sortKey: string;
    private sortAsc: boolean;

    constructor(private httpService: HttpService) {
        this.words = new Array<Word>();
        this.wordsCount = 999999;
        this.sortKey = 'level';
        this.sortAsc = true;

        PubSub.Clear('editWord');
        PubSub.Sub('editWord', (...args: Array<any>) => this.updateWordAfterEdit(args));
        this.loadWords();
    }

    private askRemoveWord(word: Word) {
        let config = CommonHelper.buildOkCancelModalConfig(
            `Remove word`,
            `Do you really want remove ${word.English}`,
            this.removeWord.bind(this, word));

        ModalWindowServise.showModalWindow(config);
    }

    private removeWord(word: Word) {
        this.startLoading();

        // hidden
        word.Hidden = true;

        let wordsCloudModel = new WordsCloudModel();
        wordsCloudModel.UserName = ConstantStorage.getUserName();
        wordsCloudModel.Words = [word];

        let result = this.httpService.processPost(wordsCloudModel, ConstantStorage.getDeleteWordController());

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
        let wordIndex = this.words.indexOf(word);

        if (wordIndex === -1) {
            return
        }

        this.words.splice(wordIndex, 1);
        this.wordsCount--;
    }

    // ************* EDIT LOGIC *********************
    private updateWordAfterEdit(...args: Array<any>) {
        if (args && args.length) {
            let word = args[0][0];
            let index = this.words.map(item => {
                return item.English;
            }).indexOf(word.English);

            if (index !== -1) {
                this.words[index] = word;
            }
        }
    }

    private editWord(word: Word) {
        let modalWindowModel = new ModalWindowModel();
        modalWindowModel.HeaderText = `Edit word: "${word.English}"`;
        modalWindowModel.IsCancelButton = false;
        modalWindowModel.InnerComponent = true;
        modalWindowModel.InnerComponentType = EditMyWord;
        modalWindowModel.InnerComponentOptions = word;

        ModalWindowServise.showModalWindow(modalWindowModel);
    }

    // ************* END OF EDIT LOGIC **************

    // ************* SORT LOGIC *********************

    private headerClick(sortKey: string) {
        if (sortKey === this.sortKey) {
            this.sortAsc = !this.sortAsc;
        }
        else {
            this.sortKey = sortKey;
            this.sortAsc = true;
        }

        // reset words
        this.words = []
        this.loadWords();
    }
    // ************* END OF SORT LOGIC **************

    // ************* PAGING LOGIC (START START REGION) ****************
    @HostListener('window:scroll', ['$event'])
    private scrollEvent (event) {
        let scrollTop = (document.documentElement && document.documentElement.scrollTop)
            || document.body.scrollTop;
        let scrollHeight = document.body.scrollHeight;
        let clientHeight = document.documentElement.clientHeight;
        let tenPercents = (scrollTop + clientHeight) / 100 * 10;

        if (scrollTop + clientHeight + tenPercents >= scrollHeight && this.canLoadPage()) {
            this.loadWords();
        }
    }

    @HostListener('window: resize', ['$event'])
    private resuzeEvent (event) {
        this.isNeedMoreWords();
    }

    private loadWords () {
        let userId = ConstantStorage.getUserId();

        if (!userId) {
            return;
        }

        this.startLoading();

        let url = `${ConstantStorage.getWordsManagerController()}`;
        url += `?UserId=${userId}`;
        url += `&CurrentWordsCount=${this.words.length}`;
        url += `&WordsPerPage=${this.wordsPerPage}`;
        url += `&sortKey=${this.sortKey}`;
        url += `&sortAsc=${this.sortAsc}`;

        let result = this.httpService.processGet<UserWords>(url);
        result.then(
            json => { this.setUserWords(json); this.endLoading(); this.isNeedMoreWords(); },
            error => { this.endLoading(); alert('ERROR of loading words!'); });
    }

    // when page is loaded but scroll is hidden
    private isNeedMoreWords () {
        let self = this;

        // wait for render
        setTimeout(function () {
            let scrollHeight = document.body.scrollHeight;
            let clientHeight = document.documentElement.clientHeight;

            if (scrollHeight === clientHeight && self.canLoadPage()) {
                self.loadWords();
            }
        }, 300);
    }

    private startLoading () {
        // disable scroll
        let x = window.scrollX;
        let y = window.scrollY;
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
        return this.wordsCount > this.words.length && this.isLoading === false;
    }

    // ************* PAGING LOGIC (END OF REGION) ****************
}