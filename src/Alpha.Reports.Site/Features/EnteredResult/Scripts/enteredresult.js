"use strict";
var EnterResultView = /** @class */ (function () {
    function EnterResultView($, dateRangePickerComponent, sortComponent) {
        var _this = this;
        this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.dateRangePickerComponent.render();
            _this.cacheDOMElements();
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort, true, function () { }, { 0: { sorter: "text" } }));
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
            _this.$.unblockUI();
        };
        this.checkReady = function () {
            return _this.$("#IsHistoricReport").length > 0;
        };
        this.cacheDOMElements = function () {
            _this.$pageTitle = _this.$(".page-title ");
            _this.$tableReport = _this.$("#tbl-enteredresult-report");
            _this.$reportForm = _this.$("#report-form");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$historicReport = _this.$("#historic-report");
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$filterIcon = _this.$(".icon-filter");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$submitButton = _this.$(".btn-submit");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
        };
        this.registerEvents = function () {
            _this.$historicReport.on("click", _this.onSubmitHistoricReportEvent);
            _this.$filterIcon.on("click", _this.onFilterEvent);
            _this.$excelIcon.on("click", _this.onExportExcelEvent);
            _this.$reportForm.submit(_this.onSubmitNormalReportEvent);
        };
        this.onFilterEvent = function () {
            var currentHideState = _this.$reportForm.hasClass("hide");
            currentHideState ? _this.showFilter() : _this.hideFilter();
        };
        this.showFilter = function () {
            _this.$pageTitle.removeClass("hide-filter");
            _this.$reportForm.removeClass("hide");
        };
        this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        this.onExportExcelEvent = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onSubmitNormalReportEvent = function () {
            _this.setHiddenValue();
        };
        this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val("");
            _this.$toDate.val("");
            _this.$isHistoricReport.val("true");
            _this.setHiddenValue();
            _this.$reportForm.trigger("submit");
        };
        this.setHiddenValue = function () {
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
            _this.$.blockUI();
        };
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }
    return EnterResultView;
}());
$(function () {
    var enterResultView = new EnterResultView($, new DateRangePickerComponent($), new SortComponent($, new LinkBuilderComponent($)));
    enterResultView.init();
});
//# sourceMappingURL=enteredresult.js.map