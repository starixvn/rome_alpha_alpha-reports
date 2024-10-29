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
/// <reference path="../core.ts" />
"use strict";
var ReportTooltipOption = /** @class */ (function () {
    function ReportTooltipOption(tooltipId, custId) {
        if (tooltipId === void 0) { tooltipId = "tooltip"; }
        if (custId === void 0) { custId = 0; }
        this.tooltipId = tooltipId;
        this.custId = custId;
    }
    return ReportTooltipOption;
}());
var ReportTooltip = /** @class */ (function () {
    function ReportTooltip($) {
        var _this = this;
        this.tooltipCookieId = "TooltipShowCount";
        this.timeToShowTooltip = 5000;
        this.show = function (tooltipOption) {
            if (tooltipOption === void 0) { tooltipOption = new ReportTooltipOption(); }
            var tooltipCookieId = _this.tooltipCookieId + "-" + tooltipOption.tooltipId + "-" + tooltipOption.custId;
            var tooltipShowCount = parseInt(alphaReports.localStorage.get(tooltipCookieId), 0) || 0;
            if (tooltipShowCount >= alphaReports.maxTimesToShowTooltip) {
                return;
            }
            var tooltipElement = _this.$("#" + tooltipOption.tooltipId);
            tooltipElement.tooltip({ placement: "bottom" });
            tooltipElement.tooltip("show");
            alphaReports.localStorage.set(tooltipCookieId, tooltipShowCount + 1);
            var this$ = _this.$;
            setTimeout(function () {
                tooltipElement.tooltip("hide");
                this$(document).off("click.tooltip");
            }, _this.timeToShowTooltip);
            _this.$(document).off("click.tooltip").on("click.tooltip", function () {
                tooltipElement.tooltip("hide");
            });
        };
        this.$ = $;
    }
    return ReportTooltip;
}());
//# sourceMappingURL=tooltip.js.map
/// <reference path="../core.ts" />
"use strict";
var ProductMultiSelectionOption = /** @class */ (function () {
    function ProductMultiSelectionOption(dropdownId, validateBeforeSubmit, productsInputId, onChange) {
        if (dropdownId === void 0) { dropdownId = "dropdown-product"; }
        if (validateBeforeSubmit === void 0) { validateBeforeSubmit = true; }
        if (productsInputId === void 0) { productsInputId = "product-ids"; }
        if (onChange === void 0) { onChange = null; }
        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
        this.productsInputId = productsInputId;
        this.onChange = onChange;
    }
    return ProductMultiSelectionOption;
}());
var ProductMultiSelectionComponent = /** @class */ (function () {
    function ProductMultiSelectionComponent($, productDetailMultiSelectionComponent) {
        if (productDetailMultiSelectionComponent === void 0) { productDetailMultiSelectionComponent = undefined; }
        var _this = this;
        this.options = new ProductMultiSelectionOption();
        this.handleSelectedAllChanged = false;
        this.render = function (options) {
            if (options === void 0) { options = new ProductMultiSelectionOption(); }
            _this.productDetailMultiSelectionComponent && _this.productDetailMultiSelectionComponent.render();
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$dropdownProduct = _this.$("#" + _this.options.dropdownId);
            var hasAnyProduct = alphaReports.convertAttributeToBool(_this.$dropdownProduct.data("has-any-product"));
            _this.$hiddenInput = _this.$("#" + _this.options.productsInputId);
            _this.$excludeProducts = _this.$(".exclude-product");
            if (_this.$excludeProducts.length) {
                _this.$excludeHiddenInput = _this.$("#exclude-product-ids");
                _this.$excludeProducts
                    .off("change")
                    .on("change", _this.excludeProductOnChange);
            }
            _this.$dropdownProduct.multiselect({
                maxHeight: 300,
                numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
                nonSelectedText: hasAnyProduct
                    ? _this.$dropdownProduct.data("label-select-products")
                    : _this.$dropdownProduct.data("label-there-is-no-product"),
                nSelectedText: " " + _this.$dropdownProduct.data("label-selected"),
                allSelectedText: _this.$dropdownProduct.data("label-all-products"),
                selectAllText: _this.$dropdownProduct.data("label-select-all"),
                includeSelectAllOption: hasAnyProduct,
                onChange: _this.onChange,
                onSelectAll: _this.onChange,
                language: _this.$dropdownProduct.data("language"),
                enableCollapsibleOptGroups: true,
                optionClass: _this.optionClass
            });
            _this.$dropdownProduct.next().find(".multiselect-container li").removeClass("hidden");
            _this.$multiSelectButton = _this.$dropdownProduct.parent().find(".multiselect.btn");
            if (_this.productDetailMultiSelectionComponent) {
                var self_1 = _this;
                _this.$dropdownProduct.find("option").each(function () {
                    self_1.productDetailMultiSelectionComponent.shouldEnableDropdown(self_1.$(this).is(":selected"), this.value);
                });
            }
            _this.handleSelectedAllChanged = true;
            return _this.$dropdownProduct;
        };
        this.onChange = function (option, checked) {
            _this.validate();
            _this.$hiddenInput.val(_this.getSelectedItems().join(","));
            if (_this.handleSelectedAllChanged && _this.productDetailMultiSelectionComponent) {
                typeof option === "boolean"
                    ? _this.productDetailMultiSelectionComponent.disableAllOptions(option)
                    : _this.productDetailMultiSelectionComponent.disableOptions(checked, option.context.value);
            }
            if (_this.options.onChange) {
                _this.options.onChange();
            }
        };
        this.optionClass = function (element) {
            if (_this.$(element).data("isnested") === 1) {
                return "multiselect-nested-item";
            }
        };
        this.validate = function () {
            if (_this.options.validateBeforeSubmit
                && _this.$multiSelectButton
                && _this.$dropdownProduct.find("option:selected").length
                && _this.$multiSelectButton.hasClass("error")) {
                _this.$multiSelectButton.removeClass("error");
                _this.removeErrorClassFromExcludeProductLabels();
            }
        };
        this.getSelectedItems = function () {
            var selected = [];
            var selectedOptions = _this.$dropdownProduct.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected.concat(_this.getExcludeSelectedItems());
        };
        this.excludeProductOnChange = function () {
            _this.validateExcludeProductOnChange();
            _this.$hiddenInput.val(_this.getSelectedItems().join(","));
            _this.$excludeHiddenInput.val(_this.getExcludeSelectedItems().join(","));
        };
        this.getExcludeSelectedItems = function () {
            var selected = [];
            if (_this.$excludeProducts.length) {
                var self_2 = _this;
                _this.$excludeProducts.each(function () {
                    if (this.checked) {
                        selected.push(self_2.$(this).val());
                    }
                });
            }
            return selected;
        };
        this.addClassToExcludeProductLabels = function (className) {
            if (_this.$excludeProducts.length) {
                var self_3 = _this;
                _this.$excludeProducts.each(function () {
                    self_3.$(this).closest("label").addClass(className);
                });
            }
        };
        this.removeErrorClassFromExcludeProductLabels = function () {
            if (_this.$excludeProducts.length) {
                var self_4 = _this;
                _this.$excludeProducts.each(function () {
                    self_4.$(this).closest("label").removeClass("error");
                });
            }
        };
        this.validateExcludeProductOnChange = function () {
            if (_this.options.validateBeforeSubmit
                && _this.$multiSelectButton
                && _this.$multiSelectButton.hasClass("error")) {
                _this.$multiSelectButton.removeClass("error");
                _this.removeErrorClassFromExcludeProductLabels();
            }
        };
        this.isAnyEnabledProductDetailSelected = function () {
            return !_this.productDetailMultiSelectionComponent
                || _this.productDetailMultiSelectionComponent.isAnyEnabledSelected();
        };
        this.addErrorToProductDetailComponent = function () {
            _this.productDetailMultiSelectionComponent
                && _this.productDetailMultiSelectionComponent.addError();
        };
        this.$ = $;
        this.productDetailMultiSelectionComponent = productDetailMultiSelectionComponent;
    }
    return ProductMultiSelectionComponent;
}());
//# sourceMappingURL=productmultiselection.js.map
/// <reference path="../core.ts" />
"use strict";
var ProductDetailMultiSelectionOption = /** @class */ (function () {
    function ProductDetailMultiSelectionOption(dropdownId, validateBeforeSubmit, resourcesInputId) {
        if (dropdownId === void 0) { dropdownId = "dropdown-product-detail"; }
        if (validateBeforeSubmit === void 0) { validateBeforeSubmit = true; }
        if (resourcesInputId === void 0) { resourcesInputId = "resource-ids"; }
        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
        this.resourcesInputId = resourcesInputId;
    }
    return ProductDetailMultiSelectionOption;
}());
var ProductDetailMultiSelectionComponent = /** @class */ (function () {
    function ProductDetailMultiSelectionComponent($) {
        var _this = this;
        this.options = new ProductDetailMultiSelectionOption();
        this.render = function (options) {
            if (options === void 0) { options = new ProductDetailMultiSelectionOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$dropdownProductDetail = _this.$("#" + _this.options.dropdownId);
            _this.$hiddenInput = _this.$("#" + _this.options.resourcesInputId);
            var hasAnyProductDetail = "True" === _this.$dropdownProductDetail.data("has-any-product-detail");
            var selectAllText = _this.$dropdownProductDetail.data("label-all-product-detail");
            _this.$dropdownProductDetail.multiselect({
                maxHeight: 300,
                numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
                nonSelectedText: _this.$dropdownProductDetail.data("label-select-product-detail"),
                nSelectedText: " " + _this.$dropdownProductDetail.data("label-selected"),
                allSelectedText: selectAllText,
                selectAllText: selectAllText,
                includeSelectAllOption: hasAnyProductDetail,
                onChange: _this.onChange,
                onSelectAll: _this.onChange,
                language: _this.$dropdownProductDetail.data("language")
            });
            _this.$multiSelectButton = _this.$dropdownProductDetail.parent().find(".multiselect.btn");
            return _this.$dropdownProductDetail;
        };
        this.onChange = function () {
            _this.validate();
            _this.$hiddenInput.val(_this.getSelectedItems().join(","));
        };
        this.getSelectedItems = function () {
            var selected = [];
            var this$ = _this.$;
            _this.$dropdownProductDetail.find("option:selected").each(function () {
                selected.push(this$(this).val());
            });
            return selected;
        };
        this.disableAllOptions = function (checked) {
            _this.$dropdownProductDetail.multiselect(checked ? "enable" : "disable");
            _this.$dropdownProductDetail
                .find("option")
                .prop("selected", checked)
                .prop("disabled", !checked);
            _this.$dropdownProductDetail.multiselect("refresh");
        };
        this.disableOptions = function (checked, productId) {
            var $options = _this.$dropdownProductDetail.find('option[data-productid="' + productId + '"]');
            if ($options.length) {
                $options
                    .prop("selected", checked)
                    .prop("disabled", !checked);
                _this.$dropdownProductDetail.multiselect("refresh");
                if (checked && _this.$dropdownProductDetail.find("option:enabled").length) {
                    _this.$dropdownProductDetail.multiselect("enable");
                }
                else if (!_this.$dropdownProductDetail.find("option:enabled").length) {
                    _this.$dropdownProductDetail.multiselect("disable");
                }
                _this.onChange();
            }
        };
        this.shouldEnableDropdown = function (selected, productId) {
            var filterOptions = _this.$dropdownProductDetail.find('option[data-productid="' + productId + '"]');
            if (filterOptions.length) {
                if (selected) {
                    filterOptions.prop("disabled", false);
                    _this.$dropdownProductDetail.multiselect("enable");
                }
                else {
                    filterOptions.prop("selected", false);
                }
                _this.$dropdownProductDetail.multiselect("refresh");
            }
        };
        this.validate = function () {
            if (_this.options.validateBeforeSubmit
                && _this.$multiSelectButton
                && _this.isAnyEnabledSelected()
                && _this.$multiSelectButton.hasClass("error")) {
                _this.$multiSelectButton.removeClass("error");
                _this.$dropdownProductDetail.removeClass("error");
            }
        };
        this.addError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.$dropdownProductDetail.addClass("error");
        };
        this.isAnyEnabledSelected = function () {
            if (!_this.$multiSelectButton.length || _this.$multiSelectButton.is(":disabled")) {
                return true;
            }
            return _this.$dropdownProductDetail.find("option:selected").length > 0;
        };
        this.$ = $;
    }
    return ProductDetailMultiSelectionComponent;
}());
//# sourceMappingURL=productdetail-multiselection.js.map
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
var BaseReportNew = /** @class */ (function () {
    function BaseReportNew($, linkBuilderComponent) {
        var _this = this;
        this.isClickedCollapseExpandAll = false;
        this.initHelp = function () {
            _this.$loadLegendURL = _this.$("#LoadLegendURL");
            _this.$iconInformation = _this.$(".icon-information");
            _this.$iconInformationContainer = _this.$(".icon-information-container");
            _this.loadLegend();
            _this.$iconInformation.on("click", _this.showLegend);
        };
        this.loadLegend = function () {
            _this.$.ajax({
                method: "GET",
                url: _this.$loadLegendURL.val(),
                cache: false,
                data: {},
                success: _this.renderLegend,
                complete: _this.$.unblockUI
            });
        };
        this.renderLegend = function (html) {
            if (_this.$reportLegend) {
                _this.$reportLegend.remove();
            }
            _this.$("body").append(html);
            if (_this.$.trim(html) === "") {
                _this.$iconInformationContainer.remove();
            }
            else {
                _this.$iconInformationContainer.removeClass("hide");
            }
            _this.$reportLegend = _this.$("#report-legend");
        };
        this.showLegend = function () {
            _this.$reportLegend.modal({ show: true, backdrop: false });
            var iconHelpPosition = _this.$iconInformation.position();
            _this.$reportLegend.css({
                bottom: "auto",
                top: _this.$(".entered-non-entered").length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
            });
        };
        this.isDescendingSort = function () { return _this.$dataTable.order()[0][1] !== 'asc'; };
        this.doAdjustHeader = function () {
            _this.$dataTable.fixedHeader.adjust();
            var scrollTo = _this.$('.fixedHeader-floating thead').length > 0 ? _this.$(document).height() : 0;
            _this.$(window).scrollTop(scrollTo);
        };
        this.onClickFooterRow = function (parentRow, childRow) {
            parentRow.hasClass('parent') ?
                childRow.addClass('tbl-footer') :
                childRow.removeClass('tbl-footer');
            if (_this.isClickedCollapseExpandAll) {
                _this.isClickedCollapseExpandAll = false;
                return;
            }
            setTimeout(function () {
                var scrollTo = _this.$(document).height();
                _this.$(window).scrollTop(scrollTo + 15);
            }, 100);
        };
        this.onClickBodyRow = function (parentRow, childRow) {
            if (childRow.hasClass('child')) {
                parentRow.hasClass('even') ?
                    childRow.removeClass('odd').addClass('even') :
                    childRow.removeClass('even').addClass('odd');
            }
        };
        this.showFilter = function () {
            _this.$pageTitle.removeClass("hide-filter");
            _this.$reportForm.removeClass("hide");
        };
        this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        this.replaceHrefParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        this.collapseExpandAll = function (element) {
            _this.$.blockUI();
            _this.isClickedCollapseExpandAll = true;
            setTimeout(function () {
                var targetParent = _this.$(element).parent();
                if (targetParent.hasClass("parent")) {
                    targetParent.removeClass("parent");
                    _this.triggerClickDetailWhenSwitchAll(_this.$('tbody tr.parent'));
                }
                else {
                    targetParent.addClass("parent");
                    _this.triggerClickDetailWhenSwitchAll(_this.$('tbody tr').not(".parent"));
                }
                _this.$.unblockUI();
            }, 100);
        };
        this.triggerClickDetailWhenSwitchAll = function (controlArray) {
            controlArray.each(function (index, detailButton) { return _this.$(detailButton).find('.control').click(); });
            _this.$dataTable.fixedHeader.adjust();
        };
        this.getTableOptions = function () {
            var self = _this;
            return {
                language: {
                    processing: '',
                    emptyTable: _this.$('#NoInformationIsAvailable').val()
                },
                responsive: {
                    details: {
                        type: 'column',
                        target: -1,
                        renderer: function (api, rowIdx) {
                            var data = api.cells(rowIdx, ':hidden').eq(0).map(function (cell) {
                                var header = self.$(api.column(cell.column).header());
                                var idx = api.cell(cell).index();
                                if (header.hasClass('control') || header.hasClass('never')) {
                                    return '';
                                }
                                var dtPrivate = api.settings()[0];
                                var cellData = dtPrivate.oApi._fnGetCellData(dtPrivate, idx.row, idx.column, 'display');
                                return self.renderData(idx.column, header.text(), cellData);
                            }).toArray().join('');
                            return self.renderDetail(rowIdx, data);
                        }
                    }
                },
                columnDefs: [{
                        className: 'control',
                        orderable: false,
                        targets: -1
                    }],
                ordering: _this.canSort,
                paging: false,
                searching: false,
                bInfo: false,
                fixedHeader: true,
                processing: true,
                drawCallback: _this.drawCallback
            };
        };
        this.renderData = function (idxColumn, headerText, cellData) { return "<li data-dtr-index=\"" + idxColumn + "\">" +
            ("<span class=\"dtr-title\">" + headerText + "</span> ") +
            ("<span class=\"dtr-data\">" + cellData + "</span></li>"); };
        this.renderDetail = function (rowIdx, data) { return data ?
            _this.$("<ul data-dtr-index=\"" + rowIdx + "\"/>").append(data) :
            false; };
        this.drawCallback = function () {
            _this.$('tbody').append(_this.$('tr.tbl-footer'));
            _this.$tableReport.removeClass('hidden');
        };
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }
    BaseReportNew.prototype.init = function () {
        this.cacheDOMElements();
    };
    BaseReportNew.prototype.cacheDOMElements = function () {
        this.$filterIcon = this.$(".icon-filter");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$pageTitle = this.$(".page-title");
        this.$reportForm = this.$("#report-form");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$submitButton = this.$(".btn-submit");
        this.$tableReport = this.$("#tbl-report");
        this.canSort = parseInt(this.$tableReport.data('totalrecords'), 0) > 1;
    };
    BaseReportNew.prototype.registerEvents = function () {
        var _this = this;
        this.$filterIcon.on("click", function () { return _this.onClickIconFilter(); });
    };
    BaseReportNew.prototype.triggerRedrawTable = function () {
        var _this = this;
        this.doAdjustHeader();
        this.$('tbody tr.parent').each(function (index, element) { return _this.onClickDetailControl(element); });
    };
    BaseReportNew.prototype.onClickDetailControl = function (element) {
        var parentRow = this.$(element).closest('tr');
        var childRow = this.$(parentRow).next();
        parentRow.hasClass('tbl-footer') ?
            this.onClickFooterRow(parentRow, childRow) :
            this.onClickBodyRow(parentRow, childRow);
    };
    ;
    BaseReportNew.prototype.onClickIconFilter = function () {
        this.currentHideState = this.$reportForm.hasClass("hide");
        this.currentHideState ? this.showFilter() : this.hideFilter();
        this.doAdjustHeader();
    };
    BaseReportNew.prototype.initDataTable = function (options) {
        var _this = this;
        if (options === void 0) { options = {}; }
        var tableOptions = this.getTableOptions();
        this.$.extend(tableOptions, options);
        this.$dataTable = this.$tableReport.DataTable(tableOptions);
        this.$tableReport.find('tbody').on('click', 'td.control', function (event) { return _this.onClickDetailControl(event.currentTarget); });
        this.$tableReport.find('thead').on('click', 'th.control', function (event) { return _this.collapseExpandAll(event.currentTarget); });
        this.$dataTable.on('draw.dt', function () { return _this.triggerRedrawTable(); });
    };
    return BaseReportNew;
}());
//# sourceMappingURL=base-report-new.js.map
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
var BaseHistoricReport = /** @class */ (function (_super) {
    __extends(BaseHistoricReport, _super);
    function BaseHistoricReport($, dateRangePickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.doAdjustDatePickerCalendar = function () {
            if (alphaReports.screenSmallThan768() && !alphaReports.screenLandscapeMode()) {
                var width = _this.$(window).width(), widget = _this.$(".bootstrap-datetimepicker-widget.dropdown-menu");
                widget.css({ width: width });
            }
        };
        _this.checkReady = function () { return _this.$("#IsHistoricReport").length > 0; };
        _this.dateRangePickerComponent = dateRangePickerComponent;
        return _this;
    }
    BaseHistoricReport.prototype.cacheDOMElements = function () {
        _super.prototype.cacheDOMElements.call(this);
        this.$excelIcon = this.$(".icon-excel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$fromDate = this.$("#from-date");
        this.$historicReport = this.$("#historic-report");
        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.$toDate = this.$("#to-date");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
    };
    BaseHistoricReport.prototype.initDataTable = function (options) {
        if (options === void 0) { options = {}; }
        _super.prototype.initDataTable.call(this, options);
        this.doAdjustDataTable();
    };
    BaseHistoricReport.prototype.doAdjustDataTable = function () {
        var browserWidth = this.$(window).width(), deviceWidth = screen.width || window.outerWidth, columnCount = this.$dataTable.columns()[0].length;
        if (deviceWidth < 768 && browserWidth >= 768) {
            for (var i = 3; i < columnCount - 1; i++) {
                this.$(this.$dataTable.column(i).header()).removeClass("desktop min-tablet-l mobile").addClass("mobile");
            }
        }
        if (this.$tableReport.width() > browserWidth) {
            this.$tableReport.find("tbody tr td:first-child").css("white-space", "nowrap");
            this.$(this.$dataTable.column(2).header()).removeClass("all").addClass("min-tablet-l");
        }
        this.$dataTable.responsive.rebuild();
        this.$dataTable.responsive.recalc();
    };
    BaseHistoricReport.prototype.registerEvents = function () {
        var _this = this;
        _super.prototype.registerEvents.call(this);
        this.$excelIcon.on("click", function () { return _this.onExportExcelEvent(); });
        this.$historicReport.on("click", function () { return _this.onSubmitHistoricReportEvent(); });
        this.$reportForm.submit(function (event) { return _this.onSubmitReportEvent(event); });
        this.$fromDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
        this.$toDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
    };
    BaseHistoricReport.prototype.onExportExcelEvent = function () {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    BaseHistoricReport.prototype.onSubmitHistoricReportEvent = function () { };
    BaseHistoricReport.prototype.onSubmitReportEvent = function (event) { };
    return BaseHistoricReport;
}(BaseReportNew));
//# sourceMappingURL=base-historic-report.js.map
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
var WinLossViewNew = /** @class */ (function (_super) {
    __extends(WinLossViewNew, _super);
    function WinLossViewNew($, dateRangePickerComponent, productMultiSelectionComponent, reportTooltip, monthPickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, dateRangePickerComponent, linkBuilderComponent) || this;
        _this.isHistoricReport = false;
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _super.prototype.init.call(_this);
            _this.custLevelId = _this.$custLevelId.val();
            _this.linkBuilderComponent.render();
            _this.bindFilter();
            _this.initDataTable();
            _this.initHelp();
            _this.registerEvents();
            _this.$.unblockUI();
        };
        _this.initDataTable = function () {
            var columnSortIndex = _this.getColumnSortIndex();
            var columnSortDirection = JSON.parse(_this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc';
            _super.prototype.initDataTable.call(_this, {
                order: [[columnSortIndex, columnSortDirection]]
            });
        };
        _this.renderData = function (idxColumn, headerText, cellData) {
            var transformedHeaderText = _this.getColumnNameByCustLevel(idxColumn, headerText);
            return "<li data-dtr-index=\"" + idxColumn + "\">" +
                ("<span class=\"dtr-title\">" + transformedHeaderText + "</span> ") +
                ("<span class=\"dtr-data\">" + cellData + "</span></li>");
        };
        _this.triggerRedrawTable = function () {
            _super.prototype.triggerRedrawTable.call(_this);
            _this.$sortPropertyName.val(_this.getColumnSortName());
            _this.$isDescendingSort.val(String(_this.isDescendingSort()));
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheDOMElements.call(_this);
            _this.$custLevelId = _this.$('#CustLevelId');
            _this.$drilldownLinks = _this.$('.downline-link');
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$('#dropdown-product').parent().find('.multiselect.btn');
            if (alphaReports.screenSmallThan768()) {
                _this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', parseInt(_this.$('#CustId').val(), 0)));
            }
            _this.isHistoricReport = _this.$isHistoricReport.val().toLocaleLowerCase() === 'true';
            if (_this.isHistoricReport) {
                _this.$reportForm.removeClass('hide');
                _this.monthPickerComponent.render();
                if (_this.$isFilterVisible.val().toLowerCase() !== 'true') {
                    _this.$reportForm.addClass('hide');
                }
            }
            else {
                _this.dateRangePickerComponent.render();
            }
            if (_this.$isFilterVisible.val().toString().toLowerCase() === 'false') {
                _this.hideFilter();
            }
        };
        _this.onClickIconFilter = function () {
            _super.prototype.onClickIconFilter.call(_this);
            _this.replaceHrefParam("IsFilterVisible", _this.currentHideState);
        };
        _this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val('');
            _this.$toDate.val('');
            _this.isHistoricReport = !_this.$historicReport.data('is-historic-report');
            _this.$reportForm.trigger('submit');
        };
        _this.getColumnSortName = function () { return (columnSortName[_this.custLevelId] || [])[_this.$dataTable.order()[0][0]] || _this.$tableReport.data('sortedcolumn'); };
        _this.getColumnSortIndex = function () {
            var columnSortIndex = (columnSortName[_this.custLevelId] || []).indexOf(_this.$sortPropertyName.val());
            return columnSortIndex > -1 ? columnSortIndex : 0;
        };
        _this.getColumnNameByCustLevel = function (columnIndex, defaultColumnName) { return (columnName[_this.custLevelId] || [])[columnIndex] || defaultColumnName; };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                CustId: _this.$tableReport.data('custid'),
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                IsStackMode: false,
                IsHistoricReport: _this.isHistoricReport,
                SortPropertyName: _this.getColumnSortName(),
                IsDescendingSort: _this.isDescendingSort(),
                HideFilter: _this.$reportForm.is(':hidden')
            };
        };
        _this.showValidateProductError = function () {
            _this.$multiSelectButton.addClass('error');
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels('error');
        };
        _this.showValidaProductDetailError = function () {
            _this.productMultiSelectionComponent.addErrorToProductDetailComponent();
        };
        _this.clearError = function () {
            _this.$multiSelectButton.removeClass('error');
        };
        _this.onSubmitReportEvent = function (event) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                _this.$.unblockUI();
                event.preventDefault();
                return;
            }
            if (!params.IsAnyProductDetailSelected) {
                _this.showValidaProductDetailError();
                _this.$.unblockUI();
                event.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$sortPropertyName.val(params.SortPropertyName);
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
        };
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        _this.monthPickerComponent = monthPickerComponent;
        return _this;
    }
    return WinLossViewNew;
}(BaseHistoricReport));
$(function () {
    var winLossViewNew = new WinLossViewNew($, new DateRangePickerComponent($), new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)), new ReportTooltip($), new MonthPickerComponent($), new LinkBuilderComponent($));
    winLossViewNew.init();
});
//# sourceMappingURL=winloss-new.js.map