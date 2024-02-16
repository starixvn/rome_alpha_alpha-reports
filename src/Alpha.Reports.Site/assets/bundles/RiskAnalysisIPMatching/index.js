"use strict";
var AlphaReports = /** @class */ (function () {
    function AlphaReports($, store) {
        var _this = this;
        this.format = {
            monthFormat: "MMMM - yyyy",
            shortDateWithSlash_DatePicker: "MM/DD/YYYY",
            shortDateWithSlash_MonthPicker: "MM/dd/yyyy",
            monthDayWithSlash: "MM/DD",
            dayMonthName: "DD MMM",
            day: "DD"
        };
        this.sort = {
            none: 0,
            ascending: 1,
            descending: -1
        };
        this.keyCode = {
            enter: 13
        };
        this.numberDisplayedProductsOnFilter = 1;
        this.maxTimesToShowTooltip = 5;
        this.screenSmallThan768 = function () {
            var currentWindowWidth = screen.width || _this.$(window).width();
            return currentWindowWidth < 768;
        };
        this.screenLandscapeMode = function () { return screen.width > screen.height || _this.$(window).width() > _this.$(window).height(); };
        this.splitCell = function (firstCell, cellClass, lastCellStyle) {
            if (cellClass === void 0) { cellClass = ""; }
            if (lastCellStyle === void 0) { lastCellStyle = "bold"; }
            var this$ = _this.$;
            var combinedCells = _this.$("." + firstCell + "-combined");
            combinedCells.each(function (index) {
                var oldValues = this$(this).find("li");
                var cellsLength = oldValues.length;
                var oldCells = "";
                for (var i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                    oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                        (i === lastCellIndex ? " " + lastCellStyle : "") + "\">" +
                        this$(oldValues[i]).html() +
                        "</td>";
                }
                this$(combinedCells[index]).before(oldCells);
            });
        };
        this.combineConsecutiveCells = function (firstCells, numberOfCombinedCells, combinedCellClass, combinedCellColSpan) {
            if (combinedCellColSpan === void 0) { combinedCellColSpan = 1; }
            if (numberOfCombinedCells <= 1) {
                return;
            }
            var this$ = _this.$;
            firstCells.each(function (index) {
                var newCell = "<td class=\"visible-stack-mode combinedcell " +
                    combinedCellClass + "\" colspan=\"" + combinedCellColSpan + "\">\
                    <ul>";
                var currentCell = this$(firstCells[index]);
                var isLastCell = false;
                for (var i = 0; i < numberOfCombinedCells; i++) {
                    newCell += "<li>" + currentCell.html() + "</li>";
                    if (currentCell.next().length) {
                        currentCell = currentCell.next();
                    }
                    else {
                        isLastCell = true;
                    }
                }
                newCell += "</ul>\
                        </td>";
                if (isLastCell) {
                    currentCell.after(newCell);
                }
                else {
                    currentCell.prev().after(newCell);
                }
            });
        };
        this.localStorage = {
            get: function (key) {
                return _this.store.enabled ? _this.store.get(key) : _this.$.cookie(key);
            },
            set: function (key, value) {
                _this.store.enabled ? _this.store.set(key, value) : _this.$.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                if (_this.isIe()) {
                    cookieExpire = { expires: 365 };
                }
                return cookieExpire;
            }
        };
        this.isIe = function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
        };
        this.replaceQueryStringValue = function (queryString, paramName, paramValue) {
            if (typeof queryString === "undefined"
                || queryString === null) {
                return "";
            }
            var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");
            var newQueryString = regex.test(queryString)
                ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
                : queryString + (queryString.indexOf("?") >= 0 ? "&" : "?") + paramName + "=" + paramValue;
            return newQueryString;
        };
        this.correctEvenRowClass = function ($tableReport) {
            var this$ = _this.$;
            var isEvenRow = false;
            $tableReport.find("tbody > tr").each(function () {
                var $row = this$(this);
                if ($row.hasClass("even-row") && !isEvenRow) {
                    $row.removeClass("even-row");
                }
                else if (!$row.hasClass("even-row") && isEvenRow) {
                    $row.addClass("even-row");
                }
                isEvenRow = !isEvenRow;
            });
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.formatNumber = function (value) { return value.toLocaleString("en-US"); };
        this.formatCurrency = function (value) { return value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); };
        this.$ = $;
        this.store = store;
    }
    AlphaReports.prototype.getFormData = function ($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};
        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });
        return indexed_array;
    };
    return AlphaReports;
}());
var alphaReports = new AlphaReports($, store);
//# sourceMappingURL=core.js.map
/// <reference path="../core.ts" />
"use strict";
var DateRangePickerOption = /** @class */ (function () {
    function DateRangePickerOption(selectorId, reportFormId, onChange, onBeforeQuickChooseChange) {
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (reportFormId === void 0) { reportFormId = "report-form"; }
        if (onChange === void 0) { onChange = null; }
        if (onBeforeQuickChooseChange === void 0) { onBeforeQuickChooseChange = null; }
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
    }
    return DateRangePickerOption;
}());
var DateRangePickerComponent = /** @class */ (function () {
    function DateRangePickerComponent($) {
        var _this = this;
        this.options = new DateRangePickerOption();
        this.quickChooseType = {
            specificDates: 7,
            olderThan3Months: 8
        };
        this.isCompleteInitDateRangePicker = false;
        this.render = function (options) {
            if (options === void 0) { options = new DateRangePickerOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var dateRangePicker = _this.$("#" + _this.options.selectorId);
            var currentDate = dateRangePicker.data("currentdate");
            var minDate = dateRangePicker.data("mindate");
            var maxDate = dateRangePicker.data("maxdate");
            _this.$reportForm = _this.$("#" + _this.options.reportFormId);
            dateRangePicker.nexDatePicker({
                datePickerFormat: {
                    dateFormat: alphaReports.format.shortDateWithSlash_DatePicker
                },
                selectedItem: dateRangePicker.data("selecteddaterange"),
                minDate: new Date(minDate),
                maxDate: maxDate === "" ? null : maxDate,
                currentDate: new Date(currentDate),
                onSelectDatePicker: function () {
                    if (_this.options.onChange) {
                        _this.options.onChange();
                    }
                },
                onQuickChooseChange: function (selectValue) {
                    _this.options.onBeforeQuickChooseChange && _this.options.onBeforeQuickChooseChange();
                    if (_this.isCompleteInitDateRangePicker && selectValue !== _this.quickChooseType.specificDates) {
                        if (selectValue === _this.quickChooseType.olderThan3Months) {
                            _this.$reportForm.find("input[id=IsHistoricReport]").val("true");
                            _this.$reportForm.find("a[id=historic-report]").trigger("click");
                        }
                        else {
                            _this.$reportForm.submit();
                        }
                    }
                }
            });
            _this.isCompleteInitDateRangePicker = true;
            return dateRangePicker;
        };
        this.$ = $;
    }
    return DateRangePickerComponent;
}());
//# sourceMappingURL=daterangepicker.js.map
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
var LinkBuilderOption = /** @class */ (function () {
    function LinkBuilderOption(linkClasses) {
        if (linkClasses === void 0) { linkClasses = ["a.breadcrumb-link,a.downline-link"]; }
        this.linkClasses = linkClasses;
    }
    return LinkBuilderOption;
}());
var LinkBuilderComponent = /** @class */ (function () {
    function LinkBuilderComponent($) {
        var _this = this;
        this.options = new LinkBuilderOption();
        this.render = function (options) {
            if (options === void 0) { options = new LinkBuilderOption; }
            _this.options = _this.$.extend({}, _this.options, options);
        };
        this.updateParam = function (name, value) {
            var me = _this;
            _this.options.linkClasses.forEach(function (className) {
                var $linkClass = _this.$(className);
                $linkClass.each(function (index, element) {
                    var $link = me.$(element);
                    var currentUrl = $link.attr("href");
                    currentUrl = alphaReports.replaceQueryStringValue(currentUrl, name, value);
                    $link.attr("href", currentUrl);
                });
            });
        };
        this.$ = $;
    }
    return LinkBuilderComponent;
}());
//# sourceMappingURL=link-builder.js.map
/// <reference path="../core.ts" />
"use strict";
var PagerOption = /** @class */ (function () {
    function PagerOption(onSelectPage, pagerId, formId, pageIndexId, pageSizeId) {
        if (onSelectPage === void 0) { onSelectPage = null; }
        if (pagerId === void 0) { pagerId = "report-pager"; }
        if (formId === void 0) { formId = "report-form"; }
        if (pageIndexId === void 0) { pageIndexId = "PageIndex"; }
        if (pageSizeId === void 0) { pageSizeId = "PageSize"; }
        this.onSelectPage = onSelectPage;
        this.pagerId = pagerId;
        this.formId = formId;
        this.pageIndexId = pageIndexId;
        this.pageSizeId = pageSizeId;
    }
    return PagerOption;
}());
var PagerComponent = /** @class */ (function () {
    function PagerComponent($) {
        var _this = this;
        this.pageList = [1000, 2000, 3000, 5000];
        this.render = function (options) {
            if (options === void 0) { options = new PagerOption(); }
            var self = _this;
            var pagerOption = _this.$.extend({}, _this.options, options);
            var pager = _this.$("#" + pagerOption.pagerId);
            _this.$reportForm = _this.$("#" + _this.options.formId);
            _this.totalRecords = pager.data("totalrecords");
            _this.pageSize = pager.data("pagesize");
            _this.pageIndex = pager.data("pageindex");
            _this.$pageIndex =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageIndexId + "\" name=\"" + pagerOption.pageIndexId + "\" value=\"" + _this.pageIndex + "\" />");
            _this.$pageSize =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageSizeId + "\" name=\"" + pagerOption.pageSizeId + "\" value=\"" + _this.pageSize + "\" />");
            _this.$reportForm.append(_this.$pageIndex).append(_this.$pageSize);
            if (_this.isShow()) {
                pager.pagination({
                    pageList: _this.pageList,
                    total: _this.totalRecords,
                    pageSize: _this.pageSize,
                    pageNumber: _this.pageIndex,
                    showRefresh: false,
                    onSelectPage: function (pageNumber, pageSize) {
                        self.updateHiddenFields(pageNumber, pageSize);
                        self.onChange();
                    },
                    beforePageText: pager.data("lablepage"),
                    afterPageText: pager.data("labelof") + " {pages}",
                    displayMsg: pager.data("labeldisplayitems")
                });
                pager.removeClass("hide");
            }
            else {
                pager.addClass("hide");
            }
            return pager;
        };
        this.onChange = function () {
            _this.$reportForm.trigger("submit");
        };
        this.updateHiddenFields = function (pageNumber, pageSize) {
            _this.$pageIndex.val(pageNumber);
            _this.$pageSize.val(pageSize);
        };
        this.isShow = function () {
            // total records less than min page size => do not need to show pager
            return _this.pageList[0] <= _this.totalRecords;
        };
        this.hasAllDataOnClient = function () {
            var result = _this.totalRecords <= _this.pageSize;
            return result;
        };
        this.options = new PagerOption(this.onChange);
        this.$ = $;
    }
    return PagerComponent;
}());
//# sourceMappingURL=pager.js.map
/// <reference path="../core.ts" />
"use strict";
var DatePickerOption = /** @class */ (function () {
    function DatePickerOption(minDate, allowInputToggle) {
        if (allowInputToggle === void 0) { allowInputToggle = false; }
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
    }
    return DatePickerOption;
}());
var RiskAnalysisIPMatching = /** @class */ (function () {
    function RiskAnalysisIPMatching($) {
        var _this = this;
        this.hasAllDataOnClient = false;
        this.init = function () {
            _this.$.blockUI();
            _this.$("SELECT").on("change", function (event) {
                _this.$('#MemberCount').val(event.target.value);
            });
            var options = new DatePickerOption(_this.$('#MinDate').val(), true);
            _this.$('#dpkContainer').datepicker(options).data("DateTimePicker").maxDate(_this.$('#Today').val());
            _this.$('#dpkContainer').on("dp.change", function () {
                _this.$('#SelectedWinlostDate').val(_this.$('#WinlostDate').val());
            });
            _this.$("#icon-excel").on("click", function () {
                _this.$("#export-excel-form").submit();
            });
            _this.$(".icon-filter").on("click", function () {
                var $reportForm = _this.$("#report-form");
                var $pageTitle = _this.$(".page-title");
                var currentHideState = $reportForm.hasClass("hide");
                if (currentHideState) {
                    $pageTitle.removeClass("hide-filter");
                    $reportForm.removeClass("hide");
                }
                else {
                    $pageTitle.addClass("hide-filter");
                    $reportForm.addClass("hide");
                }
            });
            _this.$(".view-bet-list").on("click", function () {
                _this.$.blockUI();
            });
            _this.$.unblockUI();
        };
        this.$ = $;
    }
    return RiskAnalysisIPMatching;
}());
$(function () {
    var riskAnalysisIPMatching = new RiskAnalysisIPMatching($);
    riskAnalysisIPMatching.init();
});
//# sourceMappingURL=index.js.map