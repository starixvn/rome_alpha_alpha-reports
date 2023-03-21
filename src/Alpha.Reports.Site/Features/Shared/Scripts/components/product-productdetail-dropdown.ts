"use strict";
interface JQuery {
    multiselect(option: any): void;
    multiselect(method: string, value: any): void;
}

class ProductAndProductDetailDropdownOption {
    constructor(
        public onDropdownHidden = Function(),
        public productSelectorId = "dropdown-product",
        public productDetailSelectorId = "dropdown-product-detail") {
        this.onDropdownHidden = onDropdownHidden;
        this.productSelectorId = productSelectorId;
        this.productDetailSelectorId = productDetailSelectorId;
    }
}

class ProductAndProductDetailDropdownComponent {
    private $: JQueryStatic;
    private $dropdownProduct: JQuery;
    private $dropdownProductContainer: JQuery;
    private $dropdownProductDetail: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(options: ProductAndProductDetailDropdownOption) {
        this.renderProduct(options);
        this.renderProductDetail(options);
    }

    private renderProduct(options: ProductAndProductDetailDropdownOption) {
        this.$dropdownProduct = this.$("#" + options.productSelectorId);
        this.$dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProduct.data("label-select-products"),
            nSelectedText: ` ${this.$dropdownProduct.data("label-selected")}`,
            allSelectedText: this.$dropdownProduct.data("label-all-products"),
            selectAllText: this.$dropdownProduct.data("label-select-all"),
            includeSelectAllOption: true,
            onChange: (option, checked) => this.checkOrUncheckProductDetail(checked, option.context.value),
            enableCollapsibleOptGroups: true,
            optionClass: this.optionClass,
            onDropdownHidden: options.onDropdownHidden
        });

        this.$dropdownProduct.next().find(".multiselect-container li").removeClass("hidden");
        this.$dropdownProductContainer = this.$dropdownProduct.parent().find(".multiselect.btn");

        // Cannot use build in function 'onSelectAll'.
        // Must register CheckAll checkbox event manually.
        this.$("#product-multiselect-container .multiselect-all input[type=checkbox]").change(
            (e) => this.checkOrUncheckProductDetail(this.$(e.target).is(":checked"))
        );
    }

    private checkOrUncheckProductDetail(checked: boolean, productId: number = undefined) {
        this.validateProduct();

        const optionElements = productId
            ? this.$dropdownProductDetail.find(`option[data-productid="${productId}"]`)
            : this.$dropdownProductDetail.find("option");
        let selectedProductIds = [];
        optionElements.each((_, item) => selectedProductIds.push(item["value"]));
        this.$dropdownProductDetail.multiselect(checked ? "select" : "deselect", selectedProductIds);
    }

    private validateProduct() {
        if (this.$dropdownProduct.find("option:selected").length) {
            this.$dropdownProductContainer.removeClass("error");
        } else {
            this.$dropdownProductContainer.addClass("error");
        }
    }

    private optionClass = (element: HTMLElement) => {
        if (this.$(element).data("isnested") === 1) {
            return "multiselect-nested-item";
        }
    };

    private renderProductDetail(options: ProductAndProductDetailDropdownOption) {
        this.$dropdownProductDetail = this.$("#" + options.productDetailSelectorId);
        const selectAllText = this.$dropdownProductDetail.data("label-all-product-detail");

        this.$dropdownProductDetail.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProductDetail.data("label-select-product-detail"),
            nSelectedText: ` ${this.$dropdownProductDetail.data("label-selected")}`,
            allSelectedText: selectAllText,
            selectAllText: selectAllText,
            includeSelectAllOption: true,
            onChange: this.onChangeProductDetail,
            onDropdownHidden: options.onDropdownHidden
        });

        // Cannot use build in function 'onSelectAll'.
        // Must register CheckAll checkbox event manually.
        this.$("#product-detail-multiselect-container .multiselect-all input[type=checkbox]").change(this.onSelectAllProductDetail);
    }

    private onChangeProductDetail = (option: any, checked: boolean) => {
        const productId = this.$(option.context).data("productid");
        if (checked) {
            if (!this.$dropdownProduct.find(`option[value="${productId}"]:selected`).length) {
                this.$dropdownProduct.multiselect("select", productId);
            }
        } else {
            if (!this.$dropdownProductDetail.find(`option[data-productid="${productId}"]:selected`).length) {
                this.$dropdownProduct.multiselect("deselect", productId)
            }
        }

        this.validateProduct();
    };

    private onSelectAllProductDetail = (e) => {
        const productIds = [];
        this.$dropdownProductDetail.find("option").each((_, item) => productIds.push(this.$(item).data("productid")));
        const uniqueProductIds = productIds.filter((value, index) => productIds.indexOf(value) === index);

        const checked = this.$(e.target).is(":checked");
        this.$dropdownProduct.multiselect(checked ? "select" : "deselect", uniqueProductIds);

        this.validateProduct();
    };

    getProductSelectedItems() {
        const selectedProductIds = [];
        this.$dropdownProduct.find("option:selected").each((_, item) => selectedProductIds.push(item["value"]));

        return selectedProductIds.join();
    }

    getProductDetailSelectedItems() {
        const selectedProductDetailIds = [];
        this.$dropdownProductDetail.find("option:selected").each((_, item) => selectedProductDetailIds.push(item["value"]));

        return selectedProductDetailIds.join();
    }
}