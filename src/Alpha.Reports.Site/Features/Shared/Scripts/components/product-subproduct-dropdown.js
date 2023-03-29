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
    function ProductAndSubProductDropdownComponent() {
    }
    return ProductAndSubProductDropdownComponent;
}());
//# sourceMappingURL=product-subproduct-dropdown.js.map