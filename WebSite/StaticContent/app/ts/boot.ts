///<reference path="./../../typings/main.d.ts"/>
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app';
import { Home } from './home';
import { AddWord } from './addWord';
import { MyWords } from './myWords';
import { PickerTest } from './pickerTest';
import { appRoutingProviders, routing } from './app.routes';
import { APP_BASE_HREF } from '@angular/common';
import { HttpService } from './services/httpService';
import { HttpModule } from '@angular/http';
import { LoadingAnimation } from './helpComponents/loadingAnimation';
import { ProgressBar } from './helpComponents/progressBar';

// For textbox binding
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';

@NgModule({
    imports: [BrowserModule, routing, HttpModule, FormsModule, ReactiveFormsModule],

    declarations: [AppComponent, Home, AddWord, MyWords, PickerTest, LoadingAnimation, ProgressBar],

    bootstrap: [AppComponent],

    providers: [
        { provide: APP_BASE_HREF, useValue: '/#' },
        appRoutingProviders,
        HttpService
    ]
})
export class AppModule { }