import { Routes, RouterModule }   from '@angular/router';
import { ModuleWithProviders }  from '@angular/core';
import { AddWord } from './components/addWord';
import { MyWords } from './components/myWords';
import { PickerTest } from './components/pickerTest';
import { AppComponent } from './components/app';
import { UserStat } from './components/userStat';
import { IrregularVerbs } from './components/irregularVerbs';
import { WriteTest } from './components/writeTest';

const appRoutes: Routes = [
    { path: '', redirectTo: 'user-stat', pathMatch: 'full' },
    { path: 'add-word', component: AddWord },
    { path: 'my-words', component: MyWords },
    { path: 'picker-test', component: PickerTest },
    { path: 'user-stat', component: UserStat },
    { path: 'irregular-verbs', component: IrregularVerbs },
    { path: 'write-test', component: WriteTest }
];

export const appRoutingProviders: any[] = [];
export const routing: ModuleWithProviders
    = RouterModule.forRoot(appRoutes);