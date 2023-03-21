"use strict";
var TotalEnteredSportOption = /** @class */ (function () {
    function TotalEnteredSportOption(selectorId, dateFormat) {
        if (selectorId === void 0) { selectorId = "entered-non-entered-container"; }
        if (dateFormat === void 0) { dateFormat = "MM/DD"; }
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
    return TotalEnteredSportOption;
}());
var TotalEnteredSportComponent = /** @class */ (function () {
    function TotalEnteredSportComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    TotalEnteredSportComponent.prototype.init = function (options) {
        this.totalEnteredSportOption = options;
        this.enteredNonEnteredContainer = this.$("#" + options.selectorId);
        this.yesterdayNonenteredContainer = this.enteredNonEnteredContainer.find("#yesterday-nonentered-container");
        this.labelYesterday = this.enteredNonEnteredContainer.find(".label-yesterday");
        this.labelYesterdayNonentered = this.enteredNonEnteredContainer.find("#label-yesterday-nonentered");
        this.yesterdayEnteredContainer = this.enteredNonEnteredContainer.find("#yesterday-entered-container");
        this.labelToday = this.enteredNonEnteredContainer.find("#label-today");
        this.labelTodayNonentered = this.enteredNonEnteredContainer.find("#label-today-nonentered");
    };
    TotalEnteredSportComponent.prototype.render = function (data) {
        this.assignEnteredNonEnteredYestoday(data.EnteredSportYesterday);
        this.assignNonEnteredToday(data.EnteredSportToday);
        this.enteredNonEnteredContainer.removeClass("hide");
    };
    TotalEnteredSportComponent.prototype.assignEnteredNonEnteredYestoday = function (data) {
        this.labelYesterday.text(this.moment(data.EventDate).format(this.totalEnteredSportOption.dateFormat));
        if (data.NonEntered) {
            this.labelYesterdayNonentered.text(data.NonEntered);
            this.yesterdayNonenteredContainer.removeClass("hide");
            this.yesterdayEnteredContainer.addClass("hide");
        }
        else {
            this.yesterdayNonenteredContainer.addClass("hide");
            this.yesterdayEnteredContainer.removeClass("hide");
        }
    };
    TotalEnteredSportComponent.prototype.assignNonEnteredToday = function (data) {
        this.labelToday.text(this.moment(data.EventDate).format(this.totalEnteredSportOption.dateFormat));
        this.labelTodayNonentered.text(data.NonEntered);
    };
    return TotalEnteredSportComponent;
}());
//# sourceMappingURL=total-entered-sport.js.map