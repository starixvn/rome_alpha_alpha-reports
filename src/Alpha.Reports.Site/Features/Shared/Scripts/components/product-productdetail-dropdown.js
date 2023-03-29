"use strict";
var ProductAndProductDetailDropdownOption = /** @class */ (function () {
    function ProductAndProductDetailDropdownOption(products, productDetails, onDropdownHidden, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-product"; }
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
    return ProductAndProductDetailDropdownOption;
}());
var ProductAndProductDetailDropdownComponent = /** @class */ (function () {
    function ProductAndProductDetailDropdownComponent($) {
        this.$ = $;
    }
    ProductAndProductDetailDropdownComponent.prototype.render = function (options) {
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
        var ddlProduct = this.$("#" + options.selectorId);
        this.dropdownProduct = ddlProduct.fanexMultiSelect({
            checkAllText: ddlProduct.data("label-selectall"),
            selectedText: "# " + ddlProduct.data("label-selected"),
            noneSelectedText: ddlProduct.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: ddlProduct.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: ddlProduct.data("label-filterplaceholder"),
            noItemFoundText: ddlProduct.data("label-noitemfound"),
            isCheckAll: true,
            minWidth: 290,
            //maxHeight: 250,
            showTooltip: false,
            listData: productList,
            onLeave: options.onDropdownHidden
        });
        this.dropdownProduct.fanexMultiSelect("toggleCollapseExpandAll");
    };
    ProductAndProductDetailDropdownComponent.prototype.buildProductDetailObject = function (productDetails) {
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
    ProductAndProductDetailDropdownComponent.prototype.buildCheckboxProduct = function (product, productDetailObject, cssClass) {
        if (cssClass === void 0) { cssClass = "product"; }
        var checkboxItem = this.buildCheckboxItem(product, cssClass);
        checkboxItem.items = productDetailObject[product.Id];
        return checkboxItem;
    };
    ProductAndProductDetailDropdownComponent.prototype.buildCheckboxProductDetail = function (product) {
        return this.buildCheckboxItem(product, "productdetail", product.ProductId);
    };
    ProductAndProductDetailDropdownComponent.prototype.buildCheckboxItem = function (item, cssClass, name) {
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
    ProductAndProductDetailDropdownComponent.prototype.getSelectedItems = function () {
        var productIds = this.getSelectedProductIds();
        var result = this.getSelectedResourceIds();
        return {
            productIds: this.uniqe(productIds.concat(result.parentProductIds)).join(),
            resourceIds: result.resourceIds.join()
        };
    };
    ProductAndProductDetailDropdownComponent.prototype.getSelectedProductIds = function () {
        var productIds = [];
        this.dropdownProduct.fanexMultiSelect("getItemsByCondition", '.product > input[type="checkbox"]:checked').each(function () {
            productIds.push(this.value);
        });
        return productIds;
    };
    ProductAndProductDetailDropdownComponent.prototype.getSelectedResourceIds = function () {
        var parentProductIds = [];
        var resourceIds = [];
        this.dropdownProduct.fanexMultiSelect("getItemsByCondition", '.productdetail > input[type="checkbox"]:checked').each(function () {
            parentProductIds.push(this.name);
            resourceIds.push(this.value);
        });
        return { parentProductIds: parentProductIds, resourceIds: resourceIds, };
    };
    ProductAndProductDetailDropdownComponent.prototype.uniqe = function (array) {
        var uniqeObject = {};
        return array.filter(function (item) {
            return uniqeObject.hasOwnProperty(item)
                ? false
                : (uniqeObject[item] = true);
        });
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map