"use strict";

interface Window {
    productList: any,
    productDetailList: any,
    indexManager: any
}

interface JQuery {
    tablesorter(option: any): JQuery;
}

class WinLossDetailViewNew extends BaseReportView {
    private totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent;
    private dateRangePickerComponent: DateRangePickerJQueryComponent;
    private productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent;
    private visibleColumnDropdownComponent: VisibleColumnDropdownComponent;
    private weekRangeComponent: WeekRangeComponent
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private tableReportContainer: JQuery;
    private tableReport: JQuery;
    private winLossDetailUrl: string
    private productList: any;
    private productDetailList: any;
    private globalFormData: any;
    private sortColumnName = "username";
    private sortIndicator = 0; // asc: 0, desc: 1

    constructor(
        totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent,
        dateRangePickerComponent: DateRangePickerJQueryComponent,
        productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent,
        visibleColumnDropdownComponent: VisibleColumnDropdownComponent,
        weekRangeComponent: WeekRangeComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        productList: any,
        productDetailList: any) {
        super();
        this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        this.weekRangeComponent = weekRangeComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.productList = productList;
        this.productDetailList = productDetailList;
    }

    init() {
        this.tableReportContainer = $("#tbl-report-container");
        this.winLossDetailUrl = $("#hiddenWinLossDetailUrl").val();

        this.renderComponents();
        this.bindEvents();

        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.tableReportContainer.removeAttr("style");
    }

    private renderComponents() {
        const viewerId = +$("#hiddenViewerId").val();
        const viewerLevel = +$("#hiddenViewerLevel").val();
        const weekFromDate = $("#week-from-date");
        const weekToDate = $("#week-to-date");

        this.totalEnteredNonEnteredComponent.init();

        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption((dateRange) => {
            $.extend(this.globalFormData, dateRange);
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.getWinLossDetailDataAndRenderWeekRange();
        }));

        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(
            this.productList,
            this.productDetailList,
            (productsAndResources) => {
                $.extend(this.globalFormData, this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.getWinLossDetailDataAndRenderWeekRange();
            }));

        this.visibleColumnDropdownComponent.render(new VisibleColumnDropdownOption(
            `WinLossDetail_${viewerId}_${$("#hiddenSubAccId").val()}:Columns`,
            this.getListColumn(viewerLevel),
            (visibleColumn) => this.showHideColumns(visibleColumn)));

        const dateRange = this.dateRangePickerComponent.getDateRangeValues();
        this.weekRangeComponent.init(new WeekRangeOption((dateRange) => {
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.addIsWeekSelectionForStatistic();

            $.extend(this.globalFormData, dateRange);
            this.getWinLossDetailData();
        }, this.getMomentLocale()));
        const activeWeekRange = weekFromDate.length > 0 && weekToDate.length > 0
            ? { weekFromDate: weekFromDate.val(), weekToDate: weekToDate.val() }
            : undefined;
        this.weekRangeComponent.render(dateRange, activeWeekRange);

        const breadcrumbLevelJson = $("#hiddenBreadcrumbLevelJson").val();
        const breadcrumbs = breadcrumbLevelJson.length
            ? JSON.parse(breadcrumbLevelJson)
            : [{ custId: viewerId, custName: $("#hiddenViewerName").val(), custLevelId: viewerLevel }];
        this.breadcrumbLevelComponent.init(new BreadcrumbLevelOption(
            breadcrumbs,
            (breadcrumbData) => {
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                $.extend(this.globalFormData, breadcrumbData);
                this.getWinLossDetailData(() => {
                    this.breadcrumbLevelComponent.removeBreadcrumbLevel(breadcrumbData.custLevelId);
                    this.updateCurrentUrl();
                });
            }));

        this.globalFormData = $.extend({}, {
            custId: +$("#hiddenCustId").val(),
            custName: $("#hiddenCustName").val(),
            custLevelId: +$("#hiddenCustLevel").val()
        }, dateRange, this.productAndProductDetailDropdownComponent.getCheckedValues());
        // update fromDate/toDate of globalFormData if activeWeekRange is NOT undefined
        if (activeWeekRange) {
            this.globalFormData.fromDate = activeWeekRange.weekFromDate;
            this.globalFormData.toDate = activeWeekRange.weekToDate;
        }
    }

    private getMomentLocale() {
        const locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };

        return locale[window.top.$("#language").data("currentlanguage")] || locale.default;
    }

    private getWinLossDetailDataAndRenderWeekRange() {
        this.getWinLossDetailData(() => {
            this.weekRangeComponent.render(this.globalFormData, undefined);
            this.updateCurrentUrl();
        });
    }

    private getWinLossDetailData(callback = Function()) {
        this.addMoreParamsForStatistic();

        this.tableReportContainer.block();
        $.get(`${this.winLossDetailUrl}?${$.param(this.globalFormData)}`, (html) => {
            callback();
            this.tableReportContainer.html(html);
            this.bindTableReportEvents();
            this.totalEnteredNonEnteredComponent.update(
                { eventDate: this.tableReport.data("yesterday"), nonEntered: this.tableReport.data("non-entered-yesterday") },
                { eventDate: this.tableReport.data("today"), nonEntered: this.tableReport.data("non-entered-today") });
            this.tableReportContainer.unblock();
        }).fail((jqXHR) => {
            this.tableReportContainer.unblock();
            if (jqXHR.status === 429) {
                window.location.href = window.top["alpha"].urls.mainRootPath + "error/blocked";
            } else if (jqXHR.status === 500 && jqXHR.responseText) {
                const responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    window.location.href = window.top["alpha"].urls.mainRootPath + "partialum/index?" + $.param(responseObject);
                }
            }
        }).always(() => this.removeIsWeekSelectionForStatistic());
    }

    private isInvalidGlobalFormData() {
        return this.globalFormData.productIds.length === 0;
    }

    private bindEvents() {
        this.initReportLegend($("#hiddenLegendUrl").val(), 800, 540);

        $("#icon-excel").click((e) => {
            if (this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }

            const $this = $(e.target);
            $this.attr("href", `${$this.data("url")}?${$.param(this.globalFormData)}`);
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });

        this.bindTableReportEvents();
    }

    private bindTableReportEvents() {
        this.tableReport = this.tableReportContainer.find("#tbl-report");
        this.tableReport.find('[data-toggle="tooltip"]').tooltip();

        this.tableReport.find(".betlists-link").off().click((e) => {
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.tableReport.block();
            const $this = $(e.target);
            const custId = $this.data("custid");
            const custName = $this.data("custname");
            const breadcrumbs = this.breadcrumbLevelComponent.getBreadcrumbs();
            breadcrumbs.push({ custId, custName, custLevelId: 1 });
            const breadcrumbLevelJson = JSON.stringify(breadcrumbs);
            const dateRange = this.dateRangePickerComponent.getDateRangeValues();
            let betListsParams = this.weekRangeComponent.getActiveWeekRange();
            $.extend(betListsParams, this.globalFormData, dateRange, { custId, custName, breadcrumbLevelJson });
            $this.attr("href", `${$("#hiddenBetListsUrl").val()}?${$.param(betListsParams)}`);
        });

        this.tableReport.find(".downline-link").off().click((e) => {
            e.preventDefault();
            if (this.isInvalidGlobalFormData()) {
                return;
            }

            const $this = $(e.target);
            const breadcrumbData = {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: $this.data("custlevelid")
            };
            $.extend(this.globalFormData, breadcrumbData);

            this.getWinLossDetailData(() => {
                this.breadcrumbLevelComponent.addBreadcrumbLevel(breadcrumbData);
                this.updateCurrentUrl();
            });
        });

        this.initTableSorter();
        this.showHideColumns(this.visibleColumnDropdownComponent.getColumns());
        this.tableReport.stickyTableHeaders();
    }

    private initTableSorter() {
        const sortColumns = this.tableReport.find(".sorting");

        if (this.tableReport.data("cansort")) {
            const headers = {};
            sortColumns.each(function (index) {
                headers[index] = { sorter: index === 0 || index === 1 ? "text" : "digit" };
            });

            this.tableReport.tablesorter({
                headers: headers,
                sortList: [[this.mapSortColumnNameToIndex(), this.sortIndicator]]
            }).bind("sortEnd", () => {
                const sortedColumns = this.tableReport.find(".sorting-asc, .sorting-desc");
                if (sortedColumns.length) {
                    const firstSortedColumn = sortedColumns.eq(0);
                    this.sortColumnName = firstSortedColumn.data("colname");
                    this.sortIndicator = firstSortedColumn.hasClass("sorting-desc") ? 1 : 0; // asc: 0, desc: 1
                }
            });
        } else {
            sortColumns.removeClass("sorting");
        }
    }

    private mapSortColumnNameToIndex() {
        const tableHeaderCells = this.tableReport.find(`thead th[data-colname=${this.sortColumnName}]`);
        if (tableHeaderCells.length) {
            return tableHeaderCells.eq(0).data("colindex");
        }

        // reset to default
        this.sortColumnName = "username";
        this.sortIndicator = 0;
        return 0;
    }

    private getListColumn(viewerLevel: number) {
        const dropdown = $("#dropdown-column");
        const member = dropdown.data("label-member");
        const agent = dropdown.data("label-agent");
        const winloss = dropdown.data("label-winloss");
        const comm = dropdown.data("label-comm");
        const total = dropdown.data("label-total");
        const listColumn = [
            { value: "fullname", text: dropdown.data("label-fullname") },
            { value: "betcount", text: dropdown.data("label-betcount") },
            { value: "turnover", text: dropdown.data("label-turnover") },
            { value: "netturnover", text: dropdown.data("label-netturnover") },
            { value: "grosscomm", text: dropdown.data("label-grosscomm") },
            { value: "pwinloss", text: member + " " + winloss },
            { value: "pcomm", text: member + " " + comm },
            { value: "ptotal", text: member + " " + total },
            { value: "awinloss", text: agent + " " + winloss },
            { value: "acomm", text: agent + " " + comm },
            { value: "atotal", text: agent + " " + total },
        ];

        if (viewerLevel >= 3) { // master or super
            const master = dropdown.data("label-master");
            listColumn.push(
                { value: "mwinloss", text: master + " " + winloss },
                { value: "mcomm", text: master + " " + comm },
                { value: "mtotal", text: master + " " + total });
        }

        if (viewerLevel === 4) { // super
            const superText = dropdown.data("label-super");
            listColumn.push(
                { value: "swinloss", text: superText + " " + winloss },
                { value: "scomm", text: superText + " " + comm },
                { value: "stotal", text: superText + " " + total });
        }

        listColumn.push({ value: "company", text: dropdown.data("label-company") });

        return listColumn;
    }

    private showHideColumns(column) {
        this.showHideSpecificColumn(column, "fullname");
        this.showHideSpecificColumn(column, "betcount");
        this.showHideSpecificColumn(column, "turnover");
        this.showHideSpecificColumn(column, "netturnover");
        this.showHideSpecificColumn(column, "grosscomm");
        this.showHideGroupColumn(column, "p", "groupColumnMember");
        this.showHideGroupColumn(column, "a", "groupColumnAgent");
        this.showHideGroupColumn(column, "m", "groupColumnMaster");
        this.showHideGroupColumn(column, "s", "groupColumnSuper");
        this.showHideSpecificColumn(column, "company");
    }

    private showHideGroupColumn(column, prefix, groupColumnId) {
        const groupColumn = this.tableReport.find("#" + groupColumnId);
        if (groupColumn.length) {
            let colspan = 0;
            if (this.showHideSpecificColumn(column, prefix + "winloss")) {
                colspan++;
            }
            if (this.showHideSpecificColumn(column, prefix + "comm")) {
                colspan++;
            }
            if (this.showHideSpecificColumn(column, prefix + "total")) {
                colspan++;
            }

            colspan ? groupColumn.attr("colspan", colspan).show() : groupColumn.hide();
        }
    }

    private showHideSpecificColumn(column, key) {
        const matchedColumns = this.tableReport.find(`[data-colname=${key}]`);
        const visible = column[key];
        visible ? matchedColumns.show() : matchedColumns.hide();

        return visible;
    }

    private updateCurrentUrl() {
        if (this.globalFormData.productIds) {
            const formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbs()) });
            window.top.indexManager.updateCurrentUrl($.param(formData));
        }
    }

    private addMoreParamsForStatistic() {
        let moreParams: any = {};
        const productAndProductDetail = this.productAndProductDetailDropdownComponent.getUncheckedValues();
        if (productAndProductDetail.unselectedProductIds) {
            moreParams.unselectedProductIds = productAndProductDetail.unselectedProductIds;
        } else {
            delete this.globalFormData.unselectedProductIds;
        }

        if (productAndProductDetail.unselectedResourceIds) {
            moreParams.unselectedResourceIds = productAndProductDetail.unselectedResourceIds;
        } else {
            delete this.globalFormData.unselectedResourceIds;
        }

        const hideColumns = this.visibleColumnDropdownComponent.getHideColumns();
        if (hideColumns) {
            moreParams.hideColumns = hideColumns;
        } else {
            delete this.globalFormData.hideColumns;
        }

        $.extend(this.globalFormData, moreParams);
    }

    private addIsWeekSelectionForStatistic() {
        this.globalFormData.isWeekSelection = 1;
    }

    private removeIsWeekSelectionForStatistic() {
        delete this.globalFormData.isWeekSelection;
    }
}

$(function () {
    new WinLossDetailViewNew(
        new TotalEnteredNonEnteredComponent(),
        new DateRangePickerJQueryComponent(),
        new ProductAndProductDetailDropdownComponent(),
        new VisibleColumnDropdownComponent(),
        new WeekRangeComponent(),
        new BreadcrumbLevelComponent(),
        window.productList,
        window.productDetailList).init();
});