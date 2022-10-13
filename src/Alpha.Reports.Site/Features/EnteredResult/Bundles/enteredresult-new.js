"use strict";
var AlphaReports = /** @class */ (function () {
    function AlphaReports($, store) {
        var _this = this;
        this.format = {
            monthFormat: "MMMM - yyyy",
            shortDateWithSlash_DatePicker: "MM/DD/YYYY",
            shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
        };
        this.sort = {
            none: 0,
            ascending: 1,
            descending: -1
        };
        this.keyCode = {
            enter: 13
        };
        this.numberDisplayedProductsOnFilter = 1;
        this.maxTimesToShowTooltip = 5;
        this.screenSmallThan768 = function () {
            var currentWindowWidth = screen.width || _this.$(window).width();
            return currentWindowWidth < 768;
        };
        this.screenLandscapeMode = function () { return screen.width > screen.height || _this.$(window).width() > _this.$(window).height(); };
        this.splitCell = function (firstCell, cellClass, lastCellStyle) {
            if (cellClass === void 0) { cellClass = ""; }
            if (lastCellStyle === void 0) { lastCellStyle = "bold"; }
            var this$ = _this.$;
            var combinedCells = _this.$("." + firstCell + "-combined");
            combinedCells.each(function (index) {
                var oldValues = this$(this).find("li");
                var cellsLength = oldValues.length;
                var oldCells = "";
                for (var i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                    oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                        (i === lastCellIndex ? " " + lastCellStyle : "") + "\">" +
                        this$(oldValues[i]).html() +
                        "</td>";
                }
                this$(combinedCells[index]).before(oldCells);
            });
        };
        this.combineConsecutiveCells = function (firstCells, numberOfCombinedCells, combinedCellClass, combinedCellColSpan) {
            if (combinedCellColSpan === void 0) { combinedCellColSpan = 1; }
            if (numberOfCombinedCells <= 1) {
                return;
            }
            var this$ = _this.$;
            firstCells.each(function (index) {
                var newCell = "<td class=\"visible-stack-mode combinedcell " +
                    combinedCellClass + "\" colspan=\"" + combinedCellColSpan + "\">\
                    <ul>";
                var currentCell = this$(firstCells[index]);
                var isLastCell = false;
                for (var i = 0; i < numberOfCombinedCells; i++) {
                    newCell += "<li>" + currentCell.html() + "</li>";
                    if (currentCell.next().length) {
                        currentCell = currentCell.next();
                    }
                    else {
                        isLastCell = true;
                    }
                }
                newCell += "</ul>\
                        </td>";
                if (isLastCell) {
                    currentCell.after(newCell);
                }
                else {
                    currentCell.prev().after(newCell);
                }
            });
        };
        this.localStorage = {
            get: function (key) {
                return _this.store.enabled ? _this.store.get(key) : _this.$.cookie(key);
            },
            set: function (key, value) {
                _this.store.enabled ? _this.store.set(key, value) : _this.$.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                if (_this.isIe()) {
                    cookieExpire = { expires: 365 };
                }
                return cookieExpire;
            }
        };
        this.isIe = function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
        };
        this.replaceQueryStringValue = function (queryString, paramName, paramValue) {
            if (typeof queryString === "undefined"
                || queryString === null) {
                return "";
            }
            var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");
            var newQueryString = regex.test(queryString)
                ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
                : queryString + (queryString.indexOf("?") >= 0 ? "&" : "?") + paramName + "=" + paramValue;
            return newQueryString;
        };
        this.correctEvenRowClass = function ($tableReport) {
            var this$ = _this.$;
            var isEvenRow = false;
            $tableReport.find("tbody > tr").each(function () {
                var $row = this$(this);
                if ($row.hasClass("even-row") && !isEvenRow) {
                    $row.removeClass("even-row");
                }
                else if (!$row.hasClass("even-row") && isEvenRow) {
                    $row.addClass("even-row");
                }
                isEvenRow = !isEvenRow;
            });
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.formatNumber = function (value) { return value.toLocaleString("en-US"); };
        this.formatCurrency = function (value) { return value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); };
        this.$ = $;
        this.store = store;
    }
    AlphaReports.prototype.getFormData = function ($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};
        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });
        return indexed_array;
    };
    return AlphaReports;
}());
var alphaReports = new AlphaReports($, store);
//# sourceMappingURL=core.js.map
/// <reference path="../core.ts" />
"use strict";
var DateRangePickerOption = /** @class */ (function () {
    function DateRangePickerOption(selectorId, reportFormId, onChange, onBeforeQuickChooseChange) {
        if (selectorId === void 0) { selectorId = "daterange-picker"; }
        if (reportFormId === void 0) { reportFormId = "report-form"; }
        if (onChange === void 0) { onChange = null; }
        if (onBeforeQuickChooseChange === void 0) { onBeforeQuickChooseChange = null; }
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
        this.selectorId = selectorId;
        this.reportFormId = reportFormId;
        this.onChange = onChange;
        this.onBeforeQuickChooseChange = onBeforeQuickChooseChange;
    }
    return DateRangePickerOption;
}());
var DateRangePickerComponent = /** @class */ (function () {
    function DateRangePickerComponent($) {
        var _this = this;
        this.options = new DateRangePickerOption();
        this.quickChooseType = {
            specificDates: 7
        };
        this.isCompleteInitDateRangePicker = false;
        this.render = function (options) {
            if (options === void 0) { options = new DateRangePickerOption(); }
            _this.options = _this.$.extend({}, _this.options, options);
            var dateRangePicker = _this.$("#" + _this.options.selectorId);
            var currentDate = dateRangePicker.data("currentdate");
            var minDate = dateRangePicker.data("mindate");
            var maxDate = dateRangePicker.data("maxdate");
            _this.$reportForm = _this.$("#" + _this.options.reportFormId);
            dateRangePicker.nexDatePicker({
                datePickerFormat: {
                    dateFormat: alphaReports.format.shortDateWithSlash_DatePicker
                },
                selectedItem: dateRangePicker.data("selecteddaterange"),
                minDate: new Date(minDate),
                maxDate: maxDate === "" ? null : maxDate,
                currentDate: new Date(currentDate),
                onSelectDatePicker: function () {
                    if (_this.options.onChange) {
                        _this.options.onChange();
                    }
                },
                onQuickChooseChange: function (selectValue) {
                    _this.options.onBeforeQuickChooseChange && _this.options.onBeforeQuickChooseChange();
                    if (_this.isCompleteInitDateRangePicker && selectValue !== _this.quickChooseType.specificDates) {
                        _this.$reportForm.submit();
                    }
                }
            });
            _this.isCompleteInitDateRangePicker = true;
            return dateRangePicker;
        };
        this.$ = $;
    }
    return DateRangePickerComponent;
}());
//# sourceMappingURL=daterangepicker.js.map
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
                top: iconHelpPosition.top + 60
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
var BaseHistoricReport = /** @class */ (function (_super) {
    __extends(BaseHistoricReport, _super);
    function BaseHistoricReport($, dateRangePickerComponent, linkBuilderComponent) {
        var _this = _super.call(this, $, linkBuilderComponent) || this;
        _this.doAdjustDatePickerCalendar = function () {
            if (alphaReports.screenSmallThan768() && !alphaReports.screenLandscapeMode()) {
                var width = _this.$(window).width(), widget = _this.$(".bootstrap-datetimepicker-widget.dropdown-menu");
                widget.css({ width: width });
            }
        };
        _this.checkReady = function () { return _this.$("#IsHistoricReport").length > 0; };
        _this.dateRangePickerComponent = dateRangePickerComponent;
        return _this;
    }
    BaseHistoricReport.prototype.cacheDOMElements = function () {
        _super.prototype.cacheDOMElements.call(this);
        this.$excelIcon = this.$(".icon-excel");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$fromDate = this.$("#from-date");
        this.$historicReport = this.$("#historic-report");
        this.$isHistoricReport = this.$("#IsHistoricReport");
        this.$toDate = this.$("#to-date");
        this.$userSelectedDateRangeOption = this.$("#UserSelectedDateRangeOption");
    };
    BaseHistoricReport.prototype.initDataTable = function (options) {
        if (options === void 0) { options = {}; }
        _super.prototype.initDataTable.call(this, options);
        this.doAdjustDataTable();
    };
    BaseHistoricReport.prototype.doAdjustDataTable = function () {
        var browserWidth = this.$(window).width(), deviceWidth = screen.width || window.outerWidth, columnCount = this.$dataTable.columns()[0].length;
        if (deviceWidth < 768 && browserWidth >= 768) {
            for (var i = 3; i < columnCount - 1; i++) {
                this.$(this.$dataTable.column(i).header()).removeClass("desktop min-tablet-l mobile").addClass("mobile");
            }
        }
        if (this.$tableReport.width() > browserWidth) {
            this.$tableReport.find("tbody tr td:first-child").css("white-space", "nowrap");
            this.$(this.$dataTable.column(2).header()).removeClass("all").addClass("min-tablet-l");
        }
        this.$dataTable.responsive.rebuild();
        this.$dataTable.responsive.recalc();
    };
    BaseHistoricReport.prototype.registerEvents = function () {
        var _this = this;
        _super.prototype.registerEvents.call(this);
        this.$excelIcon.on("click", function () { return _this.onExportExcelEvent(); });
        this.$historicReport.on("click", function () { return _this.onSubmitHistoricReportEvent(); });
        this.$reportForm.submit(function (event) { return _this.onSubmitReportEvent(event); });
        this.$fromDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
        this.$toDate.parent().on("dp.show", this.doAdjustDatePickerCalendar);
    };
    BaseHistoricReport.prototype.onExportExcelEvent = function () {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
    BaseHistoricReport.prototype.onSubmitHistoricReportEvent = function () { };
    BaseHistoricReport.prototype.onSubmitReportEvent = function (event) { };
    return BaseHistoricReport;
}(BaseReportNew));
//# sourceMappingURL=base-historic-report.js.map
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
var EnterResultViewNew = /** @class */ (function (_super) {
    __extends(EnterResultViewNew, _super);
    function EnterResultViewNew($, dateRangePickerComponent) {
        var _this = _super.call(this, $, dateRangePickerComponent, null) || this;
        _this.init = function () {
            if (!_this.checkReady()) {
                return;
            }
            _super.prototype.init.call(_this);
            _this.isMobileView = JSON.parse(_this.$isMobileView.val().toLowerCase());
            _this.dateRangePickerComponent.render();
            _this.initDataTable();
            _this.registerEvents();
            _this.$.unblockUI();
        };
        _this.cacheDOMElements = function () {
            _super.prototype.cacheDOMElements.call(_this);
            _this.$isMobileView = _this.$("#IsMobileView");
        };
        _this.initDataTable = function () {
            _super.prototype.initDataTable.call(_this, _this.isMobileView
                ? { order: [] }
                : { responsive: false, columnDefs: [], order: [] });
        };
        _this.renderDetail = function (rowIdx, data) {
            var toDate = "<li><b>" + _this.$('#ToDate').val() + "</b></li>";
            return data ?
                _this.$("<ul data-dtr-index=\"" + rowIdx + "\"/>").append(toDate, data) :
                false;
        };
        _this.drawCallback = function () {
            _this.$tableReport.removeClass('hidden');
        };
        _this.onSubmitReportEvent = function (event) {
            _this.setHiddenValue();
        };
        _this.onSubmitHistoricReportEvent = function () {
            _this.$fromDate.val("");
            _this.$toDate.val("");
            _this.$isHistoricReport.val("true");
            _this.setHiddenValue();
            _this.$reportForm.trigger("submit");
        };
        _this.setHiddenValue = function () {
            _this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : true));
            _this.$.blockUI();
        };
        return _this;
    }
    return EnterResultViewNew;
}(BaseHistoricReport));
$(function () {
    var enterResultViewNew = new EnterResultViewNew($, new DateRangePickerComponent($));
    enterResultViewNew.init();
});
//# sourceMappingURL=enteredresult-new.js.map