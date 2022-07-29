/// <reference path="../core.ts" />
"use strict";
var SingleSelectionDropdownOption = /** @class */ (function () {
    function SingleSelectionDropdownOption(dropdownId, inputId, onChange) {
        if (dropdownId === void 0) { dropdownId = "dropdown-single"; }
        if (inputId === void 0) { inputId = "UserSelectedId"; }
        if (onChange === void 0) { onChange = null; }
        this.dropdownId = dropdownId;
        this.inputId = inputId;
        this.onChange = onChange;
    }
    return SingleSelectionDropdownOption;
}());
var SingleSelectionDropdownComponent = /** @class */ (function () {
    function SingleSelectionDropdownComponent($) {
        var _this = this;
        this.options = new SingleSelectionDropdownOption();
        this.render = function (options) {
            if (options === void 0) { options = new SingleSelectionDropdownOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var $dropdown = _this.$("#" + _this.options.dropdownId);
            _this.$dropdown = $dropdown;
            var $hiddenInput = _this.$("#" + _this.options.inputId);
            _this.$hiddenInput = $hiddenInput;
            var self = _this;
            _this.selectedValue = self.getSelectedItem();
            _this.$dropdown.multiselect({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false,
                onChange: _this.onchange
            });
        };
        this.getSelectedItem = function () {
            var selected = [];
            var selectedOptions = _this.$dropdown.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected.length ? selected[0].toString() : "";
        };
        this.updateSelectedValue = function () {
            _this.$hiddenInput.val(_this.selectedValue);
        };
        this.onchange = function () {
            _this.selectedValue = _this.getSelectedItem();
            if (_this.options.onChange) {
                _this.options.onChange();
            }
        };
        this.$ = $;
    }
    return SingleSelectionDropdownComponent;
}());
//# sourceMappingURL=single-selection-dropdown.js.map