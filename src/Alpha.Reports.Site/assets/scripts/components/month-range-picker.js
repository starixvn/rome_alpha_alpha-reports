/// <reference path="../core.ts" />
"use strict";
var MonthRangePickerOption = /** @class */ (function () {
    function MonthRangePickerOption(fromSelectorId, toSelectorId, reportFormId) {
        if (fromSelectorId === void 0) { fromSelectorId = "from-monthpicker"; }
        if (toSelectorId === void 0) { toSelectorId = "to-monthpicker"; }
        if (reportFormId === void 0) { reportFormId = "report-form"; }
        this.fromSelectorId = fromSelectorId;
        this.toSelectorId = toSelectorId;
        this.reportFormId = reportFormId;
    }
    return MonthRangePickerOption;
}());
var MonthRangePickerComponent = /** @class */ (function () {
    function MonthRangePickerComponent($) {
        var _this = this;
        this.options = new MonthRangePickerOption();
        this.render = function (options) {
            if (options === void 0) { options = new MonthRangePickerOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$dropdownFromMonth = _this.renderDropdown(_this.options.fromSelectorId);
            _this.$dropdownToMonth = _this.renderDropdown(_this.options.toSelectorId);
        };
        this.renderDropdown = function (dropdownId) {
            var dropdownMonth = _this.$("#" + dropdownId);
            var maxDate = dropdownMonth.data("maxdate");
            dropdownMonth.monPickr({
                longMonthsInYear: dropdownMonth.data("longmonths-inyear").split(","),
                maxDate: maxDate,
                minDate: dropdownMonth.data("mindate"),
                month: new Date(dropdownMonth.data("dateinmonth")),
                today: new Date(dropdownMonth.data("currentdate")),
                showDateRange: false,
                monthFormat: alphaReports.format.monthFormat,
                fromDayId: "ddl-fromday",
                toDayId: "ddl-today",
                fromDayLabel: "",
                toDayLabel: "-",
                sortDesc: true,
                log: _this.logMessage,
                afterChangeMonth: _this.onChange
            });
            return dropdownMonth;
        };
        this.logMessage = function (msg) {
            console.warn(msg);
        };
        this.onChange = function () {
            _this.$fromDate.val(_this.$dropdownFromMonth.monPickr("getFromDate", alphaReports.format.shortDateWithSlash_MonthPicker));
            _this.$toDate.val(_this.$dropdownToMonth.monPickr("getToDate", alphaReports.format.shortDateWithSlash_MonthPicker));
        };
        this.isLeast3Month = function () {
            var fromDate = new Date(_this.$fromDate.val());
            var toDate = new Date(_this.$toDate.val());
            return _this.getMonthDiff(fromDate, toDate) >= 2;
        };
        this.getMonthDiff = function (fromDate, toDate) {
            return toDate.getMonth() - fromDate.getMonth() + 12 * (toDate.getFullYear() - fromDate.getFullYear());
        };
        this.$ = $;
    }
    return MonthRangePickerComponent;
}());
//# sourceMappingURL=month-range-picker.js.map