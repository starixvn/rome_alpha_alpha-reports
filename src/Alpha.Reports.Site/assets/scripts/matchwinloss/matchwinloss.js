/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />
"use strict";
var MatchWinLossView = /** @class */ (function () {
    function MatchWinLossView($, dateRangePickerComponent, sortComponent) {
        var _this = this;
        this.init = function () {
            _this.$.blockUI();
            _this.cacheElements();
            _this.dateRangePickerComponent.render();
            var canSort = _this.$tableReport.data("cansort");
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort && canSort.toLowerCase() === "true", true, _this.sortComponent.afterSort, { 0: { sorter: "text" }, 1: { sorter: "text" } }, _this.$reportForm));
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
            _this.$.unblockUI();
        };
        this.cacheElements = function () {
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$pageTitle = _this.$(".page-title");
            _this.$tableReport = _this.$("#tbl-report");
            _this.$reportForm = _this.$("#report-form");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$isDescendingSort = _this.$("#IsDescendingSort");
            _this.$sortPropertyName = _this.$("#SortPropertyName");
        };
        this.registerEvents = function () {
            _this.$(".icon-filter").on("click", _this.onClickIconFilter);
            _this.$("#icon-excel").on("click", _this.onExportExcel);
            _this.$reportForm.submit(_this.onSubmitReport);
        };
        this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onSubmitReport = function () {
            var this$ = _this.$;
            this$.blockUI();
            var sortInfo = _this.sortComponent.getSortInfo();
            _this.$isDescendingSort.val(String(sortInfo.isDescendingSort));
            _this.$sortPropertyName.val(sortInfo.sortPropertyName);
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : !_this.$reportForm.is(":hidden")));
        };
        this.onClickIconFilter = function () {
            var filter = _this.$reportForm;
            var currentHideState = filter.hasClass("hide");
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$reportForm.addClass("hide");
            }
        };
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }
    return MatchWinLossView;
}());
$(function () {
    var matchWinLossView = new MatchWinLossView($, new DateRangePickerComponent($), new SortComponent($, new LinkBuilderComponent($)));
    matchWinLossView.init();
});
//# sourceMappingURL=matchwinloss.js.map