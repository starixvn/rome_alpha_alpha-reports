describe("Consolidation Detail", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const levelTypeComponent = new SingleSelectionDropdownComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const linkBuilderComponent = new LinkBuilderComponent($);

    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);
    let consolidationDetailView: ConsolidationDetailView;

    let renderProductMultiSelectionComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy;

    beforeEach(() => {
        consolidationDetailView = new ConsolidationDetailView(
            jQueryFake,
            productMultiSelectionComponent,
            levelTypeComponent,
            dateRangePickerComponent,
            reportTooltip,
            linkBuilderComponent,
            pagerComponent,
            sortComponent);

        let cacheElements = spyOn(consolidationDetailView, "cacheElements");
        cacheElements.and.callThrough();

        let registerEvents = spyOn(consolidationDetailView, "registerEvents");
        registerEvents.and.callThrough();

        spyOn(consolidationDetailView, "initHelp");
        spyOn(consolidationDetailView, "saveConsolidationParam").and.callThrough();

        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        spyOn(dateRangePickerComponent, "render");
        spyOn(netTurnoverTooltipComponent, "render");
    });
    describe("_init", () => {
        describe("_Document is not ready", () => {
            beforeEach(() => {
                // act
                consolidationDetailView.init();
            });
            it("_Does not call cacheElements", () => {
                // assert
                expect(consolidationDetailView.cacheElements).not.toHaveBeenCalled();
            });
            it("_Does not call registerEvents", () => {
                // assert
                expect(consolidationDetailView.registerEvents).not.toHaveBeenCalled();
            });
            it("_Does not call blockUI", () => {
                // assert
                expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            });
            it("_Does not call unblockUI", () => {
                // assert
                expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
            });
            it("_Does not call productMultiSelectionComponent.render", () => {
                // assert
                expect(productMultiSelectionComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call pagerComponent.render", () => {
                // assert
                expect(pagerComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call sortComponent.render", () => {
                // assert
                expect(sortComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call netTurnoverTooltipComponent.render", () => {
                // assert
                expect(netTurnoverTooltipComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call saveConsolidationParam", () => {
                // assert
                expect(consolidationDetailView.saveConsolidationParam).not.toHaveBeenCalled();
            });
        });

        describe("_Document is ready", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationDetailTemplate.normalModeReport);

                // act
                consolidationDetailView.init();
            });

            it("_Calls cacheElements", () => {
                // assert
                expect(consolidationDetailView.cacheElements).toHaveBeenCalled();
            });
            it("_Calls registerEvents", () => {
                // assert
                expect(consolidationDetailView.registerEvents).toHaveBeenCalled();
            });
            it("_Calls blockUI", () => {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Calls unblockUI", () => {
                // assert
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
            it("_Calls productMultiSelectionComponent.render", () => {
                // assert
                expect(productMultiSelectionComponent.render).toHaveBeenCalled();
            });
            it("_Calls pagerComponent.render", () => {
                // assert
                expect(pagerComponent.render).toHaveBeenCalled();
            });
            it("_Calls sortComponent.render", () => {
                // assert
                expect(sortComponent.render).toHaveBeenCalled();
            });

            it("_Always -> Render sort component with correct options", () => {
                // arrange
                spyOn(pagerComponent, "hasAllDataOnClient").and.callFake(() => true);

                // act
                consolidationDetailView.init();

                // assert
                expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                    consolidationDetailView.$tableReport,
                    false,
                    true,
                    sortComponent.afterSort,
                    { 0: { sorter: "text" } },
                    consolidationDetailView.$reportForm
                ));
            });
            it("_Does call saveConsolidationParam", () => {
                // assert
                expect(consolidationDetailView.saveConsolidationParam).toHaveBeenCalled();
            });
        });
    });
    describe("_checkReady", () => {
        it("_Document is not loaded yet (empty) -> Returns False", () => {
            // arrange
            setFixtures("<body></body>");

            // act
            var actualIsReady = consolidationDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("_Document is loaded (ready) -> Returns True", () => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);

            // act
            var actualIsReady = consolidationDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });

    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);

            // act
            consolidationDetailView.cacheElements();

            // assert
            expect(consolidationDetailView.$tableReport).toBeDefined();
            expect(consolidationDetailView.$reportForm).toBeDefined();
            expect(consolidationDetailView.$exportExcelForm).toBeDefined();
            expect(consolidationDetailView.$pageTitle).toBeDefined();
            expect(consolidationDetailView.$isFilterVisible).toBeDefined();
            expect(consolidationDetailView.$isStackMode).toBeDefined();
            expect(consolidationDetailView.$isDescendingSort).toBeDefined();
            expect(consolidationDetailView.$sortPropertyName).toBeDefined();
            expect(consolidationDetailView.$iconNormalColumn).toBeDefined();
        });
    });
    describe("_onClickIconFilter", () => {
        describe("_Filter is currently shown", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationDetailTemplate.shownFilterReport);
                consolidationDetailView.cacheElements();

                spyOn(consolidationDetailView, "hideFilter");
                spyOn(consolidationDetailView, "replaceHrefParam");

                // act
                consolidationDetailView.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(consolidationDetailView.hideFilter).toHaveBeenCalled();
            });
            it("_Update Href Param to False", () => {
                // assert
                expect(consolidationDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
            });
        });
        describe("_Filter is currently hidden", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationDetailTemplate.hiddenFilterReport);
                consolidationDetailView.cacheElements();

                spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
                spyOn(consolidationDetailView, "replaceHrefParam");

                // act
                consolidationDetailView.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(consolidationDetailView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(consolidationDetailView.$reportForm.hasClass("hide")).toBeFalsy();
            });
            it("_Update Href Param to True", () => {
                // assert
                expect(consolidationDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
            });
        });
    });
    describe("_onSwitchMode", () => {
        describe("_1st Switch on Normal Mode", () => {
            let combineConsecutiveCellsSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(consolidationDetailTemplate.normalModeReport);
                consolidationDetailView.cacheElements();
                spyOn(consolidationDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

                // act
                consolidationDetailView.onSwitchMode();
            });

            it("_Blocks UI", () => {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Unblocks UI", () => {
                // assert
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
            it("_Adds Class stack-mode to Table Report", () => {
                // assert
                expect(consolidationDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Adds Class stack-mode to Page Title", () => {
                // assert
                expect(consolidationDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Updates HRef Param", () => {
                // assert
                expect(consolidationDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
            });
            it("_Combines Cells", () => {
                // assert
                expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                    [jasmine.any(Object), 3, "col-turnover-combined"],
                    [jasmine.any(Object), 3, "col-member-winloss-combined"],
                    [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                    [jasmine.any(Object), 3, "col-master-winloss-combined"],
                    [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]]);
            });
            it("_Rebinds Table Sorter", () => {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(consolidationDetailView.getSortedColumnName());
            });
        });
        describe("_1st Switch on Stack Mode", () => {
            let splitCellSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(consolidationDetailTemplate.stackModeReport);
                consolidationDetailView.cacheElements();
                spyOn(consolidationDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                splitCellSpy = spyOn(alphaReports, "splitCell");

                // act
                consolidationDetailView.onSwitchMode();
            });
            it("_Blocks UI", () => {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Unblocks UI", () => {
                // assert
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
            it("_Removes Class stack-mode to Table Report", () => {
                // assert
                expect(consolidationDetailView.$tableReport.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Removes Class stack-mode to Page Title", () => {
                // assert
                expect(consolidationDetailView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Updates HRef Param", () => {
                // assert
                expect(consolidationDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", false);
            });
            it("_Splits Cells", () => {
                // assert
                expect(splitCellSpy.calls.allArgs()).toEqual([
                    ["col-turnover"],
                    ["col-member-winloss", ""],
                    ["col-agent-winloss", " altercol"],
                    ["col-master-winloss", ""],
                    ["col-super-winloss", " altercol"]]);
            });
            it("_Rebinds Table Sorter", () => {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(consolidationDetailView.getSortedColumnName());
            });
        });
        describe("_2nd+ Switch on Normal Mode", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationDetailTemplate.normalModeReport);
                consolidationDetailView.cacheElements();
                consolidationDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                consolidationDetailView.onSwitchMode();
            });
            it("_Not Combine Cells", () => {
                // assert
                expect(alphaReports.combineConsecutiveCells).not.toHaveBeenCalled();
            });
            it("_Not Splits Cells", () => {
                // assert
                expect(alphaReports.splitCell).not.toHaveBeenCalled();
            });
        });
        describe("_2nd+ Switch on Stack Mode", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationDetailTemplate.normalModeReport);
                consolidationDetailView.cacheElements();
                consolidationDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                consolidationDetailView.onSwitchMode();
            });

            it("Not Combine Cells", () => {
                // assert
                expect(alphaReports.combineConsecutiveCells).not.toHaveBeenCalled();
            });
            it("Not Splits Cells", () => {
                // assert
                expect(alphaReports.splitCell).not.toHaveBeenCalled();
            });
        });
    });
    describe("_onSubmitReport", () => {
        it("_Not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            consolidationDetailView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);
            spyOn(consolidationDetailView, "showValidateProductError");

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            consolidationDetailView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(consolidationDetailView.showValidateProductError).toHaveBeenCalled();
        });
        it("_Select product;Screen smaller than 768 -> Blocks UI; Set Params to Hidden field;", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            consolidationDetailView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.returnValue([1]);
            spyOn(consolidationDetailView, "showValidateProductError");
            spyOn(alphaReports, "screenSmallThan768").and.returnValue(true);

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            consolidationDetailView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(consolidationDetailView.showValidateProductError).not.toHaveBeenCalled();
            expect(consolidationDetailView.$isFilterVisible.val()).toBe("false");
            expect(consolidationDetailView.$isStackMode.val()).toBe("false");
        });
        it("_Select product;Screen greater than 768 -> Blocks UI; Set Params to Hidden field;", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            consolidationDetailView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.returnValue([1]);
            spyOn(consolidationDetailView, "showValidateProductError");
            spyOn(alphaReports, "screenSmallThan768").and.returnValue(false);

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            consolidationDetailView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(consolidationDetailView.showValidateProductError).not.toHaveBeenCalled();
            expect(consolidationDetailView.$isFilterVisible.val()).toBe("true");
            expect(consolidationDetailView.$isStackMode.val()).toBe("false");
        });
    });
    describe("_onExportExcel_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);
            consolidationDetailView.cacheElements();
            spyOn(consolidationDetailView.$exportExcelForm, "submit");

            // act
            consolidationDetailView.onExportExcel();
        });
        it("_Submits Export Excel Form", () => {
            // assert
            expect(consolidationDetailView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    it("_replaceHrefParam_always_Calls linkBuilderComponent update param", () => {
        // arrange
        let paramName = "paramName";
        let paramValue = "paramValue";
        spyOn(linkBuilderComponent, "updateParam");

        // act
        consolidationDetailView.replaceHrefParam(paramName, paramValue);

        // assert
        expect(linkBuilderComponent.updateParam).toHaveBeenCalledWith("paramName", "paramValue");
    });
    describe("_showValidateProductError", () => {
        it("_Always -> Adds class error to multiple select button of dropdown product", () => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);
            consolidationDetailView.init();

            // act
            consolidationDetailView.showValidateProductError();

            // assert
            expect(consolidationDetailView.$productButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("_hideFilter_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);
            consolidationDetailView.cacheElements();
            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");

            // act
            consolidationDetailView.hideFilter();
        });
        it("_Updates Page Title", () => {
            // assert
            expect(consolidationDetailView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
        });
        it("_Updates Filter", () => {
            // assert
            expect(consolidationDetailView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });
    describe("_getSortedColumnName", () => {
        it("_Normal Mode => Returns Sorted Column name data on table report", () => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);
            consolidationDetailView.init();

            // act
            var actualSortedColumnName = consolidationDetailView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual("WinLoss");
        });

        let sortReportOnStackModeTest = (sortedColumnName: string, expectedSortedCoumnResult: string) => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithStackMode);
            consolidationDetailView.init();
            consolidationDetailView.$tableReport.data("sortedcolumn", sortedColumnName);

            // act
            var actualSortedColumnName = consolidationDetailView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual(expectedSortedCoumnResult);
        };

        it("_Stack Mode, Sort on NetTurnover => Returns Name Turnover", () => {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
        });
        it("_Stack Mode, Sort on GrossComm => Returns Name Turnover", () => {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
        });
        it("_Stack Mode, Sort on MemberNetWinLoss.Comm => Returns Name MemberNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MemberNetWinLoss.Comm", "MemberNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MemberNetWinLoss.Total => Returns Name MemberNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MemberNetWinLoss.Total", "MemberNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentNetWinLoss.Comm => Returns Name AgentNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("AgentNetWinLoss.Comm", "AgentNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentNetWinLoss.Total => Returns Name AgentNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MasterNetWinLoss.Comm => Returns Name MasterNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MasterNetWinLoss.Comm", "MasterNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MasterNetWinLoss.Total => Returns Name MasterNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on SuperNetWinLoss.Comm => Returns Name SuperNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("SuperNetWinLoss.Comm", "SuperNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on SuperNetWinLoss.Total => Returns Name SuperNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("SuperNetWinLoss.Total", "SuperNetWinLoss.WinLoss");
        });

        it("_Stack Mode, Sort on Username => Returns Name Username", () => {
            sortReportOnStackModeTest("Username", "Username");
        });
        it("_Stack Mode, Sort on BetCount => Returns Name BetCount", () => {
            sortReportOnStackModeTest("BetCount", "BetCount");
        });
        it("_Stack Mode, Sort on Turnover => Returns Name Turnover", () => {
            sortReportOnStackModeTest("Turnover", "Turnover");
        });
        it("_Stack Mode, Sort on Company => Returns Name Company", () => {
            sortReportOnStackModeTest("Company", "Company");
        });

        it("_Stack Mode, Sort on MemberNetWinLoss.WinLoss => Returns Name MemberNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MemberNetWinLoss.WinLoss", "MemberNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentNetWinLoss.WinLoss => Returns Name AgentNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MasterNetWinLoss.WinLoss => Returns Name MasterNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on SuperNetWinLoss.WinLoss => Returns Name SuperNetWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("SuperNetWinLoss.WinLoss", "SuperNetWinLoss.WinLoss");
        });
    });
    describe("_getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);
            consolidationDetailView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => selectedProduct);
            spyOn(sortComponent, "getSortInfo").and.callFake(() => {
                return {
                    sortPropertyName: "WinLoss",
                    isDescendingSort: true
                };
            });

            // act
            var actualParam = consolidationDetailView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: expectedIsAnyProductSelected,
                IsAnyProductDetailSelected: true,
                IsStackMode: false,
                IsFilterVisible: true,

                SortPropertyName: "WinLoss",
                IsDescendingSort: true,
                FromDate: undefined,
                ToDate: undefined,
                UserSelectedDateRangeOption: undefined,
                UserSelectedProductIds: undefined
            });
        };

        it("_No product selected -> Returns correct params", () => {
            getParamsTest([], false);
        });
        it("_Have product selected -> Returns correct params", () => {
            getParamsTest([1], true);
        });
    });
    describe("_saveConsolidationParam", () => {
        it("_Always_Save Consolidation Param to 1st breadcrumb", () => {
            console.log("123");
            // arrange
            setFixtures(consolidationDetailTemplate.normalModeReport);
            consolidationDetailView.cacheElements();

            // act
            var actualParam = consolidationDetailView.saveConsolidationParam();

            // assert
            expect($(".breadcrumb-link").attr("href")).toEqual("Consolidation/ConsolidationByUsername?CustId=123&ConsolidationFromDate=6/1/2017&ConsolidationToDate=9/1/2017&ConsolidationUserSelectedDateRangeOption=undefined&ConsolidationProductIds=1,2");
        });
    });
});