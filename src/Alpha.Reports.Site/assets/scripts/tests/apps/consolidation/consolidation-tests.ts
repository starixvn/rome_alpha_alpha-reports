describe("Consolidation", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const currencySelectionComponent = new CurrencySelectionComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    let consolidationView: ConsolidationView;

    let renderProductMultiSelectionComponent: jasmine.Spy,
        renderCurrencySelectionComponent: jasmine.Spy;

    beforeEach(() => {
        consolidationView = new ConsolidationView(
            jQueryFake,
            productMultiSelectionComponent,
            currencySelectionComponent,
            dateRangePickerComponent,
            reportTooltip,
            linkBuilderComponent);

        let cacheElements = spyOn(consolidationView, "cacheElements");
        cacheElements.and.callThrough();

        let registerEvents = spyOn(consolidationView, "registerEvents");
        registerEvents.and.callThrough();

        spyOn(consolidationView, "initHelp");
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderCurrencySelectionComponent = spyOn(currencySelectionComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });
    describe("_init", () => {
        describe("_Document is not ready", () => {
            beforeEach(() => {
                // act
                consolidationView.init();
            });
            it("_Does not call cacheElements", () => {
                // assert
                expect(consolidationView.cacheElements).not.toHaveBeenCalled();
            });
            it("_Does not call registerEvents", () => {
                // assert
                expect(consolidationView.registerEvents).not.toHaveBeenCalled();
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
            it("_Does not call currencySelectionComponent.render", () => {
                // assert
                expect(currencySelectionComponent.render).not.toHaveBeenCalled();
            });
            it("_Does not call dateRangePickerComponent.render", () => {
                // assert
                expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
            });
        });
        describe("_Document is ready", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationTemplate.normalModeReport);

                // act
                consolidationView.init();
            });

            it("_Calls cacheElements", () => {
                // assert
                expect(consolidationView.cacheElements).toHaveBeenCalled();
            });
            it("_Calls registerEvents", () => {
                // assert
                expect(consolidationView.registerEvents).toHaveBeenCalled();
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
            it("_Calls currencySelectionComponent.render", () => {
                // assert
                expect(currencySelectionComponent.render).toHaveBeenCalled();
            });
            it("_Calls dateRangePickerComponent.render", () => {
                // assert
                expect(dateRangePickerComponent.render).toHaveBeenCalled();
            });
        });
        it("_Report Of Top Level With no-Submit Param on Mobile -> Set Stack Mode as Default", () => {
            // arrange
            setFixtures(consolidationTemplate.reportOfTopLevel);
            let setStackMode = spyOn(consolidationView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            // act
            consolidationView.init();

            // assert
            expect(setStackMode).toHaveBeenCalled();
        });
        it("_Report Of Top Level With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", () => {
            // arrange
            setFixtures(consolidationTemplate.reportOfTopLevel);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);
            spyOn(reportTooltip, "show");

            // act
            consolidationView.init();

            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });
    });
    describe("_checkReady", () => {
        it("_Document is not loaded yet (empty) -> Returns False", () => {
            // arrange
            setFixtures("");

            // act
            var actualIsReady = consolidationView.checkReady();

            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("_Document is loaded (ready) -> Returns True", () => {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);

            // act
            var actualIsReady = consolidationView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
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
    describe("_setStackMode_Always", () => {
        let combineConsecutiveCellsSpy: jasmine.Spy;
        beforeEach(() => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
            consolidationView.cacheElements();
            spyOn(consolidationView, "replaceHrefParam");
            combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            consolidationView.setStackMode();
        });

        it("_Set stack-mode to table reports", () => {
            // assert
            expect(consolidationView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(consolidationView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
        });
        it("_Combines Columns", () => {
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
                [jasmine.any(Object), 4, "col-member-owner-turnover-combined"]]);
        });
    });
    describe("_onClickIconFilter", () => {
        describe("_Filter is currently shown", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationTemplate.shownFilterReport);
                consolidationView.cacheElements();
                spyOn(consolidationView, "replaceHrefParam");

                // act
                consolidationView.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(consolidationView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(consolidationView.$reportForm.hasClass("hide")).toBeTruthy();
            });
            it("_Update Href Param to False", () => {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
            });
        });
        describe("_Filter is currently hidden", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationTemplate.hiddenFilterReport);
                consolidationView.cacheElements();

                spyOn(consolidationView, "replaceHrefParam");

                // act
                consolidationView.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(consolidationView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(consolidationView.$reportForm.hasClass("hide")).toBeFalsy();
            });
            it("_Update Href Param to True", () => {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
            });
        });
    });
    describe("_onSwitchMode", () => {
        describe("_1st Switch on Normal Mode", () => {
            let combineConsecutiveCellsSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(consolidationTemplate.normalModeReport);
                consolidationView.cacheElements();
                spyOn(consolidationView, "replaceHrefParam");
                combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

                // act
                consolidationView.onSwitchMode();
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
                expect(consolidationView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Adds Class stack-mode to Page Title", () => {
                // assert
                expect(consolidationView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Updates HRef Param", () => {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);
            });
            it("_Combines Cells", () => {
                // assert
                expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                    [jasmine.any(Object), 3, "col-member-winloss-combined"],
                    [jasmine.any(Object), 4, "col-member-owner-turnover-combined"]]);
            });
        });
        describe("_1st Switch on Stack Mode", () => {
            let splitCellSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(consolidationTemplate.stackModeReport);
                consolidationView.cacheElements();
                spyOn(consolidationView, "replaceHrefParam");
                splitCellSpy = spyOn(alphaReports, "splitCell");

                // act
                consolidationView.onSwitchMode();
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
                expect(consolidationView.$tableReport.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Removes Class stack-mode to Page Title", () => {
                // assert
                expect(consolidationView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Updates HRef Param", () => {
                // assert
                expect(consolidationView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", false);
            });
            it("_Splits Cells", () => {
                // assert
                expect(splitCellSpy.calls.allArgs()).toEqual([
                    ["col-member-winloss"],
                    ["col-owner-turnover"]]);
            });
        });
        describe("_2nd+ Switch on Normal Mode", () => {
            beforeEach(() => {
                // arrange
                setFixtures(consolidationTemplate.normalModeReport);
                consolidationView.cacheElements();
                consolidationView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                consolidationView.onSwitchMode();
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
                setFixtures(consolidationTemplate.stackModeReport);
                consolidationView.cacheElements();
                consolidationView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                consolidationView.onSwitchMode();
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
    describe("_onSubmitReport", () => {
        it("_Not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            renderCurrencySelectionComponent.and.callThrough();
            consolidationView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            consolidationView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Valid Param; Screen smaller than 768 -> Blocks UI; Set Params to hidden field;", () => {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            renderCurrencySelectionComponent.and.callThrough();
            consolidationView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1]);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            consolidationView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(consolidationView.$isFilterVisible.val()).toBe("false");
            expect(consolidationView.$isStackMode.val()).toBe("false");
        });
        it("_Valid Param; Screen not smaller than 768 -> Blocks UI; Set Params to hidden field;", () => {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            renderCurrencySelectionComponent.and.callThrough();
            consolidationView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1]);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => false);

            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            consolidationView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(consolidationView.$isFilterVisible.val()).toBe("true");
            expect(consolidationView.$isStackMode.val()).toBe("false");
        });
    });
    describe("_onExportExcel_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            consolidationView.cacheElements();
            spyOn(consolidationView.$exportExcelForm, "submit");

            // act
            consolidationView.onExportExcel();
        });
        it("_Submits Export Excel Form", () => {
            // assert
            expect(consolidationView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    it("_replaceHrefParam_always_Call linkBuilderComponent update param", () => {
        // arrange
        let paramName = "paramName";
        let paramValue = "paramValue";
        spyOn(linkBuilderComponent, "updateParam");

        // act
        consolidationView.replaceHrefParam(paramName, paramValue);

        // assert
        expect(linkBuilderComponent.updateParam).toHaveBeenCalledWith("paramName", "paramValue");
    });
    describe("_getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            setFixtures(consolidationTemplate.normalModeReport);
            consolidationView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => selectedProduct);

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
            setFixtures(consolidationTemplate.normalModeReport);
            renderProductMultiSelectionComponent.and.callThrough();
            consolidationView.init();

            // act
            consolidationView.showValidateProductError();

            // assert
            expect(consolidationView.$productButton.hasClass("error")).toBeTruthy();
        });
    });
    describe("_onChangeReportType", () => {
        it("_Always -> Submit Report", () => {
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