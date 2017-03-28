describe("MatchWinLoss", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const sortComponent = new SortComponent($, new LinkBuilderComponent($));

    let matchWinLossView: MatchWinLossView;

    beforeEach(() => {
        matchWinLossView = new MatchWinLossView(
            jQueryFake,
            dateRangePickerComponent,
            sortComponent);

        spyOn(matchWinLossView, "cacheElements").and.callThrough();
        spyOn(matchWinLossView, "registerEvents").and.callThrough();
        spyOn(matchWinLossView, "onSubmitReport").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");

        spyOn(dateRangePickerComponent, "render");
        spyOn(sortComponent, "render").and.callThrough();

        spyOn(jQueryFake.fn, "stickyTableHeaders");

        setFixtures(`<body>
                        <form id="report-form" role="form">
                            <input id="IsDescendingSort" value="False" type="hidden" />
                            <input id="SortPropertyName" value="EventInfo" type="hidden" />
                        </form>
                        <span class="icon-filter" title="Hide filters"></span>
                        <span id="icon-excel" class="icon-excel" title="@Resources.ExportToExcel"></span>
                        <div class="page-title"></div>
                        <table id="tbl-report" data-cansort="False"
                            data-sortedcolumn="Event Info"
                            data-descendingsort="False">
                        </table>
                    </body>`);
    });

    describe("_init", () => {
        beforeEach(() => {
            // act
            matchWinLossView.init();
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(matchWinLossView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls registerEvents", () => {
            // assert
            expect(matchWinLossView.registerEvents).toHaveBeenCalled();
        });
        it("_Calls blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Calls dateRangePickerComponent.render", () => {
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Calls sortComponent.render", () => {
            // assert
            expect(sortComponent.render).toHaveBeenCalled();
        });
        it("_Calls sortComponent.stickyTableHeaders", () => {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // act
            matchWinLossView.cacheElements();

            // assert
            expect(matchWinLossView.$tableReport).toBeDefined();
            expect(matchWinLossView.$reportForm).toBeDefined();
            expect(matchWinLossView.$exportExcelForm).toBeDefined();

            expect(matchWinLossView.$isDescendingSort).toBeDefined();
            expect(matchWinLossView.$sortPropertyName).toBeDefined();
        });
    });
    describe("_onExportExcel_Always", () => {
        beforeEach(() => {
            // arrange
            matchWinLossView.cacheElements();
            spyOn(matchWinLossView.$exportExcelForm, "submit");

            // act
            matchWinLossView.onExportExcel();
        });
        it("_Submits Export Excel Form", () => {
            // assert
            expect(matchWinLossView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", () => {
        it("_Always -> Blocks UI; Unblock UI;", () => {
            // arrange
            matchWinLossView.init();

            // act
            matchWinLossView.onSubmitReport();

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onClickIconFilter", () => {
        describe("_Filter is currently shown", () => {
            it("_Hides Filter", () => {
                // arrange
                matchWinLossView.cacheElements();

                // act
                matchWinLossView.onClickIconFilter();

                // assert
                expect(matchWinLossView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(matchWinLossView.$reportForm.hasClass("hide")).toBeTruthy();
            });
        });
        describe("_Filter is currently hidden", () => {
            it("_Shows Filter", () => {
                // arrange
                setFixtures(`<body>
                                <form id="report-form" role="form" class="hide">
                                    <input id="IsDescendingSort" value="False" type="hidden" />
                                    <input id="SortPropertyName" value="EventInfo" type="hidden" />
                                </form>
                                <span class="icon-filter hide" title="Hide filters"></span>
                                <span id="icon-excel" class="icon-excel" title="@Resources.ExportToExcel"></span>
                                <div class="page-title hide-filter"></div>
                                <table id="tbl-report" data-cansort="False"
                                    data-sortedcolumn="Event Info"
                                    data-descendingsort="False">
                                </table>
                            </body>`);

                matchWinLossView.cacheElements();

                // act
                matchWinLossView.onClickIconFilter();

                // assert
                expect(matchWinLossView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(matchWinLossView.$reportForm.hasClass("hide")).toBeFalsy();
            });
        });
    });
});