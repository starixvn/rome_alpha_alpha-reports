/// <reference path="../core.ts" />
"use strict";

class PagerComponent {
    options = {
        pagerId: "report-pager",
        onSelectPage: null
    };

    pageIndex = 0;
    pageSize = 0;

    render = (options: any = {}) => {
        var pickerOptions = $.extend({}, this.options, options);

        var $this = this;

        var pager = $("#" + pickerOptions.pagerId);

        var totalRecords = pager.data("totalrecords");
        this.pageSize = pager.data("pagesize");
        this.pageIndex = pager.data("pageindex");

        if (options.totalRecords < alphaReports.pageList[0]) {
            // total records less than min page size => do not need to show pager
            pager.addClass("hide");
        } else {
            pager.pagination({
                pageList: alphaReports.pageList,
                total: options.totalRecords,
                pageSize: options.pageSize,
                pageNumber: options.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    pager.data("pageindex", pageNumber);
                    pager.data("pagesize", pageSize);

                    options.onSelectPage();
                },
                beforePageText: pager.data("lablepage"),
                afterPageText: pager.data("labelof") + " {pages}",
                displayMsg: pager.data("labeldisplayitems")
            });

            pager.removeClass("hide");
        }

        return pager;
    };
}

const pagerComponent = new PagerComponent();