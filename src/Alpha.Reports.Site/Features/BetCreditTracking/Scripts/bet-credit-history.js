"use strict";
var BetCreditHistory = /** @class */ (function () {
    function BetCreditHistory($, dateRangePickerComponent, pagerComponent) {
        var _this = this;
        this.MAX_LENGTH_REFNO = 19;
        this.resetPaging = function () {
            _this.$("#PageIndex").val("1");
            _this.$("#PageSize").val("100");
        };
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
        this.onExportExcel = function () {
            _this.$.blockUI();
            if (!_this.validateUsername()) {
                _this.showValidationMsgError(_this.resources.usernameIsRequired, _this.$txtMember);
                _this.$.unblockUI();
                return;
            }
            _this.hideValidationMsgError();
            _this.$exportExcelForm.submit();
            _this.$.unblockUI();
        };
        this.onSubmitReport = function (e) {
            _this.$.blockUI();
            if (!_this.validateUsername()) {
                _this.showValidationMsgError(_this.resources.usernameIsRequired, _this.$txtMember);
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            if (!_this.validateRefNo()) {
                _this.showValidationMsgError(_this.resources.invalidRefNo, _this.$txtRefNo);
                _this.$.unblockUI();
                e.preventDefault();
                return;
            }
            _this.hideValidationMsgError();
            return true;
        };
        this.validateUsername = function () { return _this.$txtMember.val() !== ""; };
        this.validateRefNo = function () { return _this.$txtRefNo.val().length <= _this.MAX_LENGTH_REFNO && _this.$txtRefNo.val().trim().match(/^[0-9]*$/); };
        this.showValidationMsgError = function (msg, element) {
            TipsyHelper.showError(element, msg, "nw");
            _this.$("#tbl-report").remove();
            _this.$("#report-pager").remove();
        };
        this.hideValidationMsgError = function () {
            TipsyHelper.clear(_this.$txtMember);
            TipsyHelper.clear(_this.$txtRefNo);
        };
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.pagerComponent = pagerComponent;
    }
    BetCreditHistory.prototype.init = function () {
        this.$.blockUI();
        this.cacheElements();
        this.initComponents();
        this.bindEvents();
        var $errMsg = this.$("#ErrMsg");
        $errMsg.length && TipsyHelper.showError(this.$txtMember, $errMsg.val(), "nw");
        this.$.unblockUI();
    };
    BetCreditHistory.prototype.cacheElements = function () {
        this.$reportForm = this.$("#report-form");
        this.$pageTitle = this.$(".page-title");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$txtMember = this.$("#txt-member");
        this.$txtRefNo = this.$("#txt-refno");
        this.$msgError = this.$("#msg-error");
        this.resources = {
            usernameIsRequired: this.$('#UsernameIsRequired').val(),
            invalidRefNo: this.$('#InvalidRefNo').val(),
        };
    };
    BetCreditHistory.prototype.initComponents = function () {
        this.dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", null));
        this.pagerComponent.pageList = [10, 50, 100, 500];
        this.pagerComponent.render();
        this.$("#tbl-report").stickyTableHeaders();
    };
    BetCreditHistory.prototype.bindEvents = function () {
        var _this = this;
        this.$(".icon-filter").click(this.onClickIconFilter);
        this.$txtMember.on("input", function () { return TipsyHelper.clear(_this.$txtMember); });
        this.$txtRefNo.on("input", function () { return TipsyHelper.clear(_this.$txtRefNo); });
        this.$reportForm.submit(this.onSubmitReport);
        $('input[type="submit"]').click(function () {
            _this.resetPaging();
        });
        $(".range-option").click(function () {
            _this.resetPaging();
        });
        this.$("#icon-excel").click(this.onExportExcel);
    };
    return BetCreditHistory;
}());
$(function () {
    var betCreditHistory = new BetCreditHistory($, new DateRangePickerComponent($), new PagerComponent($));
    betCreditHistory.init();
});
//# sourceMappingURL=bet-credit-history.js.map