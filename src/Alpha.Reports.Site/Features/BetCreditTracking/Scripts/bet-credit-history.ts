"use strict";
class BetCreditHistory {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    $reportForm: JQuery;
    $pageTitle: JQuery;
    $txtMember: JQuery;
    $txtRefNo: JQuery;
    $msgError: JQuery;
    $validationMsgError: JQuery;
    $isFilterVisible: JQuery;
    $exportExcelForm: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: PagerComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    resources = {
        usernameIsRequired: (<HTMLInputElement>document.getElementById('UsernameIsRequired')).value,
        invalidRefNo: (<HTMLInputElement>document.getElementById('InvalidRefNo')).value,
    }

    public init() {
        this.$.blockUI();

        this.cacheElements();
        this.initComponents();
        this.bindEvents();

        this.$.unblockUI();
    }

    public cacheElements() {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$txtMember = this.$("#txt-member");
        this.$txtRefNo = this.$("#txt-refno");
        this.$msgError = this.$("#msg-error");
        this.$validationMsgError = this.$("#validation-msg-error");
    }

    public initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption("daterange-picker", "report-form", null));

        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();

        this.$("#tbl-report").stickyTableHeaders();
    }

    public bindEvents() {
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#btn-submit").click(() => this.$("#PageIndex").val("1"));

        this.$("#icon-excel").click(this.onExportExcel);
    }

    onClickIconFilter = () => {
        const isHide = this.$reportForm.hasClass("hide");
        this.$isFilterVisible.val(isHide.toString());

        if (isHide) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
        }
    }

    onExportExcel = () => {
        this.$.blockUI();

        if (!this.validateUsername()) {
            this.showValidationMsgError(this.resources.usernameIsRequired);
            this.$.unblockUI();

            return;
        }

        this.hideValidationMsgError();
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSubmitReport = (e: any) => {
        this.$.blockUI();

        if (!this.validateUsername()) {
            this.showValidationMsgError(this.resources.usernameIsRequired);
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        if (!this.validateRefNo()) {
            this.showValidationMsgError(this.resources.invalidRefNo);
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.hideValidationMsgError();
        return true;
    }

    validateUsername = () => this.$txtMember.val() !== "";

    validateRefNo = () => this.$txtRefNo.val().trim().match(/^[0-9]*$/);

    showValidationMsgError = (msg) => {
        this.$msgError.addClass("hide");
        this.$validationMsgError.text(msg);
        this.$validationMsgError.removeClass("hide");
        this.$("#tbl-report").remove();
        this.$("#report-pager").remove();
    }

    hideValidationMsgError = () => {
        this.$validationMsgError.text("");
        this.$validationMsgError.addClass("hide");
    }
}

$(function () {
    const betCreditHistory = new BetCreditHistory(
        $,
        new DateRangePickerComponent($),
        new PagerComponent($)
    );
    betCreditHistory.init();
});