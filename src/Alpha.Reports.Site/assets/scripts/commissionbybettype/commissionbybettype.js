/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />
"use strict";
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
var CommissionByBettype = /** @class */ (function (_super) {
    __extends(CommissionByBettype, _super);
    function CommissionByBettype($, levelTypeDropdown, dateRangePicker, sorter, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _this.cacheDOMElements();
            _this.registerEvents();
            _this.levelTypeDropdown.render(new SingleSelectionDropdownOption("dropdown-level-type"));
            _this.dateRangePicker.render();
            var sortHeaders = {
                0: { sorter: "text" }
            };
            if (_this.levelTypeDropdown.$dropdown.length) {
                sortHeaders[1] = { sorter: "text" };
            }
            _this.sorter.render(new SortOption(_this.$tableReport, parseInt(_this.$tableReport.data("totalrecords"), 0) > 1, true, _this.sorter.afterSort, sortHeaders));
            _this.$tableReport.stickyTableHeaders();
            _this.initHelp();
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheElements.call(_this);
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$isDescendingSort = _this.$("#IsDescendingSort");
            _this.$sortPropertyName = _this.$("#SortPropertyName");
        };
        _this.onExportExcelEvent = function () {
            _this.$exportExcelForm.submit();
        };
        _this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            var params = _this.getParams();
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$sortPropertyName.val(params.SortPropertyName);
        };
        _this.getParams = function () {
            var sortInfo = _this.sorter.getSortInfo();
            return {
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.levelTypeDropdown = levelTypeDropdown;
        _this.dateRangePicker = dateRangePicker;
        _this.sorter = sorter;
        return _this;
    }
    CommissionByBettype.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$reportForm.submit(this.onSubmitReport);
    };
    return CommissionByBettype;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var commissionByBettype = new CommissionByBettype($, new SingleSelectionDropdownComponent($), new DateRangePickerComponent($), new SortComponent($, linkBuilderComponent), linkBuilderComponent);
    commissionByBettype.init();
});
//# sourceMappingURL=commissionbybettype.js.map