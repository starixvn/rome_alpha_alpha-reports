/// <reference path="../core.ts" />

"use strict";

class DatePickerOption {
    constructor(
        public minDate: null,
        public allowInputToggle: boolean = false) {
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
    }
}

class RiskAnalysisIPMatching {
    $: JQueryStatic;
    hasAllDataOnClient = false;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.$.blockUI();

        this.$("SELECT").on("change", (event) => {
            this.$('#MemberCount').val((<HTMLInputElement>event.target).value);
        });

        const options = new DatePickerOption(this.$('#MinDate').val(), true);

        this.$('#dpkContainer').datepicker(options).data("DateTimePicker").maxDate(this.$('#Today').val());

        this.$('#dpkContainer').on("dp.change", () => {
            this.$('#SelectedWinlostDate').val(this.$('#WinlostDate').val());
        });

        this.$("#icon-excel").on("click", () => {
            this.$("#export-excel-form").submit();
        });

        this.$(".icon-filter").on("click", () => {
            const $reportForm = this.$("#report-form");
            const $pageTitle = this.$(".page-title");

            let currentHideState = $reportForm.hasClass("hide");

            if (currentHideState) {
                $pageTitle.removeClass("hide-filter");
                $reportForm.removeClass("hide");
            } else {
                $pageTitle.addClass("hide-filter");
                $reportForm.addClass("hide");
            }
        });

        this.$(".view-bet-list").on("click", () => {
            this.$.blockUI();
        });

        this.$.unblockUI();
    };
}

$(function () {
    const riskAnalysisIPMatching = new RiskAnalysisIPMatching($);
    riskAnalysisIPMatching.init();
});