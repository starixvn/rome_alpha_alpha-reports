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
        _this.cacheElements = function () {
            _super.prototype.cacheElements.call(_this);
            _this.$reportForm = _this.$("#report-form");
            _this.$sortPropertyName = _this.$("#SortPropertyName");
            _this.$isDescendingSort = _this.$("#IsDescendingSort");
            _this.$isFilterVisible = _this.$("#IsFilterVisible");
        };
        _this.registerEvents = function () {
            _super.prototype.registerEvents.call(_this);
            _this.$reportForm.submit(_this.onSubmitReport);
        };
        _this.initDataTable = function () {
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
                    "render": _this.$.fn.dataTable.render.number(',', '.', 2, '')
                },
                {
                    "data": viewModel.DisplayOutstandingLevels[1].DisplayName + "Outstanding",
                    "render": _this.$.fn.dataTable.render.number(',', '.', 2, '')
                }
            ];
            if (_this.isMobileView) {
                if (JSON.parse(_this.$('#showExpandIcon').val().toLowerCase())) {
                    columns.push({
                        "className": 'details-control col-60',
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
                        "data": viewModel.DisplayOutstandingLevels[i].DisplayName + "Outstanding",
                        "render": _this.$.fn.dataTable.render.number(',', '.', 2, '')
                    });
                }
            }
            var colunmSortIndex = 0;
            _this.$.each(viewModel.DisplayOutstandingLevels, function (key, value) {
                if (_this.$(_this.$sortPropertyName).val().toLowerCase() === (value.DisplayName + "Outstanding").toLowerCase()) {
                    colunmSortIndex = key + 1;
                    return;
                }
            });
            _this.$table = _this.$('.tblRpt').DataTable({
                "data": viewModel.OutstandingDatasource,
                "columns": columns,
                "order": [[colunmSortIndex, JSON.parse(_this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']],
                "paging": false,
                "searching": false,
                "bInfo": false,
                "fixedHeader": {
                    "header": true,
                    "footer": true
                }
            });
            _this.$('.tblRpt thead').on('click', 'th.details-control', function (event) { return _this.onClickDetailControlOnHeader(event); });
            _this.$('.tblRpt tbody').on('click', 'td.details-control', function (event) { return _this.onClickDetailControlOnBody(event, true); });
            _this.$('.tblRpt tfoot').on('click', 'td.details-control', function (event) { return _this.onClickDetailControlOnFooter(event); });
            _this.$('.tblRpt').on('draw.dt', function () { return _this.triggerRedrawTable(); });
        };
        _this.formatDetailRow = function (data, parentRow) {
            var rows = [];
            var heigh = _this.$(parentRow).height();
            for (var i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
                var level = viewModel.DisplayOutstandingLevels[i].DisplayName + "Outstanding";
                rows.push("<div class=\"child-row-content\"><span>" + viewModel.DisplayOutstandingLevels[i].DisplayName + "</span><span>" + data[level] + "</span></div>");
            }
            return rows.join("\n");
        };
        _this.onClickDetailControlOnHeader = function (event) {
            var tr = _this.$(event.currentTarget).closest('tr');
            if (tr.hasClass('shown')) {
                tr.removeClass('shown');
                _this.$('tbody tr.shown').each(function (index, element) {
                    _this.$(element).find('.details-control').click();
                });
                _this.$('tfoot tr.shown').each(function (index, element) {
                    _this.$(element).find('.details-control').click();
                });
            }
            else {
                tr.addClass('shown');
                _this.$('tbody tr').not('.shown').each(function (index, element) {
                    _this.$(element).find('.details-control').click();
                });
                _this.$('tfoot tr').not('.shown').each(function (index, element) {
                    _this.$(element).find('.details-control').click();
                });
            }
            _this.doAdjustHeader();
        };
        _this.onClickDetailControlOnBody = function (event, adjustHeader) {
            if (adjustHeader === void 0) { adjustHeader = false; }
            var tr = _this.$(event.currentTarget).closest('tr');
            var row = _this.$table.row(tr);
            if (row.child.isShown()) {
                row.child.hide();
                tr.removeClass('shown');
            }
            else {
                row.child(_this.formatDetailRow(row.data(), tr), "child-row " + (tr.hasClass('odd') ? 'odd' : 'even')).show();
                tr.addClass('shown');
            }
            if (adjustHeader) {
                _this.doAdjustHeader();
            }
            else {
                _this.$(window).scrollTop(_this.$(window).scrollTop() + 1);
            }
        };
        _this.onClickDetailControlOnFooter = function (event) {
            var tr = _this.$(event.currentTarget).closest('tr');
            if (tr.hasClass('shown')) {
                tr.removeClass('shown');
                tr.next().addClass('hide');
            }
            else {
                tr.addClass('shown');
                tr.next().removeClass('hide');
            }
        };
        _this.triggerRedrawTable = function () {
            _this.$('td.details-control').each(function (index, element) {
                var tr = _this.$(element).closest('tr');
                var row = _this.$table.row(tr);
                if (row.child.isShown()) {
                    row.child(_this.formatDetailRow(row.data(), tr), "child-row " + (tr.hasClass('odd') ? 'odd' : 'even')).show();
                }
            });
            _this.doAdjustHeader();
            _this.replaceHrefParam("SortPropertyName", _this.getColumnSortName());
            _this.replaceHrefParam("IsDescendingSort", _this.isDescendingSort());
        };
        _this.doAdjustHeader = function () {
            _this.$table.fixedHeader.adjust();
            var scrollTo = _this.$('.fixedHeader-floating thead').length > 0 ? _this.$(document).height() : 0;
            _this.$(window).scrollTop(scrollTo);
        };
        _this.getColumnSortName = function () {
            var order = _this.$table.order();
            var length = _this.isMobileView ? 1 : viewModel.DisplayOutstandingLevels.length;
            for (var index = 0; index < length; index++) {
                if (order[0][0] - 1 === index) {
                    return viewModel.DisplayOutstandingLevels[index].DisplayName + "Outstanding";
                }
            }
            return 'CustName';
        };
        _this.isDescendingSort = function () {
            var order = _this.$table.order();
            return order[0][1] !== 'asc';
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
            _this.replaceHrefParam("SortPropertyName", _this.getColumnSortName());
            _this.replaceHrefParam("IsDescendingSort", _this.isDescendingSort());
            _this.$sortPropertyName.val(_this.getColumnSortName());
            _this.$isDescendingSort.val(_this.isDescendingSort ? 'True' : 'False');
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
    return OutstandingViewNew;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var outstandingViewNew = new OutstandingViewNew($, new ProductMultiSelectionComponent($), linkBuilderComponent);
    outstandingViewNew.init();
});
//# sourceMappingURL=outstanding-new.js.map