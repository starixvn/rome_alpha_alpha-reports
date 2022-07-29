describe("MatchWinLoss", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var sortComponent = new SortComponent($, new LinkBuilderComponent($));
    var matchWinLossView;
    beforeEach(function () {
        matchWinLossView = new MatchWinLossView(jQueryFake, dateRangePickerComponent, sortComponent);
        spyOn(matchWinLossView, "cacheElements").and.callThrough();
        spyOn(matchWinLossView, "registerEvents").and.callThrough();
        spyOn(matchWinLossView, "onSubmitReport").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        spyOn(dateRangePickerComponent, "render");
        spyOn(sortComponent, "render").and.callThrough();
        spyOn(jQueryFake.fn, "stickyTableHeaders");
        setFixtures("<body>\n                        <form id=\"report-form\" role=\"form\">\n                            <input id=\"IsDescendingSort\" value=\"False\" type=\"hidden\" />\n                            <input id=\"SortPropertyName\" value=\"EventInfo\" type=\"hidden\" />\n                        </form>\n                        <span class=\"icon-filter\" title=\"Hide filters\"></span>\n                        <span id=\"icon-excel\" class=\"icon-excel\" title=\"@Resources.ExportToExcel\"></span>\n                        <table id=\"tbl-report\" data-cansort=\"False\"\n                            data-sortedcolumn=\"Event Info\"\n                            data-descendingsort=\"False\">\n                        </table>\n                    </body>");
    });
    describe("_registerEvents_Always", function () {
        beforeEach(function () {
            // arrange
            matchWinLossView.cacheElements();
            spyOn(matchWinLossView, "onExportExcel");
            spyOn(matchWinLossView, "onClickIconFilter");
            // act
            matchWinLossView.registerEvents();
        });
        it("_Bind onExportExcel", function () {
            // act
            matchWinLossView.$("#icon-excel").trigger("click");
            // assert
            expect(matchWinLossView.onExportExcel).toHaveBeenCalled();
        });
        it("_Submit functions have been registered", function () {
            // arrange
            matchWinLossView.init();
            // act
            matchWinLossView.$reportForm.trigger("submit");
            // assert
            expect(matchWinLossView.onSubmitReport).toHaveBeenCalled();
        });
        it("_Bind onClickIconFilter", function () {
            // act
            $(".icon-filter").trigger("click");
            // assert
            expect(matchWinLossView.onClickIconFilter).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=matchwinloss-registerEvents-tests.js.map