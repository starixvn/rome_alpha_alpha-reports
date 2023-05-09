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
"use strict";
var TotalEnteredNonEnteredComponent = /** @class */ (function () {
    function TotalEnteredNonEnteredComponent() {
    }
    TotalEnteredNonEnteredComponent.prototype.init = function () {
        this.nonEnteredYesterdayContainer = $("#total-non-entered-yesterday");
        this.eventDateYesterday = $(".event-date-yesterday");
        this.nonEnteredYesterday = this.nonEnteredYesterdayContainer.find("#total-non-entered-yesterday-number");
        this.enteredYesterdayContainer = $("#total-entered-yesterday");
        var nonEnteredTodayContainer = $("#total-non-entered-today");
        this.eventDateToday = nonEnteredTodayContainer.find("#total-non-entered-today-text");
        this.nonEnteredToday = nonEnteredTodayContainer.find("#total-non-entered-today-number");
    };
    TotalEnteredNonEnteredComponent.prototype.update = function (yesterday, today) {
        this.eventDateYesterday.text(yesterday.eventDate);
        if (yesterday.nonEntered > 0) {
            this.enteredYesterdayContainer.hide();
            this.nonEnteredYesterday.text(alphaReports.formatNumber(yesterday.nonEntered));
            this.nonEnteredYesterdayContainer.show();
        }
        else {
            this.nonEnteredYesterdayContainer.hide();
            this.enteredYesterdayContainer.show();
        }
        this.eventDateToday.text(today.eventDate);
        this.nonEnteredToday.text(alphaReports.formatNumber(today.nonEntered));
    };
    return TotalEnteredNonEnteredComponent;
}());
//# sourceMappingURL=total-entered-nonentered.js.map
"use strict";
var DateRangePickerJQueryOption = /** @class */ (function () {
    function DateRangePickerJQueryOption(onClosed, selectorId) {
        if (onClosed === void 0) { onClosed = Function(); }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        this.onClosed = onClosed;
        this.selectorId = selectorId;
        this.onClosed = onClosed;
        this.selectorId = selectorId;
    }
    return DateRangePickerJQueryOption;
}());
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent() {
        this.historyMaxDateRange = 30;
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = $("#" + options.selectorId);
        var historicPivotDateText = dateRangePicker.data("historicpivotdate");
        var historicPivotDate = moment(historicPivotDateText);
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        dateRangePicker
            .dateRangePicker({
            format: alphaReports.format.shortDateWithSlash_DatePicker,
            separator: " - ",
            language: "en",
            startDate: dateRangePicker.data("mindate"),
            endDate: dateRangePicker.data("maxdate"),
            showShortcuts: true,
            shortcuts: null,
            customShortcuts: this.getPredefinedRanges(dateRangePicker),
            duration: 0,
            singleMonth: true,
            monthSelect: true,
            yearSelect: true
        }).bind("datepicker-change", function (_, obj) {
            var startDate = moment(obj.date1);
            var endDate = moment(obj.date2);
            if (endDate.isBefore(historicPivotDate) && endDate.diff(startDate, "d") > _this.historyMaxDateRange) {
                window.top.bootboxAlert("Data before " + historicPivotDateText + " can be retrieved " + (_this.historyMaxDateRange + 1) + " days in max, please review your date range.");
                dateRangePicker.data("dateRangePicker").setDateRange(_this.fromDate, _this.toDate, false);
                return;
            }
            if (startDate.isBefore(historicPivotDate) && endDate.isSameOrAfter(historicPivotDate) && endDate.diff(startDate, "d") > _this.historyMaxDateRange) {
                window.top.bootboxAlert("Data before " + historicPivotDateText + " can be retrieved " + (_this.historyMaxDateRange + 1) + " days in max, please review your date range.");
                dateRangePicker.data("dateRangePicker").setDateRange(_this.fromDate, _this.toDate, false);
                return;
            }
            _this.fromDate = startDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            _this.toDate = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
        })
            .bind("datepicker-closed", function () { return options.onClosed(_this.getDateRangeValues()); })
            .data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
    };
    DateRangePickerJQueryComponent.prototype.getPredefinedRanges = function (dateRangePicker) {
        var minDate = moment(dateRangePicker.data("mindate"));
        var today = moment(dateRangePicker.data("currentdate"));
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        return [
            {
                name: dateRangePicker.data("label-today"),
                dates: function () {
                    return [todayValue, todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-yesterday"),
                dates: function () {
                    var yesterdayValue = yesterday.toDate();
                    return [yesterdayValue, yesterdayValue];
                }
            },
            {
                name: dateRangePicker.data("label-thisweek"),
                dates: function () {
                    return [thisWeekStartOfDay.toDate(), todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-lastweek"),
                dates: function () {
                    return [moment(thisWeekStartOfDay).subtract(1, "w").toDate(), moment(thisWeekStartOfDay).subtract(1, "d").toDate()];
                }
            },
            {
                name: dateRangePicker.data("label-thismonth"),
                dates: function () {
                    return [moment(today).startOf("M").toDate(), todayValue];
                }
            },
            {
                name: dateRangePicker.data("label-lastmonth"),
                dates: function () {
                    return [
                        lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(),
                        moment(lastMonthStartOfDay).endOf("M").toDate()
                    ];
                }
            },
            {
                name: dateRangePicker.data("label-sincelastmonth"),
                dates: function () {
                    return [
                        lastMonthStartOfDay.diff(minDate) >= 0 ? lastMonthStartOfDay.toDate() : minDate.toDate(),
                        todayValue
                    ];
                }
            }
        ];
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map
"use strict";
var ProductAndProductDetailDropdownOption = /** @class */ (function () {
    function ProductAndProductDetailDropdownOption(products, productDetails, onDropdownHidden, selectorProductId, selectorProductDetailId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorProductId === void 0) { selectorProductId = "dropdown-product"; }
        if (selectorProductDetailId === void 0) { selectorProductDetailId = "dropdown-product-detail"; }
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
    }
    return ProductAndProductDetailDropdownOption;
}());
var ProductAndProductDetailDropdownComponent = /** @class */ (function () {
    function ProductAndProductDetailDropdownComponent() {
    }
    ProductAndProductDetailDropdownComponent.prototype.render = function (options) {
        this.renderProductDetailDropdown(options);
        this.renderProductDropdown(options);
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDetailDropdown = function (options) {
        var self = this;
        var productDetailList = options.productDetails.map(function (item) {
            return self.buildCheckboxItem(item, {
                productId: item.ProductId
            });
        });
        var dropdown = $("#" + options.selectorProductDetailId);
        this.dropdownProductDetail = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: productDetailList,
            onLeave: function () {
                var productIds = self.dropdownProductDetail.fanexMultiSelect("getUncheckedProductIds");
                if (productIds.length) {
                    self.dropdownProduct.fanexMultiSelect("uncheck", productIds);
                }
                options.onDropdownHidden(self.getCheckedValues());
            }
        });
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDropdown = function (options) {
        var self = this;
        var productIdWithSubProductIds = {};
        var uncheckedProductIds = [];
        var productList = options.products.map(function (item) {
            var checkboxItem = self.buildCheckboxItem(item);
            if (item.Products) {
                var subProductIds_1 = [];
                productIdWithSubProductIds[item.Id] = subProductIds_1;
                checkboxItem.items = item.Products.map(function (item) {
                    subProductIds_1.push(item.Id);
                    item.Checked || uncheckedProductIds.push(item.Id);
                    return self.buildCheckboxItem(item);
                });
            }
            else {
                item.Checked || uncheckedProductIds.push(item.Id);
            }
            return checkboxItem;
        });
        var dropdown = $("#" + options.selectorProductId);
        this.dropdownProduct = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            expandGroupsByDefault: false,
            listData: productList,
            onCheck: function () {
                var checked = this.checked;
                var productId = this.value;
                var subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    subProductIds.forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
                else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                var checked = this.checked;
                self.dropdownProductDetail.fanexMultiSelect("showAllItems", checked);
                self.dropdownProductDetail.fanexMultiSelect("disable", !checked);
            },
            onLeave: function () {
                options.onDropdownHidden(self.getCheckedValues());
            }
        });
        this.initState(uncheckedProductIds);
    };
    ProductAndProductDetailDropdownComponent.prototype.initState = function (uncheckedProductIds) {
        var _this = this;
        if (uncheckedProductIds.length) {
            uncheckedProductIds.forEach(function (item) { return _this.handleOnCheckProduct(item, false); });
        }
    };
    ProductAndProductDetailDropdownComponent.prototype.handleOnCheckProduct = function (productId, checked) {
        var action = checked ? "showItemsByCustomAttributes" : "hideItemsByCustomAttributes";
        this.dropdownProductDetail.fanexMultiSelect(action, { productId: productId });
    };
    ProductAndProductDetailDropdownComponent.prototype.buildCheckboxItem = function (item, attributes) {
        if (attributes === void 0) { attributes = undefined; }
        return {
            value: item.Id,
            text: item.Name,
            checked: item.Checked,
            attributes: attributes
        };
    };
    ProductAndProductDetailDropdownComponent.prototype.getCheckedValues = function () {
        var productsAndResources = {
            productIds: this.dropdownProduct.fanexMultiSelect("getCheckedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getCheckedValues")
        };
        this.dropdownProduct.fanexMultiSelect("addError", productsAndResources.productIds.length === 0);
        return productsAndResources;
    };
    ProductAndProductDetailDropdownComponent.prototype.getUncheckedValues = function () {
        return {
            unselectedProductIds: this.dropdownProduct.fanexMultiSelect("getUncheckedValues"),
            unselectedResourceIds: this.dropdownProductDetail.fanexMultiSelect("getUncheckedValues")
        };
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map
"use strict";
var VisibleColumnDropdownOption = /** @class */ (function () {
    function VisibleColumnDropdownOption(visibleColumnKey, listColumn, onDropdownHidden, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-column"; }
        this.visibleColumnKey = visibleColumnKey;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.visibleColumnKey = visibleColumnKey;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
    return VisibleColumnDropdownOption;
}());
var VisibleColumnDropdownComponent = /** @class */ (function () {
    function VisibleColumnDropdownComponent() {
    }
    VisibleColumnDropdownComponent.prototype.render = function (options) {
        var _this = this;
        var isCheckAll = true;
        var dropdown = $("#" + options.selectorId);
        var visibleColumnObject = {};
        var visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns !== "undefined") {
            var visibleColumnArray = visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = options.listColumn.length === visibleColumnArray.length;
        }
        this.dropdownColumn = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            isCheckAll: isCheckAll,
            minWidth: 220,
            maxHeight: 250,
            showTooltip: false,
            listData: options.listColumn.map(function (item) {
                return {
                    value: item.value,
                    text: item.text,
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(item.value)
                };
            }),
            onLeave: function () { return _this.onDropdownHidden(options.visibleColumnKey, options.onDropdownHidden); }
        });
    };
    VisibleColumnDropdownComponent.prototype.onDropdownHidden = function (visibleColumnKey, callback) {
        var columns = this.getColumns();
        var selectedColumnsValue = [];
        Object.entries(columns).forEach(function (_a) {
            var key = _a[0], value = _a[1];
            return value && selectedColumnsValue.push(key);
        });
        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
        callback(columns);
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    };
    VisibleColumnDropdownComponent.prototype.getHideColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getUncheckedValues");
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map
"use strict";
var WeekRangeOption = /** @class */ (function () {
    function WeekRangeOption(onClick, language, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (language === void 0) { language = "en"; }
        if (selectorId === void 0) { selectorId = "week-range-container"; }
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.language = language;
        this.selectorId = selectorId;
    }
    return WeekRangeOption;
}());
var WeekRangeComponent = /** @class */ (function () {
    function WeekRangeComponent() {
    }
    WeekRangeComponent.prototype.init = function (options) {
        this.weekRangeOption = options;
        this.weekRangeContainer = $("#" + options.selectorId);
    };
    WeekRangeComponent.prototype.render = function (dateRange, activeWeekRange) {
        var _this = this;
        moment.locale(this.weekRangeOption.language);
        this.weekRangeContainer.html(this.buildWeekRangeHtml(dateRange, activeWeekRange));
        var labelWeeks = this.weekRangeContainer.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekRangeOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
        });
    };
    WeekRangeComponent.prototype.buildWeekRangeHtml = function (dateRange, activeWeekRange) {
        var weekRange = [];
        var startDate = moment(dateRange.fromDate);
        var endDate = moment(dateRange.toDate);
        while (endDate.diff(startDate, "d") >= 0) {
            var endDateStartOfWeek = moment(endDate).startOf("isoWeek");
            var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
            var fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : startDateOrEndDateStartOfWeek.format(alphaReports.format.day) + " - " + endDate.format(alphaReports.format.dayMonthName);
            var activeClassName = activeWeekRange
                && activeWeekRange.weekFromDate === fromDateValue
                && activeWeekRange.weekToDate === toDateValue ? " active" : "";
            weekRange.push("<a href=\"javascript:;\" class=\"label-week" + activeClassName + "\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
    };
    WeekRangeComponent.prototype.getActiveWeekRange = function () {
        var activeWeekRange = this.weekRangeContainer.find(".label-week.active");
        if (activeWeekRange.length) {
            var firstActiveWeekRange = activeWeekRange.eq(0);
            return {
                weekFromDate: firstActiveWeekRange.data("fromdate"),
                weekToDate: firstActiveWeekRange.data("todate"),
            };
        }
        return {};
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map
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
"use strict";
var BaseReportView = /** @class */ (function () {
    function BaseReportView() {
    }
    BaseReportView.prototype.initReportLegend = function (legendUrl, width, height) {
        $("#icon-report-legend").click(function () {
            top.window["popupManager"].open(typeof legendUrl === "function" ? legendUrl() : legendUrl, undefined, width, height, "modal-mobile-friendly");
        });
    };
    return BaseReportView;
}());
//# sourceMappingURL=base-report-view.js.map