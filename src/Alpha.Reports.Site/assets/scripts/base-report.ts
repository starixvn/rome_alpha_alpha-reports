/// <reference path="core.ts" />

class BaseReport {
    $loadLegendURL: JQuery;
    $reportLegend: JQuery;
    $iconHelp: JQuery;

    initHelp = () => {
        this.$loadLegendURL = $("#LoadLegendURL");
        this.$iconHelp = $("#icon-help");
        this.$iconHelp.on("click", this.loadLegend);
    };
    loadLegend = () => {
        $.blockUI();
        $.ajax({
            method: "GET",
            url: this.$loadLegendURL.val(),
            cache: false,
            data: {},
            success: this.renderLegend,
            complete: $.unblockUI
        });
    };
    renderLegend = (html: string) => {
        if (this.$reportLegend) {
            this.$reportLegend.remove();
        }

        $("body").append(html);
        this.$reportLegend = $("#report-legend");
        this.$reportLegend.modal({ show: true, backdrop: false });

        var iconHelpPosition = this.$iconHelp.position();
        this.$reportLegend.css({
            left: iconHelpPosition.left - 20,
            right: "auto",
            bottom: "auto",
            top: iconHelpPosition.top + 60,
            width: "auto"
        });
    };
}