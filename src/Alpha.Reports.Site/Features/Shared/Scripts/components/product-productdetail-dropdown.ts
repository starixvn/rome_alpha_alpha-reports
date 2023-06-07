"use strict";

class ProductAndProductDetailDropdownOption {
    constructor(
        public products: any,
        public productDetails: any,
        public onDropdownHidden = Function(),
        public selectorProductId = "dropdown-product",
        public selectorProductDetailId = "dropdown-product-detail") {
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorProductId = selectorProductId;
        this.selectorProductDetailId = selectorProductDetailId;
    }
}

class ProductAndProductDetailDropdownComponent {
    private dropdownProduct: JQuery;
    private dropdownProductDetail: JQuery;
    private onlyShowHistoricProductIds = [];

    render(options: ProductAndProductDetailDropdownOption) {
        this.renderProductDetailDropdown(options);
        this.renderProductDropdown(options);
    }

    private renderProductDetailDropdown(options: ProductAndProductDetailDropdownOption) {
        const self = this;
        const productDetailList = options.productDetails.map(function (item) {
            return self.buildCheckboxItem(item, {
                productId: item.ProductId
            });
        });

        const dropdown = $("#" + options.selectorProductDetailId);
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
                const productIds = self.dropdownProductDetail.fanexMultiSelect("getUncheckedProductIds");
                if (productIds.length) {
                    self.dropdownProduct.fanexMultiSelect("uncheck", productIds);
                }

                options.onDropdownHidden(self.getCheckedValues());
            }
        });
    }

    private renderProductDropdown(options: ProductAndProductDetailDropdownOption) {
        const productIdWithSubProductIds = {};
        const uncheckedProductIds = [];
        const productList = options.products.map((item) => {
            item.OnlyShowHistoric && this.onlyShowHistoricProductIds.push(item.Id);
            const checkboxItem: any = this.buildCheckboxItem(item);

            if (item.Products) {
                const subProductIds = [];
                productIdWithSubProductIds[item.Id] = subProductIds;

                checkboxItem.items = item.Products.map((item) => {
                    item.OnlyShowHistoric && this.onlyShowHistoricProductIds.push(item.Id);
                    subProductIds.push(item.Id);
                    item.Checked || uncheckedProductIds.push(item.Id);
                    return this.buildCheckboxItem(item);
                });
            } else {
                item.Checked || uncheckedProductIds.push(item.Id);
            }

            return checkboxItem;
        });

        const self = this;
        const handleOnCheckProductInternal = function (productId, checked) {
            const subProductIds = productIdWithSubProductIds[productId];
            if (subProductIds) {
                subProductIds.forEach(function (id) {
                    self.handleOnCheckProduct(id, checked);
                });
            } else {
                self.handleOnCheckProduct(productId, checked);
            }
        };

        const dropdown = $("#" + options.selectorProductId);
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
            expandGroupsByDefault: false,
            listData: productList,
            onCheck: function () {
                handleOnCheckProductInternal(this.value, this.checked);
            },
            onCheckAll: function () {
                const checked = this.checked;
                const checkedValues = self.dropdownProduct.fanexMultiSelect("getCheckedValues");
                const uncheckedValues = self.dropdownProduct.fanexMultiSelect("getUncheckedValues");

                if ((checked && !uncheckedValues.length) || (!checked && !checkedValues.length)) {
                    self.dropdownProductDetail.fanexMultiSelect("showAllItems", checked);
                    self.dropdownProductDetail.fanexMultiSelect("disable", !checked);
                } else {
                    const productIdString = checked
                        ? checkedValues
                        : uncheckedValues;
                    productIdString.split(",").forEach(function (id) {
                        handleOnCheckProductInternal(id, checked);
                    });
                }
            },
            onLeave: function () {
                options.onDropdownHidden(self.getCheckedValues());
            }
        });

        this.initState(uncheckedProductIds);
    }

    private initState(uncheckedProductIds) {
        if (uncheckedProductIds.length) {
            uncheckedProductIds.forEach((item) => this.handleOnCheckProduct(item, false));
        }
    }

    private handleOnCheckProduct(productId, checked) {
        const action = checked ? "showItemsByCustomAttributes" : "hideItemsByCustomAttributes";
        this.dropdownProductDetail.fanexMultiSelect(action, { productId });
    }

    private buildCheckboxItem(item, attributes = undefined) {
        return {
            value: item.Id,
            text: item.Name,
            checked: item.Checked,
            attributes
        }
    }

    getCheckedValues() {
        const productsAndResources = {
            productIds: this.dropdownProduct.fanexMultiSelect("getCheckedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getCheckedValues")
        };

        this.dropdownProduct.fanexMultiSelect("addError", productsAndResources.productIds.length === 0);
        return productsAndResources;
    }

    getUncheckedValues() {
        return {
            unselectedProductIds: this.dropdownProduct.fanexMultiSelect("getUncheckedValues"),
            unselectedResourceIds: this.dropdownProductDetail.fanexMultiSelect("getUncheckedValues")
        };
    }

    showHistoricProducts() {
        this.onlyShowHistoricProductIds.length
            && this.dropdownProduct.fanexMultiSelect("showItemsByValues", this.onlyShowHistoricProductIds);
    }

    hideHistoricProducts() {
        this.onlyShowHistoricProductIds.length
            && this.dropdownProduct.fanexMultiSelect("hideItemsByValues", this.onlyShowHistoricProductIds);
    }
}