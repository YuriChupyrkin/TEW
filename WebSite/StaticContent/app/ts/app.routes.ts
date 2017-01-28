import { Routes, RouterModule }   from '@angular/router';
import { ModuleWithProviders }  from '@angular/core';
import { Home } from './components/home';
import { AddWord } from './components/addWord';
import { MyWords } from './components/myWords';
import { PickerTest } from './components/pickerTest';
import { AppComponent } from './components/app';
import { UserStat } from './components/userStat';
import { IrregularVerbs } from './components/irregularVerbs';

const appRoutes: Routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: Home },
    { path: 'add-word', component: AddWord },
    { path: 'my-words', component: MyWords },
    { path: 'picker-test', component: PickerTest },
    { path: 'user-stat', component: UserStat },
    { path: 'irregular-verbs', component: IrregularVerbs }
];

export const appRoutingProviders: any[] = [];
export const routing: ModuleWithProviders
    = RouterModule.forRoot(appRoutes);