/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/link-builder.ts" />
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
var PlayForFunView = /** @class */ (function (_super) {
    __extends(PlayForFunView, _super);
    function PlayForFunView($, dateRangePickerComponent, sortComponent, productSelectionComponent, statusSelectionComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _this.$.blockUI();
            _this.cacheElements();
            _this.productSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-product", "UserSelectedProductId"));
            _this.statusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-statuses", "UserSelectedStatusId"));
            _this.dateRangePickerComponent.render();
            _this.initTableReport();
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.$.unblockUI();
        };
        _this.initTableReport = function () {
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort, true, _this.sortComponent.afterSort, _this.getSorterHeader(), _this.$reportForm));
            _this.$tableReport.stickyTableHeaders();
        };
        _this.getParams = function () {
            var sortInfo = _this.sortComponent.getSortInfo();
            return {
                IsFilterVisible: !_this.$reportForm.hasClass("hide"),
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort
            };
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.sortComponent = sortComponent;
        _this.productSelectionComponent = productSelectionComponent;
        _this.statusSelectionComponent = statusSelectionComponent;
        return _this;
    }
    PlayForFunView.prototype.cacheElements = function () {
        _super.prototype.cacheElements.call(this);
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
    };
    PlayForFunView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        var self = this;
        this.$reportForm.submit(function () { return self.onSubmitReport(); });
        this.$("#icon-excel").on("click", this.onExportExcel);
    };
    PlayForFunView.prototype.getSorterHeader = function () {
        return { 0: { sorter: "text" } };
    };
    PlayForFunView.prototype.onSubmitReport = function () {
        this.$.blockUI();
        var params = this.getParams();
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.productSelectionComponent.updateSelectedValue();
        this.statusSelectionComponent.updateSelectedValue();
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$sortPropertyName.val(params.SortPropertyName);
    };
    return PlayForFunView;
}(BaseReport));
//# sourceMappingURL=playforfun.js.map