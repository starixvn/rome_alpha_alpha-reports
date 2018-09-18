describe("ProgresiveContribution_", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);

    let progressiveContributionView: ProgressiveContributionView,
        cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        bindFilter: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        stickyTableHeaders: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        progressiveContributionView = new ProgressiveContributionView(
            jQueryFake,
            dateRangePickerComponent,
            monthPickerComponent,
            productMultiSelectionComponent,
            reportTooltip,
            linkBuilderComponent);

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

    describe("init", () => {
        it("Document is not ready -> Does not call any Method", () => {
            // act
            progressiveContributionView.init();

            // assert
            expect(progressiveContributionView.cacheDOMElements).not.toHaveBeenCalled();
            expect(progressiveContributionView.registerEvents).not.toHaveBeenCalled();
        });

        it("Document is ready -> Calls Specific Methods", () => {
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

        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);

            // act
            progressiveContributionView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });

        it("Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.historicReportWithHiddenFilter);

            // act
            progressiveContributionView.init();

            // assert
            expect(monthPickerComponent.render).toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
        });

        it("Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", () => {
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