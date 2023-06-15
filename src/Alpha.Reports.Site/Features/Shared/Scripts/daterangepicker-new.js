//class DateRangePickerNewOption {
//    constructor(
//        public selectorId: string = null,
//        public reportFormId: string = null,
//        public onChange: Function = null) {
//        this.selectorId = selectorId || "daterangepicker";
//        this.reportFormId = reportFormId || "report-form";
//        this.onChange = onChange;
//    }
//}
//class DateRangePickerNewComponent {
//    $: JQueryStatic;
//    quickChooseType = {
//        specificDates: 7
//    };
//    isCompleteInitDateRangePicker = false;
//    constructor($: JQueryStatic) {
//        this.$ = $;
//    }
//    render = (options: DateRangePickerNewOption = new DateRangePickerNewOption()) => {
//        const defaults = {
//            autoApply: false,
//            locale: {
//                format: alphaReports.format.shortDateWithSlash_DatePicker
//            },
//            onChange: (start, end, label) => {
//                if (options.onChange) {
//                    options.onChange(start, end, label);
//                }
//            },
//            onQuickChooseChange: (selectedValue) => {
//                if (this.isCompleteInitDateRangePicker && selectedValue !== this.quickChooseType.specificDates) {
//                    let reportFormEl = this.$("#" + options.reportFormId);
//                    reportFormEl.submit();
//                }
//            }
//        };
//        let dateRangePickerEl = this.$("#" + options.selectorId);
//        dateRangePickerEl.nexDateRangePicker(defaults);
//        this.isCompleteInitDateRangePicker = true;
//        return dateRangePickerEl;
//    };
//}
//# sourceMappingURL=daterangepicker-new.js.map