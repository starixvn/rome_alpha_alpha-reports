/// <reference path="winlossdetail-view.ts" />

"use strict";
declare var $: any;

class WinLossDetailController {
    currentURL ='';

    init = () => {
        winLossDetailView.init();
    };

    getReport = () => {
        $.blockUI();

        var params = winLossDetailView.getParams();
        $.ajax({
            url: this.currentURL,
            data: params,
            method: 'GET',
            success: (responseHtml) => {
                setTimeout(() => {
                    const newDoc = document.open("text/html", "replace");
                    newDoc.write(responseHtml);
                    newDoc.close();
                }, 100);
            },
            error: () => $.unblockUI()
        });
    };

    pageReport = () => {
        // implement later
    };
};

const winLossDetailController = new WinLossDetailController();

$(function () {
    winLossDetailController.init();
});
