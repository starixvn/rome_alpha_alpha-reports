describe("WinLossDetail", function () {
    var jQueryFake = $;
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var pagerComponent = new PagerComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var sortComponent = new SortComponent($, linkBuilderComponent);
    var monthPickerComponent = new MonthPickerComponent($);
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var winLossDetailView;
    var cacheElements, registerEvents, blockUI, unblockUI, renderProductMultiSelectionComponent, renderPagerComponent, renderSortComponent, renderLinkBuilderComponent;
    beforeEach(function () {
        winLossDetailView = new WinLossDetailView(jQueryFake, dateRangePickerComponent, pagerComponent, productMultiSelectionComponent, sortComponent, reportTooltip, monthPickerComponent, linkBuilderComponent);
        spyOn(winLossDetailView, "loadLegend");
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
        renderLinkBuilderComponent.and.callThrough();
        spyOn(dateRangePickerComponent, "render");
    });
    it("Inherit BaseReport_Always_WinLossDetailView is BaseReport", function () {
        // assert
        expect(winLossDetailView instanceof BaseReport).toBeTruthy();
    });
    describe("_init", function () {
        describe("_Document is not ready", function () {
            beforeEach(function () {
                // act
                winLossDetailView.init();
            });
            it("_Does not call cacheElements", function () {
                // assert
                expect(winLossDetailView.cacheElements).not.toHaveBeenCalled();
            });
            it("_Does not call registerEvents", function () {
                // assert
                expect(winLossDetailView.registerEvents).not.toHaveBeenCalled();
            });
            it("_Does not call blockUI", function () {
                // assert
                expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            });
            it("_Does not call unblockUI", function () {
                // assert
                expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
            });
            it("_Does not call productMultiSelectionComponent.render", function () {
                // assert
                expect(productMultiSelectionComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call pagerComponent.render", function () {
                // assert
                expect(pagerComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call sortComponent.render", function () {
                // assert
                expect(sortComponent.render).not.toHaveBeenCalled();
            });
        });
        describe("_Document is ready", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                spyOn(winLossDetailView, "initHelp");
                // act
                winLossDetailView.init();
            });
            it("_Calls cacheElements", function () {
                // assert
                expect(winLossDetailView.cacheElements).toHaveBeenCalled();
            });
            it("_Calls registerEvents", function () {
                // assert
                expect(winLossDetailView.registerEvents).toHaveBeenCalled();
            });
            it("_Calls blockUI", function () {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Calls unblockUI", function () {
                // assert
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
            it("_Calls productMultiSelectionComponent.render", function () {
                // assert
                expect(productMultiSelectionComponent.render).toHaveBeenCalled();
            });
            it("_Calls pagerComponent.render", function () {
                // assert
                expect(pagerComponent.render).toHaveBeenCalled();
            });
            it("_Calls sortComponent.render", function () {
                // assert
                expect(sortComponent.render).toHaveBeenCalled();
            });
            it("_Init Help", function () {
                // assert
                expect(winLossDetailView.initHelp).toHaveBeenCalled();
            });
        });
        it("_Current Report -> Renders Date Range Picker And Not Render Month Picker", function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            spyOn(monthPickerComponent, "render");
            // act
            winLossDetailView.init();
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });
        it("_Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", function () {
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
        it("_Historic Report With Hidden Filter -> Shows Filter Before Renders Month Picker", function () {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            var actualIsHidden;
            spyOn(monthPickerComponent, "render").and.callFake(function () {
                actualIsHidden = $("#report-form").hasClass("hide");
            });
            // act
            winLossDetailView.init();
            // assert
            expect(actualIsHidden).toBeFalsy();
        });
        // historic report: we must show filter before render month picker
        // therefore, after init we also hide it if before that it is hidden
        it("_Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", function () {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render").and.callThrough();
            // act
            winLossDetailView.init();
            var actualIsHidden = $("#report-form").hasClass("hide");
            // assert
            expect(actualIsHidden).toBeTruthy();
        });
        it("_Report Of Top Level With no-Submit Param on Mobile -> Set Stack Mode as Default", function () {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            var setStackMode = spyOn(winLossDetailView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return true; });
            // act
            winLossDetailView.init();
            // assert
            expect(setStackMode).toHaveBeenCalled();
        });
        it("_Report Of Top Level With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", function () {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return true; });
            spyOn(reportTooltip, "show");
            // act
            winLossDetailView.init();
            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });
        it("_Always -> Render sort component with correct options", function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            spyOn(pagerComponent, "hasAllDataOnClient").and.callFake(function () { return true; });
            // act
            winLossDetailView.init();
            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(winLossDetailView.$tableReport, false, true, sortComponent.afterSort, { 0: { sorter: "text" } }, winLossDetailView.$reportForm));
        });
    });
    describe("_checkReady", function () {
        it("_Document is not loaded yet (empty) -> Returns False", function () {
            // arrange
            setFixtures("");
            // act
            var actualIsReady = winLossDetailView.checkReady();
            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("_Document is loaded (ready) -> Returns True", function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            // act
            var actualIsReady = winLossDetailView.checkReady();
            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
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
    describe("_onSwitchReport", function () {
        describe("_Current Report", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView.$reportForm, "trigger");
                // act
                winLossDetailView.onSwitchReport();
            });
            it("_Switch to Historic Report", function () {
                // assert
                expect(winLossDetailView.isSwitchView).toBeTruthy();
                expect(winLossDetailView.isHistoricReport).toBeTruthy();
            });
            it("_Submit", function () {
                // assert
                expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
            });
        });
        describe("_Historic Report", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView.$reportForm, "trigger");
                // act
                winLossDetailView.onSwitchReport();
            });
            it("_Switch to Current Report", function () {
                // assert
                expect(winLossDetailView.isSwitchView).toBeTruthy();
                expect(winLossDetailView.isHistoricReport).toBeFalsy();
            });
            it("_Submit", function () {
                // assert
                expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
            });
        });
    });
    describe("_setStackMode_Always", function () {
        var combineConsecutiveCellsSpy;
        beforeEach(function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView, "replaceHrefParam");
            combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
            // act
            winLossDetailView.setStackMode();
        });
        it("_Blocks UI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Set stack-mode to table reports", function () {
            // assert
            expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
        });
        it("_Combines Columns", function () {
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "col-turnover-combined"],
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
                [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                [jasmine.any(Object), 3, "col-master-winloss-combined"],
                [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]
            ]);
        });
        it("_Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onClickIconFilter", function () {
        describe("_Filter is currently shown", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView, "hideFilter");
                spyOn(winLossDetailView, "replaceHrefParam");
                // act
                winLossDetailView.onClickIconFilter();
            });
            it("_Hides Filter", function () {
                // assert
                expect(winLossDetailView.hideFilter).toHaveBeenCalled();
            });
            it("_Update Href Param to False", function () {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
            });
        });
        describe("_Filter is currently hidden", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
                winLossDetailView.cacheElements();
                spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
                spyOn(winLossDetailView, "replaceHrefParam");
                // act
                winLossDetailView.onClickIconFilter();
            });
            it("_Hides Filter", function () {
                // assert
                expect(winLossDetailView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(winLossDetailView.$reportForm.hasClass("hide")).toBeFalsy();
            });
            it("_Update Href Param to True", function () {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
            });
        });
    });
    describe("_onSwitchMode", function () {
        describe("_1st Switch on Normal Mode", function () {
            var combineConsecutiveCellsSpy;
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
                // act
                winLossDetailView.onSwitchMode();
            });
            it("_Blocks UI", function () {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Unblocks UI", function () {
                // assert
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
            it("_Adds Class stack-mode to Table Report", function () {
                // assert
                expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Adds Class stack-mode to Page Title", function () {
                // assert
                expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Updates HRef Param", function () {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
            });
            it("_Combines Cells", function () {
                // assert
                expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                    [jasmine.any(Object), 3, "col-turnover-combined"],
                    [jasmine.any(Object), 3, "col-member-winloss-combined"],
                    [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                    [jasmine.any(Object), 3, "col-master-winloss-combined"],
                    [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]
                ]);
            });
            it("_Rebinds Table Sorter", function () {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(winLossDetailView.getSortedColumnName());
            });
        });
        describe("_1st Switch on Stack Mode", function () {
            var splitCellSpy;
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithStackMode);
                winLossDetailView.cacheElements();
                spyOn(winLossDetailView, "replaceHrefParam");
                spyOn(sortComponent, "bindTableSorter");
                splitCellSpy = spyOn(alphaReports, "splitCell");
                // act
                winLossDetailView.onSwitchMode();
            });
            it("_Blocks UI", function () {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Unblocks UI", function () {
                // assert
                expect(jQueryFake.unblockUI).toHaveBeenCalled();
            });
            it("_Removes Class stack-mode to Table Report", function () {
                // assert
                expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Removes Class stack-mode to Page Title", function () {
                // assert
                expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Updates HRef Param", function () {
                // assert
                expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", false);
            });
            it("_Splits Cells", function () {
                // assert
                expect(splitCellSpy.calls.allArgs()).toEqual([
                    ["col-turnover"],
                    ["col-member-winloss", ""],
                    ["col-agent-winloss", " altercol"],
                    ["col-master-winloss", ""],
                    ["col-super-winloss", " altercol"]
                ]);
            });
            it("_Rebinds Table Sorter", function () {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalledWith(winLossDetailView.getSortedColumnName());
            });
        });
        describe("_2nd+ Switch on Normal Mode", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                winLossDetailView.cacheElements();
                winLossDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");
                // act
                winLossDetailView.onSwitchMode();
            });
            it("_Not Combine Cells", function () {
                // assert
                expect(alphaReports.combineConsecutiveCells).not.toHaveBeenCalled();
            });
            it("_Not Splits Cells", function () {
                // assert
                expect(alphaReports.splitCell).not.toHaveBeenCalled();
            });
        });
        describe("_2nd+ Switch on Stack Mode", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithStackMode);
                winLossDetailView.cacheElements();
                winLossDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");
                // act
                winLossDetailView.onSwitchMode();
            });
            it("Not Combine Cells", function () {
                // assert
                expect(alphaReports.combineConsecutiveCells).not.toHaveBeenCalled();
            });
            it("Not Splits Cells", function () {
                // assert
                expect(alphaReports.splitCell).not.toHaveBeenCalled();
            });
        });
    });
    describe("_onExportExcel_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$exportExcelForm, "submit");
            // act
            winLossDetailView.onExportExcel();
        });
        it("_Submits Export Excel Form", function () {
            // assert
            expect(winLossDetailView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_hideFilter_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
            // act
            winLossDetailView.hideFilter();
        });
        it("_Updates Page Title", function () {
            // assert
            expect(winLossDetailView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
        });
        it("_Updates Filter", function () {
            // assert
            expect(winLossDetailView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });
    describe("_getParams", function () {
        var getParamsTest = function (selectedProduct, expectedIsAnyProductSelected) {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return selectedProduct; });
            spyOn(sortComponent, "getSortInfo").and.callFake(function () {
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
                IsAnyProductDetailSelected: true,
                SortPropertyName: "WinLoss",
                IsDescendingSort: true,
                IsStackMode: false,
                IsHistoricReport: false,
                IsFilterVisible: true
            });
        };
        it("_No product selected -> Returns correct params", function () {
            getParamsTest([], false);
        });
        it("_Have product selected -> Returns correct params", function () {
            getParamsTest([1], true);
        });
    });
    describe("_showValidateProductError", function () {
        it("_Always -> Adds class error to multiple select button of dropdown product", function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            productMultiSelectionComponent.$excludeProducts = [];
            winLossDetailView.init();
            // act
            winLossDetailView.showValidateProductError();
            // assert
            expect(winLossDetailView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("_clearError", function () {
        it("_Always -> Removes class error from multiple select button of dropdown product", function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.init();
            // act
            winLossDetailView.clearError();
            // assert
            expect(winLossDetailView.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
    });
    describe("_onSubmitReport", function () {
        describe("_Switch View", function () {
            beforeEach(function () {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                renderProductMultiSelectionComponent.and.callThrough();
                renderSortComponent.and.callThrough();
                winLossDetailView.init();
                winLossDetailView.isSwitchView = true;
                // act
                winLossDetailView.onSubmitReport(null);
            });
            it("_Blocks UI", function () {
                // assert
                expect(jQueryFake.blockUI).toHaveBeenCalled();
            });
            it("_Empty FromDate", function () {
                // assert
                expect(winLossDetailView.$fromDate.val()).toEqual("");
            });
            it("_Empty Todate", function () {
                // assert
                expect(winLossDetailView.$toDate.val()).toEqual("");
            });
            it("_Sets params to hidden fields", function () {
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
        it("_Switch View on Mobile -> Set Filter Is Visible to false", function () {
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
        it("_Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossDetailView.init();
            winLossDetailView.isSwitchView = false;
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            winLossDetailView.onSubmitReport(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_getSortedColumnName", function () {
        it("_Normal Mode => Returns Sorted Column name data on table report", function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            winLossDetailView.init();
            // act
            var actualSortedColumnName = winLossDetailView.getSortedColumnName();
            // assert
            expect(actualSortedColumnName).toEqual("WinLoss");
        });
        var sortReportOnStackModeTest = function (sortedColumnName, expectedSortedCoumnResult) {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithStackMode);
            winLossDetailView.init();
            winLossDetailView.$tableReport.data("sortedcolumn", sortedColumnName);
            // act
            var actualSortedColumnName = winLossDetailView.getSortedColumnName();
            // assert
            expect(actualSortedColumnName).toEqual(expectedSortedCoumnResult);
        };
        it("_Stack Mode, Sort on NetTurnover => Returns Name Turnover", function () {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
        });
        it("_Stack Mode, Sort on GrossComm => Returns Name Turnover", function () {
            sortReportOnStackModeTest("NetTurnover", "Turnover");
        });
        it("_Stack Mode, Sort on MemberNetWinLoss.Comm => Returns Name MemberNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MemberNetWinLoss.Comm", "MemberNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MemberNetWinLoss.Total => Returns Name MemberNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MemberNetWinLoss.Total", "MemberNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentNetWinLoss.Comm => Returns Name AgentNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("AgentNetWinLoss.Comm", "AgentNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentNetWinLoss.Total => Returns Name AgentNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MasterNetWinLoss.Comm => Returns Name MasterNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MasterNetWinLoss.Comm", "MasterNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MasterNetWinLoss.Total => Returns Name MasterNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on SuperNetWinLoss.Comm => Returns Name SuperNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("SuperNetWinLoss.Comm", "SuperNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on SuperNetWinLoss.Total => Returns Name SuperNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("SuperNetWinLoss.Total", "SuperNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on Username => Returns Name Username", function () {
            sortReportOnStackModeTest("Username", "Username");
        });
        it("_Stack Mode, Sort on BetCount => Returns Name BetCount", function () {
            sortReportOnStackModeTest("BetCount", "BetCount");
        });
        it("_Stack Mode, Sort on Turnover => Returns Name Turnover", function () {
            sortReportOnStackModeTest("Turnover", "Turnover");
        });
        it("_Stack Mode, Sort on Company => Returns Name Company", function () {
            sortReportOnStackModeTest("Company", "Company");
        });
        it("_Stack Mode, Sort on MemberNetWinLoss.WinLoss => Returns Name MemberNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MemberNetWinLoss.WinLoss", "MemberNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on AgentNetWinLoss.WinLoss => Returns Name AgentNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("AgentNetWinLoss.Total", "AgentNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on MasterNetWinLoss.WinLoss => Returns Name MasterNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("MasterNetWinLoss.Total", "MasterNetWinLoss.WinLoss");
        });
        it("_Stack Mode, Sort on SuperNetWinLoss.WinLoss => Returns Name SuperNetWinLoss.WinLoss", function () {
            sortReportOnStackModeTest("SuperNetWinLoss.WinLoss", "SuperNetWinLoss.WinLoss");
        });
    });
});
//# sourceMappingURL=winlossdetail-tests.js.map