/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class ProductMultiSelectionOption {
    constructor(
        public dropdownId: string = "dropdown-product",
        public validateBeforeSubmit: boolean = true,
        public productsInputId: string = "product-ids",
        public onChange: Function = null) {
    } 
}

class ProductMultiSelectionComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdownProduct: JQuery;
    $multiSelectButton: JQuery;
    $excludeProducts: any;
    $excludeHiddenInput: JQuery;

    options = new ProductMultiSelectionOption();
    productDetailMultiSelectionComponent: ProductDetailMultiSelectionComponent;
    handleSelectedAllChanged = false;

    constructor($: JQueryStatic, productDetailMultiSelectionComponent: ProductDetailMultiSelectionComponent = undefined) {
        this.$ = $;
        this.productDetailMultiSelectionComponent = productDetailMultiSelectionComponent;
    }

    render = (options: ProductMultiSelectionOption = new ProductMultiSelectionOption()) => {
        this.productDetailMultiSelectionComponent && this.productDetailMultiSelectionComponent.render();

        this.options = <ProductMultiSelectionOption>this.$.extend({}, this.options, options);

        this.$dropdownProduct = this.$("#" + this.options.dropdownId);
        let hasAnyProduct = alphaReports.convertAttributeToBool(this.$dropdownProduct.data("has-any-product"));
        this.$hiddenInput = this.$("#" + this.options.productsInputId);

        this.$excludeProducts = this.$(".exclude-product");
        if (this.$excludeProducts.length) {
            this.$excludeHiddenInput = this.$("#exclude-product-ids");

            this.$excludeProducts
                .off("change")
                .on("change", this.excludeProductOnChange);
        }

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
            language: this.$dropdownProduct.data("language"),
            enableCollapsibleOptGroups: true,
            optionClass: this.optionClass
        });

        this.$dropdownProduct.next().find(".multiselect-container li").removeClass("hidden");

        this.$multiSelectButton = this.$dropdownProduct.parent().find(".multiselect.btn");

        if (this.productDetailMultiSelectionComponent) {
            const self: any = this;
            this.$dropdownProduct.find("option").each(function () {
                self.productDetailMultiSelectionComponent.shouldEnableDropdown(self.$(this).is(":selected"), this.value);
            });
        }

        this.handleSelectedAllChanged = true;
        return this.$dropdownProduct;
    };

    onChange = (option: any, checked: boolean) => {
        this.validate();
        this.$hiddenInput.val(this.getSelectedItems().join(","));

        if (this.handleSelectedAllChanged && this.productDetailMultiSelectionComponent) {
            typeof option === "boolean"
                ? this.productDetailMultiSelectionComponent.disableAllOptions(option)
                : this.productDetailMultiSelectionComponent.disableOptions(checked, option.context.value);
        }

        if (this.options.onChange) {
            this.options.onChange();
        }
    };

    optionClass = (element: HTMLElement) => {
        if (this.$(element).data("isnested") === 1) {
            return "multiselect-nested-item";
        }
    };

    validate = () => {
        if (this.options.validateBeforeSubmit
            && this.$multiSelectButton
            && this.$dropdownProduct.find("option:selected").length
            && this.$multiSelectButton.hasClass("error")) {
            this.$multiSelectButton.removeClass("error");
            this.removeErrorClassFromExcludeProductLabels();
        }
    };

    getSelectedItems = () => {
        let selected = [];
        let selectedOptions = this.$dropdownProduct.find("option:selected");
        let this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected.concat(this.getExcludeSelectedItems());
    };

    excludeProductOnChange = () => {
        this.validateExcludeProductOnChange();
        this.$hiddenInput.val(this.getSelectedItems().join(","));
        this.$excludeHiddenInput.val(this.getExcludeSelectedItems().join(","));
    };

    getExcludeSelectedItems = (): any => {
        const selected: any = [];

        if (this.$excludeProducts.length) {
            const self: any = this;

            this.$excludeProducts.each(function () {
                if (this.checked) {
                    selected.push(self.$(this).val());
                }
            });
        }

        return selected;
    };

    addClassToExcludeProductLabels = (className: string): void => {
        if (this.$excludeProducts.length) {
            const self: any = this;

            this.$excludeProducts.each(function () {
                self.$(this).closest("label").addClass(className);
            });
        }
    };

    removeErrorClassFromExcludeProductLabels = (): void => {
        if (this.$excludeProducts.length) {
            const self: any = this;

            this.$excludeProducts.each(function () {
                self.$(this).closest("label").removeClass("error");
            });
        }
    };

    validateExcludeProductOnChange = () => {
        if (this.options.validateBeforeSubmit
            && this.$multiSelectButton
            && this.$multiSelectButton.hasClass("error")) {
            this.$multiSelectButton.removeClass("error");
            this.removeErrorClassFromExcludeProductLabels();
        }
    };

    isAnyEnabledProductDetailSelected = () => {
        return !this.productDetailMultiSelectionComponent
            || this.productDetailMultiSelectionComponent.isAnyEnabledSelected();
    };

    addErrorToProductDetailComponent = () => {
        this.productDetailMultiSelectionComponent
            && this.productDetailMultiSelectionComponent.addError();
    };
}