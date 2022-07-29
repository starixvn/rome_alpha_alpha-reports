describe("EnterResult", function () {
    var jQueryFake = $;
    var sortComponent = new SortComponent($, new LinkBuilderComponent($));
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var enterResultView;
    var cacheDOMElements, registerEvents, blockUI, unblockUI, renderSortComponent;
    beforeEach(function () {
        enterResultView = new EnterResultView(jQueryFake, dateRangePickerComponent, sortComponent);
        cacheDOMElements = spyOn(enterResultView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();
        registerEvents = spyOn(enterResultView, "registerEvents");
        registerEvents.and.callThrough();
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        renderSortComponent = spyOn(sortComponent, "render");
        spyOn(dateRangePickerComponent, "render");
    });
    describe("_init", function () {
        it("_Document is not ready -> Does not call any Method", function () {
            // act
            enterResultView.init();
            // assert
            expect(enterResultView.cacheDOMElements).not.toHaveBeenCalled();
            expect(enterResultView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
            expect(sortComponent.render).not.toHaveBeenCalled();
        });
        it("_Document is ready -> Calls Specific Methods", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            // act
            enterResultView.init();
            // assert
            expect(enterResultView.cacheDOMElements).toHaveBeenCalled();
            expect(enterResultView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(sortComponent.render).toHaveBeenCalled();
        });
        it("_Current Report -> Renders Date Range Picker", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            // act
            enterResultView.init();
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Always -> Render sort component with correct options", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            // act
            enterResultView.init();
            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(enterResultView.$tableReport, false, true, jasmine.any(Function), { 0: { sorter: "text" } }));
        });
    });
    describe("checkReady", function () {
        it("Document is not loaded yet (empty) -> Returns False", function () {
            // arrange
            setFixtures("");
            // act
            var actualIsReady = enterResultView.checkReady();
            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("_Document is loaded (ready) -> Returns True", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            // act
            var actualIsReady = enterResultView.checkReady();
            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("cacheDOMElements", function () {
        it("Always -> All neccessary elements have been cached", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            // act
            enterResultView.cacheDOMElements();
            // assert
            expect(enterResultView.$pageTitle).toBeDefined();
            expect(enterResultView.$tableReport).toBeDefined();
            expect(enterResultView.$reportForm).toBeDefined();
            expect(enterResultView.$isFilterVisible).toBeDefined();
            expect(enterResultView.$reportForm).toBeDefined();
            expect(enterResultView.$userSelectedDateRangeOption).toBeDefined();
            expect(enterResultView.$historicReport).toBeDefined();
            expect(enterResultView.$filterIcon).toBeDefined();
            expect(enterResultView.$excelIcon).toBeDefined();
            expect(enterResultView.$submitButton).toBeDefined();
            expect(enterResultView.$fromDate).toBeDefined();
            expect(enterResultView.$toDate).toBeDefined();
        });
    });
    describe("onFilterEvent", function () {
        it("Filter is currently shown -> Hides Filter and Update Href Param to False", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            enterResultView.cacheDOMElements();
            spyOn(enterResultView, "hideFilter");
            spyOn(enterResultView, "showFilter");
            // act
            enterResultView.onFilterEvent();
            // assert
            expect(enterResultView.hideFilter).toHaveBeenCalled();
        });
        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", function () {
            // arrange
            setFixtures(winLossTemplate.historicReportWithHiddenFilter);
            enterResultView.cacheDOMElements();
            spyOn(enterResultView, "hideFilter");
            spyOn(enterResultView, "showFilter");
            // act
            enterResultView.onFilterEvent();
            // assert
            expect(enterResultView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
        });
    });
    describe("hideFilter", function () {
        describe("_Always", function () {
            beforeEach(function () {
                // arrange
                setFixtures(enteredResultTemplate.reportContent);
                enterResultView.cacheDOMElements();
                // act
                enterResultView.hideFilter();
            });
            it("Add Hide class and hide filter", function () {
                // assert
                expect(enterResultView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            });
            it("Add Hide class and hide filter", function () {
                // assert
                expect(enterResultView.$reportForm.hasClass("hide")).toBeTruthy();
            });
        });
    });
    describe("showFilter", function () {
        it("Always -> Remove Hide class and showfilter", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            enterResultView.cacheDOMElements();
            // act
            enterResultView.showFilter();
            // assert
            expect(enterResultView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(enterResultView.$reportForm.hasClass("hide")).toBeFalsy();
        });
    });
    describe("onExportExcelEvent", function () {
        it("Always -> Submits Export Excel Form And Unblocks UI", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            enterResultView.cacheDOMElements();
            spyOn(enterResultView.$exportExcelForm, "submit");
            // act
            enterResultView.onExportExcelEvent();
            // assert
            expect(enterResultView.$exportExcelForm.submit).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("setHiddenValue", function () {
        it("Submit Normal Report -> Blocks UI; Sets params to hidden fields", function () {
            // arrange
            setFixtures(enteredResultTemplate.reportContent);
            enterResultView.init();
            // act
            enterResultView.setHiddenValue();
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(enterResultView.$isFilterVisible.val()).toBeTruthy();
            expect(enterResultView.$isHistoricReport.val()).toBeTruthy();
        });
    });
    describe("onSubmitNormalReportEvent", function () {
        it("Submit Normal Report -> Set Value For Hidden Field Before Submit", function () {
            // arrange
            spyOn(enterResultView, "setHiddenValue");
            // act
            enterResultView.onSubmitNormalReportEvent();
            // assert
            expect(enterResultView.setHiddenValue).toHaveBeenCalled();
        });
    });
    describe("onSubmitHistoricReportEvent", function () {
        it("Submit Historic Report -> Set Value For Hidden Field Before Submit", function () {
            // arrange
            spyOn(enterResultView, "setHiddenValue");
            enterResultView.cacheDOMElements();
            // act
            enterResultView.onSubmitHistoricReportEvent();
            // assert
            expect(enterResultView.setHiddenValue).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=enteredresult-tests.js.map