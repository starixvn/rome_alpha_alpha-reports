/// <reference path="../../../assets/scripts/core.ts" />
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var OutstandingViewNew = /** @class */ (function (_super) {
    __extends(OutstandingViewNew, _super);
    function OutstandingViewNew($, productMultiSelectionComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _this.cacheElements();
            _this.productMultiSelectionComponent.render();
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.isMobileView = JSON.parse(_this.$('#IsMobileView').val().toLowerCase());
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
            _this.initDataTable();
        };
        _this.onSubmitReport = function (e) {
            var this$ = _this.$;
            this$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                this$.unblockUI();
                e.preventDefault();
                return;
            }
            var order = _this.$table.order();
            _this.replaceHrefParam("SortPropertyName", _this.$table.column(order[0][0]).header());
            _this.replaceHrefParam("IsDescendingSort", order[0][1] !== 'asc');
            _this.$sortPropertyName.val(_this.$table.column(order[0][0]).header());
            _this.$isDescendingSort.val(order[0][1] !== 'asc' ? 'True' : 'False');
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        };
        _this.getParams = function () {
            var selectedProducts = _this.productMultiSelectionComponent.getSelectedItems();
            return {
                IsAnyProductSelected: selectedProducts.length > 0,
                IsFilterVisible: !_this.$reportForm.is(":hidden")
            };
        };
        _this.showValidateProductError = function () {
            _this.$buttonProduct.addClass("error");
        };
        _this.$ = $;
        _this.productMultiSelectionComponent = productMultiSelectionComponent;
        _this.linkBuilderComponent = linkBuilderComponent;
        return _this;
    }
    OutstandingViewNew.prototype.format = function (data) {
        var rows = [];
        for (var i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
            var level = viewModel.DisplayOutstandingLevels[i].DisplayName + "Outstanding";
            rows.push("<div><span style=\"float:left\">" + level + "</span><span>" + data[level] + "</span></div>");
        }
        return rows.join("\n");
    };
    OutstandingViewNew.prototype.initDataTable = function () {
        var that = this;
        var columns = [
            {
                "className": null,
                "orderable": true,
                "data": null,
                "defaultContent": '',
                "render": function (row) {
                    return "<a  href=\"" + row.DownlineLink + "\" data-custname=\"" + row.CustName + "\" class=\"downline-link\">" + row.CustName + "</a>";
                }
            },
            {
                "data": viewModel.DisplayOutstandingLevels[0].DisplayName + "Outstanding",
            },
            {
                "data": viewModel.DisplayOutstandingLevels[1].DisplayName + "Outstanding"
            }
        ];
        if (this.isMobileView) {
            if (JSON.parse(this.$('#showExpandIcon').val().toLowerCase())) {
                columns.push({
                    "className": 'details-control',
                    "orderable": false,
                    "data": null,
                    "defaultContent": '',
                    "render": function (row) {
                        return null;
                    }
                });
            }
        }
        else {
            for (var i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
                columns.push({
                    data: viewModel.DisplayOutstandingLevels[i].DisplayName + "Outstanding"
                });
            }
        }
        this.$table = this.$('.tblRpt').DataTable({
            "data": viewModel.OutstandingDatasource,
            "columns": columns,
            "order": [[0, 'asc']],
            "paging": false,
            "searching": false,
            "bInfo": false,
            "fixedHeader": {
                "header": true,
                "footer": true
            }
        });
        that.$('.tblRpt tbody').on('click', 'td.details-control', function () {
            var tr = that.$(this).closest('tr');
            var row = that.$table.row(tr);
            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                row.child(that.format(row.data()), "" + (tr.hasClass('odd') ? 'odd' : 'even')).show();
                tr.addClass('shown');
            }
        });
        that.$('.tblRpt tfoot').on('click', 'td.details-control', function () {
            var tr = that.$(this).closest('tr');
            if (tr.hasClass('shown')) {
                tr.removeClass('shown');
                tr.next().addClass('hide');
            }
            else {
                tr.addClass('shown');
                tr.next().removeClass('hide');
            }
        });
        that.$('.tblRpt').on('draw.dt', function () {
            that.$('td.details-control').each(function () {
                var tr = that.$(this).closest('tr');
                var row = that.$table.row(tr);
                if (row.child.isShown()) {
                    row.child(that.format(row.data()), "" + (tr.hasClass('odd') ? 'odd' : 'even')).show();
                }
            });
            that.$table.fixedHeader.adjust();
            var scrollTo = that.$('.fixedHeader-floating thead').length > 0 ? that.$(document).height() : 0;
            that.$(window).scrollTop(scrollTo);
            var order = that.$table.order();
            var title = that.$table.column(order[0][0]).header();
            that.replaceHrefParam("SortPropertyName", that.$(title).data('name'));
            that.replaceHrefParam("IsDescendingSort", order[0][1] !== 'asc');
        });
        that.$('.tblRpt thead').on('click', 'th.details-control', function () {
            var tr = that.$(this).closest('tr');
            if (tr.hasClass('shown')) {
                tr.removeClass('shown');
                that.$('tbody tr.shown').each(function () {
                    that.$(this).find('.details-control').click();
                });
                that.$('tfoot tr.shown').each(function () {
                    that.$(this).find('.details-control').click();
                });
            }
            else {
                tr.addClass('shown');
                that.$('tbody tr').not('.shown').each(function () {
                    that.$(this).find('.details-control').click();
                });
                that.$('tfoot tr').not('.shown').each(function () {
                    that.$(this).find('.details-control').click();
                });
            }
            that.$table.fixedHeader.adjust();
            var scrollTo = that.$('.fixedHeader-floating thead').length > 0 ? that.$(document).height() : 0;
            that.$(window).scrollTop(scrollTo);
        });
    };
    OutstandingViewNew.prototype.cacheElements = function () {
        _super.prototype.cacheElements.call(this);
        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$pageTitle = this.$(".page-title");
    };
    ;
    OutstandingViewNew.prototype.registerEvents = function () {
        _super.prototype.registerEvents.call(this);
        this.$reportForm.submit(this.onSubmitReport);
    };
    ;
    return OutstandingViewNew;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var outstandingViewNew = new OutstandingViewNew($, new ProductMultiSelectionComponent($), linkBuilderComponent);
    outstandingViewNew.init();
});
//# sourceMappingURL=outstanding-new.js.map