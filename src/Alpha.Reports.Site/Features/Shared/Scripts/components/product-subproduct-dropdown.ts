"use strict";

class ProductAndSubProductDropdownOption {
    constructor(
        public products: any,
        public productDetails: any,
        public onDropdownHidden = Function(),
        public selectorId = "ddlProduct") {
        this.products = products;
        this.productDetails = productDetails;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
}

class ProductAndSubProductDropdownComponent {
    private $: JQueryStatic;
    private $dropdownProduct: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(options: ProductAndSubProductDropdownOption) {
        const productDetailObject = this.buildProductDetailObject(options.productDetails);

        const productList = [];
        options.products.forEach((item) => {
            const isGroup = typeof item.Products !== "undefined";
            const cssClass = isGroup ? "groupproduct" : "product";
            const productItem: any = this.buildCheckboxProduct(item, productDetailObject, cssClass);
            if (isGroup) {
                productItem.items = item.Products.map(item => this.buildCheckboxProduct(item, productDetailObject));
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
    }

    private buildProductDetailObject(productDetails) {
        const productDetailObject = {};

        productDetails.forEach((item) => {
            let items = productDetailObject[item.ProductId];
            if (typeof items === "undefined") {
                items = [];
                productDetailObject[item.ProductId] = items;
            }

            items.push(this.buildCheckboxProductDetail(item));
        });

        return productDetailObject;
    }

    private buildCheckboxProduct(product: any, productDetailObject: any, cssClass = "product") {
        const checkboxItem: any = this.buildCheckboxItem(product, cssClass);
        checkboxItem.items = productDetailObject[product.Id];

        return checkboxItem;
    }

    private buildCheckboxProductDetail(product: any) {
        return this.buildCheckboxItem(product, "productdetail", product.ProductId);
    }

    private buildCheckboxItem(item: any, cssClass: string, name = undefined) {
        return {
            id: cssClass + "-" + item.Id,
            name: name,
            value: item.Id,
            text: item.Name,
            cssClass: cssClass,
            checked: true,
            enable: true
        }
    }

    getSelectedItems() {
        const productIds = this.getSelectedProductIds();
        const result = this.getSelectedResourceIds();

        return {
            productIds: this.uniqe(productIds.concat(result.parentProductIds)).join(),
            resourceIds: result.resourceIds.join()
        };
    }

    private getSelectedProductIds() {
        const productIds = [];
        this.$dropdownProduct.fanexMultiSelect(
            "getItemsByCondition",
            '.product > input[type="checkbox"]:checked').each(function () {
                productIds.push(this.value);
            });

        return productIds;
    }

    private getSelectedResourceIds() {
        const parentProductIds = [];
        const resourceIds = [];
        this.$dropdownProduct.fanexMultiSelect(
            "getItemsByCondition",
            '.productdetail > input[type="checkbox"]:checked').each(function () {
                parentProductIds.push(this.name);
                resourceIds.push(this.value);
            });

        return { parentProductIds, resourceIds, };
    }

    private uniqe(array) {
        const uniqeObject = {};
        return array.filter(function (item) {
            return uniqeObject.hasOwnProperty(item)
                ? false
                : (uniqeObject[item] = true);
        });
    }
}