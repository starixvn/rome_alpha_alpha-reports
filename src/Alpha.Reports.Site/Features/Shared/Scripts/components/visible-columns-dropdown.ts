"use strict";

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
    private $: JQueryStatic;
    private dropdownColumn: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(options: VisibleColumnDropdownOption) {
        let isCheckAll = true;
        const ddlColumn = this.$("#" + options.selectorId);
        const ddlColumnOptions = ddlColumn.find("option");

        const visibleColumnObject = {};
        const visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns !== "undefined") {
            const visibleColumnArray = visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = ddlColumnOptions.length === visibleColumnArray.length;
        }

        this.dropdownColumn = ddlColumn.fanexMultiSelect({
            checkAllText: ddlColumn.data("label-selectall"),
            selectedText: "# " + ddlColumn.data("label-selected"),
            noneSelectedText: ddlColumn.data("label-noneselected"),
            useCustomCheckAllText: true,
            customCheckAllText: ddlColumn.data("label-all") + " (#)",
            isCheckAll: isCheckAll,
            minWidth: 220,
            //maxHeight: 250,
            showTooltip: false,
            listData: ddlColumnOptions.map(function () {
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

        return this.getColumns();
    }

    private onDropdownHidden(visibleColumnKey: string, callback: Function) {
        const selectedColumnsValue = [];
        const columns = {};
        this.dropdownColumn.fanexMultiSelect(
            "getItemsByCondition",
            '.visible-column > input[type="checkbox"]').each(function () {
                const value = this.value;
                const checked = this.checked;
                checked && selectedColumnsValue.push(value);
                columns[value] = checked;
            });

        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
        callback(columns);
    }

    getColumns() {
        const columns = {};
        this.dropdownColumn.fanexMultiSelect(
            "getItemsByCondition",
            '.visible-column > input[type="checkbox"]').each(function () {
                columns[this.value] = this.checked;
            });

        return columns;
    }
}