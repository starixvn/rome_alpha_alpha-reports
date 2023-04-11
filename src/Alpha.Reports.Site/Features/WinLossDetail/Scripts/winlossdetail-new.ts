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
    private sortColumnIndex = 0;
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
        this.tableReportContainer.removeAttr("style");
    }

    private renderComponents() {
        this.totalEnteredNonEnteredComponent.init();

        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption((dateRange) => {
            $.extend(this.globalFormData, dateRange);
            this.getWinLossDetailDataAndRenderWeekRange();
        }));

        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(
            this.productList,
            this.productDetailList,
            (productsAndResources) => {
                $.extend(this.globalFormData, this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
                this.getWinLossDetailDataAndRenderWeekRange();
            }));

        const viewerId = +$("#hiddenViewerId").val();
        this.visibleColumnDropdownComponent.render(new VisibleColumnDropdownOption(
            `WinLossDetail_${viewerId}_${$("#hiddenSubAccId").val()}:Columns`,
            (visibleColumn) => this.showHideColumns(visibleColumn)));

        const dateRange = this.dateRangePickerComponent.getDateRangeValues();
        this.weekRangeComponent.init(new WeekRangeOption((dateRange) => {
            $.extend(this.globalFormData, dateRange);
            this.getWinLossDetailData();
        }, this.getMomentLocale()));
        this.weekRangeComponent.render(dateRange);

        const breadcrumbLevelJson = $("#hiddenBreadcrumbLevelJson").val();
        const breadcrumbs = breadcrumbLevelJson.length
            ? JSON.parse(breadcrumbLevelJson)
            : [{ custId: viewerId, custName: $("#hiddenViewerName").val(), custLevelId: +$("#hiddenViewerLevel").val() }];
        this.breadcrumbLevelComponent.init(new BreadcrumbLevelOption(
            breadcrumbs,
            (breadcrumbData) => {
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
        }, dateRange, this.productAndProductDetailDropdownComponent.getSelectedItems());
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
            this.weekRangeComponent.render(this.globalFormData);
            this.updateCurrentUrl();
        });
    }

    private getWinLossDetailData(callback = Function()) {
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
        });
    }

    private bindEvents() {
        this.initReportLegend($("#hiddenLegendUrl").val(), 800, 540);

        $("#icon-excel").click((e) => {
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
            this.tableReport.block();
            const $this = $(e.target);
            const custId = $this.data("custid");
            const custName = $this.data("custname");
            const breadcrumbs = this.breadcrumbLevelComponent.getBreadcrumbs();
            breadcrumbs.push({ custId, custName, custLevelId: 1 });
            const breadcrumbLevelJson = JSON.stringify(breadcrumbs);
            const betListsParams = $.extend({}, this.globalFormData, { custId, custName, breadcrumbLevelJson });
            $this.attr("href", `${$("#hiddenBetListsUrl").val()}?${$.param(betListsParams)}`);
        });

        this.tableReport.find(".downline-link").off().click((e) => {
            e.preventDefault();
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
    }

    private initTableSorter() {
        const sortColumns = this.tableReport.find(".sorting");

        if (this.tableReport.data("cansort")) {
            const headers = {};
            sortColumns.each(function (_, item) {
                const $item = $(item);
                let colIndex = $item.data("colindex");
                if (colIndex === -1) {
                    colIndex = sortColumns.length - 1;
                    $item.attr("data-colindex", colIndex);
                }

                const isText = colIndex === 0 || colIndex === 1;
                headers[colIndex] = { sorter: isText ? "text" : "digit" };
            });

            this.tableReport.tablesorter({
                headers: headers,
                sortList: [[this.sortColumnIndex, this.sortIndicator]]
            }).bind("sortEnd", () => {
                const sortedColumns = this.tableReport.find(".sorting-asc, .sorting-desc");
                if (sortedColumns.length) {
                    const firstSortedColumn = sortedColumns.eq(0);
                    this.sortColumnIndex = firstSortedColumn.data("colindex");
                    this.sortIndicator = firstSortedColumn.hasClass("sorting-desc") ? 1 : 0; // asc: 0, desc: 1
                }
            });
        } else {
            sortColumns.removeClass("sorting");
        }
    }

    private showHideColumns(column) {
        this.showHideFirstLastName(column);
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

    private showHideFirstLastName(column) {
        let colspan = 1;
        if (this.showHideSpecificColumn(column, "fullname")) {
            colspan++;
        }

        this.tableReport.find("#totalFirstLastName").attr("colspan", colspan);
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

    //$(window).on('load scroll resize', function () {
    //    var winLeft = $(window).scrollLeft();
    //    console.log(winLeft);
    //    $('#dataTable_toolbar').css('transform', 'translateX(' + winLeft + 'px)');

    //});
});