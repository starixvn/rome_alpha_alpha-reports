"use strict";
class WeekRangeOption {
    constructor(
        public onClick = Function(),
        public selectorId = "labelWeekRange",
        public textFromDateFormat = "DD",
        public textToDateFormat = "DD MMM",
        public valueDateFormat = "MM/DD/YYYY") {
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.textFromDateFormat = textFromDateFormat;
        this.textToDateFormat = textToDateFormat;
        this.valueDateFormat = valueDateFormat;
    }
}

class WeekRangeComponent {
    private $: JQueryStatic;
    private moment: moment.MomentStatic;
    private weekRangeOption: WeekRangeOption;
    private labelWeekRange: JQuery;

    constructor($: JQueryStatic, moment: moment.MomentStatic) {
        this.$ = $;
        this.moment = moment;
    }

    init(options: WeekRangeOption) {
        this.weekRangeOption = options;
        this.labelWeekRange = $("#" + options.selectorId);
    }

    render(formData) {
        this.labelWeekRange.html(this.buildWeekRangeHtml(formData.fromDate, formData.toDate));
        const labelWeeks = this.labelWeekRange.find(".label-week");
        labelWeeks
            .off()
            .click((e) => {
                const $this = this.$(e.target);
                labelWeeks.removeClass("active");
                $this.addClass("active");
                this.weekRangeOption.onClick($this.data("fromdate"), $this.data("todate"));
            });
    }

    private buildWeekRangeHtml(fromDate: string, toDate: string) {
        const weekRange = [];
        const startDate = this.moment(fromDate);
        let endDate = this.moment(toDate);

        while (endDate.diff(startDate, "d") >= 0) {
            const endDateStartOfWeek = this.moment(endDate).startOf("isoWeek");
            const startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;

            const fromDateValue = startDateOrEndDateStartOfWeek.format(this.weekRangeOption.valueDateFormat);
            const toDateValue = endDate.format(this.weekRangeOption.valueDateFormat);
            const label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(this.weekRangeOption.textToDateFormat)
                : `${startDateOrEndDateStartOfWeek.format(this.weekRangeOption.textFromDateFormat)} - ${endDate.format(this.weekRangeOption.textToDateFormat)}`;
            weekRange.push(`<a href="javascript:;" class="label-week" data-fromdate="${fromDateValue}" data-todate="${toDateValue}">${label}</a>`);

            endDate = this.moment(endDateStartOfWeek).subtract(1, "d");
        }

        return weekRange.join("");
    }
}
