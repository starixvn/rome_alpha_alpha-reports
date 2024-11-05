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
var BaseReportNew = /** @class */ (function () {
    function BaseReportNew($, linkBuilderComponent) {
        var _this = this;
        this.isClickedCollapseExpandAll = false;
        this.initHelp = function () {
            _this.$loadLegendURL = _this.$("#LoadLegendURL");
            if (_this.$loadLegendURL.length) {
                _this.$iconInformation = _this.$(".icon-information");
                _this.$iconInformationContainer = _this.$(".icon-information-container");
                _this.loadLegend();
                _this.$iconInformation.on("click", _this.showLegend);
            }
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
var SingleSelectionDropdownOption = /** @class */ (function () {
    function SingleSelectionDropdownOption(dropdownId, inputId, onChange) {
        if (dropdownId === void 0) { dropdownId = "dropdown-single"; }
        if (inputId === void 0) { inputId = "UserSelectedId"; }
        if (onChange === void 0) { onChange = null; }
        this.dropdownId = dropdownId;
        this.inputId = inputId;
        this.onChange = onChange;
    }
    return SingleSelectionDropdownOption;
}());
var SingleSelectionDropdownComponent = /** @class */ (function () {
    function SingleSelectionDropdownComponent($) {
        var _this = this;
        this.options = new SingleSelectionDropdownOption();
        this.render = function (options) {
            if (options === void 0) { options = new SingleSelectionDropdownOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var $dropdown = _this.$("#" + _this.options.dropdownId);
            _this.$dropdown = $dropdown;
            var $hiddenInput = _this.$("#" + _this.options.inputId);
            _this.$hiddenInput = $hiddenInput;
            var self = _this;
            _this.selectedValue = self.getSelectedItem();
            _this.$dropdown.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false,
                onChange: _this.onchange
            });
        };
        this.getSelectedItem = function () {
            var selected = [];
            var selectedOptions = _this.$dropdown.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected.length ? selected[0].toString() : "";
        };
        this.updateSelectedValue = function () {
            _this.$hiddenInput.val(_this.selectedValue);
        };
        this.onchange = function () {
            _this.selectedValue = _this.getSelectedItem();
            if (_this.options.onChange) {
                _this.options.onChange();
            }
        };
        this.$ = $;
    }
    return SingleSelectionDropdownComponent;
}());
//# sourceMappingURL=single-selection-dropdown.js.map
/// <reference path="../../../assets/scripts/core.ts" />
/// <reference path="../../shared/scripts/base-report-new.ts" />
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
var OutstandingViewNew = /** @class */ (function (_super) {
    __extends(OutstandingViewNew, _super);
    function OutstandingViewNew($, productMultiSelectionComponent, linkBuilderComponent, displayOutstandingLevels, levelTypeDropdown) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _super.prototype.init.call(_this);
            _this.productMultiSelectionComponent.render();
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.levelTypeDropdown.render(new SingleSelectionDropdownOption("dropdown-level-type"));
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
            _this.$reportForm.show();
            _this.initDataTable();
        };
        _this.registerEvents = function () {
            _super.prototype.registerEvents.call(_this);
            _this.$reportForm.submit(_this.onSubmitReport);
        };
        _this.initDataTable = function () {
            var colunmSortIndex = 0;
            _this.$.each(_this.displayOutstandingLevels, function (key, value) {
                if (_this.$(_this.$sortPropertyName).val().toLowerCase() === (value.DisplayName + "Outstanding").toLowerCase()) {
                    colunmSortIndex = key + 1;
                    return;
                }
            });
            _super.prototype.initDataTable.call(_this, { order: [[colunmSortIndex, JSON.parse(_this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']], });
        };
        _this.triggerRedrawTable = function () {
            _super.prototype.triggerRedrawTable.call(_this);
            _this.replaceHrefParam("SortPropertyName", _this.getColumnSortName());
            _this.replaceHrefParam("IsDescendingSort", _this.isDescendingSort());
        };
        _this.getColumnSortName = function () {
            var order = _this.$dataTable.order();
            var length = _this.displayOutstandingLevels.length;
            for (var index = 0; index < length; index++) {
                if (order[0][0] - 1 === index) {
                    return _this.displayOutstandingLevels[index].DisplayName + "Outstanding";
                }
            }
            return 'CustName';
        };
        _this.onSubmitReport = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            var columnSortName = _this.getColumnSortName();
            var isDescendingSort = _this.isDescendingSort();
            _this.replaceHrefParam("SortPropertyName", columnSortName);
            _this.replaceHrefParam("IsDescendingSort", isDescendingSort);
            _this.$sortPropertyName.val(columnSortName);
            _this.$isDescendingSort.val(isDescendingSort ? 'True' : 'False');
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.showValidateProductError = function () {
            _this.$buttonProduct.addClass("error");
        };
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.displayOutstandingLevels = displayOutstandingLevels;
        _this.levelTypeDropdown = levelTypeDropdown;
        return _this;
    }
    return OutstandingViewNew;
}(BaseReportNew));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var outstandingViewNew = new OutstandingViewNew($, new ProductMultiSelectionComponent($), linkBuilderComponent, displayOutstandingLevels, new SingleSelectionDropdownComponent($));
    outstandingViewNew.init();
});
//# sourceMappingURL=outstanding-new.js.map