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
        renderSortComponent: jasmine.Spy;

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
        spyOn(dateRangePickerComponent, "render");
    });

    describe("init", () => {
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

        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);

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

        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });

        it("Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(monthPickerComponent.render).toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
        });

        // historic report: we must show filter before render month picker
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

        // historic report: we must show filter before render month picker
        // therefore, after init we also hide it if before that it is hidden
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

        it("Report Of Top Level With no-Submit Param on Mobile -> Set Stack Mode as Default", () => {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            let setStackMode = spyOn(winLossDetailView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            // act
            winLossDetailView.init();

            // assert
            expect(setStackMode).toHaveBeenCalled();
        });

        it("Report Of Top Level With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", () => {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);
            spyOn(reportTooltip, "show");

            // act
            winLossDetailView.init();

            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });

        it("Always -> Render sort component with correct options", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            spyOn(pagerComponent, "hasAllDataOnClient").and.callFake(() => true);

            // act
            winLossDetailView.init();

            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                winLossDetailView.$tableReport,
                false,
                true,
                winLossDetailView.afterSort,
                { 0: { sorter: "text" } }
            ));
        });
    });
    describe("checkReady", () => {
        it("Document is not loaded yet (empty) -> Returns False", () => {
            // arrange
            setFixtures("");

            // act
            var actualIsReady = winLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("Document is loaded (ready) -> Returns True", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);

            // act
            var actualIsReady = winLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("cacheElements", () => {
        it("Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);

            // act
            winLossDetailView.cacheElements();

            // assert
            expect(winLossDetailView.$pageTitle).toBeDefined();
            expect(winLossDetailView.$tableReport).toBeDefined();
            expect(winLossDetailView.$drilldownLinks).toBeDefined();
            expect(winLossDetailView.$isFilterVisible).toBeDefined();
            expect(winLossDetailView.$isStackMode).toBeDefined();
            expect(winLossDetailView.$isDescendingSort).toBeDefined();
            expect(winLossDetailView.$sortPropertyName).toBeDefined();
            expect(winLossDetailView.$reportForm).toBeDefined();
            expect(winLossDetailView.$exportExcelForm).toBeDefined();
            expect(winLossDetailView.$userSelectedDateRangeOption).toBeDefined();
            expect(winLossDetailView.$multiSelectButton).toBeDefined();
            expect(winLossDetailView.$reportSwitch).toBeDefined();
            expect(winLossDetailView.$fromDate).toBeDefined();
            expect(winLossDetailView.$toDate).toBeDefined();
            expect(winLossDetailView.$iconNormalColumn).toBeDefined();
            expect(winLossDetailView.$hasSubmittedParams).toBeDefined();
            expect(winLossDetailView.$breadcrumbLinks).toBeDefined();
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
        it("Always -> Blocks UI; Set stack-mode to table reports; Combines Columns", () => {
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

            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "col-turnover-combined"],
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
                [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                [jasmine.any(Object), 3, "col-master-winloss-combined"],
                [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]]);

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

        let haveAllDataOnClientTests = (currentHtml: string, expectedIsDescendingSort: boolean, expectedTableBody: string) => {
            // arrange
            setFixtures(currentHtml);
            winLossDetailView.hasAllDataOnClient = true;
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$reportForm, "trigger");
            spyOn(winLossDetailView, "onSubmitReport");
            let replaceHrefParamSpy = spyOn(winLossDetailView, "replaceHrefParam");

            // act
            winLossDetailView.afterSort();
            let actulaTableBody = $.trim(winLossDetailView.$tableReport.find("tbody").html());

            // assert
            expect(winLossDetailView.$reportForm.trigger).not.toHaveBeenCalledWith("submit");
            expect(replaceHrefParamSpy.calls.allArgs()).toEqual([
                ["SortPropertyName", "WinLoss"],
                ["IsDescendingSort", expectedIsDescendingSort]]);

            expect(actulaTableBody).toEqual(expectedTableBody);
        };

        it(`Have All Data on Client and Currently Sorts ASC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(winLossDetailTemplate.sortASCOnNormalMode, false, `<tr class=""><td>Row 1</td></tr>
                                    <tr class="even-row"><td>Row 2</td></tr>
                                    <tr><td>Row 3</td></tr>
                                    <tr class="even-row"><td>Row 4</td></tr>`);
            });

        it(`Have All Data on Client and Currently Sorts DESC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(winLossDetailTemplate.sortDESCOnNormalMode, true, "");
            });
        it(`Have All Data on Client and Currently Sorts DESC Combined Column on Stack Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(winLossDetailTemplate.sortDESCCombinedColumnOnStackMode, true, "");
            });
    });
    describe("hideFilter", () => {
        it("Always -> Updates Page Title/Filter/Net Turnover Tooltip", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");

            // act
            winLossDetailView.hideFilter();

            // assert
            expect(winLossDetailView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(winLossDetailView.$reportForm.hasClass("hide")).toBeTruthy();
            expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalledWith(winLossDetailView.$netTurnoverTooltip);
        });
    });
    describe("getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => selectedProduct);
            spyOn(sortComponent, "getSortInfo").and.callFake(() => {
                return {
                    sortPropertyName: "WinLoss",
                    isDescendingSort: true
                };
            });

            // act
            var actualParam = winLossDetailView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: expectedIsAnyProductSelected,
                SortPropertyName: "WinLoss",
                IsDescendingSort: true,
                IsStackMode: false,
                IsHistoricReport: false,
                IsFilterVisible: true
            });
        };

        it("No product selected -> Updates Page Title/Filter/Net Turnover Tooltip", () => {
            getParamsTest([], false);
        });
        it("Have product selected -> Updates Page Title/Filter/Net Turnover Tooltip", () => {
            getParamsTest([1], true);
        });
    });
    describe("showValidateProductError", () => {
        it("Always -> Adds class error to multiple select button of dropdown product", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();

            // act
            winLossDetailView.showValidateProductError();

            // assert
            expect(winLossDetailView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("clearError", () => {
        it("Always -> Removes class error from multiple select button of dropdown product", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();

            // act
            winLossDetailView.clearError();

            // assert
            expect(winLossDetailView.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
    });
    describe("onSubmitReport", () => {
        it("Switch View -> Blocks UI; Empty FromDate/Todate; Sets params to hidden fields", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossDetailView.init();
            winLossDetailView.isSwitchView = true;

            // act
            winLossDetailView.onSubmitReport(null);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();

            expect(winLossDetailView.$fromDate.val()).toEqual("");
            expect(winLossDetailView.$toDate.val()).toEqual("");
            expect(winLossDetailView.$isFilterVisible.val()).toBeTruthy();
            expect(winLossDetailView.$isDescendingSort.val()).toBeTruthy();
            expect(winLossDetailView.$isHistoricReport.val()).toEqual("false");
            expect(winLossDetailView.$isStackMode.val()).toEqual("false");
            expect(winLossDetailView.$sortPropertyName.val()).toEqual("WinLoss");
        });
        it("Switch View on Mobile -> Set Filter Is Visible to false", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossDetailView.init();
            winLossDetailView.isSwitchView = true;

            // act
            winLossDetailView.onSubmitReport(null);

            // assert
            expect(winLossDetailView.$isFilterVisible.val()).toEqual("true");
        });
        it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossDetailView.init();
            winLossDetailView.isSwitchView = false;
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossDetailView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("getSortedColumnName", () => {
        it("Normal Mode => Returns Sorted Column name data on table report", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.init();

            // act
            var actualSortedColumnName = winLossDetailView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual("WinLoss");
        });

        let sortReportOnStackModeTest = (sortedColumnName: string, expectedSortedCoumnResult: string) => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithStackMode);
            winLossDetailView.init();
            winLossDetailView.$tableReport.data("sortedcolumn", sortedColumnName);

            // act
            var actualSortedColumnName = winLossDetailView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual(expectedSortedCoumnResult);
        };

        it("Stack Mode, Sort on NetTurnover => Returns Name Turnover", () => {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
        });
        it("Stack Mode, Sort on GrossComm => Returns Name Turnover", () => {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
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
        it("Stack Mode, Sort on NoOfTickets => Returns Name NoOfTickets", () => {
            sortReportOnStackModeTest("NoOfTickets", "NoOfTickets");
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
});