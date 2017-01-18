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
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");

        spyOn(dateRangePickerComponent, "render");
        spyOn(sortComponent, "render");

        spyOn(jQueryFake.fn, "stickyTableHeaders");

        setFixtures(`<body>
                        <span id="icon-excel" class="icon-excel" title="@Resources.ExportToExcel"></span>
                        <table id="tbl-report" data-totalrecords="0"></table>
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
        });
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
});