/// <reference path="../../../assets/scripts/core.ts" />

"use strict";

declare var viewModel;
declare var showExpandIcon;

class OutstandingViewNew extends BaseReport {
    $: JQueryStatic;

    productMultiSelectionComponent: ProductMultiSelectionComponent;
    linkBuilderComponent: LinkBuilderComponent;

    $tableReport: JQuery;
    $reportForm: JQuery;
    $buttonProduct: JQuery;

    $sortPropertyName: JQuery;
    $isDescendingSort: JQuery;
    $isFilterVisible: JQuery;
    $pageTitle: JQuery;
	$table: JQuery;

	isMobileView: boolean;

    constructor(
        $: JQueryStatic,
        productMultiSelectionComponent: ProductMultiSelectionComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.$ = $;
        this.productMultiSelectionComponent = productMultiSelectionComponent;
        this.linkBuilderComponent = linkBuilderComponent;
    }

	init = () => {
		this.cacheElements();

        this.productMultiSelectionComponent.render();
		this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");
		this.isMobileView = JSON.parse(this.$('#IsMobileView').val().toLowerCase());

        this.linkBuilderComponent.render();

		this.registerEvents();

		this.initHelp();
		this.initDataTable();
    };

	format(data) {
		let rows = [];

		for (let i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
			const level = `${viewModel.DisplayOutstandingLevels[i].DisplayName}Outstanding`;
			rows.push(`<div><span style="float:left">${level}</span><span>${data[level]}</span></div>`);
		}

		return rows.join("\n");
	}

	initDataTable() {
		let that = this;

		var columns = [
			{
				"className": null,
				"orderable": true,
				"data": null,
				"defaultContent": '',
				"render": function (row) {
					return `<a  href="${row.DownlineLink}" data-custname="${row.CustName}" class="downline-link">${row.CustName}</a>`;
				}
			},
			{
				"data": `${viewModel.DisplayOutstandingLevels[0].DisplayName}Outstanding`,
			},
			{
				"data": `${viewModel.DisplayOutstandingLevels[1].DisplayName}Outstanding`
			}
		];

		if (this.isMobileView) {
			if (JSON.parse(this.$('#showExpandIcon').val().toLowerCase())) {
				columns.push({
					"className": 'details-control',
					"orderable": false,
					"data": null,
					"defaultContent": '',
					"render": function (row) {
						return null;
					}
				});
			}
		}
		else {
			for (let i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
				columns.push({
					data: `${viewModel.DisplayOutstandingLevels[i].DisplayName}Outstanding`
				});
			}
		}
		
		this.$table = this.$('.tblRpt').DataTable({
			"data": viewModel.OutstandingDatasource,
			"columns": columns,
			"order": [[0, 'asc']],
			"paging": false,
			"searching": false,
			"bInfo": false,
			"fixedHeader": {
				"header": true,
				"footer": true
			}
		});

		that.$('.tblRpt tbody').on('click', 'td.details-control', function () {
			var tr = that.$(this).closest('tr');
			var row = that.$table.row(tr);

			if (row.child.isShown()) {
				row.child.hide();
				tr.removeClass('shown');
			}
			else {
				row.child(that.format(row.data()), `${tr.hasClass('odd') ? 'odd' : 'even'}`).show();
				tr.addClass('shown');
			}
		});

		that.$('.tblRpt tfoot').on('click', 'td.details-control', function () {
			var tr = that.$(this).closest('tr');

			if (tr.hasClass('shown')) {
				tr.removeClass('shown');
				tr.next().addClass('hide');
			}
			else {
				
				tr.addClass('shown');
				tr.next().removeClass('hide');
				
			}
		});

		that.$('.tblRpt').on('draw.dt', function () {
			that.$('td.details-control').each(function () {
				var tr = that.$(this).closest('tr');
				var row = that.$table.row(tr);

				if (row.child.isShown()) {
					row.child(that.format(row.data()), `${tr.hasClass('odd') ? 'odd' : 'even'}`).show();
				}
			});

			that.$table.fixedHeader.adjust();

			let scrollTo = that.$('.fixedHeader-floating thead').length > 0 ? that.$(document).height() : 0;
			that.$(window).scrollTop(scrollTo);

			let order = that.$table.order();
			let title = that.$table.column(order[0][0]).header();
			that.replaceHrefParam("SortPropertyName", that.$(title).data('name'));
			that.replaceHrefParam("IsDescendingSort", order[0][1] !== 'asc');
		});

		that.$('.tblRpt thead').on('click', 'th.details-control', function () {
			var tr = that.$(this).closest('tr');

			if (tr.hasClass('shown')) {
				tr.removeClass('shown');

				that.$('tbody tr.shown').each(function () {
					that.$(this).find('.details-control').click();
				});

				that.$('tfoot tr.shown').each(function () {
					that.$(this).find('.details-control').click();
				});
			}
			else {
				tr.addClass('shown');

				that.$('tbody tr').not('.shown').each(function () {
					that.$(this).find('.details-control').click();
				});

				that.$('tfoot tr').not('.shown').each(function () {
					that.$(this).find('.details-control').click();
				});
			}

			that.$table.fixedHeader.adjust();

			let scrollTo = that.$('.fixedHeader-floating thead').length > 0 ? that.$(document).height() : 0;
			that.$(window).scrollTop(scrollTo);
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
    };

    public registerEvents() {
        super.registerEvents();
        this.$reportForm.submit(this.onSubmitReport);
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

		let order = this.$table.order();
		this.replaceHrefParam("SortPropertyName", this.$table.column(order[0][0]).header());
		this.replaceHrefParam("IsDescendingSort", order[0][1] !== 'asc');
		this.$sortPropertyName.val(this.$table.column(order[0][0]).header());
		this.$isDescendingSort.val(order[0][1] !== 'asc' ? 'True' : 'False');
        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
    };
    getParams = () => {
        let selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

        return {
            IsAnyProductSelected: selectedProducts.length > 0,
            IsFilterVisible: !this.$reportForm.is(":hidden")
        };
    };
    showValidateProductError = () => {
        this.$buttonProduct.addClass("error");
    };
}

$(function () {
    const linkBuilderComponent = new LinkBuilderComponent($);
	const outstandingViewNew = new OutstandingViewNew(
        $,
        new ProductMultiSelectionComponent($),
        linkBuilderComponent);

	outstandingViewNew.init();
});
