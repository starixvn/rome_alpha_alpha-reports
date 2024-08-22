'use strict';
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DailyWinLossView = /** @class */ (function (_super) {
    __extends(DailyWinLossView, _super);
    function DailyWinLossView(dateRangePicker, whiteLabelDropdown, usernameAutoSuggestion) {
        var _this = _super.call(this) || this;
        _this.dateRangePicker = dateRangePicker;
        _this.whiteLabelDropdown = whiteLabelDropdown;
        _this.usernameAutoSuggestion = usernameAutoSuggestion;
        // ser default sortIndicator
        _this.sortColumnIndex = undefined;
        return _this;
    }
    DailyWinLossView.prototype.init = function () {
        this.cacheElements();
        this.initComponents();
        this.bindEventsAfterRenderWinLoss();
        this.$tblReportContainer.removeAttr('style');
    };
    DailyWinLossView.prototype.cacheElements = function () {
        this.$formReport = $('#report-form');
        this.$tblReportContainer = $('#tbl-report-container');
    };
    DailyWinLossView.prototype.initComponents = function () {
        var _this = this;
        var $iconExcel = $('#icon-excel');
        this.initShowHideFilter(function (isHideFilter) {
            if (isHideFilter) {
                _this.$formReport.hide();
                $iconExcel.hide();
            }
            else {
                _this.$formReport.show();
                $iconExcel.show();
            }
        });
        this.dateRangePicker.render({
            onClosed: function () { return _this.getReportData(); }
        });
        if (Array.isArray(whiteLabels)) {
            this.whiteLabelDropdown.render('dropdown-whitelabel', {
                listData: whiteLabels.map(function (wl) {
                    return {
                        text: wl.Name,
                        value: wl.Id,
                        checked: true
                    };
                }),
                onLeave: function () { return _this.getReportData(); }
            });
        }
        this.usernameAutoSuggestion.init(new UsernameAutoSuggestionOption($("#hiddenUsernameAutoSuggestionUrl").val(), false, false, function () { return _this.getReportData(); }));
        var excelUrl = $iconExcel.attr('href');
        $iconExcel.off().tooltip();
        $iconExcel.click(function () {
            $iconExcel.attr('href', excelUrl + "?" + $.param(_this.getFormData()));
            setTimeout(function () {
                $iconExcel.attr('href', 'javascript:;');
            }, 1000);
        }).attr('href', 'javascript:;');
    };
    DailyWinLossView.prototype.getReportData = function () {
        var _this = this;
        $.blockUI();
        var queryString = $.param(this.getFormData());
        $.get(this.$formReport.attr('action') + "?" + queryString, function (html) {
            _this.$tblReportContainer.html(html);
            _this.bindEventsAfterRenderWinLoss();
            _this.updateCurrentUrl(queryString);
        }).fail(alphaReports.handleAjaxFail).always($.unblockUI);
    };
    DailyWinLossView.prototype.getFormData = function () {
        var formData = this.dateRangePicker.getDateRangeValues();
        var whiteLabels = this.whiteLabelDropdown.getCheckedValues();
        if (whiteLabels) {
            formData.whiteLabels = whiteLabels;
        }
        var username = this.usernameAutoSuggestion.getUsername();
        if (username) {
            formData.username = username;
        }
        return formData;
    };
    DailyWinLossView.prototype.bindEventsAfterRenderWinLoss = function () {
        var $tblReport = $('#tbl-report');
        this.tableSorter($tblReport);
        this.stickyTableHeaders($tblReport);
    };
    return DailyWinLossView;
}(BaseReportView));
$(function () {
    new DailyWinLossView(new DateRangePickerJQueryComponent(), new MultipleDropdownComponent(), new UsernameAutoSuggestionComponent()).init();
});
//# sourceMappingURL=daily-winloss.js.map