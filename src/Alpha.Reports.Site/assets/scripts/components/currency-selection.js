/// <reference path="../core.ts" />
"use strict";
var CurrencySelectionOption = /** @class */ (function () {
    function CurrencySelectionOption(dropdownId, currencyInputId) {
        if (dropdownId === void 0) { dropdownId = "dropdown-currencies"; }
        if (currencyInputId === void 0) { currencyInputId = "UserSelectedCurrencyId"; }
        this.dropdownId = dropdownId;
        this.currencyInputId = currencyInputId;
    }
    return CurrencySelectionOption;
}());
var CurrencySelectionComponent = /** @class */ (function () {
    function CurrencySelectionComponent($) {
        var _this = this;
        this.options = new CurrencySelectionOption();
        this.render = function (options) {
            if (options === void 0) { options = new CurrencySelectionOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var $dropdownCurrency = _this.$("#" + _this.options.dropdownId);
            _this.$dropdownCurrency = $dropdownCurrency;
            var $hiddenInput = _this.$("#" + _this.options.currencyInputId);
            _this.$hiddenInput = $hiddenInput;
            var self = _this;
            _this.$dropdownCurrency.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: true,
                onChange: function () {
                    self.selectedValue = self.getSelectedItems();
                }
            });
            _this.selectedValue = _this.getSelectedItems();
        };
        this.getSelectedItems = function () {
            var selected = [];
            var selectedOptions = _this.$dropdownCurrency.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected[0].toString();
        };
        this.updateSelectedValue = function () {
            _this.$hiddenInput.val(_this.selectedValue);
        };
        this.$ = $;
    }
    return CurrencySelectionComponent;
}());
//# sourceMappingURL=currency-selection.js.map