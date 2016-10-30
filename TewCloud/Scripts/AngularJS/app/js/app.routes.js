"use strict";
var router_1 = require('@angular/router');
var home_1 = require('./home');
var addWord_1 = require('./addWord');
var myWords_1 = require('./myWords');
var appRoutes = [
    { path: '', component: home_1.Home },
    { path: 'add-word', component: addWord_1.AddWord },
    { path: 'my-words', component: myWords_1.MyWords }
];
exports.appRoutingProviders = [];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
