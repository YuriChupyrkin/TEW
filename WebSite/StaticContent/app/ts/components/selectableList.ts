import { Component, Input, Output, EventEmitter} from '@angular/core';
import { SelectableListItemModel } from '../models/selectableListItemModel';

@Component({
    selector: 'selectable-list',
    templateUrl: '../../StaticContent/app/templates/components/selectableList.html',
    styleUrls: ['../StaticContent/app/css/components/selectableList.css']
})

export class SelectableList {
    @Input() header: string;
    @Input() showSecondary: boolean;
    @Input() items: Array<SelectableListItemModel>;
    @Output() itemClick: EventEmitter<SelectableListItemModel> = new EventEmitter();
    @Output() itemDbclick: EventEmitter<SelectableListItemModel> = new EventEmitter();

    constructor() {
    }

    private click(item: SelectableListItemModel) {
        this.setActive(item);
        this.itemClick.emit(item);
    }

    private dbclick(item: SelectableListItemModel) {
        this.itemDbclick.emit(item);
    }

    private setActive(activeItem: SelectableListItemModel) {
        this.items.forEach(item => {
            item.IsActive = activeItem.Value === item.Value;
        });
    }
}
