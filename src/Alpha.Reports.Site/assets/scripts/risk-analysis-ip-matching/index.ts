/// <reference path="../core.ts" />
/// <reference path="../components/pager.ts" />

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
    pagerComponent: PagerComponent;
    hasAllDataOnClient = false;

    constructor($: JQueryStatic, pageComponent: PagerComponent) {
        this.$ = $;
        this.pagerComponent = pageComponent;
    }

    init = () => {
        this.$.blockUI();

        this.$("SELECT").on("change", (event) => {
            this.$('#MemberCount').val((<HTMLInputElement>event.target).value);
        });

        let options = new DatePickerOption(this.$('#MinDate').val(), true);

        this.$('#dpkContainer').datepicker(options).data("DateTimePicker").maxDate(this.$('#Today').val());

        this.$('#dpkContainer').on("dp.change", (e) => {
            this.$('#SelectedWinlostDate').val(this.$('#WinlostDate').val());
        });

        this.$("#icon-excel").on("click", () => {
            this.$("#export-excel-form").submit();
        });

        this.pagerComponent.render();

        //this.hasAllDataOnClient = this.pagerComponent.hasAllDataOnClient();

        this.$.unblockUI();
    };
}

$(function () {
    const riskAnalysisIPMatching = new RiskAnalysisIPMatching($, new PagerComponent($));
    riskAnalysisIPMatching.init();
});