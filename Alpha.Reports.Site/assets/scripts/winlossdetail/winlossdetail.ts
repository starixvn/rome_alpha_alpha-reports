/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/netturnover-tooltip.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class WinLossDetailView {
    $: any;
    $pageTitle: any;
    $tableReport: any;

    $isHistoricReport: any;
    $multiSelectButton: any;

    $drilldownLinks: any;
    $hideFilter: any;
    $isStackMode: any;
    $isDescendingSort: any;
    $sortPropertyName: any;
    $reportForm: any;
    $userSelectedDateRangeOption: any;
    $netTurnoverTooltip: any;
    $reportSwitch: any;
    $fromDate: any;
    $toDate: any;
    $exportExcelForm: any;

    $custId: any;
    $productIds: any;
    $pageIndex: any;
    $pageSize: any;

    isFirstSwitchMode = true;
    isHistoricReport = false;
    isSwitchView = false;
    levels = ["member", "agent", "master", "super"];

    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    sortComponent: SortComponent;

    constructor(
        $: any,
        dateRangePickerComponent: DateRangePickerComponent,
        pageComponent: PagerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pageComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.sortComponent = sortComponent;
    }

    init = () => {
        var self = this;

        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === "true";
        this.isHistoricReport
            ? monthPickerComponent.render()
            : this.dateRangePickerComponent.render();
        this.productMultiSelectionComponent.render();
        this.cacheDOMElements();

        if (this.$hideFilter.val() === "True") {
            this.hideFilter();
        }

        this.$netTurnoverTooltip = netTurnoverTooltipComponent.render();
        this.pagerComponent.render();

        var hasAllDataOnClient = self.pagerComponent.hasAllDataOnClient();
        self.bindTableSorter();

        var canSort = parseInt(this.$tableReport.data("totalrecords"), 0) > 1;
        this.sortComponent.render({
            sortContainer: this.$tableReport,
            canSort: canSort,
            hasAllDataOnClient: hasAllDataOnClient,
            afterSortFunction: function () {
                if (hasAllDataOnClient) {
                    self.replaceDrilldownParam("SortPropertyName", self.$tableReport.data("sortedcolumn"));
                    self.replaceDrilldownParam("IsDescendingSort", self.$tableReport.data("descendingsort") === "1");
                } else {
                    self.$reportForm.trigger("submit");
                }
            }
        });

        if (!canSort) {
            this.$(".sorting").removeClass("sorting");
        }

        this.$tableReport.stickyTableHeaders();

        this.registerEvents();
    };
    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$drilldownLinks = this.$(".downline-link");
        this.$hideFilter = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$multiSelectButton = this.$(".multiselect.btn");
        this.$reportSwitch = this.$("#report-switch");

        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");

        this.$custId = this.$("#CustId");
        this.$productIds = this.$("#product-ids");
        this.$pageIndex = this.$("#PageIndex");
        this.$pageSize = this.$("#PageSize");
    };
    registerEvents = () => {
        var self = this;

        this.$reportSwitch.on("click", function () {
            self.$fromDate.val("");
            self.$toDate.val("");
            self.isSwitchView = true;
            self.isHistoricReport = !self.$reportSwitch.data("is-historic-report");
            self.$reportForm.trigger("submit");
        });

        this.$(".icon-filter").on("click", function () {
            var filter = self.$reportForm;
            var currentHideState = filter.is(":hidden");

            if (currentHideState) {
                self.$pageTitle.removeClass("hide-filter");
                filter.slideDown("fast", undefined, function () {
                    netTurnoverTooltipComponent.positionNetTurnoverTooltip(self.$netTurnoverTooltip);
                });
            } else {
                self.hideFilter();
            }

            self.replaceDrilldownParam("IsFilterVisible", !currentHideState);
        });

        this.$(".icon-switchmode").on("click", function () {
            var tableReport = self.$tableReport;
            var currentStackModeState = tableReport.hasClass("stack-mode");

            if (currentStackModeState) {
                self.$pageTitle.removeClass("stack-mode");
                tableReport.removeClass("stack-mode");
            } else {
                self.$pageTitle.addClass("stack-mode");
                tableReport.addClass("stack-mode");
            }

            self.replaceDrilldownParam("IsStackMode", !currentStackModeState);

            if (self.isFirstSwitchMode) {
                if (currentStackModeState) {
                    alphaReports.splitCell("col-turnover");

                    self.$.each(self.levels, function (index: number) {
                        var level = self.levels[index];
                        alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                    });
                } else {
                    alphaReports.combineConsecutiveCells(self.$("td.col-turnover"), 3, "text-right col-turnover-combined");

                    self.$.each(self.levels, function (index: number) {
                        var level = self.levels[index];
                        var cellCss = "text-right col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                        alphaReports.combineConsecutiveCells(self.$("td.col-" + level + "-winloss"), 3, cellCss);
                    });
                }

                self.isFirstSwitchMode = false;
            }

            self.bindTableSorter();
        });

        this.$reportForm.submit(this.onSubmitReportEvent);

        this.$("#icon-excel").on("click", function () {
            self.$exportExcelForm.submit();
            self.$.unblockUI();
        });

        this.$(".icon-back").on("click", function () {
            window.history.back();
        });
    };
    hideFilter = () => {
        var self = this;

        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.slideUp("fast", undefined, function () {
            netTurnoverTooltipComponent.positionNetTurnoverTooltip(self.$netTurnoverTooltip);
        });
    };
    getParams = () => {
        var selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        var sortInfo = this.sortComponent.getSortInfo();
        var tableReport = this.$tableReport;

        return {
            CustId: tableReport.data("custid"),
            IsAnyProductSelected: selectedProducts.length > 0,
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: this.isHistoricReport,
            HideFilter: this.$reportForm.is(":hidden"),
        };
    };
    showValidateProductError = () => {
        this.$multiSelectButton.addClass("error");
    };
    clearError = () => {
        this.$multiSelectButton.removeClass("error");
    };
    replaceDrilldownParam = (paramName: string, paramValue: any) => {
        var self = this;
        this.$drilldownLinks.each(function () {
            var queryString = self.$(this).attr("href");
            var newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
            self.$(this).attr("href", newQueryString);
        });
    };
    onSubmitReportEvent = (e: any) => {
        this.$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$hideFilter.val(alphaReports.screenSmallThan768() ? true : params.HideFilter);
        this.$isDescendingSort.val(params.IsDescendingSort);
        this.$isHistoricReport.val(params.IsHistoricReport);
        this.$isStackMode.val(params.IsStackMode);
        this.$sortPropertyName.val(params.SortPropertyName);

    };
    bindTableSorter = () => {
        if (!this.pagerComponent.hasAllDataOnClient()) {
            return;
        }

        var self = this;

        var headers = {
            0: { sorter: "text" }
        };

        var disabledSortColumns = self.$tableReport.find("th[data-disabledsort='true']");
        disabledSortColumns.each(function () {
            headers[this.cellIndex] = { sorter: false };
        });

        var sortedColumnHeader = self.$tableReport.find("[data-sortedname='" + self.getSortedColumnName() + "']" +
            (self.$tableReport.hasClass("stack-mode") ? ":not([class*='hidden-stack-mode'])" : ""))[0];
        var isDescendingSort = self.$tableReport.data("descendingsort") === "1" || self.$tableReport.data("descendingsort").toString().toLowerCase() === "true";
        this.$tableReport.tablesorter(
            {
                defaultParser: 1,
                headers: headers,
                sortList: [[self.$(sortedColumnHeader).data("colindex"), isDescendingSort ? 1 : 0]],
                textExtraction: function (node: any) {
                    var $node = self.$(node);

                    if ($node.hasClass("combinedcell")) {
                        return $node.find("ul>li:first-child").html();
                    }

                    if ($node.data("islink")) {
                        return $node.find("a").html();
                    }

                    return $node.html();
                }
            });
    };

    getSortedColumnName = () => {
        var sortedColumn = this.$tableReport.data("sortedcolumn");
        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        if (sortedColumn == "NetTurnover" || sortedColumn == "GrossComm") {
            return "Turnover";
        }

        if (sortedColumn == "MemberNetWinLoss.Comm" || sortedColumn == "MemberNetWinLoss.Total") {
            return "MemberNetWinLoss.WinLoss";
        }

        if (sortedColumn == "AgentNetWinLoss.Comm" || sortedColumn == "AgentNetWinLoss.Total") {
            return "AgentNetWinLoss.WinLoss";
        }

        if (sortedColumn == "MasterNetWinLoss.Comm" || sortedColumn == "MasterNetWinLoss.Total") {
            return "MasterNetWinLoss.WinLoss";
        }

        if (sortedColumn == "SuperNetWinLoss.Comm" || sortedColumn == "SuperNetWinLoss.Total") {
            return "SuperNetWinLoss.WinLoss";
        }

        return sortedColumn;
    };
}

const winLossDetailView = new WinLossDetailView(
    $,
    new DateRangePickerComponent($),
    new PagerComponent($),
    new ProductMultiSelectionComponent($),
    new SortComponent($));

$(function () {
    winLossDetailView.init();
});
