import { Routes, RouterModule }   from '@angular/router';
import { Home } from './home';
import { MyWords } from './myWords';
import { AppComponent } from './app';
import { ModuleWithProviders }  from '@angular/core';

const appRoutes: Routes = [
    { path: '', component: Home },
    { path: 'my-words', component: MyWords }
];

export const appRoutingProviders: any[] = [];
export const routing: ModuleWithProviders
    = RouterModule.forRoot(appRoutes);