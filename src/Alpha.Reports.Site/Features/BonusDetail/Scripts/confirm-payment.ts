"use strict";
interface JQuery {
    datepicker(options: any): JQuery;
}

class ConfirmPayment {
    private $: JQueryStatic;
    private alert: Alert;
    private callback: Function;
    private $confirmPaymentModal: JQuery;
    private $confirmPaymentForm: JQuery;
    private $custIdHidden: JQuery;
    private $transIdHidden: JQuery;
    private $toMemberLabel: JQuery;
    private $amountLabel: JQuery;
    private $refNoLabel: JQuery;
    private $paymentDatePicker: JQuery;

    constructor($: JQueryStatic, alert: Alert, callback: Function) {
        this.$ = $;
        this.alert = alert;
        this.callback = callback;
        this.$confirmPaymentModal = this.$("#report-legend");
    }

    public init() {
        if (this.$confirmPaymentModal.length) {
            this.cacheElements();
            this.bindEvents();
        }
    }

    public cacheElements() {
        this.$confirmPaymentForm = this.$("#form-confirmPayment");
        this.$custIdHidden = this.$confirmPaymentForm.find("#hidden-custId");
        this.$transIdHidden = this.$confirmPaymentForm.find("#hidden-transId");
        this.$toMemberLabel = this.$confirmPaymentForm.find("#lbl-toMember");
        this.$amountLabel = this.$confirmPaymentForm.find("#lbl-amount");
        this.$refNoLabel = this.$confirmPaymentForm.find("#lbl-transId");
        this.$paymentDatePicker = this.$("#payment-date");
        this.$paymentDatePicker.datepicker({
            allowInputToggle: true,
            maxDate: this.$paymentDatePicker.val(),
            debug: true
        });
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
        } else {
            this.alert.show(data.Code, data.Message);
            data.IsOk && setTimeout(() => {
                this.callback();
                this.$confirmPaymentModal.modal("hide");
            }, 1000 * 3);
        }
    }

    public registerIconPaymentDateClick(elements) {
        if (this.$confirmPaymentModal.length) {
            const self = this;
            elements.click(function () {
                self.assignConfirmPaymentFormData(self.$(this));
                self.$confirmPaymentModal.modal({ show: true, backdrop: false });
            });
        }
    }

    public assignConfirmPaymentFormData(iconPayment) {
        this.alert.hide();
        const transId = iconPayment.data("transid");

        this.$custIdHidden.val(iconPayment.data("custid"));
        this.$transIdHidden.val(transId);
        this.$toMemberLabel.text(iconPayment.data("username"));
        this.$amountLabel.text(iconPayment.data("amount"));
        this.$refNoLabel.text(transId);
        this.$paymentDatePicker.data("DateTimePicker").minDate(iconPayment.data("createddate"));
    }
}
