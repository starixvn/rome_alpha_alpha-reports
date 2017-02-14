describe("WinLossByGameType_", () => {
    const jQueryFake = $;
    const sortComponent = new SortComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const reportTooltip = new ReportTooltip($);
    let winLossByGameTypeView: WinLossByGameTypeView;

    let cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        winLossByGameTypeView = new WinLossByGameTypeView(
            jQueryFake,
            dateRangePickerComponent,
            sortComponent,
            reportTooltip,
            linkBuilderComponent);

        cacheDOMElements = spyOn(winLossByGameTypeView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();

        registerEvents = spyOn(winLossByGameTypeView, "registerEvents");
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
            winLossByGameTypeView.init();

            // assert
            expect(winLossByGameTypeView.cacheDOMElements).not.toHaveBeenCalled();
            expect(winLossByGameTypeView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();

            expect(sortComponent.render).not.toHaveBeenCalled();
        });

        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);

            // act
            winLossByGameTypeView.init();

            // assert
            expect(winLossByGameTypeView.cacheDOMElements).toHaveBeenCalled();
            expect(winLossByGameTypeView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();

            expect(sortComponent.render).toHaveBeenCalled();
        });

        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);

            // act
            winLossByGameTypeView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });

        it("Always -> Render sort component with correct options", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);

            // act
            winLossByGameTypeView.init();

            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                winLossByGameTypeView.$tableReport,
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
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);

            // act
            winLossByGameTypeView.cacheDOMElements();

            // assert
            expect(winLossByGameTypeView.$pageTitle).toBeDefined();
            expect(winLossByGameTypeView.$tableReport).toBeDefined();
            expect(winLossByGameTypeView.$drilldownLinks).toBeDefined();
            expect(winLossByGameTypeView.$reportForm).toBeDefined();
            expect(winLossByGameTypeView.$isFilterVisible).toBeDefined();
            expect(winLossByGameTypeView.$isStackMode).toBeDefined();
            expect(winLossByGameTypeView.$isAgentLevel).toBeDefined();
            expect(winLossByGameTypeView.$reportForm).toBeDefined();
            expect(winLossByGameTypeView.$userSelectedDateRangeOption).toBeDefined();
            expect(winLossByGameTypeView.$filterIcon).toBeDefined();
            expect(winLossByGameTypeView.$excelIcon).toBeDefined();
            expect(winLossByGameTypeView.$switchModeIcon).toBeDefined();
            expect(winLossByGameTypeView.$fromDate).toBeDefined();
            expect(winLossByGameTypeView.$toDate).toBeDefined();
            expect(winLossByGameTypeView.$sortPropertyName).toBeDefined();
            expect(winLossByGameTypeView.$isDescendingSort).toBeDefined();
        });
    });

    describe("onFilterEvent", () => {
        it("Filter is currently shown -> Hides Filter and Update Href Param to False", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();

            spyOn(winLossByGameTypeView, "hideFilter");
            spyOn(winLossByGameTypeView, "showFilter");
            spyOn(winLossByGameTypeView, "replaceDrilldownParam");

            // act
            winLossByGameTypeView.onFilterEvent();

            // assert
            expect(winLossByGameTypeView.hideFilter).toHaveBeenCalled();
            expect(winLossByGameTypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsFilterVisible", false);
        });

        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();

            spyOn(winLossByGameTypeView, "hideFilter");
            spyOn(winLossByGameTypeView, "showFilter");
            spyOn(winLossByGameTypeView, "replaceDrilldownParam");

            // act
            winLossByGameTypeView.onFilterEvent();

            // assert
            expect(winLossByGameTypeView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossByGameTypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsFilterVisible", false);
        });
    });

    describe("onExportExcelEvent", () => {
        it("Always -> Submits Export Excel Form And Unblocks UI", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();
            spyOn(winLossByGameTypeView.$exportExcelForm, "submit");

            // act
            winLossByGameTypeView.onExportExcelEvent();

            // assert
            expect(winLossByGameTypeView.$exportExcelForm.submit).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("hideFilter", () => {
        it("Always -> Add Hide class and hide filter", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();

            // act
            winLossByGameTypeView.hideFilter();

            // assert
            expect(winLossByGameTypeView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(winLossByGameTypeView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });

    describe("showFilter", () => {
        it("Always -> Remove Hide class and showfilter", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();

            // act
            winLossByGameTypeView.showFilter();

            // assert
            expect(winLossByGameTypeView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossByGameTypeView.$reportForm.hasClass("hide")).toBeFalsy();
        });
    });

    describe("getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();

            // act
            var actualParam = winLossByGameTypeView.getParams();

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
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossByGameTypeView.init();
            winLossByGameTypeView.isSwitchView = true;

            // act
            winLossByGameTypeView.onSubmitReportEvent(null);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();

            expect(winLossByGameTypeView.$isFilterVisible.val()).toBeTruthy();
        });

        it("Switch View on Mobile -> Set Filter Is Visible to false", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossByGameTypeView.init();
            winLossByGameTypeView.isSwitchView = true;

            // act
            winLossByGameTypeView.onSubmitReportEvent(null);

            // assert
            expect(winLossByGameTypeView.$isFilterVisible.val()).toEqual("true");
        });

        it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            renderSortComponent.and.callThrough();
            winLossByGameTypeView.init();
            winLossByGameTypeView.isSwitchView = false;

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossByGameTypeView.onSubmitReportEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("renderAtFirstSwitchMode", () => {
        it("Stack Mode Switch to Normal Mode -> Call Split Columns Method", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();
            spyOn(winLossByGameTypeView, "splitColumns");

            // act
            winLossByGameTypeView.renderAtFirstSwitchMode(true);

            // assert
            expect(winLossByGameTypeView.splitColumns).toHaveBeenCalled();
        });

        it("Normal Mode Switch to Stack Mode -> Call Split combineColumns Method", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();
            spyOn(winLossByGameTypeView, "combineColumns");

            // act
            winLossByGameTypeView.renderAtFirstSwitchMode(false);

            // assert
            expect(winLossByGameTypeView.combineColumns).toHaveBeenCalled();
        });
    });

    describe("setCssBySwitchMode", () => {
        it("Stack Mode Switch to Normal Mode -> Remove stack-mode class", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();

            // act
            winLossByGameTypeView.setCssBySwitchMode(true);

            // assert
            expect(winLossByGameTypeView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            expect(winLossByGameTypeView.$tableReport.hasClass("stack-mode")).toBeFalsy();
        });

        it("Normal Mode Switch to Stack Mode -> Add stack-mode class", () => {
            // arrange
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();

            // act
            winLossByGameTypeView.setCssBySwitchMode(false);

            // assert
            expect(winLossByGameTypeView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossByGameTypeView.$tableReport.hasClass("stack-mode")).toBeTruthy();
        });
    });

    describe("combineColumns", () => {
        it("Stack Mode At Agent Level -> Call combineToTurnOverColumnAgentLevel Method", () => {
            // arrange
            spyOn(winLossByGameTypeView, "combineToTurnOverColumnAgentLevel");
            spyOn(winLossByGameTypeView, "combineToWinLossColumn");

            // act
            winLossByGameTypeView.combineColumns(true);

            // assert
            expect(winLossByGameTypeView.combineToTurnOverColumnAgentLevel).toHaveBeenCalled();
            expect(winLossByGameTypeView.combineToWinLossColumn).toHaveBeenCalled();
        });

        it("Stack Mode At Super Master Level -> Call combineToTurnOverColumnSuperMasterLevel AndcombineToWinLossColumn Method", () => {
            // arrange
            spyOn(winLossByGameTypeView, "combineToTurnOverColumnSuperMasterLevel");
            spyOn(winLossByGameTypeView, "combineToWinLossColumn");

            // act
            winLossByGameTypeView.combineColumns(false);

            // assert
            expect(winLossByGameTypeView.combineToTurnOverColumnSuperMasterLevel).toHaveBeenCalled();
            expect(winLossByGameTypeView.combineToWinLossColumn).toHaveBeenCalled();
        });
    });

    describe("combineToWinLossColumn", () => {
        it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", () => {
            // arrange
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            winLossByGameTypeView.combineToWinLossColumn('agent', 1);

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
            winLossByGameTypeView.combineToTurnOverColumnAgentLevel();

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
            winLossByGameTypeView.combineToTurnOverColumnSuperMasterLevel();

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
            winLossByGameTypeView.splitColumns();

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
            setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
            winLossByGameTypeView.cacheDOMElements();
            spyOn(linkBuilderComponent, "updateParam");

            // act
            winLossByGameTypeView.replaceDrilldownParam("IsFilterVisible", true);

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
                setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
                winLossByGameTypeView.cacheDOMElements();

                spyOn(winLossByGameTypeView, "setCssBySwitchMode");
                spyOn(winLossByGameTypeView, "replaceDrilldownParam");
                spyOn(sortComponent, "bindTableSorter");

                // act
                winLossByGameTypeView.onSwitchModeEvent();

                // assert
                expect(winLossByGameTypeView.setCssBySwitchMode).toHaveBeenCalledWith(false);
                expect(winLossByGameTypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsStackMode", true);
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });

        it(`From Second Switch Mode->
            Call setCssBySwitchMode Method,
            Call bindTableSorter Method,
            Call replaceDrilldownParam Method`, () => {
                // arrange
                setFixtures(winlossbygametypeTemplate.reportWithNormalMode);
                winLossByGameTypeView.cacheDOMElements();
                spyOn(sortComponent, "bindTableSorter");
                spyOn(winLossByGameTypeView, "setCssBySwitchMode");
                spyOn(winLossByGameTypeView, "replaceDrilldownParam");

                // act
                winLossByGameTypeView.onSwitchModeEvent();

                // assert
                expect(winLossByGameTypeView.setCssBySwitchMode).toHaveBeenCalled();
                expect(winLossByGameTypeView.replaceDrilldownParam).toHaveBeenCalledWith("IsStackMode", true);
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });
    });
});