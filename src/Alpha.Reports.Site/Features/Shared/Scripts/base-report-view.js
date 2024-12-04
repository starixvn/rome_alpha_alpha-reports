"use strict";
var BaseReportView = /** @class */ (function () {
    function BaseReportView() {
        this.sortColumnIndex = 0;
        this.sortIndicator = 0; // asc: 0, desc: 1
    }
    BaseReportView.prototype.initReportLegend = function (legendUrl) {
        $.get(legendUrl, function (html) {
            $("body").append(html);
            $("#icon-report-legend").click(function () {
                $("#report-legend").modal({ show: true, backdrop: false });
            });
        });
    };
    BaseReportView.prototype.initShowHideFilter = function (callback) {
        if (callback === void 0) { callback = undefined; }
        var hideFilterText = $("#hiddenHideFilterText").val();
        var showFilterText = $("#hiddenShowFilterText").val();
        var iconFilter = $("#icon-filter").click(function () {
            var isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            $('#report-form').toggle(!isHideFilter);
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("data-original-title", showFilterText);
            }
            else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("data-original-title", hideFilterText);
            }
            callback && callback(isHideFilter);
        });
    };
    BaseReportView.prototype.tableSorter = function ($tblReport, canSort, sortEndCallback) {
        var _this = this;
        if (canSort === void 0) { canSort = undefined; }
        if (sortEndCallback === void 0) { sortEndCallback = undefined; }
        canSort = canSort || $tblReport.data('cansort').toLowerCase() === 'true';
        var sortColumns = $tblReport.find('.sorting');
        if (canSort) {
            var headers_1 = {};
            sortColumns.each(function () {
                // sorttype is text or digit
                var $this = $(this);
                console.log("$this col", $this);
                headers_1[$this.data('colindex')] = { sorter: $this.data('sorttype') || 'digit' };
            });
            var sortList = typeof this.sortColumnIndex === 'undefined'
                ? []
                : [[this.mapSortColumnIndex(), this.sortIndicator]];
            $tblReport.tablesorter({
                headers: headers_1,
                sortList: sortList
            }).bind("sortEnd", function () {
                var sortedColumns = $tblReport.find(".sorting-asc, .sorting-desc");
                if (sortedColumns.length) {
                    var firstSortedColumn = sortedColumns.eq(0);
                    _this.sortColumnName = firstSortedColumn.data("colname");
                    _this.sortColumnIndex = firstSortedColumn.data("colindex");
                    _this.sortIndicator = firstSortedColumn.hasClass("sorting-desc") ? 1 : 0; // asc: 0, desc: 1
                }
                sortEndCallback && sortEndCallback();
            });
        }
        else {
            sortColumns.removeClass('sorting');
        }
    };
    BaseReportView.prototype.mapSortColumnIndex = function () {
        return this.sortColumnIndex;
    };
    BaseReportView.prototype.stickyTableHeaders = function ($tblReport) {
        $tblReport.stickyTableHeaders('destroy');
        $tblReport.stickyTableHeaders();
    };
    BaseReportView.prototype.updateCurrentUrl = function (queryString) {
        try {
            window.top.indexManager.updateCurrentUrl(queryString);
        }
        catch (e) { }
    };
    return BaseReportView;
}());
//# sourceMappingURL=base-report-view.js.map