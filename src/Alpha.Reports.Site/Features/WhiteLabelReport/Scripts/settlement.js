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
var SettlementView = /** @class */ (function (_super) {
    __extends(SettlementView, _super);
    function SettlementView(monthRangePicker, whiteLabelDropdown, usernameAutoSuggestion, visibleColumns) {
        var _this = _super.call(this) || this;
        _this.monthRangePicker = monthRangePicker;
        _this.whiteLabelDropdown = whiteLabelDropdown;
        _this.usernameAutoSuggestion = usernameAutoSuggestion;
        _this.visibleColumns = visibleColumns;
        // ser default sortIndicator
        _this.sortColumnIndex = undefined;
        return _this;
    }
    SettlementView.prototype.init = function () {
        this.$formReport = $('#report-form');
        this.$tblReportContainer = $('#tbl-report-container');
        this.initComponents();
        this.bindEventsAfterRenderReportData();
        this.$tblReportContainer.removeAttr('style');
    };
    SettlementView.prototype.initComponents = function () {
        var _this = this;
        this.initShowHideFilter(function (isHideFilter) { return _this.$formReport.toggle(!isHideFilter); });
        this.monthRangePicker.render(new MonthRangePickerOption(undefined, undefined, undefined, function () { return _this.getReportData(); }));
        if (Array.isArray(whiteLabels)) {
            var maxWhiteLabelLength_1 = 0;
            var listData = whiteLabels.map(function (wl) {
                if (wl.Name.length > maxWhiteLabelLength_1) {
                    maxWhiteLabelLength_1 = wl.Name.length;
                }
                return {
                    text: wl.Name,
                    value: wl.Id,
                    checked: true
                };
            });
            maxWhiteLabelLength_1 *= 11;
            this.whiteLabelDropdown.render('dropdown-whitelabel', {
                listData: listData,
                onLeave: function () { return _this.getReportData(); },
                minWidth: maxWhiteLabelLength_1 > 220 ? maxWhiteLabelLength_1 : 220
            });
        }
        this.usernameAutoSuggestion.init(new UsernameAutoSuggestionOption($("#hiddenUsernameAutoSuggestionUrl").val(), false, false, function () { return _this.getReportData(); }));
        this.visibleColumns.init();
        var $iconExcel = $('#icon-excel').tooltip();
        var excelUrl = $iconExcel.attr('href');
        $iconExcel.click(function () {
            $iconExcel.attr('href', excelUrl + "?" + $.param(_this.getFormData()));
            setTimeout(function () {
                $iconExcel.attr('href', 'javascript:;');
            }, 1000);
        }).attr('href', 'javascript:;');
    };
    SettlementView.prototype.getReportData = function () {
        var _this = this;
        $.blockUI();
        var queryString = $.param(this.getFormData());
        $.get(this.$formReport.attr('action') + "?" + queryString, function (html) {
            _this.$tblReportContainer.html(html);
            _this.bindEventsAfterRenderReportData();
            _this.updateCurrentUrl(queryString);
        }).fail(alphaReports.handleAjaxFail).always($.unblockUI);
    };
    SettlementView.prototype.getFormData = function () {
        var formData = this.monthRangePicker.getDateRangeValues();
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
    SettlementView.prototype.bindEventsAfterRenderReportData = function () {
        this.visibleColumns.showHideColumns();
        var $tblReport = $('#tbl-report');
        this.tableSorter($tblReport);
        this.stickyTableHeaders($tblReport);
    };
    return SettlementView;
}(BaseReportView));
$(function () {
    new SettlementView(new MonthRangePickerComponent($), new MultipleDropdownComponent(), new UsernameAutoSuggestionComponent(), new SettlementColumns(new VisibleColumnDropdownComponent())).init();
});
//# sourceMappingURL=settlement.js.map