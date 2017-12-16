import { Component, Input } from '@angular/core';
import '../../scss/components/common.scss';

@Component({
    selector: 'my-words-header',
    templateUrl: '../../StaticContent/app/templates/helpComponents/myWordsHeader.html',
})

export class MyWordsHeader {
    @Input() Title: string;
    @Input() SortKey: string;
    @Input() Asc: boolean;
    @Input() SortProperty: string;
}