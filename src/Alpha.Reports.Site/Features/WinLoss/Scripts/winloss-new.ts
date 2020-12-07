'use strict';

declare var
    columnName,
    columnSortName;

class WinLossViewNew extends BaseHistoricReport {
    $custLevelId: JQuery;
    $drilldownLinks: JQuery;
    $multiSelectButton: JQuery;

    monthPickerComponent: MonthPickerComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    reportTooltip: ReportTooltip;

    custLevelId: string;
    isHistoricReport = false;

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

        this.custLevelId = this.$custLevelId.val();

        this.linkBuilderComponent.render();
        this.bindFilter();
        this.initDataTable();
        this.initHelp();

        this.registerEvents();
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

        this.$custLevelId = this.$('#CustLevelId');
        this.$drilldownLinks = this.$('.downline-link');
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

    getColumnSortName = () => (columnSortName[this.custLevelId] || [])[this.$dataTable.order()[0][0]] || this.$tableReport.data('sortedcolumn');

    getColumnSortIndex = () => {
        const columnSortIndex = (columnSortName[this.custLevelId] || []).indexOf(this.$sortPropertyName.val());
        return columnSortIndex > -1 ? columnSortIndex : 0;
    };

    getColumnNameByCustLevel = (columnIndex: number, defaultColumnName: string) => (columnName[this.custLevelId] || [])[columnIndex] || defaultColumnName;

    getParams = () => {
        var selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            CustId: this.$tableReport.data('custid'),
            IsAnyProductSelected: selectedProducts.length > 0,
            IsAnyProductDetailSelected: this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
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

    showValidaProductDetailError = () => {
        this.productMultiSelectionComponent.addErrorToProductDetailComponent();
    };

    clearError = () => {
        this.$multiSelectButton.removeClass('error');
    };

    onSubmitReportEvent = (event: any) => {
        this.$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this.$.unblockUI();
            event.preventDefault();

            return;
        }

        if (!params.IsAnyProductDetailSelected) {
            this.showValidaProductDetailError();
            this.$.unblockUI();
            event.preventDefault();

            return;
        }

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
        this.$isHistoricReport.val(String(params.IsHistoricReport));
        this.$sortPropertyName.val(params.SortPropertyName);
        this.$isDescendingSort.val(String(params.IsDescendingSort));
    };
}

$(function () {
    const winLossViewNew = new WinLossViewNew(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)),
        new ReportTooltip($),
        new MonthPickerComponent($),
        new LinkBuilderComponent($));
    winLossViewNew.init();
});