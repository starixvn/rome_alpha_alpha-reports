/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/link-builder.ts" />

"use strict";

class PlayForFunView extends BaseReport {
    dateRangePickerComponent: DateRangePickerComponent;
    sortComponent: SortComponent;
    productSelectionComponent: SingleSelectionDropdownComponent;
    statusSelectionComponent: SingleSelectionDropdownComponent;

    $isDescendingSort: JQuery;
    $sortPropertyName: JQuery;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponent,
        sortComponent: SortComponent,
        productSelectionComponent: SingleSelectionDropdownComponent,
        statusSelectionComponent: SingleSelectionDropdownComponent,
        linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);

        this.dateRangePickerComponent = dateRangePickerComponent;
        this.sortComponent = sortComponent;
        this.productSelectionComponent = productSelectionComponent;
        this.statusSelectionComponent = statusSelectionComponent;
    }

    init = () => {
        this.$.blockUI();
        this.cacheElements();

        this.productSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-product", "UserSelectedProductId"));
        this.statusSelectionComponent.render(new SingleSelectionDropdownOption("dropdown-statuses", "UserSelectedStatusId"));
        this.dateRangePickerComponent.render();
        this.initTableReport();
        this.linkBuilderComponent.render();

        this.registerEvents();
        this.$.unblockUI();
    };
    public cacheElements() {
        super.cacheElements();

        this.$isDescendingSort = this.$("#IsDescendingSort");
        this.$sortPropertyName = this.$("#SortPropertyName");
    }
    public registerEvents() {
        super.registerEvents();

        let self = this;
        this.$reportForm.submit(() => self.onSubmitReport());
        this.$("#icon-excel").on("click", this.onExportExcel);
    }
    public getSorterHeader() {
        return { 0: { sorter: "text" } };
    }
    public onSubmitReport() {
        this.$.blockUI();
        let params = this.getParams();

        this.$isFilterVisible.val(String(alphaReports.screenSmallThan768() ? false : params.IsFilterVisible));
        this.productSelectionComponent.updateSelectedValue();
        this.statusSelectionComponent.updateSelectedValue();
        this.$isDescendingSort.val(String(params.IsDescendingSort));
        this.$sortPropertyName.val(params.SortPropertyName);
    }

    initTableReport = () => {
        let canSort = parseInt(this.$tableReport.data("totalrecords"), 0) > 1;
        this.sortComponent.render(new SortOption(
            this.$tableReport,
            canSort,
            true,
            this.sortComponent.afterSort,
            this.getSorterHeader(),
            this.$reportForm));

        this.$tableReport.stickyTableHeaders();
    };
    getParams = () => {
        let sortInfo = this.sortComponent.getSortInfo();

        return {
            IsFilterVisible: !this.$reportForm.hasClass("hide"),
            SortPropertyName: sortInfo.sortPropertyName,
            IsDescendingSort: sortInfo.isDescendingSort
        };
    };
    onExportExcel = () => {
        this.$exportExcelForm.submit();
        this.$.unblockUI();
    };
}