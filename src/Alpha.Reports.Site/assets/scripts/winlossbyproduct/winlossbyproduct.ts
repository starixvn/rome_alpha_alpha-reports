/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/productdetail-multiselection.ts" />
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
        this.$switchModeIcon = this.$(".ic-switchmode");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
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

        var self = this;
        this.$(".product-row").on("click", function () {
            self.onClickProduct(self.$(this));
        });

        this.$(".sport-row").on("click", function () {
            self.onClickSport(self.$(this));
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
    };

    onClickProduct = (productRow: JQuery) => {
        var productId = productRow.data("product-id");
        var isHide = alphaReports.convertAttributeToBool(productRow.data("is-hide"));

        var columnUsername = productRow.find(".col-username");
        var sportRows = this.$(".sport-of-" + productId);
        var betTypeRows = this.$(".bet-type-of-product-" + productId);

        if (isHide) {
            columnUsername.addClass("active");
            columnUsername.find(".ic-arrow-right").removeClass("ic-arrow-right").addClass("ic-arrow-down");

            sportRows.find(".col-username").addClass("active");
            sportRows.find(".col-username .ic-arrow-right").removeClass("ic-arrow-right").addClass("ic-arrow-down");

            sportRows.data("is-hide", "false");
            sportRows.removeClass("hide");
            betTypeRows.removeClass("hide");
        } else {
            columnUsername.removeClass("active");
            columnUsername.find(".ic-arrow-down").removeClass("ic-arrow-down").addClass("ic-arrow-right");

            sportRows.find(".col-username").removeClass("active");
            sportRows.find(".col-username .ic-arrow-down").removeClass("ic-arrow-down").addClass("ic-arrow-right");

            sportRows.data("is-hide", "true");
            sportRows.addClass("hide");
            betTypeRows.addClass("hide");
        }

        productRow.data("is-hide", !isHide);
    };

    onClickSport = (sportRow: JQuery) => {
        var productId = sportRow.data("product-id");
        var sportId = sportRow.data("sport-id");

        var isHide = alphaReports.convertAttributeToBool(sportRow.data("is-hide"));

        var columnUsername = sportRow.find(".col-username");
        var betTypeRows = this.$(".bet-type-of-product-" + productId + ".bet-type-of-sport-" + sportId);

        if (isHide) {
            columnUsername.addClass("active");
            columnUsername.find(".ic-arrow-right").removeClass("ic-arrow-right").addClass("ic-arrow-down");

            betTypeRows.removeClass("hide");
        } else {
            columnUsername.removeClass("active");
            columnUsername.find(".ic-arrow-down").removeClass("ic-arrow-down").addClass("ic-arrow-right");

            betTypeRows.addClass("hide");
        }

        sportRow.data("is-hide", !isHide);
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
            IsAnyProductDetailSelected: this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
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

        if (!params.IsAnyProductDetailSelected) {
            this.showValidaProductDetailError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

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
}

const winLossByProductView = new WinLossByProductView(
    $,
    new DateRangePickerComponent($),
    new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)),
    new ReportTooltip($),
    new LinkBuilderComponent($));

$(function () {
    winLossByProductView.init();
});