describe("Consolidation Detail", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
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
            spyOn(consolidationDetailView, "onSubmitReport");
            spyOn(consolidationDetailView, "onExportExcel");

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
    });
});