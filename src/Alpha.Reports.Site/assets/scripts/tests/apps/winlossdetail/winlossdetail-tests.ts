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
            winLossDetailView.init(false);

            // assert
            expect(winLossDetailView.cacheElements).not.toHaveBeenCalled();
        });
    });

    describe("init_", () => {
        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);

            // act
            winLossDetailView.init(true);

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
            winLossDetailView.init(true);

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });
    });
    describe("init_", () => {
        it("Historic Report -> Renders Month Picker And Not Render Date Range Picker", () => {
            console.log("1234");
            // arrange
            setFixtures(winLossDetailTemplate.historicReportHasHiddenFilter);

            let reportForm = $("#report-form");
            spyOn(winLossDetailView, "cacheReportForm").and.callFake(function () {
                winLossDetailView.$reportForm = reportForm;
            });

            let expectedCallOrder = "Shows $reportForm > Renders monthPickerComponent > Hides $reportForm";

            let actualCallOrder = "";
            spyOn(reportForm, "removeClass").and.callFake(function (className) {
                actualCallOrder += (className == "hide") ? "Shows $reportForm" : "";
            });

            spyOn(reportForm, "addClass").and.callFake(function (className) {
                actualCallOrder += (className == "hide") ? " > Hides $reportForm" : "";
            });

            spyOn(monthPickerComponent, "render").and.callFake(function () {
                actualCallOrder += " > Renders monthPickerComponent";
            });

            // act
            winLossDetailView.init(true);

            // assert
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
            expect(expectedCallOrder).toEqual(actualCallOrder);
        });
    });
});