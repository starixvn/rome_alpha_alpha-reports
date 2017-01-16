/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class WinLossView {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;

    $isHistoricReport: JQuery;
    $multiSelectButton: JQuery;

    $drilldownLinks: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;
    $isAgentLevel: JQuery;
    $reportForm: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $historicReport: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $submitButton: JQuery;
    $switchModeIcon: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;

    isFirstSwitchMode = true;
    isHistoricReport = false;
    isSwitchView = false;
    levels = ["member", "agent", "master", "super"];

    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;
    monthPickerComponent: MonthPickerComponent;
    linkBuilderComponent: LinkBuilderComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip,
        monthPickerComponent: MonthPickerComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
        this.monthPickerComponent = monthPickerComponent;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();

        this.cacheDOMElements();

        this.bindFilter();
        this.bindSorting();
        this.$tableReport.stickyTableHeaders();
        this.linkBuilderComponent.render();
        this.registerEvents();
        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$drilldownLinks = this.$(".downline-link");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isAgentLevel = this.$("#IsAgentLevel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$reportForm = this.$("#report-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$multiSelectButton = this.$(".multiselect.btn");
        this.$historicReport = this.$("#historic-report");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$switchModeIcon = this.$(".icon-switchmode");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isHistoricReport = this.$("#IsHistoricReport");
    };

    checkReady = () => {
        return this.$("#IsHistoricReport").length > 0;
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render();

        if (alphaReports.screenSmallThan768()) {
            this.reportTooltip.show(
                new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0))
            );
        }

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

        if (this.$isFilterVisible.val().toString().toLowerCase() === "false") {
            this.hideFilter();
        }
    };

    bindSorting = () => {
        let canSort = parseInt(this.$tableReport.data("totalrecords"), 0) > 1;
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort,
            true,
            this.afterSort,
            { 0: { sorter: "text" } }));
    };

    registerEvents = () => {
        this.$historicReport.on("click", this.onSubmitHistoricReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);
        this.$reportForm.submit(this.onSubmitReportEvent);
    };

    onFilterEvent = () => {
        let currentHideState = this.$reportForm.hasClass("hide");
        currentHideState ? this.showFilter() : this.hideFilter();
        this.replaceDrilldownParam("IsFilterVisible", currentHideState);
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSubmitHistoricReportEvent = () => {
        this.$fromDate.val("");
        this.$toDate.val("");
        this.isSwitchView = true;
        this.isHistoricReport = !this.$historicReport.data("is-historic-report");
        this.$reportForm.trigger("submit");
    };

    onSwitchModeEvent = () => {
        var isStackMode = this.$tableReport.hasClass("stack-mode");

        this.setCssBySwitchMode(isStackMode);
        this.replaceDrilldownParam("IsStackMode", !isStackMode);

        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }

        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) => {
        let isAgentLevel = this.$isAgentLevel.val().toLocaleLowerCase() === "true";
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns(isAgentLevel);
    }

    setCssBySwitchMode = (isSwitchToNormalMode: boolean) => {
        if (isSwitchToNormalMode) {
            this.$pageTitle.removeClass("stack-mode");
            this.$tableReport.removeClass("stack-mode");
        } else {
            this.$pageTitle.addClass("stack-mode");
            this.$tableReport.addClass("stack-mode");
        }
    };

    splitColumns = () => {
        let this$ = this.$;
        let levels = this.levels;

        alphaReports.splitCell("col-turnover");
        this$.each(levels, function (index: number) {
            var level = levels[index];
            alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
        });
    };

    combineColumns = (isAgentLevel: boolean) => {
        let levels = this.levels;
        var self = this;

        isAgentLevel ? this.combineToTurnOverColumnAgentLevel() : this.combineToTurnOverColumnSuperMasterLevel();
        this.$.each(levels, function (index: number) {
            self.combineToWinLossColumn(levels[index], index);
        });
    };

    combineToTurnOverColumnAgentLevel = () => {
        alphaReports.combineConsecutiveCells(this.$("td.col-turnover"), 2, "text-right col-turnover-combined");
    };

    combineToTurnOverColumnSuperMasterLevel = () => {
        alphaReports.combineConsecutiveCells(this.$("td.col-turnover"), 3, "text-right col-turnover-combined");
    };

    combineToWinLossColumn = (level: string, index: number) => {
        var cellCss = "text-right col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
        alphaReports.combineConsecutiveCells(this.$("td.col-" + level + "-winloss"), 3, cellCss);
    };

    showFilter = () => {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.removeClass("hide");
    };

    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };

    getParams = () => {
        var selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        var tableReport = this.$tableReport;
        let sortInfo = this.sortComponent.getSortInfo();

        return {
            CustId: tableReport.data("custid"),
            IsAnyProductSelected: selectedProducts.length > 0,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: this.isHistoricReport,
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            HideFilter: this.$reportForm.is(":hidden"),
        };
    };

    showValidaProductError = () => {
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

    afterSort = () => {
        var this$ = this.$;
        var sortedColumn = this$(".sorting-asc:visible");
        var direction = !sortedColumn.length;
        if (sortedColumn.length === 0) {
            sortedColumn = this$(".sorting-desc");
        }

        var sortedColumnName = sortedColumn.data("sortedname");

        this.replaceDrilldownParam("SortPropertyName", sortedColumnName || sortedColumn.parent().data("sortedname"));
        this.replaceDrilldownParam("IsDescendingSort", direction);

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
    };

    onSubmitReportEvent = (e: any) => {
        this.$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidaProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
        this.$isHistoricReport.val(String(params.IsHistoricReport));
        this.$isStackMode.val(String(params.IsStackMode));
        this.$sortPropertyName.val(params.SortPropertyName);
        this.$isDescendingSort.val(String(params.IsDescendingSort));
    };

    getSortedColumnName = () => {
        let sortedColumn = this.$tableReport.data("sortedcolumn");
        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        if (sortedColumn === "GrossComm") {
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

const winLossView = new WinLossView(
    $,
    new DateRangePickerComponent($),
    new ProductMultiSelectionComponent($),
    new SortComponent($),
    new ReportTooltip($),
    new MonthPickerComponent($),
    new LinkBuilderComponent($));

$(function () {
    winLossView.init();
});