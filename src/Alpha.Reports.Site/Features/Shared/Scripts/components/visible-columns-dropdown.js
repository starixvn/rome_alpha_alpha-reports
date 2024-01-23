"use strict";
var VisibleColumnDropdownOption = /** @class */ (function () {
    function VisibleColumnDropdownOption(visibleColumns, listColumn, onDropdownHidden, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-column"; }
        this.visibleColumns = visibleColumns;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.visibleColumns = visibleColumns;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
    return VisibleColumnDropdownOption;
}());
var VisibleColumnDropdownComponent = /** @class */ (function () {
    function VisibleColumnDropdownComponent() {
    }
    VisibleColumnDropdownComponent.prototype.render = function (options) {
        var _this = this;
        var isCheckAll = false;
        var dropdown = $("#" + options.selectorId);
        var visibleColumnObject = {};
        if (options.visibleColumns.length) {
            var visibleColumnArray = options.visibleColumns.split(",");
            visibleColumnArray.forEach(function (item) {
                visibleColumnObject[item] = true;
            });
            isCheckAll = options.listColumn.length === visibleColumnArray.length;
        }
        else {
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
            onLeave: function () { return options.onDropdownHidden(_this.getColumns()); }
        });
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    };
    VisibleColumnDropdownComponent.prototype.getShowColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getCheckedValues");
    };
    VisibleColumnDropdownComponent.prototype.getHideColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getUncheckedValues");
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map