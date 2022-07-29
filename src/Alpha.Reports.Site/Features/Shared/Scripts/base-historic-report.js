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
var BaseHistoricReport = /** @class */ (function (_super) {
    __extends(BaseHistoricReport, _super);
    function BaseHistoricReport($, dateRangePickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.doAdjustDatePickerCalendar = function () {
            if (alphaReports.screenSmallThan768() && !alphaReports.screenLandscapeMode()) {
                var width = _this.$(window).width(), widget = _this.$(".bootstrap-datetimepicker-widget.dropdown-menu");
                widget.css({ width: width });
            }
        };
        _this.checkReady = function () { return _this.$("#IsHistoricReport").length > 0; };
        _this.dateRangePickerComponent = dateRangePickerComponent;
        return _this;
    }
    BaseHistoricReport.prototype.cacheDOMElements = function () {
        _super.prototype.cacheDOMElements.call(this);
        this.$excelIcon = this.$(".icon-excel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$fromDate = this.$("#from-date");
        this.$historicReport = this.$("#historic-report");
        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.$toDate = this.$("#to-date");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
    };
    BaseHistoricReport.prototype.initDataTable = function (options) {
        if (options === void 0) { options = {}; }
        _super.prototype.initDataTable.call(this, options);
        this.doAdjustDataTable();
    };
    BaseHistoricReport.prototype.doAdjustDataTable = function () {
        var browserWidth = this.$(window).width(), deviceWidth = screen.width || window.outerWidth, columnCount = this.$dataTable.columns()[0].length;
        if (deviceWidth < 768 && browserWidth >= 768) {
            for (var i = 3; i < columnCount - 1; i++) {
                this.$(this.$dataTable.column(i).header()).removeClass("desktop min-tablet-l mobile").addClass("mobile");
            }
        }
        if (this.$tableReport.width() > browserWidth) {
            this.$tableReport.find("tbody tr td:first-child").css("white-space", "nowrap");
            this.$(this.$dataTable.column(2).header()).removeClass("all").addClass("min-tablet-l");
        }
        this.$dataTable.responsive.rebuild();
        this.$dataTable.responsive.recalc();
    };
    BaseHistoricReport.prototype.registerEvents = function () {
        var _this = this;
        _super.prototype.registerEvents.call(this);
        this.$excelIcon.on("click", function () { return _this.onExportExcelEvent(); });
        this.$historicReport.on("click", function () { return _this.onSubmitHistoricReportEvent(); });
        this.$reportForm.submit(function (event) { return _this.onSubmitReportEvent(event); });
        this.$fromDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
        this.$toDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
    };
    BaseHistoricReport.prototype.onExportExcelEvent = function () {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    BaseHistoricReport.prototype.onSubmitHistoricReportEvent = function () { };
    BaseHistoricReport.prototype.onSubmitReportEvent = function (event) { };
    return BaseHistoricReport;
}(BaseReportNew));
//# sourceMappingURL=base-historic-report.js.map