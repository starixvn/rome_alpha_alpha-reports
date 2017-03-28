/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="statement.ts" />
$(() => {
    const memberStatementView = new MemberStatementView(
        $,
        new DateRangePickerComponent($),
        new ClientPagerComponent($),
        window.top,
        new SingleSelectionDropdownComponent($),
        window.location,
        new LinkBuilderComponent($));

    memberStatementView.init();
});