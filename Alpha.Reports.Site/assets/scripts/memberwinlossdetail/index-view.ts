/// <reference path="../core.ts" />
"use strict";

var indexView = {
    domElements: {
        dateRangePicker: null
    },
    init: function () {
        indexView.cacheDOMElements();
        indexView.renderDateRangePicker();
    },
    cacheDOMElements: function () {
    },
    renderDateRangePicker: function () {
        var dateRangePicker = $("#daterange-picker");

        var currentDate = dateRangePicker.data("currentdate");
        var minDate = dateRangePicker.data("mindate");

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
                Label: dateRangePicker.data("labeldate"),
                LabelCss: "",
                OptionCss: ""
            },
            firstDatePicker: {
                Id: "picker-fromdate",
                Label: dateRangePicker.data("labelfrom"),
                InitDate: currentDate,
                LabelCss: ""
            },
            secondDatePicker: {
                Id: "picker-todate",
                Label: dateRangePicker.data("labelto"),
                InitDate: currentDate,
                LabelCss: ""
            },
            selectedItem: 0,
            onSelectDatePicker: function () {
            },
            minDate: new Date(minDate),
            maxDate: null,
            optionItems: [
                { Name: dateRangePicker.data("labeltoday"), Value: 0 },
                { Name: dateRangePicker.data("labelyesterday"), Value: 1 },
                { Name: dateRangePicker.data("labelcurrentweek"), Value: 2 },
                { Name: dateRangePicker.data("labellastweek"), Value: 3 },
                { Name: dateRangePicker.data("labelcurrentmonth"), Value: 4 },
                { Name: dateRangePicker.data("labellastmonth"), Value: 5 },
                { Name: dateRangePicker.data("labelsincelastmonth"), Value: 6 },
                { Name: dateRangePicker.data("labelspecificdate"), Value: 7 }
            ]
        });

        indexView.domElements.dateRangePicker = $("#daterange-picker");
    }
};