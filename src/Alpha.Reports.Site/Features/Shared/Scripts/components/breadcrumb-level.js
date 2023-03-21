"use strict";
var BreadcrumbLevelOption = /** @class */ (function () {
    function BreadcrumbLevelOption(onClick, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "breadcrumb"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return BreadcrumbLevelOption;
}());
var BreadcrumbLevelComponent = /** @class */ (function () {
    function BreadcrumbLevelComponent($) {
        this.breadcrumbs = [];
        this.$ = $;
    }
    BreadcrumbLevelComponent.prototype.init = function (options, custId, custName, custLevelId) {
        this.breadcrumbLevelOption = options;
        this.breadcrumbSelector = this.$("#" + options.selectorId);
        this.addBreadcrumbLevel(custId, custName, custLevelId);
    };
    BreadcrumbLevelComponent.prototype.addBreadcrumbLevel = function (custId, custName, custLevelId) {
        this.breadcrumbs.push({ custId: custId, custName: custName, custLevelId: custLevelId });
        this.render();
    };
    BreadcrumbLevelComponent.prototype.removeBreadcrumbLevel = function (custLevelId) {
        this.breadcrumbs = this.breadcrumbs.filter(function (item) { return item.custLevelId >= custLevelId; });
        this.render();
    };
    BreadcrumbLevelComponent.prototype.render = function () {
        var _this = this;
        var breadcrumbLength = this.breadcrumbs.length;
        if (breadcrumbLength > 1) {
            var html = this.breadcrumbs.map(function (item, index) {
                return index + 1 === breadcrumbLength
                    ? "<li>" + item.custName + "</li>"
                    : "<li><a class=\"upline-link\" href=\"javascript:;\" data-custid=\"" + item.custId + "\" data-custname=\"" + item.custName + "\" data-custlevelid=\"" + item.custLevelId + "\">" + item.custName + "</a></li>";
            });
            this.breadcrumbSelector
                .html(html.join(""))
                .removeClass("hide")
                .find(".upline-link").off().click(function (e) {
                e.preventDefault();
                var $this = _this.$(e.target);
                $this.addClass("active");
                _this.breadcrumbLevelOption.onClick($this.data("custid"), $this.data("custname"), $this.data("custlevelid"));
            });
        }
        else {
            this.breadcrumbSelector.addClass("hide").html("");
        }
    };
    return BreadcrumbLevelComponent;
}());
//# sourceMappingURL=breadcrumb-level.js.map