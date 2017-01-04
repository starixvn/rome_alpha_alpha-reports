// winLossDetailView.$reportForm.trigger("submit") make error
// error: Timeout occurred when executing test file
// while Running:D:\Nexcel\GitLab\Rome\AgencyI1\alpha-reports\src\Alpha.Reports.Site\assets\scripts\tests\apps\winlossdetail\winlossdetail-tests.ts
describe("WinLossDetail_", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);

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
            netTurnoverTooltipComponent);

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

    describe("registerEvents", () => {
        it("Always -> All neccessary functions have been registered", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();

            spyOn(winLossDetailView, "onSwitchReport");
            spyOn(winLossDetailView, "onClickIconFilter");
            spyOn(winLossDetailView, "onSwitchMode");
            spyOn(winLossDetailView, "onExportExcel");

            // act
            winLossDetailView.registerEvents();
            winLossDetailView.$reportSwitch.trigger("click");
            $(".icon-filter").trigger("click");
            winLossDetailView.$(".icon-switchmode").trigger("click");
            winLossDetailView.$("#icon-excel").trigger("click");

            // assert
            expect(winLossDetailView.onSwitchReport).toHaveBeenCalled();
            expect(winLossDetailView.onClickIconFilter).toHaveBeenCalled();
            expect(winLossDetailView.onSwitchMode).toHaveBeenCalled();
            expect(winLossDetailView.onExportExcel).toHaveBeenCalled();
        });
        it("Always -> Submit functions have been registered", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();

            spyOn(winLossDetailView, "onSubmitReport");

            // act
            winLossDetailView.registerEvents();
            winLossDetailView.$reportForm.trigger("submit");

            // assert
            expect(winLossDetailView.onSubmitReport).toHaveBeenCalled();
        });
    });
});