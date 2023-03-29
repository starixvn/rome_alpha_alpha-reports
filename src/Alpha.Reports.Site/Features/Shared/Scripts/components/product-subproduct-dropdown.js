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