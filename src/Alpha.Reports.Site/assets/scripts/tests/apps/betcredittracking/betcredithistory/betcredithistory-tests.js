describe("BetCreditHistory", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var pagerComponent = new PagerComponent($);
    var betCreditHistory;
    beforeEach(function () {
        betCreditHistory = new BetCreditHistory(jQueryFake, dateRangePickerComponent, pagerComponent);
        spyOn(betCreditHistory, "cacheElements").and.callThrough();
        spyOn(betCreditHistory, "bindEvents").and.callThrough();
        spyOn(betCreditHistory, "initComponents").and.callThrough();
        spyOn(betCreditHistory, "onSubmitReport").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        spyOn(dateRangePickerComponent, "render");
        spyOn(pagerComponent, "render");
        spyOn(jQueryFake.fn, "stickyTableHeaders");
        setFixtures("<body>\n                        <form id=\"report-form\" role=\"form\">\n                            <div class=\"form-group\">\n                                <label>Member <span style=\"color: #E54748;\">*</span></label>\n                                <input type=\"text\" style=\"width: 200px;\" autocomplete=\"off\" id=\"txt-member\" name=\"Member\" value=\"test\" />\n                            </div>\n\n                            <div class=\"form-group\">\n                                <label>Refno</label>\n                                <input type=\"text\" autocomplete=\"off\" name=\"RefNo\" value=\"12345\" id=\"txt-refno\"/>\n                            </div>\n                        </form>\n                    </body>");
    });
    describe("_init", function () {
        beforeEach(function () {
            // act
            betCreditHistory.init();
        });
        it("_Calls cacheElements", function () {
            // assert
            expect(betCreditHistory.cacheElements).toHaveBeenCalled();
        });
        it("_Calls initComponents", function () {
            // assert
            expect(betCreditHistory.initComponents).toHaveBeenCalled();
        });
        it("_Calls bindEvents", function () {
            // assert
            expect(betCreditHistory.bindEvents).toHaveBeenCalled();
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
            betCreditHistory.cacheElements();
            // assert
            expect(betCreditHistory.$reportForm).toBeDefined();
            expect(betCreditHistory.$pageTitle).toBeDefined();
            expect(betCreditHistory.$exportExcelForm).toBeDefined();
            expect(betCreditHistory.$msgError).toBeDefined();
            expect(betCreditHistory.$isFilterVisible).toBeDefined();
        });
    });
    describe("_onExportExcel_Always", function () {
        beforeEach(function () {
            // arrange
            betCreditHistory.cacheElements();
            spyOn(betCreditHistory.$exportExcelForm, "submit");
            // act
            betCreditHistory.onExportExcel();
        });
        it("_Submits Export Excel Form", function () {
            // assert
            expect(betCreditHistory.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", function () {
        it("_Always -> Blocks UI; Unblock UI;", function () {
            // arrange
            betCreditHistory.init();
            var event = { preventDefault: jasmine.createSpy("preventDefault", function () { return null; }) };
            // act
            betCreditHistory.onSubmitReport(event);
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=betcredithistory-tests.js.map