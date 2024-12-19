/// <reference path="../core.ts" />
"use strict";
var ClientPagerOption = /** @class */ (function () {
    function ClientPagerOption(onSelectPage, pagerId, formId, pageIndexId, pageSizeId, tableReportId) {
        if (onSelectPage === void 0) { onSelectPage = null; }
        if (pagerId === void 0) { pagerId = "report-pager"; }
        if (formId === void 0) { formId = "report-form"; }
        if (pageIndexId === void 0) { pageIndexId = "PageIndex"; }
        if (pageSizeId === void 0) { pageSizeId = "PageSize"; }
        if (tableReportId === void 0) { tableReportId = "tbl-report"; }
        this.onSelectPage = onSelectPage;
        this.pagerId = pagerId;
        this.formId = formId;
        this.pageIndexId = pageIndexId;
        this.pageSizeId = pageSizeId;
        this.tableReportId = tableReportId;
    }
    return ClientPagerOption;
}());
var ClientPagerComponent = /** @class */ (function () {
    function ClientPagerComponent($) {
        var _this = this;
        this.pageList = [1000, 2000, 3000, 5000];
        this.smallPageList = [10, 50, 100, 500];
        this.options = new ClientPagerOption();
        this.render = function (options) {
            if (options === void 0) { options = new ClientPagerOption(); }
            var pagerOption = _this.$.extend({}, _this.options, options);
            var pager = _this.$("#" + pagerOption.pagerId);
            _this.$reportForm = _this.$("#" + _this.options.formId);
            _this.$bodyRows = _this.$("#" + _this.options.tableReportId + " > tbody > tr");
            _this.totalRecords = parseInt(pager.data("totalrecords"), 0);
            _this.pageSize = parseInt(pager.data("pagesize"), 0);
            _this.pageIndex = parseInt(pager.data("pageindex"), 0);
            _this.$pageIndex =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageIndexId + "\" name=\"" + pagerOption.pageIndexId + "\" value=\"" + _this.pageIndex + "\" />");
            _this.$pageSize =
                _this.$("<input type=\"hidden\" id=\"" + pagerOption.pageSizeId + "\" name=\"" + pagerOption.pageSizeId + "\" value=\"" + _this.pageSize + "\" />");
            _this.$reportForm.append(_this.$pageIndex).append(_this.$pageSize);
            pager.pagination({
                pageList: _this.pageList,
                total: _this.totalRecords,
                pageSize: _this.pageSize,
                pageNumber: _this.pageIndex,
                showRefresh: false,
                onSelectPage: _this.onSelectPage,
                beforePageText: pager.data("labelpage"),
                afterPageText: pager.data("labelof") + " {pages}",
                displayMsg: pager.data("labeldisplayitems")
            });
            _this.showBodyRows();
            pager.removeClass("hide");
            return pager;
        };
        this.onSelectPage = function (pageNumber, pageSize) {
            _this.updateHiddenFields(pageNumber, pageSize);
            _this.showBodyRows();
        };
        this.updateHiddenFields = function (pageNumber, pageSize) {
            _this.$pageIndex.val(pageNumber);
            _this.$pageSize.val(pageSize);
            _this.pageSize = pageSize;
            _this.pageIndex = pageNumber;
        };
        this.showBodyRows = function () {
            if (!_this.totalRecords) {
                return;
            }
            var startIndex = _this.pageSize * (_this.pageIndex - 1);
            var endIndex = startIndex + _this.pageSize;
            _this.$bodyRows.addClass("hide");
            for (var i = startIndex; i < endIndex && i < _this.totalRecords; i++) {
                _this.$(_this.$bodyRows[i]).removeClass("hide");
            }
        };
        this.$ = $;
    }
    return ClientPagerComponent;
}());
//# sourceMappingURL=client-pager.js.map