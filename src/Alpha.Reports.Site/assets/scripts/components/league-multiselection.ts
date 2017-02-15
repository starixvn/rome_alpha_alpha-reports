/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class LeagueMultiSelectionOption {
    constructor(
        public dropdownId: string = "dropdown-league",
        public validateBeforeSubmit: boolean = true,
        public leaguesInputId: string = "league-ids") {
    }
}

class LeagueMultiSelectionComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdownLeague: JQuery;
    $multiSelectButton: JQuery;

    options = new LeagueMultiSelectionOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: LeagueMultiSelectionOption = new LeagueMultiSelectionOption()) => {
        this.options = <LeagueMultiSelectionOption>this.$.extend({}, this.options, options);

        this.$dropdownLeague = this.$("#" + this.options.dropdownId);
        let hasAnyLeague = alphaReports.convertAttributeToBool(this.$dropdownLeague.data("has-any-league"));
        this.$hiddenInput = this.$("#" + this.options.leaguesInputId);

        this.$dropdownLeague.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: hasAnyLeague
                ? this.$dropdownLeague.data("label-select-leagues")
                : this.$dropdownLeague.data("label-there-is-no-league"),
            nSelectedText: ` ${this.$dropdownLeague.data("label-selected")}`,
            allSelectedText: this.$dropdownLeague.data("label-all-leagues"),
            includeSelectAllOption: hasAnyLeague,
            onChange: this.onChange,
            onSelectAll: this.onChange,
            language: this.$dropdownLeague.data("language"),
            enableCaseInsensitiveFiltering: true
        });

        this.$multiSelectButton = this.$dropdownLeague.parent().find(".multiselect.btn");

        return this.$dropdownLeague;
    };

    onChange = (option: any, checked: boolean) => {
        this.validate();
        this.$hiddenInput.val(this.getSelectedItems().join(","));
    };

    validate = () => {
        if (this.options.validateBeforeSubmit
            && this.$multiSelectButton
            && this.$dropdownLeague.find("option:selected").length
            && this.$multiSelectButton.hasClass("error")) {
            this.$multiSelectButton.removeClass("error");
        }
    };

    getSelectedItems = () => {
        let selected = [];
        let selectedOptions = this.$dropdownLeague.find("option:selected");
        let this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected;
    };
}