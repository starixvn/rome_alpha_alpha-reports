'use strict';
declare var whiteLabels: any;

class DailyWinLossView extends BaseReportView {
    private dateRangePicker: DateRangePickerJQueryComponent;
    private whiteLabelDropdown: MultipleDropdownComponent;
    private usernameAutoSuggestion: UsernameAutoSuggestionComponent;
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
        this.$tblReportContainer = $('#tbl-report-container');
    }

    private initComponents() {
        const $iconExcel = $('#icon-excel');

        this.initShowHideFilter();
        this.dateRangePicker.render({
            onClosed: () => this.getReportData()
        });

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
        $.get(`${$('#hiddenGetDataUrl').val()}?${queryString}`, (html) => {
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
