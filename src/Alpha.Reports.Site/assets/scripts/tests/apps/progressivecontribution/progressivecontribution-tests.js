describe("ProgresiveContribution_", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var monthPickerComponent = new MonthPickerComponent($);
    var reportTooltip = new ReportTooltip($);
    var productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var progressiveContributionView, cacheDOMElements, registerEvents, bindFilter, blockUI, unblockUI, stickyTableHeaders, renderProductMultiSelectionComponent, renderLinkBuilderComponent;
    beforeEach(function () {
        progressiveContributionView = new ProgressiveContributionView(jQueryFake, dateRangePickerComponent, monthPickerComponent, productMultiSelectionComponent, reportTooltip, linkBuilderComponent);
        cacheDOMElements = spyOn(progressiveContributionView, "cacheDOMElements");
        cacheDOMElements.and.callThrough();
        registerEvents = spyOn(progressiveContributionView, "registerEvents");
        registerEvents.and.callThrough();
        bindFilter = spyOn(progressiveContributionView, "bindFilter");
        bindFilter.and.callThrough();
        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        stickyTableHeaders = spyOn($.fn, "stickyTableHeaders");
        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderProductMultiSelectionComponent.and.callThrough();
        spyOn(dateRangePickerComponent, "render");
        spyOn(monthPickerComponent, "render");
    });
    describe("cacheDOMElements", function () {
        it("Always -> All neccessary elements have been cached", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            // act
            progressiveContributionView.cacheDOMElements();
            // assert
            expect(progressiveContributionView.$pageTitle).toBeDefined();
            expect(progressiveContributionView.$tableReport).toBeDefined();
            expect(progressiveContributionView.$reportForm).toBeDefined();
            expect(progressiveContributionView.$isFilterVisible).toBeDefined();
            expect(progressiveContributionView.$isStackMode).toBeDefined();
            expect(progressiveContributionView.$reportForm).toBeDefined();
            expect(progressiveContributionView.$historicReport).toBeDefined();
            expect(progressiveContributionView.$excelIcon).toBeDefined();
            expect(progressiveContributionView.$switchModeIcon).toBeDefined();
            expect(progressiveContributionView.$fromDate).toBeDefined();
            expect(progressiveContributionView.$toDate).toBeDefined();
            expect(progressiveContributionView.$isHistoricReport).toBeDefined();
        });
    });
    describe("bindFilter", function () {
        beforeEach(function () {
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheDOMElements();
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return true; });
        });
        it("_Report With no-Submit Param on Mobile -> Set Stack Mode as Default", function () {
            // arrange
            var setStackMode = spyOn(progressiveContributionView, "setStackMode");
            // act
            progressiveContributionView.bindFilter();
            // assert
            expect(setStackMode).toHaveBeenCalled();
        });
        it("_Report With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", function () {
            // arrange
            spyOn(reportTooltip, "show");
            // act
            progressiveContributionView.bindFilter();
            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });
    });
    describe("hideFilter", function () {
        it("Always -> Add Hide class and hide filter", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheDOMElements();
            // act
            progressiveContributionView.hideFilter();
            // assert
            expect(progressiveContributionView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(progressiveContributionView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });
    describe("_onExportExcel_Always", function () {
        it("_Submits Export Excel Form", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheElements();
            spyOn(progressiveContributionView.$exportExcelForm, "submit");
            // act
            progressiveContributionView.onExportExcel();
            // assert
            expect(progressiveContributionView.$exportExcelForm.submit).toHaveBeenCalled();
        });
    });
    describe("_onSubmitHistoricReport", function () {
        it("Current Report -> Switch to Historic Report and Submit", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheDOMElements();
            spyOn(progressiveContributionView.$reportForm, "trigger");
            // act
            progressiveContributionView.onSubmitHistoricReport();
            // assert
            expect(progressiveContributionView.isHistoricReport).toBeTruthy();
            expect(progressiveContributionView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
        it("Historic Report -> Switch to Current Report and Submit", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.historicReportWithShownFilter);
            progressiveContributionView.cacheDOMElements();
            spyOn(progressiveContributionView.$reportForm, "trigger");
            // act
            progressiveContributionView.onSubmitHistoricReport();
            // assert
            expect(progressiveContributionView.isHistoricReport).toBeTruthy();
            expect(progressiveContributionView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
    });
    describe("_onSubmitReport", function () {
        it("_Not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            progressiveContributionView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            progressiveContributionView.onSubmitReport(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(progressiveContributionView.$buttonProduct.hasClass("error")).toBeTruthy();
        });
        it("Have selected any products -> Blocks UI; Unblock UI; Set correct Params;", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            progressiveContributionView.init();
            spyOn(progressiveContributionView.$isStackMode, "val");
            spyOn(progressiveContributionView.$isFilterVisible, "val");
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            progressiveContributionView.onSubmitReport(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(progressiveContributionView.$isStackMode.val).toHaveBeenCalledWith("false");
            expect(progressiveContributionView.$isFilterVisible.val).toHaveBeenCalledWith("true");
            expect(progressiveContributionView.$buttonProduct.hasClass("error")).toBeFalsy();
        });
    });
    describe("_getParams", function () {
        it("_No product selected -> Returns correct params", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
            // act
            var actualParam = progressiveContributionView.getParams();
            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: false,
                IsStackMode: false,
                IsHistoricReport: false,
                IsFilterVisible: true
            });
        });
        it("_Have product selected -> Returns correct params", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [1]; });
            // act
            var actualParam = progressiveContributionView.getParams();
            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: true,
                IsStackMode: false,
                IsHistoricReport: false,
                IsFilterVisible: true
            });
        });
    });
    describe("onSwitchMode   ", function () {
        it("First Switch Mode->\n            Call setCssBySwitchMode Method,\n            Call renderAtFirstSwitchMode Method", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheDOMElements();
            spyOn(progressiveContributionView, "setCssBySwitchMode");
            spyOn(progressiveContributionView, "renderAtFirstSwitchMode");
            // act
            progressiveContributionView.onSwitchMode();
            // assert
            expect(progressiveContributionView.setCssBySwitchMode).toHaveBeenCalledWith(false);
            expect(progressiveContributionView.renderAtFirstSwitchMode).toHaveBeenCalledWith(false);
        });
        it("From Second Switch Mode->\n            Call setCssBySwitchMode Method,\n            Call renderAtFirstSwitchMode Method", function () {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheDOMElements();
            spyOn(progressiveContributionView, "setCssBySwitchMode");
            spyOn(progressiveContributionView, "renderAtFirstSwitchMode");
            // act
            progressiveContributionView.onSwitchMode();
            // assert
            expect(progressiveContributionView.setCssBySwitchMode).toHaveBeenCalled();
            expect(progressiveContributionView.renderAtFirstSwitchMode).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=progressivecontribution-tests.js.map