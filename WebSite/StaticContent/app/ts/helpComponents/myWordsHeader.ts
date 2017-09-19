import { Component, Input } from '@angular/core';

@Component({
    selector: 'my-words-header',
    templateUrl: '../../StaticContent/app/templates/helpComponents/myWordsHeader.html',
    styleUrls: ['../StaticContent/app/css/components/common.css']
})

export class MyWordsHeader {
    @Input() Title: string;
    @Input() SortKey: string;
    @Input() Asc: boolean;
    @Input() SortProperty: string;
}