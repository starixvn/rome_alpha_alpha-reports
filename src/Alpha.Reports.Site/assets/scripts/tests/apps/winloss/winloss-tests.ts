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
                 jasmine.any(Function),
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
             expect(winLossView.$reportForm).toBeDefined();
             expect(winLossView.$isFilterVisible).toBeDefined();
             expect(winLossView.$isStackMode).toBeDefined();
             expect(winLossView.$isAgentLevel).toBeDefined();
             expect(winLossView.$reportForm).toBeDefined();
             expect(winLossView.$userSelectedDateRangeOption).toBeDefined();
             expect(winLossView.$multiSelectButton).toBeDefined();
             expect(winLossView.$reportSwitch).toBeDefined();
         });
     });

     describe("onReportSwitchEvent", () => {
         it("Current Report -> Switch to Historic Report and Submit", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             spyOn(winLossView.$reportForm, "trigger");

             // act
             winLossView.onReportSwitchEvent();

             // assert
             expect(winLossView.isSwitchView).toBeTruthy();
             expect(winLossView.isHistoricReport).toBeTruthy();
             expect(winLossView.$reportForm.trigger).toHaveBeenCalledWith("submit");
         });
         it("Historic Report -> Switch to Current Report and Submit", () => {
             // arrange
             setFixtures(winLossTemplate.historicReportWithShownFilter);
             winLossView.cacheDOMElements();
             spyOn(winLossView.$reportForm, "trigger");

             // act
             winLossView.onReportSwitchEvent();

             // assert
             expect(winLossView.isSwitchView).toBeTruthy();
             expect(winLossView.isHistoricReport).toBeFalsy();
             expect(winLossView.$reportForm.trigger).toHaveBeenCalledWith("submit");
         });
     });

     describe("onFilterEvent", () => {
         it("Filter is currently shown -> Hides Filter and Update Href Param to False", () => {
             // arrange
             setFixtures(winLossTemplate.historicReportWithShownFilter);
             winLossView.cacheDOMElements();

             spyOn(winLossView, "hideFilter");
             spyOn(winLossView, "showFilter");
             spyOn(winLossView, "replaceDrilldownParam");

             // act
             winLossView.onFilterEvent();

             // assert
             expect(winLossView.hideFilter).toHaveBeenCalled();
             expect(winLossView.replaceDrilldownParam).toHaveBeenCalledWith("IsFilterVisible", false);
         });

         it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
             // arrange
             setFixtures(winLossTemplate.historicReportWithHiddenFilter);
             winLossView.cacheDOMElements();

             spyOn(winLossView, "hideFilter");
             spyOn(winLossView, "showFilter");
             spyOn(winLossView, "replaceDrilldownParam");

             // act
             winLossView.onFilterEvent();

             // assert
             expect(winLossView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
             expect(winLossView.replaceDrilldownParam).toHaveBeenCalledWith("IsFilterVisible", true);
         });
     });

     describe("onExportExcelEvent", () => {
         it("Always -> Submits Export Excel Form And Unblocks UI", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             spyOn(winLossView.$exportExcelForm, "submit");

             // act
             winLossView.onExportExcelEvent();

             // assert
             expect(winLossView.$exportExcelForm.submit).toHaveBeenCalled();
             expect(jQueryFake.unblockUI).toHaveBeenCalled();
         });
     });

     describe("hideFilter", () => {
         it("Always -> Add Hide class and hide filter", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();

             // act
             winLossView.hideFilter();

             // assert
             expect(winLossView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
             expect(winLossView.$reportForm.hasClass("hide")).toBeTruthy();
         });
     });

     describe("showFilter", () => {
         it("Always -> Remove Hide class and showfilter", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();

             // act
             winLossView.showFilter();

             // assert
             expect(winLossView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
             expect(winLossView.$reportForm.hasClass("hide")).toBeFalsy();
         });
     });

     describe("getParams", () => {

         let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => selectedProduct);
            
             // act
             var actualParam = winLossView.getParams();

             // assert
             expect(actualParam).toEqual({
                 IsAnyProductSelected: expectedIsAnyProductSelected,
                 IsStackMode: false,
                 IsHistoricReport: false,
                 IsFilterVisible: true
             });
         };
     });

     describe("showValidaProductError", () => {
         it("Always -> Adds class error to multiple select button of dropdown product", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();

             // act
             winLossView.showValidaProductError();

             // assert
             expect(winLossView.$multiSelectButton.hasClass("error")).toBeTruthy();
         });
     });

     describe("clearError", () => {
         it("Always -> Removes class error from multiple select button of dropdown product", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();

             // act
             winLossView.clearError();

             // assert
             expect(winLossView.$multiSelectButton.hasClass("error")).toBeFalsy();
         });
     });

     describe("onSubmitReportEvent", () => {
         it("Switch View -> Blocks UI; Empty FromDate/Todate; Sets params to hidden fields", () => {
             // arrange
             setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
             renderProductMultiSelectionComponent.and.callThrough();
             renderSortComponent.and.callThrough();
             winLossView.init();
             winLossView.isSwitchView = true;

             // act
             winLossView.onSubmitReportEvent(null);

             // assert
             expect(jQueryFake.blockUI).toHaveBeenCalled();

             expect(winLossView.$isFilterVisible.val()).toBeTruthy();
             expect(winLossView.$isHistoricReport.val()).toEqual("false");
         });

         it("Switch View on Mobile -> Set Filter Is Visible to false", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             renderProductMultiSelectionComponent.and.callThrough();
             renderSortComponent.and.callThrough();
             winLossView.init();
             winLossView.isSwitchView = true;

             // act
             winLossView.onSubmitReportEvent(null);

             // assert
             expect(winLossView.$isFilterVisible.val()).toEqual("true");
         });

         it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             renderProductMultiSelectionComponent.and.callThrough();
             renderSortComponent.and.callThrough();
             winLossView.init();
             winLossView.isSwitchView = false;
             spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

             var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

             // act
             winLossView.onSubmitReportEvent(event);

             // assert
             expect(jQueryFake.blockUI).toHaveBeenCalled();
             expect(jQueryFake.unblockUI).toHaveBeenCalled();
         });
     });

});