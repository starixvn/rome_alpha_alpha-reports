/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="statement.ts" />
$(function () {
    var smaStatementView = new SMAStatementView($, new DateRangePickerComponent($), new ClientPagerComponent($), window.top, new LinkBuilderComponent($));
    smaStatementView.init();
});
//# sourceMappingURL=sma-statement-init.js.map