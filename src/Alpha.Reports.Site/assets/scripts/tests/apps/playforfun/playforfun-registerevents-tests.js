describe("PlayForFun", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var sortComponent = new SortComponent($, linkBuilderComponent);
    var statusSelectionComponent = new SingleSelectionDropdownComponent($);
    var productSelectionComponent = new SingleSelectionDropdownComponent($);
    var playForFunView;
    var template = {
        report: "<body>\n                    <div class=\"page-title\"></div>\n                    <form id=\"report-form\"></form>\n                    <div id=\"tbl-report\"></div>\n                    <div class=\"icon-filter\"></div>\n                    <div id=\"icon-excel\"></div>\n                    <input id=\"IsFilterVisible\" type=\"hidden\" />\n                    <form id=\"export-excel-form\"></form>\n                 </body>"
    };
    beforeEach(function () {
        playForFunView = new PlayForFunView(jQueryFake, dateRangePickerComponent, sortComponent, productSelectionComponent, statusSelectionComponent, linkBuilderComponent);
    });
    describe("_registerEvents_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.report);
            playForFunView.cacheElements();
            spyOn(playForFunView, "onSubmitReport");
            spyOn(playForFunView, "onExportExcel");
            spyOn(PlayForFunView.prototype, "registerEvents").and.callThrough();
            // act
            playForFunView.registerEvents();
        });
        it("_Calls super.registerEvents", function () {
            expect(PlayForFunView.prototype.registerEvents).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", function () {
            // act
            playForFunView.$("#icon-excel").trigger("click");
            // assert
            expect(playForFunView.onExportExcel).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", function () {
            // act
            playForFunView.$reportForm.trigger("submit");
            // assert
            expect(playForFunView.onSubmitReport).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=playforfun-registerevents-tests.js.map