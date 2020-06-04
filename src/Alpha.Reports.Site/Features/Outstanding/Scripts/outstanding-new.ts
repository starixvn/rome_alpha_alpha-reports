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

	initDataTable = () => {
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
				"render": this.$.fn.dataTable.render.number(',', '.', 2, '')
			},
			{
				"data": `${viewModel.DisplayOutstandingLevels[1].DisplayName}Outstanding`,
				"render": this.$.fn.dataTable.render.number(',', '.', 2, '')
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
					"data": `${viewModel.DisplayOutstandingLevels[i].DisplayName}Outstanding`,
					"render": this.$.fn.dataTable.render.number(',', '.', 2, '')
				});
			}
		}

		let colunmSortIndex = 0;
		this.$.each(viewModel.DisplayOutstandingLevels, (key, value) => {
			if (this.$(this.$sortPropertyName).val().toLowerCase() === `${value.DisplayName}Outstanding`.toLowerCase()) {
				colunmSortIndex = key + 1;
				return;
			}
		});
		
		this.$table = this.$('.tblRpt').DataTable({
			"data": viewModel.OutstandingDatasource,
			"columns": columns,
			"order": [[colunmSortIndex, JSON.parse(this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']],
			"paging": false,
			"searching": false,
			"bInfo": false,
			"fixedHeader": {
				"header": true,
				"footer": true
			},
			"drawCallback": () => {
				
			}
		});

		this.$('.tblRpt thead').on('click', 'th.details-control', event => this.onClickDetailControlOnHeader(event));
		this.$('.tblRpt tbody').on('click', 'td.details-control', event => this.onClickDetailControlOnBody(event));
		this.$('.tblRpt tfoot').on('click', 'td.details-control', event => this.onClickDetailControlOnFooter(event));
		this.$('.tblRpt').on('draw.dt', () => this.triggerRedrawTable());
	}

	formatDetailRow = (data, parentRow) => {
		let rows = [];
		const heigh = this.$(parentRow).height();
		for (let i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
			const level = `${viewModel.DisplayOutstandingLevels[i].DisplayName}Outstanding`;
			rows.push(`<div class="child-row" style="height: ${heigh}px"><span>${level}</span><span>${data[level]}</span></div>`);
		}

		return rows.join("\n");
	}

	onClickDetailControlOnHeader = (event) => {
		var tr = this.$(event.currentTarget).closest('tr');

		if (tr.hasClass('shown')) {
			tr.removeClass('shown');

			this.$('tbody tr.shown').each((index, element) => {
				this.$(element).find('.details-control').click();
			});

			this.$('tfoot tr.shown').each((index, element) => {
				this.$(element).find('.details-control').click();
			});
		}
		else {
			tr.addClass('shown');

			this.$('tbody tr').not('.shown').each((index, element) => {
				this.$(element).find('.details-control').click();
			});

			this.$('tfoot tr').not('.shown').each((index, element) => {
				this.$(element).find('.details-control').click();
			});
		}

		this.$table.fixedHeader.adjust();

		let scrollTo = this.$('.fixedHeader-floating thead').length > 0 ? this.$(document).height() : 0;
		this.$(window).scrollTop(scrollTo);
	}

	onClickDetailControlOnBody = (event) => {
		var tr = this.$(event.currentTarget).closest('tr');
		var row = this.$table.row(tr);

		if (row.child.isShown()) {
			row.child.hide();
			tr.removeClass('shown');
		}
		else {
			row.child(this.formatDetailRow(row.data(), tr), `child-row ${tr.hasClass('odd') ? 'odd' : 'even'}`).show();
			tr.addClass('shown');
		}
	}

	onClickDetailControlOnFooter = (event) => {
		var tr = this.$(event.currentTarget).closest('tr');

		if (tr.hasClass('shown')) {
			tr.removeClass('shown');
			tr.next().addClass('hide');
		}
		else {

			tr.addClass('shown');
			tr.next().removeClass('hide');
		}
	}

	triggerRedrawTable = () => {
		this.$('td.details-control').each((index, element) => {
			let tr = this.$(element).closest('tr');
			let row = this.$table.row(tr);

			if (row.child.isShown()) {
				row.child(this.formatDetailRow(row.data(), tr), `child-row ${tr.hasClass('odd') ? 'odd' : 'even'}`).show();
			}
		});

		this.$table.fixedHeader.adjust();

		let scrollTo = this.$('.fixedHeader-floating thead').length > 0 ? this.$(document).height() : 0;
		this.$(window).scrollTop(scrollTo);

		this.replaceHrefParam("SortPropertyName", this.getColumnSortName());
		this.replaceHrefParam("IsDescendingSort", this.isDescendingSort());
	}

	getColumnSortName = () => {
		let order = this.$table.order();
		const length = this.isMobileView ? 1 : viewModel.DisplayOutstandingLevels.length;

		for (let index = 0; index < length; index++) {
			if (order[0][0] - 1 === index) {
				return `${viewModel.DisplayOutstandingLevels[index].DisplayName}Outstanding`;
			}
		}

		return 'CustName';
	}

	isDescendingSort = () => {
		let order = this.$table.order();
		return order[0][1] !== 'asc';
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

		this.replaceHrefParam("SortPropertyName", this.getColumnSortName());
		this.replaceHrefParam("IsDescendingSort", this.isDescendingSort());
		this.$sortPropertyName.val(this.getColumnSortName());
		this.$isDescendingSort.val(this.isDescendingSort ? 'True' : 'False');
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
