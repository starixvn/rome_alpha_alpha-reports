/// <reference path="../core.ts" />
"use strict";
class ProductMultiSelectionOption {
    dropdownId: string;
    validateBeforeSubmit: boolean;

    constructor(dropdownId: string = "dropdown-product",
        validateBeforeSubmit: boolean = true) {

        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
    }
}

class ProductMultiSelectionComponent {
    $: any;

    options = new ProductMultiSelectionOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: any = {}) => {
        var pickerOptions = this.$.extend({}, this.options, options);

        var self = this;

        var dropdownProduct = this.$("#" + pickerOptions.dropdownId);
        var multiSelectButton = this.$(".multiselect.btn");
        var hasProduct = parseInt(dropdownProduct.data("total-product"), 0) > 0;

        dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: hasProduct ?
                dropdownProduct.data("labelselectproducts")
                : dropdownProduct.data("labelthereisnoproduct"),
            nSelectedText: " " + dropdownProduct.data("labelselected"),
            allSelectedText: dropdownProduct.data("labelallproducts"),
            includeSelectAllOption: hasProduct,
            onChange: function (option: any, checked: boolean) {
                self.validate(dropdownProduct, multiSelectButton, pickerOptions);
            },
            onSelectAll: function () {
                self.validate(dropdownProduct, multiSelectButton, pickerOptions);
            },
            language: dropdownProduct.data("language")
        });

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
        var self = this;

        var selected = [];

        var selectedOptions = dropdownProduct.find("option:selected");
        selectedOptions.each(function () {
            selected.push([self.$(this).val()]);
        });

        return selected;
    };
}

const productMultiSelectionComponent = new ProductMultiSelectionComponent($);