///<reference path="./../../typings/main.d.ts"/>
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app';
import { Home } from './home';
import { AddWord } from './addWord';
import { MyWords } from './myWords';
import { appRoutingProviders, routing } from './app.routes';
import { APP_BASE_HREF } from '@angular/common';
import { GetPostService } from './services/getPostService';
import { HttpModule } from '@angular/http';

@NgModule({
    imports: [BrowserModule, routing, HttpModule ],

    declarations: [AppComponent, Home, AddWord, MyWords],

    bootstrap: [AppComponent],

    providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
        appRoutingProviders,
        GetPostService
    ]
})
export class AppModule { }