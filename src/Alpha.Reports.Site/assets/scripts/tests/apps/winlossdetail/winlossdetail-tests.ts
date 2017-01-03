describe("WinLossDetail_", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);

    const winLossDetailView = new WinLossDetailView(
        jQueryFake,
        dateRangePickerComponent,
        pagerComponent,
        productMultiSelectionComponent,
        sortComponent,
        new ReportTooltip($),
        monthPickerComponent);

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderMonthPickerComponent: jasmine.Spy,
        renderdateRangePickerComponent: jasmine.Spy;

    beforeEach(() => {
        cacheElements = spyOn(winLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(winLossDetailView, "registerEvents");
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        renderdateRangePickerComponent = spyOn(dateRangePickerComponent, "render");
    });

    describe("init_", () => {
        it("Document is not ready -> Does not call any Method", () => {
            // act
            winLossDetailView.init();

            // assert
            expect(winLossDetailView.cacheElements).not.toHaveBeenCalled();
            expect(winLossDetailView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();

            expect(productMultiSelectionComponent.render).not.toHaveBeenCalled();
            expect(pagerComponent.render).not.toHaveBeenCalled();
            expect(sortComponent.render).not.toHaveBeenCalled();
        });
    });

    describe("init_", () => {
        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);

            // act
            winLossDetailView.init();

            // assert
            expect(winLossDetailView.cacheElements).toHaveBeenCalled();
            expect(winLossDetailView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();

            expect(productMultiSelectionComponent.render).toHaveBeenCalled();
            expect(pagerComponent.render).toHaveBeenCalled();
            expect(sortComponent.render).toHaveBeenCalled();
        });
    });

    describe("init_", () => {
        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);
            renderMonthPickerComponent = spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });
    });
    describe("init_", () => {
        it("Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            renderMonthPickerComponent = spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(monthPickerComponent.render).toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
        });
    });
    describe("init_", () => {
        // Historic report: we must show filter before render month picker
        it("Historic Report With Hidden Filter -> Shows Filter Before Renders Month Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);

            let actualIsHidden;
            spyOn(monthPickerComponent, "render").and.callFake(() => {
                actualIsHidden = $("#report-form").hasClass("hide");
            });

            // act
            winLossDetailView.init();

            // assert
            expect(actualIsHidden).toBeFalsy();
        });
    });
    describe("init_", () => {
        // Historic report: we must show filter before render month picker
        // Therefore, after init we also hide it if before that it is hidden
        it("Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render").and.callThrough();

            // act
            winLossDetailView.init();
            var actualIsHidden = $("#report-form").hasClass("hide");

            // assert
            expect(actualIsHidden).toBeTruthy();
        });
    });
});