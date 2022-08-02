"use strict";
var MoveStatementHistory = /** @class */ (function () {
    function MoveStatementHistory($, monthPickerComponent, pagerComponent) {
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
        this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onSubmitReport = function () {
            _this.$.blockUI();
            return true;
        };
        this.$ = $;
        this.monthPickerComponent = monthPickerComponent;
        this.pagerComponent = pagerComponent;
    }
    MoveStatementHistory.prototype.init = function () {
        this.$.blockUI();
        this.cacheElements();
        this.initComponents();
        this.bindEvents();
        this.$.unblockUI();
    };
    MoveStatementHistory.prototype.cacheElements = function () {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
    };
    MoveStatementHistory.prototype.initComponents = function () {
        this.monthPickerComponent.render();
        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
        this.$("#tbl-report").stickyTableHeaders();
    };
    MoveStatementHistory.prototype.bindEvents = function () {
        var _this = this;
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#btn-submit").click(function () {
            _this.$("#PageIndex").val("1");
            _this.$("#PageSize").val("100");
        });
        this.$("#icon-excel").click(this.onExportExcel);
    };
    return MoveStatementHistory;
}());
$(function () {
    var moveStatementHistory = new MoveStatementHistory($, new MonthPickerComponent($), new PagerComponent($));
    moveStatementHistory.init();
});
//# sourceMappingURL=move-statement-history.js.map