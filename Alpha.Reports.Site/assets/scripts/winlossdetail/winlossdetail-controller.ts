/// <reference path="winlossdetail-view.ts" />

"use strict";
declare var $: any;

class WinLossDetailController {
    init = () => {
        winLossDetailView.init();
    };

    pageReport = () => {
        // implement later
    };
};

const winLossDetailController = new WinLossDetailController();

$(function () {
    winLossDetailController.init();
});
