/// <reference path="core.ts" />
/// <reference path="components/link-builder.ts" />

class BaseReport {
    $: JQueryStatic;
    linkBuilderComponent: LinkBuilderComponent;

    $reportForm: JQuery;
    $pageTitle: JQuery;
    $tableReport: JQuery;

    $isFilterVisible: JQuery;
    $exportExcelForm: JQuery;

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
}


