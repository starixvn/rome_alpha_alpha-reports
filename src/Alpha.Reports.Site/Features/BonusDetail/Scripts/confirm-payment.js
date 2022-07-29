var ConfirmPayment = /** @class */ (function () {
    function ConfirmPayment($, alert, reloadFormCallback, $popupConfirmPayment) {
        var _this = this;
        this.confirmPaymentPostSuccess = function (data) {
            if (data.ErrCode) {
                _this.alert.showError(data.ErrMsg);
                _this.$buttonConfirmPayment.prop("disabled", false);
            }
            else {
                _this.alert.show(data.Code, data.Message);
                if (data.IsOk) {
                    setTimeout(function () {
                        _this.$popupConfirmPayment.modal("hide");
                        _this.reloadFormCallback();
                    }, 3000);
                }
                else {
                    _this.$buttonConfirmPayment.prop("disabled", false);
                }
            }
        };
        this.$ = $;
        this.alert = alert;
        this.reloadFormCallback = reloadFormCallback;
        this.$popupConfirmPayment = $popupConfirmPayment;
    }
    ConfirmPayment.prototype.init = function () {
        this.cacheElements();
        this.bindEvents();
    };
    ConfirmPayment.prototype.cacheElements = function () {
        this.$formConfirmPayment = this.$popupConfirmPayment.find("#form-confirmPayment");
        this.$hiddenCustId = this.$formConfirmPayment.find("#hidden-custId");
        this.$hiddenTransId = this.$formConfirmPayment.find("#hidden-transId");
        this.$paymentDatePicker = this.$formConfirmPayment.find("#payment-date");
        this.$paymentDatePicker.datepicker({
            allowInputToggle: true,
            maxDate: this.$paymentDatePicker.val(),
            widgetPositioning: {
                horizontal: "left",
                vertical: "bottom"
            }
        });
        this.$labelToMember = this.$formConfirmPayment.find("#lbl-toMember");
        this.$labelAmount = this.$formConfirmPayment.find("#lbl-amount");
        this.$labelBonusDetailRefNo = this.$formConfirmPayment.find("#lbl-bonusDetailRefNo");
        this.$buttonConfirmPayment = this.$formConfirmPayment.find("#btn-confirmPayment");
    };
    ConfirmPayment.prototype.bindEvents = function () {
        var self = this;
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
    };
    ConfirmPayment.prototype.confirmPaymentPost = function () {
        var _this = this;
        this.$.blockUI();
        this.$buttonConfirmPayment.prop("disabled", true);
        this.$.ajax({
            type: this.$formConfirmPayment.attr("method"),
            url: this.$formConfirmPayment.attr("action"),
            beforeSend: function (xhr) { return xhr.setRequestHeader("__RequestVerificationToken", _this.$formConfirmPayment.find('input[name="__RequestVerificationToken"]').val()); },
            data: this.$formConfirmPayment.serialize(),
            dataType: "json",
            success: this.confirmPaymentPostSuccess,
            complete: this.$.unblockUI
        });
    };
    ConfirmPayment.prototype.assignConfirmPaymentFormData = function (iconPayment) {
        this.alert.hide();
        var transId = iconPayment.data("transid");
        var paymentDate = iconPayment.data("createddate");
        this.$hiddenCustId.val(iconPayment.data("custid"));
        this.$hiddenTransId.val(transId);
        this.$paymentDatePicker.val(paymentDate);
        this.$paymentDatePicker.data("DateTimePicker").minDate(paymentDate);
        this.$labelToMember.text(iconPayment.data("username"));
        this.$labelAmount.text(iconPayment.data("amount"));
        this.$labelBonusDetailRefNo.text(transId);
    };
    return ConfirmPayment;
}());
//# sourceMappingURL=confirm-payment.js.map