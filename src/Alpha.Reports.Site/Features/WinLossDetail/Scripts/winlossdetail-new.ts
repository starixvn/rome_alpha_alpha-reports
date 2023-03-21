"use strict";

interface Document {
    webkitExitFullscreen(): void;
    msExitFullscreen(): void;
}

interface HTMLElement {
    webkitRequestFullscreen(): void;
    msRequestFullscreen(): void
}

class WinLossDetailViewNew {
    private $: JQueryStatic;
    private dateRangePickerComponent: DateRangePickerNewComponent;
    private weekRangeComponent: WeekRangeComponent
    private productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent;
    private hiddenCustId: JQuery;
    private hiddenCustLevel: number;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerNewComponent,
        weekRangeComponent: WeekRangeComponent,
        productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.weekRangeComponent = weekRangeComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
    }

    private openFullscreen() {
        const element = document.documentElement;

        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.webkitRequestFullscreen) { /* Safari */
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) { /* IE11 */
            element.msRequestFullscreen();
        }
    }

    private closeFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { /* Safari */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE11 */
            document.msExitFullscreen();
        }
    }

    init() {
        this.dateRangePickerComponent.render();
        this.productAndProductDetailDropdownComponent.render();

        this.cacheDOMElements();
        this.bindEvents();
    }

    private cacheDOMElements() {
        this.hiddenCustId = this.$("#hiddenCustId");
        this.hiddenCustLevel = +this.$("#hiddenCustLevel").val();
    }

    private bindEvents() {
        const reportForm = this.$("#report-form");
        const dataTable = $("#tbl-report").DataTable(this.buildDataTableOptions(reportForm.attr("action")));

        reportForm.submit((e) => {
            e.preventDefault();
            dataTable.ajax.reload();
            const formData = dataTable.ajax.param();
            this.weekRangeComponent.render(formData.fromDate, formData.toDate, function (fromDate, toDate) { });
        });

        this.$(".icon-filter").click(this.openFullscreen);
    }

    private buildDataTableOptions(winlossDetailUrl: string) {
        //const sortDefinitions = {
        //    CreatedDate: 1,
        //    WinningLosingAmount: 2,
        //    WinningThreshold: 3,
        //    LosingThreshold: 4
        //};
        const isMasterOrSuperLevel = this.isMasterOrSuperLevel();
        const isSuperLevel = this.isSuperLevel();
        const self = this;
        return {
            serverSide: true,
            ajax: {
                type: "POST",
                url: winlossDetailUrl,
                data: function (formData) {
                    const dateRange = self.dateRangePickerComponent.getDateRangeValues();
                    const productIds = self.productAndProductDetailDropdownComponent.getProductSelectedItems();
                    const resourceIds = self.productAndProductDetailDropdownComponent.getProductDetailSelectedItems();

                    formData.fromDate = dateRange.fromDate;
                    formData.toDate = dateRange.toDate;
                    formData.userSelectedProductIds = productIds;
                    formData.userSelectedResourceIds = resourceIds;
                },
                dataType: "json",
                dataSrc: "WinLossDetail.DataSource"
            },
            //ajax: (data, callback) => {
            //    //const columnName = data.columns[data.order[0].column].name;
            //    //const sortType = sortDefinitions[columnName];
            //    this.$.get(
            //        `${winlossDetailUrl}?${this.$.param(this.getFormData())}`,
            //        jsonData => callback({ data: jsonData.WinLossDetail.DataSource }));
            //},
            columns: this.buildDataTableColumns(isMasterOrSuperLevel, isSuperLevel),
            ordering: true,
            order: [0, "asc"],
            paging: false,
            searching: false,
            bInfo: false,
            dom: 'rt<"bottom"iflp<"clear">>',
            fixedHeader: true,
            processing: true,
            language: {
                zeroRecords: this.$("#hiddenNoInformation").val()
            },
            initComplete: function (_, json) {
                //self.updateTotalFooter(json, this.api());
                //if (json.WinLossDetail.TotalRecords) {
                //    const formatDecimal = $.fn.dataTable.render.number(",", ".", 2, "");

                //} else {
                //    console.dir(this.api().table().footer());
                //}
            },
            footerCallback: function (row, data, start, end, display) {
                console.log('footerCallback');
                self.updateTotalFooter(row, this.api(), isMasterOrSuperLevel, isSuperLevel);
            }
            //drawCallback: _ => tableReport.removeClass("hidden")
        };
    }

    private buildDataTableColumns(isMasterOrSuperLevel: boolean, isSuperLevel: boolean) {
        const formatInt = $.fn.dataTable.render.number(",", ".", 0, "").display;
        const formatDecimal = $.fn.dataTable.render.number(",", ".", 2, "").display;
        return [
            { data: "CustName" },
            { data: "BetCount", render: formatInt },
            { data: "Turnover", render: formatDecimal },
            { data: "NetTurnover", render: formatDecimal },
            { data: "GrossComm", render: formatDecimal },
            {
                data: null,
                render: (data, type, row) => type === "display"
                    ? this.renderGroupColumn(row.Turnover, row.NetTurnover, row.GrossComm, formatDecimal)
                    : data,
                visible: false
            },
            { data: "MemberNetWinLoss.WinLoss", render: formatDecimal },
            { data: "MemberNetWinLoss.Comm", render: formatDecimal },
            { data: "MemberNetWinLoss.Total", render: formatDecimal },
            {
                data: null,
                render: (data, type, row) => type === "display"
                    ? this.renderGroupColumn(row.MemberNetWinLoss.WinLoss, row.MemberNetWinLoss.Comm, row.MemberNetWinLoss.Total, formatDecimal)
                    : data,
                visible: false
            },
            { data: "AgentNetWinLoss.WinLoss", render: formatDecimal, className: "altercol" },
            { data: "AgentNetWinLoss.Comm", render: formatDecimal, className: "altercol" },
            { data: "AgentNetWinLoss.Total", render: formatDecimal, className: "altercol" },
            {
                data: null,
                render: (data, type, row) => type === "display"
                    ? this.renderGroupColumn(row.AgentNetWinLoss.WinLoss, row.AgentNetWinLoss.Comm, row.AgentNetWinLoss.Total, formatDecimal)
                    : data,
                className: "altercol",
                visible: false
            },
            { data: "MasterNetWinLoss.WinLoss", render: formatDecimal, visible: isMasterOrSuperLevel },
            { data: "MasterNetWinLoss.Comm", render: formatDecimal, visible: isMasterOrSuperLevel },
            { data: "MasterNetWinLoss.Total", render: formatDecimal, visible: isMasterOrSuperLevel },
            {
                data: null,
                render: (data, type, row) => type === "display"
                    ? this.renderGroupColumn(row.MasterNetWinLoss.WinLoss, row.MasterNetWinLoss.Comm, row.MasterNetWinLoss.Total, formatDecimal)
                    : data,
                visible: false
            },
            { data: "SuperNetWinLoss.WinLoss", render: formatDecimal, className: "altercol", visible: isSuperLevel },
            { data: "SuperNetWinLoss.Comm", render: formatDecimal, className: "altercol", visible: isSuperLevel },
            { data: "SuperNetWinLoss.Total", render: formatDecimal, className: "altercol", visible: isSuperLevel },
            {
                data: null,
                render: (data, type, row) => type === "display"
                    ? this.renderGroupColumn(row.SuperNetWinLoss.WinLoss, row.SuperNetWinLoss.Comm, row.SuperNetWinLoss.Total, formatDecimal)
                    : data,
                className: "altercol",
                visible: false
            },
            { data: "Company", render: formatDecimal },
        ];
    }

    private updateTotalFooter(row, api, isMasterOrSuperLevel: boolean, isSuperLevel: boolean) {
        const winLossDetail = api.ajax.json().WinLossDetail;
        if (winLossDetail.TotalRecords) {
            const formatInt = $.fn.dataTable.render.number(",", ".", 0, "").display;
            const formatDecimal = $.fn.dataTable.render.number(",", ".", 2, "").display;
            const total = winLossDetail.TotalData;

            this.$(api.column(1).footer()).html(formatInt(total.BetCount));
            this.$(api.column(2).footer()).html(formatDecimal(total.Turnover));
            this.$(api.column(3).footer()).html(formatDecimal(total.NetTurnover));
            this.$(api.column(4).footer()).html(formatDecimal(total.GrossComm));
            this.$(api.column(5).footer()).html(this.renderGroupColumn(total.Turnover, total.NetTurnover, total.GrossComm, formatDecimal));
            this.$(api.column(6).footer()).html(formatDecimal(total.MemberNetWinLoss.WinLoss));
            this.$(api.column(7).footer()).html(formatDecimal(total.MemberNetWinLoss.Comm));
            this.$(api.column(8).footer()).html(formatDecimal(total.MemberNetWinLoss.Total));
            this.$(api.column(9).footer()).html(this.renderGroupColumn(total.MemberNetWinLoss.WinLoss, total.MemberNetWinLoss.Comm, total.MemberNetWinLoss.Total, formatDecimal));
            this.$(api.column(10).footer()).html(formatDecimal(total.AgentNetWinLoss.WinLoss));
            this.$(api.column(11).footer()).html(formatDecimal(total.AgentNetWinLoss.Comm));
            this.$(api.column(12).footer()).html(formatDecimal(total.AgentNetWinLoss.Total));
            this.$(api.column(13).footer()).html(this.renderGroupColumn(total.AgentNetWinLoss.WinLoss, total.AgentNetWinLoss.Comm, total.AgentNetWinLoss.Total, formatDecimal));

            if (isMasterOrSuperLevel) {
                this.$(api.column(14).footer()).html(formatDecimal(total.MasterNetWinLoss.WinLoss));
                this.$(api.column(15).footer()).html(formatDecimal(total.MasterNetWinLoss.Comm));
                this.$(api.column(16).footer()).html(formatDecimal(total.MasterNetWinLoss.Total));
                this.$(api.column(17).footer()).html(this.renderGroupColumn(total.MasterNetWinLoss.WinLoss, total.MasterNetWinLoss.Comm, total.MasterNetWinLoss.Total, formatDecimal));
            }
            
            if (isSuperLevel) {
                this.$(api.column(18).footer()).html(formatDecimal(total.SuperNetWinLoss.WinLoss));
                this.$(api.column(19).footer()).html(formatDecimal(total.SuperNetWinLoss.Comm));
                this.$(api.column(20).footer()).html(formatDecimal(total.SuperNetWinLoss.Total));
                this.$(api.column(21).footer()).html(this.renderGroupColumn(total.SuperNetWinLoss.WinLoss, total.SuperNetWinLoss.Comm, total.SuperNetWinLoss.Total, formatDecimal));
            }

            this.$(api.column(22).footer()).html(formatDecimal(total.Company));
        }
    }

    private renderGroupColumn(number1: number, number2: number, number3: number, formatDecimal) {
        return `<div>${formatDecimal(number1)}</div><div>${formatDecimal(number2)}</div><div>${formatDecimal(number3)}</div>`;
    }

    private isMasterOrSuperLevel() {
        return this.hiddenCustLevel >= 3;
    }

    private isSuperLevel() {
        return this.hiddenCustLevel === 4;
    }

    //private getFormData() {
    //    const dateRange = this.dateRangePickerComponent.getDateRangeValues();
    //    const productIds = this.productAndProductDetailDropdownComponent.getProductSelectedItems();
    //    const resourceIds = this.productAndProductDetailDropdownComponent.getProductDetailSelectedItems();

    //    return this.$.extend({
    //        custId: this.hiddenCustId.val(),
    //        userSelectedProductIds: productIds,
    //        UserSelectedResourceIds: resourceIds
    //    }, dateRange);
    //}
}

$(function () {
    new WinLossDetailViewNew(
        $,
        new DateRangePickerNewComponent($),
        new WeekRangeComponent($, moment),
        new ProductAndProductDetailDropdownComponent($)).init();
});