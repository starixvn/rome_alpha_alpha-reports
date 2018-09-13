// winLossDetailView.$reportForm.trigger("submit") make error
// error: Timeout occurred when executing test file
// while Running: alpha-reports\src\Alpha.Reports.Site\assets\scripts\tests\apps\pokergames-winlossdetail\pokergames-winlossdetail-tests.ts
describe("PokerGamesWinLossDetail", () => {
    const jQueryFake = $;
    const pagerComponent = new PagerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);

    let winLossDetailView: PokerGamesWinLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy;

    beforeEach(() => {
        winLossDetailView = new PokerGamesWinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            pagerComponent,
            sortComponent,
            reportTooltip,
            monthPickerComponent,
            linkBuilderComponent);

        cacheElements = spyOn(winLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(winLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });

    describe("_registerEvents_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
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