describe("EnterResult_", () => {
    const jQueryFake = $;

    const sortComponent = new SortComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
  
    let enterResultView: EnterResultView;

    let cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderSortComponent: jasmine.Spy;

    beforeEach(() => {
        enterResultView = new EnterResultView(
            jQueryFake,
            dateRangePickerComponent,
            sortComponent);

        cacheDOMElements = spyOn(enterResultView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();

        registerEvents = spyOn(enterResultView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderSortComponent = spyOn(sortComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });

    describe("init", () => {
        it("Document is not ready -> Does not call any Method", () => {
            // act
            enterResultView.init();

            // assert
            expect(enterResultView.cacheDOMElements).not.toHaveBeenCalled();
            expect(enterResultView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
            expect(sortComponent.render).not.toHaveBeenCalled();
        });
    });
});