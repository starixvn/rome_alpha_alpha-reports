/// <reference path="../core.ts" />
"use strict";
var DateRangePickerOption = /** @class */ (function () {
    function DateRangePickerOption(selectorId, reportFormId, onChange, onBeforeQuickChooseChange) {
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (reportFormId === void 0) { reportFormId = "report-form"; }
        if (onChange === void 0) { onChange = null; }
        if (onBeforeQuickChooseChange === void 0) { onBeforeQuickChooseChange = null; }
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
    }
    return DateRangePickerOption;
}());
var DateRangePickerComponent = /** @class */ (function () {
    function DateRangePickerComponent($) {
        var _this = this;
        this.options = new DateRangePickerOption();
        this.quickChooseType = {
            specificDates: 7
        };
        this.isCompleteInitDateRangePicker = false;
        this.render = function (options) {
            if (options === void 0) { options = new DateRangePickerOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var dateRangePicker = _this.$("#" + _this.options.selectorId);
            var currentDate = dateRangePicker.data("currentdate");
            var minDate = dateRangePicker.data("mindate");
            var maxDate = dateRangePicker.data("maxdate");
            _this.$reportForm = _this.$("#" + _this.options.reportFormId);
            dateRangePicker.nexDatePicker({
                datePickerFormat: {
                    dateFormat: alphaReports.format.shortDateWithSlash_DatePicker
                },
                selectedItem: dateRangePicker.data("selecteddaterange"),
                minDate: new Date(minDate),
                maxDate: maxDate === "" ? null : maxDate,
                currentDate: new Date(currentDate),
                onSelectDatePicker: function () {
                    if (_this.options.onChange) {
                        _this.options.onChange();
                    }
                },
                onQuickChooseChange: function (selectValue) {
                    _this.options.onBeforeQuickChooseChange && _this.options.onBeforeQuickChooseChange();
                    if (_this.isCompleteInitDateRangePicker && selectValue !== _this.quickChooseType.specificDates) {
                        _this.$reportForm.submit();
                    }
                }
            });
            _this.isCompleteInitDateRangePicker = true;
            return dateRangePicker;
        };
        this.$ = $;
    }
    return DateRangePickerComponent;
}());
//# sourceMappingURL=daterangepicker.js.map
"use strict";
var Alert = /** @class */ (function () {
    function Alert($) {
        this.$ = $;
        this.cacheDOMElements();
    }
    Alert.prototype.cacheDOMElements = function () {
        this.divAlert = this.$("#alert-message");
        this.divAlertMessage = this.$("#alert-message-content");
    };
    Alert.prototype.showSuccess = function (message) {
        this.divAlertMessage
            .addClass("msg msg-success")
            .html(message);
        this.divAlert.removeClass("hidden");
    };
    Alert.prototype.showError = function (error) {
        this.divAlertMessage
            .addClass("msg msg-error")
            .html(error);
        this.divAlert.removeClass("hidden");
    };
    Alert.prototype.show = function (code, message) {
        this.divAlertMessage
            .addClass("msg " + (code ? "msg-error" : "msg-success"))
            .html(message);
        this.divAlert.removeClass("hidden");
    };
    Alert.prototype.hide = function () {
        this.divAlert.addClass("hidden");
        this.divAlertMessage.removeClass().html("");
    };
    return Alert;
}());
//# sourceMappingURL=alert.js.map
"use strict";
var ConfirmPayment = /** @class */ (function () {
    function ConfirmPayment($, topWindow, alert) {
        var _this = this;
        this.confirmPaymentPostSuccess = function (data) {
            if (data.ErrCode) {
                _this.alert.showError(data.ErrMsg);
                _this.$submitButton.prop("disabled", false);
            }
            else {
                _this.alert.show(data.Code, data.Message);
                if (data.IsOk) {
                    _this.topWindow.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
                }
                else {
                    _this.$submitButton.prop("disabled", false);
                }
            }
        };
        this.$ = $;
        this.topWindow = topWindow;
        this.alert = alert;
    }
    ConfirmPayment.prototype.init = function () {
        this.cacheElements();
        this.bindEvents();
    };
    ConfirmPayment.prototype.cacheElements = function () {
        this.$confirmPaymentForm = this.$("#form-confirmPayment");
        this.$paymentDatePicker = this.$("#payment-date");
        this.$paymentDatePicker.datepicker({
            allowInputToggle: true,
            maxDate: new Date(this.$("#hidden-maxPaymentDate").val()),
            minDate: new Date(this.$paymentDatePicker.val())
        });
        this.$submitButton = this.$("#btn-submit");
    };
    ConfirmPayment.prototype.bindEvents = function () {
        var self = this;
        this.$confirmPaymentForm.submit(function (e) {
            e.preventDefault();
            self.confirmPaymentPost();
        });
    };
    ConfirmPayment.prototype.confirmPaymentPost = function () {
        var _this = this;
        this.$.blockUI();
        this.$submitButton.prop("disabled", true);
        this.$.ajax({
            type: this.$confirmPaymentForm.attr("method"),
            url: this.$confirmPaymentForm.attr("action"),
            beforeSend: function (xhr) { return xhr.setRequestHeader("__RequestVerificationToken", _this.$confirmPaymentForm.find('input[name="__RequestVerificationToken"]').val()); },
            data: this.$confirmPaymentForm.serialize(),
            dataType: "json",
            success: this.confirmPaymentPostSuccess,
            complete: this.$.unblockUI
        });
    };
    return ConfirmPayment;
}());
$(function () {
    new ConfirmPayment($, window.top, new Alert($)).init();
});
//# sourceMappingURL=confirm-payment.js.map