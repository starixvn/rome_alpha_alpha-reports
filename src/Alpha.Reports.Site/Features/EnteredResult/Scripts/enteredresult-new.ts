"use strict";

class EnterResultViewNew {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $tableReport: JQuery;
    $reportForm: JQuery;
    $isFilterVisible: JQuery;
    $userSelectedDateRangeOption: JQuery;
    $historicReport: JQuery;
    $isHistoricReport: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $submitButton: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    dateRangePickerComponent: DateRangePickerComponent;
    canSort: boolean;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
    }

    init = () => {
        if (!this.checkReady()) {
            return;
        }

        this.$.blockUI();
        this.dateRangePickerComponent.render();
        this.cacheDOMElements();

        this.canSort = parseInt(this.$("#tbl-enteredresult-report").data("totalrecords"), 0) > 1;
        this.initDataTable();
        this.registerEvents();
        this.$.unblockUI();
    };

    checkReady = () => {
        return this.$("#IsHistoricReport").length > 0;
    };

    cacheDOMElements = () => {
        this.$pageTitle = this.$(".page-title ");
        this.$reportForm = this.$("#report-form");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$historicReport = this.$("#historic-report");
        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$submitButton = this.$(".btn-submit");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
    };

    registerEvents = () => {
        this.$historicReport.on("click", this.onSubmitHistoricReportEvent);
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcelEvent);
        this.$reportForm.submit(this.onSubmitNormalReportEvent);
    };

    initDataTable = () => {
        const self = this;

        this.$tableReport = this.$("#tbl-enteredresult-report").DataTable({
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

                            return `<li data-dtr-index="${idx.column}">` +
                                `<span class="dtr-title">${header.text()}</span> ` +
                                `<span class="dtr-data">${cellData}</span></li>`;
                        }).toArray().join('');

                        const toDate = `<li><b>${self.$('#ToDate').val()}</b></li>`;

                        return data ?
                            self.$(`<ul data-dtr-index="${rowIdx}"/>`).append(toDate, data) :
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
            order: [[0, 'asc']],
            paging: false,
            searching: false,
            fixedHeader: true,
            processing: true
        });

        this.$('#tbl-enteredresult-report thead').on('click', 'th.control', event => this.collapsedExpandedAll(event));
        this.$tableReport.on('draw.dt', () => this.triggerRedrawTable());
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
    }

    doAdjustHeader = () => {
        this.$tableReport.fixedHeader.adjust();

        const scrollTo = this.$('.fixedHeader-floating thead').length > 0 ? this.$(document).height() : 0;
        this.$(window).scrollTop(scrollTo);
    }

    onFilterEvent = () => {
        let currentHideState = this.$reportForm.hasClass("hide");
        currentHideState ? this.showFilter() : this.hideFilter();
    };

    showFilter = () => {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.removeClass("hide");
    };

    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };

    onExportExcelEvent = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };

    onSubmitNormalReportEvent = () => {
        this.setHiddenValue();
    };

    onSubmitHistoricReportEvent = () => {
        this.$fromDate.val("");
        this.$toDate.val("");
        this.$isHistoricReport.val("true");
        this.setHiddenValue();
        this.$reportForm.trigger("submit");
    };

    setHiddenValue = () => {
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
        this.$.blockUI();
    };
}

$(function () {
    const enterResultView = new EnterResultViewNew(
        $,
        new DateRangePickerComponent($));
    enterResultView.init();
});