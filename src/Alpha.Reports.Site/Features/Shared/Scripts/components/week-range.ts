"use strict";
class WeekRangeOption {
    constructor(
        public onClick = Function(),
        public language = "en",
        public selectorId = "labelWeekRange") {
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
    }
}

class WeekRangeComponent {
    private weekRangeOption: WeekRangeOption;
    private labelWeekRange: JQuery;

    init(options: WeekRangeOption) {
        this.weekRangeOption = options;
        this.labelWeekRange = $("#" + options.selectorId);
    }

    render(formData) {
        moment.locale(this.weekRangeOption.language);
        this.labelWeekRange.html(this.buildWeekRangeHtml(formData.fromDate, formData.toDate));
        const labelWeeks = this.labelWeekRange.find(".label-week");
        labelWeeks
            .off()
            .click((e) => {
                const $this = $(e.target);
                labelWeeks.removeClass("active");
                $this.addClass("active");
                this.weekRangeOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
            });
    }

    private buildWeekRangeHtml(fromDate: string, toDate: string) {
        const weekRange = [];
        const startDate = moment(fromDate);
        let endDate = moment(toDate);

        while (endDate.diff(startDate, "d") >= 0) {
            const endDateStartOfWeek = moment(endDate).startOf("isoWeek");
            const startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;

            const fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            const toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            const label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : `${startDateOrEndDateStartOfWeek.format(alphaReports.format.day)} - ${endDate.format(alphaReports.format.dayMonthName)}`;
            weekRange.push(`<a href="javascript:;" class="label-week" data-fromdate="${fromDateValue}" data-todate="${toDateValue}">${label}</a>`);

            endDate = moment(endDateStartOfWeek).subtract(1, "d");
        }

        return weekRange.join("");
    }
}
