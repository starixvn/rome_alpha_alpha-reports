describe("WinLossAnalysis_", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    let winLossAnalysisView: WinLossAnalysisView;

    let cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        winLossAnalysisView = new WinLossAnalysisView(
            jQueryFake,
            dateRangePickerComponent,
            productMultiSelectionComponent,
            sortComponent,
            reportTooltip,
            linkBuilderComponent);

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

    describe("init", () => {
        it("Document is not ready -> Does not call any Method", () => {
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

        it("Document is ready -> Calls Specific Methods", () => {
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

        it("Current Report -> Renders Date Range Picker", () => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);

            // act
            winLossAnalysisView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });


        it("Always -> Render sort component with correct options", () => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);

            // act
            winLossAnalysisView.init();

            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                winLossAnalysisView.$tableReport,
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
            var actualIsReady = winLossAnalysisView.checkReady();

            // assert
            expect(actualIsReady).toBeFalsy();
        });

        it("Document is loaded (ready) -> Returns True", () => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);

            // act
            var actualIsReady = winLossAnalysisView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });

    describe("cacheDOMElements", () => {
        it("Always -> All neccessary elements have been cached", () => {
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

    describe("setStackMode", () => {
        let combineConsecutiveCellsSpy: jasmine.Spy;
        beforeEach(() => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "setCssBySwitchMode");
            spyOn(winLossAnalysisView, "renderAtFirstSwitchMode");

            // act
            winLossAnalysisView.setStackMode();
        });

        it("Always -> Blocks UI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });

        it(`Navigate From dashboard on mobile->
            Call setCssBySwitchMode Method,
            Call renderAtFirstSwitchMode Method`, () => {

                // assert
                expect(winLossAnalysisView.setCssBySwitchMode).toHaveBeenCalledWith(false);
                expect(winLossAnalysisView.renderAtFirstSwitchMode).toHaveBeenCalledWith(false);
            });

        it("Always -> Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("onFilterEvent", () => {
        it("Filter is currently hidden -> Call hideFilter Function ", () => {
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

        it("Filter is currently hidden -> Page title is Hidden", () => {
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

    describe("onExportExcelEvent", () => {
        it("Always -> Submits Export Excel Form And Unblocks UI", () => {
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

    describe("hideFilter", () => {
        it("Always -> Add Hide class and hide filter", () => {
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

    describe("showFilter", () => {
        it("Always -> Remove Hide class and showfilter", () => {
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

    describe("getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => selectedProduct);

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

    describe("showValidaProductError", () => {
        it("Always -> Adds class error to multiple select button of dropdown product", () => {
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

    describe("clearError", () => {
        it("Always -> Removes class error from multiple select button of dropdown product", () => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.init();

            // act
            winLossAnalysisView.clearError();

            // assert
            expect(winLossAnalysisView.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
    });

    describe("onSubmitReportEvent", () => {
        it("Switch View -> Blocks UI; Empty FromDate/Todate; Sets params to hidden fields", () => {
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

        it("Switch View on Mobile -> Set Filter Is Visible to false", () => {
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

        it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            renderSortComponent.and.callThrough();
            winLossAnalysisView.init();
            winLossAnalysisView.isSwitchView = false;
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossAnalysisView.onSubmitReportEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("getSortedColumnName", () => {
        it("Normal Mode => Returns Sorted Column name data on table report", () => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.init();

            // act
            var actualSortedColumnName = winLossAnalysisView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual("ReportTypeName");
        });

        let sortReportOnStackModeTest = (sortedColumnName: string, expectedSortedCoumnResult: string) => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithStackMode);
            winLossAnalysisView.init();
            winLossAnalysisView.$tableReport.data("sortedcolumn", sortedColumnName);

            // act
            var actualSortedColumnName = winLossAnalysisView.getSortedColumnName();

            // assert
            expect(actualSortedColumnName).toEqual(expectedSortedCoumnResult);
        };

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

        it("Stack Mode, Sort on BetCount => Returns Name BetCount", () => {
            sortReportOnStackModeTest("BetCount", "BetCount");
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
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();
            spyOn(winLossAnalysisView, "splitColumns");

            // act
            winLossAnalysisView.renderAtFirstSwitchMode(true);

            // assert
            expect(winLossAnalysisView.splitColumns).toHaveBeenCalled();
        });

        it("Normal Mode Switch to Stack Mode -> Call Split combineColumns Method", () => {
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

    describe("setCssBySwitchMode", () => {
        it("Stack Mode Switch to Normal Mode -> Remove stack-mode class", () => {
            // arrange
            setFixtures(winLossAnalysisTemplate.reportWithNormalMode);
            winLossAnalysisView.cacheDOMElements();

            // act
            winLossAnalysisView.setCssBySwitchMode(true);

            // assert
            expect(winLossAnalysisView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            expect(winLossAnalysisView.$tableReport.hasClass("stack-mode")).toBeFalsy();
        });

        it("Normal Mode Switch to Stack Mode -> Add stack-mode class", () => {
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

    describe("combineColumns", () => {
        it("Stack Mode -> Call combine column Method", () => {
            // arrange
            let combineConsecutiveCellsSpy: jasmine.Spy;
            combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            winLossAnalysisView.combineColumns();

            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
                [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                [jasmine.any(Object), 3, "col-master-winloss-combined"],
                [jasmine.any(Object), 3, "col-super-winloss-combined altercol"]]);
        });
    });

    describe("splitColumns ", () => {
        it("Switch Normal Mode to Normal Mode -> Call splitCell Method", () => {
            // arrange
            var splitCellSpy = spyOn(alphaReports, "splitCell");

            // act
            winLossAnalysisView.splitColumns();

            // assert
            expect(splitCellSpy.calls.allArgs()).toEqual([
                ["col-member-winloss", ""],
                ["col-agent-winloss", " altercol"],
                ["col-master-winloss", ""],
                ["col-super-winloss", " altercol"]]);
        });
    });


    describe("onSwitchModeEvent   ", () => {
        it(`First Switch Mode->
            Call setCssBySwitchMode Method,
            Call renderAtFirstSwitchMode Method`, () => {
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

        it(`From Second Switch Mode->
            Call setCssBySwitchMode Method,
            Call bindTableSorter Method`, () => {
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