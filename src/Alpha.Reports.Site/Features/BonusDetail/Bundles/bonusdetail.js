"use strict";
var AlphaReports = /** @class */ (function () {
    function AlphaReports($, store) {
        var _this = this;
        this.format = {
            monthFormat: "MMMM - yyyy",
            shortDateWithSlash_DatePicker: "MM/DD/YYYY",
            shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
        };
        this.sort = {
            none: 0,
            ascending: 1,
            descending: -1
        };
        this.keyCode = {
            enter: 13
        };
        this.numberDisplayedProductsOnFilter = 1;
        this.maxTimesToShowTooltip = 5;
        this.screenSmallThan768 = function () {
            var currentWindowWidth = screen.width || _this.$(window).width();
            return currentWindowWidth < 768;
        };
        this.screenLandscapeMode = function () { return screen.width > screen.height || _this.$(window).width() > _this.$(window).height(); };
        this.splitCell = function (firstCell, cellClass, lastCellStyle) {
            if (cellClass === void 0) { cellClass = ""; }
            if (lastCellStyle === void 0) { lastCellStyle = "bold"; }
            var this$ = _this.$;
            var combinedCells = _this.$("." + firstCell + "-combined");
            combinedCells.each(function (index) {
                var oldValues = this$(this).find("li");
                var cellsLength = oldValues.length;
                var oldCells = "";
                for (var i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                    oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                        (i === lastCellIndex ? " " + lastCellStyle : "") + "\">" +
                        this$(oldValues[i]).html() +
                        "</td>";
                }
                this$(combinedCells[index]).before(oldCells);
            });
        };
        this.combineConsecutiveCells = function (firstCells, numberOfCombinedCells, combinedCellClass, combinedCellColSpan) {
            if (combinedCellColSpan === void 0) { combinedCellColSpan = 1; }
            if (numberOfCombinedCells <= 1) {
                return;
            }
            var this$ = _this.$;
            firstCells.each(function (index) {
                var newCell = "<td class=\"visible-stack-mode combinedcell " +
                    combinedCellClass + "\" colspan=\"" + combinedCellColSpan + "\">\
                    <ul>";
                var currentCell = this$(firstCells[index]);
                var isLastCell = false;
                for (var i = 0; i < numberOfCombinedCells; i++) {
                    newCell += "<li>" + currentCell.html() + "</li>";
                    if (currentCell.next().length) {
                        currentCell = currentCell.next();
                    }
                    else {
                        isLastCell = true;
                    }
                }
                newCell += "</ul>\
                        </td>";
                if (isLastCell) {
                    currentCell.after(newCell);
                }
                else {
                    currentCell.prev().after(newCell);
                }
            });
        };
        this.localStorage = {
            get: function (key) {
                return _this.store.enabled ? _this.store.get(key) : _this.$.cookie(key);
            },
            set: function (key, value) {
                _this.store.enabled ? _this.store.set(key, value) : _this.$.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                if (_this.isIe()) {
                    cookieExpire = { expires: 365 };
                }
                return cookieExpire;
            }
        };
        this.isIe = function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
        };
        this.replaceQueryStringValue = function (queryString, paramName, paramValue) {
            if (typeof queryString === "undefined"
                || queryString === null) {
                return "";
            }
            var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");
            var newQueryString = regex.test(queryString)
                ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
                : queryString + (queryString.indexOf("?") >= 0 ? "&" : "?") + paramName + "=" + paramValue;
            return newQueryString;
        };
        this.correctEvenRowClass = function ($tableReport) {
            var this$ = _this.$;
            var isEvenRow = false;
            $tableReport.find("tbody > tr").each(function () {
                var $row = this$(this);
                if ($row.hasClass("even-row") && !isEvenRow) {
                    $row.removeClass("even-row");
                }
                else if (!$row.hasClass("even-row") && isEvenRow) {
                    $row.addClass("even-row");
                }
                isEvenRow = !isEvenRow;
            });
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.formatNumber = function (value) { return value.toLocaleString("en-US"); };
        this.formatCurrency = function (value) { return value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); };
        this.$ = $;
        this.store = store;
    }
    AlphaReports.prototype.getFormData = function ($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};
        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });
        return indexed_array;
    };
    return AlphaReports;
}());
var alphaReports = new AlphaReports($, store);
//# sourceMappingURL=core.js.map
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
/// <reference path="../core.ts" />
"use strict";
var PagerOption = /** @class */ (function () {
    function PagerOption(onSelectPage, pagerId, formId, pageIndexId, pageSizeId) {
        if (onSelectPage === void 0) { onSelectPage = null; }
        if (pagerId === void 0) { pagerId = "report-pager"; }
        if (formId === void 0) { formId = "report-form"; }
        if (pageIndexId === void 0) { pageIndexId = "PageIndex"; }
        if (pageSizeId === void 0) { pageSizeId = "PageSize"; }
        this.onSelectPage = onSelectPage;
        this.pagerId = pagerId;
        this.formId = formId;
        this.pageIndexId = pageIndexId;
        this.pageSizeId = pageSizeId;
    }
    return PagerOption;
}());
var PagerComponent = /** @class */ (function () {
    function PagerComponent($) {
        var _this = this;
        this.pageList = [1000, 2000, 3000, 5000];
        this.render = function (options) {
            if (options === void 0) { options = new PagerOption(); }
            var self = _this;
            var pagerOption = _this.$.extend({}, _this.options, options);
            var pager = _this.$("#" + pagerOption.pagerId);
            _this.$reportForm = _this.$("#" + _this.options.formId);
            _this.totalRecords = pager.data("totalrecords");
            _this.pageSize = pager.data("pagesize");
            _this.pageIndex = pager.data("pageindex");
            _this.$pageIndex =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageIndexId + "\" name=\"" + pagerOption.pageIndexId + "\" value=\"" + _this.pageIndex + "\" />");
            _this.$pageSize =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageSizeId + "\" name=\"" + pagerOption.pageSizeId + "\" value=\"" + _this.pageSize + "\" />");
            _this.$reportForm.append(_this.$pageIndex).append(_this.$pageSize);
            if (_this.isShow()) {
                pager.pagination({
                    pageList: _this.pageList,
                    total: _this.totalRecords,
                    pageSize: _this.pageSize,
                    pageNumber: _this.pageIndex,
                    showRefresh: false,
                    onSelectPage: function (pageNumber, pageSize) {
                        self.updateHiddenFields(pageNumber, pageSize);
                        self.onChange();
                    },
                    beforePageText: pager.data("lablepage"),
                    afterPageText: pager.data("labelof") + " {pages}",
                    displayMsg: pager.data("labeldisplayitems")
                });
                pager.removeClass("hide");
            }
            else {
                pager.addClass("hide");
            }
            return pager;
        };
        this.onChange = function () {
            _this.$reportForm.trigger("submit");
        };
        this.updateHiddenFields = function (pageNumber, pageSize) {
            _this.$pageIndex.val(pageNumber);
            _this.$pageSize.val(pageSize);
        };
        this.isShow = function () {
            // total records less than min page size => do not need to show pager
            return _this.pageList[0] <= _this.totalRecords;
        };
        this.hasAllDataOnClient = function () {
            var result = _this.totalRecords <= _this.pageSize;
            return result;
        };
        this.options = new PagerOption(this.onChange);
        this.$ = $;
    }
    return PagerComponent;
}());
//# sourceMappingURL=pager.js.map
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
"use strict";
var BonusDetail = /** @class */ (function () {
    function BonusDetail($, topWindow, dateRangePickerComponent, pagerComponent) {
        var _this = this;
        this.onClickIconFilter = function () {
            var isHide = _this.$reportForm.hasClass("hide");
            _this.$isFilterVisible.val(isHide.toString());
            if (isHide) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$reportForm.addClass("hide");
            }
        };
        this.onSubmitReport = function () {
            _this.$.blockUI();
            return true;
        };
        this.$ = $;
        this.topWindow = topWindow;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }
    BonusDetail.prototype.init = function () {
        this.$.blockUI();
        this.cacheElements();
        this.initComponents();
        this.bindEvents();
        this.$.unblockUI();
    };
    BonusDetail.prototype.cacheElements = function () {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
    };
    BonusDetail.prototype.initComponents = function () {
        var _this = this;
        this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", null, function () { return _this.$("#PageIndex").val("1"); }));
        var dropdownOptions = { numberDisplayed: 1 };
        this.$("#dropdown-statusId").multiselect(dropdownOptions);
        this.$("#dropdown-isComplained").multiselect(dropdownOptions);
        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
        this.$("#tbl-report").stickyTableHeaders();
    };
    BonusDetail.prototype.bindEvents = function () {
        var _this = this;
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#btn-submit").click(function () { return _this.$("#PageIndex").val("1"); });
        var $popupConfirmPayment = this.$("#popup-confirmPayment");
        if ($popupConfirmPayment.length) {
            new ConfirmPayment(this.$, new Alert(this.$), function () { return _this.$reportForm.submit(); }, $popupConfirmPayment).init();
        }
    };
    return BonusDetail;
}());
$(function () {
    new BonusDetail($, window.top, new DateRangePickerComponent($), new PagerComponent($)).init();
});
//# sourceMappingURL=bonus-detail.js.map