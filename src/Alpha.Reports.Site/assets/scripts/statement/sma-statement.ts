/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="statement.ts" />

class SMAStatementView extends StatementView {
    public init() {
        this.$.blockUI();

        super.init();

        this.$.unblockUI();
    }
}