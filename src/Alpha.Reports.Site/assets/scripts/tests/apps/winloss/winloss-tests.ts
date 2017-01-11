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
             expect(winLossView.$historicReport).toBeDefined();
             expect(winLossView.$filterIcon).toBeDefined();
             expect(winLossView.$excelIcon).toBeDefined();
             expect(winLossView.$switchModeIcon).toBeDefined();
             expect(winLossView.$fromDate).toBeDefined();
             expect(winLossView.$toDate).toBeDefined();
             expect(winLossView.$sortPropertyName).toBeDefined();
             expect(winLossView.$isDescendingSort).toBeDefined();
         });
     });

     describe("onSubmitHistoricReportEvent", () => {
         it("Current Report -> Switch to Historic Report and Submit", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             spyOn(winLossView.$reportForm, "trigger");

             // act
             winLossView.onSubmitHistoricReportEvent();

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
             winLossView.onSubmitHistoricReportEvent();

             // assert
             expect(winLossView.isSwitchView).toBeTruthy();
             expect(winLossView.isHistoricReport).toBeTruthy();
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
     });

     describe("getSortedColumnName", () => {
         it("Normal Mode => Returns Sorted Column name data on table report", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.init();

             // act
             var actualSortedColumnName = winLossView.getSortedColumnName();

             // assert
             expect(actualSortedColumnName).toEqual("WinLoss");
         });

         let sortReportOnStackModeTest = (sortedColumnName: string, expectedSortedCoumnResult: string) => {
             // arrange
             setFixtures(winLossDetailTemplate.currentReportWithStackMode);
             winLossView.init();
             winLossView.$tableReport.data("sortedcolumn", sortedColumnName);

             // act
             var actualSortedColumnName = winLossView.getSortedColumnName();

             // assert
             expect(actualSortedColumnName).toEqual(expectedSortedCoumnResult);
         };

         it("Stack Mode, Sort on GrossComm => Returns Name Turnover", () => {
             sortReportOnStackModeTest("GrossComm", "Turnover");
         });

         it("Stack Mode, Sort on MemberNetWinLoss.Comm => Returns Name MemberNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("MemberNetWinLoss.Comm", "MemberNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on MemberNetWinLoss.Total => Returns Name MemberNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("MemberNetWinLoss.Total", "MemberNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on AgentNetWinLoss.Comm => Returns Name AgentNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("AgentNetWinLoss.Comm", "AgentNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on AgentNetWinLoss.Total => Returns Name AgentNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on MasterNetWinLoss.Comm => Returns Name MasterNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("MasterNetWinLoss.Comm", "MasterNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on MasterNetWinLoss.Total => Returns Name MasterNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on SuperNetWinLoss.Comm => Returns Name SuperNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("SuperNetWinLoss.Comm", "SuperNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on SuperNetWinLoss.Total => Returns Name SuperNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("SuperNetWinLoss.Total", "SuperNetWinLoss.WinLoss");
         });

         it("Stack Mode, Sort on Username => Returns Name Username", () => {
             sortReportOnStackModeTest("Username", "Username");
         });
         it("Stack Mode, Sort on Turnover => Returns Name Turnover", () => {
             sortReportOnStackModeTest("Turnover", "Turnover");
         });
         it("Stack Mode, Sort on Company => Returns Name Company", () => {
             sortReportOnStackModeTest("Company", "Company");
         });

         it("Stack Mode, Sort on MemberNetWinLoss.WinLoss => Returns Name MemberNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("MemberNetWinLoss.WinLoss", "MemberNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on AgentNetWinLoss.WinLoss => Returns Name AgentNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on MasterNetWinLoss.WinLoss => Returns Name MasterNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
         });
         it("Stack Mode, Sort on SuperNetWinLoss.WinLoss => Returns Name SuperNetWinLoss.WinLoss", () => {
             sortReportOnStackModeTest("SuperNetWinLoss.WinLoss", "SuperNetWinLoss.WinLoss");
         });
     });

     describe("renderAtFirstSwitchMode", () => {
         it("Stack Mode Switch to Normal Mode -> Call Split Columns Method", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             spyOn(winLossView, "splitColumns");

             // act
             winLossView.renderAtFirstSwitchMode(true);

             // assert
             expect(winLossView.splitColumns).toHaveBeenCalled();
         });

         it("Normal Mode Switch to Stack Mode -> Call Split combineColumns Method", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             spyOn(winLossView, "combineColumns");

             // act
             winLossView.renderAtFirstSwitchMode(false);

             // assert
             expect(winLossView.combineColumns).toHaveBeenCalled();
         });
     });

     describe("setCssBySwitchMode", () => {
         it("Stack Mode Switch to Normal Mode -> Remove stack-mode class", () => {          
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();

             // act
             winLossView.setCssBySwitchMode(true);

             // assert
             expect(winLossView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
             expect(winLossView.$tableReport.hasClass("stack-mode")).toBeFalsy();
         });

         it("Normal Mode Switch to Stack Mode -> Add stack-mode class", () => {  
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
            
             // act
             winLossView.setCssBySwitchMode(false);

             // assert
             expect(winLossView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
             expect(winLossView.$tableReport.hasClass("stack-mode")).toBeTruthy();
         });
     });

     describe("combineColumns", () => {
         it("Stack Mode At Agent Level -> Call combineToTurnOverColumnAgentLevel Method", () => {
             // arrange
             spyOn(winLossView, "combineToTurnOverColumnAgentLevel");
             spyOn(winLossView, "combineToWinLossColumn");

             // act
             winLossView.combineColumns(true);

             // assert
             expect(winLossView.combineToTurnOverColumnAgentLevel).toHaveBeenCalled();
             expect(winLossView.combineToWinLossColumn).toHaveBeenCalled();
         });

         it("Stack Mode At Super Master Level -> Call combineToTurnOverColumnSuperMasterLevel AndcombineToWinLossColumn Method", () => {
             // arrange
             spyOn(winLossView, "combineToTurnOverColumnSuperMasterLevel");
             spyOn(winLossView, "combineToWinLossColumn");

             // act
             winLossView.combineColumns(false);

             // assert
             expect(winLossView.combineToTurnOverColumnSuperMasterLevel).toHaveBeenCalled();
             expect(winLossView.combineToWinLossColumn).toHaveBeenCalled();
         });
     });

     describe("combineToWinLossColumn", () => {
         it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", () => {
             // arrange
             var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

             // act
             winLossView.combineToWinLossColumn('agent', 1);

             // assert
             expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                 [jasmine.any(Object), 3, "text-right col-agent-winloss-combined altercol"]]);
         });
     });

     describe("combineToTurnOverColumnAgentLevel", () => {
         it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", () => {
             // arrange
             var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

             // act
             winLossView.combineToTurnOverColumnAgentLevel();

             // assert
             expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                 [jasmine.any(Object), 2, "text-right col-turnover-combined"]]);
         });
     });

     describe("combineToTurnOverColumnSuperMasterLevel", () => {
         it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", () => {
             // arrange
             var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

             // act
             winLossView.combineToTurnOverColumnSuperMasterLevel();

             // assert
             expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                 [jasmine.any(Object), 3, "text-right col-turnover-combined"]]);
         });
     });

     describe("splitColumns ", () => {
         it("Switch Normal Mode to Normal Mode -> Call splitCell Method", () => {
             // arrange
             var splitCellSpy = spyOn(alphaReports, "splitCell");

             // act
             winLossView.splitColumns();

             // assert
             expect(splitCellSpy.calls.allArgs()).toEqual([
                 ["col-turnover"],
                 ["col-member-winloss", ""],
                 ["col-agent-winloss", " altercol"],
                 ["col-master-winloss", ""],
                 ["col-super-winloss", " altercol"]]);
         });
     });

     describe("replaceDrilldownParam  ", () => {
         it("Drill Down-> Call replaceQueryStringValue Method", () => {
             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             spyOn(alphaReports,"replaceQueryStringValue");

             // act
             winLossView.replaceDrilldownParam("IsFilterVisible", true);

             // assert
             expect(alphaReports.replaceQueryStringValue).toHaveBeenCalled();
         });
     });

     describe("onSwitchModeEvent   ", () => {
         it(`First Switch Mode-> 
            Call setCssBySwitchMode Method,
            Call bindTableSorter Method,
            Call renderAtFirstSwitchMode Method,
            Set isFirstSwitchMode is False,
            Call replaceDrilldownParam Method`, () => {

             // arrange
             setFixtures(winLossTemplate.currentReportWithNormalMode);
             winLossView.cacheDOMElements();
             
             spyOn(winLossView, "renderAtFirstSwitchMode");
             spyOn(winLossView, "setCssBySwitchMode");
             spyOn(winLossView, "replaceDrilldownParam");
             spyOn(sortComponent, "bindTableSorter");

             // act
             winLossView.onSwitchModeEvent();

             // assert
             expect(winLossView.setCssBySwitchMode).toHaveBeenCalled();
             expect(winLossView.replaceDrilldownParam).toHaveBeenCalledWith("IsStackMode", true);
             expect(winLossView.renderAtFirstSwitchMode).toHaveBeenCalledWith(false);
             expect(sortComponent.bindTableSorter).toHaveBeenCalled();
             expect(winLossView.isFirstSwitchMode).toBeFalsy();
             });

         it(`From Second Switch Mode-> 
            Call setCssBySwitchMode Method,
            Call bindTableSorter Method,
            Call replaceDrilldownParam Method`, () => {

                 // arrange
                 setFixtures(winLossTemplate.currentReportWithNormalMode);
                 winLossView.cacheDOMElements();
                 spyOn(sortComponent, "bindTableSorter");
                 spyOn(winLossView, "setCssBySwitchMode");
                 spyOn(winLossView, "replaceDrilldownParam");

                 // act
                 winLossView.onSwitchModeEvent();

                 // assert
                 expect(winLossView.setCssBySwitchMode).toHaveBeenCalled();
                 expect(winLossView.replaceDrilldownParam).toHaveBeenCalledWith("IsStackMode", true);
                 expect(sortComponent.bindTableSorter).toHaveBeenCalled();
             });
     });
});