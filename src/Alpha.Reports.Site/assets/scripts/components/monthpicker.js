/// <reference path="../core.ts" />
"use strict";
var MonthPickerOption = (function () {
    function MonthPickerOption(selectorId, longMonthsInYear) {
        if (selectorId === void 0) { selectorId = "dropdown-month"; }
        if (longMonthsInYear === void 0) { longMonthsInYear = null; }
        this.selectorId = selectorId;
    }
    return MonthPickerOption;
}());
var MonthPickerComponent = (function () {
    function MonthPickerComponent($) {
        var _this = this;
        this.options = new MonthPickerOption();
        this.render = function (options) {
            if (options === void 0) { options = new MonthPickerOption; }
            var pickerOptions = _this.$.extend({}, _this.options, options);
            var dropdownMonth = _this.$("#" + pickerOptions.selectorId);
            var maxDate = dropdownMonth.data("maxdate");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            dropdownMonth.monPickr({
                longMonthsInYear: dropdownMonth.data("longmonths-inyear").split(","),
                maxDate: maxDate === "" ? dropdownMonth.data("currentdate") : maxDate,
                minDate: dropdownMonth.data("mindate"),
                month: new Date(dropdownMonth.data("dateinmonth")),
                fromDay: dropdownMonth.data("fromdate"),
                toDay: dropdownMonth.data("todate"),
                showDateRange: true,
                monthFormat: alphaReports.format.monthFormat,
                fromDayId: "ddl-fromday",
                toDayId: "ddl-today",
                fromDayLabel: "",
                toDayLabel: "-",
                sortDesc: true,
                log: function (msg) {
                    console.warn(msg);
                },
                afterChangeMonth: _this.onChange,
                afterChangeDay: _this.onChange
            });
            _this.$dropdownMonth = dropdownMonth;
            return dropdownMonth;
        };
        this.onChange = function () {
            _this.$fromDate.val(_this.$dropdownMonth.monPickr("getFromDate", alphaReports.format.shortDateWithSlash_MonthPicker));
            _this.$toDate.val(_this.$dropdownMonth.monPickr("getToDate", alphaReports.format.shortDateWithSlash_MonthPicker));
        };
        this.$ = $;
    }
    return MonthPickerComponent;
}());
//# sourceMappingURL=monthpicker.js.map