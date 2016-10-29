///<reference path="./../../typings/main.d.ts"/>
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app';
import { Home } from './home';
import { AddWord } from './addWord';
import { appRoutingProviders, routing } from './app.routes';
import { APP_BASE_HREF } from '@angular/common';

@NgModule({
    imports: [BrowserModule, routing ],

    declarations: [AppComponent, Home, AddWord],
    bootstrap: [AppComponent],
    providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        appRoutingProviders
    ]
})
export class AppModule { }