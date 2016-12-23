/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />
"use strict";
var EnterResultView = (function () {
    function EnterResultView($, dateRangePickerComponent, sortComponent) {
        var _this = this;
        this.isSwitchView = false;
        this.init = function () {
            var self = _this;
            _this.dateRangePickerComponent.render();
            _this.cacheDOMElements();
            _this.sortComponent.render({
                sortContainer: _this.$tableReport,
                canSort: parseInt(_this.$tableReport.data("totalrecords"), 0) > 1,
                hasAllDataOnClient: true,
                headers: {
                    0: { sorter: "text" }
                },
                afterSortFunction: function () { }
            });
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
        };
        this.cacheDOMElements = function () {
            _this.$pageTitle = _this.$(".page-title ");
            _this.$tableReport = _this.$("#tbl-report");
            _this.$reportForm = _this.$("#report-form");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$reportSwitch = _this.$("#report-switch");
        };
        this.registerEvents = function () {
            var self = _this;
            _this.$reportSwitch.on("click", function () {
                self.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : true);
                self.$isStackMode.val(alphaReports.screenSmallThan768());
                self.$reportForm.trigger("submit");
            });
            _this.$(".icon-filter").on("click", function () {
                var filter = self.$reportForm;
                var currentHideState = filter.is(":hidden");
                if (currentHideState) {
                    self.$pageTitle.removeClass("hide-filter");
                    filter.slideDown("fast", undefined, function () { });
                }
                else {
                    self.hideFilter();
                }
            });
            _this.$("#icon-excel").on("click", function () {
                self.$("#export-excel-form").submit();
                self.$.unblockUI();
            });
            _this.$(".btn-submit").on("click", function () {
                self.$("#IsHistoricReport").val(false);
                self.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : true);
                self.$isStackMode.val(alphaReports.screenSmallThan768());
                self.onSubmitReportEvent();
            });
        };
        this.hideFilter = function () {
            var self = _this;
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.slideUp("fast", undefined, function () { });
        };
        this.onSubmitReportEvent = function () {
            _this.$.blockUI();
        };
        this.bindTableSorter = function () {
            var self = _this;
            var headers = {
                0: { sorter: "text" }
            };
            var disabledSortColumns = self.$tableReport.find("th[data-disabledsort='true']");
            disabledSortColumns.each(function () {
                headers[this.cellIndex] = { sorter: false };
            });
            var sortedColumnHeader = self.$tableReport.find("[data-sortedname='" + self.$tableReport.data("sortedcolumn") + "']")[0];
            _this.$tableReport.tablesorter({
                defaultParser: 1,
                headers: headers,
                sortList: [[self.$(sortedColumnHeader).data("colindex"), 1]],
                textExtraction: function () { }
            });
        };
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }
    return EnterResultView;
}());
var enterResultView = new EnterResultView($, new DateRangePickerComponent($), new SortComponent($));
$(function () {
    enterResultView.init();
});
//# sourceMappingURL=enteredresult.js.map