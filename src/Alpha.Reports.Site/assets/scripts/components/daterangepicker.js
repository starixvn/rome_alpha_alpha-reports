/// <reference path="../core.ts" />
"use strict";
var DateRangePickerOption = (function () {
    function DateRangePickerOption(selectorId) {
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        this.selectorId = selectorId;
    }
    return DateRangePickerOption;
}());
var DateRangePickerComponent = (function () {
    function DateRangePickerComponent($, options) {
        var _this = this;
        if (options === void 0) { options = new DateRangePickerOption(); }
        this.options = new DateRangePickerOption();
        this.render = function () {
            var dateRangePicker = _this.$("#" + _this.options.selectorId);
            var currentDate = dateRangePicker.data("currentdate");
            var minDate = dateRangePicker.data("mindate");
            var maxDate = dateRangePicker.data("maxdate");
            dateRangePicker.nexDatePicker({
                datePickerFormat: {
                    dateFormat: alphaReports.format.shortDateWithSlash_DatePicker
                },
                selectedItem: dateRangePicker.data("selecteddaterange"),
                minDate: new Date(minDate),
                maxDate: maxDate === "" ? null : maxDate,
                currentDate: new Date(currentDate)
            });
            _this.$("#dropdown-daterange").multiselect({
                maxHeight: 300
            });
            return dateRangePicker;
        };
        this.$ = $;
        this.options = this.$.extend({}, this.options, options);
    }
    return DateRangePickerComponent;
}());
//# sourceMappingURL=daterangepicker.js.map