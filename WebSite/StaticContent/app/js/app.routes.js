"use strict";
var router_1 = require("@angular/router");
var home_1 = require("./home");
var addWord_1 = require("./addWord");
var myWords_1 = require("./myWords");
var pickerTest_1 = require("./pickerTest");
var userStat_1 = require("./userStat");
var appRoutes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: home_1.Home },
    { path: 'add-word', component: addWord_1.AddWord },
    { path: 'my-words', component: myWords_1.MyWords },
    { path: 'picker-test', component: pickerTest_1.PickerTest },
    { path: 'user-stat', component: userStat_1.UserStat }
];
exports.appRoutingProviders = [];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
