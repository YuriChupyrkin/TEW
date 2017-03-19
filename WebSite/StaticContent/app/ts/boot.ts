///<reference path="./../../typings/main.d.ts"/>
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './components/app';
import { Home } from './components/home';
import { AddWord } from './components/addWord';
import { MyWords } from './components/myWords';
import { PickerTest } from './components/pickerTest';
import { appRoutingProviders, routing } from './app.routes';
import { APP_BASE_HREF } from '@angular/common';
import { HttpService } from './services/httpService';
import { LoadingAnimation } from './helpComponents/loadingAnimation';
import { LoadingLockDisplay } from './helpComponents/loadingLockDisplay';
import { ProgressBar } from './helpComponents/progressBar';
import { ModalWindow } from './helpComponents/modalWindow';
import { UserStat } from './components/userStat';
import { IrregularVerbs } from './components/irregularVerbs';
import { MyWordsHeader } from './helpComponents/myWordsHeader';
import { EditMyWord } from './helpComponents/editMyWord';

// For textbox binding
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';

@NgModule({
    imports: [BrowserModule, routing, HttpModule, FormsModule, ReactiveFormsModule],

    declarations: [
        AppComponent, 
        Home, 
        AddWord, 
        MyWords, 
        PickerTest, 
        LoadingAnimation, 
        ProgressBar, 
        ModalWindow, 
        UserStat,
        IrregularVerbs,
        LoadingLockDisplay,
        MyWordsHeader,
        EditMyWord
    ],

    bootstrap: [AppComponent],

    entryComponents: [
        EditMyWord
    ],

    providers: [
        { provide: APP_BASE_HREF, useValue: '/#' },
        appRoutingProviders,
        HttpService
    ]
})
export class AppModule { }