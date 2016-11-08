"use strict";

class ProductMultiSelectionComponent {
    options = {
        dropdownId: "dropdown-product",
        validateBeforeSubmit: true
    };

    render = (options: any = {}) => {
        var pickerOptions = $.extend({}, this.options, options);

        var $this = this;

        var dropdownProduct = $("#" + pickerOptions.dropdownId);
        var multiSelectButton = $(".multiselect.btn");
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
}

const productMultiSelectionComponent = new ProductMultiSelectionComponent();