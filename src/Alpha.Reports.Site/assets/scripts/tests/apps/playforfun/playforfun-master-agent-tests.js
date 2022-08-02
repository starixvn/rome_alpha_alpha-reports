describe("PlayForFunMasterAgent", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var sortComponent = new SortComponent($, linkBuilderComponent);
    var statusSelectionComponent = new SingleSelectionDropdownComponent($);
    var productSelectionComponent = new SingleSelectionDropdownComponent($);
    var playForFunView;
    beforeEach(function () {
        playForFunView = new PlayForFunMasterAgentView(jQueryFake, dateRangePickerComponent, sortComponent, productSelectionComponent, statusSelectionComponent, linkBuilderComponent);
    });
    it("Inherit PlayForFunView_Always_PlayForFunMemberView is BaseReport", function () {
        // assert
        expect(playForFunView instanceof PlayForFunView).toBeTruthy();
    });
    describe("_getSorterHeader", function () {
        it("_Always_Returns Correct Sorter Header", function () {
            // arrange
            var expectedSorterHeader = { 0: { sorter: "text" } };
            // act
            var sorterHeader = playForFunView.getSorterHeader();
            // assert
            expect(sorterHeader).toEqual(expectedSorterHeader);
        });
    });
});
//# sourceMappingURL=playforfun-master-agent-tests.js.map