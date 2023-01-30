var BaseReportNew = /** @class */ (function () {
    function BaseReportNew($, linkBuilderComponent) {
        var _this = this;
        this.isClickedCollapseExpandAll = false;
        this.initHelp = function () {
            _this.$loadLegendURL = _this.$("#LoadLegendURL");
            _this.$iconInformation = _this.$(".icon-information");
            _this.$iconInformationContainer = _this.$(".icon-information-container");
            _this.loadLegend();
            _this.$iconInformation.on("click", _this.showLegend);
        };
        this.loadLegend = function () {
            _this.$.ajax({
                method: "GET",
                url: _this.$loadLegendURL.val(),
                cache: false,
                data: {},
                success: _this.renderLegend,
                complete: _this.$.unblockUI
            });
        };
        this.renderLegend = function (html) {
            if (_this.$reportLegend) {
                _this.$reportLegend.remove();
            }
            _this.$("body").append(html);
            if (_this.$.trim(html) === "") {
                _this.$iconInformationContainer.remove();
            }
            else {
                _this.$iconInformationContainer.removeClass("hide");
            }
            _this.$reportLegend = _this.$("#report-legend");
        };
        this.showLegend = function () {
            _this.$reportLegend.modal({ show: true, backdrop: false });
            var iconHelpPosition = _this.$iconInformation.position();
            _this.$reportLegend.css({
                bottom: "auto",
                top: _this.$(".entered-non-entered").length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
            });
        };
        this.isDescendingSort = function () { return _this.$dataTable.order()[0][1] !== 'asc'; };
        this.doAdjustHeader = function () {
            _this.$dataTable.fixedHeader.adjust();
            var scrollTo = _this.$('.fixedHeader-floating thead').length > 0 ? _this.$(document).height() : 0;
            _this.$(window).scrollTop(scrollTo);
        };
        this.onClickFooterRow = function (parentRow, childRow) {
            parentRow.hasClass('parent') ?
                childRow.addClass('tbl-footer') :
                childRow.removeClass('tbl-footer');
            if (_this.isClickedCollapseExpandAll) {
                _this.isClickedCollapseExpandAll = false;
                return;
            }
            setTimeout(function () {
                var scrollTo = _this.$(document).height();
                _this.$(window).scrollTop(scrollTo + 15);
            }, 100);
        };
        this.onClickBodyRow = function (parentRow, childRow) {
            if (childRow.hasClass('child')) {
                parentRow.hasClass('even') ?
                    childRow.removeClass('odd').addClass('even') :
                    childRow.removeClass('even').addClass('odd');
            }
        };
        this.showFilter = function () {
            _this.$pageTitle.removeClass("hide-filter");
            _this.$reportForm.removeClass("hide");
        };
        this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        this.replaceHrefParam = function (paramName, paramValue) {
            _this.linkBuilderComponent.updateParam(paramName, paramValue);
        };
        this.collapseExpandAll = function (element) {
            _this.$.blockUI();
            _this.isClickedCollapseExpandAll = true;
            setTimeout(function () {
                var targetParent = _this.$(element).parent();
                if (targetParent.hasClass("parent")) {
                    targetParent.removeClass("parent");
                    _this.triggerClickDetailWhenSwitchAll(_this.$('tbody tr.parent'));
                }
                else {
                    targetParent.addClass("parent");
                    _this.triggerClickDetailWhenSwitchAll(_this.$('tbody tr').not(".parent"));
                }
                _this.$.unblockUI();
            }, 100);
        };
        this.triggerClickDetailWhenSwitchAll = function (controlArray) {
            controlArray.each(function (index, detailButton) { return _this.$(detailButton).find('.control').click(); });
            _this.$dataTable.fixedHeader.adjust();
        };
        this.getTableOptions = function () {
            var self = _this;
            return {
                language: {
                    processing: '',
                    emptyTable: _this.$('#NoInformationIsAvailable').val()
                },
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
                                return self.renderData(idx.column, header.text(), cellData);
                            }).toArray().join('');
                            return self.renderDetail(rowIdx, data);
                        }
                    }
                },
                columnDefs: [{
                        className: 'control',
                        orderable: false,
                        targets: -1
                    }],
                ordering: _this.canSort,
                paging: false,
                searching: false,
                bInfo: false,
                fixedHeader: true,
                processing: true,
                drawCallback: _this.drawCallback
            };
        };
        this.renderData = function (idxColumn, headerText, cellData) { return "<li data-dtr-index=\"" + idxColumn + "\">" +
            ("<span class=\"dtr-title\">" + headerText + "</span> ") +
            ("<span class=\"dtr-data\">" + cellData + "</span></li>"); };
        this.renderDetail = function (rowIdx, data) { return data ?
            _this.$("<ul data-dtr-index=\"" + rowIdx + "\"/>").append(data) :
            false; };
        this.drawCallback = function () {
            _this.$('tbody').append(_this.$('tr.tbl-footer'));
            _this.$tableReport.removeClass('hidden');
        };
        this.$ = $;
        this.linkBuilderComponent = linkBuilderComponent;
    }
    BaseReportNew.prototype.init = function () {
        this.cacheDOMElements();
    };
    BaseReportNew.prototype.cacheDOMElements = function () {
        this.$filterIcon = this.$(".icon-filter");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$pageTitle = this.$(".page-title");
        this.$reportForm = this.$("#report-form");
        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$submitButton = this.$(".btn-submit");
        this.$tableReport = this.$("#tbl-report");
        this.canSort = parseInt(this.$tableReport.data('totalrecords'), 0) > 1;
    };
    BaseReportNew.prototype.registerEvents = function () {
        var _this = this;
        this.$filterIcon.on("click", function () { return _this.onClickIconFilter(); });
    };
    BaseReportNew.prototype.triggerRedrawTable = function () {
        var _this = this;
        this.doAdjustHeader();
        this.$('tbody tr.parent').each(function (index, element) { return _this.onClickDetailControl(element); });
    };
    BaseReportNew.prototype.onClickDetailControl = function (element) {
        var parentRow = this.$(element).closest('tr');
        var childRow = this.$(parentRow).next();
        parentRow.hasClass('tbl-footer') ?
            this.onClickFooterRow(parentRow, childRow) :
            this.onClickBodyRow(parentRow, childRow);
    };
    ;
    BaseReportNew.prototype.onClickIconFilter = function () {
        this.currentHideState = this.$reportForm.hasClass("hide");
        this.currentHideState ? this.showFilter() : this.hideFilter();
        this.doAdjustHeader();
    };
    BaseReportNew.prototype.initDataTable = function (options) {
        var _this = this;
        if (options === void 0) { options = {}; }
        var tableOptions = this.getTableOptions();
        this.$.extend(tableOptions, options);
        this.$dataTable = this.$tableReport.DataTable(tableOptions);
        this.$tableReport.find('tbody').on('click', 'td.control', function (event) { return _this.onClickDetailControl(event.currentTarget); });
        this.$tableReport.find('thead').on('click', 'th.control', function (event) { return _this.collapseExpandAll(event.currentTarget); });
        this.$dataTable.on('draw.dt', function () { return _this.triggerRedrawTable(); });
    };
    return BaseReportNew;
}());
//# sourceMappingURL=base-report-new.js.map