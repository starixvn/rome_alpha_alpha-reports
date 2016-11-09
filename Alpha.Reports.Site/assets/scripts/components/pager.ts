/// <reference path="../core.ts" />
"use strict";
class PagerOption {
    pagerId: string;
    onSelectPage: any;

    constructor(pagerId: string = "report-pager",
        onSelectPage: any = null) {
        this.pagerId = pagerId;
        this.onSelectPage = onSelectPage;
    }
}

class PagerComponent {
    $: any;

    options = new PagerOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: PagerOption = new PagerOption()) => {
        var pickerOptions = this.$.extend({}, this.options, options);
        var pager = this.$("#" + pickerOptions.pagerId);

        pickerOptions.totalRecords = pager.data("totalrecords");
        pickerOptions.pageSize = pager.data("pagesize");
        pickerOptions.pageIndex = pager.data("pageindex");

        if (pickerOptions.totalRecords < alphaReports.pageList[0]) {
            // total records less than min page size => do not need to show pager
            pager.addClass("hide");
        } else {
            pager.pagination({
                pageList: alphaReports.pageList,
                total: pickerOptions.totalRecords,
                pageSize: pickerOptions.pageSize,
                pageNumber: pickerOptions.pageIndex,

                showRefresh: false,
                onSelectPage: function (pageNumber: number, pageSize: number) {
                    pager.data("pageindex", pageNumber);
                    pager.data("pagesize", pageSize);

                    pickerOptions.onSelectPage();
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

const pagerComponent = new PagerComponent($);