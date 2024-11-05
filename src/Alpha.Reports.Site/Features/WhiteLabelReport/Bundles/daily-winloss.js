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
    AlphaReports.prototype.handleAjaxFail = function (jqXHR) {
        if (jqXHR) {
            if (jqXHR.status === 401) {
                window.top["age"].SignOut();
            }
            else if (jqXHR.status === 429) {
                location.href = window.top["alpha"].urls.mainRootPath + "Error/Blocked";
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = window.top["alpha"].urls.mainRootPath + "PartialUM/Index?umfrom=" + jsonData.UMFrom + "&umto=" + jsonData.UMTo;
                }
            }
        }
    };
    return AlphaReports;
}());
var alphaReports = new AlphaReports($, store);
//# sourceMappingURL=core.js.map
"use strict";
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent() {
        this.dateFormat = "MM/DD/YYYY";
        this.locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "cn",
            "zh-TW": "tc",
            "ja-JP": "ja"
        };
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = $("#daterange-picker");
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        var customShortcuts = this.buildCustomShortcuts(dateRangePicker);
        var defaults = {
            format: this.dateFormat,
            separator: " - ",
            language: this.locale[dateRangePicker.data("language")] || "default",
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            showShortcuts: customShortcuts.length > 0,
            shortcuts: null,
            customShortcuts: customShortcuts,
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true
        };
        dateRangePicker
            .dateRangePicker($.extend({}, defaults, options)).bind("datepicker-change", function (_, obj) {
            _this.fromDate = moment(obj.date1).format(_this.dateFormat);
            _this.toDate = moment(obj.date2).format(_this.dateFormat);
        })
            .bind("datepicker-closed", function () { return options.onClosed(_this.getDateRangeValues()); })
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    };
    DateRangePickerJQueryComponent.prototype.buildCustomShortcuts = function ($dateRangePicker) {
        var minDate = moment($dateRangePicker.data("mindate"), this.dateFormat);
        var today = moment($dateRangePicker.data("today"), this.dateFormat);
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var yesterdayValue = yesterday.toDate();
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastWeekStartOfDay = moment(thisWeekStartOfDay).subtract(1, "w");
        var thisMonthStartOfDay = moment(today).startOf("M");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var lastMonthEndOfDay = moment(lastMonthStartOfDay).endOf("M").toDate();
        var last3MonthsStartOfDay = moment(today).subtract(3, "M").startOf("M");
        var realLastMonthStartOfDay = lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate();
        var realLast3MonthsStartOfDay = last3MonthsStartOfDay.diff(minDate) >= 0 ? last3MonthsStartOfDay.toDate() : minDate.toDate();
        var shortcuts = [];
        this.addShortcut($dateRangePicker, shortcuts, "labeltoday", todayValue, todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labelyesterday", yesterdayValue, yesterdayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labelthisweek", thisWeekStartOfDay.diff(minDate) >= 0 ? thisWeekStartOfDay.toDate() : minDate.toDate(), todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labellastweek", lastWeekStartOfDay.diff(minDate) >= 0 ? lastWeekStartOfDay.toDate() : minDate.toDate(), moment(thisWeekStartOfDay).subtract(1, "d").toDate());
        this.addShortcut($dateRangePicker, shortcuts, "labelthismonth", thisMonthStartOfDay.diff(minDate) >= 0 ? thisMonthStartOfDay.toDate() : minDate.toDate(), todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labellastmonth", realLastMonthStartOfDay, lastMonthEndOfDay);
        this.addShortcut($dateRangePicker, shortcuts, "labelsincelastmonth", lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(), todayValue);
        this.addShortcut($dateRangePicker, shortcuts, "labellast3months", realLast3MonthsStartOfDay, lastMonthEndOfDay);
        return shortcuts;
    };
    DateRangePickerJQueryComponent.prototype.addShortcut = function ($dateRangePicker, shortcuts, labelName, startDate, endDate) {
        var name = $dateRangePicker.data(labelName);
        name &&
            shortcuts.push({
                name: name,
                dates: function () {
                    return [startDate, endDate];
                }
            });
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map
'use strict';
var MultipleDropdownComponent = /** @class */ (function () {
    function MultipleDropdownComponent() {
    }
    MultipleDropdownComponent.prototype.render = function (selectorId, options) {
        var $dropdown = $('#' + selectorId);
        var defaults = $.extend({
            selectAllText: $dropdown.data('label-selectall'),
            selectedText: '# ' + $dropdown.data('label-selected'),
            noneSelectedText: $dropdown.data('label-noneselected'),
            customSelectAllText: $dropdown.data('label-all') + ' (#)',
            filterPlaceHolder: $dropdown.data('label-filterplaceholder'),
            noItemFoundText: $dropdown.data('label-noitemfound'),
            noItemText: $dropdown.data('label-nodatatext'),
            minWidth: 220,
            maxHeight: 250,
            showTooltip: false
        }, options);
        this.$dropdownComponent = $dropdown.fanexMultiSelect(defaults);
    };
    MultipleDropdownComponent.prototype.getCheckedValues = function () {
        if (this.$dropdownComponent && this.$dropdownComponent.length) {
            return this.$dropdownComponent.fanexMultiSelect('getCheckedValues');
        }
    };
    return MultipleDropdownComponent;
}());
//# sourceMappingURL=multiple-dropdown.js.map
"use strict";
var UsernameAutoSuggestionOption = /** @class */ (function () {
    function UsernameAutoSuggestionOption(url, directDownlines, isAccInfo, onBlur, selectorId) {
        if (directDownlines === void 0) { directDownlines = false; }
        if (isAccInfo === void 0) { isAccInfo = true; }
        if (onBlur === void 0) { onBlur = Function(); }
        if (selectorId === void 0) { selectorId = "txt-username"; }
        this.url = url;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
        this.onBlur = onBlur;
        this.selectorId = selectorId;
        this.url = url;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
        this.onBlur = onBlur;
        this.selectorId = selectorId;
    }
    return UsernameAutoSuggestionOption;
}());
var UsernameAutoSuggestionComponent = /** @class */ (function () {
    function UsernameAutoSuggestionComponent() {
    }
    UsernameAutoSuggestionComponent.prototype.init = function (options) {
        var _this = this;
        this.usernameInput = $("#" + options.selectorId);
        if (!this.usernameInput.length) {
            return;
        }
        var queryString = this.buildQueryString(options);
        var urlAndQueryString = options.url + "?" + queryString + (queryString.length ? "&" : "");
        var typeaheadOptions = {
            source: function (query, process) {
                return $.get(urlAndQueryString + "suggestionquery=" + query, process);
            },
            items: 10,
            minLength: 2,
            highlighter: function (displayText) {
                var parts = displayText.split("#");
                return parts[0] + " <br/> " + parts[1];
            },
            displayText: function (item) {
                var nickname = item.Username2 ? " (" + item.Username2 + ")" : "";
                return "" + item.Username + nickname + "#" + item.FirstName + " " + item.LastName;
            },
            autoSelect: false,
            afterSelect: function (item) { return _this.usernameInput.val(item.Username); },
            delay: 200
        };
        var hitEnterTextboxUsername = false;
        this.usernameInput.typeahead(typeaheadOptions);
        this.usernameInput.keypress(function (e) {
            var key = e.which || e.keyCode;
            if (key === 13) {
                hitEnterTextboxUsername = true;
                options.onBlur();
            }
        }).blur(function () {
            if (hitEnterTextboxUsername) {
                hitEnterTextboxUsername = false;
            }
            else {
                var visible = _this.usernameInput.next(".typeahead").is(":visible");
                !visible && options.onBlur();
            }
        });
    };
    UsernameAutoSuggestionComponent.prototype.buildQueryString = function (options) {
        var queryString = [];
        if (options.directDownlines) {
            queryString.push("directdownlines=" + options.directDownlines);
        }
        if (!options.isAccInfo) {
            queryString.push("isaccinfo=" + options.isAccInfo);
        }
        return queryString.join('&');
    };
    UsernameAutoSuggestionComponent.prototype.getUsername = function () {
        if (this.usernameInput.length) {
            return $.trim(this.usernameInput.val());
        }
    };
    UsernameAutoSuggestionComponent.prototype.resetUsername = function () {
        this.usernameInput.val("");
    };
    return UsernameAutoSuggestionComponent;
}());
//# sourceMappingURL=username-autosuggestion.js.map
"use strict";
var BaseReportView = /** @class */ (function () {
    function BaseReportView() {
        this.sortColumnIndex = 0;
        this.sortIndicator = 0; // asc: 0, desc: 1
    }
    BaseReportView.prototype.initReportLegend = function (legendUrl) {
        $.get(legendUrl, function (html) {
            $("body").append(html);
            $("#icon-report-legend").click(function () {
                $("#report-legend").modal({ show: true, backdrop: false });
            });
        });
    };
    BaseReportView.prototype.initShowHideFilter = function (callback) {
        if (callback === void 0) { callback = undefined; }
        var hideFilterText = $("#hiddenHideFilterText").val();
        var showFilterText = $("#hiddenShowFilterText").val();
        var iconFilter = $("#icon-filter").click(function () {
            var isHideFilter = iconFilter.hasClass("ic-arrow-up-circle");
            $('#report-form').toggle(!isHideFilter);
            if (isHideFilter) {
                iconFilter
                    .removeClass("ic-arrow-up-circle")
                    .addClass("ic-arrow-down-circle")
                    .attr("title", showFilterText);
            }
            else {
                iconFilter
                    .removeClass("ic-arrow-down-circle")
                    .addClass("ic-arrow-up-circle")
                    .attr("title", hideFilterText);
            }
            callback && callback(isHideFilter);
        });
    };
    BaseReportView.prototype.tableSorter = function ($tblReport, canSort, sortEndCallback) {
        var _this = this;
        if (canSort === void 0) { canSort = undefined; }
        if (sortEndCallback === void 0) { sortEndCallback = undefined; }
        canSort = canSort || $tblReport.data('cansort').toLowerCase() === 'true';
        var sortColumns = $tblReport.find('.sorting');
        if (canSort) {
            var headers_1 = {};
            sortColumns.each(function () {
                // sorttype is text or digit
                var $this = $(this);
                headers_1[$this.data('colindex')] = { sorter: $this.data('sorttype') || 'digit' };
            });
            var sortList = typeof this.sortColumnIndex === 'undefined'
                ? []
                : [[this.mapSortColumnIndex(), this.sortIndicator]];
            $tblReport.tablesorter({
                headers: headers_1,
                sortList: sortList
            }).bind("sortEnd", function () {
                var sortedColumns = $tblReport.find(".sorting-asc, .sorting-desc");
                if (sortedColumns.length) {
                    var firstSortedColumn = sortedColumns.eq(0);
                    _this.sortColumnName = firstSortedColumn.data("colname");
                    _this.sortColumnIndex = firstSortedColumn.data("colindex");
                    _this.sortIndicator = firstSortedColumn.hasClass("sorting-desc") ? 1 : 0; // asc: 0, desc: 1
                }
                sortEndCallback && sortEndCallback();
            });
        }
        else {
            sortColumns.removeClass('sorting');
        }
    };
    BaseReportView.prototype.mapSortColumnIndex = function () {
        return this.sortColumnIndex;
    };
    BaseReportView.prototype.stickyTableHeaders = function ($tblReport) {
        $tblReport.stickyTableHeaders('destroy');
        $tblReport.stickyTableHeaders();
    };
    BaseReportView.prototype.updateCurrentUrl = function (queryString) {
        try {
            window.top.indexManager.updateCurrentUrl(queryString);
        }
        catch (e) { }
    };
    return BaseReportView;
}());
//# sourceMappingURL=base-report-view.js.map
'use strict';
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DailyWinLossView = /** @class */ (function (_super) {
    __extends(DailyWinLossView, _super);
    function DailyWinLossView(dateRangePicker, whiteLabelDropdown, usernameAutoSuggestion) {
        var _this = _super.call(this) || this;
        _this.dateRangePicker = dateRangePicker;
        _this.whiteLabelDropdown = whiteLabelDropdown;
        _this.usernameAutoSuggestion = usernameAutoSuggestion;
        // ser default sortIndicator
        _this.sortColumnIndex = undefined;
        return _this;
    }
    DailyWinLossView.prototype.init = function () {
        this.cacheElements();
        this.initComponents();
        this.bindEventsAfterRenderWinLoss();
        this.$tblReportContainer.removeAttr('style');
    };
    DailyWinLossView.prototype.cacheElements = function () {
        this.$tblReportContainer = $('#tbl-report-container');
    };
    DailyWinLossView.prototype.initComponents = function () {
        var _this = this;
        var $iconExcel = $('#icon-excel');
        this.initShowHideFilter();
        this.dateRangePicker.render({
            onClosed: function () { return _this.getReportData(); }
        });
        if (Array.isArray(whiteLabels)) {
            var maxWhiteLabelLength_1 = 0;
            var listData = whiteLabels.map(function (wl) {
                if (wl.Name.length > maxWhiteLabelLength_1) {
                    maxWhiteLabelLength_1 = wl.Name.length;
                }
                return {
                    text: wl.Name,
                    value: wl.Id,
                    checked: true
                };
            });
            maxWhiteLabelLength_1 *= 11;
            this.whiteLabelDropdown.render('dropdown-whitelabel', {
                showFilter: true,
                listData: listData,
                onLeave: function () { return _this.getReportData(); },
                minWidth: maxWhiteLabelLength_1 > 220 ? maxWhiteLabelLength_1 : 220
            });
        }
        this.usernameAutoSuggestion.init(new UsernameAutoSuggestionOption($("#hiddenUsernameAutoSuggestionUrl").val(), false, false, function () { return _this.getReportData(); }));
        var excelUrl = $iconExcel.attr('href');
        $iconExcel.off().tooltip();
        $iconExcel.click(function () {
            $iconExcel.attr('href', excelUrl + "?" + $.param(_this.getFormData()));
            setTimeout(function () {
                $iconExcel.attr('href', 'javascript:;');
            }, 1000);
        }).attr('href', 'javascript:;');
    };
    DailyWinLossView.prototype.getReportData = function () {
        var _this = this;
        $.blockUI();
        var queryString = $.param(this.getFormData());
        $.get($('#hiddenGetDataUrl').val() + "?" + queryString, function (html) {
            _this.$tblReportContainer.html(html);
            _this.bindEventsAfterRenderWinLoss();
            _this.updateCurrentUrl(queryString);
        }).fail(alphaReports.handleAjaxFail).always($.unblockUI);
    };
    DailyWinLossView.prototype.getFormData = function () {
        var formData = this.dateRangePicker.getDateRangeValues();
        var whiteLabels = this.whiteLabelDropdown.getCheckedValues();
        if (whiteLabels) {
            formData.whiteLabels = whiteLabels;
        }
        var username = this.usernameAutoSuggestion.getUsername();
        if (username) {
            formData.username = username;
        }
        return formData;
    };
    DailyWinLossView.prototype.bindEventsAfterRenderWinLoss = function () {
        var $tblReport = $('#tbl-report');
        this.tableSorter($tblReport);
        this.stickyTableHeaders($tblReport);
    };
    return DailyWinLossView;
}(BaseReportView));
$(function () {
    new DailyWinLossView(new DateRangePickerJQueryComponent(), new MultipleDropdownComponent(), new UsernameAutoSuggestionComponent()).init();
});
//# sourceMappingURL=daily-winloss.js.map