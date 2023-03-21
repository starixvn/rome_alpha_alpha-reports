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
    private $dropdownColumn: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(options: VisibleColumnDropdownOption) {
        this.$dropdownColumn = this.$("#" + options.selectorId);
        this.$dropdownColumn.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownColumn.data("label-select-columns"),
            nSelectedText: ` ${this.$dropdownColumn.data("label-selected")}`,
            allSelectedText: this.$dropdownColumn.data("label-all-columns"),
            selectAllText: this.$dropdownColumn.data("label-select-all"),
            includeSelectAllOption: true,
            onDropdownHidden: () => this.onDropdownHidden(options.visibleColumnKey, options.onDropdownHidden)
        });

        const visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (visibleColumns) {
            this.$dropdownColumn.multiselect("select", visibleColumns.split(","));
        } else {
            this.$dropdownColumn.multiselect("selectAll", false)
            this.$dropdownColumn.multiselect("updateButtonText");
        }

        return this.getColumns();
    }

    private onDropdownHidden(visibleColumnKey: string, callback: Function) {
        const selectedColumnsValue = [];
        const columns = {};
        this.$dropdownColumn.find("option").each((_, item) => {
            const $this = this.$(item);
            const value = $this.val();
            const checked = $this.is(":checked");
            checked && selectedColumnsValue.push(value);
            columns[value] = checked;
        });

        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
        callback(columns);
    }

    getColumns() {
        const columns = {};
        this.$dropdownColumn.find("option").each((_, item) => {
            const $this = this.$(item);
            columns[$this.val()] = $this.is(":checked");
        });

        return columns;
    }
}