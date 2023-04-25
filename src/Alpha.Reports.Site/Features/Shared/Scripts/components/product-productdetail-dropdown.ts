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
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
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

                options.onDropdownHidden(self.getSelectedItems());
            }
        });
    }

    private renderProductDropdown(options: ProductAndProductDetailDropdownOption) {
        const self = this;
        const productIdWithSubProductIds = {};
        const uncheckedProductIds = [];
        const productList = options.products.map(function (item) {
            const checkboxItem: any = self.buildCheckboxItem(item);

            if (item.Products) {
                const subProductIds = [];
                productIdWithSubProductIds[item.Id] = subProductIds;

                checkboxItem.items = item.Products.map(function (item) {
                    subProductIds.push(item.Id);
                    item.Checked || uncheckedProductIds.push(item.Id);
                    return self.buildCheckboxItem(item);
                });
            } else {
                item.Checked || uncheckedProductIds.push(item.Id);
            }

            return checkboxItem;
        });

        const dropdown = $("#" + options.selectorProductId);
        this.dropdownProduct = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            minWidth: 250,
            maxHeight: 250,
            showTooltip: false,
            expandGroupsByDefault: false,
            listData: productList,
            onCheck: function () {
                const checked = this.checked;
                const productId = this.value;
                const subProductIds = productIdWithSubProductIds[productId];
                if (subProductIds) {
                    subProductIds.forEach(function (id) {
                        self.handleOnCheckProduct(id, checked);
                    });
                } else {
                    self.handleOnCheckProduct(productId, checked);
                }
            },
            onCheckAll: function () {
                const checked = this.checked;
                self.dropdownProductDetail.fanexMultiSelect("showAllItems", checked);
                self.dropdownProductDetail.fanexMultiSelect("disable", !checked);
            },
            onLeave: function () {
                options.onDropdownHidden(self.getSelectedItems());
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

    getSelectedItems() {
        const productsAndResources = {
            productIds: this.dropdownProduct.fanexMultiSelect("getCheckedValues"),
            resourceIds: this.dropdownProductDetail.fanexMultiSelect("getCheckedValues")
        };

        this.dropdownProduct.fanexMultiSelect("addError", productsAndResources.productIds.length === 0);
        return productsAndResources;
    }
}