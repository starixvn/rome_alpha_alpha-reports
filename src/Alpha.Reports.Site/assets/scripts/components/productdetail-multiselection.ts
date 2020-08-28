/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class ProductDetailMultiSelectionOption {
    constructor(
        public dropdownId: string = "dropdown-product-detail",
        public validateBeforeSubmit: boolean = true,
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
        const selectAllText = this.$dropdownProductDetail.data("label-all-product-detail");

        this.$dropdownProductDetail.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProductDetail.data("label-select-product-detail"),
            nSelectedText: ` ${this.$dropdownProductDetail.data("label-selected")}`,
            allSelectedText: selectAllText,
            selectAllText: selectAllText,
            includeSelectAllOption: hasAnyProductDetail,
            onChange: this.onChange,
            onSelectAll: this.onChange,
            language: this.$dropdownProductDetail.data("language")
        });

        this.$multiSelectButton = this.$dropdownProductDetail.parent().find(".multiselect.btn");

        return this.$dropdownProductDetail;
    };

    onChange = () => {
        this.validate();
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };

    getSelectedItems = () => {
        let selected = [];
        const this$ = this.$;

        this.$dropdownProductDetail.find("option:selected").each(function () {
            selected.push(this$(this).val());
        });

        return selected;
    };

    disableAllOptions = (checked: boolean) => {
        this.$dropdownProductDetail.multiselect(checked ? "enable" : "disable");

        this.$dropdownProductDetail
            .find("option")
            .prop("selected", checked)
            .prop("disabled", !checked);
        this.$dropdownProductDetail.multiselect("refresh");
    };

    disableOptions = (checked: boolean, productId: number) => {
        const $options = this.$dropdownProductDetail.find('option[data-productid="' + productId + '"]');

        if ($options.length) {
            $options
                .prop("selected", checked)
                .prop("disabled", !checked);
            this.$dropdownProductDetail.multiselect("refresh");

            if (checked && !this.$dropdownProductDetail.find('option:not([data-productid="' + productId + '"]):disabled').length) {
                this.$dropdownProductDetail.multiselect("enable");
            } else if (!this.$dropdownProductDetail.find('option:not([data-productid="' + productId + '"]):enabled').length) {
                this.$dropdownProductDetail.multiselect("disable");
            }
        }
    };

    shouldEnableDropdown = (productId: number) => {
        this.$dropdownProductDetail.find('option:selected[data-productid="' + productId + '"]').length
            && this.$dropdownProductDetail.multiselect("enable");
    };

    validate = () => {
        if (this.options.validateBeforeSubmit
            && this.$multiSelectButton
            && this.isAnyEnabledSelected()
            && this.$multiSelectButton.hasClass("error")) {
            this.$multiSelectButton.removeClass("error");
            this.$dropdownProductDetail.removeClass("error");
        }
    };

    addError = () => {
        this.$multiSelectButton.addClass("error");
        this.$dropdownProductDetail.addClass("error");
    };

    isAnyEnabledSelected = () => {
        if (!this.$multiSelectButton.length || this.$multiSelectButton.is(":disabled")) {
            return true;
        }

        return this.$dropdownProductDetail.find("option:selected").length > 0;
    };
}