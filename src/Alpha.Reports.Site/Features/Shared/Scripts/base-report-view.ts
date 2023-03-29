"use strict";

enum CustomerLevel {
    Super = 4,
    Master = 3,
    Agent = 2
}

class BaseReportView {
    protected $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    protected initReportLegend(legendUrl: string, width: number, height: number) {
        this.$("#icon-report-legend").click(function () {
            top.window["popupManager"].open(
                legendUrl,
                undefined,
                width,
                height,
                "modal-mobile-friendly")
        });
    }

    protected isNotUndefined(value: any) {
        return !this.isUndefined(value);
    }

    protected isUndefined(value: any) {
        return typeof value === "undefined";
    }

    protected formatInt(value: number) {
        return $.fn.dataTable.render.number(",", ".", 0).display(value);
    }

    protected formatDecimal(value: number) {
        const text = $.fn.dataTable.render.number(",", ".", 2).display(Math.round(Math.abs(value) * 100) / 100);
        return value >= 0 ? text : "-" + text;
    }

    protected renderEllipsis(value: string, cutoff: number) {
        return value.length > cutoff
            ? `<span data-toggle="tooltip" data-placement="bottom" title="${value}">${value.substr(0, cutoff)}&#8230;</span>`
            : value;
    }
}