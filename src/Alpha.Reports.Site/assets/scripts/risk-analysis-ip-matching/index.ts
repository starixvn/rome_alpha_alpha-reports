/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" /> 
/// <reference path="../components/link-builder.ts" />

"use strict";

class RiskAnalysisIPMatching {
    $: JQueryStatic;
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sortComponent: SortComponent) {

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
    }

    init = () => {
        this.$("SELECT").on("change", (event) => {
            this.$('#MemberCount').val((<HTMLInputElement>event.target).value);
        });
    };
}

$(function () {
    const riskAnalysisIPMatching = new RiskAnalysisIPMatching(
        $,
        new DateRangePickerComponent($),
        new SortComponent($, new LinkBuilderComponent($)));
    riskAnalysisIPMatching.init();
});