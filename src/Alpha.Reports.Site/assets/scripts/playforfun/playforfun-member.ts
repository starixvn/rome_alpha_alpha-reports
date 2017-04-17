/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="playforfun.ts" />

"use strict";

class PlayForFunMemberView extends PlayForFunView {
    public getSorterHeader() {
        return { 0: { sorter: "text" }, 1: { sorter: "text" } };
    }
}

$(() => {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const playForFunMemberView = new PlayForFunMemberView(
        $,
        new DateRangePickerComponent($),
        new SortComponent($, linkBuilderComponent),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        linkBuilderComponent);

    playForFunMemberView.init();
});