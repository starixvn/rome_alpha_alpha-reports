describe("Consolidation", function () {
    var jQueryFake = $;
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var currencySelectionComponent = new CurrencySelectionComponent($);
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var consolidationView;
    var renderProductMultiSelectionComponent, renderCurrencySelectionComponent;
    beforeEach(function () {
        consolidationView = new ConsolidationView(jQueryFake, productMultiSelectionComponent, currencySelectionComponent, dateRangePickerComponent, reportTooltip, linkBuilderComponent);
        var cacheElements = spyOn(consolidationView, "cacheElements");
        cacheElements.and.callThrough();
        var registerEvents = spyOn(consolidationView, "registerEvents");
        registerEvents.and.callThrough();
        spyOn(consolidationView, "initHelp");
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderCurrencySelectionComponent = spyOn(currencySelectionComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });
    describe("_init", function () {
        describe("_Document is not ready", function () {
            beforeEach(function () {
                // act
                consolidationView.init();
            });
            it("_Does not call cacheElements", function () {
                // assert
                expect(consolidationView.cacheElements).not.toHaveBeenCalled();
            });
            it("_Does not call registerEvents", function () {
                // assert
                expect(consolidationView.registerEvents).not.toHaveBeenCalled();
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
            it("_Does not call currencySelectionComponent.render", function () {
                // assert
                expect(currencySelectionComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call dateRangePickerComponent.render", function () {
                // assert
                expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
            });
        });
        describe("_Document is ready", function () {
            beforeEach(function () {
                // arrange
                setFixtures(consolidationTemplate.normalModeReport);
                // act
                consolidationView.init();
            });
            it("_Calls cacheElements", function () {
                // assert
                expect(consolidationView.cacheElements).toHaveBeenCalled();
            });
            it("_Calls registerEvents", function () {
                // assert
                expect(consolidationView.registerEvents).toHaveBeenCalled();
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
            it("_Calls currencySelectionComponent.render", function () {
                // assert
                expect(currencySelectionComponent.render).toHaveBeenCalled();
            });
            it("_Calls dateRangePickerComponent.render", function () {
                // assert
                expect(dateRangePickerComponent.render).toHaveBeenCalled();
            });
        });
        it("_Report Of Top Level With no-Submit Param on Mobile -> Set Stack Mode as Default", function () {
            // arrange
            setFixtures(consolidationTemplate.reportOfTopLevel);
            var setStackMode = spyOn(consolidationView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return true; });
            // act
            consolidationView.init();
            // assert
            expect(setStackMode).toHaveBeenCalled();
        });
        it("_Report Of Top Level With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", function () {
            // arrange
            setFixtures(consolidationTemplate.reportOfTopLevel);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return true; });
            spyOn(reportTooltip, "show");
            // act
            consolidationView.init();
            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });
    });
    describe("_checkReady", function () {
        it("_Document is not loaded yet (empty) -> Returns False", function () {
            // arrange
            setFixtures("");
            // act
            var actualIsReady = consolidationView.checkReady();
            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("_Document is loaded (ready) -> Returns True", function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            // act
            var actualIsReady = consolidationView.checkReady();
            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            // act
            consolidationView.cacheElements();
            // assert
            expect(consolidationView.$tableReport).toBeDefined();
            expect(consolidationView.$isSetDefaultStackMode).toBeDefined();
            expect(consolidationView.$reportForm).toBeDefined();
            expect(consolidationView.$exportExcelForm).toBeDefined();
            expect(consolidationView.$pageTitle).toBeDefined();
            expect(consolidationView.$isFilterVisible).toBeDefined();
            expect(consolidationView.$isStackMode).toBeDefined();
        });
    });
    describe("_setStackMode_Always", function () {
        var combineConsecutiveCellsSpy;
        beforeEach(function () {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            consolidationView.cacheElements();
            spyOn(consolidationView, "replaceHrefParam");
            combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
            // act
            consolidationView.setStackMode();
        });
        it("_Set stack-mode to table reports", function () {
            // assert
            expect(consolidationView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(consolidationView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
        });
        it("_Combines Columns", function () {
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
                [jasmine.any(Object), 4, "col-member-owner-turnover-combined"]
            ]);
        });
    });
    describe("_onClickIconFilter", function () {
        describe("_Filter is currently shown", function () {
            beforeEach(function () {
                // arrange
                setFixtures(consolidationTemplate.shownFilterReport);
                consolidationView.cacheElements();
                spyOn(consolidationView, "replaceHrefParam");
                // act
                consolidationView.onClickIconFilter();
            });
            it("_Hides Filter", function () {
                // assert
                expect(consolidationView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(consolidationView.$reportForm.hasClass("hide")).toBeTruthy();
            });
            it("_Update Href Param to False", function () {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
            });
        });
        describe("_Filter is currently hidden", function () {
            beforeEach(function () {
                // arrange
                setFixtures(consolidationTemplate.hiddenFilterReport);
                consolidationView.cacheElements();
                spyOn(consolidationView, "replaceHrefParam");
                // act
                consolidationView.onClickIconFilter();
            });
            it("_Hides Filter", function () {
                // assert
                expect(consolidationView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(consolidationView.$reportForm.hasClass("hide")).toBeFalsy();
            });
            it("_Update Href Param to True", function () {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
            });
        });
    });
    describe("_onSwitchMode", function () {
        describe("_1st Switch on Normal Mode", function () {
            var combineConsecutiveCellsSpy;
            beforeEach(function () {
                // arrange
                setFixtures(consolidationTemplate.normalModeReport);
                consolidationView.cacheElements();
                spyOn(consolidationView, "replaceHrefParam");
                combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");
                // act
                consolidationView.onSwitchMode();
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
                expect(consolidationView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Adds Class stack-mode to Page Title", function () {
                // assert
                expect(consolidationView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Updates HRef Param", function () {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
            });
            it("_Combines Cells", function () {
                // assert
                expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                    [jasmine.any(Object), 3, "col-member-winloss-combined"],
                    [jasmine.any(Object), 4, "col-member-owner-turnover-combined"]
                ]);
            });
        });
        describe("_1st Switch on Stack Mode", function () {
            var splitCellSpy;
            beforeEach(function () {
                // arrange
                setFixtures(consolidationTemplate.stackModeReport);
                consolidationView.cacheElements();
                spyOn(consolidationView, "replaceHrefParam");
                splitCellSpy = spyOn(alphaReports, "splitCell");
                // act
                consolidationView.onSwitchMode();
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
                expect(consolidationView.$tableReport.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Removes Class stack-mode to Page Title", function () {
                // assert
                expect(consolidationView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Updates HRef Param", function () {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", false);
            });
            it("_Splits Cells", function () {
                // assert
                expect(splitCellSpy.calls.allArgs()).toEqual([
                    ["col-member-winloss"],
                    ["col-owner-turnover"]
                ]);
            });
        });
        describe("_2nd+ Switch on Normal Mode", function () {
            beforeEach(function () {
                // arrange
                setFixtures(consolidationTemplate.normalModeReport);
                consolidationView.cacheElements();
                consolidationView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");
                // act
                consolidationView.onSwitchMode();
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
                setFixtures(consolidationTemplate.stackModeReport);
                consolidationView.cacheElements();
                consolidationView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");
                // act
                consolidationView.onSwitchMode();
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
    describe("_onSubmitReport", function () {
        it("_Not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            renderCurrencySelectionComponent.and.callThrough();
            consolidationView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            consolidationView.onSubmitReport(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Valid Param; Screen smaller than 768 -> Blocks UI; Set Params to hidden field;", function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            renderCurrencySelectionComponent.and.callThrough();
            consolidationView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [1]; });
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return true; });
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            consolidationView.onSubmitReport(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(consolidationView.$isFilterVisible.val()).toBe("false");
            expect(consolidationView.$isStackMode.val()).toBe("false");
        });
        it("_Valid Param; Screen not smaller than 768 -> Blocks UI; Set Params to hidden field;", function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            renderCurrencySelectionComponent.and.callThrough();
            consolidationView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [1]; });
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return false; });
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            consolidationView.onSubmitReport(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(consolidationView.$isFilterVisible.val()).toBe("true");
            expect(consolidationView.$isStackMode.val()).toBe("false");
        });
    });
    describe("_onExportExcel_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            consolidationView.cacheElements();
            spyOn(consolidationView.$exportExcelForm, "submit");
            // act
            consolidationView.onExportExcel();
        });
        it("_Submits Export Excel Form", function () {
            // assert
            expect(consolidationView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    it("_replaceHrefParam_always_Call linkBuilderComponent update param", function () {
        // arrange
        var paramName = "paramName";
        var paramValue = "paramValue";
        spyOn(linkBuilderComponent, "updateParam");
        // act
        consolidationView.replaceHrefParam(paramName, paramValue);
        // assert
        expect(linkBuilderComponent.updateParam).toHaveBeenCalledWith("paramName", "paramValue");
    });
    describe("_getParams", function () {
        var getParamsTest = function (selectedProduct, expectedIsAnyProductSelected) {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            consolidationView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return selectedProduct; });
            // act
            var actualParam = consolidationView.getParams();
            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: expectedIsAnyProductSelected,
                IsAnyProductDetailSelected: true,
                IsStackMode: false,
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
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            consolidationView.init();
            // act
            consolidationView.showValidateProductError();
            // assert
            expect(consolidationView.$productButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("_onChangeReportType", function () {
        it("_Always -> Submit Report", function () {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            consolidationView.init();
            spyOn(consolidationView.$reportForm, "submit");
            // act
            consolidationView.onChangeReportType(consolidationView.$("[name=\"report-type\"]"));
            // assert
            expect(consolidationView.$reportForm.submit).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=consolidation-tests.js.map