/// <reference path="../core.ts" />
"use strict";
var DatePickerOption = /** @class */ (function () {
    function DatePickerOption(minDate, allowInputToggle) {
        if (allowInputToggle === void 0) { allowInputToggle = false; }
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
    }
    return DatePickerOption;
}());
var RiskAnalysisIPMatching = /** @class */ (function () {
    function RiskAnalysisIPMatching($) {
        var _this = this;
        this.hasAllDataOnClient = false;
        this.init = function () {
            _this.$.blockUI();
            _this.$("SELECT").on("change", function (event) {
                _this.$('#MemberCount').val(event.target.value);
            });
            var options = new DatePickerOption(_this.$('#MinDate').val(), true);
            _this.$('#dpkContainer').datepicker(options).data("DateTimePicker").maxDate(_this.$('#Today').val());
            _this.$('#dpkContainer').on("dp.change", function () {
                _this.$('#SelectedWinlostDate').val(_this.$('#WinlostDate').val());
            });
            _this.$("#icon-excel").on("click", function () {
                _this.$("#export-excel-form").submit();
            });
            _this.$(".icon-filter").on("click", function () {
                var $reportForm = _this.$("#report-form");
                var $pageTitle = _this.$(".page-title");
                var currentHideState = $reportForm.hasClass("hide");
                if (currentHideState) {
                    $pageTitle.removeClass("hide-filter");
                    $reportForm.removeClass("hide");
                }
                else {
                    $pageTitle.addClass("hide-filter");
                    $reportForm.addClass("hide");
                }
            });
            _this.$(".view-bet-list").on("click", function () {
                _this.$.blockUI();
            });
            _this.$.unblockUI();
        };
        this.$ = $;
    }
    return RiskAnalysisIPMatching;
}());
$(function () {
    var riskAnalysisIPMatching = new RiskAnalysisIPMatching($);
    riskAnalysisIPMatching.init();
});
//# sourceMappingURL=index.js.map