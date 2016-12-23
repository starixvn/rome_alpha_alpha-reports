/// <reference path="../core.ts" />
"use strict";
var ProductMultiSelectionOption = (function () {
    function ProductMultiSelectionOption(dropdownId, validateBeforeSubmit, productsInputId) {
        if (dropdownId === void 0) { dropdownId = "dropdown-product"; }
        if (validateBeforeSubmit === void 0) { validateBeforeSubmit = true; }
        if (productsInputId === void 0) { productsInputId = "product-ids"; }
        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
        this.productsInputId = productsInputId;
    }
    return ProductMultiSelectionOption;
}());
var ProductMultiSelectionComponent = (function () {
    function ProductMultiSelectionComponent($) {
        var _this = this;
        this.options = new ProductMultiSelectionOption();
        this.render = function (options) {
            if (options === void 0) { options = new ProductMultiSelectionOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$dropdownProduct = _this.$("#" + _this.options.dropdownId);
            var hasAnyProduct = _this.$dropdownProduct.data("has-any-product");
            _this.$hiddenInput = _this.$("#" + _this.options.productsInputId);
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
                language: _this.$dropdownProduct.data("language")
            });
            _this.$multiSelectButton = _this.$dropdownProduct.parent().find(".multiselect.btn");
            return _this.$dropdownProduct;
        };
        this.onChange = function (option, checked) {
            _this.validate();
            _this.$hiddenInput.val(_this.getSelectedItems().join(","));
        };
        this.validate = function () {
            if (_this.options.validateBeforeSubmit
                && _this.$multiSelectButton
                && _this.$dropdownProduct.find("option:selected").length
                && _this.$multiSelectButton.hasClass("error")) {
                _this.$multiSelectButton.removeClass("error");
            }
        };
        this.getSelectedItems = function () {
            var selected = [];
            var selectedOptions = _this.$dropdownProduct.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected;
        };
        this.$ = $;
    }
    return ProductMultiSelectionComponent;
}());
//# sourceMappingURL=productmultiselection.js.map