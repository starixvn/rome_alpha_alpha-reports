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
    function VisibleColumnDropdownComponent($) {
        this.$ = $;
    }
    VisibleColumnDropdownComponent.prototype.render = function (options) {
        var _this = this;
        var isCheckAll = true;
        var ddlColumn = this.$("#" + options.selectorId);
        var ddlColumnOptions = ddlColumn.find("option");
        var visibleColumnObject = {};
        var visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (typeof visibleColumns !== "undefined") {
            var visibleColumnArray = visibleColumns.split(",");
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
            minWidth: 250,
            //maxHeight: 250,
            listData: ddlColumnOptions.map(function () {
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
        return this.getColumns();
    };
    VisibleColumnDropdownComponent.prototype.onDropdownHidden = function (visibleColumnKey, callback) {
        var selectedColumnsValue = [];
        var columns = {};
        this.dropdownColumn.fanexMultiSelect("getItemsByCondition", '.visible-column > input[type="checkbox"]').each(function () {
            var value = this.value;
            var checked = this.checked;
            checked && selectedColumnsValue.push(value);
            columns[value] = checked;
        });
        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
        callback(columns);
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        var columns = {};
        this.dropdownColumn.fanexMultiSelect("getItemsByCondition", '.visible-column > input[type="checkbox"]').each(function () {
            columns[this.value] = this.checked;
        });
        return columns;
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map