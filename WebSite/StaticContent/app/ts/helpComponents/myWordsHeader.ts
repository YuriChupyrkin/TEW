import { Component, Input } from '@angular/core';

@Component({
    selector: 'my-words-header',
    templateUrl: '../../StaticContent/app/templates/helpComponents/myWordsHeader.html'
})

export class MyWordsHeader {
    @Input() Title: string;
    @Input() SortKey: string;
    @Input() Asc: boolean;
    @Input() SortProperty: string;
}