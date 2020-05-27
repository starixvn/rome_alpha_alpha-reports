/// <reference path="../core.ts" />
/// <reference path="../components/sort.ts" />

"use strict";

declare var outstandingDatasource;

class OutstandingView extends BaseReport {
    $: JQueryStatic;

    sortComponent: SortComponent;
    productMultiSelectionComponent: ProductMultiSelectionComponent;
    linkBuilderComponent: LinkBuilderComponent;

    $tableReport: JQuery;
    $reportForm: JQuery;
    $buttonProduct: JQuery;
    $rowEx: JQuery;

    $sortPropertyName: JQuery;
    $isDescendingSort: JQuery;
    $isFilterVisible: JQuery;
    $pageTitle: JQuery;

    constructor(
        $: JQueryStatic,
        sortComponent: SortComponent,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.sortComponent = sortComponent;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.linkBuilderComponent = linkBuilderComponent;
    }

	init = () => {
		this.initDataTable();
        this.cacheElements();

        this.sortComponent.render(new SortOption(
            this.$tableReport,
            parseInt(this.$tableReport.data("totalrecords"), 0) > 1,
            true,
            this.sortComponent.afterSort,
            { 0: { sorter: "text" } },
            this.$reportForm));

        this.$tableReport.stickyTableHeaders();

        this.productMultiSelectionComponent.render();
        this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");

        this.linkBuilderComponent.render();

        this.registerEvents();
		this.initHelp();

    };

	format(data) {
		// `d` is the original data object for the row
		return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
			'<tr>' +
			'<td>AgentOutstanding:</td>' +
			'<td>' + data.AgentOutstanding + '</td>' +
			'</tr>' +
			'<tr>' +
			'<td>MemberOutstanding:</td>' +
			'<td>' + data.MemberOutstanding + '</td>' +
			'</tr>' +
			'</table>';
	}

	initDataTable() {
		this.$('.tblRpt tfoot').on('click', 'th.details-control', function () {
			if (this.$('#detail-footer').hasClass('hidden')) {
				this.$('#detail-footer').removeClass('hidden');
			}
			else {
				this.$('#detail-footer').addClass('hidden');
			}
		});

		var table = this.$('.tblRpt').DataTable({
			"data": outstandingDatasource,
			"columns": [
				{
					"data": null,
					"render": function (row) {
						return `<a  href='${row.DownlineLink}' data-custname='${row.CustName}' class="downline-link">${row.CustName}</a>`;
					}
				},
				{ "data": "MasterOutstanding" },
				{ "data": "SuperOutstanding" },
				{
					"className": 'details-control',
					"orderable": false,
					"data": null,
					"defaultContent": ''
				}
			],
			"order": [[1, 'asc']],
			"fixedHeader": {
				"header": true,
				"footer": true
			}
		});

		// Add event listener for opening and closing details
		let that = this;
		that.$('.tblRpt tbody').on('click', 'td.details-control', function () {
			var tr = that.$(this).closest('tr');
			var row = table.row(tr);

			if (row.child.isShown()) {
				// This row is already open - close it
				row.child.hide();
				tr.removeClass('shown');
			}
			else {
				// Open this row
				row.child(that.format(row.data())).show();
				tr.addClass('shown');
			}
		});

	}

    public cacheElements() {
        super.cacheElements();

        this.$tableReport = this.$("#tbl-report");
        this.$reportForm = this.$("#report-form");

        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$isFilterVisible = this.$("#IsFilterVisible");
        this.$pageTitle = this.$(".page-title");
        this.$rowEx = this.$(".row-ex");
    };

    public registerEvents() {
        super.registerEvents();
        this.$reportForm.submit(this.onSubmitReport);
        const self = this;
        this.$(".col-function").click(function () {
            self.showHideValue(self.$(this));
        });
        this.$("#icon-total").click(function () {
            self.showHideTotalValue(self.$(this));})
    };

    public showHideTotalValue(colFunction: JQuery) { 
        const rowExtension = this.$(".row-ex-total");
        if (colFunction.hasClass("icon-plus")) {
            rowExtension.addClass("hide");
            colFunction.removeClass("icon-minus").addClass("icon-plus");
        } else {
            rowExtension.removeClass("hide");
            colFunction.removeClass("icon-plus").addClass("icon-minus");
        }
    };

    public showHideValue(colFunction: JQuery) {
        const rowExtension = this.$(".rowEx-" + colFunction.data("custid"));

        if (colFunction.hasClass("icon-plus")) {
            rowExtension.removeClass("hide");
            colFunction.removeClass("icon-plus").addClass("icon-minus");
        } else {
            rowExtension.addClass("hide");
            colFunction.removeClass("icon-minus").addClass("icon-plus");
        }
	};

    onSubmitReport = (e: any) => {
        let this$ = this.$;

        this$.blockUI();

        var params = this.getParams();
        if (!params.IsAnyProductSelected) {
            this.showValidateProductError();
            this$.unblockUI();
            e.preventDefault();

            return;
        }

        this.$sortPropertyName.val(params.SortPropertyName);
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();
        let sortInfo = this.sortComponent.getSortInfo();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort,
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
    showValidateProductError = () => {
        this.$buttonProduct.addClass("error");
    };
    afterSort = () => {
        var this$ = this.$;
        var sortedColumn = this$(".sorting-asc:visible");
        var direction = !sortedColumn.length;
        if (sortedColumn.length === 0) {
            sortedColumn = this$(".sorting-desc");
        }

        var sortedColumnName = sortedColumn.data("sortedname");

        this.replaceHrefParam("SortPropertyName", sortedColumnName);
        this.replaceHrefParam("IsDescendingSort", direction);

        var isEvenRow = false;
        this.$tableReport.find("tbody > tr").each(function () {
            var $row = this$(this);
            if ($row.hasClass("even-row") && !isEvenRow) {
                $row.removeClass("even-row");
            } else if (!$row.hasClass("even-row") && isEvenRow) {
                $row.addClass("even-row");
            }

            isEvenRow = !isEvenRow;
        });
    };
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
    const outstandingView = new OutstandingView(
        $,
        new SortComponent($, linkBuilderComponent),
        new ProductMultiSelectionComponent($),
        linkBuilderComponent);

    outstandingView.init();
});
