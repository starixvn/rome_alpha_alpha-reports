describe("MonthPickerComponent", function () {
    var monthPickerComponent;
    beforeEach(function () {
        monthPickerComponent = new MonthPickerComponent($);
    });
    describe("_render", function () {
        it("_Invalid Date -> Log Warning Message", function () {
            // arrange
            setFixtures("<body>\n                            <input id=\"from-date\" value=\"\" />\n                            <input id=\"to-date\" value=\"\" />\n                            <div id=\"dropdown-month\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-dateinmonth=\"0/1/2017\"\n                                data-fromdate=\"1\"\n                                data-todate=\"3\"\n                                data-currentdate=\"3/1/2017\"></div>\n                         </body>");
            spyOn(console, "warn");
            // act
            monthPickerComponent.render();
            // assert
            expect(console.warn).toHaveBeenCalledWith("Oops! \"Invalid Date\" is an invalid Date");
        });
    });
    describe("_onChange", function () {
        var dropdownMonth;
        beforeEach(function () {
            // arrange
            setFixtures("<body>\n                            <input id=\"from-date\" value=\"\" />\n                            <input id=\"to-date\" value=\"\" />\n                            <div id=\"dropdown-month\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-dateinmonth=\"1/1/2017\"\n                                data-fromdate=\"1\"\n                                data-todate=\"3\"\n                                data-currentdate=\"3/1/2017\"></div>\n                         </body>");
            dropdownMonth = monthPickerComponent.render();
        });
        it("_Change From Date-> Update textbox FromDate", function () {
            // act
            dropdownMonth.monPickr("setFromDay", "1");
            // assert
            expect($("#from-date").val()).toEqual("01/01/2017");
        });
        it("_Change To Date-> Update textbox ToDate", function () {
            // act
            dropdownMonth.monPickr("setToDay", "2");
            // assert
            expect($("#to-date").val()).toEqual("01/02/2017");
        });
    });
});
//# sourceMappingURL=monthpicker-tests.js.map