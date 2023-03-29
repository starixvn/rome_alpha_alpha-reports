"use strict";
var AlphaReports = /** @class */ (function () {
    function AlphaReports($, store) {
        var _this = this;
        this.format = {
            monthFormat: "MMMM - yyyy",
            shortDateWithSlash_DatePicker: "MM/DD/YYYY",
            shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
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
var TotalEnteredNonEnteredOption = /** @class */ (function () {
    function TotalEnteredNonEnteredOption(nonEnteredText, enteredText, selectorId, dateFormat) {
        if (selectorId === void 0) { selectorId = "total-entered-non-entered"; }
        if (dateFormat === void 0) { dateFormat = "MM/DD"; }
        this.nonEnteredText = nonEnteredText;
        this.enteredText = enteredText;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
    return TotalEnteredNonEnteredOption;
}());
var TotalEnteredNonEnteredComponent = /** @class */ (function () {
    function TotalEnteredNonEnteredComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    TotalEnteredNonEnteredComponent.prototype.init = function (options) {
        this.totalEnteredNonEnteredOption = options;
        this.totalEnteredNonEntered = this.$("#" + options.selectorId);
    };
    TotalEnteredNonEnteredComponent.prototype.render = function (data) {
        var html = [];
        data.EnteredSportYesterday.NonEntered
            ? this.renderNonEntered(html, data.EnteredSportYesterday)
            : this.renderEntered(html, data.EnteredSportYesterday);
        this.renderNonEntered(html, data.EnteredSportToday);
        this.totalEnteredNonEntered.html(html.join(""));
    };
    TotalEnteredNonEnteredComponent.prototype.renderNonEntered = function (html, data) {
        html.push('<div class="icon-non-entered"><div class="text">', this.moment(data.EventDate).format(this.totalEnteredNonEnteredOption.dateFormat), ': ', this.totalEnteredNonEnteredOption.nonEnteredText, '</div><div class="number">', data.NonEntered, '</div><div class="icon-clock"></div></div>');
    };
    TotalEnteredNonEnteredComponent.prototype.renderEntered = function (html, data) {
        html.push('<div class="icon-entered"><div class="text">', this.moment(data.EventDate).format(this.totalEnteredNonEnteredOption.dateFormat), ': ', this.totalEnteredNonEnteredOption.enteredText, '</div><div class="icon-check-circle"></div>');
    };
    return TotalEnteredNonEnteredComponent;
}());
//# sourceMappingURL=total-entered-nonentered.js.map
"use strict";
var DateRangePickerJQueryOption = /** @class */ (function () {
    function DateRangePickerJQueryOption(onClosed, selectorId, dateFormat) {
        if (onClosed === void 0) { onClosed = Function(); }
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (dateFormat === void 0) { dateFormat = "MM/DD/YYYY"; }
        this.onClosed = onClosed;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
        this.onClosed = onClosed;
        this.selectorId = selectorId;
        this.dateFormat = dateFormat;
    }
    return DateRangePickerJQueryOption;
}());
var DateRangePickerJQueryComponent = /** @class */ (function () {
    function DateRangePickerJQueryComponent($) {
        this.isHistoricReport = false;
        this.$ = $;
    }
    DateRangePickerJQueryComponent.prototype.render = function (options) {
        this.renderDateRangePicker(this.$("#" + options.selectorId), options);
    };
    DateRangePickerJQueryComponent.prototype.renderDateRangePicker = function (dateRangePicker, options, raiseOnClosedEvent) {
        var _this = this;
        if (raiseOnClosedEvent === void 0) { raiseOnClosedEvent = false; }
        var olderThan3Months = dateRangePicker.data("label-olderthan3months");
        var backToCurrentReport = dateRangePicker.data("label-backtocurrentreport");
        dateRangePicker
            .dateRangePicker(this.getDateRangeOptions(dateRangePicker, options))
            .bind("datepicker-change", function (_, obj) {
            if (obj.value === olderThan3Months || obj.value === backToCurrentReport) {
                _this.isHistoricReport = obj.value === olderThan3Months;
                dateRangePicker.off();
                dateRangePicker.data("dateRangePicker").destroy();
                _this.renderDateRangePicker(dateRangePicker, options, true);
            }
            else {
                _this.fromDate = moment(obj.date1).format(options.dateFormat);
                _this.toDate = moment(obj.date2).format(options.dateFormat);
            }
        }).bind("datepicker-closed", function () {
            options.onClosed();
        }).data("dateRangePicker").setDateRange(this.fromDate, this.toDate, false);
        raiseOnClosedEvent && options.onClosed();
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeOptions = function (dateRangePicker, options) {
        var dateRangeOptions;
        if (this.isHistoricReport) {
            var today = moment(dateRangePicker.data("currentdate"));
            this.fromDate = moment(today).startOf("M").format(options.dateFormat);
            this.toDate = today.format(options.dateFormat);
            dateRangeOptions = {
                startDate: dateRangePicker.data("historicmindate"),
                endDate: moment(today).endOf("M").format(options.dateFormat),
                customShortcuts: this.getHistoricPredefinedRanges(dateRangePicker),
                batchMode: "month",
            };
        }
        else {
            this.fromDate = dateRangePicker.data("fromdate");
            this.toDate = dateRangePicker.data("todate");
            dateRangeOptions = {
                startDate: dateRangePicker.data("mindate"),
                endDate: dateRangePicker.data("maxdate"),
                customShortcuts: this.getPredefinedRanges(dateRangePicker)
            };
        }
        return this.$.extend({
            format: options.dateFormat,
            separator: " - ",
            language: "en",
            showShortcuts: true,
            shortcuts: null,
            duration: 0,
            singleMonth: true
        }, dateRangeOptions);
    };
    DateRangePickerJQueryComponent.prototype.getPredefinedRanges = function (dateRangePicker) {
        var today = moment(dateRangePicker.data("currentdate"));
        var todayValue = today.toDate();
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var ranges = [
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
                    return [lastMonthStartOfDay.toDate(), moment(lastMonthStartOfDay).endOf("M").toDate()];
                }
            },
            {
                name: dateRangePicker.data("label-sincelastmonth"),
                dates: function () {
                    return [lastMonthStartOfDay.toDate(), todayValue];
                }
            }
        ];
        if (dateRangePicker.data("isshowhistoricreport") === 1) {
            ranges.push({
                name: dateRangePicker.data("label-olderthan3months"),
                dates: function () {
                    return [undefined, undefined];
                }
            });
        }
        return ranges;
    };
    DateRangePickerJQueryComponent.prototype.getHistoricPredefinedRanges = function (dateRangePicker) {
        return [{
                name: dateRangePicker.data("label-backtocurrentreport"),
                dates: function () {
                    return [undefined, undefined];
                }
            }];
    };
    DateRangePickerJQueryComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate, isHistoricReport: this.isHistoricReport };
    };
    return DateRangePickerJQueryComponent;
}());
//# sourceMappingURL=daterangepicker-jquery.js.map
"use strict";
var ProductAndProductDetailDropdownOption = /** @class */ (function () {
    function ProductAndProductDetailDropdownOption(onDropdownHidden, productSelectorId, productDetailSelectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (productSelectorId === void 0) { productSelectorId = "dropdown-product"; }
        if (productDetailSelectorId === void 0) { productDetailSelectorId = "dropdown-product-detail"; }
        this.onDropdownHidden = onDropdownHidden;
        this.productSelectorId = productSelectorId;
        this.productDetailSelectorId = productDetailSelectorId;
        this.onDropdownHidden = onDropdownHidden;
        this.productSelectorId = productSelectorId;
        this.productDetailSelectorId = productDetailSelectorId;
    }
    return ProductAndProductDetailDropdownOption;
}());
var ProductAndProductDetailDropdownComponent = /** @class */ (function () {
    function ProductAndProductDetailDropdownComponent($) {
        var _this = this;
        this.optionClass = function (element) {
            if (_this.$(element).data("isnested") === 1) {
                return "multiselect-nested-item";
            }
        };
        this.onChangeProductDetail = function (option, checked) {
            var productId = _this.$(option.context).data("productid");
            if (checked) {
                if (!_this.$dropdownProduct.find("option[value=\"" + productId + "\"]:selected").length) {
                    _this.$dropdownProduct.multiselect("select", productId);
                }
            }
            else {
                if (!_this.$dropdownProductDetail.find("option[data-productid=\"" + productId + "\"]:selected").length) {
                    _this.$dropdownProduct.multiselect("deselect", productId);
                }
            }
            _this.validateProduct();
        };
        this.onSelectAllProductDetail = function (e) {
            var productIds = [];
            _this.$dropdownProductDetail.find("option").each(function (_, item) { return productIds.push(_this.$(item).data("productid")); });
            var uniqueProductIds = productIds.filter(function (value, index) { return productIds.indexOf(value) === index; });
            var checked = _this.$(e.target).is(":checked");
            _this.$dropdownProduct.multiselect(checked ? "select" : "deselect", uniqueProductIds);
            _this.validateProduct();
        };
        this.$ = $;
    }
    ProductAndProductDetailDropdownComponent.prototype.render = function (options) {
        this.renderProduct(options);
        this.renderProductDetail(options);
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProduct = function (options) {
        var _this = this;
        this.$dropdownProduct = this.$("#" + options.productSelectorId);
        this.$dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProduct.data("label-select-products"),
            nSelectedText: " " + this.$dropdownProduct.data("label-selected"),
            allSelectedText: this.$dropdownProduct.data("label-all-products"),
            selectAllText: this.$dropdownProduct.data("label-select-all"),
            includeSelectAllOption: true,
            onChange: function (option, checked) { return _this.checkOrUncheckProductDetail(checked, option.context.value); },
            enableCollapsibleOptGroups: true,
            optionClass: this.optionClass,
            onDropdownHidden: options.onDropdownHidden
        });
        this.$dropdownProduct.next().find(".multiselect-container li").removeClass("hidden");
        this.$dropdownProductContainer = this.$dropdownProduct.parent().find(".multiselect.btn");
        // Cannot use build in function 'onSelectAll'.
        // Must register CheckAll checkbox event manually.
        this.$("#product-multiselect-container .multiselect-all input[type=checkbox]").change(function (e) { return _this.checkOrUncheckProductDetail(_this.$(e.target).is(":checked")); });
    };
    ProductAndProductDetailDropdownComponent.prototype.checkOrUncheckProductDetail = function (checked, productId) {
        if (productId === void 0) { productId = undefined; }
        this.validateProduct();
        var optionElements = productId
            ? this.$dropdownProductDetail.find("option[data-productid=\"" + productId + "\"]")
            : this.$dropdownProductDetail.find("option");
        var selectedProductIds = [];
        optionElements.each(function (_, item) { return selectedProductIds.push(item["value"]); });
        this.$dropdownProductDetail.multiselect(checked ? "select" : "deselect", selectedProductIds);
    };
    ProductAndProductDetailDropdownComponent.prototype.validateProduct = function () {
        if (this.$dropdownProduct.find("option:selected").length) {
            this.$dropdownProductContainer.removeClass("error");
        }
        else {
            this.$dropdownProductContainer.addClass("error");
        }
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDetail = function (options) {
        this.$dropdownProductDetail = this.$("#" + options.productDetailSelectorId);
        var selectAllText = this.$dropdownProductDetail.data("label-all-product-detail");
        this.$dropdownProductDetail.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProductDetail.data("label-select-product-detail"),
            nSelectedText: " " + this.$dropdownProductDetail.data("label-selected"),
            allSelectedText: selectAllText,
            selectAllText: selectAllText,
            includeSelectAllOption: true,
            onChange: this.onChangeProductDetail,
            onDropdownHidden: options.onDropdownHidden
        });
        // Cannot use build in function 'onSelectAll'.
        // Must register CheckAll checkbox event manually.
        this.$("#product-detail-multiselect-container .multiselect-all input[type=checkbox]").change(this.onSelectAllProductDetail);
    };
    ProductAndProductDetailDropdownComponent.prototype.getProductSelectedItems = function () {
        var selectedProductIds = [];
        this.$dropdownProduct.find("option:selected").each(function (_, item) { return selectedProductIds.push(item["value"]); });
        return selectedProductIds.join();
    };
    ProductAndProductDetailDropdownComponent.prototype.getProductDetailSelectedItems = function () {
        var selectedProductDetailIds = [];
        this.$dropdownProductDetail.find("option:selected").each(function (_, item) { return selectedProductDetailIds.push(item["value"]); });
        return selectedProductDetailIds.join();
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map
"use strict";
var ProductAndSubProductDropdownOption = /** @class */ (function () {
    function ProductAndSubProductDropdownOption(products, productDetails, onDropdownHidden, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "ddlProduct"; }
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
    return ProductAndSubProductDropdownOption;
}());
var ProductAndSubProductDropdownComponent = /** @class */ (function () {
    function ProductAndSubProductDropdownComponent($) {
        this.$ = $;
    }
    ProductAndSubProductDropdownComponent.prototype.render = function (options) {
        var _this = this;
        var productDetailObject = this.buildProductDetailObject(options.productDetails);
        var productList = [];
        options.products.forEach(function (item) {
            var isGroup = typeof item.Products !== "undefined";
            var cssClass = isGroup ? "groupproduct" : "product";
            var productItem = _this.buildCheckboxProduct(item, productDetailObject, cssClass);
            if (isGroup) {
                productItem.items = item.Products.map(function (item) { return _this.buildCheckboxProduct(item, productDetailObject); });
            }
            productList.push(productItem);
        });
        this.$dropdownProduct = this.$("#ddlProduct").fanexMultiSelect({
            checkAllText: "Select All",
            selectedText: "# Selected",
            noneSelectedText: "None Selected",
            noItemFoundText: "No Product Found",
            useCustomCheckAllText: true,
            customCheckAllText: "All (#)",
            showFilter: true,
            filterPlaceHolder: "",
            isCheckAll: true,
            minWidth: 250,
            //maxHeight: 250,
            listData: productList,
            onLeave: options.onDropdownHidden
        });
    };
    ProductAndSubProductDropdownComponent.prototype.buildProductDetailObject = function (productDetails) {
        var _this = this;
        var productDetailObject = {};
        productDetails.forEach(function (item) {
            var items = productDetailObject[item.ProductId];
            if (typeof items === "undefined") {
                items = [];
                productDetailObject[item.ProductId] = items;
            }
            items.push(_this.buildCheckboxProductDetail(item));
        });
        return productDetailObject;
    };
    ProductAndSubProductDropdownComponent.prototype.buildCheckboxProduct = function (product, productDetailObject, cssClass) {
        if (cssClass === void 0) { cssClass = "product"; }
        var checkboxItem = this.buildCheckboxItem(product, cssClass);
        checkboxItem.items = productDetailObject[product.Id];
        return checkboxItem;
    };
    ProductAndSubProductDropdownComponent.prototype.buildCheckboxProductDetail = function (product) {
        return this.buildCheckboxItem(product, "productdetail", product.ProductId);
    };
    ProductAndSubProductDropdownComponent.prototype.buildCheckboxItem = function (item, cssClass, name) {
        if (name === void 0) { name = undefined; }
        return {
            id: cssClass + "-" + item.Id,
            name: name,
            value: item.Id,
            text: item.Name,
            cssClass: cssClass,
            checked: true,
            enable: true
        };
    };
    ProductAndSubProductDropdownComponent.prototype.getSelectedItems = function () {
        var productIds = this.getSelectedProductIds();
        var result = this.getSelectedResourceIds();
        return {
            productIds: this.uniqe(productIds.concat(result.parentProductIds)).join(),
            resourceIds: result.resourceIds.join()
        };
    };
    ProductAndSubProductDropdownComponent.prototype.getSelectedProductIds = function () {
        var productIds = [];
        this.$dropdownProduct.fanexMultiSelect("getItemsByCondition", '.product > input[type="checkbox"]:checked').each(function () {
            productIds.push(this.value);
        });
        return productIds;
    };
    ProductAndSubProductDropdownComponent.prototype.getSelectedResourceIds = function () {
        var parentProductIds = [];
        var resourceIds = [];
        this.$dropdownProduct.fanexMultiSelect("getItemsByCondition", '.productdetail > input[type="checkbox"]:checked').each(function () {
            parentProductIds.push(this.name);
            resourceIds.push(this.value);
        });
        return { parentProductIds: parentProductIds, resourceIds: resourceIds, };
    };
    ProductAndSubProductDropdownComponent.prototype.uniqe = function (array) {
        var uniqeObject = {};
        return array.filter(function (item) {
            return uniqeObject.hasOwnProperty(item)
                ? false
                : (uniqeObject[item] = true);
        });
    };
    return ProductAndSubProductDropdownComponent;
}());
//# sourceMappingURL=product-subproduct-dropdown.js.map
"use strict";
var VisibleColumnDropdownOption = /** @class */ (function () {
    function VisibleColumnDropdownOption(visibleColumnKey, onDropdownHidden, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-column"; }
        this.visibleColumnKey = visibleColumnKey;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.visibleColumnKey = visibleColumnKey;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
    return VisibleColumnDropdownOption;
}());
var VisibleColumnDropdownComponent = /** @class */ (function () {
    function VisibleColumnDropdownComponent($) {
        this.$ = $;
    }
    VisibleColumnDropdownComponent.prototype.render = function (options) {
        var _this = this;
        this.$dropdownColumn = this.$("#" + options.selectorId);
        this.$dropdownColumn.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownColumn.data("label-select-columns"),
            nSelectedText: " " + this.$dropdownColumn.data("label-selected"),
            allSelectedText: this.$dropdownColumn.data("label-all-columns"),
            selectAllText: this.$dropdownColumn.data("label-select-all"),
            includeSelectAllOption: true,
            onDropdownHidden: function () { return _this.onDropdownHidden(options.visibleColumnKey, options.onDropdownHidden); }
        });
        var visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (visibleColumns) {
            this.$dropdownColumn.multiselect("select", visibleColumns.split(","));
        }
        else {
            this.$dropdownColumn.multiselect("selectAll", false);
            this.$dropdownColumn.multiselect("updateButtonText");
        }
        return this.getColumns();
    };
    VisibleColumnDropdownComponent.prototype.onDropdownHidden = function (visibleColumnKey, callback) {
        var _this = this;
        var selectedColumnsValue = [];
        var columns = {};
        this.$dropdownColumn.find("option").each(function (_, item) {
            var $this = _this.$(item);
            var value = $this.val();
            var checked = $this.is(":checked");
            checked && selectedColumnsValue.push(value);
            columns[value] = checked;
        });
        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
        callback(columns);
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        var _this = this;
        var columns = {};
        this.$dropdownColumn.find("option").each(function (_, item) {
            var $this = _this.$(item);
            columns[$this.val()] = $this.is(":checked");
        });
        return columns;
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map
"use strict";
var WeekRangeOption = /** @class */ (function () {
    function WeekRangeOption(onClick, selectorId, textFromDateFormat, textToDateFormat, valueDateFormat) {
        if (onClick === void 0) { onClick = Function(); }
        if (selectorId === void 0) { selectorId = "labelWeekRange"; }
        if (textFromDateFormat === void 0) { textFromDateFormat = "DD"; }
        if (textToDateFormat === void 0) { textToDateFormat = "DD MMM"; }
        if (valueDateFormat === void 0) { valueDateFormat = "MM/DD/YYYY"; }
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.textFromDateFormat = textFromDateFormat;
        this.textToDateFormat = textToDateFormat;
        this.valueDateFormat = valueDateFormat;
        this.onClick = onClick;
        this.selectorId = selectorId;
        this.textFromDateFormat = textFromDateFormat;
        this.textToDateFormat = textToDateFormat;
        this.valueDateFormat = valueDateFormat;
    }
    return WeekRangeOption;
}());
var WeekRangeComponent = /** @class */ (function () {
    function WeekRangeComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    WeekRangeComponent.prototype.init = function (options) {
        this.weekRangeOption = options;
        this.labelWeekRange = $("#" + options.selectorId);
    };
    WeekRangeComponent.prototype.render = function (formData) {
        var _this = this;
        this.labelWeekRange.html(this.buildWeekRangeHtml(formData.fromDate, formData.toDate));
        var labelWeeks = this.labelWeekRange.find(".label-week");
        labelWeeks
            .off()
            .click(function (e) {
            var $this = _this.$(e.target);
            labelWeeks.removeClass("active");
            $this.addClass("active");
            _this.weekRangeOption.onClick($this.data("fromdate"), $this.data("todate"));
        });
    };
    WeekRangeComponent.prototype.buildWeekRangeHtml = function (fromDate, toDate) {
        var weekRange = [];
        var startDate = this.moment(fromDate);
        var endDate = this.moment(toDate);
        while (endDate.diff(startDate, "d") >= 0) {
            var endDateStartOfWeek = this.moment(endDate).startOf("isoWeek");
            var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
            var fromDateValue = startDateOrEndDateStartOfWeek.format(this.weekRangeOption.valueDateFormat);
            var toDateValue = endDate.format(this.weekRangeOption.valueDateFormat);
            var label = startDateOrEndDateStartOfWeek.isSame(endDate)
                ? endDate.format(this.weekRangeOption.textToDateFormat)
                : startDateOrEndDateStartOfWeek.format(this.weekRangeOption.textFromDateFormat) + " - " + endDate.format(this.weekRangeOption.textToDateFormat);
            weekRange.push("<a href=\"javascript:;\" class=\"label-week\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</a>");
            endDate = this.moment(endDateStartOfWeek).subtract(1, "d");
        }
        return weekRange.join("");
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map
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