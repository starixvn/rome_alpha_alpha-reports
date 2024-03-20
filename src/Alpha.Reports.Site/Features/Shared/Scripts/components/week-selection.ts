"use strict";
class WeekSelectionOption {
    constructor(
        public onClick = Function(),
        public selectorId = "week-selection") {
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
}

class WeekSelectionComponent {
    private weekSelectionOption: WeekSelectionOption;
    private weekRangeContainer: JQuery;

    init(options: WeekSelectionOption) {
        this.weekSelectionOption = options;
        this.bindEvents();
    }

    bindEvents() {
        this.weekRangeContainer = $("#" + this.weekSelectionOption.selectorId);
        const labelWeeks = this.weekRangeContainer.find(".label-week:not(.disabled)");
        labelWeeks
            .off()
            .click((e) => {
                const $this = $(e.target);
                labelWeeks.removeClass("active");
                $this.addClass("active");
                this.weekSelectionOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
            });
    }

    getActiveWeek() {
        const activeWeek = this.weekRangeContainer.find(".label-week.active");
        if (activeWeek.length) {
            const firstActiveWeek = activeWeek.eq(0);
            return {
                weekFromDate: firstActiveWeek.data("fromdate"),
                weekToDate: firstActiveWeek.data("todate"),
            };
        }

        return {};
    }

    hide() {
        this.weekRangeContainer.addClass("hide");
    }

    show() {
        this.weekRangeContainer.removeClass("hide");
    }
}
