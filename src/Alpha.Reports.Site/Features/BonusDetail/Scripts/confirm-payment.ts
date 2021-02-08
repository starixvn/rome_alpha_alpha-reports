"use strict";
interface JQuery {
    datepicker(options: any): JQuery;
}

class ConfirmPayment {
    private $: JQueryStatic;
    private topWindow: any;
    private alert: Alert;
    private $confirmPaymentForm: JQuery;
    private $paymentDatePicker: JQuery;
    private $submitButton: JQuery;

    constructor($: JQueryStatic, topWindow: any, alert: Alert) {
        this.$ = $;
        this.topWindow = topWindow;
        this.alert = alert;
    }

    public init() {
        this.cacheElements();
        this.bindEvents();
    }

    public cacheElements() {
        this.$confirmPaymentForm = this.$("#form-confirmPayment");
        this.$paymentDatePicker = this.$("#payment-date");
        this.$paymentDatePicker.datepicker({
            allowInputToggle: true,
            maxDate: new Date(this.$("#hidden-maxPaymentDate").val()),
            minDate: new Date(this.$paymentDatePicker.val()),
            debug: true
        });
        this.$submitButton = this.$("#btn-submit");
    }

    public bindEvents() {
        const self = this;
        this.$confirmPaymentForm.submit(function (e) {
            e.preventDefault();
            self.confirmPaymentPost();
        });
    }

    public confirmPaymentPost() {
        this.$.blockUI();
        this.$submitButton.prop("disabled", true);
        this.$.ajax({
            type: this.$confirmPaymentForm.attr("method"),
            url: this.$confirmPaymentForm.attr("action"),
            beforeSend: xhr => xhr.setRequestHeader("__RequestVerificationToken", this.$confirmPaymentForm.find('input[name="__RequestVerificationToken"]').val()),
            data: this.$confirmPaymentForm.serialize(),
            dataType: "json",
            success: this.confirmPaymentPostSuccess,
            complete: this.$.unblockUI
        });
    }

    confirmPaymentPostSuccess = (data) => {
        if (data.ErrCode) {
            this.alert.showError(data.ErrMsg);
            this.$submitButton.prop("disabled", false);
        } else {
            this.alert.show(data.Code, data.Message);
            if (data.IsOk) {
                this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
            } else {
                this.$submitButton.prop("disabled", false);
            }
        }
    }
}

$(function () {
    new ConfirmPayment($, window.top, new Alert($)).init();
});