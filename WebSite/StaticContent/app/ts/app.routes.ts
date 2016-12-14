import { Routes, RouterModule }   from '@angular/router';
import { Home } from './home';
import { AddWord } from './addWord';
import { MyWords } from './myWords';
import { PickerTest } from './pickerTest';
import { AppComponent } from './app';
import { UserStat } from './userStat';
import { ModuleWithProviders }  from '@angular/core';

const appRoutes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: Home },
    { path: 'add-word', component: AddWord },
    { path: 'my-words', component: MyWords },
    { path: 'picker-test', component: PickerTest },
    { path: 'user-stat', component: UserStat}
];

export const appRoutingProviders: any[] = [];
export const routing: ModuleWithProviders
    = RouterModule.forRoot(appRoutes);