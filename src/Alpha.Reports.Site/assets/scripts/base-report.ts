/// <reference path="core.ts" />

class BaseReport {
    $loadLegendURL: JQuery;
    $reportLegend: JQuery;
    $iconInformation: JQuery;
    $iconInformationContainer: JQuery;

    initHelp = () => {
        this.$loadLegendURL = $("#LoadLegendURL");
        this.$iconInformation = $("#icon-information");
        this.$iconInformationContainer = $("#icon-information-container");
        this.loadLegend();
        this.$iconInformation.on("click", this.showLegend);
    };
    loadLegend = () => {
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

        if ($.trim(html) === "") {
            this.$iconInformationContainer.remove();
        } else {
            this.$iconInformationContainer.removeClass("hide");
        }

        this.$reportLegend = $("#report-legend");
    };

    showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });

        var iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            left: iconHelpPosition.left - 20,
            right: "auto",
            bottom: "auto",
            top: iconHelpPosition.top + 60,
            width: "auto"
        });
    };
}