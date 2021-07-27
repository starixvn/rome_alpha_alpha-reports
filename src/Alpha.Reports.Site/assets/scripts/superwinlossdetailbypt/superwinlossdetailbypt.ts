"use strict";

class SuperWinLossDetailByPTView extends BaseReport {
    $: JQueryStatic;

    $pageTitle: JQuery;
    $tableReport: JQuery;

    $multiSelectButton: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;
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
    $viewProductPTList: JQuery;
    $viewProductPTListPopup: JQuery;

    hasAllDataOnClient = true;
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
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

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
        this.bindSorter();
        this.registerEvents();
        this.initHelp();

        this.$.unblockUI();
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$tableReport = this.$("#tbl-report");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$reportForm = this.$("#report-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$switchModeIcon = this.$(".icon-switchmode");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
        this.$viewProductPTList = this.$("#view-product-pt-list");
        this.$viewProductPTListPopup = this.$("#product-pt-list-popup");
    };

    checkReady = () => {
        return this.$("#IsSetDefaultStackMode").length > 0;
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$("#dropdown-product").parent().find(".multiselect.btn");

        if (this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
            && alphaReports.screenSmallThan768()) {
            this.setCssBySwitchMode(false);

            this.combineColumns();
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

    bindSorter = () => {
        let canSort = JSON.parse(this.$tableReport.data("cansort"));
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort,
            this.hasAllDataOnClient,
            this.afterSort,
            { 0: { sorter: "text" } },
            this.$reportForm));

        this.$tableReport.stickyTableHeaders();
    };

    afterSort = () => {
        this.sortComponent.afterSort();

        if (this.hasAllDataOnClient) {
            this.$tableReport.find("tbody > tr.pt-row").each((index, element) => {
                let parent = this.$(element);
                let ptValue = parent.data("pt-value");
                let children = this.$tableReport.find(`tbody > tr.pt-product-row[data-pt-value="${ptValue}"]`);

                children.insertAfter(parent);
            });
        }
    };

    public registerEvents() {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$switchModeIcon.on("click", this.onSwitchModeEvent);
        this.$viewProductPTList.on("click", this.onClickViewProductPTList);

        var self = this;
        this.$(".pt-row").on("click", function () {
            self.onClickPT(self.$(this));
        });
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

        this.sortComponent.bindTableSorter(this.getSortedColumnName());
    };

    onClickViewProductPTList = () => {
        this.$viewProductPTListPopup.modal({ show: true, backdrop: false });

        var position = this.$viewProductPTList.position();
        this.$viewProductPTListPopup.css({
            bottom: "auto",
            top: position.top + 60
        });
    }

    onClickPT = (ptRow: JQuery) => {
        var ptValue = ptRow.data("pt-value");
        var isHide = alphaReports.convertAttributeToBool(ptRow.data("is-hide"));
        var columnUsername = ptRow.find(".col-username");
        var ptProductRows = this.$(`.pt-product-row[data-pt-value="${ptValue}"]`);

        if (isHide) {
            columnUsername.addClass("active");
            columnUsername.find(".icon-arrow-right").removeClass("icon-arrow-right").addClass("icon-arrow-down");
            ptProductRows.removeClass("hide");
        } else {
            columnUsername.removeClass("active");
            columnUsername.find(".icon-arrow-down").removeClass("icon-arrow-down").addClass("icon-arrow-right");
            ptProductRows.addClass("hide");
        }

        ptRow.data("is-hide", !isHide);
    };

    renderAtFirstSwitchMode = (isSwitchToNormalMode: boolean) => {
        isSwitchToNormalMode ? this.splitColumns() : this.combineColumns();
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
        alphaReports.splitCell("col-turnover");
        this.$.each(this.levels, function (index: number, level: string) {
            alphaReports.splitCell(`col-${level}-winloss`);
        });
    };

    combineColumns = () => {
        var self = this;

        alphaReports.combineConsecutiveCells(this.$("td.col-turnover"), 3, "text-right col-turnover-combined");
        this.$.each(this.levels, function (index: number, level: string) {
            self.combineToWinLossColumn(level);
        });
    };

    combineToWinLossColumn = (level: string) => {
        alphaReports.combineConsecutiveCells(this.$(`td.col-${level}-winloss`), 3, `text-right col-${level}-winloss-combined`);
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
        let tableReport = this.$tableReport;
        let sortInfo = this.sortComponent.getSortInfo();
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            CustId: tableReport.data("custid"),
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            IsAnyProductSelected: selectedProducts.length > 0,
            IsAnyProductDetailSelected: this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
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

        if (!params.IsAnyProductDetailSelected) {
            this.showValidaProductDetailError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$sortPropertyName.val(params.SortPropertyName);
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
        this.$isStackMode.val(String(params.IsStackMode));
    };

    showValidateProductError = () => {
        this.$multiSelectButton.addClass("error");
        this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
    };

    showValidaProductDetailError = () => {
        this.productMultiSelectionComponent.addErrorToProductDetailComponent();
    };

    getSortedColumnName = () => {
        let sortedColumn = this.$tableReport.data("sortedcolumn");
        if (!this.$tableReport.hasClass("stack-mode")) {
            return sortedColumn;
        }

        if (sortedColumn === "NetTurnover" || sortedColumn === "GrossComm") {
            return "Turnover";
        }

        if (sortedColumn === "MemberComm" || sortedColumn === "MemberTotal") {
            return "MemberWinLost";
        }

        if (sortedColumn === "AgComm" || sortedColumn === "AgTotal") {
            return "AgWinLost";
        }

        if (sortedColumn === "MAgComm" || sortedColumn === "MAgTotal") {
            return "MAgWinLost";
        }

        if (sortedColumn === "SAgComm" || sortedColumn === "SAgTotal") {
            return "SAgWinLost";
        }

        return sortedColumn;
    };
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const productDetailMultiSelectionComponent = new ProductDetailMultiSelectionComponent($);
    const superWinLossDetailByPTView = new SuperWinLossDetailByPTView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($, productDetailMultiSelectionComponent),
        new SortComponent($, linkBuilderComponent),
        new ReportTooltip($),
        linkBuilderComponent);

    superWinLossDetailByPTView.init();
});