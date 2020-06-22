/// <reference path="../../../assets/scripts/core.ts" />

"use strict";

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
		this.$.blockUI();
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

    registerEvents = () =>  {
        super.registerEvents();
		this.$reportForm.submit(this.onSubmitReport);
    };

	initDataTable = () => {
		const self = this;
		this.$table = this.$('#example').DataTable({
			responsive: {
				details: {
					type: 'column',
					target: -1,
					renderer: function (api, rowIdx) {
						var data = api.cells(rowIdx, ':hidden').eq(0).map(function (cell) {
							var header = self.$(api.column(cell.column).header());
							var idx = api.cell(cell).index();

							if (header.hasClass('control') || header.hasClass('never')) {
								return '';
							}

							var dtPrivate = api.settings()[0];
							var cellData = dtPrivate.oApi._fnGetCellData(
								dtPrivate, idx.row, idx.column, 'display'
							);

							var title = header.text();

							return '<li data-dtr-index="' + idx.column + '">' +
								'<span class="dtr-title">' +
								title +
								'</span> ' +
								'<span class="dtr-data">' +
								cellData +
								'</span>' +
								'</li>';
						}).toArray().join('');

						return data ?
							self.$('<ul data-dtr-index="' + rowIdx + '"/>').append(data) :
							false;
					}
				}
			},
			columnDefs: [{
				className: 'control',
				orderable: false,
				targets: -1
			}],
			paging: false,
			searching: false,
			bInfo: false,
			fixedHeader: {
				header: true,
				footer: true
			},
			initComplete: () => {
				//this.$.unblockUI();
			},
			drawCallback: () => {
				this.$('tbody').append(this.$('tr.tbl-footer'));
			}
		});

		this.$('#example thead').on('click', 'th.control', event => this.onClickDetailControlOnHeader(event));
		this.$('#example tbody').on('click', 'td.control', event => this.onClickDetailControlOnBody(event, true));
		this.$('#example tbody tr.tbl-footer').on('click', 'td.control', event => this.onClickDetailControlOnFooter(event));
		this.$('#example').on('draw.dt', () => this.triggerRedrawTable());
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

		if (tr.hasClass('parent')) {
			tr.removeClass('parent');

			this.$('tbody tr.parent').each((index, element) => {
				this.$(element).find('.control').click();
			});
		}
		else {
			tr.addClass('parent');

			this.$('tbody tr').not('.parent').each((index, element) => {
				this.$(element).find('.control').click();
			});
		}

		this.doAdjustHeader();
	}

	onClickDetailControlOnBody = (event, adjustHeader: boolean = false) => {
		var tr = this.$(event.currentTarget).closest('tr');
		if (tr.hasClass('tbl-footer')) {
			tr.hasClass('parent') ?
				this.$(tr).next().addClass('tbl-footer') :
				this.$(tr).next().removeClass('tbl-footer');
		}
		else {
			const rowNext = this.$(tr).next();
			if (rowNext.hasClass('child')) {
				if (tr.hasClass('even')) {
					this.$(tr).next().removeClass('odd').addClass('even');
				}
				else {
					this.$(tr).next().removeClass('even').addClass('odd');
				}
			}
		}
	}

	onClickDetailControlOnFooter = (event) => {
		var tr = this.$(event.currentTarget).closest('tr');
		setTimeout(() => {
			const scrollTo = this.$(document).height();
			this.$(window).scrollTop(scrollTo + 15);
		}, 100);
		
	}

	triggerRedrawTable = () => {
		this.$('td.control').each((index, element) => {
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
