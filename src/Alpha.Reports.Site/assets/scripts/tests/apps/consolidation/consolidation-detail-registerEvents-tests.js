describe("Consolidation Detail", function () {
    var jQueryFake = $;
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var levelTypeComponent = new SingleSelectionDropdownComponent($);
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var pagerComponent = new PagerComponent($);
    var sortComponent = new SortComponent($, linkBuilderComponent);
    var consolidationDetailView;
    var renderProductMultiSelectionComponent, renderCurrencySelectionComponent, renderSortComponent, renderPagerComponent;
    beforeEach(function () {
        consolidationDetailView = new ConsolidationDetailView(jQueryFake, productMultiSelectionComponent, levelTypeComponent, dateRangePickerComponent, reportTooltip, linkBuilderComponent, pagerComponent, sortComponent);
        var cacheElements = spyOn(consolidationDetailView, "cacheElements");
        cacheElements.and.callThrough();
        var registerEvents = spyOn(consolidationDetailView, "registerEvents");
        registerEvents.and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        spyOn(dateRangePickerComponent, "render");
        spyOn(netTurnoverTooltipComponent, "render");
    });
    describe("_registerEvents_Always", function () {
        beforeEach(function () {
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
        it("_Bind onClickIconFilter", function () {
            // act
            $(".icon-filter").trigger("click");
            // assert
            expect(consolidationDetailView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onSwitchMode", function () {
            // act
            consolidationDetailView.$(".icon-switchmode").trigger("click");
            // assert
            expect(consolidationDetailView.onSwitchMode).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", function () {
            // act
            consolidationDetailView.$reportForm.trigger("submit");
            // assert
            expect(consolidationDetailView.onSubmitReport).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", function () {
            // act
            consolidationDetailView.$("#icon-excel").trigger("click");
            // assert
            expect(consolidationDetailView.onExportExcel).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=consolidation-detail-registerEvents-tests.js.map