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
        this.onlyShowHistoricProductIds = [];
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
            item.OnlyShowHistoric && _this.onlyShowHistoricProductIds.push(item.Id);
            var checkboxItem = _this.buildCheckboxItem(item);
            if (item.Products) {
                var subProductIds_1 = [];
                productIdWithSubProductIds[item.Id] = subProductIds_1;
                checkboxItem.items = item.Products.map(function (item) {
                    item.OnlyShowHistoric && _this.onlyShowHistoricProductIds.push(item.Id);
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
    ProductAndProductDetailDropdownComponent.prototype.showHistoricProducts = function () {
        this.onlyShowHistoricProductIds.length
            && this.dropdownProduct.fanexMultiSelect("showItemsByValues", this.onlyShowHistoricProductIds);
    };
    ProductAndProductDetailDropdownComponent.prototype.hideHistoricProducts = function () {
        this.onlyShowHistoricProductIds.length
            && this.dropdownProduct.fanexMultiSelect("hideItemsByValues", this.onlyShowHistoricProductIds);
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map