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
var OutstandingViewNew1 = /** @class */ (function (_super) {
    __extends(OutstandingViewNew1, _super);
    function OutstandingViewNew1($, productMultiSelectionComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.init = function () {
            _this.$.blockUI();
            _this.cacheElements();
            _this.productMultiSelectionComponent.render();
            _this.$buttonProduct = _this.$("#dropdown-product").parent().find(".multiselect.btn");
            _this.canSort = JSON.parse(_this.$('#CanSort').val().toLowerCase());
            _this.initDataTable();
            _this.linkBuilderComponent.render();
            _this.registerEvents();
            _this.initHelp();
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
            var self = _this;
            var colunmSortIndex = 0;
            _this.$.each(displayOutstandingLevels, function (key, value) {
                if (_this.$(_this.$sortPropertyName).val().toLowerCase() === (value.DisplayName + "Outstanding").toLowerCase()) {
                    colunmSortIndex = key + 1;
                    return;
                }
            });
            _this.$table = _this.$('#example').DataTable({
                responsive: {
                    details: {
                        type: 'column',
                        target: -1,
                        renderer: function (api, rowIdx) {
                            var data = api.cells(rowIdx, ':hidden').eq(0).map(function (cell) {
                                var header = self.$(api.column(cell.column).header());
                                var idx = api.cell(cell).index();
                                if (header.hasClass('control') || header.hasClass('never')) {
                                    return '';
                                }
                                var dtPrivate = api.settings()[0];
                                var cellData = dtPrivate.oApi._fnGetCellData(dtPrivate, idx.row, idx.column, 'display');
                                var title = header.text();
                                return "<li data-dtr-index=\"" + idx.column + "\">" +
                                    ("<span class=\"dtr-title\">" + title + "</span> ") +
                                    ("<span class=\"dtr-data\">" + cellData + "</span></li>");
                            }).toArray().join('');
                            return data ?
                                self.$('<ul data-dtr-index="' + rowIdx + '"/>').append(data) :
                                false;
                        }
                    }
                },
                columnDefs: [{
                        className: 'control',
                        orderable: false,
                        targets: -1
                    }],
                ordering: _this.canSort,
                order: [[colunmSortIndex, JSON.parse(_this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']],
                paging: false,
                searching: false,
                bInfo: false,
                fixedHeader: true,
                initComplete: function () { _this.$.unblockUI(); },
                drawCallback: function () { _this.$('tbody').append(_this.$('tr.tbl-footer')); }
            });
            _this.$('#example thead').on('click', 'th.control', function (event) { return _this.onClickDetailControlOnHeader(event); });
            _this.$('#example tbody').on('click', 'td.control', function (event) { return _this.onClickDetailControlOnBody(event); });
            _this.$('#example tbody tr.tbl-footer').on('click', 'td.control', function () { return _this.onClickDetailControlOnFooter(); });
            _this.$('#example').on('draw.dt', function () { return _this.triggerRedrawTable(); });
        };
        _this.onClickDetailControlOnHeader = function (event) {
            var tr = _this.$(event.currentTarget).closest('tr');
            if (tr.hasClass('parent')) {
                tr.removeClass('parent');
                _this.$('tbody tr.parent').each(function (index, element) {
                    _this.$(element).find('.control').click();
                });
            }
            else {
                tr.addClass('parent');
                _this.$('tbody tr').not('.parent').each(function (index, element) {
                    _this.$(element).find('.control').click();
                });
            }
            _this.doAdjustHeader();
        };
        _this.onClickDetailControlOnBody = function (event) {
            var tr = _this.$(event.currentTarget).closest('tr');
            if (tr.hasClass('tbl-footer')) {
                tr.hasClass('parent') ?
                    _this.$(tr).next().addClass('tbl-footer') :
                    _this.$(tr).next().removeClass('tbl-footer');
            }
            else {
                var rowNext = _this.$(tr).next();
                if (rowNext.hasClass('child')) {
                    if (tr.hasClass('even')) {
                        _this.$(tr).next().removeClass('odd').addClass('even');
                    }
                    else {
                        _this.$(tr).next().removeClass('even').addClass('odd');
                    }
                }
            }
        };
        _this.onClickDetailControlOnFooter = function () {
            setTimeout(function () {
                var scrollTo = _this.$(document).height();
                _this.$(window).scrollTop(scrollTo + 15);
            }, 100);
        };
        _this.triggerRedrawTable = function () {
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
            var length = displayOutstandingLevels.length;
            for (var index = 0; index < length; index++) {
                if (order[0][0] - 1 === index) {
                    return displayOutstandingLevels[index].DisplayName + "Outstanding";
                }
            }
            return 'CustName';
        };
        _this.isDescendingSort = function () {
            var order = _this.$table.order();
            return order[0][1] !== 'asc';
        };
        _this.onSubmitReport = function (e) {
            _this.$.blockUI();
            var params = _this.getParams();
            if (!params.IsAnyProductSelected) {
                _this.showValidateProductError();
                _this.$.unblockUI();
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
    return OutstandingViewNew1;
}(BaseReport));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var outstandingViewNew1 = new OutstandingViewNew1($, new ProductMultiSelectionComponent($), linkBuilderComponent);
    outstandingViewNew1.init();
});
//# sourceMappingURL=outstanding-new-01.js.map