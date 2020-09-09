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
        var _this = _super.call(this, $, dateRangePickerComponent, linkBuilderComponent) || this;
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
            _super.prototype.init.call(_this);
            _this.isSuperLevel = JSON.parse(_this.$isSuperLevel.val().toLowerCase());
            _this.isMasterLevel = JSON.parse(_this.$isMasterLevel.val().toLowerCase());
            _this.isAgentLevel = JSON.parse(_this.$isAgentLevel.val().toLowerCase());
            _this.linkBuilderComponent.render();
            _this.adjustPageTitle();
            _this.bindFilter();
            _this.initDataTable();
            _this.registerEvents();
            _this.initHelp();
            _this.$.unblockUI();
        };
        _this.initDataTable = function () {
            var columnSortIndex = _this.getColumnSortIndex();
            var columnSortDirection = JSON.parse(_this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc';
            _super.prototype.initDataTable.call(_this, {
                order: [[columnSortIndex, columnSortDirection]]
            });
        };
        _this.renderData = function (idxColumn, headerText, cellData) {
            var transformedHeaderText = _this.getColumnNameByCustLevel(idxColumn, headerText);
            return "<li data-dtr-index=\"" + idxColumn + "\">" +
                ("<span class=\"dtr-title\">" + transformedHeaderText + "</span> ") +
                ("<span class=\"dtr-data\">" + cellData + "</span></li>");
        };
        _this.triggerRedrawTable = function () {
            _super.prototype.triggerRedrawTable.call(_this);
            _this.$sortPropertyName.val(_this.getColumnSortName());
            _this.$isDescendingSort.val(String(_this.isDescendingSort()));
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheDOMElements.call(_this);
            _this.$drilldownLinks = _this.$('.downline-link');
            _this.$isAgentLevel = _this.$('#IsAgentLevel');
            _this.$isMasterLevel = _this.$('#IsMasterLevel');
            _this.$isSuperLevel = _this.$('#IsSuperLevel');
            _this.$tableReport = _this.$('#tbl-report');
        };
        _this.adjustPageTitle = function () {
            if (_this.isAgentLevel && alphaReports.screenSmallThan768()) {
                window.orientation === 0
                    ? _this.$('.page-title .breadcrumb').css('padding', 0)
                    : _this.$('.page-title .breadcrumb').css('padding', '0 12px');
            }
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
            _super.prototype.registerEvents.call(_this);
            _this.$excelIcon.on('click', _this.onExportExcelEvent);
            _this.$historicReport.on('click', _this.onSubmitHistoricReportEvent);
            _this.$reportForm.submit(_this.onSubmitReportEvent);
            _this.$(window).on('orientationchange', _this.adjustPageTitle);
        };
        _this.onClickIconFilter = function () {
            _super.prototype.onClickIconFilter.call(_this);
            _this.replaceHrefParam("IsFilterVisible", _this.currentHideState);
        };
        _this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val('');
            _this.$toDate.val('');
            _this.isHistoricReport = !_this.$historicReport.data('is-historic-report');
            _this.$reportForm.trigger('submit');
        };
        _this.getColumnSortName = function () {
            var order = _this.$table.order();
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
        _this.showValidateProductError = function () {
            _this.$multiSelectButton.addClass('error');
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels('error');
        };
        _this.clearError = function () {
            _this.$multiSelectButton.removeClass('error');
        };
        _this.onSubmitReportEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$sortPropertyName.val(params.SortPropertyName);
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
        };
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        _this.monthPickerComponent = monthPickerComponent;
        return _this;
    }
    return WinLossViewNew;
}(BaseReportNew));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var winLossViewNew = new WinLossViewNew($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($), new ReportTooltip($), new MonthPickerComponent($), linkBuilderComponent);
    winLossViewNew.init();
});
//# sourceMappingURL=winloss-new.js.map