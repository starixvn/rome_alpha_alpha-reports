describe("MoveStatementHistory", function () {
    var jQueryFake = $;
    var monthPickerComponent = new MonthPickerComponent($);
    var pagerComponent = new PagerComponent($);
    var moveStatementHistory;
    beforeEach(function () {
        moveStatementHistory = new MoveStatementHistory(jQueryFake, monthPickerComponent, pagerComponent);
        spyOn(moveStatementHistory, "cacheElements").and.callThrough();
        spyOn(moveStatementHistory, "bindEvents").and.callThrough();
        spyOn(moveStatementHistory, "initComponents").and.callThrough();
        spyOn(moveStatementHistory, "onSubmitReport").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        spyOn(monthPickerComponent, "render");
        spyOn(pagerComponent, "render");
        spyOn(jQueryFake.fn, "stickyTableHeaders");
    });
    describe("_init", function () {
        beforeEach(function () {
            // act
            moveStatementHistory.init();
        });
        it("_Calls cacheElements", function () {
            // assert
            expect(moveStatementHistory.cacheElements).toHaveBeenCalled();
        });
        it("_Calls initComponents", function () {
            // assert
            expect(moveStatementHistory.initComponents).toHaveBeenCalled();
        });
        it("_Calls bindEvents", function () {
            // assert
            expect(moveStatementHistory.bindEvents).toHaveBeenCalled();
        });
        it("_Calls blockUI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Calls dateRangePickerComponent.render", function () {
            // assert
            expect(monthPickerComponent.render).toHaveBeenCalled();
        });
        it("_Calls pagerComponent.render", function () {
            // assert
            expect(pagerComponent.render).toHaveBeenCalled();
        });
        it("_Calls stickyTableHeaders", function () {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
            // act
            moveStatementHistory.cacheElements();
            // assert
            expect(moveStatementHistory.$reportForm).toBeDefined();
            expect(moveStatementHistory.$pageTitle).toBeDefined();
            expect(moveStatementHistory.$exportExcelForm).toBeDefined();
            expect(moveStatementHistory.$isFilterVisible).toBeDefined();
        });
    });
    describe("_onExportExcel_Always", function () {
        beforeEach(function () {
            // arrange
            moveStatementHistory.cacheElements();
            spyOn(moveStatementHistory.$exportExcelForm, "submit");
            // act
            moveStatementHistory.onExportExcel();
        });
        it("_Submits Export Excel Form", function () {
            // assert
            expect(moveStatementHistory.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", function () {
        it("_Always -> Blocks UI; Unblock UI;", function () {
            // arrange
            moveStatementHistory.init();
            // act
            moveStatementHistory.onSubmitReport();
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=movestatementhistory-test.js.map