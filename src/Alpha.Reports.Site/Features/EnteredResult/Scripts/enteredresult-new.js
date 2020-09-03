"use strict";
var EnterResultViewNew = /** @class */ (function () {
    function EnterResultViewNew($, dateRangePickerComponent) {
        var _this = this;
        this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.dateRangePickerComponent.render();
            _this.cacheDOMElements();
            _this.canSort = parseInt(_this.$("#tbl-enteredresult-report").data("totalrecords"), 0) > 1;
            _this.initDataTable();
            _this.registerEvents();
            _this.$.unblockUI();
        };
        this.checkReady = function () {
            return _this.$("#IsHistoricReport").length > 0;
        };
        this.cacheDOMElements = function () {
            _this.$pageTitle = _this.$(".page-title ");
            _this.$reportForm = _this.$("#report-form");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$historicReport = _this.$("#historic-report");
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$filterIcon = _this.$(".icon-filter");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$submitButton = _this.$(".btn-submit");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
        };
        this.registerEvents = function () {
            _this.$historicReport.on("click", _this.onSubmitHistoricReportEvent);
            _this.$filterIcon.on("click", _this.onFilterEvent);
            _this.$excelIcon.on("click", _this.onExportExcelEvent);
            _this.$reportForm.submit(_this.onSubmitNormalReportEvent);
        };
        this.initDataTable = function () {
            var self = _this;
            _this.$tableReport = _this.$("#tbl-enteredresult-report").DataTable({
                responsive: {
                    details: {
                        type: 'column',
                        target: -1,
                        renderer: function (api, rowIdx) {
                            var data = api.cells(rowIdx, ':hidden').eq(0).map(function (cell) {
                                var header = self.$(api.column(cell.column).header());
                                var idx = api.cell(cell).index();
                                if (header.hasClass('control') || header.hasClass('never')) {
                                    return '';
                                }
                                var dtPrivate = api.settings()[0];
                                var cellData = dtPrivate.oApi._fnGetCellData(dtPrivate, idx.row, idx.column, 'display');
                                return "<li data-dtr-index=\"" + idx.column + "\">" +
                                    ("<span class=\"dtr-title\">" + header.text() + "</span> ") +
                                    ("<span class=\"dtr-data\">" + cellData + "</span></li>");
                            }).toArray().join('');
                            var toDate = "<li><b>" + self.$('#ToDate').val() + "</b></li>";
                            return data ?
                                self.$("<ul data-dtr-index=\"" + rowIdx + "\"/>").append(toDate, data) :
                                false;
                        }
                    }
                },
                columnDefs: [{
                        className: 'control',
                        orderable: false,
                        targets: -1
                    }],
                ordering: _this.canSort,
                order: [[0, 'asc']],
                paging: false,
                searching: false,
                fixedHeader: true,
                processing: true
            });
            _this.$('#tbl-enteredresult-report thead').on('click', 'th.control', function (event) { return _this.collapsedExpandedAll(event); });
            _this.$tableReport.on('draw.dt', function () { return _this.triggerRedrawTable(); });
        };
        this.collapsedAll = function (header) {
            header.removeClass('parent');
            _this.$('tbody tr.parent').each(function (index, element) {
                _this.$(element).find('.control').click();
            });
        };
        this.expandedAll = function (header) {
            header.addClass('parent');
            _this.$('tbody tr').not('.parent').each(function (index, element) {
                _this.$(element).find('.control').click();
            });
        };
        this.collapsedExpandedAll = function (event) {
            _this.$.blockUI();
            var header = _this.$(event.currentTarget).closest('tr');
            header.hasClass('parent') ?
                _this.collapsedAll(header) :
                _this.expandedAll(header);
            _this.$.unblockUI();
        };
        this.triggerRedrawTable = function () {
            _this.doAdjustHeader();
        };
        this.doAdjustHeader = function () {
            _this.$tableReport.fixedHeader.adjust();
            var scrollTo = _this.$('.fixedHeader-floating thead').length > 0 ? _this.$(document).height() : 0;
            _this.$(window).scrollTop(scrollTo);
        };
        this.onFilterEvent = function () {
            var currentHideState = _this.$reportForm.hasClass("hide");
            currentHideState ? _this.showFilter() : _this.hideFilter();
        };
        this.showFilter = function () {
            _this.$pageTitle.removeClass("hide-filter");
            _this.$reportForm.removeClass("hide");
        };
        this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        this.onExportExcelEvent = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onSubmitNormalReportEvent = function () {
            _this.setHiddenValue();
        };
        this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val("");
            _this.$toDate.val("");
            _this.$isHistoricReport.val("true");
            _this.setHiddenValue();
            _this.$reportForm.trigger("submit");
        };
        this.setHiddenValue = function () {
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
            _this.$.blockUI();
        };
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
    }
    return EnterResultViewNew;
}());
$(function () {
    var enterResultView = new EnterResultViewNew($, new DateRangePickerComponent($));
    enterResultView.init();
});
//# sourceMappingURL=enteredresult-new.js.map