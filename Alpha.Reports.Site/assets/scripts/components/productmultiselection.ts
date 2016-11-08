"use strict";

class ProductMultiSelectionComponent {
    $: any;

    constructor($: any) {
        this.$ = $;
    }

    options = {
        dropdownId: "dropdown-product",
        validateBeforeSubmit: true
    };

    render = (options: any = {}) => {
        var pickerOptions = this.$.extend({}, this.options, options);

        var $this = this;

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
                $this.validate(dropdownProduct, multiSelectButton, pickerOptions);
            },
            onSelectAll: function () {
                $this.validate(dropdownProduct, multiSelectButton, pickerOptions);
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