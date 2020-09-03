'use strict';
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
var WinLossViewNew = /** @class */ (function (_super) {
    __extends(WinLossViewNew, _super);
    function WinLossViewNew($, dateRangePickerComponent, productMultiSelectionComponent, reportTooltip, monthPickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isHistoricReport = false;
        _this.superColumnSortNames = [
            'CustName', 'Turnover', 'GrossComm', 'MasterNetWinLoss.WinLoss',
            'SuperAgentNetWinLoss.WinLoss', 'SuperAgentNetWinLoss.Comm',
            'SuperAgentNetWinLoss.Total', 'Company'
        ];
        _this.masterColumnSortNames = [
            'CustName', 'Turnover', 'GrossComm', 'AgentNetWinLoss.WinLoss',
            'MasterAgentNetWinLoss.WinLoss', 'MasterAgentNetWinLoss.Comm',
            'MasterAgentNetWinLoss.Total', 'Company'
        ];
        _this.agentColumnSortNames = [
            'CustName', 'Turnover', 'GrossComm', 'MemberNetWinLoss.WinLoss',
            'MemberNetWinLoss.Comm', 'MemberNetWinLoss.Total',
            'AgentAgentNetWinLoss.WinLoss', 'AgentAgentNetWinLoss.Comm',
            'AgentAgentNetWinLoss.Total', 'Company'
        ];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheDOMElements();
            _this.bindFilter();
            _this.canSort = parseInt(_this.$tableReport.data('totalrecords'), 0) > 1;
            _this.isSuperLevel = JSON.parse(_this.$isSuperLevel.val().toLowerCase());
            _this.isMasterLevel = JSON.parse(_this.$isMasterLevel.val().toLowerCase());
            _this.isAgentLevel = JSON.parse(_this.$isAgentLevel.val().toLowerCase());
            _this.initDataTable();
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
            _this.$.unblockUI();
        };
        _this.initDataTable = function () {
            var self = _this;
            var columnSortIndex = _this.getColumnSortIndex();
            var columnSortDirection = JSON.parse(_this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc';
            _this.$dataTable = _this.$tableReport.DataTable({
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
                                var headerText = self.getColumnNameByCustLevel(idx.column, header.text());
                                return "<li data-dtr-index=\"" + idx.column + "\">" +
                                    ("<span class=\"dtr-title\">" + headerText + "</span> ") +
                                    ("<span class=\"dtr-data\">" + cellData + "</span></li>");
                            }).toArray().join('');
                            return data ?
                                self.$("<ul data-dtr-index=\"" + rowIdx + "\"/>").append(data) :
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
                order: [[columnSortIndex, columnSortDirection]],
                paging: false,
                searching: false,
                fixedHeader: true,
                processing: true,
                drawCallback: function () {
                    _this.$('tbody').append(_this.$('tr.tbl-footer'));
                    _this.$tableReport.removeClass('hidden');
                }
            });
            _this.$('#tbl-report thead').on('click', 'th.control', function (event) { return _this.collapsedExpandedAll(event); });
            _this.$dataTable.on('draw.dt', function () { return _this.triggerRedrawTable(); });
        };
        _this.collapsedAll = function (header) {
            header.removeClass('parent');
            _this.$('tbody tr.parent').each(function (index, element) {
                _this.$(element).find('.control').click();
            });
        };
        _this.expandedAll = function (header) {
            header.addClass('parent');
            _this.$('tbody tr').not('.parent').each(function (index, element) {
                _this.$(element).find('.control').click();
            });
        };
        _this.collapsedExpandedAll = function (event) {
            _this.$.blockUI();
            var header = _this.$(event.currentTarget).closest('tr');
            header.hasClass('parent') ?
                _this.collapsedAll(header) :
                _this.expandedAll(header);
            _this.$.unblockUI();
        };
        _this.triggerRedrawTable = function () {
            _this.doAdjustHeader();
            _this.$sortPropertyName.val(_this.getColumnSortName());
            _this.$isDescendingSort.val(String(_this.isDescendingSort()));
        };
        _this.doAdjustHeader = function () {
            _this.$dataTable.fixedHeader.adjust();
            var scrollTo = _this.$('.fixedHeader-floating thead').length > 0 ? _this.$(document).height() : 0;
            _this.$(window).scrollTop(scrollTo);
        };
        _this.cacheDOMElements = function () {
            _this.$pageTitle = _this.$('.page-title ');
            _this.$tableReport = _this.$('#tbl-report');
            _this.$drilldownLinks = _this.$('.downline-link');
            _this.$exportExcelForm = _this.$('#export-excel-form');
            _this.$reportForm = _this.$('#report-form');
            _this.$historicReport = _this.$('#historic-report');
            _this.$filterIcon = _this.$('.icon-filter');
            _this.$excelIcon = _this.$('#icon-excel');
            _this.$fromDate = _this.$('#from-date');
            _this.$toDate = _this.$('#to-date');
            _this.$userSelectedDateRangeOption = _this.$('#UserSelectedDateRangeOption');
            _this.$isFilterVisible = _this.$('#IsFilterVisible');
            _this.$isSuperLevel = _this.$('#IsSuperLevel');
            _this.$isMasterLevel = _this.$('#IsMasterLevel');
            _this.$isAgentLevel = _this.$('#IsAgentLevel');
            _this.$isDescendingSort = _this.$('#IsDescendingSort');
            _this.$sortPropertyName = _this.$('#SortPropertyName');
            _this.$isHistoricReport = _this.$('#IsHistoricReport');
        };
        _this.checkReady = function () {
            return _this.$('#IsHistoricReport').length > 0;
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$('#dropdown-product').parent().find('.multiselect.btn');
            if (alphaReports.screenSmallThan768()) {
                _this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', parseInt(_this.$('#CustId').val(), 0)));
            }
            _this.isHistoricReport = _this.$isHistoricReport.val().toLocaleLowerCase() === 'true';
            if (_this.isHistoricReport) {
                _this.$reportForm.removeClass('hide');
                _this.monthPickerComponent.render();
                if (_this.$isFilterVisible.val().toLowerCase() !== 'true') {
                    _this.$reportForm.addClass('hide');
                }
            }
            else {
                _this.dateRangePickerComponent.render();
            }
            if (_this.$isFilterVisible.val().toString().toLowerCase() === 'false') {
                _this.hideFilter();
            }
        };
        _this.registerEvents = function () {
            _this.$reportForm.submit(_this.onSubmitReportEvent);
            _this.$historicReport.on('click', _this.onSubmitHistoricReportEvent);
            _this.$filterIcon.on('click', _this.onFilterEvent);
            _this.$excelIcon.on('click', _this.onExportExcelEvent);
        };
        _this.onFilterEvent = function () {
            var currentHideState = _this.$reportForm.hasClass('hide');
            currentHideState ? _this.showFilter() : _this.hideFilter();
            _this.replaceDrilldownParam('IsFilterVisible', currentHideState);
        };
        _this.onExportExcelEvent = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val('');
            _this.$toDate.val('');
            _this.isHistoricReport = !_this.$historicReport.data('is-historic-report');
            _this.$reportForm.trigger('submit');
        };
        _this.showFilter = function () {
            _this.$pageTitle.removeClass('hide-filter');
            _this.$reportForm.removeClass('hide');
        };
        _this.hideFilter = function () {
            _this.$pageTitle.addClass('hide-filter');
            _this.$reportForm.addClass('hide');
        };
        _this.isDescendingSort = function () {
            var order = _this.$dataTable.order();
            return order[0][1] !== 'asc';
        };
        _this.getColumnSortName = function () {
            var order = _this.$dataTable.order();
            var columnSortIndex = order[0][0];
            if (_this.isSuperLevel) {
                return _this.superColumnSortNames[columnSortIndex];
            }
            if (_this.isMasterLevel) {
                return _this.masterColumnSortNames[columnSortIndex];
            }
            if (_this.isAgentLevel) {
                return _this.agentColumnSortNames[columnSortIndex];
            }
            return _this.$tableReport.data('sortedcolumn');
        };
        _this.getColumnSortIndex = function () {
            var columnSortName = _this.$sortPropertyName.val();
            if (_this.isSuperLevel) {
                return _this.superColumnSortNames.indexOf(columnSortName);
            }
            if (_this.isMasterLevel) {
                return _this.masterColumnSortNames.indexOf(columnSortName);
            }
            if (_this.isAgentLevel) {
                return _this.agentColumnSortNames.indexOf(columnSortName);
            }
            return 0;
        };
        _this.getColumnNameByCustLevel = function (columnIndex, defaultColumnName) {
            if (_this.isSuperLevel) {
                return superColumnNames[columnIndex];
            }
            if (_this.isMasterLevel) {
                return masterColumnNames[columnIndex];
            }
            if (_this.isAgentLevel) {
                return agentColumnNames[columnIndex];
            }
            return defaultColumnName;
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                CustId: _this.$tableReport.data('custid'),
                IsAnyProductSelected: selectedProducts.length > 0,
                IsStackMode: false,
                IsHistoricReport: _this.isHistoricReport,
                SortPropertyName: _this.getColumnSortName(),
                IsDescendingSort: _this.isDescendingSort(),
                HideFilter: _this.$reportForm.is(':hidden')
            };
        };
        _this.showValidaProductError = function () {
            _this.$multiSelectButton.addClass('error');
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels('error');
        };
        _this.clearError = function () {
            _this.$multiSelectButton.removeClass('error');
        };
        _this.replaceDrilldownParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        _this.onSubmitReportEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidaProductError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$sortPropertyName.val(params.SortPropertyName);
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
        };
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        _this.monthPickerComponent = monthPickerComponent;
        _this.linkBuilderComponent = linkBuilderComponent;
        return _this;
    }
    return WinLossViewNew;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var winLossView = new WinLossViewNew($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($), new ReportTooltip($), new MonthPickerComponent($), linkBuilderComponent);
    winLossView.init();
});
//# sourceMappingURL=winloss-new.js.map