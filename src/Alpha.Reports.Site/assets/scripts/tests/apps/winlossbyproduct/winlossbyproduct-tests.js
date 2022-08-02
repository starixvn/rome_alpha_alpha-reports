describe("WinLossByProduct_", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var winLossByProductView;
    var linkBuilderComponent = new LinkBuilderComponent($);
    var cacheDOMElements, registerEvents, blockUI, unblockUI, renderProductMultiSelectionComponent, renderPagerComponent, renderSortComponent, renderLinkBuilderComponent;
    beforeEach(function () {
        winLossByProductView = new WinLossByProductView(jQueryFake, dateRangePickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent);
        spyOn(winLossByProductView, "loadLegend");
        cacheDOMElements = spyOn(winLossByProductView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();
        registerEvents = spyOn(winLossByProductView, "registerEvents");
        registerEvents.and.callThrough();
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        spyOn(dateRangePickerComponent, "render");
    });
    describe("init", function () {
        it("Document is not ready -> Does not call any Method", function () {
            // act
            winLossByProductView.init();
            // assert
            expect(winLossByProductView.cacheDOMElements).not.toHaveBeenCalled();
            expect(winLossByProductView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
        });
        it("Document is ready -> Calls Specific Methods", function () {
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
        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            // act
            winLossByProductView.init();
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
    });
    describe("registerEvents", function () {
        beforeEach(function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            spyOn(winLossByProductView, "onClickProduct");
            spyOn(winLossByProductView, "onClickSport");
            winLossByProductView.cacheDOMElements();
            // act
            winLossByProductView.registerEvents();
        });
        it("_Always -> Binds Click event to product rows", function () {
            // act
            $(".product-row[data-product-id='1']").click();
            // assert
            expect(winLossByProductView.onClickProduct).toHaveBeenCalled();
        });
        it("_Always -> Binds Click event to sport rows", function () {
            // act
            $(".sport-row[data-sport-id='1']").click();
            // assert
            expect(winLossByProductView.onClickSport).toHaveBeenCalled();
        });
    });
    describe("cacheDOMElements", function () {
        it("Always -> All neccessary elements have been cached", function () {
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
    describe("onFilterEvent", function () {
        it("Filter is currently shown -> Hides Filter and Update Href Param to False", function () {
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
        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", function () {
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
    describe("onExportExcelEvent", function () {
        it("Always -> Submits Export Excel Form And Unblocks UI", function () {
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
    describe("hideFilter", function () {
        it("Always -> Add Hide class and hide filter", function () {
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
    describe("showFilter", function () {
        it("Always -> Remove Hide class and showfilter", function () {
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
    describe("getParams", function () {
        var getParamsTest = function (selectedProduct, expectedIsAnyProductSelected) {
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
    describe("onSubmitReportEvent", function () {
        it("Switch View -> Blocks UI; Empty FromDate/Todate; Sets params to hidden fields", function () {
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
        it("Switch View on Mobile -> Set Filter Is Visible to false", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.init();
            winLossByProductView.isSwitchView = true;
            // act
            winLossByProductView.onSubmitReportEvent(null);
            // assert
            expect(winLossByProductView.$isFilterVisible.val()).toEqual("true");
        });
        it("Not switch view and not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.init();
            winLossByProductView.isSwitchView = false;
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            winLossByProductView.onSubmitReportEvent(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("renderAtFirstSwitchMode", function () {
        it("Stack Mode Switch to Normal Mode -> Call Split Columns Method", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();
            spyOn(winLossByProductView, "splitColumns");
            // act
            winLossByProductView.renderAtFirstSwitchMode(true);
            // assert
            expect(winLossByProductView.splitColumns).toHaveBeenCalled();
        });
        it("Normal Mode Switch to Stack Mode -> Call Split combineColumns Method", function () {
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
    describe("setCssBySwitchMode", function () {
        it("Stack Mode Switch to Normal Mode -> Remove stack-mode class", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();
            // act
            winLossByProductView.setCssBySwitchMode(true);
            // assert
            expect(winLossByProductView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            expect(winLossByProductView.$tableReport.hasClass("stack-mode")).toBeFalsy();
        });
        it("Normal Mode Switch to Stack Mode -> Add stack-mode class", function () {
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
    describe("combineColumns", function () {
        it("Stack Mode At Agent Level -> Call combineToTurnOverColumnAgentLevel Method", function () {
            // arrange
            spyOn(winLossByProductView, "combineToTurnOverColumnAgentLevel");
            spyOn(winLossByProductView, "combineToWinLossColumn");
            // act
            winLossByProductView.combineColumns(true);
            // assert
            expect(winLossByProductView.combineToTurnOverColumnAgentLevel).toHaveBeenCalled();
            expect(winLossByProductView.combineToWinLossColumn).toHaveBeenCalled();
        });
        it("Stack Mode At Super Master Level -> Call combineToTurnOverColumnSuperMasterLevel AndcombineToWinLossColumn Method", function () {
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
    describe("combineToWinLossColumn", function () {
        it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", function () {
            // arrange
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
            // act
            winLossByProductView.combineToWinLossColumn('agent', 1);
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "text-right col-agent-winloss-combined altercol"]
            ]);
        });
    });
    describe("combineToTurnOverColumnAgentLevel", function () {
        it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", function () {
            // arrange
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
            // act
            winLossByProductView.combineToTurnOverColumnAgentLevel();
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 2, "text-right col-turnover-combined"]
            ]);
        });
    });
    describe("combineToTurnOverColumnSuperMasterLevel", function () {
        it("Switch Normal Mode to Normal Mode -> Call combineConsecutiveCells Method", function () {
            // arrange
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
            // act
            winLossByProductView.combineToTurnOverColumnSuperMasterLevel();
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "text-right col-turnover-combined"]
            ]);
        });
    });
    describe("splitColumns", function () {
        it("Switch Normal Mode to Normal Mode -> Call splitCell Method", function () {
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
                ["col-super-winloss", " altercol"]
            ]);
        });
    });
    describe("onSwitchModeEvent", function () {
        it("First Switch Mode->\n            Call setCssBySwitchMode Method,\n            Call renderAtFirstSwitchMode Method", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.cacheDOMElements();
            spyOn(winLossByProductView, "setCssBySwitchMode");
            // act
            winLossByProductView.onSwitchModeEvent();
            // assert
            expect(winLossByProductView.setCssBySwitchMode).toHaveBeenCalledWith(false);
        });
        it("From Second Switch Mode->\n            Call setCssBySwitchMode Method,\n            Call bindTableSorter Method", function () {
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
    describe("onClickProduct", function () {
        it("_Product group is shown -> Hides Sport Rows/Bet Type Rows", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.clickProduct);
            var productRow = $(".product-row[data-product-id='1']");
            // act
            winLossByProductView.onClickProduct(productRow);
            // assert
            expect($(".sport-of-1").hasClass("hide")).toBeTruthy();
            expect($(".sport-of-1").data("is-hide")).toEqual("true");
            expect($(".bet-type-of-product-1").hasClass("hide")).toBeTruthy();
            expect(productRow.find(".col-username").hasClass("active")).toBeFalsy();
            expect(productRow.data("is-hide")).toEqual(true);
        });
        it("_Product group is hidden/Sport is Hidden -> Shows Sport Rows/Bet Type Rows", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.clickProduct);
            var productRow = $(".product-row[data-product-id='3']");
            // act
            winLossByProductView.onClickProduct(productRow);
            // assert
            expect($(".sport-of-3").hasClass("hide")).toBeFalsy();
            expect($(".sport-of-3").data("is-hide")).toEqual("false");
            expect($(".bet-type-of-product-3").hasClass("hide")).toBeFalsy();
            expect(productRow.find(".col-username").hasClass("active")).toBeTruthy();
            expect(productRow.data("is-hide")).toEqual(false);
        });
        it("_Product group is hidden/Sport is Shown -> Shows Sport Rows/Bet Type Rows", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.clickProduct);
            var productRow = $(".product-row[data-product-id='4']");
            // act
            winLossByProductView.onClickProduct(productRow);
            // assert
            expect($(".sport-of-4").hasClass("hide")).toBeFalsy();
            expect($(".sport-of-4").data("is-hide")).toEqual("false");
            expect($(".bet-type-of-product-4").hasClass("hide")).toBeFalsy();
            expect(productRow.find(".col-username").hasClass("active")).toBeTruthy();
            expect(productRow.data("is-hide")).toEqual(false);
        });
    });
    describe("onClickSport", function () {
        it("_Sport group is shown -> Hides Bet Type Rows", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.clickProduct);
            var sportRow = $(".sport-row[data-sport-id='1'][data-product-id='1']");
            // act
            winLossByProductView.onClickSport(sportRow);
            // assert
            expect($(".bet-type-of-product-1.bet-type-of-sport-1").hasClass("hide")).toBeTruthy();
            expect(sportRow.find(".col-username").hasClass("active")).toBeFalsy();
            expect(sportRow.data("is-hide")).toEqual(true);
        });
        it("_Sport group is hidden -> Shows Bet Type Rows", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.clickProduct);
            var sportRow = $(".sport-row[data-sport-id='1'][data-product-id='3']");
            // act
            winLossByProductView.onClickSport(sportRow);
            // assert
            expect($(".bet-type-of-product-3.bet-type-of-sport-1").hasClass("hide")).toBeFalsy();
            expect(sportRow.find(".col-username").hasClass("active")).toBeTruthy();
            expect(sportRow.data("is-hide")).toEqual(false);
        });
    });
    describe("showValidateProductError", function () {
        it("$multiSelectButton -> has class error", function () {
            // arrange
            setFixtures(winlossbyproductTemplate.reportWithNormalMode);
            winLossByProductView.$multiSelectButton = $(".multiselect.btn");
            // act
            winLossByProductView.showValidateProductError();
            // assert
            expect(winLossByProductView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_addClassToExcludeProductLabels have been called with error parameter", function () {
            // arrange
            winLossByProductView.$multiSelectButton = $(".multiselect.btn");
            spyOn(winLossByProductView.productMultiSelectionComponent, "addClassToExcludeProductLabels");
            // act
            winLossByProductView.showValidateProductError();
            // assert
            expect(winLossByProductView.productMultiSelectionComponent.addClassToExcludeProductLabels).toHaveBeenCalledWith("error");
        });
    });
});
//# sourceMappingURL=winlossbyproduct-tests.js.map