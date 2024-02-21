/// <reference path="../../../assets/scripts/core.ts" />
/// <reference path="../../../assets/scripts/base-report.ts" />
/// <reference path="../../../assets/scripts/components/daterangepicker.ts" />
/// <reference path="../../../assets/scripts/components/monthpicker.ts" />
/// <reference path="../../../assets/scripts/components/pager.ts" />
/// <reference path="../../../assets/scripts/components/productmultiselection.ts" />
/// <reference path="../../../assets/scripts/components/productdetail-multiselection.ts" />
/// <reference path="../../../assets/scripts/components/sort.ts" />
/// <reference path="../../../assets/scripts/components/link-builder.ts" />
/// <reference path="../../../assets/scripts/components/tooltip.ts" />

"use strict";

class WinLossDetailView extends BaseReport {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;

    $isHistoricReport: JQuery;
    $multiSelectButton: JQuery;

    $drilldownLinks: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;
    $reportForm: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $reportSwitch: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;
    $exportExcelForm: JQuery;
    $iconNormalColumn: JQuery;
    $isSetDefaultStackMode: JQuery;
    $breadcrumbLinks: JQuery;

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
    linkBuilderComponent: LinkBuilderComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pageComponent: PagerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip,
        monthPickerComponent: MonthPickerComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pageComponent;
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
        this.$multiSelectButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

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

        if (this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
            && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(
                new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0))
            );
        }
        this.linkBuilderComponent.render();
        this.registerEvents();

        this.initHelp();
        this.$.unblockUI();
    };
    checkReady = () => {
        return this.$("#IsHistoricReport").length > 0;
    };
    public cacheElements() {
        super.cacheElements();

        this.$drilldownLinks = this.$(".downline-link");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$reportSwitch = this.$("#historic-report");

        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$iconNormalColumn = this.$("#icon-normal-column");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
        this.$breadcrumbLinks = this.$(".breadcrumb-link");
    }

    public registerEvents() {
        super.registerEvents();

        this.$reportSwitch.on("click", this.onSwitchReport);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").on("click", this.onExportExcel);
    }
    onSwitchReport = () => {
        this.isSwitchView = true;
        this.isHistoricReport = !this.$reportSwitch.data("is-historic-report");
        this.$reportForm.trigger("submit");
    };
    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
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

    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let sortInfo = this.sortComponent.getSortInfo();
        let tableReport = this.$tableReport;

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            IsAnyProductDetailSelected: this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            IsStackMode: tableReport.hasClass("stack-mode"),
            IsHistoricReport: this.isHistoricReport,
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
    showValidateProductError = () => {
        this.$multiSelectButton.addClass("error");
        this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
    };
    showValidaProductDetailError = () => {
        this.productMultiSelectionComponent.addErrorToProductDetailComponent();
    };
    clearError = () => {
        this.$multiSelectButton.removeClass("error");
    };
    replaceHrefParam = (paramName: string, paramValue: any) => {
        this.linkBuilderComponent.updateParam(paramName, paramValue);
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
        } else if (!params.IsAnyProductDetailSelected) {
            this.showValidaProductDetailError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$isHistoricReport.val(String(params.IsHistoricReport));
        this.$isStackMode.val(String(params.IsStackMode));
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
    const linkBuilderComponent = new LinkBuilderComponent($);
    const winLossDetailView = new WinLossDetailView(
        $,
        new DateRangePickerComponent($),
        new PagerComponent($),
        new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)),
        new SortComponent($, linkBuilderComponent),
        new ReportTooltip($),
        new MonthPickerComponent($),
        linkBuilderComponent);

    winLossDetailView.init();
});