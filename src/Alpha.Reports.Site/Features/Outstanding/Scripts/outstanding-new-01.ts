/// <reference path="../../../assets/scripts/core.ts" />

"use strict";

declare var viewModel;
declare var showExpandIcon;

interface Window {
	$: JQueryStatic;
}

class OutstandingViewNew1 extends BaseReport {
    $: JQueryStatic;

    productMultiSelectionComponent: ProductMultiSelectionComponent;
    linkBuilderComponent: LinkBuilderComponent;

    $reportForm: JQuery;
    $buttonProduct: JQuery;

    $sortPropertyName: JQuery;
    $isDescendingSort: JQuery;
    $isFilterVisible: JQuery;
	$table: JQuery;

	isMobileView: boolean;

	data: any;

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
		//this.zoomDisable();
		//this.$.unblockUI();
		this.data = viewModel.OutstandingDatasource;
		this.cacheElements();

        this.productMultiSelectionComponent.render();
		this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");
		this.isMobileView = JSON.parse(this.$('#IsMobileView').val().toLowerCase());

        this.linkBuilderComponent.render();

		this.registerEvents();

		this.initHelp();
		this.initDataTable();
    };

    cacheElements = () =>  {
		super.cacheElements();

        this.$reportForm = this.$("#report-form");

        this.$sortPropertyName = this.$("#SortPropertyName");
        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$isFilterVisible = this.$("#IsFilterVisible");
    };

	buildViewportMetaContent = (initialScale, userScalable) => {
		return `<meta name='viewport' content='width=device-width, initial-scale=${initialScale}, user-scalable=${userScalable}' />`;
	}

	zoomDisable = () => {
		const viewportMetaData = 'head meta[name=viewport]';

		this.$(viewportMetaData).remove();
		this.$('head').prepend(this.buildViewportMetaContent('1.0', 'no'));

		window.top.$(viewportMetaData).remove();
		window.top.$('head').prepend(this.buildViewportMetaContent('0.7', 'no')); 
	}

    registerEvents = () =>  {
        super.registerEvents();
		this.$reportForm.submit(this.onSubmitReport);

		this.$(window).on('orientationchange', () => {
			//this.$(window).scrollTop(0);
			//setTimeout(() => {
			//		this.$table.draw();
			//}, 1500);
			
			//this.zoomDisable();
			
			const frms = window.top.parent.frames;
			if (frms && frms["main"]) {
				//this.$.blockUI();
				this.$table.clear();
				this.$table.destroy();
				this.$('.tblRpt').empty();

				frms["main"].location = frms["main"].location;
			}

			//this.$table.clear();
			//this.$table.destroy();
			//this.$('.tblRpt').empty();

			//setTimeout(() => {
			//	this.$table.rows.add(this.data).draw();
			//}, 3000);

			//this.$('.tblRpt').DataTable().clear().destroy();
			//this.$('.tblRpt').empty();

			////this.$table..destroy();
			//this.initDataTable();
		});
    };

	initDataTable = () => {
		//const columns = [
		//	{
		//		"className": null,
		//		"orderable": true,
		//		"data": null,
		//		"defaultContent": '',
		//		"render": function (row) {
		//			return `<a  href="${row.DownlineLink}" data-custname="${row.CustName}" class="downline-link">${row.CustName}</a>`;
		//		}
		//	},
		//	{
		//		"className" : "text-right",
		//		"data" : `${viewModel.DisplayOutstandingLevels[0].DisplayName}Outstanding`,
		//		"render" : this.$.fn.dataTable.render.number(',', '.', 2, '')
		//	},
		//	{
		//		"className": "text-right",
		//		"data": `${viewModel.DisplayOutstandingLevels[1].DisplayName}Outstanding`,
		//		"render": this.$.fn.dataTable.render.number(',', '.', 2, '')
		//	}
		//];

		//for (let i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
		//	columns.push({
		//		"className": "text-right",
		//		"data": `${viewModel.DisplayOutstandingLevels[i].DisplayName}Outstanding`,
		//		"render": this.$.fn.dataTable.render.number(',', '.', 2, '')
		//	});
		//}

		//let colunmSortIndex = 0;
		//this.$.each(viewModel.DisplayOutstandingLevels, (key, value) => {
		//	if (this.$(this.$sortPropertyName).val().toLowerCase() === `${value.DisplayName}Outstanding`.toLowerCase()) {
		//		colunmSortIndex = key + 1;
		//		return;
		//	}
		//});

		var table = this.$('#example').DataTable({
			responsive: true,
			paging: false
		});

			//new $.fn.dataTable.FixedHeader(table);
		//this.$table = this.$('.tblRpt').DataTable({
		//	//"data": this.data,
		//	//"columns": columns,
		//	//"order": [[colunmSortIndex, JSON.parse(this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']],
		//	//"paging": false,
		//	////"searching": false,
		//	////"bInfo": false,
		//	"responsive": true,
		//	//"columnDefs": [{
		//	//	"className": 'control',
		//	//	"orderable": false,
		//	//	"targets": -1
		//	//}],
		//	//"fixedHeader": {
		//	//	"header": true,
		//	//	"footer": true
		//	//},
		//	//"initComplete": () => {
		//	//	//this.$(window).scrollTop(0);
		//	//}
		//});

		//this.$('.tblRpt thead').on('click', 'th.details-control', event => this.onClickDetailControlOnHeader(event));
		//this.$('.tblRpt tbody').on('click', 'td.details-control', event => this.onClickDetailControlOnBody(event, true));
		//this.$('.tblRpt tfoot').on('click', 'td.details-control', event => this.onClickDetailControlOnFooter(event));
		//this.$('.tblRpt').on('draw.dt', () => this.triggerRedrawTable());
	}

	formatDetailRow = (data, parentRow) => {
		let rows = [];
		const heigh = this.$(parentRow).height();
		for (let i = 2; i < viewModel.DisplayOutstandingLevels.length; i++) {
			const level = `${viewModel.DisplayOutstandingLevels[i].DisplayName}Outstanding`;
			rows.push(`<div class="child-row" style="height: ${heigh}px"><span>${viewModel.DisplayOutstandingLevels[i].DisplayName}</span><span>${data[level]}</span></div>`);
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

		this.doAdjustHeader();
	}

	onClickDetailControlOnBody = (event, adjustHeader: boolean = false) => {
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

		if (adjustHeader) {
			this.doAdjustHeader();
		}
		else {
			this.$(window).scrollTop(this.$(window).scrollTop() + 1);
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

		this.doAdjustHeader();

		this.replaceHrefParam("SortPropertyName", this.getColumnSortName());
		this.replaceHrefParam("IsDescendingSort", this.isDescendingSort());
	}

	doAdjustHeader = () => {
		this.$table.fixedHeader.adjust();

		let scrollTo = this.$('.fixedHeader-floating thead').length > 0 ? this.$(document).height() : 0;
		this.$(window).scrollTop(scrollTo);
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
	const outstandingViewNew1 = new OutstandingViewNew1(
        $,
        new ProductMultiSelectionComponent($),
        linkBuilderComponent);

	outstandingViewNew1.init();
});
