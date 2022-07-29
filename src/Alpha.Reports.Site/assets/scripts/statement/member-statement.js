/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
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
var MemberStatementView = /** @class */ (function (_super) {
    __extends(MemberStatementView, _super);
    function MemberStatementView($, dateRangePickerComponent, pagerComponent, topWindow, reportTypeComponent, windowLocation, linkBuilderComponent) {
        var _this = _super.call(this, $, dateRangePickerComponent, pagerComponent, topWindow, linkBuilderComponent) || this;
        _this.transactionType = {
            WinLoss: 1,
            VoidCancelled: 2,
            Outstanding: 3,
            GrandPrize: 4
        };
        _this.changeReportType = function () {
            var url = _this.reportTypeComponent.$dropdown.find("option:selected").data("url");
            _this.windowLocation.href = url;
        };
        _this.viewBetList = function (element, transactionType) {
            var param = {
                custid: _this.custId,
                winlossdate: element.data("transfer-date"),
                transtype: transactionType,
                custname: _this.custName
            };
            var url = _this.betListUrl + "?" + jQuery.param(param);
            if (_this.mainDomain) {
                _this.mainDomain = _this.mainDomain.indexOf('http') === -1 ?
                    location.origin.split(':')[0] + ':' + _this.mainDomain + '/site-main' : _this.mainDomain;
                _this.topWindow.postMessage({
                    event_id: "daily-statement",
                    title: _this.betListTitle,
                    url: url
                }, _this.mainDomain);
            }
            else {
                _this.topWindow.popupManager.open(url, _this.betListTitle, 900);
            }
        };
        _this.reportTypeComponent = reportTypeComponent;
        _this.windowLocation = windowLocation;
        return _this;
    }
    MemberStatementView.prototype.init = function () {
        this.$.blockUI();
        _super.prototype.init.call(this);
        var reportTypeComponentOption = new SingleSelectionDropdownOption("dropdown-report-type", "ReportType", this.changeReportType);
        this.reportTypeComponent.render(reportTypeComponentOption);
        this.betListUrl = this.$("#BetListUrl").val();
        this.betListTitle = this.$("#BetListTitle").val();
        this.mainDomain = this.$("#MainDomain").val();
        this.$.unblockUI();
    };
    MemberStatementView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        var self = this;
        this.$tableReport.find(".col-void-amount").click(function () {
            self.viewBetList(self.$(this), self.transactionType.VoidCancelled);
        });
        this.$tableReport.find(".col-outstanding").click(function () {
            self.viewBetList(self.$(this), self.transactionType.Outstanding);
        });
        this.$tableReport.find(".col-win-loss").click(function () {
            self.viewBetList(self.$(this), self.transactionType.WinLoss);
        });
        this.$tableReport.find(".col-grand-prize").click(function () {
            self.viewBetList(self.$(this), self.transactionType.GrandPrize);
        });
    };
    return MemberStatementView;
}(StatementView));
//# sourceMappingURL=member-statement.js.map