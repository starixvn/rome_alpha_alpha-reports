/// <reference path="../core.ts" />
"use strict";
interface JQuery {
    pagination(option: any): void;
}

class ClientPagerOption {
    constructor(
        public onSelectPage: () => void = null,
        public pagerId: string = "report-pager",
        public formId: string = "report-form",
        public pageIndexId: string = "PageIndex",
        public pageSizeId: string = "PageSize",
        public tableReportId: string = "tbl-report") {
    }
}

class ClientPagerComponent {
    $: JQueryStatic;
    $pageIndex: JQuery;
    $pageSize: JQuery;
    $reportForm: JQuery;

    $bodyRows: JQuery;

    totalRecords: number;
    pageSize: number;
    pageIndex: number;

    pageList: number[] = [1000, 2000, 3000, 5000];
    smallPageList: number[] = [10, 50, 100, 500];

    options = new ClientPagerOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: ClientPagerOption = new ClientPagerOption()) => {
        let pagerOption = <ClientPagerOption>this.$.extend({}, this.options, options);
        let pager = this.$("#" + pagerOption.pagerId);

        this.$reportForm = this.$("#" + this.options.formId);
        this.$bodyRows = this.$("#" + this.options.tableReportId + " > tbody > tr");

        this.totalRecords = parseInt(pager.data("totalrecords"), 0);
        this.pageSize = parseInt(pager.data("pagesize"), 0);
        this.pageIndex = parseInt(pager.data("pageindex"), 0);
        this.$pageIndex =
            this.$(`<input type="hidden" id="${pagerOption.pageIndexId}" name="${pagerOption.pageIndexId}" value="${this.pageIndex}" />`);
        this.$pageSize =
            this.$(`<input type="hidden" id="${pagerOption.pageSizeId}" name="${pagerOption.pageSizeId}" value="${this.pageSize}" />`);

        this.$reportForm.append(this.$pageIndex).append(this.$pageSize);

        pager.pagination({
            pageList: this.pageList,
            total: this.totalRecords,
            pageSize: this.pageSize,
            pageNumber: this.pageIndex,

            showRefresh: false,
            onSelectPage: this.onSelectPage,
            beforePageText: pager.data("labelpage"),
            afterPageText: pager.data("labelof") + " {pages}",
            displayMsg: pager.data("labeldisplayitems")
        });

        this.showBodyRows();
        pager.removeClass("hide");

        return pager;
    };

    onSelectPage = (pageNumber: number, pageSize: number) => {
        this.updateHiddenFields(pageNumber, pageSize);
        this.showBodyRows();
    };
    updateHiddenFields = (pageNumber: number, pageSize: number) => {
        this.$pageIndex.val(pageNumber);
        this.$pageSize.val(pageSize);

        this.pageSize = pageSize;
        this.pageIndex = pageNumber;
    };
    showBodyRows = () => {
        if (!this.totalRecords) {
            return;
        }

        let startIndex = this.pageSize * (this.pageIndex - 1);
        let endIndex = startIndex + this.pageSize;

        this.$bodyRows.addClass("hide");
        for (var i = startIndex; i < endIndex && i < this.totalRecords; i++) {
            this.$(this.$bodyRows[i]).removeClass("hide");
        }
    };
}