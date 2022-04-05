describe("BetCreditHistory", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);

    let betCreditHistory: BetCreditHistory;

    beforeEach(() => {
        betCreditHistory = new BetCreditHistory(jQueryFake, dateRangePickerComponent);

        spyOn(betCreditHistory, "cacheElements").and.callThrough();
        spyOn(betCreditHistory, "bindEvents").and.callThrough();
        spyOn(betCreditHistory, "initComponents").and.callThrough();
        spyOn(betCreditHistory, "onSubmitReport").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");

        spyOn(dateRangePickerComponent, "render");

        spyOn(jQueryFake.fn, "stickyTableHeaders");
    });

    describe("_init", () => {
        beforeEach(() => {
            // act
            betCreditHistory.init();
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(betCreditHistory.cacheElements).toHaveBeenCalled();
        });
        it("_Calls initComponents", () => {
            // assert
            expect(betCreditHistory.initComponents).toHaveBeenCalled();
        });
        it("_Calls bindEvents", () => {
            // assert
            expect(betCreditHistory.bindEvents).toHaveBeenCalled();
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
            betCreditHistory.cacheElements();

            // assert
            expect(betCreditHistory.$reportForm).toBeDefined();
            expect(betCreditHistory.$pageTitle).toBeDefined();
            expect(betCreditHistory.$exportExcelForm).toBeDefined();
            expect(betCreditHistory.$pageTitle).toBeDefined();
            expect(betCreditHistory.$msgError).toBeDefined();
            expect(betCreditHistory.$requiredMsgError).toBeDefined();
            expect(betCreditHistory.$isFilterVisible).toBeDefined();
            expect(betCreditHistory.$exportExcelForm).toBeDefined();
        });
    });
    describe("_onExportExcel_Always", () => {
        beforeEach(() => {
            // arrange
            betCreditHistory.cacheElements();
            spyOn(betCreditHistory.$exportExcelForm, "submit");

            // act
            betCreditHistory.onExportExcel();
        });
        it("_Submits Export Excel Form", () => {
            // assert
            expect(betCreditHistory.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", () => {
        it("_Always -> Blocks UI; Unblock UI;", () => {
            // arrange
            betCreditHistory.init();
            var event = { preventDefault: jasmine.createSpy("preventDefault", () => null) };

            // act
            betCreditHistory.onSubmitReport(event);

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
})