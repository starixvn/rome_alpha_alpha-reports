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
var MonthPickerOption = /** @class */ (function () {
    function MonthPickerOption(selectorId, longMonthsInYear) {
        if (selectorId === void 0) { selectorId = "dropdown-month"; }
        if (longMonthsInYear === void 0) { longMonthsInYear = null; }
        this.selectorId = selectorId;
    }
    return MonthPickerOption;
}());
var MonthPickerComponent = /** @class */ (function () {
    function MonthPickerComponent($) {
        var _this = this;
        this.options = new MonthPickerOption();
        this.render = function (options) {
            if (options === void 0) { options = new MonthPickerOption; }
            var pickerOptions = _this.$.extend({}, _this.options, options);
            var dropdownMonth = _this.$("#" + pickerOptions.selectorId);
            var maxDate = dropdownMonth.data("maxdate");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            dropdownMonth.monPickr({
                longMonthsInYear: dropdownMonth.data("longmonths-inyear").split(","),
                maxDate: maxDate === "" ? dropdownMonth.data("currentdate") : maxDate,
                minDate: dropdownMonth.data("mindate"),
                month: new Date(dropdownMonth.data("dateinmonth")),
                fromDay: dropdownMonth.data("fromdate"),
                toDay: dropdownMonth.data("todate"),
                showDateRange: true,
                monthFormat: alphaReports.format.monthFormat,
                fromDayId: "ddl-fromday",
                toDayId: "ddl-today",
                fromDayLabel: "",
                toDayLabel: "-",
                sortDesc: true,
                log: function (msg) {
                    console.warn(msg);
                },
                afterChangeMonth: _this.onChange,
                afterChangeDay: _this.onChange
            });
            _this.$dropdownMonth = dropdownMonth;
            return dropdownMonth;
        };
        this.onChange = function () {
            _this.$fromDate.val(_this.$dropdownMonth.monPickr("getFromDate", alphaReports.format.shortDateWithSlash_MonthPicker));
            _this.$toDate.val(_this.$dropdownMonth.monPickr("getToDate", alphaReports.format.shortDateWithSlash_MonthPicker));
        };
        this.$ = $;
    }
    return MonthPickerComponent;
}());
//# sourceMappingURL=monthpicker.js.map
"use strict";
var MoveStatementHistory = /** @class */ (function () {
    function MoveStatementHistory($, monthPickerComponent, pagerComponent) {
        var _this = this;
        this.onClickIconFilter = function () {
            var isHide = _this.$reportForm.hasClass("hide");
            _this.$isFilterVisible.val(isHide.toString());
            if (isHide) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$reportForm.addClass("hide");
            }
        };
        this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onSubmitReport = function () {
            _this.$.blockUI();
            return true;
        };
        this.$ = $;
        this.monthPickerComponent = monthPickerComponent;
        this.pagerComponent = pagerComponent;
    }
    MoveStatementHistory.prototype.init = function () {
        this.$.blockUI();
        this.cacheElements();
        this.initComponents();
        this.bindEvents();
        this.$.unblockUI();
    };
    MoveStatementHistory.prototype.cacheElements = function () {
        this.$reportForm = this.$("#report-form");
        this.$detailLink = this.$("#detail-link");
        this.$backLink = this.$("#back-link");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
    };
    MoveStatementHistory.prototype.initComponents = function () {
        this.monthPickerComponent.render();
        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
        this.$("#tbl-report").stickyTableHeaders();
    };
    MoveStatementHistory.prototype.bindEvents = function () {
        var _this = this;
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#btn-submit").click(function () {
            _this.$("#IsMainReport").val("true");
            _this.$("#PageIndex").val("1");
            _this.$("#PageSize").val("100");
        });
        this.$("#icon-excel").click(this.onExportExcel);
        this.$backLink.click(this.onClickBackLink);
        this.$detailLink.click(this.onClickDetailLink);
    };
    MoveStatementHistory.prototype.onClickBackLink = function () {
        $("#IsMainReport").val("true");
        this.$reportForm.submit();
    };
    MoveStatementHistory.prototype.onClickDetailLink = function () {
        $("#IsMainReport").val("false");
        this.$reportForm.submit();
    };
    return MoveStatementHistory;
}());
$(function () {
    var moveStatementHistory = new MoveStatementHistory($, new MonthPickerComponent($), new PagerComponent($));
    moveStatementHistory.init();
});
//# sourceMappingURL=move-statement-history.js.map