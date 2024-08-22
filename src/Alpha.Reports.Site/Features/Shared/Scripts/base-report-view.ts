"use strict";

class BaseReportView {
    protected sortColumnName: string;
    protected sortColumnIndex = 0;
    protected sortIndicator = 0; // asc: 0, desc: 1

    protected initReportLegend(legendUrl: string) {
        $.get(legendUrl, function (html) {
            $("body").append(html);
            $("#icon-report-legend").click(function () {
                $("#report-legend").modal({ show: true, backdrop: false });
            });
        });
    }

    protected initShowHideFilter(callback) {
        const hideFilterText = $("#hiddenHideFilterText").val();
        const showFilterText = $("#hiddenShowFilterText").val();
        const iconFilter = $("#icon-filter").click(function () {
            const isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("data-original-title", showFilterText);
            } else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("data-original-title", hideFilterText);
            }

            callback(isHideFilter);
        });
    }

    protected tableSorter($tblReport: JQuery, canSort: boolean = undefined, sortEndCallback: Function = undefined) {
        canSort = canSort || $tblReport.data('cansort').toLowerCase() === 'true';
        const sortColumns = $tblReport.find('.sorting');

        if (canSort) {
            const headers = {};
            sortColumns.each(function () {
                // sorttype is text or digit
                const $this = $(this);
                headers[$this.data('colindex')] = { sorter: $this.data('sorttype') || 'digit' };
            });
            const sortList = typeof this.sortColumnIndex === 'undefined'
                ? []
                : [[this.mapSortColumnIndex(), this.sortIndicator]];

            $tblReport.tablesorter({
                headers: headers,
                sortList: sortList
            }).bind("sortEnd", () => {
                const sortedColumns = $tblReport.find(".sorting-asc, .sorting-desc");
                if (sortedColumns.length) {
                    const firstSortedColumn = sortedColumns.eq(0);
                    this.sortColumnName = firstSortedColumn.data("colname");
                    this.sortColumnIndex = firstSortedColumn.data("colindex");
                    this.sortIndicator = firstSortedColumn.hasClass("sorting-desc") ? 1 : 0; // asc: 0, desc: 1
                }

                sortEndCallback && sortEndCallback();
            });
        } else {
            sortColumns.removeClass('sorting');
        }
    }

    protected mapSortColumnIndex() {
        return this.sortColumnIndex;
    }

    protected stickyTableHeaders($tblReport: JQuery) {
        $tblReport.stickyTableHeaders('destroy');
        $tblReport.stickyTableHeaders();
    }

    protected updateCurrentUrl(queryString) {
        try {
            window.top.indexManager.updateCurrentUrl(queryString);
        } catch (e) { }
    }
}