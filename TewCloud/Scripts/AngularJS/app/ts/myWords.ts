import { Component } from '@angular/core';
import { GetPostService } from './services/getPostService';
import { UserWords } from './models/userWords';

@Component({
    selector: 'my-words',
    templateUrl: '../../scripts/angularjs/app/templates/myWords.html'
})

export class MyWords {
    description: string = "myWords";
    userWords: UserWords;

    constructor(private getPostService: GetPostService) {
        this.userWords = new UserWords();
        this.userWords.UserName = 'hello';
    }

    public getWords() {
        var url = '/api/WordsManager?userName=yurec37@yandex.ru';
        var result = this.getPostService.getRequest<UserWords>(url);

        let js = 'json';
        result.subscribe(json => this.userWords = json);
    }

    public showWords() {
        console.dir(this.userWords);
    }
}