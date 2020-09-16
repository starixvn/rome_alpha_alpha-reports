var DateRangePickerNewOption = /** @class */ (function () {
    function DateRangePickerNewOption(selectorId, reportFormId, onChange) {
        if (selectorId === void 0) { selectorId = null; }
        if (reportFormId === void 0) { reportFormId = null; }
        if (onChange === void 0) { onChange = null; }
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.selectorId = selectorId || "daterangepicker";
        this.reportFormId = reportFormId || "report-form";
        this.onChange = onChange;
    }
    return DateRangePickerNewOption;
}());
var DateRangePickerNewComponent = /** @class */ (function () {
    function DateRangePickerNewComponent($) {
        var _this = this;
        this.quickChooseType = {
            specificDates: 7
        };
        this.isCompleteInitDateRangePicker = false;
        this.render = function (options) {
            if (options === void 0) { options = new DateRangePickerNewOption(); }
            var format = alphaReports.format.shortDateWithSlash_DatePicker;
            var defaults = {
                locale: {
                    format: format
                },
                onChange: function (start, end, label) {
                    if (options.onChange) {
                        options.onChange(start, end, label);
                    }
                },
                onQuickChooseChange: function (selectedValue) {
                    if (_this.isCompleteInitDateRangePicker && selectedValue !== _this.quickChooseType.specificDates) {
                        var reportFormEl = _this.$("#" + options.reportFormId);
                        reportFormEl.submit();
                    }
                }
            };
            var dateRangePickerEl = _this.$("#" + options.selectorId);
            dateRangePickerEl.nexDateRangePicker(defaults);
            _this.isCompleteInitDateRangePicker = true;
            return dateRangePickerEl;
        };
        this.$ = $;
    }
    return DateRangePickerNewComponent;
}());
//# sourceMappingURL=daterangepicker-new.js.map