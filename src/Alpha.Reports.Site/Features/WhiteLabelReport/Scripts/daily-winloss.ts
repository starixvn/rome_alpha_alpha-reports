'use strict';
declare var whiteLabels: any;

class DailyWinLossView extends BaseReportView {
    private dateRangePicker: DateRangePickerJQueryComponent;
    private whiteLabelDropdown: MultipleDropdownComponent;
    private usernameAutoSuggestion: UsernameAutoSuggestionComponent;
    private $formReport: JQuery;
    private $tblReportContainer: JQuery;

    constructor(
        dateRangePicker: DateRangePickerJQueryComponent,
        whiteLabelDropdown: MultipleDropdownComponent,
        usernameAutoSuggestion: UsernameAutoSuggestionComponent) {
        super();
        this.dateRangePicker = dateRangePicker;
        this.whiteLabelDropdown = whiteLabelDropdown;
        this.usernameAutoSuggestion = usernameAutoSuggestion;
        // ser default sortIndicator
        this.sortColumnIndex = undefined
    }

    init() {
        this.cacheElements();
        this.initComponents();
        this.bindEventsAfterRenderWinLoss();
        this.$tblReportContainer.removeAttr('style');
    }

    private cacheElements() {
        this.$formReport = $('#report-form');
        this.$tblReportContainer = $('#tbl-report-container');
    }

    private initComponents() {
        const $iconExcel = $('#icon-excel');

        this.initShowHideFilter((isHideFilter) => {
            if (isHideFilter) {
                this.$formReport.hide();
                $iconExcel.hide();
            } else {
                this.$formReport.show();
                $iconExcel.show();
            }
        });
        this.dateRangePicker.render({
            onClosed: () => this.getReportData()
        });

        if (Array.isArray(whiteLabels)) {
            this.whiteLabelDropdown.render('dropdown-whitelabel', {
                listData: whiteLabels.map(function (wl) {
                    return {
                        text: wl.Name,
                        value: wl.Id,
                        checked: true
                    }
                }),
                onLeave: () => this.getReportData()
            });
        }

        this.usernameAutoSuggestion.init(new UsernameAutoSuggestionOption(
            $("#hiddenUsernameAutoSuggestionUrl").val(),
            false,
            false,
            () => this.getReportData()));

        const excelUrl = $iconExcel.attr('href');
        $iconExcel.off().tooltip();
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
        $.get(`${this.$formReport.attr('action')}?${queryString}`, (html) => {
            this.$tblReportContainer.html(html);
            this.bindEventsAfterRenderWinLoss();
            this.updateCurrentUrl(queryString);
        }).fail(alphaReports.handleAjaxFail).always($.unblockUI);
    }

    private getFormData() {
        let formData: any = this.dateRangePicker.getDateRangeValues();

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

    private bindEventsAfterRenderWinLoss() {
        const $tblReport = $('#tbl-report');
        this.tableSorter($tblReport);
        this.stickyTableHeaders($tblReport);
    }
}

$(function () {
    new DailyWinLossView(
        new DateRangePickerJQueryComponent(),
        new MultipleDropdownComponent(),
        new UsernameAutoSuggestionComponent()).init();
});
