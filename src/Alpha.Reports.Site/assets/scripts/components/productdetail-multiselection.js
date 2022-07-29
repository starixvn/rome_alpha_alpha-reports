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