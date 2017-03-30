describe("WinLossByProduct_", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    let winLossByProductView: WinLossByProductView;
    const linkBuilderComponent = new LinkBuilderComponent($);

    let cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        winLossByProductView = new WinLossByProductView(
            jQueryFake,
            dateRangePickerComponent,
            productMultiSelectionComponent,
            reportTooltip,
            linkBuilderComponent);
        spyOn(winLossByProductView, "loadLegend");

        cacheDOMElements = spyOn(winLossByProductView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();

        registerEvents = spyOn(winLossByProductView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        spyOn(dateRangePickerComponent, "render");
    });

    describe("init", () => {
        it("Document is not ready -> Does not call any Method", () => {
            // act
            winLossByProductView.init();

            // assert
            expect(winLossByProductView.cacheDOMElements).not.toHaveBeenCalled();
            expect(winLossByProductView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
        });

        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);

            // act
            winLossByProductView.init();

            // assert
            expect(winLossByProductView.cacheDOMElements).toHaveBeenCalled();
            expect(winLossByProductView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });

        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);

            // act
            winLossByProductView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
    });

    describe("cacheDOMElements", () => {
        it("Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);

            // act
            winLossByProductView.cacheDOMElements();

            // assert
            expect(winLossByProductView.$pageTitle).toBeDefined();
            expect(winLossByProductView.$tableReport).toBeDefined();
            expect(winLossByProductView.$reportForm).toBeDefined();
            expect(winLossByProductView.$isFilterVisible).toBeDefined();
            expect(winLossByProductView.$isStackMode).toBeDefined();
            expect(winLossByProductView.$isAgentLevel).toBeDefined();
            expect(winLossByProductView.$reportForm).toBeDefined();
            expect(winLossByProductView.$userSelectedDateRangeOption).toBeDefined();
            expect(winLossByProductView.$filterIcon).toBeDefined();
            expect(winLossByProductView.$excelIcon).toBeDefined();
            expect(winLossByProductView.$switchModeIcon).toBeDefined();
            expect(winLossByProductView.$fromDate).toBeDefined();
            expect(winLossByProductView.$toDate).toBeDefined();
        });
    });

    describe("onFilterEvent", () => {
        it("Filter is currently shown -> Hides Filter and Update Href Param to False", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();

            spyOn(winLossByProductView, "hideFilter");
            spyOn(winLossByProductView, "showFilter");

            // act
            winLossByProductView.onFilterEvent();

            // assert
            expect(winLossByProductView.hideFilter).toHaveBeenCalled();
        });

        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();

            spyOn(winLossByProductView, "hideFilter");
            spyOn(winLossByProductView, "showFilter");

            // act
            winLossByProductView.onFilterEvent();

            // assert
            expect(winLossByProductView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
        });
    });

    describe("onExportExcelEvent", () => {
        it("Always -> Submits Export Excel Form And Unblocks UI", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();
            spyOn(winLossByProductView.$exportExcelForm, "submit");

            // act
            winLossByProductView.onExportExcelEvent();

            // assert
            expect(winLossByProductView.$exportExcelForm.submit).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("hideFilter", () => {
        it("Always -> Add Hide class and hide filter", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();

            // act
            winLossByProductView.hideFilter();

            // assert
            expect(winLossByProductView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(winLossByProductView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });

    describe("showFilter", () => {
        it("Always -> Remove Hide class and showfilter", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();

            // act
            winLossByProductView.showFilter();

            // assert
            expect(winLossByProductView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossByProductView.$reportForm.hasClass("hide")).toBeFalsy();
        });
    });

    describe("getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();

            // act
            var actualParam = winLossByProductView.getParams();

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
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.init();
            winLossByProductView.isSwitchView = true;

            // act
            winLossByProductView.onSubmitReportEvent(null);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();

            expect(winLossByProductView.$isFilterVisible.val()).toBeTruthy();
        });

        it("Switch View on Mobile -> Set Filter Is Visible to false", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.init();
            winLossByProductView.isSwitchView = true;

            // act
            winLossByProductView.onSubmitReportEvent(null);

            // assert
            expect(winLossByProductView.$isFilterVisible.val()).toEqual("true");
        });

        it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.init();
            winLossByProductView.isSwitchView = false;

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossByProductView.onSubmitReportEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });

    describe("renderAtFirstSwitchMode", () => {
        it("Stack Mode Switch to Normal Mode -> Call Split Columns Method", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();
            spyOn(winLossByProductView, "splitColumns");

            // act
            winLossByProductView.renderAtFirstSwitchMode(true);

            // assert
            expect(winLossByProductView.splitColumns).toHaveBeenCalled();
        });

        it("Normal Mode Switch to Stack Mode -> Call Split combineColumns Method", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();
            spyOn(winLossByProductView, "combineColumns");

            // act
            winLossByProductView.renderAtFirstSwitchMode(false);

            // assert
            expect(winLossByProductView.combineColumns).toHaveBeenCalled();
        });
    });

    describe("setCssBySwitchMode", () => {
        it("Stack Mode Switch to Normal Mode -> Remove stack-mode class", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();

            // act
            winLossByProductView.setCssBySwitchMode(true);

            // assert
            expect(winLossByProductView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            expect(winLossByProductView.$tableReport.hasClass("stack-mode")).toBeFalsy();
        });

        it("Normal Mode Switch to Stack Mode -> Add stack-mode class", () => {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();

            // act
            winLossByProductView.setCssBySwitchMode(false);

            // assert
            expect(winLossByProductView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossByProductView.$tableReport.hasClass("stack-mode")).toBeTruthy();
        });
    });

    describe("combineColumns", () => {
        it("Stack Mode At Agent Level -> Call combineToTurnOverColumnAgentLevel Method", () => {
            // arrange
            spyOn(winLossByProductView, "combineToTurnOverColumnAgentLevel");
            spyOn(winLossByProductView, "combineToWinLossColumn");

            // act
            winLossByProductView.combineColumns(true);

            // assert
            expect(winLossByProductView.combineToTurnOverColumnAgentLevel).toHaveBeenCalled();
            expect(winLossByProductView.combineToWinLossColumn).toHaveBeenCalled();
        });

        it("Stack Mode At Super Master Level -> Call combineToTurnOverColumnSuperMasterLevel AndcombineToWinLossColumn Method", () => {
            // arrange
            spyOn(winLossByProductView, "combineToTurnOverColumnSuperMasterLevel");
            spyOn(winLossByProductView, "combineToWinLossColumn");

            // act
            winLossByProductView.combineColumns(false);

            // assert
            expect(winLossByProductView.combineToTurnOverColumnSuperMasterLevel).toHaveBeenCalled();
            expect(winLossByProductView.combineToWinLossColumn).toHaveBeenCalled();
        });
    });

    describe("combineToWinLossColumn", () => {
        it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", () => {
            // arrange
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            winLossByProductView.combineToWinLossColumn('agent', 1);

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
            winLossByProductView.combineToTurnOverColumnAgentLevel();

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
            winLossByProductView.combineToTurnOverColumnSuperMasterLevel();

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
            winLossByProductView.splitColumns();

            // assert
            expect(splitCellSpy.calls.allArgs()).toEqual([
                ["col-turnover"],
                ["col-member-winloss", ""],
                ["col-agent-winloss", " altercol"],
                ["col-master-winloss", " altercol"],
                ["col-super-winloss", " altercol"]]);
        });
    });

    describe("onSwitchModeEvent   ", () => {
        it(`First Switch Mode->
            Call setCssBySwitchMode Method,
            Call renderAtFirstSwitchMode Method`, () => {
                // arrange
                setFixtures(winlossbyproductTemplate.reportWithNormalMode);
                winLossByProductView.cacheDOMElements();

                spyOn(winLossByProductView, "setCssBySwitchMode");

                // act
                winLossByProductView.onSwitchModeEvent();

                // assert
                expect(winLossByProductView.setCssBySwitchMode).toHaveBeenCalledWith(false);
            });

        it(`From Second Switch Mode->
            Call setCssBySwitchMode Method,
            Call bindTableSorter Method`, () => {
                // arrange
                setFixtures(winlossbyproductTemplate.reportWithNormalMode);
                winLossByProductView.cacheDOMElements();
                spyOn(winLossByProductView, "setCssBySwitchMode");

                // act
                winLossByProductView.onSwitchModeEvent();

                // assert
                expect(winLossByProductView.setCssBySwitchMode).toHaveBeenCalled();
            });
    });
});