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
        setFixtures("<body>\n                        <form id=\"report-form\" role=\"form\">\n                            <input id=\"IsDescendingSort\" value=\"False\" type=\"hidden\" />\n                            <input id=\"SortPropertyName\" value=\"EventInfo\" type=\"hidden\" />\n                        </form>\n                        <span class=\"icon-filter\" title=\"Hide filters\"></span>\n                        <span id=\"icon-excel\" class=\"icon-excel\" title=\"@Resources.ExportToExcel\"></span>\n                        <div class=\"page-title\"></div>\n                        <table id=\"tbl-report\" data-cansort=\"False\"\n                            data-sortedcolumn=\"Event Info\"\n                            data-descendingsort=\"False\">\n                        </table>\n                    </body>");
    });
    describe("_init", function () {
        beforeEach(function () {
            // act
            matchWinLossView.init();
        });
        it("_Calls cacheElements", function () {
            // assert
            expect(matchWinLossView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls registerEvents", function () {
            // assert
            expect(matchWinLossView.registerEvents).toHaveBeenCalled();
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
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Calls sortComponent.render", function () {
            // assert
            expect(sortComponent.render).toHaveBeenCalled();
        });
        it("_Calls sortComponent.stickyTableHeaders", function () {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
            // act
            matchWinLossView.cacheElements();
            // assert
            expect(matchWinLossView.$tableReport).toBeDefined();
            expect(matchWinLossView.$reportForm).toBeDefined();
            expect(matchWinLossView.$exportExcelForm).toBeDefined();
            expect(matchWinLossView.$isDescendingSort).toBeDefined();
            expect(matchWinLossView.$sortPropertyName).toBeDefined();
        });
    });
    describe("_onExportExcel_Always", function () {
        beforeEach(function () {
            // arrange
            matchWinLossView.cacheElements();
            spyOn(matchWinLossView.$exportExcelForm, "submit");
            // act
            matchWinLossView.onExportExcel();
        });
        it("_Submits Export Excel Form", function () {
            // assert
            expect(matchWinLossView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", function () {
        it("_Always -> Blocks UI; Unblock UI;", function () {
            // arrange
            matchWinLossView.init();
            // act
            matchWinLossView.onSubmitReport();
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onClickIconFilter", function () {
        describe("_Filter is currently shown", function () {
            it("_Hides Filter", function () {
                // arrange
                matchWinLossView.cacheElements();
                // act
                matchWinLossView.onClickIconFilter();
                // assert
                expect(matchWinLossView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(matchWinLossView.$reportForm.hasClass("hide")).toBeTruthy();
            });
        });
        describe("_Filter is currently hidden", function () {
            it("_Shows Filter", function () {
                // arrange
                setFixtures("<body>\n                                <form id=\"report-form\" role=\"form\" class=\"hide\">\n                                    <input id=\"IsDescendingSort\" value=\"False\" type=\"hidden\" />\n                                    <input id=\"SortPropertyName\" value=\"EventInfo\" type=\"hidden\" />\n                                </form>\n                                <span class=\"icon-filter hide\" title=\"Hide filters\"></span>\n                                <span id=\"icon-excel\" class=\"icon-excel\" title=\"@Resources.ExportToExcel\"></span>\n                                <div class=\"page-title hide-filter\"></div>\n                                <table id=\"tbl-report\" data-cansort=\"False\"\n                                    data-sortedcolumn=\"Event Info\"\n                                    data-descendingsort=\"False\">\n                                </table>\n                            </body>");
                matchWinLossView.cacheElements();
                // act
                matchWinLossView.onClickIconFilter();
                // assert
                expect(matchWinLossView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(matchWinLossView.$reportForm.hasClass("hide")).toBeFalsy();
            });
        });
    });
});
//# sourceMappingURL=matchwinloss-tests.js.map