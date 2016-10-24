/// <reference path="index-view.ts" />

"use strict";
declare var $: any;

var indexController = {
    init: function () {
        indexView.init();
    },
    pageReport: function () {
        // implement later

    }
};

$(function () {
    indexController.init();
});