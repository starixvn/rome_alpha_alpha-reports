describe("StatementDetail", function () {
    var jQueryFake = $;
    var statementDetailView;
    var template = {
        normal: "<body>\n                    <div id=\"icon-excel\"></div>\n                    <div id=\"tbl-report\"></div>\n                 </body>"
    };
    beforeEach(function () {
        statementDetailView = new StatementDetailView(jQueryFake, null);
    });
    describe("_init", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(statementDetailView, "cacheElements").and.callThrough();
            spyOn(statementDetailView, "registerEvents");
            // act
            statementDetailView.init();
        });
        it("_Calls cacheElements", function () {
            // assert
            expect(statementDetailView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls registerEvents", function () {
            // assert
            expect(statementDetailView.registerEvents).toHaveBeenCalled();
        });
        it("_Calls blockUI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
            // arrange
            setFixtures(template.normal);
            // act
            statementDetailView.cacheElements();
            // assert
            expect(statementDetailView.$exportExcelForm).toBeDefined();
        });
    });
    describe("_registerEvents", function () {
        it("_Bind onExportExcel", function () {
            // arrange
            setFixtures(template.normal);
            statementDetailView.cacheElements();
            spyOn(statementDetailView, "onExportExcel");
            statementDetailView.registerEvents();
            // act
            statementDetailView.$("#icon-excel").trigger("click");
            // assert
            expect(statementDetailView.onExportExcel).toHaveBeenCalled();
        });
    });
    describe("_onExportExcel", function () {
        it("_Bind onExportExcel", function () {
            // arrange
            setFixtures(template.normal);
            statementDetailView.cacheElements();
            spyOn(jQueryFake.fn, "submit");
            // act
            statementDetailView.onExportExcel();
            // assert
            expect(jQueryFake.fn.submit).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=statement-details-tests.js.map