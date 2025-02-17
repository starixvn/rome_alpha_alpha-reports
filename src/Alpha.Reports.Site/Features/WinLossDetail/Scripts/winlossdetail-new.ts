"use strict";

interface Window {
    productList: any,
    productDetailList: any,
    columnList: any,
    reportTitle: any,
    levelTypeText: any,
    indexManager: any,
    alpha: any,
}

interface JQuery {
    tablesorter(option?: any): JQuery;
}

class WinLossDetailViewNew extends BaseReportView {
    private totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent;
    private dateRangePickerComponent: DateRangePickerJQueryComponent;
    private productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent;
    private usernameAutoSuggestionComponent: UsernameAutoSuggestionComponent;
    private winLossDetailColumns: WinLossDetailColumns;
    private weekSelectionComponent: WeekSelectionComponent
    private breadcrumbLevelComponent: BreadcrumbLevelComponent;
    private winLossDetailCopyHtml: WinLossDetailCopyHtml;
    private reportWeekSelection: JQuery;
    private tableReportContainer: JQuery;
    private tableReport: JQuery;
    private winLossDetailGetDataUrl: string
    private winLossDetailGetWeeksUrl: string;
    private winLossDetailGetDataWeekUrl: string
    private globalFormData: any;
    private isGettingReportData = false;
    private isWeekSelectionClicked = false;
    private levelTypeDropdown: SingleSelectionDropdownComponent;

    constructor(
        totalEnteredNonEnteredComponent: TotalEnteredNonEnteredComponent,
        dateRangePickerComponent: DateRangePickerJQueryComponent,
        productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent,
        usernameAutoSuggestionComponent: UsernameAutoSuggestionComponent,
        winLossDetailColumns: WinLossDetailColumns,
        weekSelectionComponent: WeekSelectionComponent,
        breadcrumbLevelComponent: BreadcrumbLevelComponent,
        winLossDetailCopyHtml: WinLossDetailCopyHtml,
        levelTypeDropdown: SingleSelectionDropdownComponent) {
        super();
        this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        this.usernameAutoSuggestionComponent = usernameAutoSuggestionComponent;
        this.winLossDetailColumns = winLossDetailColumns;
        this.weekSelectionComponent = weekSelectionComponent;
        this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        this.winLossDetailCopyHtml = winLossDetailCopyHtml;
        this.levelTypeDropdown = levelTypeDropdown;
        // set default sort column name
        this.sortColumnName = "username";
    }

    init() {
        this.reportWeekSelection = $("#report-week-selection");
        this.tableReportContainer = $("#tbl-report-container");
        this.winLossDetailGetDataUrl = $("#hiddenGetDataUrl").val();
        this.winLossDetailGetWeeksUrl = $("#hiddenGetWeeksUrl").val();
        this.winLossDetailGetDataWeekUrl = $("#hiddenGetDataWeekUrl").val();

        this.renderComponents();
        this.bindEvents();

        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.reportWeekSelection.removeAttr("style");

        setTimeout(function () {
            let mainDomain = $("#hiddenMainDomain").val();
            if (mainDomain.length) {
                mainDomain = mainDomain.indexOf("http") === -1
                    ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                    : mainDomain;
                window.top.postMessage({
                    event_id: "winlossdetailnew-feedback",
                    feedback_id: 2
                }, mainDomain);
            } else {
                window.top.indexManager.initFeedbackFunction(2);
            }
        }, 1000);
    }

    private renderComponents() {
        const weekFromDate = $("#hiddenWeekFromDate");
        const weekToDate = $("#hiddenWeekToDate");

        this.totalEnteredNonEnteredComponent.init();

        this.dateRangePickerComponent.render({
            maxDays: 92,
            onClosed: (dateRange) => {
                if (this.builGlobalFormData(dateRange)) {
                    this.getWinLossDetailDataAndRenderWeekRange();
                }
            }
        });
        const dateRange = this.dateRangePickerComponent.getDateRangeValues();

        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(
            window.productList,
            window.productDetailList,
            (productsAndResources) => {
                if (this.builGlobalFormData(undefined, productsAndResources)) {
                    this.getWinLossDetailDataAndRenderWeekRange();
                }
            }));

        this.levelTypeDropdown.render(new SingleSelectionDropdownOption("dropdown-level-type", "LevelType", () => {
            if (this.builGlobalFormData(undefined, undefined, this.levelTypeDropdown.selectedValue)) {
                const isRebuildBreadcrumb = this.globalFormData.username && this.globalFormData.username.length > 0;
                this.getWinLossDetailDataAndRenderWeekRange(isRebuildBreadcrumb);
            }
        }));

        this.usernameAutoSuggestionComponent.init(new UsernameAutoSuggestionOption(
            $("#hiddenUsernameAutoSuggestionUrl").val(),
            false,
            true,
            () => {
                this.isGettingReportData = true;
                const username = this.usernameAutoSuggestionComponent.getUsername()
                // reset params: custId, custName, custLevelId
                delete this.globalFormData.custId;
                delete this.globalFormData.custName;
                delete this.globalFormData.custLevelId;
                delete this.globalFormData.breadcrumbLevelJson;
                this.levelTypeDropdown.setSelectedValue("0")
                if (this.builGlobalFormData(undefined, undefined, undefined, username)) {
                    this.getWinLossDetailDataAndRenderWeekRange(true);
                } else {
                    this.isGettingReportData = false;
                }
            }));

        this.winLossDetailColumns.init(
            $("#hiddenShowColumns").val(),
            (visibleColumns) => {
                $.extend(this.globalFormData, { showColumns: visibleColumns });
                this.updateCurrentUrl();
            });

        this.weekSelectionComponent.init(new WeekSelectionOption((dateRange) => {
            if (this.isGettingReportData) {
                return;
            }

            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.addIsWeekSelectionForStatistic();

            $.extend(this.globalFormData, dateRange);
            this.getWinLossDetailDataWeek();
        }));
        const activeWeekRange = weekFromDate.length > 0 && weekToDate.length > 0
            ? { weekFromDate: weekFromDate.val(), weekToDate: weekToDate.val() }
            : {};

        this.breadcrumbLevelComponent.init(new BreadcrumbLevelOption(
            this.buildBreadcrumbs(),
            (breadcrumbData) => {
                if (this.isInvalidGlobalFormData()) {
                    return;
                }

                this.usernameAutoSuggestionComponent.resetUsername();
                delete this.globalFormData.username;
                delete this.globalFormData.levelType;

                const dateRange = this.dateRangePickerComponent.getDateRangeValues();
                const activeWeek = this.weekSelectionComponent.getActiveWeek();
                $.extend(this.globalFormData, dateRange, activeWeek, breadcrumbData);
                this.getWinLossDetailWeeks();
                this.getWinLossDetailData(() => {
                    this.breadcrumbLevelComponent.removeBreadcrumbLevel(breadcrumbData.custLevelId);
                    this.showHideLevelTypeFilterAndResetToAll(breadcrumbData.custLevelId);
                    this.updateCurrentUrl();
                });
            },
            window.reportTitle));

        this.globalFormData = $.extend({}, {
            custId: +$("#hiddenCustId").val(),
            custName: $("#hiddenCustName").val(),
            custLevelId: +$("#hiddenCustLevel").val(),
            username: this.usernameAutoSuggestionComponent.getUsername(),
            showColumns: $("#hiddenShowColumns").val()
        }, dateRange, this.productAndProductDetailDropdownComponent.getCheckedValues(), activeWeekRange);

        const levelType = this.levelTypeDropdown.selectedValue;
        if (levelType) {
            this.globalFormData.levelType = levelType;
        }
    }

    private builGlobalFormData(dateRange = undefined, productsAndResources = undefined, levelType = undefined, username = undefined) {
        dateRange = dateRange || this.dateRangePickerComponent.getDateRangeValues();
        productsAndResources = productsAndResources || this.productAndProductDetailDropdownComponent.getCheckedValues();
        const others: any = {
            username: username || this.usernameAutoSuggestionComponent.getUsername()
        };
        if (this.levelTypeDropdown.$dropdown.length) {
            others.levelType = levelType || this.levelTypeDropdown.getSelectedItem();
        }

        this.resetActiveWeek();
        $.extend(this.globalFormData, dateRange, productsAndResources, others);

        return !this.isInvalidGlobalFormData();
    }

    //private constraintDateRangeAndProductDropdownList(dateRange) {
    //    const historicPivotDate = moment(this.dateRangePickerComponent.getHistoricPivotDate());
    //    if (moment(dateRange.fromDate).isSameOrAfter(historicPivotDate)) {
    //        this.productAndProductDetailDropdownComponent.hideHistoricProducts();
    //    } else {
    //        this.productAndProductDetailDropdownComponent.showHistoricProducts();
    //    }
    //}

    private getWinLossDetailDataAndRenderWeekRange(isRebuildBreadcrumb: boolean = false) {
        this.getWinLossDetailWeeks();
        this.getWinLossDetailData(() => {
            if (isRebuildBreadcrumb) {
                const breadcrumbs = this.buildBreadcrumbs();
                const breadcrumbsLength = breadcrumbs.length;
                if (breadcrumbsLength) {
                    const currentBreadcrumb = breadcrumbs[breadcrumbsLength - 1];
                    this.showHideLevelTypeFilter(currentBreadcrumb.custLevelId);
                    this.breadcrumbLevelComponent.setBreadcrumbs(breadcrumbs);
                }
            } else {
                this.breadcrumbLevelComponent.render();
            }
            this.updateCurrentUrl();
        });
    }

    private getWinLossDetailData(callback = Function()) {
        //this.addMoreParamsForStatistic();

        this.reportWeekSelection.block();
        $.get(`${this.winLossDetailGetDataUrl}?${$.param(this.globalFormData)}`, (html) => {
            this.reportWeekSelection.html(html);
            this.tableReportContainer = $("#tbl-report-container");
            this.bindCopyAndExcelEvents();
            this.bindTableReportEvents();
            this.totalEnteredNonEnteredComponent.update(
                { eventDate: this.tableReport.data("yesterday"), nonEntered: this.tableReport.data("non-entered-yesterday") },
                { eventDate: this.tableReport.data("today"), nonEntered: this.tableReport.data("non-entered-today") });
            callback();
            this.isWeekSelectionClicked = false;
            this.reportWeekSelection.unblock();
            this.isGettingReportData = false;
        }).fail((jqXHR) => {
            this.reportWeekSelection.unblock();
            this.isGettingReportData = false;
            if (jqXHR.status === 429) {
                window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
            } else if (jqXHR.status === 500 && jqXHR.responseText) {
                const responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    const cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    const siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top.alpha.urls.mainRootPath;
                    window.location.href = `${siteMainUrl}partialum/index?${$.param(responseObject)}`;
                }
            }
        }).always(() => this.removeIsWeekSelectionForStatistic());
    }

    private getWinLossDetailWeeks() {
        $.get(`${this.winLossDetailGetWeeksUrl}?${$.param(this.globalFormData)}`, (html) =>
            this.weekSelectionComponent.reRender(html)
        ).fail((jqXHR) => {
            if (jqXHR.status === 429) {
                window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
            } else if (jqXHR.status === 500 && jqXHR.responseText) {
                const responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    const cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    const siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top.alpha.urls.mainRootPath;
                    window.location.href = `${siteMainUrl}partialum/index?${$.param(responseObject)}`;
                }
            }
        });
    }

    private getWinLossDetailDataWeek(callback = Function()) {
        //this.addMoreParamsForStatistic();

        this.tableReportContainer.block();
        $.get(`${this.winLossDetailGetDataWeekUrl}?${$.param(this.globalFormData)}`, (html) => {
            this.tableReportContainer.html(html);
            this.bindTableReportEvents();
            this.totalEnteredNonEnteredComponent.update(
                { eventDate: this.tableReport.data("yesterday"), nonEntered: this.tableReport.data("non-entered-yesterday") },
                { eventDate: this.tableReport.data("today"), nonEntered: this.tableReport.data("non-entered-today") });
            callback();
            this.updateCurrentUrl();
            this.isWeekSelectionClicked = true;
            this.tableReportContainer.unblock();
            this.isGettingReportData = false;
        }).fail((jqXHR) => {
            this.tableReportContainer.unblock();
            this.isGettingReportData = false;
            if (jqXHR.status === 429) {
                window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
            } else if (jqXHR.status === 500 && jqXHR.responseText) {
                const responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    const cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    const siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top.alpha.urls.mainRootPath;
                    window.location.href = `${siteMainUrl}partialum/index?${$.param(responseObject)}`;
                }
            }
        }).always(() => this.removeIsWeekSelectionForStatistic());
    }

    private buildBreadcrumbs() {
        this.tableReport = this.tableReport || this.tableReportContainer.find("#tbl-report");
        const breadcrumbLevelJson = this.tableReport.data("breadcrumbleveljson");
        return typeof breadcrumbLevelJson !== "string"
            ? []
            : JSON.parse(breadcrumbLevelJson.replace(/'/g, '"'))
                .map(function (b) {
                    return {
                        custId: b.CustId,
                        custName: b.CustName.toUpperCase(),
                        custLevelId: b.CustLevelId
                    };
                });
    }

    private isInvalidGlobalFormData() {
        return this.globalFormData.productIds.length === 0;
    }

    private bindEvents() {
        this.initReportLegend($("#hiddenLegendUrl").val());

        this.initShowHideFilter((isHideFilter) => {
            if (isHideFilter) {
                this.weekSelectionComponent.hide();
                $('#msg-warning-note').hide();
            } else {
                this.weekSelectionComponent.show();
                $('#msg-warning-note').show();
            }
        });

        this.bindCopyAndExcelEvents();
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
            let betListsParams = this.weekSelectionComponent.getActiveWeek();
            $.extend(betListsParams, this.globalFormData, dateRange, { custId, custName, breadcrumbLevelJson });
            $this.attr("href", `${$("#hiddenBetListsUrl").val()}?${$.param(betListsParams)}`);
        });

        this.tableReport.find(".downline-link").off().click((e) => {
            e.preventDefault();
            if (this.isGettingReportData) {
                return;
            }

            if (this.isInvalidGlobalFormData()) {
                return;
            }

            this.usernameAutoSuggestionComponent.resetUsername();
            delete this.globalFormData.username;
            delete this.globalFormData.levelType;

            const $this = $(e.target);
            const dateRange = this.dateRangePickerComponent.getDateRangeValues();
            const activeWeek = this.weekSelectionComponent.getActiveWeek();
            const breadcrumbData = {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: $this.data("custlevelid")
            };
            $.extend(this.globalFormData, dateRange, activeWeek, breadcrumbData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbs()) });

            this.getWinLossDetailWeeks();
            this.getWinLossDetailData(() => {
                this.breadcrumbLevelComponent.addBreadcrumbLevel(breadcrumbData);
                this.showHideLevelTypeFilterAndResetToAll(breadcrumbData.custLevelId);
                this.updateCurrentUrl();
            });

            this.tableReport.find(".ic-dormant-accounts").tooltip();
        });

        this.tableReport.find(".ic-plus-box").off("click").click((e) => {
            window.top["reloadMyWatchlistDataTable"] = () => this.isWeekSelectionClicked ? this.getWinLossDetailDataWeek() : this.getWinLossDetailDataAndRenderWeekRange(true);
            const custName = $(e.target).data("custname");
            const url = `${$("#hiddenMemberInfoUrl").val()}MyWatchlistAccounts/ManageWatchlistAccounts?usernames=${custName}`;
            const width = 820;
            const height = 550;
            let mainDomain = $("#hiddenMainDomain").val();
            if (mainDomain.length) {
                mainDomain = mainDomain.indexOf("http") === -1
                    ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                    : mainDomain;
                window.top.postMessage({
                    event_id: "winlossdetailnew-addtomywatchlist",
                    url: url,
                    width: width,
                    height: height
                }, mainDomain);
            } else {
                window.top["popupManager"].open(url, undefined, width, height, "modal-mobile-friendly");
            }
        });

        this.initTableSorter();
        this.winLossDetailColumns.showHideColumns();
    }

    private bindCopyAndExcelEvents() {
        this.winLossDetailCopyHtml.init();

        $("#icon-excel").off().click((e) => {
            if (this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }

            const $this = $(e.target);
            $this.attr("href", `${$this.data("url")}?${$.param(this.globalFormData)}`);
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        }).tooltip();
    }

    private initTableSorter() {
        this.tableSorter(this.tableReport, undefined, () => {
            const dormantAccountsRow = this.tableReport.find('.dormant-accounts-mark');
            dormantAccountsRow.length && this.tableReport.append(dormantAccountsRow);
        });
    }

    protected mapSortColumnIndex() {
        const tableHeaderCells = this.tableReport.find(`thead th[data-colname=${this.sortColumnName}]`);
        if (tableHeaderCells.length) {
            this.sortColumnIndex = tableHeaderCells.eq(0).data("colindex");
        } else {
            // reset to default
            this.sortColumnName = "username";
            this.sortColumnIndex = 0;
            this.sortIndicator = 0;
        }

        return this.sortColumnIndex;
    }

    protected updateCurrentUrl() {
        if (this.globalFormData.productIds) {
            const formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbs()) });
            super.updateCurrentUrl($.param(formData));
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

        const hideColumns = this.winLossDetailColumns.getHideColumns();
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

    private resetActiveWeek() {
        delete this.globalFormData.weekFromDate;
        delete this.globalFormData.weekToDate;
    }

    private showHideLevelTypeFilter(custLevel: number) {
        const levelTypeDropdownContainer = this.levelTypeDropdown.$dropdown.closest('.level-multiselect-container');

        if (custLevel === 2) {
            levelTypeDropdownContainer.hide();
        } else if (custLevel === 3 || custLevel === 4) {
            const masterAgentText = window.levelTypeText[custLevel];
            //if (this.levelTypeDropdown.selectedValue !== "0") {
            //    // update selected value to All
            //    this.levelTypeDropdown.setSelectedValue("0");
            //}

            // update option text
            this.levelTypeDropdown.$dropdown.find('option[value="1"]').text(masterAgentText);

            // update dropdown text
            const label = levelTypeDropdownContainer.find('.dropdown-menu').find('input[type="radio"][value="1"]').parent();
            label.contents().filter(function () {
                return this.nodeType === 3; /* Node type 3 is a text node */
            }).first().replaceWith(masterAgentText);

            levelTypeDropdownContainer.show();
        }
    }

    private showHideLevelTypeFilterAndResetToAll(custLevel: number) {
        const levelTypeDropdownContainer = this.levelTypeDropdown.$dropdown.closest('.level-multiselect-container');

        if (custLevel === 2) {
            levelTypeDropdownContainer.hide();
        } else if (custLevel === 3 || custLevel === 4) {
            const masterAgentText = window.levelTypeText[custLevel];
            if (this.levelTypeDropdown.selectedValue !== "0") {
                // update selected value to All
                this.levelTypeDropdown.setSelectedValue("0");
            }

            // update option text
            this.levelTypeDropdown.$dropdown.find('option[value="1"]').text(masterAgentText);

            // update dropdown text
            const label = levelTypeDropdownContainer.find('.dropdown-menu').find('input[type="radio"][value="1"]').parent();
            label.contents().filter(function () {
                return this.nodeType === 3; /* Node type 3 is a text node */
            }).first().replaceWith(masterAgentText);

            levelTypeDropdownContainer.show();
        }
    }
}

$(function () {
    new WinLossDetailViewNew(
        new TotalEnteredNonEnteredComponent(),
        new DateRangePickerJQueryComponent(),
        new ProductAndProductDetailDropdownComponent(),
        new UsernameAutoSuggestionComponent(),
        new WinLossDetailColumns(new VisibleColumnDropdownComponent(), window.columnList),
        new WeekSelectionComponent(),
        new BreadcrumbLevelComponent(),
        new WinLossDetailCopyHtml(Clipboard),
        new SingleSelectionDropdownComponent($)).init();
});