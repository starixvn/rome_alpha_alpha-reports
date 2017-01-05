describe("WinLoss_", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const sortComponent = new SortComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);

    let winLossView: WinLossView;

    let cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy;

     beforeEach(() => {
         winLossView = new WinLossView(
             jQueryFake,
             dateRangePickerComponent,
             productMultiSelectionComponent,
             sortComponent,
             monthPickerComponent);

         cacheDOMElements = spyOn(winLossView, "cacheDOMElements");
         cacheDOMElements.and.callThrough();

         registerEvents = spyOn(winLossView, "registerEvents");
         registerEvents.and.callThrough();

         blockUI = spyOn(jQueryFake, "blockUI");
         unblockUI = spyOn(jQueryFake, "unblockUI");

         renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
         renderSortComponent = spyOn(sortComponent, "render");
         spyOn(dateRangePickerComponent, "render");
     });

     describe("init", () => {
         it("Document is not ready -> Does not call any Method", () => {
             // act
             winLossView.init();

             // assert
             expect(winLossView.cacheDOMElements).not.toHaveBeenCalled();
             expect(winLossView.registerEvents).not.toHaveBeenCalled();
             expect(jQueryFake.blockUI).not.toHaveBeenCalled();
             expect(jQueryFake.unblockUI).not.toHaveBeenCalled();

             expect(productMultiSelectionComponent.render).not.toHaveBeenCalled();
             expect(sortComponent.render).not.toHaveBeenCalled();
         });

         it("Document is ready -> Calls Specific Methods", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);

             // act
             winLossView.init();

             // assert
             expect(winLossView.cacheDOMElements).toHaveBeenCalled();
             expect(winLossView.registerEvents).toHaveBeenCalled();
             expect(jQueryFake.blockUI).toHaveBeenCalled();
             expect(jQueryFake.unblockUI).toHaveBeenCalled();

             expect(productMultiSelectionComponent.render).toHaveBeenCalled();
             expect(sortComponent.render).toHaveBeenCalled();
         });

         it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             spyOn(monthPickerComponent, "render");

             // act
             winLossView.init();

             // assert
             expect(dateRangePickerComponent.render).toHaveBeenCalled();
             expect(monthPickerComponent.render).not.toHaveBeenCalled();
         });

         it("Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", () => {
             // arrange
             setFixtures(winLossTemplate.historicReportWithHiddenFilter);
             spyOn(monthPickerComponent, "render");

             // act
             winLossView.init();

             // assert
             expect(monthPickerComponent.render).toHaveBeenCalled();
             expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
         });

         it("Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", () => {
             // arrange
             setFixtures(winLossTemplate.historicReportWithHiddenFilter);
             spyOn(monthPickerComponent, "render").and.callThrough();

             // act
             winLossView.init();
             var actualIsHidden = $("#report-form").hasClass("hide");

             // assert
             expect(actualIsHidden).toBeTruthy();
         });

         it("Always -> Render sort component with correct options", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);

             // act
             winLossView.init();

             // assert
             expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                 winLossView.$tableReport,
                 false,
                 true,
                 function () { },
                 { 0: { sorter: "text" } }
             ));
         });
     });

     describe("checkReady", () => {
         it("Document is not loaded yet (empty) -> Returns False", () => {
             // arrange
             setFixtures("");

             // act
             var actualIsReady = winLossView.checkReady();

             // assert
             expect(actualIsReady).toBeFalsy();
         });

         it("Document is loaded (ready) -> Returns True", () => {
             // arrange
             setFixtures(winLossDetailTemplate.currentReportWithNormalMode);

             // act
             var actualIsReady = winLossView.checkReady();

             // assert
             expect(actualIsReady).toBeTruthy();
         });
     });

     describe("cacheDOMElements", () => {
         it("Always -> All neccessary elements have been cached", () => {
             // arrange
             setFixtures(winLossDetailTemplate.currentReportWithNormalMode);

             // act
             winLossView.cacheDOMElements();

             // assert
             expect(winLossView.$pageTitle).toBeDefined();
             expect(winLossView.$tableReport).toBeDefined();
             expect(winLossView.$drilldownLinks).toBeDefined();
             expect(winLossView.$isFilterVisible).toBeDefined();
             expect(winLossView.$isStackMode).toBeDefined();
             expect(winLossView.$isAgentLevel).toBeDefined();
             expect(winLossView.$reportForm).toBeDefined();
             expect(winLossView.$userSelectedDateRangeOption).toBeDefined();
             expect(winLossView.$multiSelectButton).toBeDefined();
             expect(winLossView.$reportSwitch).toBeDefined();
         });
     });
});