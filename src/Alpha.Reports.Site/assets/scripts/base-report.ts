/// <reference path="core.ts" />

class BaseReport {
    $: JQueryStatic;
    linkBuilderComponent: LinkBuilderComponent;

    $reportForm: JQuery;
    $pageTitle: JQuery;
    $tableReport: JQuery;

    $isFilterVisible: JQuery;
    $exportExcelForm: JQuery;

    $loadLegendURL: JQuery;
    $reportLegend: JQuery;
    $iconInformation: JQuery;
    $iconInformationContainer: JQuery;

    constructor($: JQueryStatic, linkBuilderComponent: LinkBuilderComponent) {
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }

    public cacheElements() {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$tableReport = this.$("#tbl-report");

        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
    }

    public registerEvents() {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
    }

    onClickIconFilter = () => {
        let filter = this.$reportForm;
        let currentHideState = filter.hasClass("hide");

        if (currentHideState) {
            this.$pageTitle.removeClass("hide-filter");
            this.$reportForm.removeClass("hide");
        } else {
            this.$pageTitle.addClass("hide-filter");
            this.$reportForm.addClass("hide");
        }

        this.replaceHrefParam("IsFilterVisible", currentHideState);
    };
    replaceHrefParam = (paramName: string, paramValue: any) => {
        this.linkBuilderComponent.updateParam(paramName, paramValue);
    };

    initHelp = () => {
        this.$loadLegendURL = this.$("#LoadLegendURL");
        this.$iconInformation = this.$("#icon-information");
        this.$iconInformationContainer = this.$("#icon-information-container");
        this.loadLegend();
        this.$iconInformation.on("click", this.showLegend);
    };
    loadLegend = () => {
        this.$.ajax({
            method: "GET",
            url: this.$loadLegendURL.val(),
            cache: false,
            data: {},
            success: this.renderLegend,
            complete: this.$.unblockUI
        });
    };
    renderLegend = (html: string) => {
        if (this.$reportLegend) {
            this.$reportLegend.remove();
        }

        this.$("body").append(html);

        if (this.$.trim(html) === "") {
            this.$iconInformationContainer.remove();
        } else {
            this.$iconInformationContainer.removeClass("hide");
        }

        this.$reportLegend = this.$("#report-legend");
    };

    showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });

        var iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            top: this.$(".entered-non-entered").length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    };
}