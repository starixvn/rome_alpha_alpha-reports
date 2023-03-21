"use strict";
var TotalEnteredNonEnteredOption = /** @class */ (function () {
    function TotalEnteredNonEnteredOption(nonEnteredText, enteredText, selectorId, dateFormat) {
        if (selectorId === void 0) { selectorId = "total-entered-non-entered"; }
        if (dateFormat === void 0) { dateFormat = "MM/DD"; }
        this.nonEnteredText = nonEnteredText;
        this.enteredText = enteredText;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
    return TotalEnteredNonEnteredOption;
}());
var TotalEnteredNonEnteredComponent = /** @class */ (function () {
    function TotalEnteredNonEnteredComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    TotalEnteredNonEnteredComponent.prototype.init = function (options) {
        this.totalEnteredNonEnteredOption = options;
        this.totalEnteredNonEntered = this.$("#" + options.selectorId);
    };
    TotalEnteredNonEnteredComponent.prototype.render = function (data) {
        var html = [];
        data.EnteredSportYesterday.NonEntered
            ? this.renderNonEntered(html, data.EnteredSportYesterday)
            : this.renderEntered(html, data.EnteredSportYesterday);
        this.renderNonEntered(html, data.EnteredSportToday);
        this.totalEnteredNonEntered.html(html.join(""));
    };
    TotalEnteredNonEnteredComponent.prototype.renderNonEntered = function (html, data) {
        html.push('<div class="icon-non-entered"><div class="text">', this.moment(data.EventDate).format(this.totalEnteredNonEnteredOption.dateFormat), ': ', this.totalEnteredNonEnteredOption.nonEnteredText, '</div><div class="number">', data.NonEntered, '</div><div class="icon-clock"></div></div>');
    };
    TotalEnteredNonEnteredComponent.prototype.renderEntered = function (html, data) {
        html.push('<div class="icon-entered"><div class="text">', this.moment(data.EventDate).format(this.totalEnteredNonEnteredOption.dateFormat), ': ', this.totalEnteredNonEnteredOption.enteredText, '</div><div class="icon-check-circle"></div>');
    };
    return TotalEnteredNonEnteredComponent;
}());
//# sourceMappingURL=total-entered-nonentered.js.map