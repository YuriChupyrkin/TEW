import { Routes, RouterModule }   from '@angular/router';
import { Home } from './home';
import { AddWord } from './addWord';
import { AppComponent } from './app';
import { ModuleWithProviders }  from '@angular/core';

const appRoutes: Routes = [
    { path: '', component: Home },
    { path: 'add-word', component: AddWord }
];

export const appRoutingProviders: any[] = [];
export const routing: ModuleWithProviders
    = RouterModule.forRoot(appRoutes);