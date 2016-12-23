/// <reference path="../core.ts" />
"use strict";
var SortOption = (function () {
    function SortOption(sortContainer, canSort, hasAllDataOnClient, afterSortFunction, headers) {
        if (sortContainer === void 0) { sortContainer = null; }
        if (canSort === void 0) { canSort = true; }
        if (hasAllDataOnClient === void 0) { hasAllDataOnClient = false; }
        if (afterSortFunction === void 0) { afterSortFunction = null; }
        if (headers === void 0) { headers = null; }
        this.sortContainer = sortContainer;
        this.canSort = canSort;
        this.hasAllDataOnClient = hasAllDataOnClient;
        this.afterSortFunction = afterSortFunction;
        this.headers = headers;
    }
    return SortOption;
}());
var SortComponent = (function () {
    function SortComponent($) {
        var _this = this;
        this.options = new SortOption();
        this.render = function (options) {
            if (options === void 0) { options = new SortOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$sortContainer = _this.options.sortContainer;
            var sortColumns = _this.$sortContainer.find("[data-sortedname]");
            if (!_this.options.canSort) {
                sortColumns.removeAttr("data-sortedname").removeClass("sorting");
                return;
            }
            var self = _this;
            if (!_this.options.hasAllDataOnClient) {
                var isDecendingSort = _this.$sortContainer.data("descendingsort").toLowerCase() === "true";
                var sortClass = isDecendingSort ? "sorting-desc" : "sorting-asc";
                _this.$sortContainer
                    .find("[data-sortedname='" + _this.$sortContainer.data("sortedcolumn") + "']")
                    .addClass(sortClass);
                sortColumns
                    .off("click.sortcomponent")
                    .on("click.sortcomponent", function () {
                    self.onSort(self.$(this).data("sortedname"));
                });
            }
            _this.bindTableSorter(_this.$sortContainer.data("sortedcolumn"));
            return sortColumns;
        };
        this.onSort = function (sortedColumnName) {
            var descendingsort = sortedColumnName !== _this.$sortContainer.data("sortedcolumn")
                || _this.$sortContainer.data("descendingsort").toString().toLowerCase() !== "true"
                    && _this.$sortContainer.data("descendingsort") !== "1";
            _this.$sortContainer
                .data("descendingsort", descendingsort ? "1" : "0")
                .data("sortedcolumn", sortedColumnName);
            if (_this.options.afterSortFunction &&
                typeof _this.options.afterSortFunction === "function") {
                _this.options.afterSortFunction();
            }
        };
        this.getSortInfo = function () {
            return {
                sortPropertyName: _this.$sortContainer.data("sortedcolumn"),
                isDescendingSort: _this.$sortContainer.data("descendingsort") ? true : false
            };
        };
        this.bindTableSorter = function (sortedColumnName) {
            if (!_this.options.canSort || !_this.options.hasAllDataOnClient) {
                return;
            }
            var self = _this;
            var disabledSortColumns = _this.$sortContainer.find("th[data-disabledsort='true']");
            disabledSortColumns.each(function () {
                self.options.headers[this.cellIndex] = { sorter: false };
            });
            var sortedColumnHeader = _this.$sortContainer.find("[data-sortedname='" + sortedColumnName + "']" +
                (_this.$sortContainer.hasClass("stack-mode") ? ":not([class*='hidden-stack-mode'])" : ":not([class*='col-combine'])"))[0];
            var isDescendingSort = _this.$sortContainer.data("descendingsort") === "1" ||
                _this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true";
            _this.$sortContainer.tablesorter({
                defaultParser: 1,
                headers: _this.options.headers,
                sortList: [[_this.$(sortedColumnHeader).data("colindex"), isDescendingSort ? 1 : 0]],
                textExtraction: function (node) {
                    var $node = self.$(node);
                    if ($node.hasClass("combinedcell")) {
                        return $node.find("ul>li:first-child").html();
                    }
                    if ($node.data("islink")) {
                        return $node.find("a").html();
                    }
                    return $node.html();
                }
            });
            _this.$sortContainer.off("sortEnd").on("sortEnd", _this.options.afterSortFunction);
        };
        this.$ = $;
    }
    return SortComponent;
}());
//# sourceMappingURL=sort.js.map