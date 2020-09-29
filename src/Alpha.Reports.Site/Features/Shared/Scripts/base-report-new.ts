class BaseReportNew {
	$: JQueryStatic;

	$filterIcon: JQuery;
	$iconInformation: JQuery;
	$iconInformationContainer: JQuery;
	$isDescendingSort: JQuery;
	$isFilterVisible: JQuery;
	$loadLegendURL: JQuery;
	$pageTitle: JQuery;
	$reportForm: JQuery;
	$reportLegend: JQuery;
	$sortPropertyName: JQuery;
	$submitButton: JQuery;
	$dataTable: JQuery;
	$tableReport: JQuery;

	linkBuilderComponent: LinkBuilderComponent;
	canSort: boolean;
	currentHideState: boolean;

	constructor(
		$: JQueryStatic,
		linkBuilderComponent: LinkBuilderComponent) {
		this.$ = $;
		this.linkBuilderComponent = linkBuilderComponent;
	}

	public init() {
		this.cacheDOMElements();
	}

	public cacheDOMElements() {
		this.$filterIcon = this.$(".icon-filter");
		this.$isDescendingSort = this.$("#IsDescendingSort");
		this.$isFilterVisible = this.$("#IsFilterVisible");
		this.$pageTitle = this.$(".page-title");
		this.$reportForm = this.$("#report-form");
		this.$sortPropertyName = this.$("#SortPropertyName");
		this.$submitButton = this.$(".btn-submit");
		this.$tableReport = this.$("#tbl-report");
		this.canSort = parseInt(this.$tableReport.data('totalrecords'), 0) > 1;
	}

	public registerEvents() {
		this.$filterIcon.on("click", () => this.onClickIconFilter());
	}

	public initHelp = () => {
		this.$loadLegendURL = this.$("#LoadLegendURL");
		this.$iconInformation = this.$(".icon-information");
		this.$iconInformationContainer = this.$(".icon-information-container");
		this.loadLegend();
		this.$iconInformation.on("click", this.showLegend);
	};

	private loadLegend = () => {
		this.$.ajax({
			method: "GET",
			url: this.$loadLegendURL.val(),
			cache: false,
			data: {},
			success: this.renderLegend,
			complete: this.$.unblockUI
		});
	};

	private renderLegend = (html: string) => {
		if (this.$reportLegend) {
			this.$reportLegend.remove();
		}

		this.$("body").append(html);

		if (this.$.trim(html) === "") {
			this.$iconInformationContainer.remove();
		} else {
			this.$iconInformationContainer.removeClass("hide");
		}

		this.$reportLegend = this.$("#report-legend");
	};

	private showLegend = () => {
		this.$reportLegend.modal({ show: true, backdrop: false });

		var iconHelpPosition = this.$iconInformation.position();
		this.$reportLegend.css({
			bottom: "auto",
			top: iconHelpPosition.top + 60
		});
	};

	public isDescendingSort = () => this.$dataTable.order()[0][1] !== 'asc';

	public triggerRedrawTable() {
		this.doAdjustHeader();
		this.$('tbody tr.parent').each((index, element) => this.onClickDetailControl(element));
	}

	private doAdjustHeader = () => {
		this.$dataTable.fixedHeader.adjust();

		const scrollTo = this.$('.fixedHeader-floating thead').length > 0 ? this.$(document).height() : 0;
		this.$(window).scrollTop(scrollTo);
	};

	private onClickFooterRow = (parentRow, childRow) => {
		parentRow.hasClass('parent') ?
			childRow.addClass('tbl-footer') :
			childRow.removeClass('tbl-footer');

		setTimeout(() => {
			const scrollTo = this.$(document).height();
			this.$(window).scrollTop(scrollTo + 15);
		}, 100);
	};

	private onClickBodyRow = (parentRow, childRow) => {
		if (childRow.hasClass('child')) {
			parentRow.hasClass('even') ?
				childRow.removeClass('odd').addClass('even') :
				childRow.removeClass('even').addClass('odd');
		}
	};

	public onClickDetailControl(element) {
		const parentRow = this.$(element).closest('tr');
		const childRow = this.$(parentRow).next();
		parentRow.hasClass('tbl-footer') ?
			this.onClickFooterRow(parentRow, childRow) :
			this.onClickBodyRow(parentRow, childRow);
	};

	public onClickIconFilter() {
		this.currentHideState = this.$reportForm.hasClass("hide");
		this.currentHideState ? this.showFilter() : this.hideFilter();
		this.doAdjustHeader();
	}

	public showFilter = () => {
		this.$pageTitle.removeClass("hide-filter");
		this.$reportForm.removeClass("hide");
	};

	public hideFilter = () => {
		this.$pageTitle.addClass("hide-filter");
		this.$reportForm.addClass("hide");
	};

	public replaceHrefParam = (paramName: string, paramValue: any) => {
		this.linkBuilderComponent.updateParam(paramName, paramValue);
	};

	public initDataTable(options = {}) {
		const tableOptions = this.getTableOptions();
		this.$.extend(tableOptions, options);

		this.$dataTable = this.$tableReport.DataTable(tableOptions);

		this.$tableReport.find('tbody').on('click', 'td.control', event => this.onClickDetailControl(event.currentTarget));
		this.$dataTable.on('draw.dt', () => this.triggerRedrawTable());
	}

	private getTableOptions = () => {
		const self = this;

		return {
			language: {
				processing: '',
				emptyTable: this.$('#NoInformationIsAvailable').val()
			},
			responsive: {
				details: {
					type: 'column',
					target: -1,
					renderer: (api, rowIdx) => {
						const data = api.cells(rowIdx, ':hidden').eq(0).map(function (cell) {
							const header = self.$(api.column(cell.column).header());
							const idx = api.cell(cell).index();

							if (header.hasClass('control') || header.hasClass('never')) {
								return '';
							}

							const dtPrivate = api.settings()[0];
							const cellData = dtPrivate.oApi._fnGetCellData(
								dtPrivate, idx.row, idx.column, 'display'
							);

							return self.renderData(idx.column, header.text(), cellData);
						}).toArray().join('');

						return self.renderDetail(rowIdx, data);
					}
				}
			},
			columnDefs: [{
				className: 'control',
				orderable: false,
				targets: -1
			}],
			ordering: this.canSort,
			paging: false,
			searching: false,
			bInfo: false,
			fixedHeader: true,
			processing: true,
			drawCallback: this.drawCallback
		}
	};

	public renderData = (idxColumn: any, headerText: string, cellData: any) => `<li data-dtr-index="${idxColumn}">` +
		`<span class="dtr-title">${headerText}</span> ` +
		`<span class="dtr-data">${cellData}</span></li>`;

	public renderDetail = (rowIdx: any, data: any) => data ?
		this.$(`<ul data-dtr-index="${rowIdx}"/>`).append(data) :
		false;

	public drawCallback = () => {
		this.$('tbody').append(this.$('tr.tbl-footer'));
		this.$tableReport.removeClass('hidden');
	};
}