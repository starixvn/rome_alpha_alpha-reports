describe("WinLossAnalysis_", function () {
    var jQueryFake = $;
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var sortComponent = new SortComponent($, linkBuilderComponent);
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var winLossAnalysisView;
    var cacheDOMElements, registerEvents, blockUI, unblockUI, renderProductMultiSelectionComponent, renderSortComponent, renderLinkBuilderComponent;
    beforeEach(function () {
        winLossAnalysisView = new WinLossAnalysisView(jQueryFake, dateRangePickerComponent, productMultiSelectionComponent, sortComponent, reportTooltip, linkBuilderComponent);
        spyOn(winLossAnalysisView, "loadLegend");
        cacheDOMElements = spyOn(winLossAnalysisView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();
        registerEvents = spyOn(winLossAnalysisView, "registerEvents");
        registerEvents.and.callThrough();
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        renderLinkBuilderComponent = spyOn(linkBuilderComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });
    describe("init", function () {
        it("Document is not ready -> Does not call any Method", function () {
            // act
            winLossAnalysisView.init();
            // assert
            expect(winLossAnalysisView.cacheDOMElements).not.toHaveBeenCalled();
            expect(winLossAnalysisView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
            expect(productMultiSelectionComponent.render).not.toHaveBeenCalled();
            expect(sortComponent.render).not.toHaveBeenCalled();
        });
        it("Document is ready -> Calls Specific Methods", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            // act
            winLossAnalysisView.init();
            // assert
            expect(winLossAnalysisView.cacheDOMElements).toHaveBeenCalled();
            expect(winLossAnalysisView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(productMultiSelectionComponent.render).toHaveBeenCalled();
            expect(sortComponent.render).toHaveBeenCalled();
        });
        it("Current Report -> Renders Date Range Picker", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            // act
            winLossAnalysisView.init();
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("Always -> Render sort component with correct options", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            // act
            winLossAnalysisView.init();
            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(winLossAnalysisView.$tableReport, false, true, jasmine.any(Function), { 0: { sorter: "text" } }));
        });
    });
    describe("checkReady", function () {
        it("Document is not loaded yet (empty) -> Returns False", function () {
            // arrange
            setFixtures("");
            // act
            var actualIsReady = winLossAnalysisView.checkReady();
            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("Document is loaded (ready) -> Returns True", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            // act
            var actualIsReady = winLossAnalysisView.checkReady();
            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("cacheDOMElements", function () {
        it("Always -> All neccessary elements have been cached", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            // act
            winLossAnalysisView.cacheDOMElements();
            // assert
            expect(winLossAnalysisView.$pageTitle).toBeDefined();
            expect(winLossAnalysisView.$tableReport).toBeDefined();
            expect(winLossAnalysisView.$reportForm).toBeDefined();
            expect(winLossAnalysisView.$isFilterVisible).toBeDefined();
            expect(winLossAnalysisView.$isStackMode).toBeDefined();
            expect(winLossAnalysisView.$reportForm).toBeDefined();
            expect(winLossAnalysisView.$userSelectedDateRangeOption).toBeDefined();
            expect(winLossAnalysisView.$filterIcon).toBeDefined();
            expect(winLossAnalysisView.$excelIcon).toBeDefined();
            expect(winLossAnalysisView.$switchModeIcon).toBeDefined();
            expect(winLossAnalysisView.$fromDate).toBeDefined();
            expect(winLossAnalysisView.$toDate).toBeDefined();
            expect(winLossAnalysisView.$sortPropertyName).toBeDefined();
            expect(winLossAnalysisView.$isDescendingSort).toBeDefined();
        });
    });
    describe("setStackMode", function () {
        var combineConsecutiveCellsSpy;
        beforeEach(function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "setCssBySwitchMode");
            spyOn(winLossAnalysisView, "renderAtFirstSwitchMode");
            // act
            winLossAnalysisView.setStackMode();
        });
        it("Always -> Blocks UI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("Navigate From dashboard on mobile->\n            Call setCssBySwitchMode Method,\n            Call renderAtFirstSwitchMode Method", function () {
            // assert
            expect(winLossAnalysisView.setCssBySwitchMode).toHaveBeenCalledWith(false);
            expect(winLossAnalysisView.renderAtFirstSwitchMode).toHaveBeenCalledWith(false);
        });
        it("Always -> Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("onFilterEvent", function () {
        it("Filter is currently hidden -> Call hideFilter Function ", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "hideFilter");
            spyOn(winLossAnalysisView, "showFilter");
            // act
            winLossAnalysisView.onFilterEvent();
            // assert
            expect(winLossAnalysisView.hideFilter).toHaveBeenCalled();
        });
        it("Filter is currently hidden -> Page title is Hidden", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "hideFilter");
            spyOn(winLossAnalysisView, "showFilter");
            // act
            winLossAnalysisView.onFilterEvent();
            // assert
            expect(winLossAnalysisView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
        });
    });
    describe("onExportExcelEvent", function () {
        it("Always -> Submits Export Excel Form And Unblocks UI", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView.$exportExcelForm, "submit");
            // act
            winLossAnalysisView.onExportExcelEvent();
            // assert
            expect(winLossAnalysisView.$exportExcelForm.submit).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("hideFilter", function () {
        it("Always -> Add Hide class and hide filter", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            // act
            winLossAnalysisView.hideFilter();
            // assert
            expect(winLossAnalysisView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(winLossAnalysisView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });
    describe("showFilter", function () {
        it("Always -> Remove Hide class and showfilter", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            // act
            winLossAnalysisView.showFilter();
            // assert
            expect(winLossAnalysisView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossAnalysisView.$reportForm.hasClass("hide")).toBeFalsy();
        });
    });
    describe("getParams", function () {
        var getParamsTest = function (selectedProduct, expectedIsAnyProductSelected) {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return selectedProduct; });
            // act
            var actualParam = winLossAnalysisView.getParams();
            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: expectedIsAnyProductSelected,
                IsStackMode: false,
                IsHistoricReport: false,
                IsFilterVisible: true,
                ReportTypeId: 2
            });
        };
    });
    describe("showValidaProductError", function () {
        it("Always -> Adds class error to multiple select button of dropdown product", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            productMultiSelectionComponent.$excludeProducts = [];
            winLossAnalysisView.init();
            // act
            winLossAnalysisView.showValidaProductError();
            // assert
            expect(winLossAnalysisView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("clearError", function () {
        it("Always -> Removes class error from multiple select button of dropdown product", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.init();
            // act
            winLossAnalysisView.clearError();
            // assert
            expect(winLossAnalysisView.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
    });
    describe("onSubmitReportEvent", function () {
        it("Switch View -> Blocks UI; Empty FromDate/Todate; Sets params to hidden fields", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossAnalysisView.init();
            winLossAnalysisView.isSwitchView = true;
            // act
            winLossAnalysisView.onSubmitReportEvent(null);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(winLossAnalysisView.$isFilterVisible.val()).toBeTruthy();
        });
        it("Switch View on Mobile -> Set Filter Is Visible to false", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossAnalysisView.init();
            winLossAnalysisView.isSwitchView = true;
            // act
            winLossAnalysisView.onSubmitReportEvent(null);
            // assert
            expect(winLossAnalysisView.$isFilterVisible.val()).toEqual("true");
        });
        it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossAnalysisView.init();
            winLossAnalysisView.isSwitchView = false;
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            winLossAnalysisView.onSubmitReportEvent(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("getSortedColumnName", function () {
        it("Normal Mode => Returns Sorted Column name data on table report", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.init();
            // act
            var actualSortedColumnName = winLossAnalysisView.getSortedColumnName();
            // assert
            expect(actualSortedColumnName).toEqual("ReportTypeName");
        });
        var sortReportOnStackModeTest = function (sortedColumnName, expectedSortedCoumnResult) {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithStackMode);
            winLossAnalysisView.init();
            winLossAnalysisView.$tableReport.data("sortedcolumn", sortedColumnName);
            // act
            var actualSortedColumnName = winLossAnalysisView.getSortedColumnName();
            // assert
            expect(actualSortedColumnName).toEqual(expectedSortedCoumnResult);
        };
        it("Stack Mode, Sort on MemberNetWinLoss.Comm => Returns Name MemberNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MemberNetWinLoss.Comm", "MemberNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on MemberNetWinLoss.Total => Returns Name MemberNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MemberNetWinLoss.Total", "MemberNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on AgentNetWinLoss.Comm => Returns Name AgentNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("AgentNetWinLoss.Comm", "AgentNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on AgentNetWinLoss.Total => Returns Name AgentNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on MasterNetWinLoss.Comm => Returns Name MasterNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MasterNetWinLoss.Comm", "MasterNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on MasterNetWinLoss.Total => Returns Name MasterNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on SuperNetWinLoss.Comm => Returns Name SuperNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("SuperNetWinLoss.Comm", "SuperNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on SuperNetWinLoss.Total => Returns Name SuperNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("SuperNetWinLoss.Total", "SuperNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on BetCount => Returns Name BetCount", function () {
            sortReportOnStackModeTest("BetCount", "BetCount");
        });
        it("Stack Mode, Sort on Company => Returns Name Company", function () {
            sortReportOnStackModeTest("Company", "Company");
        });
        it("Stack Mode, Sort on MemberNetWinLoss.WinLoss => Returns Name MemberNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MemberNetWinLoss.WinLoss", "MemberNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on AgentNetWinLoss.WinLoss => Returns Name AgentNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on MasterNetWinLoss.WinLoss => Returns Name MasterNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
        });
        it("Stack Mode, Sort on SuperNetWinLoss.WinLoss => Returns Name SuperNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("SuperNetWinLoss.WinLoss", "SuperNetWinLoss.WinLoss");
        });
    });
    describe("renderAtFirstSwitchMode", function () {
        it("Stack Mode Switch to Normal Mode -> Call Split Columns Method", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "splitColumns");
            // act
            winLossAnalysisView.renderAtFirstSwitchMode(true);
            // assert
            expect(winLossAnalysisView.splitColumns).toHaveBeenCalled();
        });
        it("Normal Mode Switch to Stack Mode -> Call Split combineColumns Method", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "combineColumns");
            // act
            winLossAnalysisView.renderAtFirstSwitchMode(false);
            // assert
            expect(winLossAnalysisView.combineColumns).toHaveBeenCalled();
        });
    });
    describe("setCssBySwitchMode", function () {
        it("Stack Mode Switch to Normal Mode -> Remove stack-mode class", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            // act
            winLossAnalysisView.setCssBySwitchMode(true);
            // assert
            expect(winLossAnalysisView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            expect(winLossAnalysisView.$tableReport.hasClass("stack-mode")).toBeFalsy();
        });
        it("Normal Mode Switch to Stack Mode -> Add stack-mode class", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            // act
            winLossAnalysisView.setCssBySwitchMode(false);
            // assert
            expect(winLossAnalysisView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossAnalysisView.$tableReport.hasClass("stack-mode")).toBeTruthy();
        });
    });
    describe("combineColumns", function () {
        it("Stack Mode -> Call combine column Method", function () {
            // arrange
            var combineConsecutiveCellsSpy;
            combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
            // act
            winLossAnalysisView.combineColumns();
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
                [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                [jasmine.any(Object), 3, "col-master-winloss-combined"],
                [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]
            ]);
        });
    });
    describe("splitColumns ", function () {
        it("Switch Normal Mode to Normal Mode -> Call splitCell Method", function () {
            // arrange
            var splitCellSpy = spyOn(alphaReports, "splitCell");
            // act
            winLossAnalysisView.splitColumns();
            // assert
            expect(splitCellSpy.calls.allArgs()).toEqual([
                ["col-member-winloss", ""],
                ["col-agent-winloss", " altercol"],
                ["col-master-winloss", ""],
                ["col-super-winloss", " altercol"]
            ]);
        });
    });
    describe("onSwitchModeEvent   ", function () {
        it("First Switch Mode->\n            Call setCssBySwitchMode Method,\n            Call renderAtFirstSwitchMode Method", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "setCssBySwitchMode");
            spyOn(sortComponent, "bindTableSorter");
            // act
            winLossAnalysisView.onSwitchModeEvent();
            // assert
            expect(winLossAnalysisView.setCssBySwitchMode).toHaveBeenCalledWith(false);
            expect(sortComponent.bindTableSorter).toHaveBeenCalled();
        });
        it("From Second Switch Mode->\n            Call setCssBySwitchMode Method,\n            Call bindTableSorter Method", function () {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(sortComponent, "bindTableSorter");
            spyOn(winLossAnalysisView, "setCssBySwitchMode");
            // act
            winLossAnalysisView.onSwitchModeEvent();
            // assert
            expect(winLossAnalysisView.setCssBySwitchMode).toHaveBeenCalled();
            expect(sortComponent.bindTableSorter).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=winloss-analysis-tests.js.map