/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

class WinLossAnalysisView {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;

    $multiSelectButton: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;
    $reportForm: JQuery;
    $reportTypeId: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $submitButton: JQuery;
    $switchModeIcon: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;
    $isSetDefaultStackMode: JQuery;

    isFirstSwitchMode = true;
    isSwitchView = false;
    levels = ["member", "agent", "master", "super"];

    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    sortComponent: SortComponent;
    reportTooltip: ReportTooltip;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        sortComponent: SortComponent,
        reportTooltip: ReportTooltip) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.sortComponent = sortComponent;
        this.reportTooltip = reportTooltip;
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
        this.registerEvents();

        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$reportForm = this.$("#report-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$switchModeIcon = this.$(".icon-switchmode");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$toDate = this.$("#to-date");
        this.$reportTypeId = this.$("#ReportTypeId");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
    };

    checkReady = () => {
        return this.$("#IsSetDefaultStackMode").length > 0;
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

        if (this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
            && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(
                new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0))
            );
        }

        this.dateRangePickerComponent.render();

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
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);

        let self = this;
        this.$(':radio[name=reportType]').change(function () {
            self.onSubmitReportEvent(this.form);
            self.$reportForm.submit();
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
        this.$isStackMode.val(String(params.IsStackMode));
        this.$sortPropertyName.val(params.SortPropertyName);
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$reportTypeId.val(params.ReportTypeId);
    };

    onFilterEvent = () => {
        let currentHiddenState = this.$reportForm.hasClass("hide");
        currentHiddenState ? this.showFilter() : this.hideFilter();
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
        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) => {
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns();
    }

    setStackMode = () => {
        this.$.blockUI();
        this.setCssBySwitchMode(false);
        this.renderAtFirstSwitchMode(false);
        this.isFirstSwitchMode = false;
        this.$.unblockUI();
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

        alphaReports.splitCell("col-number-of-tickets");
        this$.each(levels, function (index: number) {
            var level = levels[index];
            alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
        });
    };

    combineColumns = () => {
        let levels = this.levels;
        let this$ = this.$;

        alphaReports.combineConsecutiveCells(this.$("td.col-number-of-tickets"), 2, "text-right col-number-of-tickets");
        this.$.each(levels, function (index: number) {
            let level = levels[index];
            let cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
            alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
        });
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
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            HideFilter: this.$reportForm.is(":hidden"),
            ReportTypeId: this.$('input[name=reportType]:checked').val()  
        };
    };

    showValidaProductError = () => {
        this.$multiSelectButton.addClass("error");
    };

    clearError = () => {
        this.$multiSelectButton.removeClass("error");
    };

    afterSort = () => {
        var this$ = this.$;
        var sortedColumn = this$(".sorting-asc:visible");
        var direction = !sortedColumn.length;
        if (sortedColumn.length === 0) {
            sortedColumn = this$(".sorting-desc");
        }

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

    getSortedColumnName = () => {
        let sortedColumn = this.$tableReport.data("sortedcolumn");
        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        if (sortedColumn === "Turnover") {
            return "BetCount";
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

const winLossAnalysisView = new WinLossAnalysisView(
    $,
    new DateRangePickerComponent($),
    new ProductMultiSelectionComponent($),
    new SortComponent($),
    new ReportTooltip($));

$(function () {
    winLossAnalysisView.init();
});