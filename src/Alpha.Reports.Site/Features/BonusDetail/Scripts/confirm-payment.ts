"use strict";
interface JQuery {
    datepicker(options: any): JQuery;
}

class ConfirmPayment {
    private $: JQueryStatic;
    private alert: Alert;
    private $formConfirmPayment: JQuery;
    private $hiddenCustId: JQuery;
    private $hiddenTransId: JQuery;
    private $paymentDatePicker: JQuery;
    private $labelToMember: JQuery;
    private $labelAmount: JQuery;
    private $labelBonusDetailRefNo: JQuery;
    private $buttonConfirmPayment: JQuery;

    constructor($: JQueryStatic, alert: Alert) {
        this.$ = $;
        this.alert = alert;
    }

    public init() {
        this.cacheElements();
        this.bindEvents();
    }

    private cacheElements() {
        this.$formConfirmPayment = this.$("#form-confirmPayment");
        this.$hiddenCustId = this.$formConfirmPayment.find("#hidden-custId");
        this.$hiddenTransId = this.$formConfirmPayment.find("#hidden-transId");
        this.$paymentDatePicker = this.$formConfirmPayment.find("#payment-date");
        this.$paymentDatePicker.datepicker({
            allowInputToggle: true,
            maxDate: new Date(this.$("#hidden-maxPaymentDate").val())
        });
        this.$labelToMember = this.$formConfirmPayment.find("#lbl-toMember");
        this.$labelAmount = this.$formConfirmPayment.find("#lbl-amount");
        this.$labelBonusDetailRefNo = this.$formConfirmPayment.find("#lbl-bonusDetailRefNo");
        this.$buttonConfirmPayment = this.$formConfirmPayment.find("#btn-confirmPayment");
    }

    private bindEvents() {
        const self = this;
        this.$formConfirmPayment.submit(function (e) {
            e.preventDefault();
            self.confirmPaymentPost();
        });
    }

    private confirmPaymentPost() {
        this.$.blockUI();
        this.$buttonConfirmPayment.prop("disabled", true);
        this.$.ajax({
            type: this.$formConfirmPayment.attr("method"),
            url: this.$formConfirmPayment.attr("action"),
            beforeSend: xhr => xhr.setRequestHeader("__RequestVerificationToken", this.$formConfirmPayment.find('input[name="__RequestVerificationToken"]').val()),
            data: this.$formConfirmPayment.serialize(),
            dataType: "json",
            success: this.confirmPaymentPostSuccess,
            complete: this.$.unblockUI
        });
    }

    confirmPaymentPostSuccess = (data) => {
        if (data.ErrCode) {
            this.alert.showError(data.ErrMsg);
            this.$buttonConfirmPayment.prop("disabled", false);
        } else {
            this.alert.show(data.Code, data.Message);
            if (data.IsOk) {
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            } else {
                this.$buttonConfirmPayment.prop("disabled", false);
            }
        }
    }

    public setUIControlsValue(custId: number, custName: string, transId: number, amount: string, paymentDate: string) {
        this.alert.hide();
        this.$hiddenCustId.val(custId);
        this.$hiddenTransId.val(transId);
        this.$paymentDatePicker.val(paymentDate);
        this.$paymentDatePicker.data("DateTimePicker").minDate(paymentDate);
        this.$labelToMember.text(custName);
        this.$labelAmount.text(amount);
        this.$labelBonusDetailRefNo.text(transId);
    }
}
