// winLossDetailView.$reportForm.trigger("submit") make error
// error: Timeout occurred when executing test file
// while Running: alpha-reports\src\Alpha.Reports.Site\assets\scripts\tests\apps\winlossdetail\winlossdetail-tests.ts
describe("WinLossDetail", function () {
    var jQueryFake = $;
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var pagerComponent = new PagerComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var sortComponent = new SortComponent($, linkBuilderComponent);
    var monthPickerComponent = new MonthPickerComponent($);
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);
    var winLossDetailView;
    var cacheElements, registerEvents, blockUI, unblockUI, renderProductMultiSelectionComponent, renderPagerComponent, renderSortComponent;
    beforeEach(function () {
        winLossDetailView = new WinLossDetailView(jQueryFake, dateRangePickerComponent, pagerComponent, productMultiSelectionComponent, sortComponent, reportTooltip, monthPickerComponent, linkBuilderComponent);
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
    describe("_registerEvents_Always", function () {
        beforeEach(function () {
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
        it("_Bind onSwitchReport", function () {
            // act
            winLossDetailView.$reportSwitch.trigger("click");
            // assert
            expect(winLossDetailView.onSwitchReport).toHaveBeenCalled();
        });
        it("_Bind onClickIconFilter", function () {
            // act
            $(".icon-filter").trigger("click");
            // assert
            expect(winLossDetailView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onSwitchMode", function () {
            // act
            winLossDetailView.$(".icon-switchmode").trigger("click");
            // assert
            expect(winLossDetailView.onSwitchMode).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", function () {
            // act
            winLossDetailView.$("#icon-excel").trigger("click");
            // assert
            expect(winLossDetailView.onExportExcel).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", function () {
            // act
            winLossDetailView.$reportForm.trigger("submit");
            // assert
            expect(winLossDetailView.onSubmitReport).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=winlossdetail-registerEvents-tests.js.map