var StatementDetailView = /** @class */ (function () {
    function StatementDetailView($, $popup) {
        var _this = this;
        this.init = function () {
            _this.cacheElements();
            _this.registerEvents();
            _this.$tableReport.stickyTableHeaders();
        };
        this.cacheElements = function () {
            _this.$exportExcelForm = _this.$popup.find("#export-excel-form");
            _this.$tableReport = _this.$popup.find("#tbl-report");
        };
        this.registerEvents = function () {
            _this.$popup.find("#icon-excel").off("click").on("click", _this.onExportExcel);
        };
        this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
        };
        this.$ = $;
        this.$popup = $popup;
    }
    return StatementDetailView;
}());
$(function () {
    var statementDetailView = new StatementDetailView($, $(document));
    statementDetailView.init();
});
//# sourceMappingURL=statement-details.js.map