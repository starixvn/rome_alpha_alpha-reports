describe("SMAStatement", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const pagerComponent = new ClientPagerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const topWindow = {};

    let smaStatementView: SMAStatementView;

    beforeEach(() => {
        smaStatementView = new SMAStatementView(jQueryFake, dateRangePickerComponent, pagerComponent, topWindow, linkBuilderComponent);
    });

    it("Inherit StatementView_Always_SMAStatementView is StatementView", () => {
        // assert
        expect(smaStatementView instanceof StatementView).toBeTruthy();
    });
    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(StatementView.prototype, "init");

            // act
            smaStatementView.init();
        });
        it("_Calls blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Calls super.init", () => {
            // assert
            expect(StatementView.prototype.init).toHaveBeenCalled();
        });
    });
});