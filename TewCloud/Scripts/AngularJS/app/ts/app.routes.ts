import { Routes, RouterModule }   from '@angular/router';
import { MainMenu } from './mainMenu';
import { ModuleWithProviders }  from '@angular/core';

const appRoutes: Routes = [
    { path: 'main-menu', component: MainMenu },
    { path: '', redirectTo: '/main-menu', pathMatch: 'full' }
];

export const appRoutingProviders: any[] = [];
export const routing: ModuleWithProviders
    = RouterModule.forRoot(appRoutes);