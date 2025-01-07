"use strict";
declare var TipsyHelper;

class BetCreditHistory {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    pagerComponent: PagerComponent;
    $reportForm: JQuery;
    $pageTitle: JQuery;
    $txtMember: JQuery;
    $txtRefNo: JQuery;
    $msgError: JQuery;
    $isFilterVisible: JQuery;
    $exportExcelForm: JQuery;
    resources: any;

    MAX_LENGTH_REFNO = 19;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        pagerComponent: PagerComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }

    public init() {
        this.$.blockUI();

        this.cacheElements();
        this.initComponents();
        this.bindEvents();
        const $errMsg = this.$("#ErrMsg");
        $errMsg.length && TipsyHelper.showError(this.$txtMember, $errMsg.val(), "nw");

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

        this.resources = {
            usernameIsRequired: this.$('#UsernameIsRequired').val(),
            invalidRefNo: this.$('#InvalidRefNo').val(),
        }
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
        this.$txtMember.keyup(() => TipsyHelper.clear(this.$txtMember));
        this.$txtRefNo.keyup(() => TipsyHelper.clear(this.$txtRefNo));
        this.$reportForm.submit(this.onSubmitReport);
        $('input[type="submit"]').click(() => {
            this.resetPaging();
        });
        $(".range-option").click(() => {
            this.resetPaging();
        });

        this.$("#icon-excel").click(this.onExportExcel);
    }

    resetPaging = () => {
        this.$("#PageIndex").val("1");
        this.$("#PageSize").val("100");
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
            this.showValidationMsgError(this.resources.usernameIsRequired, this.$txtMember);
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
            this.showValidationMsgError(this.resources.usernameIsRequired, this.$txtMember);
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        if (!this.validateRefNo()) {
            this.showValidationMsgError(this.resources.invalidRefNo, this.$txtRefNo);
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.hideValidationMsgError();
        return true;
    }

    validateUsername = () => this.$txtMember.val() !== "";

    validateRefNo = () => this.$txtRefNo.val().length <= this.MAX_LENGTH_REFNO && this.$txtRefNo.val().trim().match(/^[0-9]*$/);

    showValidationMsgError = (msg: string, element: JQuery) => {
        TipsyHelper.showError(element, msg, "nw");
        this.$("#tbl-report").remove();
        this.$("#report-pager").remove();
    }

    hideValidationMsgError = () => {
        TipsyHelper.clear(this.$txtMember);
        TipsyHelper.clear(this.$txtRefNo)
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