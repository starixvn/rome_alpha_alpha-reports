"use strict";

class TotalEnteredNonEnteredComponent {
    private $: JQueryStatic;
    private moment: moment.MomentStatic;
    private totalEnteredNonEntered: JQuery;
    private labelNonEntered: string;
    private labelEntered: string;

    constructor($: JQueryStatic, moment: moment.MomentStatic) {
        this.$ = $;
        this.moment = moment;
    }

    init(selector = "total-entered-non-entered") {
        this.totalEnteredNonEntered = this.$("#" + selector);
        this.labelNonEntered = this.totalEnteredNonEntered.data("label-nonentered");
        this.labelEntered = this.totalEnteredNonEntered.data("label-entered");
    }

    render(data: any) {
        const html = [];
        data.EnteredSportYesterday.NonEntered
            ? this.renderNonEntered(html, data.EnteredSportYesterday)
            : this.renderEntered(html, data.EnteredSportYesterday);
        this.renderNonEntered(html, data.EnteredSportToday);
        this.totalEnteredNonEntered.html(html.join(""));
    }

    private renderNonEntered(html: string[], data: any) {
        html.push(
            '<div class="icon-non-entered"><div class="text">',
            this.moment(data.EventDate).format(alphaReports.format.monthDayWithSlash),
            ': ',
            this.labelNonEntered,
            '</div><div class="number">',
            data.NonEntered,
            '</div><div class="icon-clock"></div></div>');
    }

    private renderEntered(html: string[], data: any) {
        html.push(
            '<div class="icon-entered"><div class="text">',
            this.moment(data.EventDate).format(alphaReports.format.monthDayWithSlash),
            ': ',
            this.labelEntered,
            '</div><div class="icon-check-circle"></div>');
    }
}