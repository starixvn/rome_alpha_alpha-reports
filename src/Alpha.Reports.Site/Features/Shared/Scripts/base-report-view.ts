"use strict";

class BaseReportView {
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
                    .attr("title", showFilterText);
            } else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", hideFilterText);
            }

            callback(isHideFilter);
        });
    }
}