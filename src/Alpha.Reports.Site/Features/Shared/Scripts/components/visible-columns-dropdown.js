"use strict";
var VisibleColumnDropdownOption = /** @class */ (function () {
    function VisibleColumnDropdownOption(visibleColumnKey, onDropdownHidden, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-column"; }
        this.visibleColumnKey = visibleColumnKey;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
        this.visibleColumnKey = visibleColumnKey;
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
        var isCheckAll = true;
        var dropdown = $("#" + options.selectorId);
        var dropdownOptions = dropdown.find("option");
        var visibleColumnObject = {};
        var visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns !== "undefined") {
            var visibleColumnArray = visibleColumns.split(",");
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
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map