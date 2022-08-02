// matchWinLossDetailView.$reportForm.trigger("submit") make error
// error: Timeout occurred when executing test file
// while Running: alpha-reports\src\Alpha.Reports.Site\assets\scripts\tests\apps\matchwinlossdetail\matchwinlossdetail-tests.ts
describe("MatchWinLossDetail", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var leagueMultiSelectionComponent = new LeagueMultiSelectionComponent($);
    var reportTooltip = new ReportTooltip($);
    var matchWinLossDetailView;
    var cacheElements, registerEvents, blockUI, unblockUI, stickyTableHeaders, renderProductMultiSelectionComponent, renderLeagueMultiSelectionComponent;
    beforeEach(function () {
        matchWinLossDetailView = new MatchWinLossDetailView(jQueryFake, dateRangePickerComponent, productMultiSelectionComponent, leagueMultiSelectionComponent, reportTooltip);
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
    describe("_registerEvents_Always", function () {
        beforeEach(function () {
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
        it("_Bind onExportExcel", function () {
            // act
            matchWinLossDetailView.$("#icon-excel").trigger("click");
            // assert
            expect(matchWinLossDetailView.onExportExcel).toHaveBeenCalled();
        });
        it("_Bind onSubmitReport", function () {
            // act
            matchWinLossDetailView.$reportForm.trigger("submit");
            // assert
            expect(matchWinLossDetailView.onSubmitReport).toHaveBeenCalled();
        });
        it("_Bind onClickIconFilter", function () {
            // act
            matchWinLossDetailView.$(".icon-filter").trigger("click");
            // assert
            expect(matchWinLossDetailView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onSwitchMode", function () {
            // act
            matchWinLossDetailView.$(".icon-switchmode").trigger("click");
            // assert
            expect(matchWinLossDetailView.onSwitchMode).toHaveBeenCalled();
        });
        it("_Bind loadLeagues", function () {
            // act
            matchWinLossDetailView.$buttonLoadLeague.trigger("click");
            // assert
            expect(matchWinLossDetailView.loadLeagues).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=matchwinlossdetail-registerEvents-tests.js.map