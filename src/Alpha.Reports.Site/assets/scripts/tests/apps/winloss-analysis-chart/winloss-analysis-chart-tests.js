describe("WinLossAnalysisChart", function () {
    var chart = {};
    var jQueryFake = $;
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var reportTooltip = new ReportTooltip($);
    var monthRangePickerComponent = new MonthRangePickerComponent($);
    var dualAxisChartComponent = new DualAxisChartComponent($, chart);
    var winLossAnalysisChartView;
    var template = {
        winLossAnalysisChart: "<body>\n                                    <div class=\"page-title\"></div>\n                                    <form id=\"report-form\" role=\"form\">\n                                        <div id=\"msg-error\"></div>\n                                        <div>\n                                            <select id=\"dropdown-product\">\n                                                <option value=\"1\" selected=\"selected\"></option>\n                                            </select>\n                                            <button class=\"multiselect btn\"></button>\n                                        </div>\n                                        <div id=\"from-monthpicker\"\n                                            data-longmonths-inyear=\"['Jan','Feb','Mar','Apr']\"></div>\n                                        <div id=\"to-monthpicker\"\n                                            data-longmonths-inyear=\"['Jan','Feb','Mar','Apr']\"></div>\n                                        <div id=\"from-date\"></div>\n                                        <div id=\"to-date\"></div>\n                                        <div class=\"icon-filter\"></div>\n                                    </form>\n                                    <input id=\"IsFilterVisible\" value=\"True\" type=\"hidden\" />\n                                    <div class=\"chart-container\">\n                                        <canvas id=\"winloss-chart\" data-title=\"ReportTitle\">\n                                    </div>\n                                </body>",
        hiddenFilter: "<body>\n                                    <div class=\"page-title\"></div>\n                                    <form id=\"report-form\" role=\"form\">\n                                        <div id=\"msg-error\"></div>\n                                        <div>\n                                            <select id=\"dropdown-product\">\n                                                <option value=\"1\" selected=\"selected\"></option>\n                                            </select>\n                                            <button class=\"multiselect btn\"></button>\n                                        </div>\n                                        <div id=\"from-monthpicker\"\n                                            data-longmonths-inyear=\"['Jan','Feb','Mar','Apr']\"></div>\n                                        <div id=\"to-monthpicker\"\n                                            data-longmonths-inyear=\"['Jan','Feb','Mar','Apr']\"></div>\n                                        <div id=\"from-date\"></div>\n                                        <div id=\"to-date\"></div>\n                                        <div class=\"icon-filter\"></div>\n                                    </form>\n                                    <input id=\"IsFilterVisible\" value=\"False\" type=\"hidden\" />\n                                    <div class=\"chart-container\">\n                                        <canvas id=\"winloss-chart\" data-title=\"ReportTitle\">\n                                    </div>\n                                </body>"
    };
    var cacheDOMElements, registerEvents, blockUI, unblockUI, renderProductMultiSelectionComponent, renderLinkBuilderComponent;
    beforeEach(function () {
        winLossAnalysisChartView = new WinLossAnalysisChartView(jQueryFake, monthRangePickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent, dualAxisChartComponent);
        spyOn(winLossAnalysisChartView, "loadLegend");
        cacheDOMElements = spyOn(winLossAnalysisChartView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();
        registerEvents = spyOn(winLossAnalysisChartView, "registerEvents");
        registerEvents.and.callThrough();
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderLinkBuilderComponent = spyOn(linkBuilderComponent, "render");
        spyOn(dualAxisChartComponent, "render");
        spyOn(monthRangePickerComponent, "render");
        setFixtures(template.winLossAnalysisChart);
    });
    it("Inherit BaseReport_Always_WinLossAnalysisChartView is BaseReport", function () {
        // assert
        expect(winLossAnalysisChartView instanceof BaseReport).toBeTruthy();
    });
    describe("_init", function () {
        it("_Calls Specific Methods", function () {
            // act
            winLossAnalysisChartView.init();
            // assert
            expect(winLossAnalysisChartView.cacheDOMElements).toHaveBeenCalled();
            expect(winLossAnalysisChartView.registerEvents).toHaveBeenCalled();
            expect(monthRangePickerComponent.render).toHaveBeenCalled();
            expect(productMultiSelectionComponent.render).toHaveBeenCalled();
            expect(dualAxisChartComponent.render).toHaveBeenCalled();
        });
    });
    describe("_cacheDOMElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
            // act
            winLossAnalysisChartView.cacheDOMElements();
            // assert
            expect(winLossAnalysisChartView.$pageTitle).toBeDefined();
            expect(winLossAnalysisChartView.$canvasChart).toBeDefined();
            expect(winLossAnalysisChartView.$isFilterVisible).toBeDefined();
            expect(winLossAnalysisChartView.$reportForm).toBeDefined();
            expect(winLossAnalysisChartView.$fromDate).toBeDefined();
            expect(winLossAnalysisChartView.$toDate).toBeDefined();
            expect(winLossAnalysisChartView.$msgError).toBeDefined();
        });
    });
    describe("_getParams", function () {
        var getParamsTest = function (selectedProduct, expectedIsAnyProductSelected) {
            // arrange
            winLossAnalysisChartView.cacheDOMElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return selectedProduct; });
            // act
            var actualParam = winLossAnalysisChartView.getParams();
            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: expectedIsAnyProductSelected,
                IsLeast3MonthToChart: true,
                IsFilterVisible: true
            });
            it("_No product selected -> returns correct params", function () {
                getParamsTest([], false);
            });
            it("_Have product selected -> Returns correct params", function () {
                getParamsTest([1], true);
            });
        };
    });
    describe("_showValidationProductError", function () {
        it("_Always -> Adds class error to multiple select button of dropdown product", function () {
            // arrange
            productMultiSelectionComponent.$excludeProducts = [];
            winLossAnalysisChartView.init();
            // act
            winLossAnalysisChartView.showValidationProductError();
            // assert
            expect(winLossAnalysisChartView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("_showValidationMonthRangeError", function () {
        it("_Always -> Removes class hide to msg-error", function () {
            // arrange
            winLossAnalysisChartView.init();
            // act
            winLossAnalysisChartView.showValidationMonthRangeError();
            // assert
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeFalsy();
        });
    });
    describe("_hideValidationMonthRangeError", function () {
        it("_Always -> Adds class hide to msg-error", function () {
            // arrange
            winLossAnalysisChartView.init();
            // act
            winLossAnalysisChartView.hideValidationMonthRangeError();
            // assert
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeTruthy();
        });
    });
    describe("_onSubmitReportEvent", function () {
        it("_Not select anyProduct -> Blocks UI; Shows Validate Product Error; Unblock UI;", function () {
            //  arrange
            renderProductMultiSelectionComponent.and.callThrough();
            winLossAnalysisChartView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
            spyOn(monthRangePickerComponent, "isLeast3Month");
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            winLossAnalysisChartView.onSubmitReportEvent(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(winLossAnalysisChartView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Is Not Least 3 Months -> Blocks UI; Show Validation Month Range Error ;Unblock UI;", function () {
            //  arrange
            renderProductMultiSelectionComponent.and.callThrough();
            winLossAnalysisChartView.init();
            spyOn(monthRangePickerComponent, "isLeast3Month");
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            winLossAnalysisChartView.onSubmitReportEvent(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeFalsy();
        });
        it("_Slect products and is least 3 months -> Block UI; Sets params to hidden fields", function () {
            // arrange
            renderProductMultiSelectionComponent.and.callThrough();
            spyOn(monthRangePickerComponent, "isLeast3Month").and.returnValue(true);
            winLossAnalysisChartView.init();
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            winLossAnalysisChartView.onSubmitReportEvent(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(winLossAnalysisChartView.$multiSelectButton.hasClass("error")).toBeFalsy();
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeTruthy();
            expect(winLossAnalysisChartView.$isFilterVisible.val()).toBeTruthy();
        });
    });
});
//# sourceMappingURL=winloss-analysis-chart-tests.js.map