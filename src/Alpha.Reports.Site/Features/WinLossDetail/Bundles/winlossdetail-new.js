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
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        var _this = this;
        var dateRangePicker = $("#" + options.selectorId);
        var historicPivotDate = moment(dateRangePicker.data("historicpivotdate"));
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
            if (endDate.isBefore(historicPivotDate) && endDate.diff(startDate, "d") > 30) {
                alert("Out of date range: from and to is between 31 days.");
                dateRangePicker.data("dateRangePicker").setDateRange(_this.fromDate, _this.toDate, false);
                return;
            }
            if (startDate.isBefore(historicPivotDate) && endDate.isSameOrAfter(historicPivotDate)) {
                alert("Out of date range: gap between normal and history mode.");
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
                options.onDropdownHidden(self.getSelectedItems());
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
                options.onDropdownHidden(self.getSelectedItems());
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
    ProductAndProductDetailDropdownComponent.prototype.getSelectedItems = function () {
        var productsAndResources = {
            productIds: this.dropdownProduct.fanexMultiSelect("getCheckedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getCheckedValues")
        };
        this.dropdownProduct.fanexMultiSelect("addError", productsAndResources.productIds.length === 0);
        return productsAndResources;
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
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map
"use strict";
var WeekRangeOption = /** @class */ (function () {
    function WeekRangeOption(onClick, language, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (language === void 0) { language = "en"; }
        if (selectorId === void 0) { selectorId = "labelWeekRange"; }
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
        this.labelWeekRange = $("#" + options.selectorId);
    };
    WeekRangeComponent.prototype.render = function (formData) {
        var _this = this;
        moment.locale(this.weekRangeOption.language);
        this.labelWeekRange.html(this.buildWeekRangeHtml(formData.fromDate, formData.toDate));
        var labelWeeks = this.labelWeekRange.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekRangeOption.onClick({ fromDate: $this.data("fromdate"), toDate: $this.data("todate") });
        });
    };
    WeekRangeComponent.prototype.buildWeekRangeHtml = function (fromDate, toDate) {
        var weekRange = [];
        var startDate = moment(fromDate);
        var endDate = moment(toDate);
        while (endDate.diff(startDate, "d") >= 0) {
            var endDateStartOfWeek = moment(endDate).startOf("isoWeek");
            var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
            var fromDateValue = startDateOrEndDateStartOfWeek.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var toDateValue = endDate.format(alphaReports.format.shortDateWithSlash_DatePicker);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(alphaReports.format.dayMonthName)
                : startDateOrEndDateStartOfWeek.format(alphaReports.format.day) + " - " + endDate.format(alphaReports.format.dayMonthName);
            weekRange.push("<a href=\"javascript:;\" class=\"label-week\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
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
"use strict";
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
var WinLossDetailViewNew = /** @class */ (function (_super) {
    __extends(WinLossDetailViewNew, _super);
    function WinLossDetailViewNew(totalEnteredNonEnteredComponent, dateRangePickerComponent, productAndProductDetailDropdownComponent, visibleColumnDropdownComponent, weekRangeComponent, breadcrumbLevelComponent, productList, productDetailList) {
        var _this = _super.call(this) || this;
        _this.sortColumnName = "username";
        _this.sortIndicator = 0; // asc: 0, desc: 1
        _this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        _this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        _this.weekRangeComponent = weekRangeComponent;
        _this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        _this.productList = productList;
        _this.productDetailList = productDetailList;
        return _this;
    }
    WinLossDetailViewNew.prototype.init = function () {
        this.tableReportContainer = $("#tbl-report-container");
        this.winLossDetailUrl = $("#hiddenWinLossDetailUrl").val();
        this.renderComponents();
        this.bindEvents();
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.tableReportContainer.removeAttr("style");
    };
    WinLossDetailViewNew.prototype.renderComponents = function () {
        var _this = this;
        var viewerId = +$("#hiddenViewerId").val();
        var viewerLevel = +$("#hiddenViewerLevel").val();
        this.totalEnteredNonEnteredComponent.init();
        this.dateRangePickerComponent.render(new DateRangePickerJQueryOption(function (dateRange) {
            $.extend(_this.globalFormData, dateRange);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getWinLossDetailDataAndRenderWeekRange();
        }));
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(this.productList, this.productDetailList, function (productsAndResources) {
            $.extend(_this.globalFormData, _this.dateRangePickerComponent.getDateRangeValues(), productsAndResources);
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getWinLossDetailDataAndRenderWeekRange();
        }));
        this.visibleColumnDropdownComponent.render(new VisibleColumnDropdownOption("WinLossDetail_" + viewerId + "_" + $("#hiddenSubAccId").val() + ":Columns", this.getListColumn(viewerLevel), function (visibleColumn) { return _this.showHideColumns(visibleColumn); }));
        var dateRange = this.dateRangePickerComponent.getDateRangeValues();
        this.weekRangeComponent.init(new WeekRangeOption(function (dateRange) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            $.extend(_this.globalFormData, dateRange);
            _this.getWinLossDetailData();
        }, this.getMomentLocale()));
        this.weekRangeComponent.render(dateRange);
        var breadcrumbLevelJson = $("#hiddenBreadcrumbLevelJson").val();
        var breadcrumbs = breadcrumbLevelJson.length
            ? JSON.parse(breadcrumbLevelJson)
            : [{ custId: viewerId, custName: $("#hiddenViewerName").val(), custLevelId: viewerLevel }];
        this.breadcrumbLevelComponent.init(new BreadcrumbLevelOption(breadcrumbs, function (breadcrumbData) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            $.extend(_this.globalFormData, breadcrumbData);
            _this.getWinLossDetailData(function () {
                _this.breadcrumbLevelComponent.removeBreadcrumbLevel(breadcrumbData.custLevelId);
                _this.updateCurrentUrl();
            });
        }));
        this.globalFormData = $.extend({}, {
            custId: +$("#hiddenCustId").val(),
            custName: $("#hiddenCustName").val(),
            custLevelId: +$("#hiddenCustLevel").val()
        }, dateRange, this.productAndProductDetailDropdownComponent.getSelectedItems());
    };
    WinLossDetailViewNew.prototype.getMomentLocale = function () {
        var locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };
        return locale[window.top.$("#language").data("currentlanguage")] || locale.default;
    };
    WinLossDetailViewNew.prototype.getWinLossDetailDataAndRenderWeekRange = function () {
        var _this = this;
        this.getWinLossDetailData(function () {
            _this.weekRangeComponent.render(_this.globalFormData);
            _this.updateCurrentUrl();
        });
    };
    WinLossDetailViewNew.prototype.getWinLossDetailData = function (callback) {
        var _this = this;
        if (callback === void 0) { callback = Function(); }
        this.tableReportContainer.block();
        $.get(this.winLossDetailUrl + "?" + $.param(this.globalFormData), function (html) {
            callback();
            _this.tableReportContainer.html(html);
            _this.bindTableReportEvents();
            _this.totalEnteredNonEnteredComponent.update({ eventDate: _this.tableReport.data("yesterday"), nonEntered: _this.tableReport.data("non-entered-yesterday") }, { eventDate: _this.tableReport.data("today"), nonEntered: _this.tableReport.data("non-entered-today") });
            _this.tableReportContainer.unblock();
        }).fail(function (jqXHR) {
            _this.tableReportContainer.unblock();
            if (jqXHR.status === 429) {
                window.location.href = window.top["alpha"].urls.mainRootPath + "error/blocked";
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    window.location.href = window.top["alpha"].urls.mainRootPath + "partialum/index?" + $.param(responseObject);
                }
            }
        });
    };
    WinLossDetailViewNew.prototype.isInvalidGlobalFormData = function () {
        return this.globalFormData.productIds.length === 0;
    };
    WinLossDetailViewNew.prototype.bindEvents = function () {
        var _this = this;
        this.initReportLegend($("#hiddenLegendUrl").val(), 800, 540);
        $("#icon-excel").click(function (e) {
            if (_this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }
            var $this = $(e.target);
            $this.attr("href", $this.data("url") + "?" + $.param(_this.globalFormData));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        });
        this.bindTableReportEvents();
    };
    WinLossDetailViewNew.prototype.bindTableReportEvents = function () {
        var _this = this;
        this.tableReport = this.tableReportContainer.find("#tbl-report");
        this.tableReport.find('[data-toggle="tooltip"]').tooltip();
        this.tableReport.find(".betlists-link").off().click(function (e) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.tableReport.block();
            var $this = $(e.target);
            var custId = $this.data("custid");
            var custName = $this.data("custname");
            var breadcrumbs = _this.breadcrumbLevelComponent.getBreadcrumbs();
            breadcrumbs.push({ custId: custId, custName: custName, custLevelId: 1 });
            var breadcrumbLevelJson = JSON.stringify(breadcrumbs);
            var betListsParams = $.extend({}, _this.globalFormData, { custId: custId, custName: custName, breadcrumbLevelJson: breadcrumbLevelJson });
            $this.attr("href", $("#hiddenBetListsUrl").val() + "?" + $.param(betListsParams));
        });
        this.tableReport.find(".downline-link").off().click(function (e) {
            e.preventDefault();
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            var $this = $(e.target);
            var breadcrumbData = {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: $this.data("custlevelid")
            };
            $.extend(_this.globalFormData, breadcrumbData);
            _this.getWinLossDetailData(function () {
                _this.breadcrumbLevelComponent.addBreadcrumbLevel(breadcrumbData);
                _this.updateCurrentUrl();
            });
        });
        this.initTableSorter();
        this.showHideColumns(this.visibleColumnDropdownComponent.getColumns());
    };
    WinLossDetailViewNew.prototype.initTableSorter = function () {
        var _this = this;
        var sortColumns = this.tableReport.find(".sorting");
        if (this.tableReport.data("cansort")) {
            var headers_1 = {};
            sortColumns.each(function (index) {
                headers_1[index] = { sorter: index === 0 || index === 1 ? "text" : "digit" };
            });
            this.tableReport.tablesorter({
                headers: headers_1,
                sortList: [[this.mapSortColumnNameToIndex(), this.sortIndicator]]
            }).bind("sortEnd", function () {
                var sortedColumns = _this.tableReport.find(".sorting-asc, .sorting-desc");
                if (sortedColumns.length) {
                    var firstSortedColumn = sortedColumns.eq(0);
                    _this.sortColumnName = firstSortedColumn.data("colname");
                    _this.sortIndicator = firstSortedColumn.hasClass("sorting-desc") ? 1 : 0; // asc: 0, desc: 1
                }
            });
        }
        else {
            sortColumns.removeClass("sorting");
        }
    };
    WinLossDetailViewNew.prototype.mapSortColumnNameToIndex = function () {
        var tableHeaderCells = this.tableReport.find("thead th[data-colname=" + this.sortColumnName + "]");
        if (tableHeaderCells.length) {
            return tableHeaderCells.eq(0).data("colindex");
        }
        // reset to default
        this.sortColumnName = "username";
        this.sortIndicator = 0;
        return 0;
    };
    WinLossDetailViewNew.prototype.getListColumn = function (viewerLevel) {
        var dropdown = $("#dropdown-column");
        var member = dropdown.data("label-member");
        var agent = dropdown.data("label-agent");
        var winloss = dropdown.data("label-winloss");
        var comm = dropdown.data("label-comm");
        var total = dropdown.data("label-total");
        var listColumn = [
            { value: "fullname", text: dropdown.data("label-fullname") },
            { value: "betcount", text: dropdown.data("label-betcount") },
            { value: "turnover", text: dropdown.data("label-turnover") },
            { value: "netturnover", text: dropdown.data("label-netturnover") },
            { value: "grosscomm", text: dropdown.data("label-grosscomm") },
            { value: "pwinloss", text: member + " " + winloss },
            { value: "pcomm", text: member + " " + comm },
            { value: "ptotal", text: member + " " + total },
            { value: "awinloss", text: agent + " " + winloss },
            { value: "acomm", text: agent + " " + comm },
            { value: "atotal", text: agent + " " + total },
        ];
        if (viewerLevel >= 3) { // master or super
            var master = dropdown.data("label-master");
            listColumn.push({ value: "mwinloss", text: master + " " + winloss }, { value: "mcomm", text: master + " " + comm }, { value: "mtotal", text: master + " " + total });
        }
        if (viewerLevel === 4) { // super
            var superText = dropdown.data("label-super");
            listColumn.push({ value: "swinloss", text: superText + " " + winloss }, { value: "scomm", text: superText + " " + comm }, { value: "stotal", text: superText + " " + total });
        }
        listColumn.push({ value: "company", text: dropdown.data("label-company") });
        return listColumn;
    };
    WinLossDetailViewNew.prototype.showHideColumns = function (column) {
        this.showHideFirstLastName(column);
        this.showHideSpecificColumn(column, "betcount");
        this.showHideSpecificColumn(column, "turnover");
        this.showHideSpecificColumn(column, "netturnover");
        this.showHideSpecificColumn(column, "grosscomm");
        this.showHideGroupColumn(column, "p", "groupColumnMember");
        this.showHideGroupColumn(column, "a", "groupColumnAgent");
        this.showHideGroupColumn(column, "m", "groupColumnMaster");
        this.showHideGroupColumn(column, "s", "groupColumnSuper");
        this.showHideSpecificColumn(column, "company");
    };
    WinLossDetailViewNew.prototype.showHideFirstLastName = function (column) {
        var colspan = 1;
        if (this.showHideSpecificColumn(column, "fullname")) {
            colspan++;
        }
        this.tableReport.find("#totalFirstLastName").attr("colspan", colspan);
    };
    WinLossDetailViewNew.prototype.showHideGroupColumn = function (column, prefix, groupColumnId) {
        var groupColumn = this.tableReport.find("#" + groupColumnId);
        if (groupColumn.length) {
            var colspan = 0;
            if (this.showHideSpecificColumn(column, prefix + "winloss")) {
                colspan++;
            }
            if (this.showHideSpecificColumn(column, prefix + "comm")) {
                colspan++;
            }
            if (this.showHideSpecificColumn(column, prefix + "total")) {
                colspan++;
            }
            colspan ? groupColumn.attr("colspan", colspan).show() : groupColumn.hide();
        }
    };
    WinLossDetailViewNew.prototype.showHideSpecificColumn = function (column, key) {
        var matchedColumns = this.tableReport.find("[data-colname=" + key + "]");
        var visible = column[key];
        visible ? matchedColumns.show() : matchedColumns.hide();
        return visible;
    };
    WinLossDetailViewNew.prototype.updateCurrentUrl = function () {
        if (this.globalFormData.productIds) {
            var formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbs()) });
            window.top.indexManager.updateCurrentUrl($.param(formData));
        }
    };
    return WinLossDetailViewNew;
}(BaseReportView));
$(function () {
    new WinLossDetailViewNew(new TotalEnteredNonEnteredComponent(), new DateRangePickerJQueryComponent(), new ProductAndProductDetailDropdownComponent(), new VisibleColumnDropdownComponent(), new WeekRangeComponent(), new BreadcrumbLevelComponent(), window.productList, window.productDetailList).init();
});
//# sourceMappingURL=winlossdetail-new.js.map