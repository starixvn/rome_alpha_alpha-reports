/// <reference path="../core.ts" />
"use strict";

class WinLossDetailView {
    domElements = {
        dateRangePicker: null,
        monthPickerContainer: null,
        monthPicker: null,
        dropdownMonth: null,
        longMonthsInYear: null,
        historicReportSwitch: null,
        currentReportSwitch: null,
        filter: null,
        dropdownProduct: null,

        iconFilter: null,
        pageTitle: null,
        iconSwitchMode: null,
        buttonSubmit: null,

        tableReport: null,
        pager: null,
        netTurnoverTooltip: null,
        closeNetTurnoverTooltip: null
    };

    isFirstSwitchMode = true;

    pageNumber = 0;
    pageSize = 0;
    timeToShowNetTurnoverTooltip = 10000000;
    cookieNetTurnoverTooltip = "NetTurnoverTooltipShowCount";

    init = () => {
        this.cacheDOMElements();
        this.renderMonthPicker();
        this.renderDateRangePicker();
        this.renderDropdownProduct();
        this.renderPager();
        this.renderNetTurnoverTooltip();

        this.domElements.tableReport.stickyTableHeaders();

        if ($("#hidefilter").val() === "1") {
            this.hideFilter();
        }

        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.domElements.monthPickerContainer = $(".monthpicker-container");
        this.domElements.monthPicker = $("#monthPickerID");
        this.domElements.dropdownMonth = $("#dropdown-month");
        this.domElements.longMonthsInYear = $("#longmonths-inyear");
        this.domElements.historicReportSwitch = $("#historicreport-switch");
        this.domElements.currentReportSwitch = $("#currentreport-switch");

        this.domElements.pageTitle = $(".page-title ");
        this.domElements.filter = $("#filter");
        this.domElements.iconFilter = $(".icon-filter");
        this.domElements.iconSwitchMode = $(".icon-switchmode");

        this.domElements.dropdownProduct = $("#dropdown-product");
        this.domElements.buttonSubmit = $(".btn-submit");

        this.domElements.tableReport = $("#tbl-report");
        this.domElements.pager = $("#report-pager");

        this.domElements.netTurnoverTooltip = $("#netturnovertooltip");

        this.domElements.closeNetTurnoverTooltip = $("#close-netturnover-tooltip");
    };

    renderDateRangePicker = () => {
        var dateRangePicker = $("#daterange-picker");

        var currentDate = dateRangePicker.data("currentdate");
        var minDate = dateRangePicker.data("mindate");
        var maxDate = dateRangePicker.data("maxdate");

        dateRangePicker.nexDatePicker({
            nexDatePickerType: "quickchoose",
            datePickerFormat: {
                changeMonth: true,
                changeYear: true,
                showOtherMonths: true,
                dateFormat: alphaReports.format.shortDateWithSlash
            },
            dropDownList:
            {
                Id: "dropdown-daterange",
                Label: "",
                LabelCss: "",
                OptionCss: ""
            },
            firstDatePicker: {
                Id: "picker-fromdate",
                Label: "",
                InitDate: new Date(currentDate),
                LabelCss: ""
            },
            secondDatePicker: {
                Id: "picker-todate",
                Label: dateRangePicker.data("labelto"),
                InitDate: new Date(currentDate),
                LabelCss: ""
            },
            selectedItem: 0,
            onSelectDatePicker: null,
            minDate: new Date(minDate),
            maxDate: maxDate === "" ? null : maxDate,
            optionItems: [
                { Name: dateRangePicker.data("labeltoday"), Value: 0 },
                { Name: dateRangePicker.data("labelyesterday"), Value: 1 },
                { Name: dateRangePicker.data("labelthisweek"), Value: 2 },
                { Name: dateRangePicker.data("labellastweek"), Value: 3 },
                { Name: dateRangePicker.data("labelthismonth"), Value: 4 },
                { Name: dateRangePicker.data("labellastmonth"), Value: 5 },
                { Name: dateRangePicker.data("labelsincelastmonth"), Value: 6 },
                { Name: dateRangePicker.data("labelspecificdate"), Value: 7 }
            ]
        });

        this.domElements.dateRangePicker = $("#daterange-picker");
    };

    renderMonthPicker = () => {
        var dropdownMonth = this.domElements.dropdownMonth;

        var maxDate = dropdownMonth.data("maxdate");
        dropdownMonth.monPickr({
            longMonthsInYear: this.getLongMonthsInYear(),
            maxDate: maxDate === "" ? dropdownMonth.data("currentdate") : maxDate,
            minDate: dropdownMonth.data("mindate"),
            month: new Date(dropdownMonth.data("dateinmonth")),

            fromDay: 1,
            toDay: dropdownMonth.data("todate"),

            showDateRange: true,
            monthFormat: alphaReports.format.monthFormat,
            fromDayId: "ddl-fromday",
            toDayId: "ddl-today",
            fromDayLabel: "",
            toDayLabel: dropdownMonth.data("labelto"),
            sortDesc: true,
            log: function (msg: string) {
                console.warn(msg);
            }
        });

        if (!this.domElements.filter.hasClass("historic-report")) {
            this.domElements.monthPickerContainer.addClass("hide");
        }
    };

    getLongMonthsInYear = () => {
        // $.parseJSON: string must start/end with "
        var longMonthsInYear = $.parseJSON(this.domElements.longMonthsInYear.val());
        return longMonthsInYear;
    };

    renderDropdownProduct = () => {
        var dropdownProduct = this.domElements.dropdownProduct;
        var products = dropdownProduct.data("products");
        var selectedProduct = dropdownProduct.data("selectedproducts").split(",");

        var product;
        var productOptions = "";

        for (var i = 0, max = products.length; i < max; i++) {
            product = products[i];
            productOptions +=
                "<option" +
                " id=\"product_" + product.Id + "\"" +
                " value=\"" + product.Id + "\"" +
                (selectedProduct.indexOf(product.Id.toString()) !== -1 ? " selected=\"selected\"" : "") +
                " >" +
                product.DisplayText +
                "</options>";
        }

        dropdownProduct.html(productOptions);
        dropdownProduct.multiselect({
            maxHeight: 300,
            enableFiltering: true,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            enableCaseInsensitiveFiltering: true,
            nonSelectedText: $("#hasdata").val() === "1" ?
                dropdownProduct.data("labelselectproducts")
                : dropdownProduct.data("labelthereisnoproduct"),
            nSelectedText: " " + dropdownProduct.data("labelselected"),
            allSelectedText: dropdownProduct.data("labelallproducts"),
            includeSelectAllOption: products.length,
            onChange: function (option: any, checked: boolean) {
                // do nothing
            },
            language: dropdownProduct.data("language")
        });
    };

    registerEvents = () => {
        var $this = this;

        this.domElements.historicReportSwitch.on("click", function () {
            $this.domElements.filter.addClass("historic-report");
            $this.domElements.monthPickerContainer.removeClass("hide");
        });

        this.domElements.currentReportSwitch.on("click", function () {
            $this.domElements.monthPickerContainer.addClass("hide");
            $this.domElements.filter.removeClass("historic-report");
        });

        this.domElements.iconFilter.on("click", function () {
            var filter = $this.domElements.filter;

            if (filter.is(":hidden")) {
                $this.domElements.pageTitle.removeClass("hide-filter");
                filter.slideDown("fast");
            } else {
                $this.hideFilter();
            }
            setTimeout(function () {
                $this.positionNetTurnoverTooltip();
            }, 200);
        });

        this.domElements.iconSwitchMode.on("click", function () {
            var tableReport = $this.domElements.tableReport;
            var isStackMode = tableReport.hasClass("stack-mode");

            if (isStackMode) {
                $this.domElements.pageTitle.removeClass("stack-mode");
                tableReport.removeClass("stack-mode");
            } else {
                $this.domElements.pageTitle.addClass("stack-mode");
                tableReport.addClass("stack-mode");
            }

            if ($this.isFirstSwitchMode) {
                if (isStackMode) {
                    alphaReports.splitColumn([
                        "text-right hidden-stack-mode col-turnover",
                        "text-right hidden-stack-mode col-netturnover",
                        "text-right hidden-stack-mode col-grosscomm"],
                        ".col-turnover-combined");
                    alphaReports.splitColumn([
                        "text-right hidden-stack-mode col-winloss",
                        "text-right hidden-stack-mode col-comm",
                        "text-right bold hidden-stack-mode col-total"],
                        ".column-winloss-combined");
                } else {
                    alphaReports.combineColumns([
                        "td.col-turnover",
                        "td.col-netturnover",
                        "td.col-grosscomm"],
                        "text-right visible-stack-mode combinedcolumn col-turnover-combined", 1);
                    alphaReports.combineColumns([
                        "td.col-winloss",
                        "td.col-comm",
                        "td.col-total"],
                        "text-right visible-stack-mode combinedcolumn column-winloss-combined", 3);
                }

                $this.isFirstSwitchMode = false;
            }
        });

        if (alphaReports.screenSmallThan768) {
            this.domElements.buttonSubmit.on("click", function () {
                $this.domElements.pageTitle.addClass("hide-filter");
                $this.domElements.filter.slideUp("fast");
            });
        }

        $(window).resize(function () {
            $this.positionNetTurnoverTooltip();
        });
    };

    renderPager = () => {
        var pager = this.domElements.pager;
        var tableReport = this.domElements.tableReport;

        var options = {
            totalRecords: tableReport.data("totalrecords"),
            pageSize: tableReport.data("pagesize"),
            pageIndex: tableReport.data("pageindex")
        };

        if (options.totalRecords < alphaReports.pageList[0]) {
            // total records less than min page size => do not need to show pager
            this.domElements.pager.addClass("hide");
        } else {
            this.domElements.pager.pagination({
                pageList: alphaReports.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    this.pageNumber = pageNumber - 1;
                    this.pageSize = pageSize;

                    winLossDetailController.pageReport();
                },
                beforePageText: pager.data("lablepage"),
                afterPageText: pager.data("labelof") + " {pages}",
                displayMsg: pager.data("labeldisplayitems")
            });

            this.domElements.pager.removeClass("hide");
        }
    };

    renderNetTurnoverTooltip = () => {
        var $this = this;
        var netTurnoverTooltipShowCount = alphaReports.localStorage.get(this.cookieNetTurnoverTooltip) || 0;

        if (netTurnoverTooltipShowCount &&
            netTurnoverTooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
            this.domElements.netTurnoverTooltip.remove();
        } else {
            this.positionNetTurnoverTooltip();
            this.domElements.netTurnoverTooltip.modal({ show: true, backdrop: false });

            this.domElements.closeNetTurnoverTooltip.on("click", function () {
                alphaReports.localStorage.set(this.cookieNetTurnoverTooltip, alphaReports.maxTimesToShowTooltip);
                $this.closeNetTurnoverTooltip();
            });

            alphaReports.localStorage.set($this.cookieNetTurnoverTooltip, netTurnoverTooltipShowCount + 1);
            setTimeout(function () {
                $this.closeNetTurnoverTooltip();
            }, this.timeToShowNetTurnoverTooltip);
        }
    };

    closeNetTurnoverTooltip = () => {
        this.domElements.netTurnoverTooltip.modal("hide");
    };
    hideFilter = () => {
        this.domElements.pageTitle.addClass("hide-filter");
        this.domElements.filter.slideUp("fast");
    };
    positionNetTurnoverTooltip = () => {
        var netTurnoverPosition = $(".col-netturnover-header").position();
        var rightPosition = $(".col-netturnover-header").width();

        this.domElements.netTurnoverTooltip.css({
            left: netTurnoverPosition.left + rightPosition - 10,
            top: netTurnoverPosition.top - 22
        });
    };
};

const winLossDetailView = new WinLossDetailView();
export { winLossDetailView };
