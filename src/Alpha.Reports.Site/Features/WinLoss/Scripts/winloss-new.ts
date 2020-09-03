'use strict';

declare var
    superColumnNames,
    masterColumnNames,
    agentColumnNames;

class WinLossViewNew extends BaseReport {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;
    $dataTable: JQuery;

    $drilldownLinks: JQuery;

    $reportForm: JQuery;
    $historicReport: JQuery;
    $exportExcelForm: JQuery;

    $multiSelectButton: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $submitButton: JQuery;

    $userSelectedDateRangeOption: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    $isHistoricReport: JQuery;
    $isFilterVisible: JQuery;
    $isSuperLevel: JQuery;
    $isMasterLevel: JQuery;
    $isAgentLevel: JQuery;
    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;

    isHistoricReport = false;

    dateRangePickerComponent: DateRangePickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;
    monthPickerComponent: MonthPickerComponent;
    linkBuilderComponent: LinkBuilderComponent;

    canSort: boolean;
    isSuperLevel: boolean;
    isMasterLevel: boolean;
    isAgentLevel: boolean;

    superColumnSortNames = [
        'CustName', 'Turnover', 'GrossComm', 'MasterNetWinLoss.WinLoss',
        'SuperAgentNetWinLoss.WinLoss', 'SuperAgentNetWinLoss.Comm',
        'SuperAgentNetWinLoss.Total', 'Company'];
    masterColumnSortNames = [
        'CustName', 'Turnover', 'GrossComm', 'AgentNetWinLoss.WinLoss',
        'MasterAgentNetWinLoss.WinLoss', 'MasterAgentNetWinLoss.Comm',
        'MasterAgentNetWinLoss.Total', 'Company'];
    agentColumnSortNames = [
        'CustName', 'Turnover', 'GrossComm', 'MemberNetWinLoss.WinLoss',
        'MemberNetWinLoss.Comm', 'MemberNetWinLoss.Total',
        'AgentAgentNetWinLoss.WinLoss', 'AgentAgentNetWinLoss.Comm',
        'AgentAgentNetWinLoss.Total', 'Company'];

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        reportTooltip: ReportTooltip,
        monthPickerComponent: MonthPickerComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
        this.monthPickerComponent = monthPickerComponent;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();

        this.cacheDOMElements();

        this.bindFilter();

        this.canSort = parseInt(this.$tableReport.data('totalrecords'), 0) > 1;
        this.isSuperLevel = JSON.parse(this.$isSuperLevel.val().toLowerCase());
        this.isMasterLevel = JSON.parse(this.$isMasterLevel.val().toLowerCase());
        this.isAgentLevel = JSON.parse(this.$isAgentLevel.val().toLowerCase());

        this.initDataTable();

        this.linkBuilderComponent.render();
        this.registerEvents();

        this.initHelp();
        this.$.unblockUI();
    };

    initDataTable = () => {
        const self = this;
        const columnSortIndex = this.getColumnSortIndex();
        const columnSortDirection = JSON.parse(this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc';

        this.$dataTable = this.$tableReport.DataTable({
            responsive: {
                details: {
                    type: 'column',
                    target: -1,
                    renderer: (api, rowIdx) => {
                        const data = api.cells(rowIdx, ':hidden').eq(0).map(function (cell) {
                            const header = self.$(api.column(cell.column).header());
                            const idx = api.cell(cell).index();

                            if (header.hasClass('control') || header.hasClass('never')) {
                                return '';
                            }

                            const dtPrivate = api.settings()[0];
                            const cellData = dtPrivate.oApi._fnGetCellData(
                                dtPrivate, idx.row, idx.column, 'display'
                            );

                            const headerText = self.getColumnNameByCustLevel(idx.column, header.text());
                            return `<li data-dtr-index="${idx.column}">` +
                                `<span class="dtr-title">${headerText}</span> ` +
                                `<span class="dtr-data">${cellData}</span></li>`;
                        }).toArray().join('');

                        return data ?
                            self.$(`<ul data-dtr-index="${rowIdx}"/>`).append(data) :
                            false;
                    }
                }
            },
            columnDefs: [{
                className: 'control',
                orderable: false,
                targets: -1
            }],
            ordering: this.canSort,
            order: [[columnSortIndex, columnSortDirection]],
            paging: false,
            searching: false,
            fixedHeader: true,
            processing: true,
            drawCallback: () => {
                this.$('tbody').append(this.$('tr.tbl-footer'));
                this.$tableReport.removeClass('hidden');
            }
        });

        this.$('#tbl-report thead').on('click', 'th.control', event => this.collapsedExpandedAll(event));
        this.$dataTable.on('draw.dt', () => this.triggerRedrawTable());
    };

    collapsedAll = (header) => {
        header.removeClass('parent');

        this.$('tbody tr.parent').each((index, element) => {
            this.$(element).find('.control').click();
        });
    }

    expandedAll = (header) => {
        header.addClass('parent');

        this.$('tbody tr').not('.parent').each((index, element) => {
            this.$(element).find('.control').click();
        });
    }

    collapsedExpandedAll = (event) => {
        this.$.blockUI();
        const header = this.$(event.currentTarget).closest('tr');

        header.hasClass('parent') ?
            this.collapsedAll(header) :
            this.expandedAll(header);

        this.$.unblockUI();
    }

    triggerRedrawTable = () => {
        this.doAdjustHeader();

        this.$sortPropertyName.val(this.getColumnSortName());
        this.$isDescendingSort.val(String(this.isDescendingSort()));
    }

    doAdjustHeader = () => {
        this.$dataTable.fixedHeader.adjust();

        const scrollTo = this.$('.fixedHeader-floating thead').length > 0 ? this.$(document).height() : 0;
        this.$(window).scrollTop(scrollTo);
    }

    cacheDOMElements = () => {
        this.$pageTitle = this.$('.page-title ');
        this.$tableReport = this.$('#tbl-report');
        this.$drilldownLinks = this.$('.downline-link');

        this.$exportExcelForm = this.$('#export-excel-form');
        this.$reportForm = this.$('#report-form');
        this.$historicReport = this.$('#historic-report');

        this.$filterIcon = this.$('.icon-filter');
        this.$excelIcon = this.$('#icon-excel');

        this.$fromDate = this.$('#from-date');
        this.$toDate = this.$('#to-date');
        this.$userSelectedDateRangeOption = this.$('#UserSelectedDateRangeOption');

        this.$isFilterVisible = this.$('#IsFilterVisible');
        this.$isSuperLevel = this.$('#IsSuperLevel');
        this.$isMasterLevel = this.$('#IsMasterLevel');
        this.$isAgentLevel = this.$('#IsAgentLevel');
        this.$isDescendingSort = this.$('#IsDescendingSort');
        this.$sortPropertyName = this.$('#SortPropertyName');
        this.$isHistoricReport = this.$('#IsHistoricReport');
    };

    checkReady = () => {
        return this.$('#IsHistoricReport').length > 0;
    };

    bindFilter = () => {
        this.productMultiSelectionComponent.render();
        this.$multiSelectButton = this.$('#dropdown-product').parent().find('.multiselect.btn');

        if (alphaReports.screenSmallThan768()) {
            this.reportTooltip.show(
                new ReportTooltipOption('icon-normal-column', parseInt(this.$('#CustId').val(), 0))
            );
        }

        this.isHistoricReport = this.$isHistoricReport.val().toLocaleLowerCase() === 'true';
        if (this.isHistoricReport) {
            this.$reportForm.removeClass('hide');
            this.monthPickerComponent.render();
            if (this.$isFilterVisible.val().toLowerCase() !== 'true') {
                this.$reportForm.addClass('hide');
            }
        } else {
            this.dateRangePickerComponent.render();
        }

        if (this.$isFilterVisible.val().toString().toLowerCase() === 'false') {
            this.hideFilter();
        }
    };

    registerEvents = () => {
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$historicReport.on('click', this.onSubmitHistoricReportEvent);
        this.$filterIcon.on('click', this.onFilterEvent);
        this.$excelIcon.on('click', this.onExportExcelEvent);
    };

    onFilterEvent = () => {
        let currentHideState = this.$reportForm.hasClass('hide');
        currentHideState ? this.showFilter() : this.hideFilter();
        this.replaceDrilldownParam('IsFilterVisible', currentHideState);
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSubmitHistoricReportEvent = () => {
        this.$fromDate.val('');
        this.$toDate.val('');
        this.isHistoricReport = !this.$historicReport.data('is-historic-report');
        this.$reportForm.trigger('submit');
    };

    showFilter = () => {
        this.$pageTitle.removeClass('hide-filter');
        this.$reportForm.removeClass('hide');
    };

    hideFilter = () => {
        this.$pageTitle.addClass('hide-filter');
        this.$reportForm.addClass('hide');
    };

    isDescendingSort = () => {
        const order = this.$dataTable.order();
        return order[0][1] !== 'asc';
    }

    getColumnSortName = () => {
        const order = this.$dataTable.order();
        const columnSortIndex = order[0][0];
        if (this.isSuperLevel) {
            return this.superColumnSortNames[columnSortIndex];
        }
        if (this.isMasterLevel) {
            return this.masterColumnSortNames[columnSortIndex];
        }
        if (this.isAgentLevel) {
            return this.agentColumnSortNames[columnSortIndex];
        }

        return this.$tableReport.data('sortedcolumn');
    };

    getColumnSortIndex = () => {
        const columnSortName = this.$sortPropertyName.val();
        if (this.isSuperLevel) {
            return this.superColumnSortNames.indexOf(columnSortName);
        }
        if (this.isMasterLevel) {
            return this.masterColumnSortNames.indexOf(columnSortName);
        }
        if (this.isAgentLevel) {
            return this.agentColumnSortNames.indexOf(columnSortName);
        }

        return 0;
    };

    getColumnNameByCustLevel = (columnIndex: number, defaultColumnName: string) => {
        if (this.isSuperLevel) {
            return superColumnNames[columnIndex];
        }
        if (this.isMasterLevel) {
            return masterColumnNames[columnIndex];
        }
        if (this.isAgentLevel) {
            return agentColumnNames[columnIndex];
        }

        return defaultColumnName;
    };

    getParams = () => {
        var selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            CustId: this.$tableReport.data('custid'),
            IsAnyProductSelected: selectedProducts.length > 0,
            IsStackMode: false,
            IsHistoricReport: this.isHistoricReport,
            SortPropertyName: this.getColumnSortName(),
            IsDescendingSort: this.isDescendingSort(),
            HideFilter: this.$reportForm.is(':hidden')
        };
    };

    showValidaProductError = () => {
        this.$multiSelectButton.addClass('error');
        this.productMultiSelectionComponent.addClassToExcludeProductLabels('error');
    };

    clearError = () => {
        this.$multiSelectButton.removeClass('error');
    };

    replaceDrilldownParam = (paramName: string, paramValue: any) => {
        this.linkBuilderComponent.updateParam(paramName, paramValue);
    };

    onSubmitReportEvent = (e: any) => {
        this.$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidaProductError();
            this.$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
        this.$isHistoricReport.val(String(params.IsHistoricReport));
        this.$sortPropertyName.val(params.SortPropertyName);
        this.$isDescendingSort.val(String(params.IsDescendingSort));
    };
}

$(function () {
    let linkBuilderComponent = new LinkBuilderComponent($);

    const winLossView = new WinLossViewNew(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new ReportTooltip($),
        new MonthPickerComponent($),
        linkBuilderComponent);
    winLossView.init();
});