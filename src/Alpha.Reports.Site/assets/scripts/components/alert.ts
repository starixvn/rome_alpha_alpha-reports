"use strict";

class Alert {
    private $: JQueryStatic;
    private divAlert: JQuery;
    private divAlertMessage: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
        this.cacheDOMElements();
    }

    private cacheDOMElements() {
        this.divAlert = this.$("#alert-message");
        this.divAlertMessage = this.$("#alert-message-content");
    }

    public showSuccess(message: string) {
        this.divAlertMessage
            .addClass("msg msg-success")
            .html(message);
        this.divAlert.removeClass("hidden");
    }

    public showError(error: string) {
        this.divAlertMessage
            .addClass("msg msg-error")
            .html(error);
        this.divAlert.removeClass("hidden");
    }

    public show(code: number, message: string) {
        this.divAlertMessage
            .addClass("msg " + (code ? "msg-error" : "msg-success"))
            .html(message);
        this.divAlert.removeClass("hidden");
    }

    public hide() {
        this.divAlert.addClass("hidden");
        this.divAlertMessage.removeClass().html("");
    }
}
