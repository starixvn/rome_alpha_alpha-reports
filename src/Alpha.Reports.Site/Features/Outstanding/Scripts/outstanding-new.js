/// <reference path="../../../assets/scripts/core.ts" />
/// <reference path="../../shared/scripts/base-report-new.ts" />
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
var OutstandingViewNew = /** @class */ (function (_super) {
    __extends(OutstandingViewNew, _super);
    function OutstandingViewNew($, productMultiSelectionComponent, linkBuilderComponent, displayOutstandingLevels) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _super.prototype.init.call(_this);
            _this.productMultiSelectionComponent.render();
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
            _this.initDataTable();
        };
        _this.registerEvents = function () {
            _super.prototype.registerEvents.call(_this);
            _this.$reportForm.submit(_this.onSubmitReport);
        };
        _this.initDataTable = function () {
            var colunmSortIndex = 0;
            _this.$.each(_this.displayOutstandingLevels, function (key, value) {
                if (_this.$(_this.$sortPropertyName).val().toLowerCase() === (value.DisplayName + "Outstanding").toLowerCase()) {
                    colunmSortIndex = key + 1;
                    return;
                }
            });
            _super.prototype.initDataTable.call(_this, { order: [[colunmSortIndex, JSON.parse(_this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']], });
        };
        _this.triggerRedrawTable = function () {
            _super.prototype.triggerRedrawTable.call(_this);
            _this.replaceHrefParam("SortPropertyName", _this.getColumnSortName());
            _this.replaceHrefParam("IsDescendingSort", _this.isDescendingSort());
        };
        _this.getColumnSortName = function () {
            var order = _this.$dataTable.order();
            var length = _this.displayOutstandingLevels.length;
            for (var index = 0; index < length; index++) {
                if (order[0][0] - 1 === index) {
                    return _this.displayOutstandingLevels[index].DisplayName + "Outstanding";
                }
            }
            return 'CustName';
        };
        _this.onSubmitReport = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            var columnSortName = _this.getColumnSortName();
            var isDescendingSort = _this.isDescendingSort();
            _this.replaceHrefParam("SortPropertyName", columnSortName);
            _this.replaceHrefParam("IsDescendingSort", isDescendingSort);
            _this.$sortPropertyName.val(columnSortName);
            _this.$isDescendingSort.val(isDescendingSort ? 'True' : 'False');
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.showValidateProductError = function () {
            _this.$buttonProduct.addClass("error");
        };
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.displayOutstandingLevels = displayOutstandingLevels;
        return _this;
    }
    return OutstandingViewNew;
}(BaseReportNew));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var outstandingViewNew = new OutstandingViewNew($, new ProductMultiSelectionComponent($), linkBuilderComponent, displayOutstandingLevels);
    outstandingViewNew.init();
});
//# sourceMappingURL=outstanding-new.js.map