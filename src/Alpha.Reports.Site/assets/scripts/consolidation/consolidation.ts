/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/productdetail-multiselection.ts" />
/// <reference path="../components/currency-selection.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="../components/tooltip.ts" />

"use strict";
class ConsolidationView extends BaseReport {
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    currencySelectionComponent: CurrencySelectionComponent;
    dateRangePickerComponent: DateRangePickerComponent;
    reportTooltip: ReportTooltip;

    $productButton: JQuery;
    $currencyButton: JQuery;
    $tableReport: JQuery;
    $isSetDefaultStackMode: JQuery;

    $reportSwitch: JQuery;
    $reportForm: JQuery;
    $pageTitle: JQuery;
    $exportExcelForm: JQuery;
    $isFilterVisible: JQuery;
    $isStackMode: JQuery;
    $productPTListPopup: JQuery;
    productPTListHint: string;

    isFirstSwitchMode = true;

    constructor(
        $: JQueryStatic,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        currencySelectionComponent: CurrencySelectionComponent,
        dateRangePickerComponent: DateRangePickerComponent,
        reportTooltip: ReportTooltip,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.currencySelectionComponent = currencySelectionComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.reportTooltip = reportTooltip;
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

        this.$tableReport.stickyTableHeaders();

        if (this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
            && alphaReports.screenSmallThan768()) {
            this.setStackMode();
            this.reportTooltip.show(
                new ReportTooltipOption("icon-normal-column", parseInt(this.$("#CustId").val(), 0))
            );
        }

        this.registerEvents();
        this.initHelp();
        this.$.unblockUI();
    };
    checkReady = () => {
        return this.$("#IsSetDefaultStackMode").length > 0;
    };

    public cacheElements() {
        this.$tableReport = this.$("#tbl-report");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");

        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");

        this.$pageTitle = this.$(".page-title");

        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$isStackMode = this.$("#IsStackMode");

        this.$productPTListPopup = this.$("#consolidation-product-pt-list-popup");
        this.productPTListHint = this.$("#product-pt-list-hint").html();
    }

    public registerEvents() {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$(".ic-switchmode").on("click", this.onSwitchMode);

        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").on("click", this.onExportExcel);

        let self = this;
        let this$ = this.$;

        this.$("[name=\"report-type\"]").click(function () {
            self.onChangeReportType(this$(this));
        });

        this.$tableReport.find(".username-row").click(function () {
            self.collapseExpandRows(this$(this));
        });

        this.$tableReport.find(".downline-link").click(function (e) {
            e.stopPropagation();
        });

        this.$tableReport.find(".view-product-pt-list").click(function (e) {
            e.stopPropagation();
            self.viewProductPTList(this$(this));
        });
    }

    setStackMode = () => {
        this.$pageTitle.addClass("stack-mode");
        this.$tableReport.addClass("stack-mode");
        this.replaceHrefParam("IsStackMode", true);

        alphaReports.combineConsecutiveCells(this.$("td.col-member-winloss"), 3, "col-member-winloss-combined");
        alphaReports.combineConsecutiveCells(this.$("td.col-owner-turnover"), 4, "col-member-owner-turnover-combined");
        this.isFirstSwitchMode = false;
    };
    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
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

        if (this.isFirstSwitchMode) {
            if (currentStackModeState) {
                alphaReports.splitCell("col-member-winloss");
                alphaReports.splitCell("col-owner-turnover");
            } else {
                alphaReports.combineConsecutiveCells(this.$("td.col-member-winloss"), 3, "col-member-winloss-combined");
                alphaReports.combineConsecutiveCells(this.$("td.col-owner-turnover"), 4, "col-member-owner-turnover-combined");
            }

            this.isFirstSwitchMode = false;
        }

        this.$.unblockUI();
    };
    onSubmitReport = (e: any) => {
        let this$ = this.$;

        this$.blockUI();

        this.currencySelectionComponent.updateSelectedValue();
        let params = this.getParams();

        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this$.unblockUI();

            if (e) {
                e.preventDefault();
            }

            return;
        }

        if (!params.IsAnyProductDetailSelected) {
            this.showValidaProductDetailError();
            this$.unblockUI();

            if (e) {
                e.preventDefault();
            }

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.$isStackMode.val(String(params.IsStackMode));
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    replaceHrefParam = (paramName: string, paramValue: any) => {
        this.linkBuilderComponent.updateParam(paramName, paramValue);
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            IsAnyProductDetailSelected: this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
            IsStackMode: this.$tableReport.hasClass("stack-mode"),
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
    showValidateProductError = () => {
        this.$productButton.addClass("error");
        this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
    };
    showValidaProductDetailError = () => {
        this.productMultiSelectionComponent.addErrorToProductDetailComponent();
    };
    onChangeReportType = (actionElement: JQuery) => {
        let $reportForm = this.$reportForm;
        let action = actionElement.val();
        $reportForm.attr("action", action);
        $reportForm.submit();
    };
    collapseExpandRows = ($usernameRow) => {
        const custId = $usernameRow.data("custid");
        const $collapseExpandIcon = $usernameRow.find("#icon-" + custId);

        if ($collapseExpandIcon.hasClass("ic-arrow-right")) {
            $collapseExpandIcon.removeClass().addClass("ic-arrow-down");
            this.$tableReport.find(".child-row-" + custId).removeClass("hide");
        } else {
            $collapseExpandIcon.removeClass().addClass("ic-arrow-right");
            this.$tableReport.find(".child-row-" + custId).addClass("hide");
        }
    };
    viewProductPTList = ($icon) => {
        this.$("#product-pt-list-username").html($icon.data("username"));
        this.$("#product-pt-list-content").html(this.buildProductPTListHtml($icon.data("custid")));
        this.$("#product-pt-list-hint").html(this.productPTListHint.replace("{0}", $icon.data("userlevel")));

        this.$productPTListPopup.modal({ show: true, backdrop: false });
    };
    buildProductPTListHtml = (custId: number) => {
        let html = [];
        const self = this;
        this.$tableReport.find(".child-row-" + custId).each(function (index) {
            const $this = self.$(this);
            if ($this.hasClass("consolidation-pt-row")) {
                if (index !== 0) {
                    html.splice(-1);
                    html.push("</li>");
                }

                html.push("<li>");
                html.push($this.find(".group-pt").html());
                html.push(": ");
            } else {
                html.push($this.find(".col-product").html());
                html.push(", ");
            }
        });

        html.splice(-1);
        html.push("</li>");

        return html.join("");
    };
}

$(() => {
    const consolidationView = new ConsolidationView(
        $,
        new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)),
        new CurrencySelectionComponent($),
        new DateRangePickerComponent($),
        new ReportTooltip($),
        new LinkBuilderComponent($));

    consolidationView.init();
});