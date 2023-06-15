"use strict";
var TotalEnteredNonEnteredComponent = /** @class */ (function () {
    function TotalEnteredNonEnteredComponent() {
    }
    TotalEnteredNonEnteredComponent.prototype.init = function () {
        this.nonEnteredYesterdayContainer = $("#total-non-entered-yesterday");
        this.eventDateYesterday = $(".event-date-yesterday");
        this.nonEnteredYesterday = this.nonEnteredYesterdayContainer.find("#total-non-entered-yesterday-number");
        this.enteredYesterdayContainer = $("#total-entered-yesterday");
        var nonEnteredTodayContainer = $("#total-non-entered-today");
        this.eventDateToday = nonEnteredTodayContainer.find("#total-non-entered-today-text");
        this.nonEnteredToday = nonEnteredTodayContainer.find("#total-non-entered-today-number");
    };
    TotalEnteredNonEnteredComponent.prototype.update = function (yesterday, today) {
        this.eventDateYesterday.text(yesterday.eventDate);
        if (yesterday.nonEntered > 0) {
            this.enteredYesterdayContainer.hide();
            this.nonEnteredYesterday.text(alphaReports.formatNumber(yesterday.nonEntered));
            this.nonEnteredYesterdayContainer.show();
        }
        else {
            this.nonEnteredYesterdayContainer.hide();
            this.enteredYesterdayContainer.show();
        }
        this.eventDateToday.text(today.eventDate);
        this.nonEnteredToday.text(alphaReports.formatNumber(today.nonEntered));
    };
    return TotalEnteredNonEnteredComponent;
}());
//# sourceMappingURL=total-entered-nonentered.js.map