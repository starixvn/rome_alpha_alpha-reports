"use strict";

class WeekRangeComponent {
    private $: JQueryStatic;
    private moment: moment.MomentStatic;

    constructor($: JQueryStatic, moment: moment.MomentStatic) {
        this.$ = $;
        this.moment = moment;
    }

    render(fromDate: string, toDate: string, onClick: Function) {
        const weekRange = [];
        const TextDateFormat = "MMM YYYYY";
        const ValueDateFormat = "MM/DD/YYYYY";
        const fromDateStartOfWeek = this.moment(fromDate).startOf("isoWeek");
        const toDateStartOfWeek = this.moment(toDate).startOf("isoWeek");

        if (!fromDateStartOfWeek.isSame(toDateStartOfWeek)) {
            const startDate = moment(fromDate);
            let endDate = moment(toDate);

            while (endDate.diff(startDate, "d") >= 0) {
                const endDateStartOfWeek = moment(endDate).startOf("isoWeek");
                const startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;

                const fromDateValue = startDateOrEndDateStartOfWeek.format(ValueDateFormat);
                const toDateValue = endDate.format(ValueDateFormat);
                const label = `${startDateOrEndDateStartOfWeek.format(TextDateFormat)} - ${endDate.format(TextDateFormat)}`;
                weekRange.push(`<span class="label label-default label-week-range" data-fromdate="${fromDateValue}" data-todate="${toDateValue}">${label}</span>`);

                endDate = moment(endDateStartOfWeek).subtract(1, "d");
            }
        }

        this.$("#labelWeekRange")
            .html(weekRange.join(""))
            .find(".label-week-range")
            .off()
            .click((e) => {
                const $this = this.$(e.target);
                onClick && onClick($this.data("fromdate"), $this.data("todate"));
            });
    }
}