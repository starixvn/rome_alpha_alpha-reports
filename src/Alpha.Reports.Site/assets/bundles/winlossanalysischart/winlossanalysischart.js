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
/// <reference path="../core.ts" />
"use strict";
var MonthRangePickerOption = /** @class */ (function () {
    function MonthRangePickerOption(fromSelectorId, toSelectorId, reportFormId) {
        if (fromSelectorId === void 0) { fromSelectorId = "from-monthpicker"; }
        if (toSelectorId === void 0) { toSelectorId = "to-monthpicker"; }
        if (reportFormId === void 0) { reportFormId = "report-form"; }
        this.fromSelectorId = fromSelectorId;
        this.toSelectorId = toSelectorId;
        this.reportFormId = reportFormId;
    }
    return MonthRangePickerOption;
}());
var MonthRangePickerComponent = /** @class */ (function () {
    function MonthRangePickerComponent($) {
        var _this = this;
        this.options = new MonthRangePickerOption();
        this.render = function (options) {
            if (options === void 0) { options = new MonthRangePickerOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$dropdownFromMonth = _this.renderDropdown(_this.options.fromSelectorId);
            _this.$dropdownToMonth = _this.renderDropdown(_this.options.toSelectorId);
        };
        this.renderDropdown = function (dropdownId) {
            var dropdownMonth = _this.$("#" + dropdownId);
            var maxDate = dropdownMonth.data("maxdate");
            dropdownMonth.monPickr({
                longMonthsInYear: dropdownMonth.data("longmonths-inyear").split(","),
                maxDate: maxDate,
                minDate: dropdownMonth.data("mindate"),
                month: new Date(dropdownMonth.data("dateinmonth")),
                today: new Date(dropdownMonth.data("currentdate")),
                showDateRange: false,
                monthFormat: alphaReports.format.monthFormat,
                fromDayId: "ddl-fromday",
                toDayId: "ddl-today",
                fromDayLabel: "",
                toDayLabel: "-",
                sortDesc: true,
                log: _this.logMessage,
                afterChangeMonth: _this.onChange
            });
            return dropdownMonth;
        };
        this.logMessage = function (msg) {
            console.warn(msg);
        };
        this.onChange = function () {
            _this.$fromDate.val(_this.$dropdownFromMonth.monPickr("getFromDate", alphaReports.format.shortDateWithSlash_MonthPicker));
            _this.$toDate.val(_this.$dropdownToMonth.monPickr("getToDate", alphaReports.format.shortDateWithSlash_MonthPicker));
        };
        this.isLeast3Month = function () {
            var fromDate = new Date(_this.$fromDate.val());
            var toDate = new Date(_this.$toDate.val());
            return _this.getMonthDiff(fromDate, toDate) >= 2;
        };
        this.getMonthDiff = function (fromDate, toDate) {
            return toDate.getMonth() - fromDate.getMonth() + 12 * (toDate.getFullYear() - fromDate.getFullYear());
        };
        this.$ = $;
    }
    return MonthRangePickerComponent;
}());
//# sourceMappingURL=month-range-picker.js.map
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
                top: iconHelpPosition.top + 60
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
var DualAxisChartOption = /** @class */ (function () {
    function DualAxisChartOption(isDarkMode, selectorId) {
        if (isDarkMode === void 0) { isDarkMode = false; }
        if (selectorId === void 0) { selectorId = "winloss-chart"; }
        this.isDarkMode = isDarkMode;
        this.selectorId = selectorId;
    }
    return DualAxisChartOption;
}());
var ChartStyles = /** @class */ (function () {
    function ChartStyles(barColor, textColor, lineColor, fontFamily, defaultFontSize, defaultFontStyle) {
        if (lineColor === void 0) { lineColor = "#fcb441"; }
        if (fontFamily === void 0) { fontFamily = "Trebuchet MS"; }
        if (defaultFontSize === void 0) { defaultFontSize = 12; }
        if (defaultFontStyle === void 0) { defaultFontStyle = "bold"; }
        this.barColor = barColor;
        this.textColor = textColor;
        this.lineColor = lineColor;
        this.fontFamily = fontFamily;
        this.defaultFontSize = defaultFontSize;
        this.defaultFontStyle = defaultFontStyle;
    }
    return ChartStyles;
}());
var DualAxisChartComponent = /** @class */ (function () {
    function DualAxisChartComponent($, chart) {
        var _this = this;
        this.options = new DualAxisChartOption();
        this.render = function (options) {
            if (options === void 0) { options = new DualAxisChartOption; }
            var chartOptions = _this.$.extend({}, _this.options, options);
            var chartStyles = new ChartStyles("#418cf0", "#000"), xAxesGridLines = {}, yAxesGridLines = { drawOnChartArea: false };
            if (chartOptions.isDarkMode) {
                chartStyles = new ChartStyles("#41a4ff", "#f2f2f2");
                xAxesGridLines = { color: "#444", zeroLineColor: "#555" };
                yAxesGridLines = { color: "#444", drawOnChartArea: false };
            }
            _this.chart.defaults.global.defaultFontFamily = chartStyles.fontFamily;
            _this.chart.defaults.global.defaultFontColor = chartStyles.textColor;
            _this.chart.defaults.global.defaultFontSize = chartStyles.defaultFontSize;
            _this.chart.defaults.global.defaultFontStyle = chartStyles.defaultFontStyle;
            var chartContainer = _this.$("#" + chartOptions.selectorId);
            var chartContext = chartContainer[0].getContext("2d");
            var labels = chartContainer.data("labels").split(",");
            _this.currencyName = chartContainer.data("currency-name");
            var chartData = {
                labels: labels,
                datasets: [{
                        label: chartContainer.data("y-axis-1-label"),
                        type: "line",
                        borderColor: chartStyles.lineColor,
                        backgroundColor: chartStyles.lineColor,
                        fill: false,
                        lineTension: 0,
                        data: chartContainer.attr("data-y-axis-1").split(","),
                        yAxisID: "y-axis-1"
                    }, {
                        label: chartContainer.data("y-axis-2-label"),
                        type: "bar",
                        borderColor: chartStyles.barColor,
                        backgroundColor: chartStyles.barColor,
                        fill: false,
                        data: chartContainer.attr("data-y-axis-2").split(","),
                        yAxisID: "y-axis-2"
                    }]
            };
            _this.chart.Bar(chartContext, {
                data: chartData,
                options: {
                    responsive: true,
                    hoverMode: "index",
                    stacked: false,
                    title: {
                        display: true,
                        text: chartContainer.data("title"),
                        fontSize: 18
                    },
                    scales: {
                        xAxes: [{
                                barThickness: 30,
                                ticks: {
                                    autoSkip: false,
                                    maxRotation: 50,
                                    minRotation: 50
                                },
                                gridLines: xAxesGridLines
                            }],
                        yAxes: [{
                                type: "linear",
                                display: true,
                                position: "left",
                                id: "y-axis-1",
                                scaleLabel: {
                                    display: true,
                                    labelString: chartContainer.data("y-axis-1-label")
                                },
                                ticks: {
                                    beginAtZero: true,
                                    callback: alphaReports.formatNumber
                                },
                                gridLines: xAxesGridLines
                            }, {
                                type: "linear",
                                display: true,
                                position: "right",
                                id: "y-axis-2",
                                scaleLabel: {
                                    display: true,
                                    labelString: chartContainer.data("y-axis-2-label")
                                },
                                ticks: {
                                    beginAtZero: true,
                                    callback: alphaReports.formatNumber
                                },
                                gridLines: yAxesGridLines
                            }]
                    },
                    legend: {
                        display: true,
                        position: "bottom"
                    },
                    tooltips: {
                        callbacks: {
                            label: _this.formatTooltipLabel
                        }
                    }
                }
            });
        };
        this.formatTooltipLabel = function (tooltipItem, data) {
            var datesetLabel = data.datasets[tooltipItem.datasetIndex].label;
            return datesetLabel + " = " + alphaReports.formatCurrency(Number(tooltipItem.yLabel)) + " " + _this.currencyName;
        };
        this.$ = $;
        this.chart = chart;
    }
    return DualAxisChartComponent;
}());
//# sourceMappingURL=dual-axis-chart.js.map
/// <reference path="../core.ts" />
/// <reference path="../components/month-range-picker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
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
var WinLossAnalysisChartView = /** @class */ (function (_super) {
    __extends(WinLossAnalysisChartView, _super);
    function WinLossAnalysisChartView($, monthRangePickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent, dualAxesChartComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _this.cacheDOMElements();
            _this.bindFilter();
            _this.dualAxisChartComponent.render(new DualAxisChartOption(_this.isDarkMode));
            _this.registerEvents();
            _this.initHelp();
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheElements.call(_this);
            _this.$canvasChart = _this.$("#chart-winloss-analysis");
            _this.$userSelectedDateRangeOption = _this.$("#UserSelectedDateRangeOption");
            _this.$fromDate = _this.$("#from-date");
            _this.$toDate = _this.$("#to-date");
            _this.$msgError = _this.$("#msg-error");
            _this.isDarkMode = _this.$("#hidden-isDarkMode").val() === "1";
        };
        _this.bindFilter = function () {
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.$reportForm.removeClass("hide");
            _this.monthRangePickerComponent.render();
            if (!alphaReports.convertAttributeToBool(_this.$isFilterVisible.val())) {
                _this.$reportForm.addClass("hide");
            }
        };
        _this.onSubmitReportEvent = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            var isValid = true;
            if (!params.IsAnyProductSelected) {
                _this.showValidationProductError();
                isValid = false;
            }
            if (!params.IsLeast3MonthToChart) {
                _this.showValidationMonthRangeError();
                isValid = false;
            }
            if (!isValid) {
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.hideValidationMonthRangeError();
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? params.HideFilter : true));
        };
        _this.showValidationProductError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidationMonthRangeError = function () { return _this.$msgError.removeClass("hide"); };
        _this.hideValidationMonthRangeError = function () { return _this.$msgError.addClass("hide"); };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var canvasChart = _this.$canvasChart;
            return {
                CustId: canvasChart.data("custid"),
                IsAnyProductSelected: selectedProducts.length > 0,
                IsLeast3MonthToChart: _this.monthRangePickerComponent.isLeast3Month(),
                HideFilter: _this.$reportForm.is(":hidden")
            };
        };
        _this.$ = $;
        _this.monthRangePickerComponent = monthRangePickerComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.reportTooltip = reportTooltip;
        _this.dualAxisChartComponent = dualAxesChartComponent;
        return _this;
    }
    WinLossAnalysisChartView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$reportForm.submit(this.onSubmitReportEvent);
    };
    return WinLossAnalysisChartView;
}(BaseReport));
//# sourceMappingURL=winlossanalysischart.js.map
/// <reference path="../core.ts" />
/// <reference path="../components/month-range-picker.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../components/productmultiselection.ts" />
$(document).ready(function () {
    var winLossAnalysisChartView = new WinLossAnalysisChartView($, new MonthRangePickerComponent($), new ProductMultiSelectionComponent($), new ReportTooltip($), new LinkBuilderComponent($), new DualAxisChartComponent($, Chart));
    winLossAnalysisChartView.init();
});
//# sourceMappingURL=winlossanalysischart-ready.js.map