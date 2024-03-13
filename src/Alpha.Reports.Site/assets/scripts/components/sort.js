/// <reference path="../core.ts" />
"use strict";
var SortOption = /** @class */ (function () {
    function SortOption(sortContainer, canSort, hasAllDataOnClient, afterSortFunction, headers, reportForm) {
        if (sortContainer === void 0) { sortContainer = null; }
        if (canSort === void 0) { canSort = true; }
        if (hasAllDataOnClient === void 0) { hasAllDataOnClient = false; }
        if (afterSortFunction === void 0) { afterSortFunction = null; }
        if (headers === void 0) { headers = null; }
        if (reportForm === void 0) { reportForm = null; }
        this.sortContainer = sortContainer;
        this.canSort = canSort;
        this.hasAllDataOnClient = hasAllDataOnClient;
        this.afterSortFunction = afterSortFunction;
        this.headers = headers;
        this.reportForm = reportForm;
    }
    return SortOption;
}());
var SortComponent = /** @class */ (function () {
    function SortComponent($, linkBuilderComponent) {
        var _this = this;
        this.options = new SortOption();
        this.render = function (options) {
            if (options === void 0) { options = new SortOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$sortContainer = _this.options.sortContainer;
            _this.$reportForm = _this.options.reportForm;
            var sortColumns = _this.$sortContainer.find("[data-sortedname]");
            if (!_this.options.canSort) {
                sortColumns.removeAttr("data-sortedname").removeClass("sorting");
                return;
            }
            var self = _this;
            if (!_this.options.hasAllDataOnClient) {
                _this.$sortContainer
                    .find("[data-sortedname='" + _this.$sortContainer.data("sortedcolumn") + "']")
                    .addClass(_this.getSortClassWhenSortOnServer());
                sortColumns
                    .off("click.sortcomponent")
                    .on("click.sortcomponent", function () {
                    var $sortColumn = self.$(this);
                    self.onSort($sortColumn.data("sortedname"), self.getSortClassWhenSortOnServer() === "sorting-asc");
                });
            }
            _this.bindTableSorter(_this.$sortContainer.data("sortedcolumn"));
            return sortColumns;
        };
        this.getSortClassWhenSortOnServer = function () {
            var isDecendingSort = _this.$sortContainer.data("descendingsort").toString() === "1"
                || _this.$sortContainer.data("descendingsort").toLowerCase() === "true";
            return isDecendingSort ? "sorting-desc" : "sorting-asc";
        };
        this.onSort = function (sortedColumnName, isDecendingSort) {
            _this.$sortContainer
                .data("descendingsort", isDecendingSort ? "1" : "0")
                .data("sortedcolumn", sortedColumnName);
            if (_this.options.afterSortFunction &&
                typeof _this.options.afterSortFunction === "function") {
                _this.options.afterSortFunction();
            }
        };
        this.getSortInfo = function () {
            return {
                sortPropertyName: _this.$sortContainer.data("sortedcolumn"),
                isDescendingSort: _this.$sortContainer.data("descendingsort").toString() === "1"
                    || _this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true"
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
            var sortList = [];
            if (sortedColumnName && sortedColumnName !== "") {
                var sortedColumnHeader = _this.$sortContainer.find("[data-sortedname='" + sortedColumnName + "']" +
                    (_this.$sortContainer.hasClass("stack-mode") ? ":not([class*='hidden-stack-mode'])" : ":not([class*='col-combine'])"))[0];
                var isDescendingSort = _this.$sortContainer.data("descendingsort").toString() === "1"
                    || _this.$sortContainer.data("descendingsort").toString().toLowerCase() === "true";
                sortList = [[_this.$(sortedColumnHeader).data("colindex"), isDescendingSort ? 1 : 0]];
            }
            try {
                _this.$sortContainer.tablesorter({
                    defaultParser: 1,
                    headers: _this.options.headers,
                    sortList: sortList,
                    textExtraction: _this.textExtraction
                });
            }
            catch (e) {
                console.log(e);
            }
            _this.$sortContainer.off("sortEnd").on("sortEnd", function () {
                var $sortedcolumn = _this.$sortContainer
                    .find("thead.tableFloatingHeaderOriginal th.sorting.sorting-asc:visible" +
                    ",thead.tableFloatingHeaderOriginal th.sorting.sorting-desc:visible");
                var modeFilterClass = _this.$sortContainer.hasClass("stack-mode")
                    ? ".hidden-stack-mode"
                    : ".visible-stack-mode";
                $sortedcolumn = $sortedcolumn.not(modeFilterClass);
                if ($sortedcolumn.length === 1) {
                    _this.onSort($sortedcolumn.attr("data-sortedname"), $sortedcolumn.hasClass("sorting-desc"));
                }
                var dormantAccountsRow = _this.$sortContainer.find('.dormant-accounts-mark');
                dormantAccountsRow.length && _this.$sortContainer.append(dormantAccountsRow);
            });
        };
        this.textExtraction = function (node) {
            var $node = _this.$(node);
            if ($node.data("islink")) {
                return $node.find("a").html();
            }
            if ($node.data("sortedvalue")) {
                return String($node.data("sortedvalue"));
            }
            var html = $node.hasClass("combinedcell") ?
                $node.find("ul>li:first-child").html() :
                $node.html();
            return html;
        };
        this.afterSort = function () {
            var this$ = _this.$;
            if (_this.options.hasAllDataOnClient) {
                var sortedColumn = this$(".sorting-asc:visible");
                var direction = !sortedColumn.length;
                if (sortedColumn.length === 0) {
                    sortedColumn = this$(".sorting-desc");
                }
                var sortedColumnName = sortedColumn.data("sortedname");
                _this.linkBuilderComponent.updateParam("SortPropertyName", sortedColumnName || sortedColumn.parent().data("sortedname"));
                _this.linkBuilderComponent.updateParam("IsDescendingSort", direction.toString());
                alphaReports.correctEvenRowClass(_this.options.sortContainer);
            }
            else {
                _this.$reportForm.trigger("submit");
            }
        };
        this.mapSortedColumnToStackMode = function (sortedColumn, stackColumnGroups) {
            for (var i = 0; i < stackColumnGroups.length; i++) {
                var stackColumnGroup = stackColumnGroups[i];
                var firstColum = stackColumnGroup[0];
                for (var j = 0; j < stackColumnGroup.length; j++) {
                    if (sortedColumn.toLowerCase() === stackColumnGroup[j].toLowerCase()) {
                        return firstColum;
                    }
                }
            }
            return sortedColumn;
        };
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }
    return SortComponent;
}());
//# sourceMappingURL=sort.js.map