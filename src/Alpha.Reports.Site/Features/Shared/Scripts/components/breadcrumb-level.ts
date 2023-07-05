"use strict";
class BreadcrumbLevelOption {
    constructor(
        public breadcrumbs = [],
        public onClick = Function(),
        public reportTitle: any,
        public selectorId = "breadcrumb",
        public reportTitleSelectorId = "report-title") {
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.reportTitleSelectorId = reportTitleSelectorId;
    }
}

class BreadcrumbLevelComponent {
    private breadcrumbLevelOption: BreadcrumbLevelOption;
    private breadcrumbSelector: JQuery;
    private reportTitleSelector: JQuery;

    init(options: BreadcrumbLevelOption) {
        this.breadcrumbLevelOption = options;
        this.breadcrumbSelector = $("#" + options.selectorId);
        this.reportTitleSelector = $("#" + options.reportTitleSelectorId);
        this.render();
    }

    addBreadcrumbLevel(breadcrumbData: any) {
        this.breadcrumbLevelOption.breadcrumbs.push(breadcrumbData);
        this.render();
    }

    removeBreadcrumbLevel(custLevelId: number) {
        this.breadcrumbLevelOption.breadcrumbs = this.breadcrumbLevelOption.breadcrumbs.filter((item) => item.custLevelId >= custLevelId);
        this.render();
    }

    getBreadcrumbs() {
        return this.breadcrumbLevelOption.breadcrumbs;
    }

    setBreadcrumbs(breadcrumbs: any) {
        this.breadcrumbLevelOption.breadcrumbs = breadcrumbs;
        this.render();
    }

    private render() {
        const breadcrumbLength = this.breadcrumbLevelOption.breadcrumbs.length;
        const html = this.breadcrumbLevelOption.breadcrumbs.map(function (item, index) {
            return index + 1 === breadcrumbLength
                ? `<li>${item.custName}</li>`
                : `<li><a class="upline-link" href="javascript:;" data-custid="${item.custId}" data-custname="${item.custName}" data-custlevelid="${item.custLevelId}">${item.custName}</a></li>`;
        });

        this.reportTitleSelector.text(
            this.breadcrumbLevelOption.reportTitle[this.breadcrumbLevelOption.breadcrumbs[breadcrumbLength - 1].custLevelId]);
        this.breadcrumbSelector
            .html(html.join(""))
            .find(".upline-link").off().click((e) => {
                e.preventDefault();
                const $this = $(e.target);
                $this.addClass("active");
                this.breadcrumbLevelOption.onClick({
                    custId: $this.data("custid"),
                    custName: $this.data("custname"),
                    custLevelId: $this.data("custlevelid")
                });
            });
    }
}