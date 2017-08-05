import { Component, HostListener, Input } from '@angular/core';

@Component({
    selector: 'toolbar',
    templateUrl: '../StaticContent/app/templates/components/toolbar.html',
    styleUrls: ['../StaticContent/app/css/components/toolbar.css']
})

export class Toolbar {
    @Input() Logout: any;
    @Input() UserName: string;

    private desktopMode: boolean;
    private isLinkVisible: boolean;

    constructor() {
        this.setDesktopMode();
    }

    @HostListener('window: resize', ['$event'])
    private resuzeEvent (event) {
        this.setDesktopMode();
    }

    private setDesktopMode() {
        this.desktopMode = window.innerWidth > 780;
        this.isLinkVisible = this.desktopMode;
    }

    private logOut() {
        if (typeof this.Logout === 'function') {
            this.Logout();
        }
        this.linkClick();
    }

    private showHideLinks() {
        this.isLinkVisible = !this.isLinkVisible;
    }

    private linkClick() {
        if (!this.desktopMode) {
            this.isLinkVisible = false;
        }
    }
}
