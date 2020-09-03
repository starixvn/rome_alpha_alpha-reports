"use strict";
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
var EnterResultViewNew = /** @class */ (function (_super) {
    __extends(EnterResultViewNew, _super);
    function EnterResultViewNew($, dateRangePickerComponent) {
        var _this = _super.call(this, $, dateRangePickerComponent, null) || this;
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _super.prototype.init.call(_this);
            _this.dateRangePickerComponent.render();
            _this.initDataTable();
            _this.registerEvents();
            _this.$.unblockUI();
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheDOMElements.call(_this);
            _this.$tableReport = _this.$("#tbl-enteredresult-report");
        };
        _this.registerEvents = function () {
            _super.prototype.registerEvents.call(_this);
            _this.$excelIcon.on("click", _this.onExportExcelEvent);
            _this.$historicReport.on("click", _this.onSubmitHistoricReportEvent);
            _this.$reportForm.submit(_this.onSubmitNormalReportEvent);
        };
        _this.initDataTable = function () {
            _super.prototype.initDataTable.call(_this, {
                order: [[0, 'asc']]
            });
        };
        _this.renderDetail = function (rowIdx, data) {
            var toDate = "<li><b>" + _this.$('#ToDate').val() + "</b></li>";
            return data ?
                _this.$("<ul data-dtr-index=\"" + rowIdx + "\"/>").append(toDate, data) :
                false;
        };
        _this.drawCallback = function () { };
        _this.onSubmitNormalReportEvent = function () {
            _this.setHiddenValue();
        };
        _this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val("");
            _this.$toDate.val("");
            _this.$isHistoricReport.val("true");
            _this.setHiddenValue();
            _this.$reportForm.trigger("submit");
        };
        _this.setHiddenValue = function () {
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
            _this.$.blockUI();
        };
        return _this;
    }
    return EnterResultViewNew;
}(BaseReportNew));
$(function () {
    var enterResultViewNew = new EnterResultViewNew($, new DateRangePickerComponent($));
    enterResultViewNew.init();
});
//# sourceMappingURL=enteredresult-new.js.map