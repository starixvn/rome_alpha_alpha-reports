/// <reference path="../../../assets/scripts/core.ts" />

"use strict";

declare var displayOutstandingLevels;

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

	canSort: boolean;
	dtResizeTimer: any;
	allowPropagation: any;

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
		this.canSort = JSON.parse(this.$('#CanSort').val().toLowerCase());

		this.initDataTable();
		this.linkBuilderComponent.render();
		this.registerEvents();
		this.initHelp();
	};

	cacheElements = () => {
		super.cacheElements();

		this.$reportForm = this.$("#report-form");

		this.$sortPropertyName = this.$("#SortPropertyName");
		this.$isDescendingSort = this.$("#IsDescendingSort");
		this.$isFilterVisible = this.$("#IsFilterVisible");
	};

	makeColumnsResponsive = () => {
	const visibleColumnCount = this.$('tbody tr:first-child td:visible').length - 1;
		this.$('thead tr th').show();
		for (let i = 1; i <= this.$('thead tr').length; i++) {
		this.$('thead tr:nth-child(' + i + ') th:gt(' + visibleColumnCount + ')').hide();
	}
}

	registerEvents = () => {
		super.registerEvents();
		this.$reportForm.submit(this.onSubmitReport);

		//this.$(window).on("resize.DT-visitsTable", (event) => {
		//	this.$.blockUI();
		//	if (this.allowPropagation === false) {
		//		event.stopImmediatePropagation();
		//		clearTimeout(this.dtResizeTimer);
		//		this.dtResizeTimer = setTimeout(function () {
					
		//			this.allowPropagation = true;
		//			this.$(window).trigger("resize.DT-visitsTable");
		//		}, 100);
		//	} else {
		//		this.allowPropagation = false;
		//	}
		//});

		//this.$(window).on('resize.dtr orientationchange.dtr', () => {
		//	this.$table.destroy();
		//	this.$('#example').empty();
			
		//	const frms = window.top.parent.frames;
		//	if (frms && frms["main"]) {
		//		window.location.href = frms["main"].location;
		//	}
		//});

		this.$(window).on('resize.dtr orientationchange.dtr', () => {
			this.$.blockUI();
		});

		//this.$(window).on("resize orientationchange", function (event) {
		//	this.$.blockUI();
		//});

		//this.$('#example').on('.collapsed', function () {
		//	console.log('unblockUI');
		//	this.$.unblockUI();
		//});

		//this.$(window).on('destroy.dtr', () => {
		//	this.$.unblockUI();
		//});
	};

	initDataTable = () => {
		const self = this;

		let colunmSortIndex = 0;
		this.$.each(displayOutstandingLevels, (key, value) => {
			if (this.$(this.$sortPropertyName).val().toLowerCase() === `${value.DisplayName}Outstanding`.toLowerCase()) {
				colunmSortIndex = key + 1;
				return;
			}
		});
		
		this.$table = this.$('#example').DataTable({
			responsive: {
				details: {
					type: 'column',
					target: -1,
					auto: false,
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

							return `<li data-dtr-index="${idx.column}">` +
								`<span class="dtr-title">${title}</span> ` +
								`<span class="dtr-data">${cellData}</span></li>`;
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
			ordering: this.canSort,
			order: [[colunmSortIndex, JSON.parse(this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']],
			paging: false,
			searching: false,
			bInfo: false,
			fixedHeader: true,
			initComplete: () => {
				//this.$.unblockUI();
			},
			drawCallback: () => {
				this.$('tbody').append(this.$('tr.tbl-footer'));
			}
		});

		this.$('#example thead').on('click', 'th.control', event => this.onClickDetailControlOnHeader(event));
		this.$('#example tbody').on('click', 'td.control', event => this.onClickDetailControlOnBody(event));
		this.$('#example tbody tr.tbl-footer').on('click', 'td.control', () => this.onClickDetailControlOnFooter());
		this.$('#example').on('draw.dt', () => this.triggerRedrawTable());

		this.$('#example').on('responsive-resize.dt', function () {
			console.log(' column(s) are hidden');
		});
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

	onClickDetailControlOnBody = (event) => {
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

	onClickDetailControlOnFooter = () => {
		setTimeout(() => {
			const scrollTo = this.$(document).height();
			this.$(window).scrollTop(scrollTo + 15);
		}, 100);
	}

	triggerRedrawTable = () => {
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
		const order = this.$table.order();
		const length = displayOutstandingLevels.length;

		for (let index = 0; index < length; index++) {
			if (order[0][0] - 1 === index) {
				return `${displayOutstandingLevels[index].DisplayName}Outstanding`;
			}
		}

		return 'CustName';
	}

	isDescendingSort = () => {
		const order = this.$table.order();
		return order[0][1] !== 'asc';
	}

	onSubmitReport = (e: any) => {
		this.$.blockUI();

		var params = this.getParams();
		if (!params.IsAnyProductSelected) {
			this.showValidateProductError();
			this.$.unblockUI();
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
		const selectedProducts = this.productMultiSelectionComponent.getSelectedItems();

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
