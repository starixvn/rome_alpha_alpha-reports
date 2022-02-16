"use strict";
(function ($) {
    $.fn.tinyDraggable = function (options) {
        var settings = $.extend({ handle: 0, exclude: 0 }, options);
        return this.each(function () {
            var dx, dy, el = $(this), handle = settings.handle ? $(settings.handle, el) : el;
            handle.on({
                mousedown: function (e) {
                    //if (settings.exclude && ~$.inArray(e.target, $(settings.exclude, el))) return;
                    e.preventDefault();
                    var os = el.offset(); dx = e.pageX - os.left, dy = e.pageY - os.top;
                    $(document).on('mousemove.drag', function (e) { el.offset({ top: e.pageY - dy, left: e.pageX - dx }); });
                },
                mouseup: function (e) { $(document).off('mousemove.drag'); }
            });
        });
    }
}(jQuery));


interface JQuery {
    datepicker(options: any): JQuery;
}

class ConfirmPayment {
    private $: JQueryStatic;
    private alert: Alert;
    private reloadFormCallback: Function;
    private $popupConfirmPayment: JQuery;
    private $formConfirmPayment: JQuery;
    private $hiddenCustId: JQuery;
    private $hiddenTransId: JQuery;
    private $paymentDatePicker: JQuery;
    private $labelToMember: JQuery;
    private $labelAmount: JQuery;
    private $labelBonusDetailRefNo: JQuery;
    private $buttonConfirmPayment: JQuery;

    constructor($: JQueryStatic, alert: Alert, reloadFormCallback: Function, $popupConfirmPayment: JQuery) {
        this.$ = $;
        this.alert = alert;
        this.reloadFormCallback = reloadFormCallback;
        this.$popupConfirmPayment = $popupConfirmPayment;
    }

    public init() {
        this.cacheElements();
        this.bindEvents();
    }

    private cacheElements() {
        this.$formConfirmPayment = this.$popupConfirmPayment.find("#form-confirmPayment");
        this.$hiddenCustId = this.$formConfirmPayment.find("#hidden-custId");
        this.$hiddenTransId = this.$formConfirmPayment.find("#hidden-transId");
        this.$paymentDatePicker = this.$formConfirmPayment.find("#payment-date");
        this.$paymentDatePicker.datepicker({
            allowInputToggle: true,
            maxDate: this.$paymentDatePicker.val(),
            widgetPositioning: {
                vertical: "bottom"
            }
        });
        this.$labelToMember = this.$formConfirmPayment.find("#lbl-toMember");
        this.$labelAmount = this.$formConfirmPayment.find("#lbl-amount");
        this.$labelBonusDetailRefNo = this.$formConfirmPayment.find("#lbl-bonusDetailRefNo");
        this.$buttonConfirmPayment = this.$formConfirmPayment.find("#btn-confirmPayment");
    }

    private bindEvents() {
        const self = this;
        this.$(".icon-payment").click(function () {
            self.assignConfirmPaymentFormData(self.$(this));
            self.$popupConfirmPayment.modal({ show: true, backdrop: false });
        });

        this.$formConfirmPayment.submit(function (e) {
            e.preventDefault();
            self.confirmPaymentPost();
        });

        this.$popupConfirmPayment.draggable({
            handle: ".modal-header"
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
                setTimeout(() => {
                    this.$popupConfirmPayment.modal("hide");
                    this.reloadFormCallback();
                }, 3000);
            } else {
                this.$buttonConfirmPayment.prop("disabled", false);
            }
        }
    }

    private assignConfirmPaymentFormData(iconPayment: JQuery) {
        this.alert.hide();
        const transId = iconPayment.data("transid");
        const paymentDate = iconPayment.data("createddate");

        this.$hiddenCustId.val(iconPayment.data("custid"));
        this.$hiddenTransId.val(transId);
        this.$paymentDatePicker.val(paymentDate);
        this.$paymentDatePicker.data("DateTimePicker").minDate(paymentDate);
        this.$labelToMember.text(iconPayment.data("username"));
        this.$labelAmount.text(iconPayment.data("amount"));
        this.$labelBonusDetailRefNo.text(transId);
    }
}
