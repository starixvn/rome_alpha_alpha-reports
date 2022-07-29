/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="statement.ts" />
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var SMAStatementView = /** @class */ (function (_super) {
    __extends(SMAStatementView, _super);
    function SMAStatementView() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    SMAStatementView.prototype.init = function () {
        this.$.blockUI();
        _super.prototype.init.call(this);
        this.$.unblockUI();
    };
    return SMAStatementView;
}(StatementView));
//# sourceMappingURL=sma-statement.js.map