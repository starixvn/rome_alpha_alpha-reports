// winLossDetailView.$reportForm.trigger("submit") make error
// error: Timeout occurred when executing test file
// while Running: alpha-reports\src\Alpha.Reports.Site\assets\scripts\tests\apps\winlossdetail\winlossdetail-tests.ts
describe("WinLossDetail", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const pagerComponent = new PagerComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);

    let winLossDetailView: WinLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy;

    beforeEach(() => {
        winLossDetailView = new WinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            pagerComponent,
            productMultiSelectionComponent,
            sortComponent,
            reportTooltip,
            monthPickerComponent,
            netTurnoverTooltipComponent,
            linkBuilderComponent);

        cacheElements = spyOn(winLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(winLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });

    describe("_registerEvents_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();

            spyOn(winLossDetailView, "onSwitchReport");
            spyOn(winLossDetailView, "onClickIconFilter");
            spyOn(winLossDetailView, "onSwitchMode");
            spyOn(winLossDetailView, "onExportExcel");
            spyOn(winLossDetailView, "onSubmitReport");

            // act
            winLossDetailView.registerEvents();
        });

        it("_Bind onSwitchReport", () => {
            // act
            winLossDetailView.$reportSwitch.trigger("click");

            // assert
            expect(winLossDetailView.onSwitchReport).toHaveBeenCalled();
        });
        it("_Bind onClickIconFilter", () => {
            // act
            $(".icon-filter").trigger("click");

            // assert
            expect(winLossDetailView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onSwitchMode", () => {
            // act
            winLossDetailView.$(".icon-switchmode").trigger("click");

            // assert
            expect(winLossDetailView.onSwitchMode).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", () => {
            // act
            winLossDetailView.$("#icon-excel").trigger("click");

            // assert
            expect(winLossDetailView.onExportExcel).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", () => {
            // act
            winLossDetailView.$reportForm.trigger("submit");

            // assert
            expect(winLossDetailView.onSubmitReport).toHaveBeenCalled();
        });
    });
});