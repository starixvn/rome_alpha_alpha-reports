"use strict";
var BaseReportView = /** @class */ (function () {
    function BaseReportView() {
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
        var hideFilterText = $("#hiddenHideFilterText").val();
        var showFilterText = $("#hiddenShowFilterText").val();
        var iconFilter = $("#icon-filter").click(function () {
            var isHideFilter = iconFilter.hasClass("icon-arrow-up-drop-circle");
            if (isHideFilter) {
                iconFilter
                    .removeClass("icon-arrow-up-drop-circle")
                    .addClass("icon-arrow-down-drop-circle")
                    .attr("title", showFilterText);
            }
            else {
                iconFilter
                    .removeClass("icon-arrow-down-drop-circle")
                    .addClass("icon-arrow-up-drop-circle")
                    .attr("title", hideFilterText);
            }
            callback(isHideFilter);
        });
    };
    return BaseReportView;
}());
//# sourceMappingURL=base-report-view.js.map