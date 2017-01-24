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

    describe("_registerEvents_Always", () => {
        beforeEach(() => {
            // arrange
            matchWinLossView.cacheElements();
            spyOn(matchWinLossView, "onExportExcel");

            // act
            matchWinLossView.registerEvents();
        });
        it("_Bind onExportExcel", () => {
            // act
            matchWinLossView.$("#icon-excel").trigger("click");

            // assert
            expect(matchWinLossView.onExportExcel).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", () => {
            // arrange
            matchWinLossView.init();

            // act
            matchWinLossView.$reportForm.trigger("submit");

            // assert
            expect(matchWinLossView.onSubmitReport).toHaveBeenCalled();
        });
    });
});