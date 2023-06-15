"use strict";
var BreadcrumbLevelOption = /** @class */ (function () {
    function BreadcrumbLevelOption(breadcrumbs, onClick, selectorId) {
        if (breadcrumbs === void 0) { breadcrumbs = []; }
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "breadcrumb"; }
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.breadcrumbs = breadcrumbs;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return BreadcrumbLevelOption;
}());
var BreadcrumbLevelComponent = /** @class */ (function () {
    function BreadcrumbLevelComponent() {
    }
    BreadcrumbLevelComponent.prototype.init = function (options) {
        this.breadcrumbLevelOption = options;
        this.breadcrumbSelector = $("#" + options.selectorId);
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
    BreadcrumbLevelComponent.prototype.render = function () {
        var _this = this;
        var breadcrumbLength = this.breadcrumbLevelOption.breadcrumbs.length;
        if (breadcrumbLength > 1) {
            var html = this.breadcrumbLevelOption.breadcrumbs.map(function (item, index) {
                return index + 1 === breadcrumbLength
                    ? "<li>" + item.custName + "</li>"
                    : "<li><a class=\"upline-link\" href=\"javascript:;\" data-custid=\"" + item.custId + "\" data-custname=\"" + item.custName + "\" data-custlevelid=\"" + item.custLevelId + "\">" + item.custName + "</a></li>";
            });
            this.breadcrumbSelector
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
        }
        else {
            this.breadcrumbSelector.html("");
        }
    };
    return BreadcrumbLevelComponent;
}());
//# sourceMappingURL=breadcrumb-level.js.map