/// <reference path="../core.ts" />
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
var OutstandingView = /** @class */ (function (_super) {
    __extends(OutstandingView, _super);
    function OutstandingView($, sortComponent, productMultiSelectionComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _this.cacheElements();
            _this.sortComponent.render(new SortOption(_this.$tableReport, parseInt(_this.$tableReport.data("totalrecords"), 0) > 1, true, _this.sortComponent.afterSort, { 0: { sorter: "text" } }, _this.$reportForm));
            _this.$tableReport.stickyTableHeaders();
            _this.productMultiSelectionComponent.render();
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
        };
        _this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$sortPropertyName.val(params.SortPropertyName);
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var sortInfo = _this.sortComponent.getSortInfo();
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.showValidateProductError = function () {
            _this.$buttonProduct.addClass("error");
        };
        _this.afterSort = function () {
            var this$ = _this.$;
            var sortedColumn = this$(".sorting-asc:visible");
            var direction = !sortedColumn.length;
            if (sortedColumn.length === 0) {
                sortedColumn = this$(".sorting-desc");
            }
            var sortedColumnName = sortedColumn.data("sortedname");
            _this.replaceHrefParam("SortPropertyName", sortedColumnName);
            _this.replaceHrefParam("IsDescendingSort", direction);
            var isEvenRow = false;
            _this.$tableReport.find("tbody > tr").each(function () {
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
        _this.$ = $;
        _this.sortComponent = sortComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.linkBuilderComponent = linkBuilderComponent;
        return _this;
    }
    OutstandingView.prototype.cacheElements = function () {
        _super.prototype.cacheElements.call(this);
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$pageTitle = this.$(".page-title");
    };
    ;
    OutstandingView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$reportForm.submit(this.onSubmitReport);
    };
    ;
    return OutstandingView;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var outstandingView = new OutstandingView($, new SortComponent($, linkBuilderComponent), new ProductMultiSelectionComponent($), linkBuilderComponent);
    outstandingView.init();
});
//# sourceMappingURL=outstanding.js.map