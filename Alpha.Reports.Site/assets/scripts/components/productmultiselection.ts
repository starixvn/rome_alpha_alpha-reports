/// <reference path="../core.ts" />
"use strict";
class ProductMultiSelectionOption {
    dropdownId: string;
    validateBeforeSubmit: boolean;
    hiddenInputId: string;

    constructor(dropdownId: string = "dropdown-product",
        validateBeforeSubmit: boolean = true,
        hiddenInputId: string = "product-ids") {

        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
        this.hiddenInputId = hiddenInputId;
    }
}

class ProductMultiSelectionComponent {
    $: any;
    $hiddenInput: any;

    options = new ProductMultiSelectionOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: ProductMultiSelectionOption = new ProductMultiSelectionOption()) => {
        let pickerOptions = this.$.extend({}, this.options, options);

        let self = this;

        let dropdownProduct = this.$("#" + pickerOptions.dropdownId);
        let multiSelectButton = null;
        let hasAnyProduct = dropdownProduct.data("has-any-product");
        this.$hiddenInput = this.$("#" + pickerOptions.hiddenInputId);

        dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: hasAnyProduct ?
                dropdownProduct.data("labelselectproducts")
                : dropdownProduct.data("labelthereisnoproduct"),
            nSelectedText: " " + dropdownProduct.data("labelselected"),
            allSelectedText: dropdownProduct.data("labelallproducts"),
            includeSelectAllOption: hasAnyProduct,
            onChange: function (option: any, checked: boolean) {
                self.validate(dropdownProduct, multiSelectButton, pickerOptions);
                self.$hiddenInput.val(self.getSelectedItems(dropdownProduct).join(","));
            },
            onSelectAll: function () {
                self.validate(dropdownProduct, multiSelectButton, pickerOptions);
            },
            language: dropdownProduct.data("language")
        });

        multiSelectButton = this.$(".multiselect.btn");

        return dropdownProduct;
    };
    validate = (dropdownProduct: any,
        multiSelectButton: any,
        pickerOptions: any) => {
        if (pickerOptions.validateBeforeSubmit &&
            multiSelectButton &&
            dropdownProduct.find("option:selected").length &&
            multiSelectButton.hasClass("error")) {
            multiSelectButton.removeClass("error");
        }
    };
    getSelectedItems = (dropdownProduct: any) => {
        let self = this;

        let selected = [];

        let selectedOptions = dropdownProduct.find("option:selected");
        selectedOptions.each(function () {
            selected.push([self.$(this).val()]);
        });

        return selected;
    };
}

const productMultiSelectionComponent = new ProductMultiSelectionComponent($);