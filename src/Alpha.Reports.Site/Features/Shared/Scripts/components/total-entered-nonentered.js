"use strict";
var TotalEnteredNonEnteredComponent = /** @class */ (function () {
    function TotalEnteredNonEnteredComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    TotalEnteredNonEnteredComponent.prototype.init = function (selector) {
        if (selector === void 0) { selector = "total-entered-non-entered"; }
        this.totalEnteredNonEntered = this.$("#" + selector);
        this.labelNonEntered = this.totalEnteredNonEntered.data("label-nonentered");
        this.labelEntered = this.totalEnteredNonEntered.data("label-entered");
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
        html.push('<div class="icon-non-entered"><div class="text">', this.moment(data.EventDate).format(alphaReports.format.monthDayWithSlash), ': ', this.labelNonEntered, '</div><div class="number">', data.NonEntered, '</div><div class="icon-clock"></div></div>');
    };
    TotalEnteredNonEnteredComponent.prototype.renderEntered = function (html, data) {
        html.push('<div class="icon-entered"><div class="text">', this.moment(data.EventDate).format(alphaReports.format.monthDayWithSlash), ': ', this.labelEntered, '</div><div class="icon-check-circle"></div>');
    };
    return TotalEnteredNonEnteredComponent;
}());
//# sourceMappingURL=total-entered-nonentered.js.map