/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="statement.ts" />

$(() => {
    const smaStatementView = new SMAStatementView(
        $,
        new DateRangePickerComponent($),
        new ClientPagerComponent($),
        window.top,
        new LinkBuilderComponent($));

    smaStatementView.init();
});