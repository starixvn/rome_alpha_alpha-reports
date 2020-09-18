/// <reference path="../../../assets/scripts/core.ts" />
/// <reference path="../../shared/scripts/base-report-new.ts" />

"use strict";

declare var displayOutstandingLevels;

interface Window {
	$: JQueryStatic;
}

class OutstandingViewNew extends BaseReportNew {
	$: JQueryStatic;

	productMultiSelectionComponent: ProductMultiSelectionComponent;
	linkBuilderComponent: LinkBuilderComponent;
	$buttonProduct: JQuery;
	displayOutstandingLevels: any;

	constructor(
		$: JQueryStatic,
		productMultiSelectionComponent: ProductMultiSelectionComponent,
		linkBuilderComponent: LinkBuilderComponent,
		displayOutstandingLevels: any) {
		super($, linkBuilderComponent);
		this.productMultiSelectionComponent = productMultiSelectionComponent;
		this.displayOutstandingLevels = displayOutstandingLevels;
	}

	init = () => {
		super.init();
		
		this.productMultiSelectionComponent.render();
		this.$buttonProduct = this.$("#dropdown-product").parent().find(".multiselect.btn");

		this.linkBuilderComponent.render();
		this.registerEvents();
		this.initHelp();
		this.initDataTable();
	};

	registerEvents = () => {
		super.registerEvents();
		this.$reportForm.submit(this.onSubmitReport);
	};

	initDataTable = () => {
		let colunmSortIndex = 0;
		this.$.each(this.displayOutstandingLevels, (key, value) => {
			if (this.$(this.$sortPropertyName).val().toLowerCase() === `${value.DisplayName}Outstanding`.toLowerCase()) {
				colunmSortIndex = key + 1;
				return;
			}
		});

		super.initDataTable({ order: [[colunmSortIndex, JSON.parse(this.$isDescendingSort.val().toLowerCase()) ? 'desc' : 'asc']],})
	}

	triggerRedrawTable = () => {
		super.triggerRedrawTable();

		this.replaceHrefParam("SortPropertyName", this.getColumnSortName());
		this.replaceHrefParam("IsDescendingSort", this.isDescendingSort());
	}

	getColumnSortName = () => {
		const order = this.$dataTable.order();
		const length = this.displayOutstandingLevels.length;

		for (let index = 0; index < length; index++) {
			if (order[0][0] - 1 === index) {
				return `${this.displayOutstandingLevels[index].DisplayName}Outstanding`;
			}
		}

		return 'CustName';
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
	const outstandingViewNew = new OutstandingViewNew(
		$,
		new ProductMultiSelectionComponent($),
		linkBuilderComponent,
		displayOutstandingLevels);

	outstandingViewNew.init();
});
