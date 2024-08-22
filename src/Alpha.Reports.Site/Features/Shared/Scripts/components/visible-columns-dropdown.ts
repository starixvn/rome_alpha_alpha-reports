"use strict";

interface Object {
    entries: any;
}

class VisibleColumnDropdownOption {
    constructor(
        public visibleColumns,
        public listColumn,
        public onDropdownHidden = Function(),
        public selectorId = "dropdown-column",
        public minWidth = 220) {
        this.visibleColumns = visibleColumns;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.minWidth = minWidth;
    }
}

class VisibleColumnDropdownComponent {
    private dropdownColumn: JQuery;

    render(options: VisibleColumnDropdownOption) {
        let isCheckAll = false;
        const dropdown = $("#" + options.selectorId);

        const visibleColumnObject = {};
        if (options.visibleColumns.length) {
            const visibleColumnArray = options.visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = options.listColumn.length === visibleColumnArray.length;
        } else {
            options.listColumn.forEach(function (item) {
                if (item.checked !== false) {
                    visibleColumnObject[item.value] = true;
                }
            });
        }

        this.dropdownColumn = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            isCheckAll: isCheckAll,
            minWidth: options.minWidth,
            maxHeight: 250,
            showTooltip: false,
            listData: options.listColumn.map(function (item) {
                return {
                    value: item.value,
                    text: item.text,
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(item.value)
                };
            }),
            onLeave: () => options.onDropdownHidden(this.getColumns())
        });
    }

    getColumns() {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    }

    getShowColumns() {
        return this.dropdownColumn.fanexMultiSelect("getCheckedValues");
    }

    getHideColumns() {
        return this.dropdownColumn.fanexMultiSelect("getUncheckedValues");
    }
}