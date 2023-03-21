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
        this.$dropdownColumn = this.$("#" + options.selectorId);
        this.$dropdownColumn.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownColumn.data("label-select-columns"),
            nSelectedText: " " + this.$dropdownColumn.data("label-selected"),
            allSelectedText: this.$dropdownColumn.data("label-all-columns"),
            selectAllText: this.$dropdownColumn.data("label-select-all"),
            includeSelectAllOption: true,
            onDropdownHidden: function () { return _this.onDropdownHidden(options.visibleColumnKey, options.onDropdownHidden); }
        });
        var visibleColumns = alphaReports.localStorage.get(options.visibleColumnKey);
        if (visibleColumns) {
            this.$dropdownColumn.multiselect("select", visibleColumns.split(","));
        }
        else {
            this.$dropdownColumn.multiselect("selectAll", false);
            this.$dropdownColumn.multiselect("updateButtonText");
        }
        return this.getColumns();
    };
    VisibleColumnDropdownComponent.prototype.onDropdownHidden = function (visibleColumnKey, callback) {
        var _this = this;
        var selectedColumnsValue = [];
        var columns = {};
        this.$dropdownColumn.find("option").each(function (_, item) {
            var $this = _this.$(item);
            var value = $this.val();
            var checked = $this.is(":checked");
            checked && selectedColumnsValue.push(value);
            columns[value] = checked;
        });
        alphaReports.localStorage.set(visibleColumnKey, selectedColumnsValue.join());
        callback(columns);
    };
    VisibleColumnDropdownComponent.prototype.getColumns = function () {
        var _this = this;
        var columns = {};
        this.$dropdownColumn.find("option").each(function (_, item) {
            var $this = _this.$(item);
            columns[$this.val()] = $this.is(":checked");
        });
        return columns;
    };
    return VisibleColumnDropdownComponent;
}());
//# sourceMappingURL=visible-columns-dropdown.js.map