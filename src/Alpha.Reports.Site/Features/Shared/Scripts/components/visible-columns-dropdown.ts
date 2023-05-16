"use strict";

interface Object {
    entries: any;
}

class VisibleColumnDropdownOption {
    constructor(
        public visibleColumnKey,
        public listColumn,
        public onDropdownHidden = Function(),
        public selectorId = "dropdown-column") {
        this.visibleColumnKey = visibleColumnKey;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
}

class VisibleColumnDropdownComponent {
    private dropdownColumn: JQuery;

    render(options: VisibleColumnDropdownOption) {
        let isCheckAll = true;
        const dropdown = $("#" + options.selectorId);

        const visibleColumnObject = {};
        const visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns !== "undefined") {
            const visibleColumnArray = visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = options.listColumn.length === visibleColumnArray.length;
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
            minWidth: 220,
            maxHeight: 250,
            showTooltip: false,
            listData: options.listColumn.map(function (item) {
                return {
                    value: item.value,
                    text: item.text,
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(item.value)
                };
            }),
            onLeave: () => this.onDropdownHidden(options.visibleColumnKey, options.onDropdownHidden)
        });
    }

    private onDropdownHidden(visibleColumnKey: string, callback: Function) {
        const columns = this.getColumns();

        const selectedColumnsValue = [];
        Object.entries(columns).forEach(([key, value]) => value && selectedColumnsValue.push(key));
        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());

        callback(columns);
    }

    getColumns() {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    }

    getHideColumns() {
        return this.dropdownColumn.fanexMultiSelect("getUncheckedValues");
    }
}