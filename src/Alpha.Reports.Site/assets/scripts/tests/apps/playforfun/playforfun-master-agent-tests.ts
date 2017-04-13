describe("PlayForFunMasterAgent", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);
    const statusSelectionComponent = new SingleSelectionDropdownComponent($);
    const productSelectionComponent = new SingleSelectionDropdownComponent($);
    let playForFunView: PlayForFunMasterAgentView;

    beforeEach(() => {
        playForFunView = new PlayForFunMasterAgentView(
            jQueryFake,
            dateRangePickerComponent,
            sortComponent,
            productSelectionComponent,
            statusSelectionComponent,
            linkBuilderComponent);
    });

    it("Inherit PlayForFunView_Always_PlayForFunMemberView is BaseReport", () => {
        // assert
        expect(playForFunView instanceof PlayForFunView).toBeTruthy();
    });

    describe("_getSorterHeader", () => {
        it("_Always_Returns Correct Sorter Header", () => {
            // arrange
            let expectedSorterHeader = { 0: { sorter: "text" } };

            // act
            let sorterHeader = playForFunView.getSorterHeader();

            // assert
            expect(sorterHeader).toEqual(expectedSorterHeader);
        });
    });
});
