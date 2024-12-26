'use strict';

interface ListDataItem {
    text: string
    value: any,
    checked?: boolean
}

interface MultipleDropdownOption {
    showFilter?: boolean,
    listData: ListDataItem[],
    onLeave?: Function,
    minWidth?: number,
    maxHeight?: number
}

class MultipleDropdownComponent {
    private $dropdownComponent: JQuery;

    render(selectorId: string, options: MultipleDropdownOption) {
        const $dropdown = $('#' + selectorId);
        const defaults = $.extend({
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
    }

    getCheckedValues(): string {
        if (this.$dropdownComponent && this.$dropdownComponent.length) {
            return this.$dropdownComponent.fanexMultiSelect('getCheckedValues');
        }
    }
}