describe("ProgresiveContribution_", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);

    let progressiveContributionView: ProgressiveContributionView,
        cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        bindFilter: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        stickyTableHeaders: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        progressiveContributionView = new ProgressiveContributionView(
            jQueryFake,
            dateRangePickerComponent,
            productMultiSelectionComponent,
            reportTooltip,
            linkBuilderComponent);

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
    });

    describe("init", () => {
        it("Document is not ready -> Does not call any Method", () => {
            // act
            progressiveContributionView.init();

            // assert
            expect(progressiveContributionView.cacheDOMElements).not.toHaveBeenCalled();
            expect(progressiveContributionView.registerEvents).not.toHaveBeenCalled();
        });

        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);

            // act
            progressiveContributionView.init();

            // assert
            expect(progressiveContributionView.cacheDOMElements).toHaveBeenCalled();
            expect(progressiveContributionView.registerEvents).toHaveBeenCalled();
            expect(progressiveContributionView.bindFilter).toHaveBeenCalled();
            expect($.fn.stickyTableHeaders).toHaveBeenCalled();
        });

        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);

            // act
            progressiveContributionView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
    });

    describe("cacheDOMElements", () => {
        it("Always -> All neccessary elements have been cached", () => {
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
        });
    });

    describe("bindFilter", () => {
        beforeEach(() => {
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheDOMElements();
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);
        });

        it("_Report With no-Submit Param on Mobile -> Set Stack Mode as Default", () => {
            // arrange
            let setStackMode = spyOn(progressiveContributionView, "setStackMode");

            // act
            progressiveContributionView.bindFilter();

            // assert
            expect(setStackMode).toHaveBeenCalled();
        });

        it("_Report With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", () => {
            // arrange
            spyOn(reportTooltip, "show");

            // act
            progressiveContributionView.bindFilter();

            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });
    })

    describe("hideFilter", () => {
        it("Always -> Add Hide class and hide filter", () => {
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

    describe("_onExportExcel_Always", () => {
        it("_Submits Export Excel Form", () => {
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

    describe("_onSubmitReport", () => {
        it("_Not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            progressiveContributionView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            progressiveContributionView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(progressiveContributionView.$buttonProduct.hasClass("error")).toBeTruthy();
        });
        it("Have selected any products -> Blocks UI; Unblock UI; Set correct Params;", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            progressiveContributionView.init();
            spyOn(progressiveContributionView.$isStackMode, "val");
            spyOn(progressiveContributionView.$isFilterVisible, "val");

            let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            progressiveContributionView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(progressiveContributionView.$isStackMode.val).toHaveBeenCalledWith("false");
            expect(progressiveContributionView.$isFilterVisible.val).toHaveBeenCalledWith("true");
            expect(progressiveContributionView.$buttonProduct.hasClass("error")).toBeFalsy();
        });
    });

    describe("_getParams", () => {
        it("_No product selected -> Returns correct params", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheElements();

            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            // act
            let actualParam = progressiveContributionView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: false,
                IsStackMode: false,
                IsFilterVisible: true
            });
        });
        it("_Have product selected -> Returns correct params", () => {
            // arrange
            setFixtures(progressiveContributionTemplate.reportWithNormalMode);
            progressiveContributionView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1]);

            // act
            let actualParam = progressiveContributionView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: true,
                IsStackMode: false,
                IsFilterVisible: true
            });
        });
    });
});