describe("WinLossDetail", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);

    let winLossDetailView: WinLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        winLossDetailView = new WinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            pagerComponent,
            productMultiSelectionComponent,
            sortComponent,
            reportTooltip,
            monthPickerComponent,
            netTurnoverTooltipComponent,
            linkBuilderComponent);

        cacheElements = spyOn(winLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(winLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        renderLinkBuilderComponent = spyOn(linkBuilderComponent, "render");
        spyOn(dateRangePickerComponent, "render");
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
        });
        describe("_Document is ready", () => {
            beforeEach(() => {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);

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
        });

        it("_Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });

        it("_Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", () => {
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
        it("_Historic Report With Hidden Filter -> Shows Filter Before Renders Month Picker", () => {
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
        it("_Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render").and.callThrough();

            // act
            winLossDetailView.init();
            var actualIsHidden = $("#report-form").hasClass("hide");

            // assert
            expect(actualIsHidden).toBeTruthy();
        });

        it("_Report Of Top Level With no-Submit Param on Mobile -> Set Stack Mode as Default", () => {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            let setStackMode = spyOn(winLossDetailView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            // act
            winLossDetailView.init();

            // assert
            expect(setStackMode).toHaveBeenCalled();
        });

        it("_Report Of Top Level With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", () => {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);
            spyOn(reportTooltip, "show");

            // act
            winLossDetailView.init();

            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });

        it("_Always -> Render sort component with correct options", () => {
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
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);

            // act
            var actualIsReady = winLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
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
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
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
                setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
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
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
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
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
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
                setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
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
                setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
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
            it("_Position Net Turnover Tooltip", () => {
                // assert
                expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalled();
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
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
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
                    [jasmine.any(Object), 3, "col-member-winloss-combined"],
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
                setFixtures(winLossDetailTemplate.currentReportWithStackMode);
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
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
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
                setFixtures(winLossDetailTemplate.currentReportWithStackMode);
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
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
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
    describe("_afterSort", () => {
        it("_Not Have All Data on Client -> Submits Report", () => {
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

        it(`_Have All Data on Client and Currently Sorts ASC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(winLossDetailTemplate.sortASCOnNormalMode, false, `<tr class=""><td>Row 1</td></tr>
                                    <tr class="even-row"><td>Row 2</td></tr>
                                    <tr><td>Row 3</td></tr>
                                    <tr class="even-row"><td>Row 4</td></tr>`);
            });

        it(`_Have All Data on Client and Currently Sorts DESC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(winLossDetailTemplate.sortDESCOnNormalMode, true, "");
            });
        it(`_Have All Data on Client and Currently Sorts DESC Combined Column on Stack Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(winLossDetailTemplate.sortDESCCombinedColumnOnStackMode, true, "");
            });
    });
    describe("_hideFilter_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
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
        it("_Updates Net Turnover Tooltip", () => {
            // assert
            expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalledWith(winLossDetailView.$netTurnoverTooltip);
        });
    });
    describe("_getParams", () => {
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

        it("_No product selected -> Returns correct params", () => {
            getParamsTest([], false);
        });
        it("_Have product selected -> Returns correct params", () => {
            getParamsTest([1], true);
        });
    });
    describe("_showValidateProductError", () => {
        it("_Always -> Adds class error to multiple select button of dropdown product", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.init();

            // act
            winLossDetailView.showValidateProductError();

            // assert
            expect(winLossDetailView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("_clearError", () => {
        it("_Always -> Removes class error from multiple select button of dropdown product", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.init();

            // act
            winLossDetailView.clearError();

            // assert
            expect(winLossDetailView.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
    });
    describe("_onSubmitReport", () => {
        describe("_Switch View", () => {
            beforeEach(() => {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                renderProductMultiSelectionComponent.and.callThrough();
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
        it("_Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
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
    describe("_getSortedColumnName", () => {
        it("_Normal Mode => Returns Sorted Column name data on table report", () => {
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
        it("_Stack Mode, Sort on NoOfTickets => Returns Name NoOfTickets", () => {
            sortReportOnStackModeTest("NoOfTickets", "NoOfTickets");
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
});