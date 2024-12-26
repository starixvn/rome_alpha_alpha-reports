'use strict';
declare var whiteLabels: any;

class SettlementView extends BaseReportView {
    private monthRangePicker: MonthRangePickerComponent;
    private whiteLabelDropdown: MultipleDropdownComponent;
    private usernameAutoSuggestion: UsernameAutoSuggestionComponent;
    private visibleColumns: SettlementColumns;
    private $tblReportContainer: JQuery;

    constructor(
        monthRangePicker: MonthRangePickerComponent,
        whiteLabelDropdown: MultipleDropdownComponent,
        usernameAutoSuggestion: UsernameAutoSuggestionComponent,
        visibleColumns: SettlementColumns) {
        super();
        this.monthRangePicker = monthRangePicker;
        this.whiteLabelDropdown = whiteLabelDropdown;
        this.usernameAutoSuggestion = usernameAutoSuggestion;
        this.visibleColumns = visibleColumns;
        // default no sort
        this.sortColumnIndex = undefined;
    }

    init() {
        this.$tblReportContainer = $('#tbl-report-container');

        this.initComponents();
        this.bindEventsAfterRenderReportData();
        this.$tblReportContainer.removeAttr('style');
    }

    private initComponents() {
        this.initShowHideFilter((isHideFilter) => $('#settlement-note').toggle(!isHideFilter));
        this.monthRangePicker.render(new MonthRangePickerOption(undefined, undefined, undefined, () => this.getReportData()));

        if (Array.isArray(whiteLabels)) {
            let maxWhiteLabelLength = 0;
            const listData = whiteLabels.map(function (wl) {
                if (wl.Name.length > maxWhiteLabelLength) {
                    maxWhiteLabelLength = wl.Name.length;
                }

                return {
                    text: wl.Name,
                    value: wl.Id,
                    checked: true
                }
            });
            maxWhiteLabelLength *= 11;

            this.whiteLabelDropdown.render('dropdown-whitelabel', {
                showFilter: true,
                listData: listData,
                onLeave: () => this.getReportData(),
                minWidth: maxWhiteLabelLength > 220 ? maxWhiteLabelLength : 220
            });
        }

        this.usernameAutoSuggestion.init(new UsernameAutoSuggestionOption(
            $("#hiddenUsernameAutoSuggestionUrl").val(),
            false,
            false,
            () => this.getReportData()));

        this.visibleColumns.init();

        const $iconExcel = $('#icon-excel').tooltip();
        const excelUrl = $iconExcel.attr('href');
        $iconExcel.click(() => {
            $iconExcel.attr('href', `${excelUrl}?${$.param(this.getFormData())}`);
            setTimeout(function () {
                $iconExcel.attr('href', 'javascript:;');
            }, 1000);
        }).attr('href', 'javascript:;');
    }

    private getReportData() {
        $.blockUI();
        const queryString = $.param(this.getFormData());
        $.get(`${$('#hiddenGetDataUrl').val()}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.bindEventsAfterRenderReportData();
            this.updateCurrentUrl(queryString);
        }).fail(alphaReports.handleAjaxFail).always($.unblockUI);
    }

    private getFormData() {
        let formData: any = this.monthRangePicker.getDateRangeValues();

        const whiteLabels = this.whiteLabelDropdown.getCheckedValues();
        if (whiteLabels) {
            formData.whiteLabels = whiteLabels;
        }

        const username = this.usernameAutoSuggestion.getUsername();
        if (username) {
            formData.username = username;
        }

        return formData;
    }

    private bindEventsAfterRenderReportData() {
        const $tblReport = $('#tbl-report');
        this.tableSorter($tblReport);
        this.visibleColumns.showHideColumns();
        this.stickyTableHeaders($tblReport);
    }
}

$(function () {
    new SettlementView(
        new MonthRangePickerComponent($),
        new MultipleDropdownComponent(),
        new UsernameAutoSuggestionComponent(),
        new SettlementColumns(new VisibleColumnDropdownComponent())).init();
});
