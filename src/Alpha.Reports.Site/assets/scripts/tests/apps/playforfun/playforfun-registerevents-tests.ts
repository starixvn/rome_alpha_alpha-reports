describe("PlayForFun", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);
    const statusSelectionComponent = new SingleSelectionDropdownComponent($);
    const productSelectionComponent = new SingleSelectionDropdownComponent($);
    let playForFunView: PlayForFunView;
    let template = {
        report: `<body>
                    <div class="page-title"></div>
                    <form id="report-form"></form>
                    <div id="tbl-report"></div>
                    <div class="icon-filter"></div>
                    <div id="icon-excel"></div>
                    <input id="IsFilterVisible" type="hidden" />
                    <form id="export-excel-form"></form>
                 </body>`
    };

    beforeEach(() => {
        playForFunView = new PlayForFunView(
            jQueryFake,
            dateRangePickerComponent,
            sortComponent,
            productSelectionComponent,
            statusSelectionComponent,
            linkBuilderComponent);
    });

    describe("_registerEvents_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.report);
            playForFunView.cacheElements();

            spyOn(playForFunView, "onSubmitReport");
            spyOn(playForFunView, "onExportExcel");
            spyOn(PlayForFunView.prototype, "registerEvents").and.callThrough();

            // act
            playForFunView.registerEvents();
        });
        it("_Calls super.registerEvents", () => {
            expect(PlayForFunView.prototype.registerEvents).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", () => {
            // act
            playForFunView.$("#icon-excel").trigger("click");

            // assert
            expect(playForFunView.onExportExcel).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", () => {
            // act
            playForFunView.$reportForm.trigger("submit");

            // assert
            expect(playForFunView.onSubmitReport).toHaveBeenCalled();
        });
    });
});
