'use strict';
var MultipleDropdownComponent = /** @class */ (function () {
    function MultipleDropdownComponent() {
    }
    MultipleDropdownComponent.prototype.render = function (selectorId, options) {
        var $dropdown = $('#' + selectorId);
        var defaults = $.extend({
            selectAllText: $dropdown.data('label-selectall'),
            selectedText: '# ' + $dropdown.data('label-selected'),
            noneSelectedText: $dropdown.data('label-noneselected'),
            customSelectAllText: $dropdown.data('label-all') + ' (#)',
            filterPlaceHolder: $dropdown.data('label-filterplaceholder'),
            noItemFoundText: $dropdown.data('label-noitemfound'),
            noItemText: $dropdown.data('label-nodatatext'),
            minWidth: 220,
            maxHeight: 250,
            showTooltip: false
        }, options);
        this.$dropdownComponent = $dropdown.fanexMultiSelect(defaults);
    };
    MultipleDropdownComponent.prototype.getCheckedValues = function () {
        if (this.$dropdownComponent && this.$dropdownComponent.length) {
            return this.$dropdownComponent.fanexMultiSelect('getCheckedValues');
        }
    };
    return MultipleDropdownComponent;
}());
//# sourceMappingURL=multiple-dropdown.js.map