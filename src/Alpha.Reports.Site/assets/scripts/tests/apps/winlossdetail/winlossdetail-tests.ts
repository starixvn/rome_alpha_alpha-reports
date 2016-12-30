describe("WinLossDetail_", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($);

    const winLossDetailView = new WinLossDetailView(
        jQueryFake,
        new DateRangePickerComponent($),
        pagerComponent,
        productMultiSelectionComponent,
        sortComponent,
        new ReportTooltip($),
        new MonthPickerComponent($));

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy;

    beforeEach(() => {
        setFixtures(
            `<body>
                <div id="dropdown-product"></div>
                <div id="netturnovertooltip"></div>
                <input id="IsHistoricReport" name="IsHistoricReport" value="False" data-val="False" type="hidden" />
              </body>`);

        cacheElements = spyOn(winLossDetailView, "cacheElements");
        registerEvents = spyOn(winLossDetailView, "registerEvents");
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
    });

    describe("init_", () => {
        it("Always -> Calls Specific Methods", () => {
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
});