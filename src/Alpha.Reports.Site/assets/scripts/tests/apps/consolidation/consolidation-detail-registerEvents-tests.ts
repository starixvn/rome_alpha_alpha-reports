describe("Consolidation Detail", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const currencySelectionComponent = new CurrencySelectionComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const linkBuilderComponent = new LinkBuilderComponent($);

    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);
    let consolidationDetailView: ConsolidationDetailView;

    let renderProductMultiSelectionComponent: jasmine.Spy,
        renderCurrencySelectionComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy;

    beforeEach(() => {
        consolidationDetailView = new ConsolidationDetailView(
            jQueryFake,
            productMultiSelectionComponent,
            currencySelectionComponent,
            dateRangePickerComponent,
            reportTooltip,
            linkBuilderComponent,
            pagerComponent,
            sortComponent);

        let cacheElements = spyOn(consolidationDetailView, "cacheElements");
        cacheElements.and.callThrough();

        let registerEvents = spyOn(consolidationDetailView, "registerEvents");
        registerEvents.and.callThrough();

        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderCurrencySelectionComponent = spyOn(currencySelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        spyOn(dateRangePickerComponent, "render");
        spyOn(netTurnoverTooltipComponent, "render");
    });

    describe("_registerEvents_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);
            consolidationDetailView.cacheElements();

            spyOn(consolidationDetailView, "onClickIconFilter");
            spyOn(consolidationDetailView, "onSwitchMode");
            spyOn(consolidationDetailView, "onClickSubmitButton");
            spyOn(consolidationDetailView, "onSubmitReport");
            spyOn(consolidationDetailView, "onExportExcel");
            spyOn(consolidationDetailView, "onChangeReportType");

            // act
            consolidationDetailView.registerEvents();
        });

        it("_Bind onClickIconFilter", () => {
            // act
            $(".icon-filter").trigger("click");

            // assert
            expect(consolidationDetailView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onSwitchMode", () => {
            // act
            consolidationDetailView.$(".icon-switchmode").trigger("click");

            // assert
            expect(consolidationDetailView.onSwitchMode).toHaveBeenCalled();
        });
        it("_Bind onClickSubmitButton", () => {
            // act
            consolidationDetailView.$(".btn-submit").trigger("click");

            // assert
            expect(consolidationDetailView.onClickSubmitButton).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", () => {
            // act
            consolidationDetailView.$reportForm.trigger("submit");

            // assert
            expect(consolidationDetailView.onSubmitReport).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", () => {
            // act
            consolidationDetailView.$("#icon-excel").trigger("click");

            // assert
            expect(consolidationDetailView.onExportExcel).toHaveBeenCalled();
        });
        it("_Change Report Type have been registered", () => {
            // act
            consolidationDetailView.$("[name=\"report-type\"]").trigger("click");

            // assert
            expect(consolidationDetailView.onChangeReportType).toHaveBeenCalled();
        });
    });
});