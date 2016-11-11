/// <reference path="../core.ts" />
"use strict";
class PagerOption {
    pagerId: string;
    onSelectPage: any;
    formId: string;

    constructor(
        pagerId: string = "report-pager",
        formId: string = "report-form",
        onSelectPage: any = null) {

        this.pagerId = pagerId;
        this.onSelectPage = onSelectPage;
        this.formId = formId;
    }
}

class PagerComponent {
    $: any;
    $pageIndex: any;
    $pageSize: any;

    options = new PagerOption();

    constructor($: any) {
        this.$ = $;
    }

    render = (options: PagerOption = new PagerOption()) => {
        var self = this;
        var pickerOptions = this.$.extend({}, this.options, options);
        var pager = this.$("#" + pickerOptions.pagerId);

        pickerOptions.totalRecords = pager.data("totalrecords");
        pickerOptions.pageSize = pager.data("pagesize");
        pickerOptions.pageIndex = pager.data("pageindex");

        var reportForm = this.$("#" + this.options.formId);
        this.$pageIndex = this.$(`<input type="hidden" id="PageIndex" name="PageIndex" value="${pickerOptions.pageIndex}" />`);
        this.$pageSize = this.$(`<input type="hidden" id="PageSize" name="PageSize" value="${pickerOptions.pageSize}" />`);
        reportForm.append(this.$pageIndex);
        reportForm.append(this.$pageSize);

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
                    self.updateHiddenFields(pageNumber, pageSize);
                    pickerOptions.onSelectPage();
                },
                beforePageText: pager.data("lablepage"),
                afterPageText: pager.data("labelof") + " {pages}",
                displayMsg: pager.data("labeldisplayitems")
            });

            pager.removeClass("hide");
        }

        return pager;
    }

    updateHiddenFields = (pageNumber: number, pageSize: number) => {
        this.$pageIndex.val(pageNumber);
        this.$pageSize.val(pageSize);
    }
}

const pagerComponent = new PagerComponent($);