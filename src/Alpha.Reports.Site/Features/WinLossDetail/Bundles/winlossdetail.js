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
/// <reference path="../../../assets/scripts/core.ts" />
/// <reference path="../../../assets/scripts/base-report.ts" />
/// <reference path="../../../assets/scripts/components/daterangepicker.ts" />
/// <reference path="../../../assets/scripts/components/monthpicker.ts" />
/// <reference path="../../../assets/scripts/components/pager.ts" />
/// <reference path="../../../assets/scripts/components/productmultiselection.ts" />
/// <reference path="../../../assets/scripts/components/productdetail-multiselection.ts" />
/// <reference path="../../../assets/scripts/components/sort.ts" />
/// <reference path="../../../assets/scripts/components/link-builder.ts" />
/// <reference path="../../../assets/scripts/components/tooltip.ts" />
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
var WinLossDetailView = /** @class */ (function (_super) {
    __extends(WinLossDetailView, _super);
    function WinLossDetailView($, dateRangePickerComponent, pageComponent, productMultiSelectionComponent, sortComponent, reportTooltip, monthPickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.isFirstSwitchMode = true;
        _this.isHistoricReport = false;
        _this.isSwitchView = false;
        _this.hasAllDataOnClient = false;
        _this.levels = ["member", "agent", "master", "super"];
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _this.$.blockUI();
            _this.cacheElements();
            _this.$isHistoricReport = _this.$("#IsHistoricReport");
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
            _this.productMultiSelectionComponent.render();
            _this.$multiSelectButton = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.pagerComponent.render();
            _this.hasAllDataOnClient = _this.pagerComponent.hasAllDataOnClient();
            var canSort = parseInt(_this.$tableReport.data("totalrecords"), 0) > 1;
            _this.sortComponent.render(new SortOption(_this.$tableReport, canSort, _this.hasAllDataOnClient, _this.sortComponent.afterSort, { 0: { sorter: "text" } }, _this.$reportForm));
            _this.$tableReport.stickyTableHeaders();
            if (_this.$isSetDefaultStackMode.val().toString().toLowerCase() === "true"
                && alphaReports.screenSmallThan768()) {
                _this.setStackMode();
                _this.reportTooltip.show(new ReportTooltipOption("icon-normal-column", parseInt(_this.$("#CustId").val(), 0)));
            }
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
            _this.$.unblockUI();
        };
        _this.checkReady = function () {
            return _this.$("#IsHistoricReport").length > 0;
        };
        _this.onSwitchReport = function () {
            _this.isSwitchView = true;
            _this.isHistoricReport = !_this.$reportSwitch.data("is-historic-report");
            _this.$reportForm.trigger("submit");
        };
        _this.onClickIconFilter = function () {
            var filter = _this.$reportForm;
            var currentHideState = filter.hasClass("hide");
            if (currentHideState) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.hideFilter();
            }
            _this.replaceHrefParam("IsFilterVisible", currentHideState);
        };
        _this.setStackMode = function () {
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            _this.$pageTitle.addClass("stack-mode");
            tableReport.addClass("stack-mode");
            _this.replaceHrefParam("IsStackMode", true);
            var this$ = _this.$;
            alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 3, "col-turnover-combined");
            var levels = _this.levels;
            this$.each(levels, function (index) {
                var level = levels[index];
                var cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
            });
            _this.isFirstSwitchMode = false;
            _this.sortComponent.bindTableSorter(_this.getSortedColumnName());
            _this.$.unblockUI();
        };
        _this.onSwitchMode = function () {
            _this.$.blockUI();
            var tableReport = _this.$tableReport;
            var currentStackModeState = tableReport.hasClass("stack-mode");
            if (currentStackModeState) {
                _this.$pageTitle.removeClass("stack-mode");
                tableReport.removeClass("stack-mode");
            }
            else {
                _this.$pageTitle.addClass("stack-mode");
                tableReport.addClass("stack-mode");
            }
            _this.replaceHrefParam("IsStackMode", !currentStackModeState);
            var levels = _this.levels;
            var this$ = _this.$;
            if (_this.isFirstSwitchMode) {
                if (currentStackModeState) {
                    alphaReports.splitCell("col-turnover");
                    this$.each(levels, function (index) {
                        var level = levels[index];
                        alphaReports.splitCell("col-" + level + "-winloss", index % 2 ? " altercol" : "");
                    });
                }
                else {
                    alphaReports.combineConsecutiveCells(this$("td.col-turnover"), 3, "col-turnover-combined");
                    this$.each(levels, function (index) {
                        var level = levels[index];
                        var cellCss = "col-" + level + "-winloss-combined" + (index % 2 ? " altercol" : "");
                        alphaReports.combineConsecutiveCells(this$("td.col-" + level + "-winloss"), 3, cellCss);
                    });
                }
                _this.isFirstSwitchMode = false;
            }
            _this.sortComponent.bindTableSorter(_this.getSortedColumnName());
            _this.$.unblockUI();
        };
        _this.onExportExcel = function () {
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        _this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            var sortInfo = _this.sortComponent.getSortInfo();
            var tableReport = _this.$tableReport;
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsAnyProductDetailSelected: _this.productMultiSelectionComponent.isAnyEnabledProductDetailSelected(),
                SortPropertyName: sortInfo.sortPropertyName,
                IsDescendingSort: sortInfo.isDescendingSort,
                IsStackMode: tableReport.hasClass("stack-mode"),
                IsHistoricReport: _this.isHistoricReport,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.showValidateProductError = function () {
            _this.$multiSelectButton.addClass("error");
            _this.productMultiSelectionComponent.addClassToExcludeProductLabels("error");
        };
        _this.showValidaProductDetailError = function () {
            _this.productMultiSelectionComponent.addErrorToProductDetailComponent();
        };
        _this.clearError = function () {
            _this.$multiSelectButton.removeClass("error");
        };
        _this.replaceHrefParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        _this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            var params = _this.getParams();
            if (_this.isSwitchView) {
                _this.$fromDate.val("");
                _this.$toDate.val("");
            }
            else if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            else if (!params.IsAnyProductDetailSelected) {
                _this.showValidaProductDetailError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
            _this.$isDescendingSort.val(String(params.IsDescendingSort));
            _this.$isHistoricReport.val(String(params.IsHistoricReport));
            _this.$isStackMode.val(String(params.IsStackMode));
            _this.$sortPropertyName.val(params.SortPropertyName);
        };
        _this.getSortedColumnName = function () {
            var sortedColumn = _this.$tableReport.data("sortedcolumn");
            if (!_this.$tableReport.hasClass("stack-mode")) {
                return sortedColumn;
            }
            if (sortedColumn === "NetTurnover" || sortedColumn === "GrossComm") {
                return "Turnover";
            }
            if (sortedColumn === "MemberNetWinLoss.Comm" || sortedColumn === "MemberNetWinLoss.Total") {
                return "MemberNetWinLoss.WinLoss";
            }
            if (sortedColumn === "AgentNetWinLoss.Comm" || sortedColumn === "AgentNetWinLoss.Total") {
                return "AgentNetWinLoss.WinLoss";
            }
            if (sortedColumn === "MasterNetWinLoss.Comm" || sortedColumn === "MasterNetWinLoss.Total") {
                return "MasterNetWinLoss.WinLoss";
            }
            if (sortedColumn === "SuperNetWinLoss.Comm" || sortedColumn === "SuperNetWinLoss.Total") {
                return "SuperNetWinLoss.WinLoss";
            }
            return sortedColumn;
        };
        _this.$ = $;
        _this.dateRangePickerComponent = dateRangePickerComponent;
        _this.pagerComponent = pageComponent;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.sortComponent = sortComponent;
        _this.reportTooltip = reportTooltip;
        _this.monthPickerComponent = monthPickerComponent;
        _this.linkBuilderComponent = linkBuilderComponent;
        return _this;
    }
    WinLossDetailView.prototype.cacheElements = function () {
        _super.prototype.cacheElements.call(this);
        this.$drilldownLinks = this.$(".downline-link");
        this.$isStackMode = this.$("#IsStackMode");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
        this.$reportSwitch = this.$("#historic-report");
        this.$fromDate = this.$("#from-date");
        this.$toDate = this.$("#to-date");
        this.$iconNormalColumn = this.$("#icon-normal-column");
        this.$isSetDefaultStackMode = this.$("#IsSetDefaultStackMode");
        this.$breadcrumbLinks = this.$(".breadcrumb-link");
    };
    WinLossDetailView.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$reportSwitch.on("click", this.onSwitchReport);
        this.$(".icon-switchmode").on("click", this.onSwitchMode);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#icon-excel").on("click", this.onExportExcel);
    };
    return WinLossDetailView;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var winLossDetailView = new WinLossDetailView($, new DateRangePickerComponent($), new PagerComponent($), new ProductMultiSelectionComponent($, new ProductDetailMultiSelectionComponent($)), new SortComponent($, linkBuilderComponent), new ReportTooltip($), new MonthPickerComponent($), linkBuilderComponent);
    winLossDetailView.init();
});
//# sourceMappingURL=winlossdetail.js.map