/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class CurrencySelectionOption {
    constructor(
        public dropdownId: string = "dropdown-currencies",
        public currencyInputId: string = "UserSelectedCurrencyId") {
    }
}

class CurrencySelectionComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdownCurrency: JQuery;

    selectedValue: string;

    options = new CurrencySelectionOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: CurrencySelectionOption = new CurrencySelectionOption()) => {
        this.options = <CurrencySelectionOption>this.$.extend({}, this.options, options);

        let $dropdownCurrency = this.$("#" + this.options.dropdownId);
        this.$dropdownCurrency = $dropdownCurrency;

        let $hiddenInput = this.$("#" + this.options.currencyInputId);
        this.$hiddenInput = $hiddenInput;

        let self = this;

        this.$dropdownCurrency.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: true,
            onChange: function () {
                self.selectedValue = self.getSelectedItems();
            }
        });

        this.selectedValue = this.getSelectedItems();
    };
    getSelectedItems = () => {
        let selected = [];
        let selectedOptions = this.$dropdownCurrency.find("option:selected");
        let this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected[0].toString();
    };
    updateSelectedValue = () => {
        this.$hiddenInput.val(this.selectedValue);
    };
}