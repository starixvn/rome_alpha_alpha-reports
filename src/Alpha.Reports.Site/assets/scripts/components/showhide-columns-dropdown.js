"use strict";
var ShowHideColumnDropdownOption = /** @class */ (function () {
    function ShowHideColumnDropdownOption(onDropdownHidden, showHideColumnsKey, selectorId) {
        if (onDropdownHidden === void 0) { onDropdownHidden = Function(); }
        if (selectorId === void 0) { selectorId = "dropdown-columns"; }
        this.onDropdownHidden = onDropdownHidden;
        this.showHideColumnsKey = showHideColumnsKey;
        this.selectorId = selectorId;
        this.onDropdownHidden = onDropdownHidden;
        this.selectorId = selectorId;
    }
    return ShowHideColumnDropdownOption;
}());
var ShowHideColumnDropdownComponent = /** @class */ (function () {
    function ShowHideColumnDropdownComponent($) {
        this.$ = $;
    }
    ShowHideColumnDropdownComponent.prototype.render = function (showHideColumnDropdownOption) {
        var _this = this;
        this.$dropdownColumn = this.$("#" + showHideColumnDropdownOption.selectorId);
        this.$dropdownColumn.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownColumn.data("label-select-columns"),
            nSelectedText: " " + this.$dropdownColumn.data("label-selected"),
            allSelectedText: this.$dropdownColumn.data("label-all-columns"),
            selectAllText: this.$dropdownColumn.data("label-select-all"),
            includeSelectAllOption: true,
            onDropdownHidden: function () { return _this.onDropdownHidden(showHideColumnDropdownOption.showHideColumnsKey, showHideColumnDropdownOption.onDropdownHidden); }
        });
        var showHideColumns = alphaReports.localStorage.get(showHideColumnDropdownOption.showHideColumnsKey);
        if (showHideColumns) {
            this.$dropdownColumn.multiselect("select", showHideColumns.split(","));
        }
        else {
            this.$dropdownColumn.multiselect("selectAll", false);
            this.$dropdownColumn.multiselect('updateButtonText');
        }
        var columns = {};
        this.$dropdownColumn.find("option").each(function (_, item) {
            var $item = _this.$(item);
            columns[$item.data("columnindex")] = $item.is(":checked");
        });
        return columns;
    };
    ShowHideColumnDropdownComponent.prototype.onDropdownHidden = function (showHideColumnsKey, callback) {
        var _this = this;
        var selectedColumnsValue = [];
        var columns = {};
        this.$dropdownColumn.find("option").each(function (_, item) {
            var $item = _this.$(item);
            var checked = $item.is(":checked");
            checked && selectedColumnsValue.push($item.val());
            columns[$item.data("columnindex")] = checked;
        });
        alphaReports.localStorage.set(showHideColumnsKey, selectedColumnsValue.join());
        callback(columns);
    };
    ShowHideColumnDropdownComponent.prototype.getColumns = function () {
        var _this = this;
        var columns = {};
        this.$dropdownColumn.find("option").each(function (_, item) {
            var $item = _this.$(item);
            columns[$item.data("columnindex")] = $item.is(":checked");
        });
        return columns;
    };
    return ShowHideColumnDropdownComponent;
}());
//# sourceMappingURL=showhide-columns-dropdown.js.map