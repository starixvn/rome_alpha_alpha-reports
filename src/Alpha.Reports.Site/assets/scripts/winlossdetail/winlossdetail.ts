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
    $isFilterVisible: any;
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
    $iconNormalColumn: any;
    $hasSubmittedParams: any;
    $breadcrumbLinks: any;

    isFirstSwitchMode = true;
    isHistoricReport = false;
    isSwitchView = false;
    hasAllDataOnClient = false;
    levels = ["member", "agent", "master", "super"];

    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;
    monthPickerComponent: MonthPickerComponent;
    netTurnoverTooltipComponent: NetTurnoverTooltipComponent;

    constructor(
        $: any,
        dateRangePickerComponent: DateRangePickerComponent,
        pageComponent: PagerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip,
        monthPickerComponent: MonthPickerComponent,
        netTurnoverTooltipComponent: NetTurnoverTooltipComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pageComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
        this.monthPickerComponent = monthPickerComponent;
        this.netTurnoverTooltipComponent = netTurnoverTooltipComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();
        this.cacheElements();

        this.$isHistoricReport = this.$("#IsHistoricReport");

        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === "true";
        if (this.isHistoricReport) {
            this.$reportForm.removeClass("hide");
            this.monthPickerComponent.render();
            if (this.$isFilterVisible.val().toLowerCase() !== "true") {
                this.$reportForm.addClass("hide");
            }
        } else {
            this.dateRangePickerComponent.render();
        }

        this.productMultiSelectionComponent.render();

        this.$netTurnoverTooltip = this.netTurnoverTooltipComponent.render();
        this.pagerComponent.render();

        this.hasAllDataOnClient = this.pagerComponent.hasAllDataOnClient();

        let canSort = parseInt(this.$tableReport.data("totalrecords"), 0) > 1;
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort,
            this.hasAllDataOnClient,
            this.afterSort,
            { 0: { sorter: "text" } }));

        this.$tableReport.stickyTableHeaders();

        if (this.$hasSubmittedParams.val().toString().toLowerCase() !== "true"
            && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0)));
        }

        this.registerEvents();
        this.$.unblockUI();
    };
    checkReady = () => {
        return this.$("#IsHistoricReport").length > 0;
    };
    cacheElements = () => {
        this.$pageTitle = this.$(".page-title");
        this.$tableReport = this.$("#tbl-report");
        this.$drilldownLinks = this.$(".downline-link");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$multiSelectButton = this.$("#dropdown-product").parent().find(".multiselect.btn");
        this.$reportSwitch = this.$("#report-switch");

        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$iconNormalColumn = this.$("#icon-normal-column");
        this.$hasSubmittedParams = this.$("#HasSubmittedParams");
        this.$breadcrumbLinks = this.$(".breadcrumb-link");
    };

    registerEvents = () => {
        this.$reportSwitch.on("click", this.onSwitchReport);
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").on("click", this.onExportExcel);
    };
    onSwitchReport = () => {
        this.isSwitchView = true;
        this.isHistoricReport = !this.$reportSwitch.data("is-historic-report");
        this.$reportForm.trigger("submit");
    };
    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");
        let $netTurnoverTooltip = this.$netTurnoverTooltip;

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
            this.netTurnoverTooltipComponent.positionNetTurnoverTooltip($netTurnoverTooltip);
        } else {
            this.hideFilter();
        }

        this.replaceHrefParam("IsFilterVisible", currentHideState);
    };
    setStackMode = () => {
        this.$.blockUI();

        let tableReport = this.$tableReport;
        this.$pageTitle.addClass("stack-mode");
        tableReport.addClass("stack-mode");

        this.replaceHrefParam("IsStackMode", true);

        let this$ = this.$;
        alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 3, "col-turnover-combined");

        let levels = this.levels;
        this$.each(levels, function (index: number) {
            let level = levels[index];
            let cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
            alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
        });

        this.isFirstSwitchMode = false;

        this.sortComponent.bindTableSorter(this.getSortedColumnName());

        this.$.unblockUI();
    };
    onSwitchMode = () => {
        this.$.blockUI();
        let tableReport = this.$tableReport;
        let currentStackModeState = tableReport.hasClass("stack-mode");

        if (currentStackModeState) {
            this.$pageTitle.removeClass("stack-mode");
            tableReport.removeClass("stack-mode");
        } else {
            this.$pageTitle.addClass("stack-mode");
            tableReport.addClass("stack-mode");
        }

        this.replaceHrefParam("IsStackMode", !currentStackModeState);

        let levels = this.levels;
        let this$ = this.$;

        if (this.isFirstSwitchMode) {
            if (currentStackModeState) {
                alphaReports.splitCell("col-turnover");

                this$.each(levels, function (index: number) {
                    let level = levels[index];
                    alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                });
            } else {
                alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 3, "col-turnover-combined");

                this$.each(levels, function (index: number) {
                    let level = levels[index];
                    let cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                    alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
                });
            }

            this.isFirstSwitchMode = false;
        }

        this.sortComponent.bindTableSorter(this.getSortedColumnName());
        this.$.unblockUI();
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    afterSort = () => {
        var this$ = this.$;
        if (this.hasAllDataOnClient) {
            var sortedColumn = this$(".sorting-asc");
            var direction = !sortedColumn.length;
            if (sortedColumn.length === 0) {
                sortedColumn = this$(".sorting-desc");
            }

            var sortedColumnName = sortedColumn.data("sortedname");

            this.replaceHrefParam("SortPropertyName", sortedColumnName || sortedColumn.parent().data("sortedname"));
            this.replaceHrefParam("IsDescendingSort", direction);

            var isEvenRow = false;
            this.$tableReport.find("tbody > tr").each(function () {
                var $row = this$(this);
                if ($row.hasClass("even-row") && !isEvenRow) {
                    $row.removeClass("even-row");
                } else if (!$row.hasClass("even-row") && isEvenRow) {
                    $row.addClass("even-row");
                }

                isEvenRow = !isEvenRow;
            });
        } else {
            console.log(21);
            this.$reportForm.trigger("submit");
        }
    };

    hideFilter = () => {
        let $netTurnoverTooltip = this.$netTurnoverTooltip;

        this.$pageTitle.addClass("hide-filter");

        this.$reportForm.addClass("hide");
        this.netTurnoverTooltipComponent.positionNetTurnoverTooltip($netTurnoverTooltip);
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let sortInfo = this.sortComponent.getSortInfo();
        let tableReport = this.$tableReport;

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: this.isHistoricReport,
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
    showValidateProductError = () => {
        this.$multiSelectButton.addClass("error");
    };
    clearError = () => {
        this.$multiSelectButton.removeClass("error");
    };
    replaceHrefParam = (paramName: string, paramValue: any) => {
        let this$ = this.$;

        this.$drilldownLinks.each(function () {
            let queryString = this$(this).attr("href");
            let newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
            this$(this).attr("href", newQueryString);
        });

        this.$breadcrumbLinks.each(function () {
            let queryString = this$(this).attr("href");
            let newQueryString = alphaReports.replaceQueryStringValue(queryString, paramName, paramValue);
            this$(this).attr("href", newQueryString);
        });
    };
    onSubmitReport = (e: any) => {
        let this$ = this.$;

        this$.blockUI();

        let params = this.getParams();

        if (this.isSwitchView) {
            this.$fromDate.val("");
            this.$toDate.val("");
        } else if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible);
        this.$isDescendingSort.val(params.IsDescendingSort);
        this.$isHistoricReport.val(params.IsHistoricReport);
        this.$isStackMode.val(params.IsStackMode);
        this.$sortPropertyName.val(params.SortPropertyName);
    };

    getSortedColumnName = () => {
        let sortedColumn = this.$tableReport.data("sortedcolumn");
        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        if (sortedColumn === "NetTurnover" || sortedColumn === "GrossComm") {
            return "Turnover";
        }

        if (sortedColumn === "MemberNetWinLoss.Comm" || sortedColumn === "MemberNetWinLoss.Total") {
            return "MemberNetWinLoss.WinLoss";
        }

        if (sortedColumn === "AgentNetWinLoss.Comm" || sortedColumn === "AgentNetWinLoss.Total") {
            return "AgentNetWinLoss.WinLoss";
        }

        if (sortedColumn === "MasterNetWinLoss.Comm" || sortedColumn === "MasterNetWinLoss.Total") {
            return "MasterNetWinLoss.WinLoss";
        }

        if (sortedColumn === "SuperNetWinLoss.Comm" || sortedColumn === "SuperNetWinLoss.Total") {
            return "SuperNetWinLoss.WinLoss";
        }

        return sortedColumn;
    };
}

$(() => {
    const winLossDetailView = new WinLossDetailView(
        $,
        new DateRangePickerComponent($),
        new PagerComponent($),
        new ProductMultiSelectionComponent($),
        new SortComponent($),
        new ReportTooltip($),
        new MonthPickerComponent($),
        new NetTurnoverTooltipComponent($));

    winLossDetailView.init();
});
