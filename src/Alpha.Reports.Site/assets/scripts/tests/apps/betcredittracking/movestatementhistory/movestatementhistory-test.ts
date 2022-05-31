describe("MoveStatementHistory", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);

    let moveStatementHistory: MoveStatementHistory;

    beforeEach(() => {
        moveStatementHistory = new MoveStatementHistory(jQueryFake, dateRangePickerComponent);

        spyOn(moveStatementHistory, "cacheElements").and.callThrough();
        spyOn(moveStatementHistory, "bindEvents").and.callThrough();
        spyOn(moveStatementHistory, "initComponents").and.callThrough();
        spyOn(moveStatementHistory, "onSubmitReport").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");

        spyOn(dateRangePickerComponent, "render");

        spyOn(jQueryFake.fn, "stickyTableHeaders");
    });

    describe("_init", () => {
        beforeEach(() => {
            // act
            moveStatementHistory.init();
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(moveStatementHistory.cacheElements).toHaveBeenCalled();
        });
        it("_Calls initComponents", () => {
            // assert
            expect(moveStatementHistory.initComponents).toHaveBeenCalled();
        });
        it("_Calls bindEvents", () => {
            // assert
            expect(moveStatementHistory.bindEvents).toHaveBeenCalled();
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
        it("_Calls stickyTableHeaders", () => {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // act
            moveStatementHistory.cacheElements();

            // assert
            expect(moveStatementHistory.$reportForm).toBeDefined();
            expect(moveStatementHistory.$pageTitle).toBeDefined();
            expect(moveStatementHistory.$exportExcelForm).toBeDefined();
            expect(moveStatementHistory.$isFilterVisible).toBeDefined();
        });
    });
    describe("_onExportExcel_Always", () => {
        beforeEach(() => {
            // arrange
            moveStatementHistory.cacheElements();
            spyOn(moveStatementHistory.$exportExcelForm, "submit");

            // act
            moveStatementHistory.onExportExcel();
        });
        it("_Submits Export Excel Form", () => {
            // assert
            expect(moveStatementHistory.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", () => {
        it("_Always -> Blocks UI; Unblock UI;", () => {
            // arrange
            moveStatementHistory.init();

            // act
            moveStatementHistory.onSubmitReport();

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
})