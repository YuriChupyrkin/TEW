"use strict";
var router_1 = require('@angular/router');
var mainMenu_1 = require('./mainMenu');
var appRoutes = [
    { path: 'main-menu', component: mainMenu_1.MainMenu },
    { path: '', redirectTo: '/main-menu', pathMatch: 'full' }
];
exports.appRoutingProviders = [];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
