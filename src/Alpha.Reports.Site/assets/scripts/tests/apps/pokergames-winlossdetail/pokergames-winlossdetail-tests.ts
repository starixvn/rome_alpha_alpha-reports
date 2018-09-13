describe("WinLossDetail", () => {
    const jQueryFake = $;
    const pagerComponent = new PagerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);

    const sortComponent = new SortComponent($, linkBuilderComponent);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    let winLossDetailView: PokerGamesWinLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        winLossDetailView = new PokerGamesWinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            pagerComponent,
            sortComponent,
            reportTooltip,
            monthPickerComponent,
            linkBuilderComponent);

        spyOn(winLossDetailView, "loadLegend");
        cacheElements = spyOn(winLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(winLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        renderLinkBuilderComponent = spyOn(linkBuilderComponent, "render");
        renderLinkBuilderComponent.and.callThrough();

        spyOn(dateRangePickerComponent, "render");
    });

    it("Inherit BaseReport_Always_WinLossDetailView is BaseReport", () => {
        // assert
        expect(winLossDetailView instanceof BaseReport).toBeTruthy();
    });

    describe("_init", () => {
        describe("_Document is not ready", () => {
            beforeEach(() => {
                // act
                winLossDetailView.init();
            });
            it("_Does not call cacheElements", () => {
                // assert
                expect(winLossDetailView.cacheElements).not.toHaveBeenCalled();
            });
            it("_Does not call registerEvents", () => {
                // assert
                expect(winLossDetailView.registerEvents).not.toHaveBeenCalled();
            });
            it("_Does not call blockUI", () => {
                // assert
                expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            });
            it("_Does not call unblockUI", () => {
                // assert
                expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
            });
            it("_Does not call pagerComponent.render", () => {
                // assert
                expect(pagerComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call sortComponent.render", () => {
                // assert
                expect(sortComponent.render).not.toHaveBeenCalled();
            });
        });
        describe("_Document is ready", () => {
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
                spyOn(winLossDetailView, "initHelp");

                // act
                winLossDetailView.init();
            });

            it("_Calls cacheElements", () => {
                // assert
                expect(winLossDetailView.cacheElements).toHaveBeenCalled();
            });
            it("_Calls registerEvents", () => {
                // assert
                expect(winLossDetailView.registerEvents).toHaveBeenCalled();
            });
            it("_Calls blockUI", () => {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Calls unblockUI", () => {
                // assert
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
            it("_Calls pagerComponent.render", () => {
                // assert
                expect(pagerComponent.render).toHaveBeenCalled();
            });
            it("_Calls sortComponent.render", () => {
                // assert
                expect(sortComponent.render).toHaveBeenCalled();
            });
            it("_Init Help", () => {
                // assert
                expect(winLossDetailView.initHelp).toHaveBeenCalled();
            });
        });

        it("_Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });

        it("_Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(monthPickerComponent.render).toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
        });

        // historic report: we must show filter before render month picker
        it("_Historic Report With Hidden Filter -> Shows Filter Before Renders Month Picker", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.historicReportWithHiddenFilter);

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
        it("_Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render").and.callThrough();

            // act
            winLossDetailView.init();
            var actualIsHidden = $("#report-form").hasClass("hide");

            // assert
            expect(actualIsHidden).toBeTruthy();
        });

        it("_Report Of Top Level With no-Submit Param on Mobile -> Set Stack Mode as Default", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.reportOfTopLevel);
            let setStackMode = spyOn(winLossDetailView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            // act
            winLossDetailView.init();

            // assert
            expect(setStackMode).toHaveBeenCalled();
        });

        it("_Report Of Top Level With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.reportOfTopLevel);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);
            spyOn(reportTooltip, "show");

            // act
            winLossDetailView.init();

            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });

        it("_Always -> Render sort component with correct options", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            spyOn(pagerComponent, "hasAllDataOnClient").and.callFake(() => true);

            // act
            winLossDetailView.init();

            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                winLossDetailView.$tableReport,
                false,
                true,
                sortComponent.afterSort,
                { 0: { sorter: "text" } },
                winLossDetailView.$reportForm
            ));
        });
    });
    describe("_checkReady", () => {
        it("_Document is not loaded yet (empty) -> Returns False", () => {
            // arrange
            setFixtures("");

            // act
            var actualIsReady = winLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("_Document is loaded (ready) -> Returns True", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);

            // act
            var actualIsReady = winLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);

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
            expect(winLossDetailView.$reportSwitch).toBeDefined();
            expect(winLossDetailView.$fromDate).toBeDefined();
            expect(winLossDetailView.$toDate).toBeDefined();
            expect(winLossDetailView.$iconNormalColumn).toBeDefined();
            expect(winLossDetailView.$isSetDefaultStackMode).toBeDefined();
            expect(winLossDetailView.$breadcrumbLinks).toBeDefined();
        });
    });
    describe("_onSwitchReport", () => {
        describe("_Current Report", () => {
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView.$reportForm, "trigger");

                // act
                winLossDetailView.onSwitchReport();
            });

            it("_Switch to Historic Report", () => {
                // assert
                expect(winLossDetailView.isSwitchView).toBeTruthy();
                expect(winLossDetailView.isHistoricReport).toBeTruthy();
            });
            it("_Submit", () => {
                // assert
                expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
            });
        });
        describe("_Historic Report", () => {
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.historicReportWithShownFilter);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView.$reportForm, "trigger");

                // act
                winLossDetailView.onSwitchReport();
            });
            it("_Switch to Current Report", () => {
                // assert
                expect(winLossDetailView.isSwitchView).toBeTruthy();
                expect(winLossDetailView.isHistoricReport).toBeFalsy();
            });
            it("_Submit", () => {
                // assert
                expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
            });
        });
    });
    describe("_setStackMode_Always", () => {
        let combineConsecutiveCellsSpy: jasmine.Spy;
        beforeEach(() => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView, "replaceHrefParam");
            combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            winLossDetailView.setStackMode();
        });

        it("_Blocks UI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Set stack-mode to table reports", () => {
            // assert
            expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
        });
        it("_Combines Columns", () => {
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "col-turnover-combined"],
                [jasmine.any(Object), 4, "col-member-winloss-combined"],
                [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                [jasmine.any(Object), 3, "col-master-winloss-combined"],
                [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]]);
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onClickIconFilter", () => {
        describe("_Filter is currently shown", () => {
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.historicReportWithShownFilter);
                winLossDetailView.cacheElements();

                spyOn(winLossDetailView, "hideFilter");
                spyOn(winLossDetailView, "replaceHrefParam");

                // act
                winLossDetailView.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(winLossDetailView.hideFilter).toHaveBeenCalled();
            });
            it("_Update Href Param to False", () => {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
            });
        });
        describe("_Filter is currently hidden", () => {
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.historicReportWithHiddenFilter);
                winLossDetailView.cacheElements();

                spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
                spyOn(winLossDetailView, "replaceHrefParam");

                // act
                winLossDetailView.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(winLossDetailView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(winLossDetailView.$reportForm.hasClass("hide")).toBeFalsy();
            });
            it("_Update Href Param to True", () => {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
            });
        });
    });
    describe("_onSwitchMode", () => {
        describe("_1st Switch on Normal Mode", () => {
            let combineConsecutiveCellsSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

                // act
                winLossDetailView.onSwitchMode();
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
                expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Adds Class stack-mode to Page Title", () => {
                // assert
                expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Updates HRef Param", () => {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
            });
            it("_Combines Cells", () => {
                // assert
                expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                    [jasmine.any(Object), 3, "col-turnover-combined"],
                    [jasmine.any(Object), 4, "col-member-winloss-combined"],
                    [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                    [jasmine.any(Object), 3, "col-master-winloss-combined"],
                    [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]]);
            });
            it("_Rebinds Table Sorter", () => {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(winLossDetailView.getSortedColumnName());
            });
        });
        describe("_1st Switch on Stack Mode", () => {
            let splitCellSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithStackMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                splitCellSpy = spyOn(alphaReports, "splitCell");

                // act
                winLossDetailView.onSwitchMode();
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
                expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Removes Class stack-mode to Page Title", () => {
                // assert
                expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Updates HRef Param", () => {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", false);
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
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(winLossDetailView.getSortedColumnName());
            });
        });
        describe("_2nd+ Switch on Normal Mode", () => {
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
                winLossDetailView.cacheElements();
                winLossDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                winLossDetailView.onSwitchMode();
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
                setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithStackMode);
                winLossDetailView.cacheElements();
                winLossDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                winLossDetailView.onSwitchMode();
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
    describe("_onExportExcel_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$exportExcelForm, "submit");

            // act
            winLossDetailView.onExportExcel();
        });
        it("_Submits Export Excel Form", () => {
            // assert
            expect(winLossDetailView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_hideFilter_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");

            // act
            winLossDetailView.hideFilter();
        });
        it("_Updates Page Title", () => {
            // assert
            expect(winLossDetailView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
        });
        it("_Updates Filter", () => {
            // assert
            expect(winLossDetailView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });
    describe("_getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
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
                SortPropertyName: "WinLoss",
                IsDescendingSort: true,
                IsStackMode: false,
                IsHistoricReport: false,
                IsFilterVisible: true
            });
        };

        it("_No product selected -> Returns correct params", () => {
            getParamsTest([], false);
        });
        it("_Have product selected -> Returns correct params", () => {
            getParamsTest([1], true);
        });
    });
    describe("_onSubmitReport", () => {
        describe("_Switch View", () => {
            beforeEach(() => {
                // arrange
                setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
                renderSortComponent.and.callThrough();
                winLossDetailView.init();
                winLossDetailView.isSwitchView = true;

                // act
                winLossDetailView.onSubmitReport(null);
            });
            it("_Blocks UI", () => {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Empty FromDate", () => {
                // assert
                expect(winLossDetailView.$fromDate.val()).toEqual("");
            });
            it("_Empty Todate", () => {
                // assert
                expect(winLossDetailView.$toDate.val()).toEqual("");
            });
            it("_Sets params to hidden fields", () => {
                // assert
                expect({
                    isFilterVisible: winLossDetailView.$isFilterVisible.val(),
                    isDescendingSort: winLossDetailView.$isDescendingSort.val(),
                    isHistoricReport: winLossDetailView.$isHistoricReport.val(),
                    isStackMode: winLossDetailView.$isStackMode.val(),
                    sortPropertyName: winLossDetailView.$sortPropertyName.val()
                }).toEqual({
                    isFilterVisible: "true",
                    isDescendingSort: "false",
                    isHistoricReport: "false",
                    isStackMode: "false",
                    sortPropertyName: "WinLoss"
                });
            });
        });
        it("_Switch View on Mobile -> Set Filter Is Visible to false", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossDetailView.init();
            winLossDetailView.isSwitchView = true;

            // act
            winLossDetailView.onSubmitReport(null);

            // assert
            expect(winLossDetailView.$isFilterVisible.val()).toEqual("true");
        });
        it("_Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossDetailView.init();
            winLossDetailView.isSwitchView = false;

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossDetailView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_getSortedColumnName", () => {
        it("_Normal Mode => Returns Sorted Column name data on table report", () => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.init();

            // act
            var actualSortedColumnName = winLossDetailView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual("WinLoss");
        });

        let sortReportOnStackModeTest = (sortedColumnName: string, expectedSortedCoumnResult: string) => {
            // arrange
            setFixtures(pokerGamesWinLossDetailTemplate.currentReportWithStackMode);
            winLossDetailView.init();
            winLossDetailView.$tableReport.data("sortedcolumn", sortedColumnName);

            // act
            var actualSortedColumnName = winLossDetailView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual(expectedSortedCoumnResult);
        };

        it("_Stack Mode, Sort on NetTurnover => Returns Name Turnover", () => {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
        });
        it("_Stack Mode, Sort on GrossComm => Returns Name Turnover", () => {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
        });
        it("_Stack Mode, Sort on MemberWinLoss.Comm => Returns Name MemberWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MemberWinLoss.Comm", "MemberWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MemberWinLoss.Comm => Returns Name MemberWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MemberWinLoss.Comm", "MemberWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MemberWinLoss.Total => Returns Name MemberWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MemberWinLoss.Total", "MemberWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentWinLoss.Comm => Returns Name AgentWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("AgentWinLoss.Comm", "AgentWinLoss.InsuranceGemsWinLoss");
        });
        it("_Stack Mode, Sort on AgentWinLoss.Total => Returns Name AgentWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("AgentWinLoss.Total", "AgentWinLoss.InsuranceGemsWinLoss");
        });
        it("_Stack Mode, Sort on MasterWinLoss.Comm => Returns Name MasterWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("MasterWinLoss.Comm", "MasterWinLoss.InsuranceGemsWinLoss");
        });
        it("_Stack Mode, Sort on MasterWinLoss.Total => Returns Name MasterWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("MasterWinLoss.Total", "MasterWinLoss.InsuranceGemsWinLoss");
        });
        it("_Stack Mode, Sort on SuperWinLoss.Comm => Returns Name SuperWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("SuperWinLoss.Comm", "SuperWinLoss.InsuranceGemsWinLoss");
        });
        it("_Stack Mode, Sort on SuperWinLoss.Total => Returns Name SuperWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("SuperWinLoss.Total", "SuperWinLoss.InsuranceGemsWinLoss");
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

        it("_Stack Mode, Sort on MemberWinLoss.InsuranceGemsWinLoss => Returns Name MemberWinLoss.WinLoss", () => {
            sortReportOnStackModeTest("MemberWinLoss.WinLoss", "MemberWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentWinLoss.InsuranceGemsWinLoss => Returns Name AgentWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("AgentWinLoss.Total", "AgentWinLoss.InsuranceGemsWinLoss");
        });
        it("_Stack Mode, Sort on MasterWinLoss.InsuranceGemsWinLoss => Returns Name MasterWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("MasterWinLoss.Total", "MasterWinLoss.InsuranceGemsWinLoss");
        });
        it("_Stack Mode, Sort on SuperWinLoss.InsuranceGemsWinLoss => Returns Name SuperWinLoss.InsuranceGemsWinLoss", () => {
            sortReportOnStackModeTest("SuperWinLoss.InsuranceGemsWinLoss", "SuperWinLoss.InsuranceGemsWinLoss");
        });
    });
});