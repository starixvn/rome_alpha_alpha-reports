"use strict";
var BonusDetail = /** @class */ (function () {
    function BonusDetail($, topWindow, dateRangePickerComponent, pagerComponent) {
        var _this = this;
        this.onClickIconFilter = function () {
            var isHide = _this.$reportForm.hasClass("hide");
            _this.$isFilterVisible.val(isHide.toString());
            if (isHide) {
                _this.$pageTitle.removeClass("hide-filter");
                _this.$reportForm.removeClass("hide");
            }
            else {
                _this.$pageTitle.addClass("hide-filter");
                _this.$reportForm.addClass("hide");
            }
        };
        this.onSubmitReport = function () {
            _this.$.blockUI();
            return true;
        };
        this.$ = $;
        this.topWindow = topWindow;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }
    BonusDetail.prototype.init = function () {
        this.$.blockUI();
        this.cacheElements();
        this.initComponents();
        this.bindEvents();
        this.$.unblockUI();
    };
    BonusDetail.prototype.cacheElements = function () {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
    };
    BonusDetail.prototype.initComponents = function () {
        var _this = this;
        this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", null, function () { return _this.$("#PageIndex").val("1"); }));
        var dropdownOptions = { numberDisplayed: 1 };
        this.$("#dropdown-statusId").multiselect(dropdownOptions);
        this.$("#dropdown-isComplained").multiselect(dropdownOptions);
        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
        this.$("#tbl-report").stickyTableHeaders();
    };
    BonusDetail.prototype.bindEvents = function () {
        var _this = this;
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$reportForm.submit(this.onSubmitReport);
        this.$("#btn-submit").click(function () { return _this.$("#PageIndex").val("1"); });
        var $popupConfirmPayment = this.$("#popup-confirmPayment");
        if ($popupConfirmPayment.length) {
            new ConfirmPayment(this.$, new Alert(this.$), function () { return _this.$reportForm.submit(); }, $popupConfirmPayment).init();
        }
    };
    return BonusDetail;
}());
$(function () {
    new BonusDetail($, window.top, new DateRangePickerComponent($), new PagerComponent($)).init();
});
//# sourceMappingURL=bonus-detail.js.map