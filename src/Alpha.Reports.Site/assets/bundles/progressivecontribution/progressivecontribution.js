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
/// <reference path="core.ts" />
var BaseReport = /** @class */ (function () {
    function BaseReport($, linkBuilderComponent) {
        var _this = this;
        this.onClickIconFilter = function () {
            var filter = _this.$reportForm;
            var currentHideState = filter.hasClass("hide");
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$reportForm.addClass("hide");
            }
            _this.replaceHrefParam("IsFilterVisible", currentHideState);
        };
        this.replaceHrefParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        this.initHelp = function () {
            _this.$loadLegendURL = _this.$("#LoadLegendURL");
            _this.$iconInformation = _this.$("#icon-information");
            _this.$iconInformationContainer = _this.$("#icon-information-container");
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
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }
    BaseReport.prototype.cacheElements = function () {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$tableReport = this.$("#tbl-report");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
    };
    BaseReport.prototype.registerEvents = function () {
        this.$(".icon-filter").on("click", this.onClickIconFilter);
    };
    return BaseReport;
}());
//# sourceMappingURL=base-report.js.map
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
/// <reference path="../components/monthpicker.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/tooltip.ts" />
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
var ProgressiveContributionView = /** @class */ (function (_super) {
    __extends(ProgressiveContributionView, _super);
    function ProgressiveContributionView($, dateRangePickerComponent, monthPickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isHistoricReport = false;
        _this.isFirstSwitchMode = true;
        _this.levels = ["member", "agent", "master", "super"];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.cacheDOMElements();
            _this.bindFilter();
            _this.$tableReport.stickyTableHeaders();
            _this.registerEvents();
            _this.initHelp();
        };
        _this.checkReady = function () {
            return _this.$("#dropdown-product").length > 0;
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render(undefined);
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$custId.val(), 0)));
            }
            _this.isHistoricReport = _this.$isHistoricReport.val().toLocaleLowerCase() === "true";
            if (_this.isHistoricReport) {
                _this.$reportForm.removeClass("hide");
                _this.monthPickerComponent.render();
                if (_this.$isFilterVisible.val().toLowerCase() !== "true") {
                    _this.$reportForm.addClass("hide");
                }
            }
            else {
                _this.dateRangePickerComponent.render();
            }
            if (_this.$isFilterVisible.val().toString().toLowerCase() === "false") {
                _this.hideFilter();
            }
        };
        _this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        _this.cacheDOMElements = function () {
            _this.cacheElements();
            _this.$isSetDefaultStackMode = _this.$("#IsSetDefaultStackMode");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
            _this.$isStackMode = _this.$("#IsStackMode");
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
            _this.$isAgentLevel = _this.$("#IsAgentLevel");
            _this.$custId = _this.$("#CustId");
            _this.$custLevelId = _this.$("#CustLevelId");
            _this.$historicReport = _this.$("#historic-report");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$switchModeIcon = _this.$(".icon-switchmode");
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
        };
        _this.onSubmitReport = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.$buttonProduct.addClass("error");
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$isStackMode.val(String(params.IsStackMode));
        };
        _this.onSubmitHistoricReport = function () {
            _this.$fromDate.val("");
            _this.$toDate.val("");
            _this.isHistoricReport = !_this.$historicReport.data("is-historic-report");
            _this.$reportForm.trigger("submit");
        };
        _this.onSwitchMode = function () {
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            var isStackMode = tableReport.hasClass("stack-mode");
            _this.setCssBySwitchMode(isStackMode);
            if (_this.isFirstSwitchMode) {
                _this.renderAtFirstSwitchMode(isStackMode);
                _this.isFirstSwitchMode = false;
            }
            _this.$.unblockUI();
        };
        _this.setStackMode = function () {
            var isAgentLevel = _this.$isAgentLevel.val().toLocaleLowerCase() === "true";
            if (isAgentLevel) {
                return;
            }
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            _this.setCssBySwitchMode(false);
            _this.renderAtFirstSwitchMode(false);
            _this.isFirstSwitchMode = false;
            _this.$.unblockUI();
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var tableReport = _this.$tableReport;
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsHistoricReport: _this.isHistoricReport,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.setCssBySwitchMode = function (isSwitchToNormalMode) {
            if (isSwitchToNormalMode) {
                _this.$pageTitle.removeClass("stack-mode");
                _this.$tableReport.removeClass("stack-mode");
            }
            else {
                _this.$pageTitle.addClass("stack-mode");
                _this.$tableReport.addClass("stack-mode");
            }
        };
        _this.renderAtFirstSwitchMode = function (isSwitchToNormalMode) {
            var custLevelId = parseInt(_this.$custLevelId.val());
            var level = _this.levels[custLevelId - 1];
            if (isSwitchToNormalMode) {
                alphaReports.splitCell("col-" + level + "-contribution-share", "", "");
            }
            else {
                alphaReports.combineConsecutiveCells(_this.$("td.col-" + level + "-contribution-share"), custLevelId - 1, "text-right col-" + level + "-contribution-share-combined");
            }
        };
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.monthPickerComponent = monthPickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        return _this;
    }
    ProgressiveContributionView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$excelIcon.on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$historicReport.on("click", this.onSubmitHistoricReport);
        this.$switchModeIcon.on("click", this.onSwitchMode);
    };
    ;
    return ProgressiveContributionView;
}(BaseReport));
$(function () {
    var progressiveContributionView = new ProgressiveContributionView($, new DateRangePickerComponent($), new MonthPickerComponent($), new ProductMultiSelectionComponent($), new ReportTooltip($), new LinkBuilderComponent($));
    progressiveContributionView.init();
});
//# sourceMappingURL=progressivecontribution.js.map