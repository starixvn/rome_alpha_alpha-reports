"use strict";
var VisibleColumnDropdownOption = /** @class */ (function () {
    function VisibleColumnDropdownOption(visibleColumnKey, listColumn, onDropdownHidden, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-column"; }
        this.visibleColumnKey = visibleColumnKey;
        this.listColumn = listColumn;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.visibleColumnKey = visibleColumnKey;
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
        var visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns === "undefined") {
            options.listColumn.forEach(function (item) {
                if (item.checked !== false) {
                    visibleColumnObject[item.value] = true;
                }
            });
        }
        else {
            var visibleColumnArray = visibleColumns.split(",");
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
            onLeave: function () { return _this.onDropdownHidden(options.visibleColumnKey, options.onDropdownHidden); }
        });
    };
    VisibleColumnDropdownComponent.prototype.onDropdownHidden = function (visibleColumnKey, callback) {
        var columns = this.getColumns();
        var selectedColumnsValue = [];
        Object.entries(columns).forEach(function (_a) {
            var key = _a[0], value = _a[1];
            return value && selectedColumnsValue.push(key);
        });
        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
        callback(columns);
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getAllValues");
    };
    VisibleColumnDropdownComponent.prototype.getHideColumns = function () {
        return this.dropdownColumn.fanexMultiSelect("getUncheckedValues");
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map