/// <reference path="../core.ts" />
"use strict";
var LeagueMultiSelectionOption = /** @class */ (function () {
    function LeagueMultiSelectionOption(dropdownId, validateBeforeSubmit, leaguesInputId, isUserSelectsAllLeaguesInputId) {
        if (dropdownId === void 0) { dropdownId = "dropdown-league"; }
        if (validateBeforeSubmit === void 0) { validateBeforeSubmit = true; }
        if (leaguesInputId === void 0) { leaguesInputId = "league-ids"; }
        if (isUserSelectsAllLeaguesInputId === void 0) { isUserSelectsAllLeaguesInputId = "IsUserSelectsAllLeagues"; }
        this.dropdownId = dropdownId;
        this.validateBeforeSubmit = validateBeforeSubmit;
        this.leaguesInputId = leaguesInputId;
        this.isUserSelectsAllLeaguesInputId = isUserSelectsAllLeaguesInputId;
    }
    return LeagueMultiSelectionOption;
}());
var LeagueMultiSelectionComponent = /** @class */ (function () {
    function LeagueMultiSelectionComponent($) {
        var _this = this;
        this.options = new LeagueMultiSelectionOption();
        this.render = function (options) {
            if (options === void 0) { options = new LeagueMultiSelectionOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            _this.$dropdownLeague = _this.$("#" + _this.options.dropdownId);
            var hasAnyLeague = alphaReports.convertAttributeToBool(_this.$dropdownLeague.data("has-any-league"));
            _this.$hiddenInput = _this.$("#" + _this.options.leaguesInputId);
            _this.$isUserSelectsAllLeagues = _this.$("#" + _this.options.isUserSelectsAllLeaguesInputId);
            _this.$dropdownLeague.multiselect({
                maxHeight: 300,
                numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
                nonSelectedText: hasAnyLeague
                    ? _this.$dropdownLeague.data("label-select-leagues")
                    : _this.$dropdownLeague.data("label-there-is-no-league"),
                nSelectedText: " " + _this.$dropdownLeague.data("label-selected"),
                allSelectedText: _this.$dropdownLeague.data("label-all-leagues"),
                includeSelectAllOption: hasAnyLeague,
                onChange: _this.onChange,
                onSelectAll: _this.onSelectAll,
                language: _this.$dropdownLeague.data("language"),
                enableCaseInsensitiveFiltering: true
            });
            _this.$multiSelectButton = _this.$dropdownLeague.parent().find(".multiselect.btn:not(#btn-loadleague)");
            return _this.$dropdownLeague;
        };
        this.onChange = function () {
            _this.validate();
            var selectedItems = _this.getSelectedItems();
            _this.$hiddenInput.val(selectedItems.length === 0 ? "-1" : selectedItems.join(","));
            _this.$isUserSelectsAllLeagues.val("false");
        };
        this.onSelectAll = function () {
            _this.validate();
            var selectedItems = _this.getSelectedItems();
            var isSelectAll = selectedItems.length === _this.$dropdownLeague.find("option").length;
            _this.$hiddenInput.val(isSelectAll
                ? ""
                : selectedItems.length
                    ? selectedItems.join(",")
                    : "-1");
            _this.$isUserSelectsAllLeagues.val(isSelectAll.toString());
        };
        this.validate = function () {
            if (_this.options.validateBeforeSubmit
                && _this.$multiSelectButton
                && _this.$dropdownLeague.find("option:selected").length
                && _this.$multiSelectButton.hasClass("error")) {
                _this.$multiSelectButton.removeClass("error");
            }
        };
        this.getSelectedItems = function () {
            var selected = [];
            var selectedOptions = _this.$dropdownLeague.find("option:selected");
            var this$ = _this.$;
            selectedOptions.each(function () {
                selected.push([this$(this).val()]);
            });
            return selected;
        };
        this.$ = $;
    }
    return LeagueMultiSelectionComponent;
}());
//# sourceMappingURL=league-multiselection.js.map