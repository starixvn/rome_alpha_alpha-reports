/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/currency-selection.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/pager.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../components/link-builder.ts" />

"use strict";
class ConsolidationDetailView extends BaseReport {
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    currencySelectionComponent: CurrencySelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    reportTooltip: ReportTooltip;

    pagerComponent: PagerComponent;
    sortComponent: SortComponent;

    $productButton: JQuery;
    $currencyButton: JQuery;
    $tableReport: JQuery;

    $reportSwitch: JQuery;
    $reportForm: JQuery;
    $pageTitle: JQuery;
    $exportExcelForm: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;

    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;
    $netTurnoverTooltip: JQuery;
    $iconNormalColumn: JQuery;

    isFirstSwitchMode = true;
    hasAllDataOnClient = false;
    levels = ["member", "agent", "master", "super"];

    constructor(
        $: JQueryStatic,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        currencySelectionComponent: CurrencySelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent,
        pageComponent: PagerComponent,
        sortComponent: SortComponent) {
        super($, linkBuilderComponent);

        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.currencySelectionComponent = currencySelectionComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.reportTooltip = reportTooltip;

        this.pagerComponent = pageComponent;
        this.sortComponent = sortComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();

        this.cacheElements();

        this.productMultiSelectionComponent.render();
        this.$productButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.currencySelectionComponent.render();
        this.$currencyButton = this.$("#dropdown-currencies");

        this.dateRangePickerComponent.render();

        this.pagerComponent.render();

        this.hasAllDataOnClient = this.pagerComponent.hasAllDataOnClient();
        let canSort = parseInt(this.$tableReport.data("totalrecords"), 0) > 1;
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort,
            this.hasAllDataOnClient,
            this.sortComponent.afterSort,
            { 0: { sorter: "text" } },
            this.$reportForm));
        this.$tableReport.stickyTableHeaders();

        this.registerEvents();
        this.initHelp();
        this.$.unblockUI();
    };
    checkReady = () => {
        return this.$("#IsFilterVisible").length > 0;
    };
    public cacheElements() {
        this.$tableReport = this.$("#tbl-report");

        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");

        this.$pageTitle = this.$(".page-title");

        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");

        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$iconNormalColumn = this.$("#icon-normal-column");
    }
    public registerEvents() {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);

        this.$(".btn-submit").on("click", this.onClickSubmitButton);
        this.$reportForm.submit(this.onSubmitReport);

        this.$("#icon-excel").on("click", this.onExportExcel);

        let self = this;
        let this$ = this.$;

        this.$("[name=\"report-type\"]").click(function () {
            self.onChangeReportType(this$(this));
        });
    }
    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");
        let $netTurnoverTooltip = this.$netTurnoverTooltip;

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.hideFilter();
        }

        this.replaceHrefParam("IsFilterVisible", currentHideState);
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
    onClickSubmitButton = () => {
        this.currencySelectionComponent.updateSelectedValue();
    };
    onSubmitReport = (e: any) => {
        let this$ = this.$;

        this$.blockUI();
        let params = this.getParams();

        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this$.unblockUI();

            if (e) {
                e.preventDefault();
            }

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.$isStackMode.val(String(params.IsStackMode));

        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$sortPropertyName.val(params.SortPropertyName);
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    replaceHrefParam = (paramName: string, paramValue: any) => {
        this.linkBuilderComponent.updateParam(paramName, paramValue);
    };
    showValidateProductError = () => {
        this.$productButton.addClass("error");
    };
    hideFilter = () => {
        let $netTurnoverTooltip = this.$netTurnoverTooltip;

        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };
    getSortedColumnName = () => {
        let sortedColumn = this.$tableReport.data("sortedcolumn");

        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        sortedColumn = this.sortComponent.mapSortedColumnToStackMode(sortedColumn,
            [["Turnover", "NetTurnover", "GrossComm"],
            ["MemberNetWinLoss.WinLoss", "MemberNetWinLoss.Comm", "MemberNetWinLoss.Total"],
            ["AgentNetWinLoss.WinLoss", "AgentNetWinLoss.Comm", "AgentNetWinLoss.Total"],
            ["MasterNetWinLoss.WinLoss", "MasterNetWinLoss.Comm", "MasterNetWinLoss.Total"],
            ["SuperNetWinLoss.WinLoss", "SuperNetWinLoss.Comm", "SuperNetWinLoss.Total"]]);

        return sortedColumn;
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let sortInfo = this.sortComponent.getSortInfo();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            IsStackMode: this.$tableReport.hasClass("stack-mode"),
            IsFilterVisible: !this.$reportForm.is(":hidden"),

            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort
        };
    };
    onChangeReportType = (actionElement: JQuery) => {
        let $reportForm = this.$reportForm;
        let action = actionElement.val();
        $reportForm.attr("action", action);
        $reportForm.submit();
    };
}

$(() => {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const consolidationDetailView = new ConsolidationDetailView(
        $,
        new ProductMultiSelectionComponent($),
        new CurrencySelectionComponent($),
        new DateRangePickerComponent($),
        new ReportTooltip($),
        linkBuilderComponent,
        new PagerComponent($),
        new SortComponent($, linkBuilderComponent));

    consolidationDetailView.init();
});