/// <reference path="../core.ts" />
"use strict";

var indexView = {
    domElements: {
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
        pager: null
    },

    isFirstSwitchMode: true,

    pageNumber: 0,
    pageSize: 0,

    init: function () {
        indexView.cacheDOMElements();
        indexView.renderMonthPicker();
        indexView.renderDateRangePicker();
        indexView.renderDropdownProduct();
        indexView.renderPager({
            totalRecords: 21,
            pageSize: 10,
            pageIndex: 1
        });

        indexView.registerEvents();
    },
    cacheDOMElements: function () {
        indexView.domElements.monthPickerContainer = $(".monthpicker-container");
        indexView.domElements.monthPicker = $("#monthPickerID");
        indexView.domElements.dropdownMonth = $("#dropdown-month");
        indexView.domElements.longMonthsInYear = $("#longmonths-inyear");
        indexView.domElements.historicReportSwitch = $("#historicreport-switch");
        indexView.domElements.currentReportSwitch = $("#currentreport-switch");

        indexView.domElements.pageTitle = $(".page-title ");
        indexView.domElements.filter = $("#filter");
        indexView.domElements.iconFilter = $(".icon-filter");
        indexView.domElements.iconSwitchMode = $(".icon-switchmode");

        indexView.domElements.dropdownProduct = $("#dropdown-product");
        indexView.domElements.buttonSubmit = $(".btn-submit");

        indexView.domElements.tableReport = $("#tbl-report");
        indexView.domElements.pager = $("#report-pager");
    },
    renderDateRangePicker: function () {
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

        indexView.domElements.dateRangePicker = $("#daterange-picker");
    },
    renderMonthPicker: function () {
        var dropdownMonth = indexView.domElements.dropdownMonth;

        var maxDate = dropdownMonth.data("maxdate");
        dropdownMonth.monPickr({
            longMonthsInYear: indexView.getLongMonthsInYear(),
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

        indexView.domElements.monthPickerContainer.addClass("hide");
    },
    getLongMonthsInYear: function () {
        // $.parseJSON: string must start/end with "
        var longMonthsInYear = $.parseJSON(indexView.domElements.longMonthsInYear.val());
        return longMonthsInYear;
    },
    renderDropdownProduct: function () {
        var dropdownProduct = indexView.domElements.dropdownProduct;
        var products = dropdownProduct.data("products");

        var product;
        var productOptions = "";

        for (var i = 0, max = products.length; i < max; i++) {
            product = products[i];
            productOptions +=
                "<option" +
                " id=\"product_" + product.Id + "\"" +
                " value=\"" + product.Id + "\"" +
                " selected=\"selected\"" +
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
            nonSelectedText: dropdownProduct.data("labelselectproducts"),
            nSelectedText: " " + dropdownProduct.data("labelselected"),
            allSelectedText: dropdownProduct.data("labelallproducts"),
            includeSelectAllOption: products.length,
            onChange: function (option: any, checked: boolean) {
                // do nothing
            },
            language: dropdownProduct.data("language")
        });
    },
    registerEvents: function () {
        indexView.domElements.historicReportSwitch.on("click", function () {
            indexView.domElements.filter.addClass("historic-report");
            indexView.domElements.monthPickerContainer.removeClass("hide");
        });

        indexView.domElements.currentReportSwitch.on("click", function () {
            indexView.domElements.monthPickerContainer.addClass("hide");
            indexView.domElements.filter.removeClass("historic-report");
        });

        indexView.domElements.iconFilter.on("click", function () {
            var filter = indexView.domElements.filter;

            if (filter.is(":hidden")) {
                indexView.domElements.pageTitle.removeClass("hide-filter");
                filter.slideDown("fast");
            } else {
                indexView.domElements.pageTitle.addClass("hide-filter");
                filter.slideUp("fast");
            }
        });

        indexView.domElements.iconSwitchMode.on("click", function () {
            var tableReport = indexView.domElements.tableReport;
            var isStackMode = tableReport.hasClass("stack-mode");

            if (isStackMode) {
                indexView.domElements.pageTitle.removeClass("stack-mode");
                tableReport.removeClass("stack-mode");
            } else {
                indexView.domElements.pageTitle.addClass("stack-mode");
                tableReport.addClass("stack-mode");
            }

            if (indexView.isFirstSwitchMode) {

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
                        "text-right visible-stack-mode combinedcolumn", 1);
                    alphaReports.combineColumns([
                        "td.col-winloss",
                        "td.col-comm",
                        "td.col-total"],
                        "text-right visible-stack-mode combinedcolumn", 3);
                }

                indexView.isFirstSwitchMode = false;
            }
        });

        if (alphaReports.screenSmallThan768) {
            indexView.domElements.buttonSubmit.on("click", function () {
                indexView.domElements.pageTitle.addClass("hide-filter");
                indexView.domElements.filter.slideUp("fast");
            });
        }
    },
    renderPager: function (options: any) {
        var pager = indexView.domElements.pager;

        if (options.totalRecords < alphaReports.pageList[0]) {
            // total records less than min page size => do not need to show pager
            indexView.domElements.pager.addClass("hide");
        } else {
            indexView.domElements.pager.pagination({
                pageList: alphaReports.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    indexView.pageNumber = pageNumber - 1;
                    indexView.pageSize = pageSize;

                    indexController.pageReport();
                },
                beforePageText: pager.data("lablepage"),
                afterPageText: pager.data("labelof") + " {pages}",
                displayMsg: pager.data("labeldisplayitems")
            });

            indexView.domElements.pager.removeClass("hide");
        }
    }
};