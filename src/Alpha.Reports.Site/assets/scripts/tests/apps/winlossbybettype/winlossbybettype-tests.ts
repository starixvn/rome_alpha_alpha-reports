describe("WinLossByBetType_", () => {
    const jQueryFake = $;
    const sortComponent = new SortComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const reportTooltip = new ReportTooltip($);
    let winLossByBettypeView: WinLossByBettypeView;

    let cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        winLossByBettypeView = new WinLossByBettypeView(
            jQueryFake,
            dateRangePickerComponent,
            sortComponent,
            reportTooltip,
            linkBuilderComponent);

        cacheDOMElements = spyOn(winLossByBettypeView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();

        registerEvents = spyOn(winLossByBettypeView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderSortComponent = spyOn(sortComponent, "render");
        renderLinkBuilderComponent = spyOn(linkBuilderComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });

    describe("init", () => {
        it("Document is not ready -> Does not call any Method", () => {
            // act
            winLossByBettypeView.init();

            // assert
            expect(winLossByBettypeView.cacheDOMElements).not.toHaveBeenCalled();
            expect(winLossByBettypeView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();

            expect(sortComponent.render).not.toHaveBeenCalled();
        });

        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);

            // act
            winLossByBettypeView.init();

            // assert
            expect(winLossByBettypeView.cacheDOMElements).toHaveBeenCalled();
            expect(winLossByBettypeView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();

            expect(sortComponent.render).toHaveBeenCalled();
        });

        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);

            // act
            winLossByBettypeView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });

        it("Always -> Render sort component with correct options", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);

            // act
            winLossByBettypeView.init();

            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                winLossByBettypeView.$tableReport,
                false,
                true,
                jasmine.any(Function),
                { 0: { sorter: "text" } }
            ));
        });
    });

    describe("cacheDOMElements", () => {
        it("Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);

            // act
            winLossByBettypeView.cacheDOMElements();

            // assert
            expect(winLossByBettypeView.$pageTitle).toBeDefined();
            expect(winLossByBettypeView.$tableReport).toBeDefined();
            expect(winLossByBettypeView.$drilldownLinks).toBeDefined();
            expect(winLossByBettypeView.$reportForm).toBeDefined();
            expect(winLossByBettypeView.$isFilterVisible).toBeDefined();
            expect(winLossByBettypeView.$isStackMode).toBeDefined();
            expect(winLossByBettypeView.$isAgentLevel).toBeDefined();
            expect(winLossByBettypeView.$reportForm).toBeDefined();
            expect(winLossByBettypeView.$userSelectedDateRangeOption).toBeDefined();
            expect(winLossByBettypeView.$filterIcon).toBeDefined();
            expect(winLossByBettypeView.$excelIcon).toBeDefined();
            expect(winLossByBettypeView.$switchModeIcon).toBeDefined();
            expect(winLossByBettypeView.$fromDate).toBeDefined();
            expect(winLossByBettypeView.$toDate).toBeDefined();
            expect(winLossByBettypeView.$sortPropertyName).toBeDefined();
            expect(winLossByBettypeView.$isDescendingSort).toBeDefined();
        });
    });

    describe("onFilterEvent", () => {
        it("Filter is currently shown -> Hides Filter and Update Href Param to False", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();

            spyOn(winLossByBettypeView, "hideFilter");
            spyOn(winLossByBettypeView, "showFilter");
            spyOn(winLossByBettypeView, "replaceDrilldownParam");

            // act
            winLossByBettypeView.onFilterEvent();

            // assert
            expect(winLossByBettypeView.hideFilter).toHaveBeenCalled();
            expect(winLossByBettypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsFilterVisible", false);
        });

        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();

            spyOn(winLossByBettypeView, "hideFilter");
            spyOn(winLossByBettypeView, "showFilter");
            spyOn(winLossByBettypeView, "replaceDrilldownParam");

            // act
            winLossByBettypeView.onFilterEvent();

            // assert
            expect(winLossByBettypeView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossByBettypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsFilterVisible", false);
        });
    });

    describe("onExportExcelEvent", () => {
        it("Always -> Submits Export Excel Form And Unblocks UI", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();
            spyOn(winLossByBettypeView.$exportExcelForm, "submit");

            // act
            winLossByBettypeView.onExportExcelEvent();

            // assert
            expect(winLossByBettypeView.$exportExcelForm.submit).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("hideFilter", () => {
        it("Always -> Add Hide class and hide filter", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();

            // act
            winLossByBettypeView.hideFilter();

            // assert
            expect(winLossByBettypeView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(winLossByBettypeView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });

    describe("showFilter", () => {
        it("Always -> Remove Hide class and showfilter", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();

            // act
            winLossByBettypeView.showFilter();

            // assert
            expect(winLossByBettypeView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossByBettypeView.$reportForm.hasClass("hide")).toBeFalsy();
        });
    });

    describe("getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();

            // act
            var actualParam = winLossByBettypeView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: expectedIsAnyProductSelected,
                IsStackMode: false,
                IsHistoricReport: false,
                IsFilterVisible: true
            });
        };
    });

    describe("onSubmitReportEvent", () => {
        it("Switch View -> Blocks UI; Empty FromDate/Todate; Sets params to hidden fields", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossByBettypeView.init();
            winLossByBettypeView.isSwitchView = true;

            // act
            winLossByBettypeView.onSubmitReportEvent(null);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();

            expect(winLossByBettypeView.$isFilterVisible.val()).toBeTruthy();
        });

        it("Switch View on Mobile -> Set Filter Is Visible to false", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossByBettypeView.init();
            winLossByBettypeView.isSwitchView = true;

            // act
            winLossByBettypeView.onSubmitReportEvent(null);

            // assert
            expect(winLossByBettypeView.$isFilterVisible.val()).toEqual("true");
        });

        it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossByBettypeView.init();
            winLossByBettypeView.isSwitchView = false;

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossByBettypeView.onSubmitReportEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("renderAtFirstSwitchMode", () => {
        it("Stack Mode Switch to Normal Mode -> Call Split Columns Method", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();
            spyOn(winLossByBettypeView, "splitColumns");

            // act
            winLossByBettypeView.renderAtFirstSwitchMode(true);

            // assert
            expect(winLossByBettypeView.splitColumns).toHaveBeenCalled();
        });

        it("Normal Mode Switch to Stack Mode -> Call Split combineColumns Method", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();
            spyOn(winLossByBettypeView, "combineColumns");

            // act
            winLossByBettypeView.renderAtFirstSwitchMode(false);

            // assert
            expect(winLossByBettypeView.combineColumns).toHaveBeenCalled();
        });
    });

    describe("setCssBySwitchMode", () => {
        it("Stack Mode Switch to Normal Mode -> Remove stack-mode class", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();

            // act
            winLossByBettypeView.setCssBySwitchMode(true);

            // assert
            expect(winLossByBettypeView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            expect(winLossByBettypeView.$tableReport.hasClass("stack-mode")).toBeFalsy();
        });

        it("Normal Mode Switch to Stack Mode -> Add stack-mode class", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();

            // act
            winLossByBettypeView.setCssBySwitchMode(false);

            // assert
            expect(winLossByBettypeView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossByBettypeView.$tableReport.hasClass("stack-mode")).toBeTruthy();
        });
    });

    describe("combineColumns", () => {
        it("Stack Mode At Agent Level -> Call combineToTurnOverColumnAgentLevel Method", () => {
            // arrange
            spyOn(winLossByBettypeView, "combineToTurnOverColumnAgentLevel");
            spyOn(winLossByBettypeView, "combineToWinLossColumn");

            // act
            winLossByBettypeView.combineColumns(true);

            // assert
            expect(winLossByBettypeView.combineToTurnOverColumnAgentLevel).toHaveBeenCalled();
            expect(winLossByBettypeView.combineToWinLossColumn).toHaveBeenCalled();
        });

        it("Stack Mode At Super Master Level -> Call combineToTurnOverColumnSuperMasterLevel AndcombineToWinLossColumn Method", () => {
            // arrange
            spyOn(winLossByBettypeView, "combineToTurnOverColumnSuperMasterLevel");
            spyOn(winLossByBettypeView, "combineToWinLossColumn");

            // act
            winLossByBettypeView.combineColumns(false);

            // assert
            expect(winLossByBettypeView.combineToTurnOverColumnSuperMasterLevel).toHaveBeenCalled();
            expect(winLossByBettypeView.combineToWinLossColumn).toHaveBeenCalled();
        });
    });

    describe("combineToWinLossColumn", () => {
        it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", () => {
            // arrange
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            winLossByBettypeView.combineToWinLossColumn('agent', 1);

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
            winLossByBettypeView.combineToTurnOverColumnAgentLevel();

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
            winLossByBettypeView.combineToTurnOverColumnSuperMasterLevel();

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
            winLossByBettypeView.splitColumns();

            // assert
            expect(splitCellSpy.calls.allArgs()).toEqual([
                ["col-turnover"],
                ["col-member-winloss", ""],
                ["col-agent-winloss", " altercol"],
                ["col-master-winloss", " altercol"],
                ["col-super-winloss", " altercol"]]);
        });
    });

    describe("replaceDrilldownParam  ", () => {
        it("Drill Down-> Call updateParam Method", () => {
            // arrange
            setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
            winLossByBettypeView.cacheDOMElements();
            spyOn(linkBuilderComponent, "updateParam");

            // act
            winLossByBettypeView.replaceDrilldownParam("IsFilterVisible", true);

            // assert
            expect(linkBuilderComponent.updateParam).toHaveBeenCalled();
        });
    });

    describe("onSwitchModeEvent   ", () => {
        it(`First Switch Mode->
            Call setCssBySwitchMode Method,
            Call renderAtFirstSwitchMode Method,
            Call replaceDrilldownParam Method`, () => {
                // arrange
                setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
                winLossByBettypeView.cacheDOMElements();

                spyOn(winLossByBettypeView, "setCssBySwitchMode");
                spyOn(winLossByBettypeView, "replaceDrilldownParam");
                spyOn(sortComponent, "bindTableSorter");

                // act
                winLossByBettypeView.onSwitchModeEvent();

                // assert
                expect(winLossByBettypeView.setCssBySwitchMode).toHaveBeenCalledWith(false);
                expect(winLossByBettypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsStackMode", true);
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });

        it(`From Second Switch Mode->
            Call setCssBySwitchMode Method,
            Call bindTableSorter Method,
            Call replaceDrilldownParam Method`, () => {
                // arrange
                setFixtures(winlossbybettypeTemplate.reportWithNormalMode);
                winLossByBettypeView.cacheDOMElements();
                spyOn(sortComponent, "bindTableSorter");
                spyOn(winLossByBettypeView, "setCssBySwitchMode");
                spyOn(winLossByBettypeView, "replaceDrilldownParam");

                // act
                winLossByBettypeView.onSwitchModeEvent();

                // assert
                expect(winLossByBettypeView.setCssBySwitchMode).toHaveBeenCalled();
                expect(winLossByBettypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsStackMode", true);
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });
    });

    describe("getSortedColumnName", () => {
        let sortReportOnStackModeTest = (sortedColumnName: string, expectedSortedCoumnResult: string) => {
            // arrange
            setFixtures(winLossTemplate.currentReportWithStackMode);
            winLossByBettypeView.init();
            winLossByBettypeView.$tableReport.data("sortedcolumn", sortedColumnName);

            // act
            var actualSortedColumnName = winLossByBettypeView.getSortedColumnName();

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
});