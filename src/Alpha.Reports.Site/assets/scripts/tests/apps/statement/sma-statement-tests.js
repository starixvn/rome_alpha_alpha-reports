describe("SMAStatement", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var pagerComponent = new ClientPagerComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var topWindow = {};
    var smaStatementView;
    beforeEach(function () {
        smaStatementView = new SMAStatementView(jQueryFake, dateRangePickerComponent, pagerComponent, topWindow, linkBuilderComponent);
    });
    it("Inherit StatementView_Always_SMAStatementView is StatementView", function () {
        // assert
        expect(smaStatementView instanceof StatementView).toBeTruthy();
    });
    describe("_init", function () {
        beforeEach(function () {
            // arrange
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(StatementView.prototype, "init");
            // act
            smaStatementView.init();
        });
        it("_Calls blockUI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Calls super.init", function () {
            // assert
            expect(StatementView.prototype.init).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=sma-statement-tests.js.map