describe("MonthRangePickerComponent", function () {
    var monthRangePickerComponent;
    var template = {
        monthRange: "<body>\n                    <input id=\"from-date\" value=\"\" />\n                    <input id=\"to-date\" value=\"\" />\n                    <div id=\"from-monthpicker\"\n                        data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                        data-mindate=\"01/01/2013\"\n                        data-maxdate=\"3/1/2017\"\n                        data-dateinmonth=\"1/1/2017\"\n                        data-currentdate=\"3/1/2017\"></div>\n                    <div id=\"to-monthpicker\"\n                        data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                        data-mindate=\"01/01/2013\"\n                        data-maxdate=\"3/1/2017\"\n                        data-dateinmonth=\"1/1/2017\"\n                        data-currentdate=\"3/1/2017\"></div>\n                </body>",
        lessThan3Months: "<body>\n                            <input id=\"from-date\" value=\"3/1/2017\" />\n                            <input id=\"to-date\" value=\"4/1/2017\" />\n                            <div id=\"from-monthpicker\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-maxdate=\"3/1/2017\"\n                                data-dateinmonth=\"1/1/2017\"\n                                data-currentdate=\"3/1/2017\"></div>\n                            <div id=\"to-monthpicker\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-maxdate=\"3/1/2017\"\n                                data-dateinmonth=\"1/1/2017\"\n                                data-currentdate=\"3/1/2017\"></div>\n                        </body>",
        equal3Months: "<body>\n                            <input id=\"from-date\" value=\"3/1/2017\" />\n                            <input id=\"to-date\" value=\"5/1/2017\" />\n                            <div id=\"from-monthpicker\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-maxdate=\"3/1/2017\"\n                                data-dateinmonth=\"1/1/2017\"\n                                data-currentdate=\"3/1/2017\"></div>\n                            <div id=\"to-monthpicker\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-maxdate=\"3/1/2017\"\n                                data-dateinmonth=\"1/1/2017\"\n                                data-currentdate=\"3/1/2017\"></div>\n                        </body>",
        moreThan3Months: "<body>\n                            <input id=\"from-date\" value=\"3/1/2017\" />\n                            <input id=\"to-date\" value=\"6/1/2017\" />\n                            <div id=\"from-monthpicker\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-maxdate=\"3/1/2017\"\n                                data-dateinmonth=\"1/1/2017\"\n                                data-currentdate=\"3/1/2017\"></div>\n                            <div id=\"to-monthpicker\"\n                                data-longmonths-inyear=\"Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec\"\n                                data-mindate=\"01/01/2013\"\n                                data-maxdate=\"3/1/2017\"\n                                data-dateinmonth=\"1/1/2017\"\n                                data-currentdate=\"3/1/2017\"></div>\n                        </body>",
    };
    beforeEach(function () {
        monthRangePickerComponent = new MonthRangePickerComponent($);
    });
    describe("_render", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.monthRange);
            spyOn(monthRangePickerComponent, "renderDropdown");
            // act
            monthRangePickerComponent.render();
        });
        it("_Always -> Calls render dropdown From", function () {
            // assert
            expect(monthRangePickerComponent.renderDropdown).toHaveBeenCalledWith("from-monthpicker");
        });
        it("_Always -> Calls render dropdown To", function () {
            // assert
            expect(monthRangePickerComponent.renderDropdown).toHaveBeenCalledWith("to-monthpicker");
        });
    });
    describe("_renderDropdown", function () {
        it("_Always -> bind monPickr to dropdown with correct options", function () {
            // arrage
            setFixtures(template.monthRange);
            var fromDropdownId = "from-monthpicker";
            spyOn($.fn, "monPickr");
            // act
            monthRangePickerComponent.renderDropdown(fromDropdownId);
            // assert
            expect($.fn.monPickr).toHaveBeenCalledWith({
                longMonthsInYear: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                maxDate: "3/1/2017",
                minDate: "01/01/2013",
                month: new Date("1/1/2017"),
                today: new Date("3/1/2017"),
                showDateRange: false,
                monthFormat: alphaReports.format.monthFormat,
                fromDayId: "ddl-fromday",
                toDayId: "ddl-today",
                fromDayLabel: "",
                toDayLabel: "-",
                sortDesc: true,
                log: monthRangePickerComponent.logMessage,
                afterChangeMonth: monthRangePickerComponent.onChange
            });
        });
    });
    describe("_logMessage", function () {
        it("_Always -> Calls console.warn", function () {
            // arrange
            spyOn(console, "warn");
            var message = "Oops! \"Invalid Date\" is an invalid Date";
            // act
            monthRangePickerComponent.logMessage(message);
            // assert
            expect(console.warn).toHaveBeenCalledWith(message);
        });
    });
    describe("_onChange", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.monthRange);
            monthRangePickerComponent.render();
        });
        it("_Change From Month -> update FromDate", function () {
            // arrange
            var expectedValue = "03/01/2017";
            // act
            monthRangePickerComponent.$dropdownFromMonth.monPickr("setMonth", "3/1/2017");
            // assert
            expect($("#from-date").val()).toEqual(expectedValue);
        });
        it("_Change To Month -> update ToDate", function () {
            // arrange
            var expectedValue = "03/01/2017";
            // act
            monthRangePickerComponent.$dropdownToMonth.monPickr("setMonth", "3/1/2017");
            // assert
            expect($("#to-date").val()).toEqual(expectedValue);
        });
    });
    describe("_getMonthDiff", function () {
        var getMonthDiff = function (fromDate, toDate, expectedDiff) {
            // act
            var actualDiff = monthRangePickerComponent.getMonthDiff(fromDate, toDate);
            // arrange
            expect(actualDiff).toEqual(expectedDiff);
        };
        it("_FromDate and ToDate in the same year -> retunrs correct difference", function () {
            var fromDate = new Date("3/1/2017");
            var toDate = new Date("5/1/2017");
            var expectedDiff = 2;
            getMonthDiff(fromDate, toDate, expectedDiff);
        });
        it("_FromDate and ToDate in the difference years -> retunrs correct difference", function () {
            var fromDate = new Date("3/1/2016");
            var toDate = new Date("5/1/2017");
            var expectedDiff = 14;
            getMonthDiff(fromDate, toDate, expectedDiff);
        });
    });
    describe("_isLeast3Month", function () {
        it("_Less than 3 months -> Returns False", function () {
            // arrange
            setFixtures(template.lessThan3Months);
            monthRangePickerComponent.render();
            // act
            var actualValue = monthRangePickerComponent.isLeast3Month();
            // assert
            expect(actualValue).toBeFalsy();
        });
        it("_Equal 3 months -> Returns True", function () {
            // arrange
            setFixtures(template.equal3Months);
            monthRangePickerComponent.render();
            // act
            var actualValue = monthRangePickerComponent.isLeast3Month();
            // assert
            expect(actualValue).toBeTruthy();
        });
        it("_More than 3 months -> Returns True", function () {
            // arrange
            setFixtures(template.moreThan3Months);
            monthRangePickerComponent.render();
            // act
            var actualValue = monthRangePickerComponent.isLeast3Month();
            // assert
            expect(actualValue).toBeTruthy();
        });
    });
});
//# sourceMappingURL=month-range-picker-tests.js.map