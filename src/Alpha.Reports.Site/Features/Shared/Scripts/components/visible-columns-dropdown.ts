"use strict";

interface Object {
    entries: any;
}

class VisibleColumnDropdownOption {
    constructor(
        public visibleColumnKey,
        public onDropdownHidden = Function(),
        public selectorId = "dropdown-column") {
        this.visibleColumnKey = visibleColumnKey;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
}

class VisibleColumnDropdownComponent {
    private dropdownColumn: JQuery;

    render(options: VisibleColumnDropdownOption) {
        let isCheckAll = true;
        const dropdown = $("#" + options.selectorId);
        const dropdownOptions = dropdown.find("option");

        const visibleColumnObject = {};
        const visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns !== "undefined") {
            const visibleColumnArray = visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = dropdownOptions.length === visibleColumnArray.length;
        }

        this.dropdownColumn = dropdown.fanexMultiSelect({
            checkAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: dropdown.data("label-all") + " (#)",
            showFilter: true,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            isCheckAll: isCheckAll,
            minWidth: 220,
            maxHeight: 250,
            showTooltip: false,
            listData: dropdownOptions.map(function () {
                return {
                    value: this.value,
                    text: this.innerText,
                    cssClass: "visible-column",
                    checked: isCheckAll || visibleColumnObject.hasOwnProperty(this.value),
                    enable: true
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
}