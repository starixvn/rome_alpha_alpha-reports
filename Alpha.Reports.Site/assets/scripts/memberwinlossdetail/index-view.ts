/// <reference path="../core.ts" />
"use strict";

var indexView = {
    domElements: {
        dateRangePicker: null,
        monthPicker: null,
        dropdownMonth: null,
        longMonthsInYear: null,
        historicReportSwitch: null,
        currentReportSwitch: null,
        filter: null
    },
    init: function () {
        indexView.cacheDOMElements();
        indexView.renderMonthPicker();
        indexView.renderDateRangePicker();

        indexView.registerEvents();
    },
    cacheDOMElements: function () {
        indexView.domElements.monthPicker = $("#monthPickerID");
        indexView.domElements.dropdownMonth = $("#dropdown-month");
        indexView.domElements.longMonthsInYear = $("#longmonths-inyear");
        indexView.domElements.historicReportSwitch = $("#historicreport-switch");
        indexView.domElements.currentReportSwitch = $("#currentreport-switch");
        indexView.domElements.filter = $("#filter");
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

        indexView.domElements.monthPicker.addClass("hide");
    },
    getLongMonthsInYear: function () {
        // $.parseJSON: string must start/end with "
        var longMonthsInYear = $.parseJSON(indexView.domElements.longMonthsInYear.val());
        return longMonthsInYear;
    },
    registerEvents: function () {
        indexView.domElements.historicReportSwitch.on("click", function () {
            indexView.domElements.filter.addClass("historic-report");
            indexView.domElements.monthPicker.removeClass("hide");
        });

        indexView.domElements.currentReportSwitch.on("click", function () {
            indexView.domElements.monthPicker.addClass("hide");
            indexView.domElements.filter.removeClass("historic-report");
        });
    }
};