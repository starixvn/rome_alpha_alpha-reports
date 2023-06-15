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
    ProductAndProductDetailDropdownComponent.prototype.render = function (productAndProductDetailDropdownOption) {
        this.renderProduct(productAndProductDetailDropdownOption);
        this.renderProductDetail(productAndProductDetailDropdownOption);
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProduct = function (productAndProductDetailDropdownOption) {
        var _this = this;
        this.$dropdownProduct = this.$("#" + productAndProductDetailDropdownOption.productSelectorId);
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
            onDropdownHidden: function () {
                productAndProductDetailDropdownOption.onDropdownHidden(_this.getProductSelectedItems(), _this.getProductDetailSelectedItems());
            }
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
    ProductAndProductDetailDropdownComponent.prototype.renderProductDetail = function (productAndProductDetailDropdownOption) {
        var _this = this;
        this.$dropdownProductDetail = this.$("#" + productAndProductDetailDropdownOption.productDetailSelectorId);
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
            onDropdownHidden: function () {
                productAndProductDetailDropdownOption.onDropdownHidden(_this.getProductSelectedItems(), _this.getProductDetailSelectedItems());
            }
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