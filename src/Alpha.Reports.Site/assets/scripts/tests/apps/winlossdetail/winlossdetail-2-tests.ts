// split test file because:
// error: Timeout occurred when executing test file
describe("WinLossDetail_", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);

    let winLossDetailView: WinLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderdateRangePickerComponent: jasmine.Spy;

    beforeEach(() => {
        winLossDetailView = new WinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            pagerComponent,
            productMultiSelectionComponent,
            sortComponent,
            reportTooltip,
            monthPickerComponent,
            netTurnoverTooltipComponent);

        cacheElements = spyOn(winLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(winLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        renderdateRangePickerComponent = spyOn(dateRangePickerComponent, "render");
    });

    describe("checkReady", () => {
        it("Historic Report -> Switch to Current Report and Submit", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$reportForm, "trigger");

            // act
            winLossDetailView.onSwitchReport();

            // assert
            expect(winLossDetailView.isSwitchView).toBeTruthy();
            expect(winLossDetailView.isHistoricReport).toBeFalsy();
            expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });

        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            winLossDetailView.cacheElements();

            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
            spyOn(winLossDetailView, "replaceHrefParam");

            // act
            winLossDetailView.onClickIconFilter();

            // assert
            expect(winLossDetailView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossDetailView.$reportForm.hasClass("hide")).toBeFalsy();
            expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalled();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
        });
    });
    describe("onSwitchReport", () => {
        it("Current Report -> Switch to Historic Report and Submit", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$reportForm, "trigger");

            // act
            winLossDetailView.onSwitchReport();

            // assert
            expect(winLossDetailView.isSwitchView).toBeTruthy();
            expect(winLossDetailView.isHistoricReport).toBeTruthy();
            expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
        it("Historic Report -> Switch to Current Report and Submit", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$reportForm, "trigger");

            // act
            winLossDetailView.onSwitchReport();

            // assert
            expect(winLossDetailView.isSwitchView).toBeTruthy();
            expect(winLossDetailView.isHistoricReport).toBeFalsy();
            expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
    });
    describe("setStackMode", () => {
        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView, "replaceHrefParam");
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            winLossDetailView.setStackMode();

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);

            expect(combineConsecutiveCellsSpy.calls.allArgs()[0]).toEqual([
                jasmine.any(Object), 3, "col-turnover-combined"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[1]).toEqual([
                jasmine.any(Object), 3, "col-member-winloss-combined"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[2]).toEqual([
                jasmine.any(Object), 3, "col-agent-winloss-combined altercol"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[3]).toEqual([
                jasmine.any(Object), 3, "col-master-winloss-combined"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[4]).toEqual([
                jasmine.any(Object), 3, "col-super-winloss-combined altercol"]);

            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("onClickIconFilter", () => {
        it("Filter is currently shown -> Hides Filter and Update Href Param to False", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
            winLossDetailView.cacheElements();

            spyOn(winLossDetailView, "hideFilter");
            spyOn(winLossDetailView, "replaceHrefParam");

            // act
            winLossDetailView.onClickIconFilter();

            // assert
            expect(winLossDetailView.hideFilter).toHaveBeenCalled();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
        });
    });
    describe("onSwitchMode", () => {
        it(`1st Switch on Normal Mode -> 
                Blocks/Unblocks UI;
                Adds Class stack-mode to Table Report/Page Title;
                Updates HRef Param;
                Combines Cells;
                Rebinds Table Sorter`, () => {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

                // act
                winLossDetailView.onSwitchMode();

                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
                expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
                expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();

                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);

                expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                    [jasmine.any(Object), 3, "col-turnover-combined"],
                    [jasmine.any(Object), 3, "col-member-winloss-combined"],
                    [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                    [jasmine.any(Object), 3, "col-master-winloss-combined"],
                    [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]]);
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(winLossDetailView.getSortedColumnName());
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
        it(`1st Switch on Stack Mode -> 
                Blocks/Unblocks UI;
                Removes Class stack-mode to Table Report/Page Title;
                Updates HRef Param;
                Splits Cells;
                Rebinds Table Sorter`, () => {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithStackMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                var splitCellSpy = spyOn(alphaReports, "splitCell");

                // act
                winLossDetailView.onSwitchMode();

                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
                expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
                expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeFalsy();

                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", false);

                expect(splitCellSpy.calls.allArgs()).toEqual([
                    ["col-turnover"],
                    ["col-member-winloss", ""],
                    ["col-agent-winloss", " altercol"],
                    ["col-master-winloss", ""],
                    ["col-super-winloss", " altercol"]]);
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(winLossDetailView.getSortedColumnName());
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });

        it(`2nd+ Switch on Normal Mode -> Not Combine Cells or Splits Cells`, () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            winLossDetailView.isFirstSwitchMode = false;
            spyOn(alphaReports, "combineConsecutiveCells");
            spyOn(alphaReports, "splitCell");

            // act
            winLossDetailView.onSwitchMode();

            // assert
            expect(alphaReports.splitCell).not.toHaveBeenCalled();
            expect(alphaReports.combineConsecutiveCells).not.toHaveBeenCalled();
        });
        it(`2nd+ Switch on Stack Mode -> Not Combine Cells or Splits Cells`, () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithStackMode);
            winLossDetailView.cacheElements();
            winLossDetailView.isFirstSwitchMode = false;
            spyOn(alphaReports, "combineConsecutiveCells");
            spyOn(alphaReports, "splitCell");

            // act
            winLossDetailView.onSwitchMode();

            // assert
            expect(alphaReports.splitCell).not.toHaveBeenCalled();
            expect(alphaReports.combineConsecutiveCells).not.toHaveBeenCalled();
        });
    });
    describe("onExportExcel", () => {
        it("Always -> Submits Export Excel Form And Unblocks UI", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$exportExcelForm, "submit");

            // act
            winLossDetailView.onExportExcel();

            // assert
            expect(winLossDetailView.$exportExcelForm.submit).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("afterSort", () => {
        it("Not Have All Data on Client -> Submits Report", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.hasAllDataOnClient = false;
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$reportForm, "trigger");
            spyOn(winLossDetailView, "onSubmitReport");

            // act
            winLossDetailView.afterSort();

            // assert
            expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
    });
});