"use strict";

class TotalEnteredNonEnteredOption {
    constructor(
        public nonEnteredText: string,
        public enteredText: string,
        public selectorId = "total-entered-non-entered",
        public dateFormat = "MM/DD") {
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
}

class TotalEnteredNonEnteredComponent {
    private $: JQueryStatic;
    private moment: moment.MomentStatic;
    private totalEnteredNonEnteredOption: TotalEnteredNonEnteredOption;
    private totalEnteredNonEntered: JQuery;

    constructor($: JQueryStatic, moment: moment.MomentStatic) {
        this.$ = $;
        this.moment = moment;
    }

    init(options: TotalEnteredNonEnteredOption) {
        this.totalEnteredNonEnteredOption = options;
        this.totalEnteredNonEntered = this.$("#" + options.selectorId);
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
            this.moment(data.EventDate).format(this.totalEnteredNonEnteredOption.dateFormat),
            ': ',
            this.totalEnteredNonEnteredOption.nonEnteredText,
            '</div><div class="number">',
            data.NonEntered,
            '</div><div class="icon-clock"></div></div>');
    }

    private renderEntered(html: string[], data: any) {
        html.push(
            '<div class="icon-entered"><div class="text">',
            this.moment(data.EventDate).format(this.totalEnteredNonEnteredOption.dateFormat),
            ': ',
            this.totalEnteredNonEnteredOption.enteredText,
            '</div><div class="icon-check-circle"></div>');
    }
}