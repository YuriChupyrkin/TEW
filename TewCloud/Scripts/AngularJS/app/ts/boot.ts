///<reference path="./../../typings/main.d.ts"/>
import { NgModule } from '@angular/core';
//import { RouterModule }   from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app';
import { MainMenu } from './mainMenu';
import { appRoutingProviders, routing } from './app.routes';
import { APP_BASE_HREF } from '@angular/common';

@NgModule({
    imports: [BrowserModule, routing ],

    declarations: [AppComponent, MainMenu],
    bootstrap: [AppComponent, MainMenu],
    providers: [{ provide: APP_BASE_HREF, useValue: '/' }, appRoutingProviders]
})
export class AppModule { }