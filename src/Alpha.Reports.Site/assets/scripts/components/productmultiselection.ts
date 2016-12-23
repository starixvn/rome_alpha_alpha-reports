/// <reference path="../core.ts" />
"use strict";
class ProductMultiSelectionOption {
    constructor(
        public dropdownId: string = "dropdown-product",
        public validateBeforeSubmit: boolean = true,
        public productsInputId: string = "product-ids") {
    }
}

class ProductMultiSelectionComponent {
    $: any;
    $hiddenInput: any;
    $dropdownProduct: any;
    $multiSelectButton: any;

    options = new ProductMultiSelectionOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: ProductMultiSelectionOption = new ProductMultiSelectionOption()) => {
        this.options = <ProductMultiSelectionOption>this.$.extend({}, this.options, options);

        this.$dropdownProduct = this.$("#" + this.options.dropdownId);
        let hasAnyProduct = this.$dropdownProduct.data("has-any-product");
        this.$hiddenInput = this.$("#" + this.options.productsInputId);

        this.$dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: hasAnyProduct
                ? this.$dropdownProduct.data("label-select-products")
                : this.$dropdownProduct.data("label-there-is-no-product"),
            nSelectedText: ` ${this.$dropdownProduct.data("label-selected")}`,
            allSelectedText: this.$dropdownProduct.data("label-all-products"),
            includeSelectAllOption: hasAnyProduct,
            onChange: this.onChange,
            onSelectAll: this.onChange,
            language: this.$dropdownProduct.data("language")
        });

        this.$multiSelectButton = this.$dropdownProduct.parent().find(".multiselect.btn");

        return this.$dropdownProduct;
    };

    onChange = (option: any, checked: boolean) => {
        this.validate();
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };

    validate = () => {
        if (this.options.validateBeforeSubmit
            && this.$multiSelectButton
            && this.$dropdownProduct.find("option:selected").length
            && this.$multiSelectButton.hasClass("error")) {
            this.$multiSelectButton.removeClass("error");
        }
    };

    getSelectedItems = () => {
        let selected = [];
        let selectedOptions = this.$dropdownProduct.find("option:selected");
        let this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected;
    };
}