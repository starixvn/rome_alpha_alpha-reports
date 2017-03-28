// matchWinLossDetailView.$reportForm.trigger("submit") make error
// error: Timeout occurred when executing test file
// while Running: alpha-reports\src\Alpha.Reports.Site\assets\scripts\tests\apps\matchwinlossdetail\matchwinlossdetail-tests.ts
describe("MatchWinLossDetail", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const leagueMultiSelectionComponent = new LeagueMultiSelectionComponent($);
    const reportTooltip = new ReportTooltip($);

    let matchWinLossDetailView: MatchWinLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        stickyTableHeaders: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderLeagueMultiSelectionComponent: jasmine.Spy;

    beforeEach(() => {
        matchWinLossDetailView = new MatchWinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            productMultiSelectionComponent,
            leagueMultiSelectionComponent,
            reportTooltip);

        cacheElements = spyOn(matchWinLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(matchWinLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        stickyTableHeaders = spyOn($.fn, "stickyTableHeaders");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");

        renderLeagueMultiSelectionComponent = spyOn(leagueMultiSelectionComponent, "render");
        renderLeagueMultiSelectionComponent.and.callThrough();

        spyOn(dateRangePickerComponent, "render");
    });

    describe("_registerEvents_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.cacheElements();
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");

            spyOn(matchWinLossDetailView, "onExportExcel");
            spyOn(matchWinLossDetailView, "onSubmitReport");
            spyOn(matchWinLossDetailView, "onClickIconFilter");
            spyOn(matchWinLossDetailView, "onSwitchMode");
            spyOn(matchWinLossDetailView, "loadLeagues");

            // act
            matchWinLossDetailView.registerEvents();
        });

        it("_Bind onExportExcel", () => {
            // act
            matchWinLossDetailView.$("#icon-excel").trigger("click");

            // assert
            expect(matchWinLossDetailView.onExportExcel).toHaveBeenCalled();
        });
        it("_Bind onSubmitReport", () => {
            // act
            matchWinLossDetailView.$reportForm.trigger("submit");

            // assert
            expect(matchWinLossDetailView.onSubmitReport).toHaveBeenCalled();
        });
        it("_Bind onClickIconFilter", () => {
            // act
            matchWinLossDetailView.$(".icon-filter").trigger("click");

            // assert
            expect(matchWinLossDetailView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onSwitchMode", () => {
            // act
            matchWinLossDetailView.$(".icon-switchmode").trigger("click");

            // assert
            expect(matchWinLossDetailView.onSwitchMode).toHaveBeenCalled();
        });
        it("_Bind loadLeagues", () => {
            // act
            matchWinLossDetailView.$buttonLoadLeague.trigger("click");

            // assert
            expect(matchWinLossDetailView.loadLeagues).toHaveBeenCalled();
        });
    });
});