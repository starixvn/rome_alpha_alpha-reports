describe("ProgresiveContribution_", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var monthPickerComponent = new MonthPickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var progressiveContributionView, cacheDOMElements, registerEvents, bindFilter, blockUI, unblockUI, stickyTableHeaders, renderProductMultiSelectionComponent, renderLinkBuilderComponent;
    beforeEach(function () {
        progressiveContributionView = new ProgressiveContributionView(jQueryFake, dateRangePickerComponent, monthPickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent);
        cacheDOMElements = spyOn(progressiveContributionView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();
        registerEvents = spyOn(progressiveContributionView, "registerEvents");
        registerEvents.and.callThrough();
        bindFilter = spyOn(progressiveContributionView, "bindFilter");
        bindFilter.and.callThrough();
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        stickyTableHeaders = spyOn($.fn, "stickyTableHeaders");
        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderProductMultiSelectionComponent.and.callThrough();
        spyOn(dateRangePickerComponent, "render");
        spyOn(monthPickerComponent, "render");
    });
    describe("init", function () {
        it("Document is not ready -> Does not call any Method", function () {
            // act
            progressiveContributionView.init();
            // assert
            expect(progressiveContributionView.cacheDOMElements).not.toHaveBeenCalled();
            expect(progressiveContributionView.registerEvents).not.toHaveBeenCalled();
        });
        it("Document is ready -> Calls Specific Methods", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            // act
            progressiveContributionView.init();
            // assert
            expect(progressiveContributionView.cacheDOMElements).toHaveBeenCalled();
            expect(progressiveContributionView.registerEvents).toHaveBeenCalled();
            expect(progressiveContributionView.bindFilter).toHaveBeenCalled();
            expect($.fn.stickyTableHeaders).toHaveBeenCalled();
        });
        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            // act
            progressiveContributionView.init();
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });
        it("Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.historicReportWithHiddenFilter);
            // act
            progressiveContributionView.init();
            // assert
            expect(monthPickerComponent.render).toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
        });
        it("Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.historicReportWithHiddenFilter);
            // act
            progressiveContributionView.init();
            var actualIsHidden = $("#report-form").hasClass("hide");
            // assert
            expect(actualIsHidden).toBeTruthy();
        });
    });
});
//# sourceMappingURL=progressivecontribution-init-tests.js.map