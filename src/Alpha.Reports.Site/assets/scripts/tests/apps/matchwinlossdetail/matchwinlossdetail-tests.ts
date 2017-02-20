describe("MatchWinLossDetail", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const leagueMultiSelectionComponent = new LeagueMultiSelectionComponent($);
    const reportTooltip = new ReportTooltip($);

    let matchWinLossDetailView: MatchWinLossDetailView;

    let cacheElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        stickyTableHeaders: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderLeagueMultiSelectionComponent: jasmine.Spy;

    beforeEach(() => {
        matchWinLossDetailView = new MatchWinLossDetailView(
            jQueryFake,
            dateRangePickerComponent,
            productMultiSelectionComponent,
            leagueMultiSelectionComponent,
            reportTooltip);

        cacheElements = spyOn(matchWinLossDetailView, "cacheElements");
        cacheElements.and.callThrough();

        registerEvents = spyOn(matchWinLossDetailView, "registerEvents");
        registerEvents.and.callThrough();

        blockUI = spyOn(jQueryFake, "blockUI");
        unblockUI = spyOn(jQueryFake, "unblockUI");
        stickyTableHeaders = spyOn($.fn, "stickyTableHeaders");

        renderProductMultiSelectionComponent = spyOn(productMultiSelectionComponent, "render");
        renderProductMultiSelectionComponent.and.callThrough();

        renderLeagueMultiSelectionComponent = spyOn(leagueMultiSelectionComponent, "render");
        renderLeagueMultiSelectionComponent.and.callThrough();

        spyOn(dateRangePickerComponent, "render");
    });

    describe("_init_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);

            // act
            matchWinLossDetailView.init();
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(matchWinLossDetailView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls registerEvents", () => {
            // assert
            expect(matchWinLossDetailView.registerEvents).toHaveBeenCalled();
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
        it("_Calls leagueMultiSelectionComponent.render", () => {
            // assert
            expect(leagueMultiSelectionComponent.render).toHaveBeenCalled();
        });
        it("_Binds stickyTableHeaders", () => {
            // assert
            expect($.fn.stickyTableHeaders).toHaveBeenCalled();
        });
        it("_Report With no-Submit Param on Mobile -> Set Stack Mode as Default", () => {
            // arrange
            let setStackMode = spyOn(matchWinLossDetailView, "setStackMode");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            // act
            matchWinLossDetailView.init();

            // assert
            expect(setStackMode).toHaveBeenCalled();
        });

        it("_Report With no-Submit Param on Mobile -> Show Stack Mode Tooltip with correct options", () => {
            // arrange
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);
            spyOn(reportTooltip, "show");

            // act
            matchWinLossDetailView.init();

            // assert
            expect(reportTooltip.show).toHaveBeenCalledWith(new ReportTooltipOption("icon-normal-column", 1));
        });
    });
    describe("_checkReady", () => {
        it("_Document is not loaded yet (empty) -> Returns False", () => {
            // arrange
            setFixtures("");

            // act
            let actualIsReady = matchWinLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeFalsy();
        });
        it("_Document is loaded (ready) -> Returns True", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);

            // act
            let actualIsReady = matchWinLossDetailView.checkReady();

            // assert
            expect(actualIsReady).toBeTruthy();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);

            // act
            matchWinLossDetailView.cacheElements();

            // assert
            expect(matchWinLossDetailView.$pageTitle).toBeDefined();
            expect(matchWinLossDetailView.$tableReport).toBeDefined();
            expect(matchWinLossDetailView.$reportForm).toBeDefined();
            expect(matchWinLossDetailView.$exportExcelForm).toBeDefined();
            expect(matchWinLossDetailView.$isSetDefaultStackMode).toBeDefined();
            expect(matchWinLossDetailView.$isFilterVisible).toBeDefined();
            expect(matchWinLossDetailView.$isStackMode).toBeDefined();
            expect(matchWinLossDetailView.$leagueContainer).toBeDefined();
            expect(matchWinLossDetailView.$loadLeagueURL).toBeDefined();
        });
    });
    describe("_onExportExcel_Always", () => {
        it("_Submits Export Excel Form", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.cacheElements();
            spyOn(matchWinLossDetailView.$exportExcelForm, "submit");

            // act
            matchWinLossDetailView.onExportExcel();

            // assert
            expect(matchWinLossDetailView.$exportExcelForm.submit).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", () => {
        it("_Not select any product -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            matchWinLossDetailView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            matchWinLossDetailView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(matchWinLossDetailView.$buttonProduct.hasClass("error")).toBeTruthy();
        });
        it("Have selected any products -> Blocks UI; Unblock UI; Set correct Params;", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            renderProductMultiSelectionComponent.and.callThrough();
            matchWinLossDetailView.init();
            spyOn(matchWinLossDetailView.$isStackMode, "val");
            spyOn(matchWinLossDetailView.$isFilterVisible, "val");

            let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            matchWinLossDetailView.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(matchWinLossDetailView.$isStackMode.val).toHaveBeenCalledWith("false");
            expect(matchWinLossDetailView.$isFilterVisible.val).toHaveBeenCalledWith("true");
            expect(matchWinLossDetailView.$buttonLeague.hasClass("error")).toBeFalsy();
            expect(matchWinLossDetailView.$buttonProduct.hasClass("error")).toBeFalsy();
        });
    });
    describe("_onClickIconFilter", () => {
        it("_Filter is currently shown_Hides Filter", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.cacheElements();

            // act
            matchWinLossDetailView.onClickIconFilter();

            // assert
            expect(matchWinLossDetailView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(matchWinLossDetailView.$reportForm.hasClass("hide")).toBeTruthy();
        });
        it("_Filter is currently hidden_Hides Filter", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalModeAndHiddenFilter);
            matchWinLossDetailView.cacheElements();

            // act
            matchWinLossDetailView.onClickIconFilter();

            // assert
            expect(matchWinLossDetailView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(matchWinLossDetailView.$reportForm.hasClass("hide")).toBeFalsy();
        });
    });
    describe("_onSwitchMode", () => {
        describe("_1st Switch on Normal Mode", () => {
            let combineConsecutiveCellsSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(winLossDetailTemplate.currentReportWithNormalMode);
                matchWinLossDetailView.cacheElements();
                combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

                // act
                matchWinLossDetailView.onSwitchMode();
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
                expect(matchWinLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Adds Class stack-mode to Page Title", () => {
                // assert
                expect(matchWinLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            });
            it("_Combines Cells", () => {
                // assert
                expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                    [jasmine.any(Object), 2, "col-turnover-combined"],
                    [jasmine.any(Object), 3, "col-member-winloss-combined"],
                    [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                    [jasmine.any(Object), 3, "col-master-winloss-combined"],
                    [jasmine.any(Object), 3, "col-super-winloss-combined altercol"],
                    [jasmine.any(Object), 2, "col-company-winloss-combined"]]);
            });
        });
        describe("_1st Switch on Stack Mode", () => {
            let splitCellSpy: jasmine.Spy;
            beforeEach(() => {
                // arrange
                setFixtures(matchWinLossDetailTemplate.currentReportWithStackMode);
                matchWinLossDetailView.cacheElements();
                splitCellSpy = spyOn(alphaReports, "splitCell");

                // act
                matchWinLossDetailView.onSwitchMode();
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
                expect(matchWinLossDetailView.$tableReport.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Removes Class stack-mode to Page Title", () => {
                // assert
                expect(matchWinLossDetailView.$pageTitle.hasClass("stack-mode")).toBeFalsy();
            });
            it("_Splits Cells", () => {
                // assert
                expect(splitCellSpy.calls.allArgs()).toEqual([
                    ["col-turnover", ""],
                    ["col-member-winloss", ""],
                    ["col-agent-winloss", " altercol"],
                    ["col-master-winloss", ""],
                    ["col-super-winloss", " altercol"],
                    ["col-company-winloss", ""]]);
            });
        });
        describe("_2nd+ Switch on Normal Mode", () => {
            beforeEach(() => {
                // arrange
                setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
                matchWinLossDetailView.cacheElements();
                matchWinLossDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                matchWinLossDetailView.onSwitchMode();
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
                setFixtures(matchWinLossDetailTemplate.currentReportWithStackMode);
                matchWinLossDetailView.cacheElements();
                matchWinLossDetailView.isFirstSwitchMode = false;
                spyOn(alphaReports, "combineConsecutiveCells");
                spyOn(alphaReports, "splitCell");

                // act
                matchWinLossDetailView.onSwitchMode();
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
    describe("_setStackMode_Always", () => {
        let combineConsecutiveCellsSpy: jasmine.Spy;
        beforeEach(() => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.cacheElements();
            combineConsecutiveCellsSpy = spyOn(alphaReports, "combineConsecutiveCells");

            // act
            matchWinLossDetailView.setStackMode();
        });

        it("_Blocks UI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Set stack-mode to table reports", () => {
            // assert
            expect(matchWinLossDetailView.$pageTitle.hasClass("stack-mode")).toBeTruthy();
            expect(matchWinLossDetailView.$tableReport.hasClass("stack-mode")).toBeTruthy();
        });
        it("_Combines Columns", () => {
            // assert
            expect(combineConsecutiveCellsSpy.calls.allArgs()).toEqual([
                [jasmine.any(Object), 2, "col-turnover-combined"],
                [jasmine.any(Object), 3, "col-member-winloss-combined"],
                [jasmine.any(Object), 3, "col-agent-winloss-combined altercol"],
                [jasmine.any(Object), 3, "col-master-winloss-combined"],
                [jasmine.any(Object), 3, "col-super-winloss-combined altercol"],
                [jasmine.any(Object), 2, "col-company-winloss-combined"]]);
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_getParams", () => {
        it("_No product selected -> Returns correct params", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);
            spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            // act
            let actualParam = matchWinLossDetailView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: false,
                IsStackMode: false,
                IsFilterVisible: true
            });
        });
        it("_Have product selected -> Returns correct params", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.cacheElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1]);
            spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);

            // act
            let actualParam = matchWinLossDetailView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: true,
                IsStackMode: false,
                IsFilterVisible: true
            });
        });
    });
    describe("_getLeagueParam", () => {
        it("_Alway -> Returns correct params", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.$fromDate = matchWinLossDetailView.$("#from-date");
            matchWinLossDetailView.$toDate = matchWinLossDetailView.$("#to-date");
            matchWinLossDetailView.$custId = matchWinLossDetailView.$("#CustId");
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1, 2, 3]);

            // act
            let actualParam = matchWinLossDetailView.getLeagueParam();

            // assert
            expect(actualParam).toEqual({
                custId: "1",
                fromDate: "",
                toDate: "",
                products: "1,2,3"
            });
        });
    });
    describe("_showLeagues", () => {
        it("_League is shown -> Not get param for call ajax", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalModeAndLeagueIsShown);
            spyOn(matchWinLossDetailView, "getLeagueParam");
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");

            // act
            matchWinLossDetailView.showLeagues(null);

            // assert
            expect(matchWinLossDetailView.getLeagueParam).not.toHaveBeenCalled();
        });
        it("_League is hidden/Not Change Param -> Block UI/Unblock UI", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");
            let oldParam = {
                custId: "1",
                fromDate: "01/01/2017",
                toDate: "01/01/2017",
                products: "1,2,3"
            };
            matchWinLossDetailView.getLeaguesByCustomerParam = oldParam;
            spyOn(matchWinLossDetailView, "getLeagueParam").and.callFake(() => oldParam);

            // act
            matchWinLossDetailView.showLeagues(null);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_League is hidden/Change Product to Empty -> Block UI/Unblock UI", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");
            let oldParam = {
                custId: "1",
                fromDate: "01/01/2017",
                toDate: "01/01/2017",
                products: "1,2,3"
            };

            matchWinLossDetailView.getLeaguesByCustomerParam = oldParam;
            spyOn(matchWinLossDetailView, "getLeagueParam").and.callFake(() => {
                return {
                    fromDate: "01/01/2017",
                    toDate: "01/01/2017",
                    products: ""
                };
            });

            // act
            matchWinLossDetailView.showLeagues(null);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_League is hidden/Change Product to Another Value (Not Empty) -> stopPropagation", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");
            matchWinLossDetailView.$loadLeagueURL = matchWinLossDetailView.$("#LoadLeagueURL");
            let oldParam = {
                custId: "1",
                fromDate: "01/01/2017",
                toDate: "01/01/2017",
                products: "1,2,3"
            };

            matchWinLossDetailView.getLeaguesByCustomerParam = oldParam;
            spyOn(matchWinLossDetailView, "getLeagueParam").and.callFake(() => {
                return {
                    fromDate: "01/01/2017",
                    toDate: "01/01/2017",
                    products: "1,2"
                };
            });

            let event = {
                preventDefault: jasmine.createSpy("preventDefault", () => null),
                stopPropagation: jasmine.createSpy("stopPropagation", () => null)
            };

            spyOn($, "ajax");

            // act
            matchWinLossDetailView.showLeagues(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).not.toHaveBeenCalled();
            expect(event.preventDefault).toHaveBeenCalled();
            expect(event.stopPropagation).toHaveBeenCalled();
        });
        it("_League is hidden/Change Param -> set new param", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");
            matchWinLossDetailView.$loadLeagueURL = matchWinLossDetailView.$("#LoadLeagueURL");
            let oldParam = {
                custId: "1",
                fromDate: "01/01/2017",
                toDate: "01/01/2017",
                products: "1,2,3"
            };

            matchWinLossDetailView.getLeaguesByCustomerParam = oldParam;

            let newParam = {
                fromDate: "02/01/2017",
                toDate: "01/01/2017",
                products: "1,2,3"
            };

            spyOn(matchWinLossDetailView, "getLeagueParam").and.callFake(() => newParam);

            let event = {
                preventDefault: jasmine.createSpy("preventDefault", () => null),
                stopPropagation: jasmine.createSpy("stopPropagation", () => null)
            };

            spyOn($, "ajax");

            // act
            matchWinLossDetailView.showLeagues(event);

            // assert
            expect(matchWinLossDetailView.getLeaguesByCustomerParam).toEqual(newParam);
        });
        it("_League is hidden/Change Param -> call ajax", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");
            matchWinLossDetailView.$loadLeagueURL = matchWinLossDetailView.$("#LoadLeagueURL");
            let oldParam = {
                custId: "1",
                fromDate: "01/01/2017",
                toDate: "01/01/2017",
                products: "1,2,3"
            };

            matchWinLossDetailView.getLeaguesByCustomerParam = oldParam;

            let newParam = {
                fromDate: "02/01/2017",
                toDate: "02/01/2017",
                products: "1,2"
            };

            spyOn(matchWinLossDetailView, "getLeagueParam").and.callFake(() => newParam);

            let event = {
                preventDefault: jasmine.createSpy("preventDefault", () => null),
                stopPropagation: jasmine.createSpy("stopPropagation", () => null)
            };

            spyOn($, "ajax");

            // act
            matchWinLossDetailView.showLeagues(event);

            // assert
            expect($.ajax).toHaveBeenCalledWith({
                method: "GET",
                url: "League/LeaguesByCustomer",
                cache: false,
                data: newParam,
                success: jasmine.any(Function),
                complete: jasmine.any(Function)
            });
        });
        it("_League is hidden/Change Param -> call ajax", () => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.$buttonLeague = matchWinLossDetailView.$("#dropdown-league").parent().find(".multiselect.btn");
            matchWinLossDetailView.$loadLeagueURL = matchWinLossDetailView.$("#LoadLeagueURL");
            let oldParam = {
                custId: "1",
                fromDate: "01/01/2017",
                toDate: "01/01/2017",
                products: "1,2,3"
            };

            matchWinLossDetailView.getLeaguesByCustomerParam = oldParam;

            let newParam = {
                fromDate: "02/01/2017",
                toDate: "02/01/2017",
                products: "1,2"
            };

            spyOn(matchWinLossDetailView, "getLeagueParam").and.callFake(() => newParam);

            let event = {
                preventDefault: jasmine.createSpy("preventDefault", () => null),
                stopPropagation: jasmine.createSpy("stopPropagation", () => null)
            };

            spyOn($, "ajax");

            // act
            matchWinLossDetailView.showLeagues(event);

            // assert
            expect($.ajax).toHaveBeenCalledWith({
                method: "GET",
                url: "League/LeaguesByCustomer",
                cache: false,
                data: newParam,
                success: jasmine.any(Function),
                complete: jasmine.any(Function)
            });
        });
    });
    describe("_renderLeagues", () => {
        let leagueHtml = "<div id=\"dropdown-league-container\" class=\"form-group\"><select id=\"dropdown-league\"></select></div>";

        beforeEach(() => {
            // arrange
            setFixtures(matchWinLossDetailTemplate.currentReportWithNormalMode);
            matchWinLossDetailView.init();
        });

        it("_league container empty and append new html content", () => {
            // arrange
            spyOn(matchWinLossDetailView.$leagueContainer, "empty").and.callThrough();
            spyOn(matchWinLossDetailView.$leagueContainer, "append");

            // act
            matchWinLossDetailView.renderLeagues(leagueHtml);

            // assert
            expect(matchWinLossDetailView.$leagueContainer.empty).toHaveBeenCalled();
            expect(matchWinLossDetailView.$leagueContainer.append).toHaveBeenCalledWith("<select id=\"dropdown-league\"></select>");
        });

        it("_render leagueMultiSelectionComponent", () => {
            // act
            matchWinLossDetailView.renderLeagues(leagueHtml);

            // assert
            expect(leagueMultiSelectionComponent.render).toHaveBeenCalled();
        });
        it("_rebind click event", () => {
            // arrange
            spyOn(matchWinLossDetailView, "showLeagues");

            // act
            matchWinLossDetailView.renderLeagues(leagueHtml);
            matchWinLossDetailView.$buttonLeague.trigger("click");

            // assert
            expect(matchWinLossDetailView.showLeagues).toHaveBeenCalled();
        });
        it("_show league", () => {
            // arrange
            spyOn(matchWinLossDetailView, "showLeagues");

            // act
            matchWinLossDetailView.renderLeagues(leagueHtml);

            // assert
            expect(matchWinLossDetailView.showLeagues).toHaveBeenCalled();
        });
    });
});