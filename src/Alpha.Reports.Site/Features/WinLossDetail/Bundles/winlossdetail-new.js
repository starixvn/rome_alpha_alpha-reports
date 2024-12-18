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
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent() {
        this.dateFormat = "MM/DD/YYYY";
        this.locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "cn",
            "zh-TW": "tc"
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
        if (options.productDetails.length) {
            var self_1 = this;
            var productDetailList = options.productDetails.map(function (item) {
                return self_1.buildCheckboxItem(item, {
                    productId: item.ProductId
                });
            });
            var dropdown = $("#" + options.selectorProductDetailId);
            this.dropdownProductDetail = dropdown.fanexMultiSelect({
                selectAllText: dropdown.data("label-selectall"),
                selectedText: dropdown.data("label-selected"),
                noneSelectedText: dropdown.data("label-noneselected"),
                customSelectAllText: dropdown.data("label-all") + " (#)",
                showFilter: true,
                filterPlaceHolder: dropdown.data("label-filterplaceholder"),
                noItemFoundText: dropdown.data("label-noitemfound"),
                minWidth: 250,
                maxHeight: 250,
                showTooltip: false,
                listData: productDetailList,
                onLeave: function () {
                    var productIds = self_1.dropdownProductDetail.fanexMultiSelect("getUncheckedProductIds");
                    if (productIds.length) {
                        self_1.dropdownProduct.fanexMultiSelect("uncheck", productIds);
                    }
                    options.onDropdownHidden(self_1.getCheckedValues());
                }
            });
        }
        else {
            $("#" + options.selectorProductDetailId).parent().remove();
        }
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDropdown = function (options) {
        var _this = this;
        var productIdWithSubProductIds = {};
        var uncheckedProductIds = [];
        var productList = options.products.map(function (item) {
            var checkboxItem = _this.buildCheckboxItem(item);
            if (item.Products) {
                var subProductIds_1 = [];
                productIdWithSubProductIds[item.Id] = subProductIds_1;
                checkboxItem.items = item.Products.map(function (item) {
                    subProductIds_1.push(item.Id);
                    item.Checked || uncheckedProductIds.push(item.Id);
                    return _this.buildCheckboxItem(item);
                });
            }
            else {
                item.Checked || uncheckedProductIds.push(item.Id);
            }
            return checkboxItem;
        });
        var self = this;
        var dropdown = $("#" + options.selectorProductId);
        this.dropdownProduct = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            listData: productList,
            onCheck: function () {
                var productId = this.value;
                var checked = this.checked;
                var subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    var productIdString = checked
                        ? self.dropdownProduct.fanexMultiSelect("getCheckedValues")
                        : self.dropdownProduct.fanexMultiSelect("getUncheckedValues");
                    productIdString.split(",").forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
                else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                var checked = this.checked;
                var checkedValues = self.dropdownProduct.fanexMultiSelect("getCheckedValues");
                var uncheckedValues = self.dropdownProduct.fanexMultiSelect("getUncheckedValues");
                if ((checked && !uncheckedValues.length) || (!checked && !checkedValues.length)) {
                    if (self.dropdownProductDetail) {
                        self.dropdownProductDetail.fanexMultiSelect("showAllItems", checked);
                        self.dropdownProductDetail.fanexMultiSelect("disable", !checked);
                    }
                }
                else {
                    var productIdString = checked
                        ? checkedValues
                        : uncheckedValues;
                    productIdString.split(",").forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                }
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
        this.dropdownProductDetail && this.dropdownProductDetail.fanexMultiSelect(action, { productId: productId });
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
            resourceIds: this.dropdownProductDetail
                ? this.dropdownProductDetail.fanexMultiSelect("getCheckedValues")
                : ''
        };
        this.dropdownProduct.fanexMultiSelect("addError", productsAndResources.productIds.length === 0);
        return productsAndResources;
    };
    ProductAndProductDetailDropdownComponent.prototype.getUncheckedValues = function () {
        return {
            unselectedProductIds: this.dropdownProduct.fanexMultiSelect("getUncheckedValues"),
            unselectedResourceIds: this.dropdownProductDetail
                ? this.dropdownProductDetail.fanexMultiSelect("getUncheckedValues")
                : ''
        };
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map
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
var VisibleColumnDropdownOption = /** @class */ (function () {
    function VisibleColumnDropdownOption(visibleColumns, listColumn, onDropdownHidden, selectorId, minWidth) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-column"; }
        if (minWidth === void 0) { minWidth = 220; }
        this.visibleColumns = visibleColumns;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.minWidth = minWidth;
        this.visibleColumns = visibleColumns;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.minWidth = minWidth;
    }
    return VisibleColumnDropdownOption;
}());
var VisibleColumnDropdownComponent = /** @class */ (function () {
    function VisibleColumnDropdownComponent() {
    }
    VisibleColumnDropdownComponent.prototype.render = function (options) {
        var _this = this;
        var isCheckAll = false;
        var dropdown = $("#" + options.selectorId);
        var visibleColumnObject = {};
        if (options.visibleColumns.length) {
            var visibleColumnArray = options.visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = options.listColumn.length === visibleColumnArray.length;
        }
        else {
            options.listColumn.forEach(function (item) {
                if (item.checked !== false) {
                    visibleColumnObject[item.value] = true;
                }
            });
        }
        this.dropdownColumn = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            isCheckAll: isCheckAll,
            minWidth: options.minWidth,
            maxHeight: 250,
            showTooltip: false,
            listData: options.listColumn.map(function (item) {
                return {
                    value: item.value,
                    text: item.text,
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(item.value)
                };
            }),
            onLeave: function () { return options.onDropdownHidden(_this.getColumns()); }
        });
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    };
    VisibleColumnDropdownComponent.prototype.getShowColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getCheckedValues");
    };
    VisibleColumnDropdownComponent.prototype.getHideColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getUncheckedValues");
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map
"use strict";
var WeekSelectionOption = /** @class */ (function () {
    function WeekSelectionOption(onClick, selectorId) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "week-selection"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.onClick = onClick;
        this.selectorId = selectorId;
    }
    return WeekSelectionOption;
}());
var WeekSelectionComponent = /** @class */ (function () {
    function WeekSelectionComponent() {
    }
    WeekSelectionComponent.prototype.init = function (options) {
        this.weekSelectionOption = options;
        this.bindEvents();
    };
    WeekSelectionComponent.prototype.reRender = function (html) {
        this.weekRangeContainer.html(html);
        this.bindEvents();
    };
    WeekSelectionComponent.prototype.bindEvents = function () {
        var _this = this;
        this.weekRangeContainer = $("#" + this.weekSelectionOption.selectorId);
        var labelWeeks = this.weekRangeContainer.find(".label-week:not(.disabled)");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = $(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekSelectionOption.onClick({ weekFromDate: $this.data("fromdate"), weekToDate: $this.data("todate") });
        });
    };
    WeekSelectionComponent.prototype.getActiveWeek = function () {
        var activeWeek = this.weekRangeContainer.find(".label-week.active");
        if (activeWeek.length) {
            var firstActiveWeek = activeWeek.eq(0);
            return {
                weekFromDate: firstActiveWeek.data("fromdate"),
                weekToDate: firstActiveWeek.data("todate"),
            };
        }
        return {};
    };
    WeekSelectionComponent.prototype.hide = function () {
        this.weekRangeContainer.addClass("hide");
    };
    WeekSelectionComponent.prototype.show = function () {
        this.weekRangeContainer.removeClass("hide");
    };
    return WeekSelectionComponent;
}());
//# sourceMappingURL=week-selection.js.map
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
            var isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            $('#report-form').toggle(!isHideFilter);
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("title", showFilterText);
            }
            else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
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
"use strict";
var WinLossDetailColumns = /** @class */ (function () {
    function WinLossDetailColumns(visibleColumnDropdownComponent, columnList) {
        this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        this.columnList = columnList;
    }
    WinLossDetailColumns.prototype.init = function (visibleColumns, onDropdownHidden) {
        var _this = this;
        this.visibleColumnDropdownComponent.render(new VisibleColumnDropdownOption(visibleColumns, this.columnList, function (columns) {
            _this.showHideColumns(columns);
            var selectedColumnsValue = [];
            Object.entries(columns).forEach(function (_a) {
                var key = _a[0], value = _a[1];
                return value && selectedColumnsValue.push(key);
            });
            onDropdownHidden(selectedColumnsValue.join());
        }));
    };
    WinLossDetailColumns.prototype.showHideColumns = function (visibleColumn) {
        if (visibleColumn === void 0) { visibleColumn = undefined; }
        var tableReport = $("#tbl-report");
        visibleColumn = visibleColumn || this.visibleColumnDropdownComponent.getColumns();
        this._showHideSpecificColumn(tableReport, visibleColumn, "fullname");
        this._showHideSpecificColumn(tableReport, visibleColumn, "betcount");
        this._showHideSpecificColumn(tableReport, visibleColumn, "turnover");
        this._showHideSpecificColumn(tableReport, visibleColumn, "netturnover");
        this._showHideSpecificColumn(tableReport, visibleColumn, "grosscomm");
        this._showHideGroupColumn(tableReport, visibleColumn, "p", "groupColumnMember");
        this._showHideGroupColumn(tableReport, visibleColumn, "a", "groupColumnAgent");
        this._showHideGroupColumn(tableReport, visibleColumn, "m", "groupColumnMaster");
        this._showHideGroupColumn(tableReport, visibleColumn, "s", "groupColumnSuper");
        this._showHideSpecificColumn(tableReport, visibleColumn, "company");
        this._showHideSpecificColumn(tableReport, visibleColumn, "reward");
    };
    WinLossDetailColumns.prototype._showHideGroupColumn = function (tableReport, visibleColumn, prefix, groupColumnId) {
        var groupColumn = tableReport.find("#" + groupColumnId);
        if (groupColumn.length) {
            var colspan = 0;
            if (this._showHideSpecificColumn(tableReport, visibleColumn, prefix + "winloss")) {
                colspan++;
            }
            if (this._showHideSpecificColumn(tableReport, visibleColumn, prefix + "comm")) {
                colspan++;
            }
            if (this._showHideSpecificColumn(tableReport, visibleColumn, prefix + "total")) {
                colspan++;
            }
            colspan ? groupColumn.attr("colspan", colspan).removeClass("hide") : groupColumn.addClass("hide");
        }
    };
    WinLossDetailColumns.prototype._showHideSpecificColumn = function (tableReport, visibleColumn, key) {
        var matchedColumns = tableReport.find("[data-colname=" + key + "]");
        var visible = visibleColumn[key];
        visible ? matchedColumns.removeClass("hide") : matchedColumns.addClass("hide");
        return visible;
    };
    WinLossDetailColumns.prototype.getShowColumns = function () {
        return this.visibleColumnDropdownComponent.getShowColumns();
    };
    WinLossDetailColumns.prototype.getHideColumns = function () {
        return this.visibleColumnDropdownComponent.getHideColumns();
    };
    return WinLossDetailColumns;
}());
//# sourceMappingURL=winlossdetail-columns.js.map
"use strict";
var WinLossDetailCopyHtml = /** @class */ (function () {
    function WinLossDetailCopyHtml(clipboard) {
        this.clipboard = clipboard;
    }
    WinLossDetailCopyHtml.prototype.init = function () {
        var _this = this;
        $("#icon-copy").tooltip();
        var clipboard = new this.clipboard("#icon-copy", {
            text: function () { return _this._copyTableHtml($("#tbl-report")); }
        });
        if (window.document.queryCommandSupported && window.document.queryCommandSupported("copy")) {
            clipboard.on("success", function (e) {
                var $this = $(e.trigger);
                $this.attr("data-original-title", $("#hiddenCopiedText").val());
                $this.tooltip("show");
                setTimeout(function () {
                    $this.tooltip("hide");
                    $this.attr("data-original-title", $("#hiddenCopyText").val());
                }, 3000);
            }).on("error", function (e) {
                $(e.trigger).remove();
                console.error("Action:", e.action);
                console.error("Trigger:", e.trigger);
            });
        }
    };
    WinLossDetailCopyHtml.prototype._copyTableHtml = function (tableReport) {
        var html = ['<table>'];
        html.push(this._copyTableHeader(tableReport));
        html.push(this._copyTableBody(tableReport));
        html.push(this._copyTableFooter(tableReport));
        html.push('</table>');
        return html.join("");
    };
    WinLossDetailCopyHtml.prototype._copyTableHeader = function (tableReport) {
        var html = ['<thead><tr>'];
        tableReport.find("thead tr:first th:not(.hide)").each(function () {
            var $this = $(this);
            html.push('<th');
            var rowspan = $this.attr("rowspan");
            if (rowspan) {
                html.push(' rowspan="', rowspan, '"');
            }
            var colspan = $this.attr("colspan");
            if (colspan) {
                html.push(' colspan="', colspan, '"');
            }
            html.push('>', this.innerText, '</th>');
        });
        html.push('</tr><tr>');
        tableReport.find("thead tr:last th:not(.hide)").each(function () {
            html.push('<th>', this.innerText, '</th>');
        });
        html.push('</tr></thead>');
        return html.join("");
    };
    WinLossDetailCopyHtml.prototype._copyTableBody = function (tableReport) {
        var getColumnText = function (td) {
            var title;
            var $td = $(td);
            if ($td.data("colname") === "fullname") {
                title = $td.find("div:first").data("original-title");
            }
            return title || td.innerText;
        };
        var html = ['<tbody>'];
        tableReport.find("tbody tr").each(function () {
            html.push('<tr>');
            $(this).find("td:not(.hide)").each(function () {
                html.push('<td>', getColumnText(this), '</td>');
            });
            html.push('</tr>');
        });
        html.push('</tbody>');
        return html.join("");
    };
    WinLossDetailCopyHtml.prototype._copyTableFooter = function (tableReport) {
        var html = ['<tfoot><tr>'];
        tableReport.find("tfoot tr td:not(.hide)").each(function () {
            html.push('<td>', this.innerText, '</td>');
        });
        html.push('</tr></tfoot>');
        return html.join("");
    };
    return WinLossDetailCopyHtml;
}());
//# sourceMappingURL=winlossdetail-copyhtml.js.map
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
    function WinLossDetailViewNew(totalEnteredNonEnteredComponent, dateRangePickerComponent, productAndProductDetailDropdownComponent, usernameAutoSuggestionComponent, winLossDetailColumns, weekSelectionComponent, breadcrumbLevelComponent, winLossDetailCopyHtml) {
        var _this = _super.call(this) || this;
        _this.isGettingReportData = false;
        _this.isWeekSelectionClicked = false;
        _this.totalEnteredNonEnteredComponent = totalEnteredNonEnteredComponent;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
        _this.usernameAutoSuggestionComponent = usernameAutoSuggestionComponent;
        _this.winLossDetailColumns = winLossDetailColumns;
        _this.weekSelectionComponent = weekSelectionComponent;
        _this.breadcrumbLevelComponent = breadcrumbLevelComponent;
        _this.winLossDetailCopyHtml = winLossDetailCopyHtml;
        // set default sort column name
        _this.sortColumnName = "username";
        return _this;
    }
    WinLossDetailViewNew.prototype.init = function () {
        this.reportWeekSelection = $("#report-week-selection");
        this.tableReportContainer = $("#tbl-report-container");
        this.winLossDetailGetDataUrl = $("#hiddenGetDataUrl").val();
        this.winLossDetailGetWeeksUrl = $("#hiddenGetWeeksUrl").val();
        this.winLossDetailGetDataWeekUrl = $("#hiddenGetDataWeekUrl").val();
        this.renderComponents();
        this.bindEvents();
        // remove style="visibility:hidden"
        // TableSorter cannot run with style="display:none"
        this.reportWeekSelection.removeAttr("style");
        setTimeout(function () {
            var mainDomain = $("#hiddenMainDomain").val();
            if (mainDomain.length) {
                mainDomain = mainDomain.indexOf("http") === -1
                    ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                    : mainDomain;
                window.top.postMessage({
                    event_id: "winlossdetailnew-feedback",
                    feedback_id: 2
                }, mainDomain);
            }
            else {
                window.top.indexManager.initFeedbackFunction(2);
            }
        }, 1000);
    };
    WinLossDetailViewNew.prototype.renderComponents = function () {
        var _this = this;
        var weekFromDate = $("#hiddenWeekFromDate");
        var weekToDate = $("#hiddenWeekToDate");
        this.totalEnteredNonEnteredComponent.init();
        this.dateRangePickerComponent.render({
            maxDays: 92,
            onClosed: function (dateRange) {
                //this.constraintDateRangeAndProductDropdownList(dateRange);
                var productsAndResources = _this.productAndProductDetailDropdownComponent.getCheckedValues();
                var username = _this.usernameAutoSuggestionComponent.getUsername();
                _this.resetActiveWeek();
                $.extend(_this.globalFormData, dateRange, productsAndResources, { username: username });
                if (_this.isInvalidGlobalFormData()) {
                    return;
                }
                _this.getWinLossDetailDataAndRenderWeekRange();
            }
        });
        this.productAndProductDetailDropdownComponent.render(new ProductAndProductDetailDropdownOption(window.productList, window.productDetailList, function (productsAndResources) {
            var dateRange = _this.dateRangePickerComponent.getDateRangeValues();
            var username = _this.usernameAutoSuggestionComponent.getUsername();
            _this.resetActiveWeek();
            $.extend(_this.globalFormData, dateRange, productsAndResources, { username: username });
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.getWinLossDetailDataAndRenderWeekRange();
        }));
        var dateRange = this.dateRangePickerComponent.getDateRangeValues();
        //this.constraintDateRangeAndProductDropdownList(dateRange);
        this.usernameAutoSuggestionComponent.init(new UsernameAutoSuggestionOption($("#hiddenUsernameAutoSuggestionUrl").val(), false, true, function () {
            _this.isGettingReportData = true;
            var dateRange = _this.dateRangePickerComponent.getDateRangeValues();
            var productsAndResources = _this.productAndProductDetailDropdownComponent.getCheckedValues();
            _this.resetActiveWeek();
            var username = _this.usernameAutoSuggestionComponent.getUsername();
            $.extend(_this.globalFormData, dateRange, productsAndResources, { username: username });
            // reset params: custId, custName, custLevelId
            delete _this.globalFormData.custId;
            delete _this.globalFormData.custName;
            delete _this.globalFormData.custLevelId;
            delete _this.globalFormData.breadcrumbLevelJson;
            if (_this.isInvalidGlobalFormData()) {
                _this.isGettingReportData = false;
                return;
            }
            _this.getWinLossDetailDataAndRenderWeekRange(true);
        }));
        this.winLossDetailColumns.init($("#hiddenShowColumns").val(), function (visibleColumns) {
            $.extend(_this.globalFormData, { showColumns: visibleColumns });
            _this.updateCurrentUrl();
        });
        this.weekSelectionComponent.init(new WeekSelectionOption(function (dateRange) {
            if (_this.isGettingReportData) {
                return;
            }
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.addIsWeekSelectionForStatistic();
            $.extend(_this.globalFormData, dateRange);
            _this.getWinLossDetailDataWeek();
        }));
        var activeWeekRange = weekFromDate.length > 0 && weekToDate.length > 0
            ? { weekFromDate: weekFromDate.val(), weekToDate: weekToDate.val() }
            : {};
        this.breadcrumbLevelComponent.init(new BreadcrumbLevelOption(this.buildBreadcrumbs(), function (breadcrumbData) {
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.usernameAutoSuggestionComponent.resetUsername();
            delete _this.globalFormData.username;
            var dateRange = _this.dateRangePickerComponent.getDateRangeValues();
            var activeWeek = _this.weekSelectionComponent.getActiveWeek();
            $.extend(_this.globalFormData, dateRange, activeWeek, breadcrumbData);
            _this.getWinLossDetailWeeks();
            _this.getWinLossDetailData(function () {
                _this.breadcrumbLevelComponent.removeBreadcrumbLevel(breadcrumbData.custLevelId);
                _this.updateCurrentUrl();
            });
        }, window.reportTitle));
        this.globalFormData = $.extend({}, {
            custId: +$("#hiddenCustId").val(),
            custName: $("#hiddenCustName").val(),
            custLevelId: +$("#hiddenCustLevel").val(),
            username: this.usernameAutoSuggestionComponent.getUsername(),
            showColumns: $("#hiddenShowColumns").val()
        }, dateRange, this.productAndProductDetailDropdownComponent.getCheckedValues(), activeWeekRange);
        // update fromDate/toDate of globalFormData if activeWeekRange is NOT undefined
        //if (activeWeekRange) {
        //    this.globalFormData.fromDate = activeWeekRange.weekFromDate;
        //    this.globalFormData.toDate = activeWeekRange.weekToDate;
        //}
    };
    //private constraintDateRangeAndProductDropdownList(dateRange) {
    //    const historicPivotDate = moment(this.dateRangePickerComponent.getHistoricPivotDate());
    //    if (moment(dateRange.fromDate).isSameOrAfter(historicPivotDate)) {
    //        this.productAndProductDetailDropdownComponent.hideHistoricProducts();
    //    } else {
    //        this.productAndProductDetailDropdownComponent.showHistoricProducts();
    //    }
    //}
    WinLossDetailViewNew.prototype.getWinLossDetailDataAndRenderWeekRange = function (isRebuildBreadcrumb) {
        var _this = this;
        if (isRebuildBreadcrumb === void 0) { isRebuildBreadcrumb = false; }
        this.getWinLossDetailWeeks();
        this.getWinLossDetailData(function () {
            if (isRebuildBreadcrumb) {
                _this.breadcrumbLevelComponent.setBreadcrumbs(_this.buildBreadcrumbs());
            }
            else {
                _this.breadcrumbLevelComponent.render();
            }
            _this.updateCurrentUrl();
        });
    };
    WinLossDetailViewNew.prototype.getWinLossDetailData = function (callback) {
        //this.addMoreParamsForStatistic();
        var _this = this;
        if (callback === void 0) { callback = Function(); }
        this.reportWeekSelection.block();
        $.get(this.winLossDetailGetDataUrl + "?" + $.param(this.globalFormData), function (html) {
            _this.reportWeekSelection.html(html);
            _this.tableReportContainer = $("#tbl-report-container");
            _this.bindCopyAndExcelEvents();
            _this.bindTableReportEvents();
            _this.totalEnteredNonEnteredComponent.update({ eventDate: _this.tableReport.data("yesterday"), nonEntered: _this.tableReport.data("non-entered-yesterday") }, { eventDate: _this.tableReport.data("today"), nonEntered: _this.tableReport.data("non-entered-today") });
            callback();
            _this.isWeekSelectionClicked = false;
            _this.reportWeekSelection.unblock();
            _this.isGettingReportData = false;
        }).fail(function (jqXHR) {
            _this.reportWeekSelection.unblock();
            _this.isGettingReportData = false;
            if (jqXHR.status === 429) {
                window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    var cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    var siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top.alpha.urls.mainRootPath;
                    window.location.href = siteMainUrl + "partialum/index?" + $.param(responseObject);
                }
            }
        }).always(function () { return _this.removeIsWeekSelectionForStatistic(); });
    };
    WinLossDetailViewNew.prototype.getWinLossDetailWeeks = function () {
        var _this = this;
        $.get(this.winLossDetailGetWeeksUrl + "?" + $.param(this.globalFormData), function (html) {
            return _this.weekSelectionComponent.reRender(html);
        }).fail(function (jqXHR) {
            if (jqXHR.status === 429) {
                window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    var cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    var siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top.alpha.urls.mainRootPath;
                    window.location.href = siteMainUrl + "partialum/index?" + $.param(responseObject);
                }
            }
        });
    };
    WinLossDetailViewNew.prototype.getWinLossDetailDataWeek = function (callback) {
        //this.addMoreParamsForStatistic();
        var _this = this;
        if (callback === void 0) { callback = Function(); }
        this.tableReportContainer.block();
        $.get(this.winLossDetailGetDataWeekUrl + "?" + $.param(this.globalFormData), function (html) {
            _this.tableReportContainer.html(html);
            _this.bindTableReportEvents();
            _this.totalEnteredNonEnteredComponent.update({ eventDate: _this.tableReport.data("yesterday"), nonEntered: _this.tableReport.data("non-entered-yesterday") }, { eventDate: _this.tableReport.data("today"), nonEntered: _this.tableReport.data("non-entered-today") });
            callback();
            _this.updateCurrentUrl();
            _this.isWeekSelectionClicked = true;
            _this.tableReportContainer.unblock();
            _this.isGettingReportData = false;
        }).fail(function (jqXHR) {
            _this.tableReportContainer.unblock();
            _this.isGettingReportData = false;
            if (jqXHR.status === 429) {
                window.location.href = window.top.alpha.urls.mainRootPath + "error/blocked";
            }
            else if (jqXHR.status === 500 && jqXHR.responseText) {
                var responseObject = JSON.parse(jqXHR.responseText);
                if (responseObject.UMFrom && responseObject.UMTo) {
                    var cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    var siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top.alpha.urls.mainRootPath;
                    window.location.href = siteMainUrl + "partialum/index?" + $.param(responseObject);
                }
            }
        }).always(function () { return _this.removeIsWeekSelectionForStatistic(); });
    };
    WinLossDetailViewNew.prototype.buildBreadcrumbs = function () {
        this.tableReport = this.tableReport || this.tableReportContainer.find("#tbl-report");
        var breadcrumbLevelJson = this.tableReport.data("breadcrumbleveljson");
        return typeof breadcrumbLevelJson !== "string"
            ? []
            : JSON.parse(breadcrumbLevelJson.replace(/'/g, '"'))
                .map(function (b) {
                return {
                    custId: b.CustId,
                    custName: b.CustName.toUpperCase(),
                    custLevelId: b.CustLevelId
                };
            });
    };
    WinLossDetailViewNew.prototype.isInvalidGlobalFormData = function () {
        return this.globalFormData.productIds.length === 0;
    };
    WinLossDetailViewNew.prototype.bindEvents = function () {
        var _this = this;
        this.initReportLegend($("#hiddenLegendUrl").val());
        this.initShowHideFilter(function (isHideFilter) { return isHideFilter ? _this.weekSelectionComponent.hide() : _this.weekSelectionComponent.show(); });
        $('#msg-warning-note').hide();
        $('#msg-warning-note').show();
        this.bindCopyAndExcelEvents();
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
            var dateRange = _this.dateRangePickerComponent.getDateRangeValues();
            var betListsParams = _this.weekSelectionComponent.getActiveWeek();
            $.extend(betListsParams, _this.globalFormData, dateRange, { custId: custId, custName: custName, breadcrumbLevelJson: breadcrumbLevelJson });
            $this.attr("href", $("#hiddenBetListsUrl").val() + "?" + $.param(betListsParams));
        });
        this.tableReport.find(".downline-link").off().click(function (e) {
            e.preventDefault();
            if (_this.isGettingReportData) {
                return;
            }
            if (_this.isInvalidGlobalFormData()) {
                return;
            }
            _this.usernameAutoSuggestionComponent.resetUsername();
            delete _this.globalFormData.username;
            var $this = $(e.target);
            var dateRange = _this.dateRangePickerComponent.getDateRangeValues();
            var activeWeek = _this.weekSelectionComponent.getActiveWeek();
            var breadcrumbData = {
                custId: $this.data("custid"),
                custName: $this.data("custname"),
                custLevelId: $this.data("custlevelid"),
                breadcrumbLevelJson: JSON.stringify(_this.breadcrumbLevelComponent.getBreadcrumbs())
            };
            $.extend(_this.globalFormData, dateRange, activeWeek, breadcrumbData);
            _this.getWinLossDetailWeeks();
            _this.getWinLossDetailData(function () {
                _this.breadcrumbLevelComponent.addBreadcrumbLevel(breadcrumbData);
                _this.updateCurrentUrl();
            });
            _this.tableReport.find(".icon-dormant-accounts").tooltip();
        });
        this.tableReport.find(".icon-plus-box").off("click").click(function (e) {
            window.top["reloadMyWatchlistDataTable"] = function () { return _this.isWeekSelectionClicked ? _this.getWinLossDetailDataWeek() : _this.getWinLossDetailDataAndRenderWeekRange(true); };
            var custName = $(e.target).data("custname");
            var url = $("#hiddenMemberInfoUrl").val() + "MyWatchlistAccounts/ManageWatchlistAccounts?usernames=" + custName;
            var width = 820;
            var height = 550;
            var mainDomain = $("#hiddenMainDomain").val();
            if (mainDomain.length) {
                mainDomain = mainDomain.indexOf("http") === -1
                    ? location.origin.split(":")[0] + ":" + mainDomain + "/site-main"
                    : mainDomain;
                window.top.postMessage({
                    event_id: "winlossdetailnew-addtomywatchlist",
                    url: url,
                    width: width,
                    height: height
                }, mainDomain);
            }
            else {
                window.top["popupManager"].open(url, undefined, width, height, "modal-mobile-friendly");
            }
        });
        this.initTableSorter();
        this.winLossDetailColumns.showHideColumns();
    };
    WinLossDetailViewNew.prototype.bindCopyAndExcelEvents = function () {
        var _this = this;
        this.winLossDetailCopyHtml.init();
        $("#icon-excel").off().click(function (e) {
            if (_this.isInvalidGlobalFormData()) {
                e.preventDefault();
                return;
            }
            var $this = $(e.target);
            $this.attr("href", $this.data("url") + "?" + $.param(_this.globalFormData));
            setTimeout(function () {
                $this.attr("href", "javascript:;");
            }, 1000);
        }).tooltip();
    };
    WinLossDetailViewNew.prototype.initTableSorter = function () {
        var _this = this;
        this.tableSorter(this.tableReport, undefined, function () {
            var dormantAccountsRow = _this.tableReport.find('.dormant-accounts-mark');
            dormantAccountsRow.length && _this.tableReport.append(dormantAccountsRow);
        });
    };
    WinLossDetailViewNew.prototype.mapSortColumnIndex = function () {
        var tableHeaderCells = this.tableReport.find("thead th[data-colname=" + this.sortColumnName + "]");
        if (tableHeaderCells.length) {
            this.sortColumnIndex = tableHeaderCells.eq(0).data("colindex");
        }
        else {
            // reset to default
            this.sortColumnName = "username";
            this.sortColumnIndex = 0;
            this.sortIndicator = 0;
        }
        return this.sortColumnIndex;
    };
    WinLossDetailViewNew.prototype.updateCurrentUrl = function () {
        if (this.globalFormData.productIds) {
            var formData = $.extend({}, this.globalFormData, { breadcrumbLevelJson: JSON.stringify(this.breadcrumbLevelComponent.getBreadcrumbs()) });
            _super.prototype.updateCurrentUrl.call(this, $.param(formData));
        }
    };
    WinLossDetailViewNew.prototype.addMoreParamsForStatistic = function () {
        var moreParams = {};
        var productAndProductDetail = this.productAndProductDetailDropdownComponent.getUncheckedValues();
        if (productAndProductDetail.unselectedProductIds) {
            moreParams.unselectedProductIds = productAndProductDetail.unselectedProductIds;
        }
        else {
            delete this.globalFormData.unselectedProductIds;
        }
        if (productAndProductDetail.unselectedResourceIds) {
            moreParams.unselectedResourceIds = productAndProductDetail.unselectedResourceIds;
        }
        else {
            delete this.globalFormData.unselectedResourceIds;
        }
        var hideColumns = this.winLossDetailColumns.getHideColumns();
        if (hideColumns) {
            moreParams.hideColumns = hideColumns;
        }
        else {
            delete this.globalFormData.hideColumns;
        }
        $.extend(this.globalFormData, moreParams);
    };
    WinLossDetailViewNew.prototype.addIsWeekSelectionForStatistic = function () {
        this.globalFormData.isWeekSelection = 1;
    };
    WinLossDetailViewNew.prototype.removeIsWeekSelectionForStatistic = function () {
        delete this.globalFormData.isWeekSelection;
    };
    WinLossDetailViewNew.prototype.resetActiveWeek = function () {
        delete this.globalFormData.weekFromDate;
        delete this.globalFormData.weekToDate;
    };
    return WinLossDetailViewNew;
}(BaseReportView));
$(function () {
    new WinLossDetailViewNew(new TotalEnteredNonEnteredComponent(), new DateRangePickerJQueryComponent(), new ProductAndProductDetailDropdownComponent(), new UsernameAutoSuggestionComponent(), new WinLossDetailColumns(new VisibleColumnDropdownComponent(), window.columnList), new WeekSelectionComponent(), new BreadcrumbLevelComponent(), new WinLossDetailCopyHtml(Clipboard)).init();
});
//# sourceMappingURL=winlossdetail-new.js.map