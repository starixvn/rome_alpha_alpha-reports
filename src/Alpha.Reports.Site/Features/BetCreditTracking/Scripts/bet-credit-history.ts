"use strict";
class BetCreditHistory {
    private $: JQueryStatic;
    private dateRangePickerComponent: DateRangePickerComponent;
    private $reportForm: JQuery;
    private $pageTitle: JQuery;
    private $txtMember: JQuery;
    private $msgError: JQuery;
    private $invalidMsgError: JQuery;
    private $isFilterVisible: JQuery;
    private $exportExcelForm: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
    }

    public init() {
        this.$.blockUI();

        this.cacheElements()
        this.initComponents()
        this.bindEvents();

        this.$.unblockUI();
    }

    public cacheElements() {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$txtMember = this.$("#txt-member");
        this.$msgError = this.$("#msg-error");
        this.$invalidMsgError = this.$("#invalid-msg-error");
    }

    public initComponents() {
        this.dateRangePickerComponent.render(
            new DateRangePickerOption("daterange-picker", "report-form", null));

        this.$("#tbl-report").stickyTableHeaders();

    }

    public bindEvents() {
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
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
            this.showValidationUsernameError();
            this.$.unblockUI();

            return;
        }

        this.hideValidationUsernameError();
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSubmitReport = (e: any) => {
        this.$.blockUI();

        if (!this.validateUsername()) {
            this.showValidationUsernameError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.hideValidationUsernameError();
        return true;
    }

    validateUsername = () => {
        if (this.$txtMember.val() === "") {
            return false;
        }

        return true;
    }

    showValidationUsernameError = () => {
        this.$msgError.removeClass("hide");
        this.$invalidMsgError.addClass("hide");
    }
    hideValidationUsernameError = () => this.$msgError.addClass("hide");
}

$(function () {
    const betCreditHistory = new BetCreditHistory(
        $,
        new DateRangePickerComponent($)
    );
    betCreditHistory.init();
});