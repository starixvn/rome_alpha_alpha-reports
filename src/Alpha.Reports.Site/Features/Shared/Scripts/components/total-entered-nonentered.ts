"use strict";

interface TotalEnteredNonEntered {
    eventDate: string,
    nonEntered: number,
}

class TotalEnteredNonEnteredComponent {
    private nonEnteredYesterdayContainer: JQuery;
    private eventDateYesterday: JQuery;
    private nonEnteredYesterday: JQuery;
    private enteredYesterdayContainer: JQuery;
    private eventDateToday: JQuery;
    private nonEnteredToday: JQuery;

    init() {
        this.nonEnteredYesterdayContainer = $("#total-non-entered-yesterday");
        this.eventDateYesterday = $(".event-date-yesterday");
        this.nonEnteredYesterday = this.nonEnteredYesterdayContainer.find("#total-non-entered-yesterday-number");
        this.enteredYesterdayContainer = $("#total-entered-yesterday");
        const nonEnteredTodayContainer = $("#total-non-entered-today");
        this.eventDateToday = nonEnteredTodayContainer.find("#total-non-entered-today-text");
        this.nonEnteredToday = nonEnteredTodayContainer.find("#total-non-entered-today-number");
    }

    update(yesterday: TotalEnteredNonEntered, today: TotalEnteredNonEntered) {
        this.eventDateYesterday.text(yesterday.eventDate);
        if (yesterday.nonEntered > 0) {
            this.enteredYesterdayContainer.hide();

            this.nonEnteredYesterday.text(alphaReports.formatNumber(yesterday.nonEntered));
            this.nonEnteredYesterdayContainer.show();
        } else {
            this.nonEnteredYesterdayContainer.hide();
            this.enteredYesterdayContainer.show();
        }

        this.eventDateToday.text(today.eventDate);
        this.nonEnteredToday.text(alphaReports.formatNumber(today.nonEntered));
    }
}