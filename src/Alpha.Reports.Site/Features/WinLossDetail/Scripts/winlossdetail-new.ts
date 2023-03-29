"use strict";

interface Document {
    webkitExitFullscreen(): void;
    msExitFullscreen(): void;
}

interface HTMLElement {
    webkitRequestFullscreen(): void;
    msRequestFullscreen(): void;
}

interface Object {
    entries: any;
}

class WinLossDetailViewNew {
    private $: JQueryStatic;
    private totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent;
    private dateRangePickerComponent: DateRangePickerJQueryComponent;
    private productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent;
    private visibleColumnDropdownComponent: VisibleColumnDropdownComponent;
    private weekRangeComponent: WeekRangeComponent
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private viewModel: WinLossDetailNewViewModel;
    private globalFormData: any;
    private globalTotalData: any;
    private tableJQuery: JQuery;
    private tableFooterJQuery: JQuery;
    private dataTableObject: any;
    private visibleColumns: any;
    private masterColumnIndexes: any = [11, 12, 13];
    private superColumnIndexes: any = [14, 15, 16];

    constructor(
        $: JQueryStatic,
        totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent,
        dateRangePickerComponent: DateRangePickerJQueryComponent,
        productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent,
        visibleColumnDropdownComponent: VisibleColumnDropdownComponent,
        weekRangeComponent: WeekRangeComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        viewModel: WinLossDetailNewViewModel) {
        this.$ = $;
        this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        this.weekRangeComponent = weekRangeComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.viewModel = viewModel;
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
        this.tableJQuery = this.$("#tbl-report");
        this.tableFooterJQuery = this.tableJQuery.find("#tbl-footer");

        this.viewModel.init();
        this.globalFormData = {
            custId: this.viewModel.ViewerId,
            custName: this.viewModel.ViewerName,
            custLevelId: this.viewModel.ViewerLevel
        };

        this.renderComponents();
        this.bindEvents();

        this.getWinLossDetailDataAndRenderWeekRange();
    }

    private renderComponents() {
        this.totalEnteredNonEnteredComponent.init(new TotalEnteredNonEnteredOption(
            this.viewModel.NonEnteredText,
            this.viewModel.EnteredText
        ));

        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(() => this.getWinLossDetailDataAndRenderWeekRange()));

        this.productAndProductDetailDropdownComponent.render(
            new ProductAndProductDetailDropdownOption(() => this.getWinLossDetailDataAndRenderWeekRange()));

        this.visibleColumns = this.visibleColumnDropdownComponent.render(new VisibleColumnDropdownOption(
            `WinLossDetail_${this.viewModel.ViewerId}_${this.viewModel.SubAccId}:Columns`,
            (columns) => {
                this.visibleColumns = columns;
                for (const [key, value] of Object.entries(columns)) {
                    let visible = value;
                    const keyInt = +key;

                    if (this.superColumnIndexes.find(i => i === keyInt)) {
                        visible = visible && this.getVisibleSuperColumn();
                    } else if (this.masterColumnIndexes.find(i => i === keyInt)) {
                        visible = visible && this.getVisibleMasterColumn();
                    }

                    this.dataTableObject.column(key).visible(visible);
                }
            }));

        this.weekRangeComponent.init(new WeekRangeOption((fromDate, toDate) => {
            this.globalFormData.fromDate = fromDate;
            this.globalFormData.toDate = toDate;
            this.getWinLossDetailData(this.globalFormData);
        }));
    }

    private bindEvents() {
        this.$("#icon-report-legend").click(() =>
            top.window["popupManager"].open(
                this.viewModel.LegendUrl,
                undefined,
                800,
                540,
                "modal-mobile-friendly"));

        this.$(".icon-filter").click(this.openFullscreen);
    }

    private buildDataTableOptions(dataSet) {
        const self = this;
        return {
            data: dataSet,
            columns: this.buildDataTableColumns(),
            ordering: true,
            order: [0, "asc"],
            paging: false,
            searching: false,
            bInfo: false,
            dom: '<"#dataTable_toolbar">t<"bottom"ilp<"clear">>',
            fixedHeader: true,
            language: {
                zeroRecords: this.viewModel.NoInformationText
            },
            initComplete: () => {
                this.buildDataTableToolbar();
                this.tableJQuery.removeClass("hide");
            },
            footerCallback: function () {
                self.updateTotalFooter(this.api());
            },
            drawCallback: () => this.bindDrilldownLinks()
        };
    }

    private buildDataTableColumns() {
        let columnIndex = 1; // Start at BetCount
        const columns = [];
        columns.push(
            {
                data: "CustName",
                className: "text-left",
                render: (data, type, row) => {
                    if (type === "display") {
                        if (this.isAgentView()) {
                            if (this.viewModel.CanViewBetLists) {
                                return `<a class="betlists-link" href="javascript:;" data-custid="${row.CustId}" data-custname="${data}">${data}</a>`;
                            }

                            return data;
                        }

                        const custLevelId = this.globalFormData.custLevelId - 1;
                        return `<a class="downline-link" href="javascript:;" data-custid="${row.CustId}" data-custname="${data}" data-custlevelid="${custLevelId}">${data}</a>`;
                    }

                    return data;
                }
            },
            { data: "BetCount", render: this.formatInt, visible: this.visibleColumns[columnIndex++] },
            { data: "Turnover", render: this.formatDecimal, visible: this.visibleColumns[columnIndex++] },
            { data: "NetTurnover", render: this.formatDecimal, visible: this.visibleColumns[columnIndex++] },
            { data: "GrossComm", render: this.formatDecimal, visible: this.visibleColumns[columnIndex++] },
            { data: "MemberNetWinLoss.WinLoss", render: this.formatDecimal, visible: this.visibleColumns[columnIndex++] },
            { data: "MemberNetWinLoss.Comm", render: this.formatDecimal, visible: this.visibleColumns[columnIndex++] },
            { data: "MemberNetWinLoss.Total", render: this.formatDecimal, className: "bold", visible: this.visibleColumns[columnIndex++] },
            { data: "AgentNetWinLoss.WinLoss", render: this.formatDecimal, className: "altercol", visible: this.visibleColumns[columnIndex++] },
            { data: "AgentNetWinLoss.Comm", render: this.formatDecimal, className: "altercol", visible: this.visibleColumns[columnIndex++] },
            { data: "AgentNetWinLoss.Total", render: this.formatDecimal, className: "bold altercol", visible: this.visibleColumns[columnIndex++] });

        this.viewModel.ViewerLevelIsMasterOrSuper && columns.push(
            { data: "MasterNetWinLoss.WinLoss", render: this.formatDecimal, defaultContent: "", visible: this.visibleColumns[columnIndex++] },
            { data: "MasterNetWinLoss.Comm", render: this.formatDecimal, defaultContent: "", visible: this.visibleColumns[columnIndex++] },
            { data: "MasterNetWinLoss.Total", render: this.formatDecimal, className: "bold", defaultContent: "", visible: this.visibleColumns[columnIndex++] }
        );

        this.viewModel.ViewerLevelIsSuper && columns.push(
            { data: "SuperNetWinLoss.WinLoss", render: this.formatDecimal, className: "altercol", defaultContent: "", visible: this.visibleColumns[columnIndex++] },
            { data: "SuperNetWinLoss.Comm", render: this.formatDecimal, className: "altercol", defaultContent: "", visible: this.visibleColumns[columnIndex++] },
            { data: "SuperNetWinLoss.Total", render: this.formatDecimal, className: "bold altercol", defaultContent: "", visible: this.visibleColumns[columnIndex++] }
        );

        columns.push({ data: "Company", render: this.formatDecimal, visible: this.visibleColumns[columnIndex++] });

        return columns;
    }

    private buildDataTableToolbar() {
        this.$("#dataTable_toolbar").html([
            '<div class="breadcrumb-table">',
            '<div class="breadcrumb-container">',
            '<ul class="breadcrumb hide" id="breadcrumb"></ul>',
            '</div>',
            '<div class="list-icon">',
            '<ul><li class="normalview-element"><span id="icon-excel" class="icon-excel" title="',
            this.viewModel.ExportToExcelText,
            '"></span></li>',
            '</ul></div>',
            '</div>'
        ].join(""));

        this.breadcrumbLevelComponent.init(
            new BreadcrumbLevelOption((custId: number, custName: string, custLevelId: number) => {
                this.globalFormData.custId = custId;
                this.globalFormData.custName = custName;
                this.globalFormData.custLevelId = custLevelId;
                this.getWinLossDetailData(
                    this.globalFormData,
                    () => {
                        this.breadcrumbLevelComponent.removeBreadcrumbLevel(custLevelId);
                        this.setVisibleColumns();
                    });
            }),
            this.viewModel.ViewerId,
            this.viewModel.ViewerName,
            this.viewModel.ViewerLevel);

        this.$("#icon-excel").click(() => {
            const exportExcelForm = this.$("#export-excel-form");
            exportExcelForm.find("#CustId").val(this.globalFormData.custId);
            exportExcelForm.find("#CustName").val(this.globalFormData.custName);
            exportExcelForm.find("#CustLevelId").val(this.globalFormData.custLevelId);
            exportExcelForm.find("#FromDate").val(this.globalFormData.fromDate);
            exportExcelForm.find("#ToDate").val(this.globalFormData.toDate);
            exportExcelForm.find("#ProductIds").val(this.globalFormData.productIds);
            exportExcelForm.find("#ResourceIds").val(this.globalFormData.resourceIds);
            //exportExcelForm.find("#IsHistoricReport").val(this.globalFormData.custLevelId);
            exportExcelForm.submit();
        });
    }

    private bindDrilldownLinks() {
        this.$(".betlists-link").off().click((e) => {
            const $this = this.$(e.target);
            const custId = $this.data("custid");
            const custName = $this.data("custname");
            const betListsParams = this.$.extend({}, this.globalFormData, { custId, custName });
            $this.attr("href", `${this.viewModel.BetListsUrl}?${this.$.param(betListsParams)}`);
        });

        this.$(".downline-link").off().click((e) => {
            e.preventDefault();
            const $this = this.$(e.target);
            const custId = $this.data("custid");
            const custName = $this.data("custname");
            const custLevelId = $this.data("custlevelid");

            this.globalFormData.custId = custId;
            this.globalFormData.custName = custName;
            this.globalFormData.custLevelId = custLevelId

            this.getWinLossDetailData(
                this.globalFormData,
                () => {
                    this.breadcrumbLevelComponent.addBreadcrumbLevel(custId, custName, custLevelId);
                    this.setVisibleColumns();
                });
        });
    }

    private updateTotalFooter(api) {
        let columnIndex = 1; // Start at Bet Count
        this.$(api.column(columnIndex++).footer()).html(this.formatInt(this.globalTotalData.BetCount));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.Turnover));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.NetTurnover));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.GrossComm));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.MemberNetWinLoss.WinLoss));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.MemberNetWinLoss.Comm));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.MemberNetWinLoss.Total));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.AgentNetWinLoss.WinLoss));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.AgentNetWinLoss.Comm));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(this.globalTotalData.AgentNetWinLoss.Total));

        if (this.viewModel.ViewerLevelIsMasterOrSuper) {
            columnIndex = this.updateTotalFooterMasterSuper(api, this.globalTotalData.MasterNetWinLoss, columnIndex);
        }

        if (this.viewModel.ViewerLevelIsSuper) {
            columnIndex = this.updateTotalFooterMasterSuper(api, this.globalTotalData.SuperNetWinLoss, columnIndex);
        }

        this.$(api.column(columnIndex).footer()).html(this.formatDecimal(this.globalTotalData.Company));
    }

    private updateTotalFooterMasterSuper(api: any, sm: any, columnIndex: number) {
        let winLoss = "";
        let comm = "";
        let total = "";
        if (sm) {
            winLoss = this.formatDecimal(sm.WinLoss);
            comm = this.formatDecimal(sm.Comm);
            total = this.formatDecimal(sm.Total);
        }

        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(winLoss));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(comm));
        this.$(api.column(columnIndex++).footer()).html(this.formatDecimal(total));

        return columnIndex;
    }

    private isAgentView() {
        return this.globalFormData.custLevelId === CustomerLevel.Agent;
    }

    private getWinLossDetailDataAndRenderWeekRange() {
        this.globalFormData.productIds = this.productAndProductDetailDropdownComponent.getProductSelectedItems();
        this.globalFormData.resourceIds = this.productAndProductDetailDropdownComponent.getProductDetailSelectedItems();
        this.$.extend(this.globalFormData, this.dateRangePickerComponent.getDateRangeValues());

        this.getWinLossDetailData(this.globalFormData, () => this.weekRangeComponent.render(this.globalFormData));
    }

    private getWinLossDetailData(formData: any, callback = Function()) {
        this.tableJQuery.block();
        this.$.get(`${this.viewModel.WinLossDetailUrl}?${this.$.param(formData)}`, (data) => {
            callback();
            this.renderWinLossDetailReport(data);
            this.tableJQuery.unblock();
        }).fail((jqXHR) => {
            this.tableJQuery.unblock();
            if (jqXHR.status === 429) {
                window.location.href = window.top["alpha"].urls.mainRootPath + "error/blocked";
            }
        });
    }

    private renderWinLossDetailReport(data) {
        const winlossDetail = data.WinLossDetail;
        this.globalTotalData = winlossDetail.TotalData;

        this.totalEnteredNonEnteredComponent.render(data.TotalEnteredSport);

        if (this.dataTableObject) {
            this.dataTableObject.clear();
            this.dataTableObject.rows.add(winlossDetail.DataSource).draw();
        } else {
            this.dataTableObject = this.tableJQuery.DataTable(this.buildDataTableOptions(winlossDetail.DataSource));
        }

        const hasData = winlossDetail.TotalRecords > 0;
        if (hasData) {
            this.tableFooterJQuery.removeClass("hide");
            winlossDetail.TotalRecords > 1
                ? this.tableJQuery.addClass("tablesorter")
                : this.tableJQuery.removeClass("tablesorter");
        } else {
            this.tableFooterJQuery.addClass("hide");
            this.tableJQuery.removeClass("tablesorter");
        }
    }

    private formatInt(value) {
        return $.fn.dataTable.render.number(",", ".", 0).display(value);
    }

    private formatDecimal(value) {
        const text = $.fn.dataTable.render.number(",", ".", 2).display(Math.round(Math.abs(value) * 100) / 100);
        return value >= 0 ? text : "-" + text;
    }

    private setVisibleColumns() {
        const visibleSuper = this.getVisibleSuperColumn();
        if (typeof visibleSuper !== "undefined") {
            this.superColumnIndexes.forEach(item =>
                this.dataTableObject.column(item).visible(visibleSuper && this.visibleColumns[item]));
        }

        const visibleMaster = this.getVisibleMasterColumn();
        if (typeof visibleMaster !== "undefined") {
            this.masterColumnIndexes.forEach(item =>
                this.dataTableObject.column(item).visible(visibleMaster && this.visibleColumns[item]));
        }
    }

    private getVisibleSuperColumn() {
        if (this.viewModel.ViewerLevelIsSuper) {
            return this.globalFormData.custLevelId === this.viewModel.ViewerLevel;
        }
    }

    private getVisibleMasterColumn() {
        if (this.viewModel.ViewerLevelIsMasterOrSuper) {
            return this.globalFormData.custLevelId >= CustomerLevel.Master;
        }
    }
}

$(function () {
    new WinLossDetailViewNew(
        $,
        new TotalEnteredNonEnteredComponent($, moment),
        new DateRangePickerJQueryComponent($),
        new ProductAndProductDetailDropdownComponent($),
        new VisibleColumnDropdownComponent($),
        new WeekRangeComponent($, moment),
        new BreadcrumbLevelComponent($),
        new WinLossDetailNewViewModel($)).init();
});