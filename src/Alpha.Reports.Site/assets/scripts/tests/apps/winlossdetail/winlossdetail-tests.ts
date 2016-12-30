describe("WinLossDetail_", () => {
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const jQueryFake = $;

    const winLossDetailView = new WinLossDetailView(
        jQueryFake,
        new DateRangePickerComponent($),
        new PagerComponent($),
        productMultiSelectionComponent,
        new SortComponent($),
        new ReportTooltip($),
        new MonthPickerComponent($));

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy;

    beforeEach(() => {
        setFixtures(`<div id="dropdown-product"></div>`);
        cacheElements = spyOn(winLossDetailView, "cacheElements");
        registerEvents = spyOn(winLossDetailView, "registerEvents");
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
    });

    describe("init_", () => {
        it("Always -> Calls Specific Methods", () => {
            setFixtures(`<div id="dropdown-product"></div>`);

            // act
            winLossDetailView.init();

            // assert
            expect(winLossDetailView.cacheElements).toHaveBeenCalled();
            expect(winLossDetailView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();

            expect(productMultiSelectionComponent.render).toHaveBeenCalled();
        });
    });
});