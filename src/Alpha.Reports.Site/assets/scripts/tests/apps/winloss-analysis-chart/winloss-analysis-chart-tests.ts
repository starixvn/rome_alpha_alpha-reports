describe("WinLossAnalysisChart", () => {
    let chart = {};
    const jQueryFake = $;
    const productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const reportTooltip = new ReportTooltip($);
    const monthRangePickerComponent = new MonthRangePickerComponent($);
    const dualAxisChartComponent = new DualAxisChartComponent($, chart);

    let winLossAnalysisChartView: WinLossAnalysisChartView;

    let template = {
        winLossAnalysisChart: `<body>
                                    <div class="page-title"></div>
                                    <form id="report-form" role="form">
                                        <div id="msg-error"></div>
                                        <div>
                                            <select id="dropdown-product">
                                                <option value="1" selected="selected"></option>
                                            </select>
                                            <button class="multiselect btn"></button>
                                        </div>
                                        <div id="from-monthpicker"
                                            data-longmonths-inyear="['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov', 'Dec']"></div>
                                        <div id="to-monthpicker"
                                            data-longmonths-inyear="['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov', 'Dec']"></div>
                                        <div id="from-date"></div>
                                        <div id="to-date"></div>
                                        <div class="icon-filter"></div>
                                    </form>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <div class="chart-container">
                                        <canvas id="winloss-chart" data-title="ReportTitle">
                                    </div>
                                </body>`,
        hiddenFilter: `<body>
                                    <div class="page-title"></div>
                                    <form id="report-form" role="form">
                                        <div id="msg-error"></div>
                                        <div>
                                            <select id="dropdown-product">
                                                <option value="1" selected="selected"></option>
                                            </select>
                                            <button class="multiselect btn"></button>
                                        </div>
                                        <div id="from-monthpicker"
                                            data-longmonths-inyear="['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov', 'Dec']"></div>
                                        <div id="to-monthpicker"
                                            data-longmonths-inyear="['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov', 'Dec']"></div>
                                        <div id="from-date"></div>
                                        <div id="to-date"></div>
                                        <div class="icon-filter"></div>
                                    </form>
                                    <input id="IsFilterVisible" value="False" type="hidden" />
                                    <div class="chart-container">
                                        <canvas id="winloss-chart" data-title="ReportTitle">
                                    </div>
                                </body>`
    };

    let cacheDOMElements: jasmine.Spy,
        registerEvents: jasmine.Spy,
        blockUI: jasmine.Spy,
        unblockUI: jasmine.Spy,
        renderProductMultiSelectionComponent: jasmine.Spy,
        renderLinkBuilderComponent: jasmine.Spy;

    beforeEach(() => {
        winLossAnalysisChartView = new WinLossAnalysisChartView(
            jQueryFake,
            monthRangePickerComponent,
            productMultiSelectionComponent,
            reportTooltip,
            linkBuilderComponent,
            dualAxisChartComponent);

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

    it("Inherit BaseReport_Always_WinLossAnalysisChartView is BaseReport", () => {
        // assert
        expect(winLossAnalysisChartView instanceof BaseReport).toBeTruthy();
    });

    describe("_init", () => {
        it("_Calls Specific Methods", () => {
            // act
            winLossAnalysisChartView.init();

            // assert
            expect(winLossAnalysisChartView.cacheDOMElements).toHaveBeenCalled();
            expect(winLossAnalysisChartView.registerEvents).toHaveBeenCalled();
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(monthRangePickerComponent.render).toHaveBeenCalled();
            expect(productMultiSelectionComponent.render).toHaveBeenCalled();
            expect(dualAxisChartComponent.render).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // act
            winLossAnalysisChartView.cacheDOMElements();

            // assert
            expect(winLossAnalysisChartView.$pageTitle).toBeDefined();
            expect(winLossAnalysisChartView.$canvasChart).toBeDefined();
            expect(winLossAnalysisChartView.$isFilterVisible).toBeDefined();
            expect(winLossAnalysisChartView.$reportForm).toBeDefined();
            expect(winLossAnalysisChartView.$filterIcon).toBeDefined();
            expect(winLossAnalysisChartView.$fromDate).toBeDefined();
            expect(winLossAnalysisChartView.$toDate).toBeDefined();
            expect(winLossAnalysisChartView.$msgError).toBeDefined();
        });
    });

    describe("_bindFilter", () => {
        it("_should hide filter -> Hide filter", () => {
            // arrange
            setFixtures(template.hiddenFilter);
            winLossAnalysisChartView.cacheDOMElements();

            // act
            winLossAnalysisChartView.bindFilter();

            // assert
            expect(winLossAnalysisChartView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(winLossAnalysisChartView.$reportForm.hasClass("hide")).toBeTruthy();
        });
    });

    describe("_getParams", () => {
        let getParamsTest = (selectedProduct: Array<number>, expectedIsAnyProductSelected: boolean) => {
            // arrange
            winLossAnalysisChartView.cacheDOMElements();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => selectedProduct);

            // act
            var actualParam = winLossAnalysisChartView.getParams();

            // assert
            expect(actualParam).toEqual({
                IsAnyProductSelected: expectedIsAnyProductSelected,
                IsLeast3MonthToChart: true,
                IsFilterVisible: true
            });

            it("_No product selected -> returns correct params", () => {
                getParamsTest([], false);
            });
            it("_Have product selected -> Returns correct params", () => {
                getParamsTest([1], true);
            });
        };
    });

    describe("_showValidationProductError", () => {
        it("_Always -> Adds class error to multiple select button of dropdown product", () => {
            // arrange
            winLossAnalysisChartView.init();

            // act
            winLossAnalysisChartView.showValidationProductError();

            // assert
            expect(winLossAnalysisChartView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
    });

    describe("_showValidationMonthRangeError", () => {
        it("_Always -> Removes class hide to msg-error", () => {
            // arrange
            winLossAnalysisChartView.init();

            // act
            winLossAnalysisChartView.showValidationMonthRangeError();

            // assert
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeFalsy();
        });
    });

    describe("_hideValidationMonthRangeError", () => {
        it("_Always -> Adds class hide to msg-error", () => {
            // arrange
            winLossAnalysisChartView.init();

            // act
            winLossAnalysisChartView.hideValidationMonthRangeError();

            // assert
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeTruthy();
        });
    });

    describe("_onSubmitReportEvent", () => {
        it("_Not select anyProduct -> Blocks UI; Shows Validate Product Error; Unblock UI;", () => {
            //  arrange
            renderProductMultiSelectionComponent.and.callThrough();
            winLossAnalysisChartView.init();
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => []);
            spyOn(monthRangePickerComponent, "isLeast3Month");

            let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossAnalysisChartView.onSubmitReportEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(winLossAnalysisChartView.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Is Not Least 3 Months -> Blocks UI; Show Validation Month Range Error ;Unblock UI;", () => {
            //  arrange
            renderProductMultiSelectionComponent.and.callThrough();
            winLossAnalysisChartView.init();
            spyOn(monthRangePickerComponent, "isLeast3Month");

            let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossAnalysisChartView.onSubmitReportEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeFalsy();
        });
        it("_Slect products and is least 3 months -> Block UI, Unblock UI; Sets params to hidden fields", () => {
            // arrange
            renderProductMultiSelectionComponent.and.callThrough();
            spyOn(monthRangePickerComponent, "isLeast3Month").and.returnValue(true);
            winLossAnalysisChartView.init();

            let event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            winLossAnalysisChartView.onSubmitReportEvent(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
            expect(winLossAnalysisChartView.$multiSelectButton.hasClass("error")).toBeFalsy();
            expect(winLossAnalysisChartView.$msgError.hasClass("hide")).toBeTruthy();
            expect(winLossAnalysisChartView.$isFilterVisible.val()).toBeTruthy();
        });
    });
});