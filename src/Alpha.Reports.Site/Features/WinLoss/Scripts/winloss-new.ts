'use strict';

declare var
    superColumnNames,
    masterColumnNames,
    agentColumnNames;

class WinLossViewNew extends BaseReportNew {
    $drilldownLinks: JQuery;
    $isAgentLevel: JQuery;
    $isMasterLevel: JQuery;
    $isSuperLevel: JQuery;
    $multiSelectButton: JQuery;

    monthPickerComponent: MonthPickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;
    isSuperLevel: boolean;
    isMasterLevel: boolean;
    isAgentLevel: boolean;

    isHistoricReport = false;

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
        super($, dateRangePickerComponent, linkBuilderComponent);

        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.reportTooltip = reportTooltip;
        this.monthPickerComponent = monthPickerComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        super.init();

        this.isSuperLevel = JSON.parse(this.$isSuperLevel.val().toLowerCase());
        this.isMasterLevel = JSON.parse(this.$isMasterLevel.val().toLowerCase());
        this.isAgentLevel = JSON.parse(this.$isAgentLevel.val().toLowerCase());

        this.linkBuilderComponent.render();
        this.adjustPageTitle();
        this.bindFilter();
        this.initDataTable();

        this.registerEvents();
        this.initHelp();
        this.$.unblockUI();
    };

    initDataTable = () => {
        const columnSortIndex = this.getColumnSortIndex();
        const columnSortDirection = JSON.parse(this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc';

        super.initDataTable({
            order: [[columnSortIndex, columnSortDirection]]
        });
    };

    renderData = (idxColumn: any, headerText: string, cellData: any) => {
        const transformedHeaderText = this.getColumnNameByCustLevel(idxColumn, headerText);

        return `<li data-dtr-index="${idxColumn}">` +
            `<span class="dtr-title">${transformedHeaderText}</span> ` +
            `<span class="dtr-data">${cellData}</span></li>`;
    };

    triggerRedrawTable = () => {
        super.triggerRedrawTable();

        this.$sortPropertyName.val(this.getColumnSortName());
        this.$isDescendingSort.val(String(this.isDescendingSort()));
    };

    cacheDOMElements = () => {
        super.cacheDOMElements();

        this.$drilldownLinks = this.$('.downline-link');
        this.$isAgentLevel = this.$('#IsAgentLevel');
        this.$isMasterLevel = this.$('#IsMasterLevel');
        this.$isSuperLevel = this.$('#IsSuperLevel');
        this.$tableReport = this.$('#tbl-report');
    };

    adjustPageTitle = () => {
        if (this.isAgentLevel && alphaReports.screenSmallThan768()) {
            window.orientation === 0
                ? this.$('.page-title .breadcrumb').css('padding', 0)
                : this.$('.page-title .breadcrumb').css('padding', '0 12px');
        }
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
        super.registerEvents();

        this.$excelIcon.on('click', this.onExportExcelEvent);
        this.$historicReport.on('click', this.onSubmitHistoricReportEvent);
        this.$reportForm.submit(this.onSubmitReportEvent);
        this.$(window).on('orientationchange', this.adjustPageTitle);
    };

    onClickIconFilter = () => {
        super.onClickIconFilter();

        this.replaceHrefParam("IsFilterVisible", this.currentHideState);
    };

    onSubmitHistoricReportEvent = () => {
        this.$fromDate.val('');
        this.$toDate.val('');
        this.isHistoricReport = !this.$historicReport.data('is-historic-report');
        this.$reportForm.trigger('submit');
    };

    getColumnSortName = () => {
        const order = this.$table.order();
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

    showValidateProductError = () => {
        this.$multiSelectButton.addClass('error');
        this.productMultiSelectionComponent.addClassToExcludeProductLabels('error');
    };

    clearError = () => {
        this.$multiSelectButton.removeClass('error');
    };

    onSubmitReportEvent = (e: any) => {
        this.$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
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
    const linkBuilderComponent = new LinkBuilderComponent($);
    const winLossViewNew = new WinLossViewNew(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new ReportTooltip($),
        new MonthPickerComponent($),
        linkBuilderComponent);
    winLossViewNew.init();
});