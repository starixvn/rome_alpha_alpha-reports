"use strict";
var BreadcrumbLevelOption = /** @class */ (function () {
    function BreadcrumbLevelOption(breadcrumbs, onClick, reportTitle, selectorId, reportTitleSelectorId) {
        if (breadcrumbs === void 0) { breadcrumbs = []; }
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "breadcrumb-level"; }
        if (reportTitleSelectorId === void 0) { reportTitleSelectorId = "report-title"; }
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.reportTitle = reportTitle;
        this.selectorId = selectorId;
        this.reportTitleSelectorId = reportTitleSelectorId;
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.reportTitleSelectorId = reportTitleSelectorId;
    }
    return BreadcrumbLevelOption;
}());
var BreadcrumbLevelComponent = /** @class */ (function () {
    function BreadcrumbLevelComponent() {
    }
    BreadcrumbLevelComponent.prototype.init = function (options) {
        this.breadcrumbLevelOption = options;
        //this.breadcrumbSelector = $("#" + options.selectorId);
        this.reportTitleSelector = $("#" + options.reportTitleSelectorId);
        this.render();
    };
    BreadcrumbLevelComponent.prototype.addBreadcrumbLevel = function (breadcrumbData) {
        this.breadcrumbLevelOption.breadcrumbs.push(breadcrumbData);
        this.render();
    };
    BreadcrumbLevelComponent.prototype.removeBreadcrumbLevel = function (custLevelId) {
        this.breadcrumbLevelOption.breadcrumbs = this.breadcrumbLevelOption.breadcrumbs.filter(function (item) { return item.custLevelId >= custLevelId; });
        this.render();
    };
    BreadcrumbLevelComponent.prototype.getBreadcrumbs = function () {
        return this.breadcrumbLevelOption.breadcrumbs;
    };
    BreadcrumbLevelComponent.prototype.setBreadcrumbs = function (breadcrumbs) {
        this.breadcrumbLevelOption.breadcrumbs = breadcrumbs;
        this.render();
    };
    BreadcrumbLevelComponent.prototype.render = function () {
        var _this = this;
        var breadcrumbLength = this.breadcrumbLevelOption.breadcrumbs.length;
        var html = this.breadcrumbLevelOption.breadcrumbs.map(function (item, index) {
            return index + 1 === breadcrumbLength
                ? "<li>" + item.custName + "</li>"
                : "<li><a class=\"upline-link\" href=\"javascript:;\" data-custid=\"" + item.custId + "\" data-custname=\"" + item.custName + "\" data-custlevelid=\"" + item.custLevelId + "\">" + item.custName + "</a></li>";
        });
        this.reportTitleSelector.text(this.breadcrumbLevelOption.reportTitle[this.breadcrumbLevelOption.breadcrumbs[breadcrumbLength - 1].custLevelId]);
        $("#" + this.breadcrumbLevelOption.selectorId)
            .html(html.join(""))
            .find(".upline-link").off().click(function (e) {
            e.preventDefault();
            var $this = $(e.target);
            $this.addClass("active");
            _this.breadcrumbLevelOption.onClick({
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: $this.data("custlevelid")
            });
        });
    };
    return BreadcrumbLevelComponent;
}());
//# sourceMappingURL=breadcrumb-level.js.map