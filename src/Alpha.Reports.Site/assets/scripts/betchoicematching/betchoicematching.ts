/// <reference path="../core.ts" />

"use strict";

interface JQuery {
    datepicker(option: any): void;
}

interface QueryModelItem {
    Username: string;
    TransDate: string;
    TransId: number;
    Odds: number;
    IP: string;
    MemberTurnover: number;
    MemberWinLoss: number;
    SuperWinLoss: number;
    Company: number;
}

interface QueryModel {
    Items: Array<QueryModelItem>;
}

class BetChoiceMatching {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $datePicker: JQuery;
    $datePickerInput: JQuery;
    $betCountDropdown: JQuery;
    $table: JQuery;
    $tableTbody: JQuery;
    $tableTfoot: JQuery;
    $noinfoRowHtml: string;

    constructor(
        $: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.$.blockUI();
        this.cacheElements();
        this.registerEvents();
        this.$table.hide();
        this.$table.removeClass('hide');
        this.$.unblockUI();
    };

    cacheElements = () => {
        this.$pageTitle = this.$(".page-title");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#WinlostDate");
        this.$betCountDropdown = this.$("#betCount");
        this.$table = this.$("#tbl-report");
        this.$tableTbody = this.$table.find("tbody");
        this.$tableTfoot = this.$table.find("tfoot");
        const $noinfoRow = this.$table.find("noinfo-row");
        this.$noinfoRowHtml = $noinfoRow.html();
    };

    registerEvents = () => {
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$datePicker.datepicker({
            allowInputToggle: true,
            minDate: new Date(this.$('#MinDate').val()),
            maxDate: new Date(this.$('#Today').val()),
            format: alphaReports.format.shortDateWithSlash_DatePicker
        });
    };

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

    onSubmitReport = (e: any) => {
        e.preventDefault();
        let this$ = this.$;
        this$.blockUI();
        this.$table.hide();
        const action = this.$reportForm.prop('action');
        const params = this.getParams();
        this$.post(action, params)
            .done(data => {
                this.renderTable(data);
            })
            .fail(() => {
                alert('error');
            })
            .always(() => {
                this.$table.show();
                this$.unblockUI();
            });
    };

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };

    getParams = () => {
        return {
            winlostDate: this.$datePickerInput.val(),
            betCount: this.$betCountDropdown.val()
        };
    };

    renderTable = (data: QueryModel) => {
        if (data.Items.length === 0) {
            this.$tableTbody.html(this.$noinfoRowHtml);
            this.$tableTfoot.hide();
            return;
        }

        const html = [];
        data.Items.forEach(item => {
            html.push(`<tr> \
                <td class="text-left">${item.Username}</td>
                <td class="text-center">${item.TransDate}</td>
                <td class="text-center">${item.TransId}</td>
                <td class="text-center">${item.Odds}</td>
                <td class="text-center">${item.IP}</td>
                <td>${item.MemberTurnover}</td>
                <td>${item.MemberWinLoss}</td>
                <td>${item.SuperWinLoss}</td>
                <td>${item.Company}</td>
            </tr>`);
        });
        this.$tableTbody.html(html.join(''));
        this.$tableTfoot.show();
    };
}

$(() => {
    const betChoiceMatching = new BetChoiceMatching(
        $);

    betChoiceMatching.init();
});
