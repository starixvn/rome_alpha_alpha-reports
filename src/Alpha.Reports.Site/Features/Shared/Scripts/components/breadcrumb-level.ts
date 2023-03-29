"use strict";
class BreadcrumbLevelOption {
    constructor(
        public custId: number,
        public custName: string,
        public custLevelId: number,
        public onClick = Function(),
        public selectorId = "breadcrumb") {
        this.custId = custId;
        this.custName = custName;
        this.custLevelId = custLevelId;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
}

class BreadcrumbLevelComponent {
    private $: JQueryStatic;
    private breadcrumbLevelOption: BreadcrumbLevelOption;
    private breadcrumbSelector;
    private breadcrumbs = [];

    constructor($) {
        this.$ = $;
    }

    init(options: BreadcrumbLevelOption) {
        this.breadcrumbLevelOption = options;
        this.breadcrumbSelector = this.$("#" + options.selectorId);
        this.addBreadcrumbLevel({
            custId: options.custId,
            custName: options.custName,
            custLevelId: options.custLevelId
        });
    }

    addBreadcrumbLevel(breadcrumbData: any) {
        this.breadcrumbs.push(breadcrumbData);
        this.render();
    }

    removeBreadcrumbLevel(custLevelId: number) {
        this.breadcrumbs = this.breadcrumbs.filter((item) => item.custLevelId >= custLevelId);
        this.render();
    }

    private render() {
        const breadcrumbLength = this.breadcrumbs.length;
        if (breadcrumbLength > 1) {
            const html = this.breadcrumbs.map(function (item, index) {
                return index + 1 === breadcrumbLength
                    ? `<li>${item.custName}</li>`
                    : `<li><a class="upline-link" href="javascript:;" data-custid="${item.custId}" data-custname="${item.custName}" data-custlevelid="${item.custLevelId}">${item.custName}</a></li>`;
            });

            this.breadcrumbSelector
                .html(html.join(""))
                .removeClass("hide")
                .find(".upline-link").off().click((e) => {
                    e.preventDefault();
                    const $this = this.$(e.target);
                    $this.addClass("active");
                    this.breadcrumbLevelOption.onClick({
                        custId: $this.data("custid"),
                        custName: $this.data("custname"),
                        custLevelId: $this.data("custlevelid")
                    });
                });
        } else {
            this.breadcrumbSelector.addClass("hide").html("");
        }
    }
}