/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/client-pager.ts" />
/// <reference path="../components/link-builder.ts" />
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
var StatementView = /** @class */ (function (_super) {
    __extends(StatementView, _super);
    function StatementView($, dateRangePickerComponent, pagerComponent, topWindow, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.onClickIconFilter = function () {
            var filter = _this.$reportForm;
            var currentHideState = filter.hasClass("hide");
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$reportForm.addClass("hide");
            }
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
        };
        _this.onSubmitReport = function () {
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : !_this.$reportForm.is(":hidden")));
            _this.$.blockUI();
        };
        _this.loadStatementDetail = function (transferDate) {
            var param = {
                TransferDate: transferDate,
                CustId: _this.custId,
                CustName: _this.custName
            };
            var url = _this.$statementDetailsURL.val() + "?" + _this.$.param(param);
            var title = 'Transfer Details (' + _this.custName + ')';
            if (_this.mainDomain) {
                _this.mainDomain = _this.mainDomain.indexOf('http') === -1 ?
                    location.origin.split(':')[0] + ':' + _this.mainDomain + '/site-main' : _this.mainDomain;
                _this.topWindow.postMessage({
                    event_id: "detail-statement",
                    title: title,
                    url: url
                }, _this.mainDomain);
            }
            else {
                _this.topWindow.popupManager.open(url, title, 900);
            }
        };
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.pagerComponent = pagerComponent;
        _this.topWindow = topWindow;
        return _this;
    }
    StatementView.prototype.init = function () {
        this.cacheElements();
        this.dateRangePickerComponent.render();
        this.pagerComponent.pageList = this.pagerComponent.smallPageList;
        this.pagerComponent.render();
        this.$tableReport.stickyTableHeaders();
        this.registerEvents();
        this.initHelp();
        this.mainDomain = this.$("#MainDomain").val();
    };
    StatementView.prototype.registerEvents = function () {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
        this.$("#icon-excel").on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        var self = this;
        this.statementDetailView = new StatementDetailView(this.$, this.popupStatementDetails);
        this.$(".column-transfer-amount").on("click", function () {
            self.loadStatementDetail(self.$(this).data("transfer-date"));
        });
        this.popupStatementDetails.draggable({ handle: ".modal-header" });
    };
    StatementView.prototype.cacheElements = function () {
        _super.prototype.cacheElements.call(this);
        this.$statementDetailsURL = this.$("#StatementDetailsURL");
        this.custId = this.$("#CustId").val();
        this.custName = this.$("#CustName").val();
        this.popupStatementDetails = this.$("#popup-statement-details");
        this.popupBodyStatementDetails = this.popupStatementDetails.find(".modal-body");
    };
    ;
    return StatementView;
}(BaseReport));
//# sourceMappingURL=statement.js.map