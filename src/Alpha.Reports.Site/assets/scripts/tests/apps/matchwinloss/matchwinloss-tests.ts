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
                        <span id="icon-excel" class="icon-excel" title="@Resources.ExportToExcel"></span>
                        <table id="tbl-report" data-totalrecords="0"
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
});