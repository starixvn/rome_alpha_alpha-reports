describe("Consolidation", function () {
    var jQueryFake = $;
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var currencySelectionComponent = new CurrencySelectionComponent($);
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var consolidationView;
    var renderProductMultiSelectionComponent, renderCurrencySelectionComponent;
    beforeEach(function () {
        consolidationView = new ConsolidationView(jQueryFake, productMultiSelectionComponent, currencySelectionComponent, dateRangePickerComponent, reportTooltip, linkBuilderComponent);
        var cacheElements = spyOn(consolidationView, "cacheElements");
        cacheElements.and.callThrough();
        var registerEvents = spyOn(consolidationView, "registerEvents");
        registerEvents.and.callThrough();
        spyOn(consolidationView, "initHelp");
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderCurrencySelectionComponent = spyOn(currencySelectionComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });
    describe("_registerEvents_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            consolidationView.cacheElements();
            spyOn(consolidationView, "onClickIconFilter");
            spyOn(consolidationView, "onSwitchMode");
            spyOn(consolidationView, "onSubmitReport");
            spyOn(consolidationView, "onExportExcel");
            spyOn(consolidationView, "onChangeReportType");
            // act
            consolidationView.registerEvents();
        });
        it("_Bind onClickIconFilter", function () {
            // act
            $(".icon-filter").trigger("click");
            // assert
            expect(consolidationView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onSwitchMode", function () {
            // act
            consolidationView.$(".icon-switchmode").trigger("click");
            // assert
            expect(consolidationView.onSwitchMode).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", function () {
            // act
            consolidationView.$("#icon-excel").trigger("click");
            // assert
            expect(consolidationView.onExportExcel).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", function () {
            // act
            consolidationView.$reportForm.trigger("submit");
            // assert
            expect(consolidationView.onSubmitReport).toHaveBeenCalled();
        });
        it("_Change Report Type have been registered", function () {
            // act
            consolidationView.$("[name=\"report-type\"]").trigger("click");
            // assert
            expect(consolidationView.onChangeReportType).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=consolidation-registerEvents-tests.js.map