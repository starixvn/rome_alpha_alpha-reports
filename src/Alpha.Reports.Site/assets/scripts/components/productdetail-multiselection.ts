/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class ProductDetailMultiSelectionOption {
    constructor(
        public dropdownId: string = "dropdown-product-detail",
        public resourcesInputId: string = "resource-ids") {
    }
}

class ProductDetailMultiSelectionComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdownProductDetail: JQuery;
    $multiSelectButton: JQuery;

    options = new ProductDetailMultiSelectionOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: ProductDetailMultiSelectionOption = new ProductDetailMultiSelectionOption()) => {
        this.options = <ProductDetailMultiSelectionOption>this.$.extend({}, this.options, options);

        this.$dropdownProductDetail = this.$("#" + this.options.dropdownId);
        this.$hiddenInput = this.$("#" + this.options.resourcesInputId);
        const hasAnyProductDetail = "True" === this.$dropdownProductDetail.data("has-any-product-detail");

        this.$dropdownProductDetail.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProductDetail.data("label-select-product-detail"),
            nSelectedText: ` ${this.$dropdownProductDetail.data("label-selected")}`,
            allSelectedText: this.$dropdownProductDetail.data("label-all-product-detail"),
            includeSelectAllOption: hasAnyProductDetail,
            onChange: this.onChange,
            onSelectAll: this.onChange,
            language: this.$dropdownProductDetail.data("language")
        });
    };

    onChange = () => {
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };

    getSelectedItems = () => {
        let selected = [];
        const this$ = this.$;

        this.$dropdownProductDetail.find("option:selected").each(function () {
            selected.push([this$(this).val()]);
        });

        return selected;
    };

    disableAllOptions = (checked: boolean) => {
        if (checked) {
            this.$dropdownProductDetail.multiselect("enable");
            this.$hiddenInput.val(this.getSelectedItems().join(","));
        } else {
            this.$dropdownProductDetail.multiselect("disable");
            this.$hiddenInput.val("");
        }
    };

    disableOptions = (checked: boolean, productId: number) => {
        const $this = this.$;
        const $options = this.$dropdownProductDetail.find('option[data-productid="' + productId + '"]');

        if ($options.length) {
            $options.prop("disabled", !checked);

            this.$dropdownProductDetail.multiselect("refresh");
            this.$hiddenInput.val(this.getSelectedItems().join(","));

            if (checked && !this.$dropdownProductDetail.find('option:not([data-productid="' + productId + '"]):disabled').length) {
                this.$dropdownProductDetail.multiselect("enable");
            } else if (!this.$dropdownProductDetail.find('option:not([data-productid="' + productId + '"]):enabled').length) {
                this.$dropdownProductDetail.multiselect("disable");
            }
        }
    };
}