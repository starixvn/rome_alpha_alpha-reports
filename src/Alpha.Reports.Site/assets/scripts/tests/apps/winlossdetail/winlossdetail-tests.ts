describe("WinLossDetail_", () => {
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const pagerComponent = new PagerComponent($);
    const sortComponent = new SortComponent($);
    const monthPickerComponent = new MonthPickerComponent($);
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const reportTooltip = new ReportTooltip($);
    const netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);

    let winLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderPagerComponent: jasmine.Spy,
        renderSortComponent: jasmine.Spy,
        renderdateRangePickerComponent: jasmine.Spy;

    beforeEach(() => {
        winLossDetailView = new WinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            pagerComponent,
            productMultiSelectionComponent,
            sortComponent,
            reportTooltip,
            monthPickerComponent,
            netTurnoverTooltipComponent);

        cacheElements = spyOn(winLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(winLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderPagerComponent = spyOn(pagerComponent, "render");
        renderSortComponent = spyOn(sortComponent, "render");
        renderdateRangePickerComponent = spyOn(dateRangePickerComponent, "render");
    });

    describe("init", () => {
        it("Document is not ready -> Does not call any Method", () => {
            // act
            winLossDetailView.init();

            // assert
            expect(winLossDetailView.cacheElements).not.toHaveBeenCalled();
            expect(winLossDetailView.registerEvents).not.toHaveBeenCalled();
            expect(jQueryFake.blockUI).not.toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();

            expect(productMultiSelectionComponent.render).not.toHaveBeenCalled();
            expect(pagerComponent.render).not.toHaveBeenCalled();
            expect(sortComponent.render).not.toHaveBeenCalled();
        });
    });
    describe("init", () => {
        it("Document is ready -> Calls Specific Methods", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);

            // act
            winLossDetailView.init();

            // assert
            expect(winLossDetailView.cacheElements).toHaveBeenCalled();
            expect(winLossDetailView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();

            expect(productMultiSelectionComponent.render).toHaveBeenCalled();
            expect(pagerComponent.render).toHaveBeenCalled();
            expect(sortComponent.render).toHaveBeenCalled();
        });
    });
    describe("init", () => {
        it("Current Report -> Renders Date Range Picker And Not Render Month Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);
            spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
            expect(monthPickerComponent.render).not.toHaveBeenCalled();
        });
    });
    describe("init", () => {
        it("Historic Report With Hidden Filter -> Renders Month Picker And Not Render Date Range Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render");

            // act
            winLossDetailView.init();

            // assert
            expect(monthPickerComponent.render).toHaveBeenCalled();
            expect(dateRangePickerComponent.render).not.toHaveBeenCalled();
        });
    });
    describe("init", () => {
        // historic report: we must show filter before render month picker
        it("Historic Report With Hidden Filter -> Shows Filter Before Renders Month Picker", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);

            let actualIsHidden;
            spyOn(monthPickerComponent, "render").and.callFake(() => {
                actualIsHidden = $("#report-form").hasClass("hide");
            });

            // act
            winLossDetailView.init();

            // assert
            expect(actualIsHidden).toBeFalsy();
        });
    });

    describe("init", () => {
        // historic report: we must show filter before render month picker
        // therefore, after init we also hide it if before that it is hidden
        it("Historic Report With Hidden Filter -> Hides Filter Is Hidden After Init", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            spyOn(monthPickerComponent, "render").and.callThrough();

            // act
            winLossDetailView.init();
            var actualIsHidden = $("#report-form").hasClass("hide");

            // assert
            expect(actualIsHidden).toBeTruthy();
        });

        it("Report Of Top Level With no-Submit Param on Mobile -> Set Stack Mode as Default", () => {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            let setStackMode = spyOn(winLossDetailView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            // act
            winLossDetailView.init();

            // assert
            expect(setStackMode).toHaveBeenCalled();
        });

        it("Report Of Top Level With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", () => {
            // arrange
            setFixtures(winLossDetailTemplate.reportOfTopLevel);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);
            spyOn(reportTooltip, "show");

            // act
            winLossDetailView.init();

            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });

        it("Always -> Render sort component with correct options", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);
            spyOn(pagerComponent, "hasAllDataOnClient").and.callFake(() => true);

            // act
            winLossDetailView.init();

            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                winLossDetailView.$tableReport,
                false,
                true,
                winLossDetailView.afterSort,
                { 0: { sorter: "text" } }
            ));
        });
    });

    describe("checkReady", () => {
        it("Document is not loaded yet (empty) -> Returns False", () => {
            // arrange
            setFixtures("");

            // act
            var actualIsReady = winLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeFalsy();
        });

        it("Document is loaded (ready) -> Returns True", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);

            // act
            var actualIsReady = winLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });

    describe("cacheElements", () => {
        it("Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);

            // act
            winLossDetailView.cacheElements();

            // assert
            expect(winLossDetailView.$pageTitle).toBeDefined();
            expect(winLossDetailView.$tableReport).toBeDefined();
            expect(winLossDetailView.$drilldownLinks).toBeDefined();
            expect(winLossDetailView.$isFilterVisible).toBeDefined();
            expect(winLossDetailView.$isStackMode).toBeDefined();
            expect(winLossDetailView.$isDescendingSort).toBeDefined();
            expect(winLossDetailView.$sortPropertyName).toBeDefined();
            expect(winLossDetailView.$reportForm).toBeDefined();
            expect(winLossDetailView.$exportExcelForm).toBeDefined();
            expect(winLossDetailView.$userSelectedDateRangeOption).toBeDefined();
            expect(winLossDetailView.$multiSelectButton).toBeDefined();
            expect(winLossDetailView.$reportSwitch).toBeDefined();
            expect(winLossDetailView.$fromDate).toBeDefined();
            expect(winLossDetailView.$toDate).toBeDefined();
            expect(winLossDetailView.$iconNormalColumn).toBeDefined();
            expect(winLossDetailView.$hasSubmittedParams).toBeDefined();
            expect(winLossDetailView.$breadcrumbLinks).toBeDefined();
        });
    });
    describe("registerEvents", () => {
        it("Always -> All neccessary functions have been registered", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);
            winLossDetailView.cacheElements();

            spyOn(winLossDetailView, "onSwitchReport");
            spyOn(winLossDetailView, "onClickIconFilter");
            spyOn(winLossDetailView, "onSwitchMode");
            spyOn(winLossDetailView, "onExportExcel");
            spyOn(winLossDetailView, "onSubmitReport");

            // act
            winLossDetailView.registerEvents();

            // assert
            winLossDetailView.$reportSwitch.trigger("click");
            expect(winLossDetailView.onSwitchReport).toHaveBeenCalled();

            $(".icon-filter").trigger("click");
            expect(winLossDetailView.onClickIconFilter).toHaveBeenCalled();

            winLossDetailView.$(".icon-switchmode").trigger("click");
            expect(winLossDetailView.onSwitchMode).toHaveBeenCalled();

            winLossDetailView.$("#icon-excel").trigger("click");
            expect(winLossDetailView.onExportExcel).toHaveBeenCalled();

            winLossDetailView.$reportForm.trigger("submit");
            expect(winLossDetailView.onSubmitReport).toHaveBeenCalled();
        });
    });
    describe("onSwitchReport", () => {
        it("Current Report -> Switch to Historic Report and Submit", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$reportForm, "trigger");

            // act
            winLossDetailView.onSwitchReport();

            // assert
            expect(winLossDetailView.isSwitchView).toBeTruthy();
            expect(winLossDetailView.isHistoricReport).toBeTruthy();
            expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });

        it("Historic Report -> Switch to Current Report and Submit", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView.$reportForm, "trigger");

            // act
            winLossDetailView.onSwitchReport();

            // assert
            expect(winLossDetailView.isSwitchView).toBeTruthy();
            expect(winLossDetailView.isHistoricReport).toBeFalsy();
            expect(winLossDetailView.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
    });

    describe("onClickIconFilter", () => {
        it("Filter is currently shown -> Hides Filter and Update Href Param to False", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithShownFilter);
            winLossDetailView.cacheElements();

            spyOn(winLossDetailView, "hideFilter");
            spyOn(winLossDetailView, "replaceHrefParam");

            // act
            winLossDetailView.onClickIconFilter();

            // assert
            expect(winLossDetailView.hideFilter).toHaveBeenCalled();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
        });
        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
            // arrange
            setFixtures(winLossDetailTemplate.historicReportWithHiddenFilter);
            winLossDetailView.cacheElements();

            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
            spyOn(winLossDetailView, "replaceHrefParam");

            // act
            winLossDetailView.onClickIconFilter();

            // assert
            expect(winLossDetailView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(winLossDetailView.$reportForm.hasClass("hide")).toBeFalsy();
            expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalled();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
        });
    });
    describe("setStackMode", () => {
        it("Filter is currently hidden -> Hides Filter and Update Href Param to True", () => {
            // arrange
            setFixtures(winLossDetailTemplate.currentReport);
            winLossDetailView.cacheElements();
            spyOn(winLossDetailView, "replaceHrefParam");
            var combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            winLossDetailView.setStackMode();

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(winLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            expect(winLossDetailView.replaceHrefParam).toHaveBeenCalledWith("IsStackMode", true);

            expect(combineConsecutiveCellsSpy.calls.allArgs()[0]).toEqual([
                jasmine.any(Object), 3, "col-turnover-combined"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[1]).toEqual([
                jasmine.any(Object), 3, "col-member-winloss-combined"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[2]).toEqual([
                jasmine.any(Object), 3, "col-agent-winloss-combined altercol"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[3]).toEqual([
                jasmine.any(Object), 3, "col-master-winloss-combined"]);
            expect(combineConsecutiveCellsSpy.calls.allArgs()[4]).toEqual([
                jasmine.any(Object), 3, "col-super-winloss-combined altercol"]);

            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("onSwitchMode", () => {

    });


});