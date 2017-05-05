/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/tooltip.ts" />

"use strict";

class WinLossByProductView extends BaseReport {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;

    $multiSelectButton: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;
    $isAgentLevel: JQuery;
    $reportForm: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $submitButton: JQuery;
    $switchModeIcon: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;
    $isSetDefaultStackMode: JQuery;

    isFirstSwitchMode = true;
    isSwitchView = false;
    levels = ["member", "agent", "master", "super"];

    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;
    linkBuilderComponent: LinkBuilderComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();

        this.cacheDOMElements();

        this.bindFilter();
        this.$tableReport.stickyTableHeaders();
        this.productToggle();
        this.registerEvents();

        this.initHelp();
        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isAgentLevel = this.$("#IsAgentLevel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$reportForm = this.$("#report-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$switchModeIcon = this.$(".icon-switchmode");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
    };

    checkReady = () => {
        return this.$("#IsSetDefaultStackMode").length > 0;
    };

    productToggle = () => {
        let this$ = this.$;

        this.$('tbody').each(function () {
            this$(this).find('tr:first').click(
                function () {
                    let productRow = this$(this);
                    let productRowFirstColumn = productRow.find('td:first');
                    productRowFirstColumn.hasClass("active") ? productRowFirstColumn.removeClass("active") : productRowFirstColumn.addClass("active");
                    productRow.parent().find("tr").slice(1).toggle();
                });
        });
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

        if (this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
            && alphaReports.screenSmallThan768()) {
            this.setCssBySwitchMode(false);

            let isAgentLevel = this.$isAgentLevel.val().toLocaleLowerCase() === "true";
            this.combineColumns(isAgentLevel);
            this.isFirstSwitchMode = false;

            this.reportTooltip.show(
                new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0))
            );
        }

        this.dateRangePickerComponent.render();

        if (this.$isFilterVisible.val().toString().toLowerCase() === "false") {
            this.hideFilter();
        }
    };

    public registerEvents() {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);
    }

    onFilterEvent = () => {
        let currentHideState = this.$reportForm.hasClass("hide");
        currentHideState ? this.showFilter() : this.hideFilter();
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSwitchModeEvent = () => {
        var isStackMode = this.$tableReport.hasClass("stack-mode");

        this.setCssBySwitchMode(isStackMode);

        if (this.isFirstSwitchMode) {
            this.renderAtFirstSwitchMode(isStackMode);
            this.isFirstSwitchMode = false;
        }
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
            if (level == "master") {
                alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? "" : " altercol");
            }
            else {
                alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
            }

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
        var cellCss = "text-right col-" + level + "-winloss-combined";

        if (level == "master") {
            cellCss = cellCss + (index % 2 ? "" : " altercol");
        }
        else {
            cellCss = cellCss + (index % 2 ? " altercol" : "");
        }

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
        var tableReport = this.$tableReport;
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            CustId: tableReport.data("custid"),
            IsStackMode: tableReport.hasClass("stack-mode"),
            HideFilter: this.$reportForm.is(":hidden"),
        };
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

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
        this.$isStackMode.val(String(params.IsStackMode));
    };
    showValidateProductError = () => {
        this.$multiSelectButton.addClass("error");
    };
}

const winLossByProductView = new WinLossByProductView(
    $,
    new DateRangePickerComponent($),
    new ProductMultiSelectionComponent($),
    new ReportTooltip($),
    new LinkBuilderComponent($));

$(function () {
    winLossByProductView.init();
});