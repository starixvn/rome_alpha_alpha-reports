"use strict";
class WeekRangeOption {
    constructor(
        public onClick = Function(),
        public language = "en",
        public selectorId = "week-range-container") {
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
    }
}

class WeekRangeComponent {
    private weekRangeOption: WeekRangeOption;
    private weekRangeContainer: JQuery;

    init(options: WeekRangeOption) {
        this.weekRangeOption = options;
        this.weekRangeContainer = $("#" + options.selectorId);
    }

    render(dateRange, activeWeekRange) {
        moment.locale(this.weekRangeOption.language);
        this.weekRangeContainer.html(this.buildWeekRangeHtml(dateRange, activeWeekRange));
        const labelWeeks = this.weekRangeContainer.find(".label-week");
        labelWeeks
            .off()
            .click((e) => {
                const $this = $(e.target);
                labelWeeks.removeClass("active");
                $this.addClass("active");
                this.weekRangeOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
            });
    }

    private buildWeekRangeHtml(dateRange, activeWeekRange) {
        const weekRange = [];
        const startDate = moment(dateRange.fromDate);
        let endDate = moment(dateRange.toDate);

        while (endDate.diff(startDate, "d") >= 0) {
            const endDateStartOfWeek = moment(endDate).startOf("isoWeek");
            const startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;

            const fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            const toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            const label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : `${startDateOrEndDateStartOfWeek.format(alphaReports.format.day)} - ${endDate.format(alphaReports.format.dayMonthName)}`;
            const activeClassName = activeWeekRange
                && activeWeekRange.weekFromDate === fromDateValue
                && activeWeekRange.weekToDate === toDateValue ? " active" : ""
            weekRange.push(`<a href="javascript:;" class="label-week${activeClassName}" data-fromdate="${fromDateValue}" data-todate="${toDateValue}">${label}</a>`);

            endDate = moment(endDateStartOfWeek).subtract(1, "d");
        }

        return weekRange.join("");
    }

    getActiveWeekRange() {
        const activeWeekRange = this.weekRangeContainer.find(".label-week.active");
        if (activeWeekRange.length) {
            const firstActiveWeekRange = activeWeekRange.eq(0);
            return {
                weekFromDate: firstActiveWeekRange.data("fromdate"),
                weekToDate: firstActiveWeekRange.data("todate"),
            };
        }

        return {};
    }
}
