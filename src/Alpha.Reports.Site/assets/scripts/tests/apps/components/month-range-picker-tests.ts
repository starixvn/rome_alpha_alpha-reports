describe("MonthRangePickerComponent", () => {
    let monthRangePickerComponent: MonthRangePickerComponent;
    let template = {
        monthRange: `<body>
                    <input id="from-date" value="" />
                    <input id="to-date" value="" />
                    <div id="from-monthpicker"
                        data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                        data-mindate="01/01/2013"
                        data-maxdate="3/1/2017"
                        data-dateinmonth="1/1/2017"
                        data-currentdate="3/1/2017"></div>
                    <div id="to-monthpicker"
                        data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                        data-mindate="01/01/2013"
                        data-maxdate="3/1/2017"
                        data-dateinmonth="1/1/2017"
                        data-currentdate="3/1/2017"></div>
                </body>`,
        lessThan3Months: `<body>
                            <input id="from-date" value="3/1/2017" />
                            <input id="to-date" value="4/1/2017" />
                            <div id="from-monthpicker"
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-maxdate="3/1/2017"
                                data-dateinmonth="1/1/2017"
                                data-currentdate="3/1/2017"></div>
                            <div id="to-monthpicker"
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-maxdate="3/1/2017"
                                data-dateinmonth="1/1/2017"
                                data-currentdate="3/1/2017"></div>
                        </body>`,
        equal3Months: `<body>
                            <input id="from-date" value="3/1/2017" />
                            <input id="to-date" value="5/1/2017" />
                            <div id="from-monthpicker"
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-maxdate="3/1/2017"
                                data-dateinmonth="1/1/2017"
                                data-currentdate="3/1/2017"></div>
                            <div id="to-monthpicker"
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-maxdate="3/1/2017"
                                data-dateinmonth="1/1/2017"
                                data-currentdate="3/1/2017"></div>
                        </body>`,
        moreThan3Months: `<body>
                            <input id="from-date" value="3/1/2017" />
                            <input id="to-date" value="6/1/2017" />
                            <div id="from-monthpicker"
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-maxdate="3/1/2017"
                                data-dateinmonth="1/1/2017"
                                data-currentdate="3/1/2017"></div>
                            <div id="to-monthpicker"
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-maxdate="3/1/2017"
                                data-dateinmonth="1/1/2017"
                                data-currentdate="3/1/2017"></div>
                        </body>`,
    };

    beforeEach(() => {
        monthRangePickerComponent = new MonthRangePickerComponent($);
    });

    describe("_render", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.monthRange);
            spyOn(monthRangePickerComponent, "renderDropdown");

            // act
            monthRangePickerComponent.render();
        });

        it("_Always -> Calls render dropdown From", () => {
            // assert
            expect(monthRangePickerComponent.renderDropdown).toHaveBeenCalledWith("from-monthpicker");
        });
        it("_Always -> Calls render dropdown To", () => {
            // assert
            expect(monthRangePickerComponent.renderDropdown).toHaveBeenCalledWith("to-monthpicker");
        });
    });

    describe("_renderDropdown", () => {
        it("_Always -> bind monPickr to dropdown with correct options", () => {
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
    describe("_logMessage", () => {
        it("_Always -> Calls console.warn", () => {
            // arrange
            spyOn(console, "warn");
            let message = "Oops! \"Invalid Date\" is an invalid Date";

            // act
            monthRangePickerComponent.logMessage(message);

            // assert
            expect(console.warn).toHaveBeenCalledWith(message);
        });
    });

    describe("_onChange", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.monthRange);
            monthRangePickerComponent.render();
        });

        it("_Change From Month -> update FromDate", () => {
            // arrange
            let expectedValue = "03/01/2017";

            // act
            monthRangePickerComponent.$dropdownFromMonth.monPickr("setMonth", "3/1/2017");

            // assert
            expect($("#from-date").val()).toEqual(expectedValue);
        });

        it("_Change To Month -> update ToDate", () => {
            // arrange
            let expectedValue = "03/01/2017";

            // act
            monthRangePickerComponent.$dropdownToMonth.monPickr("setMonth", "3/1/2017");

            // assert
            expect($("#to-date").val()).toEqual(expectedValue);
        });
    });

    describe("_getMonthDiff", () => {
        let getMonthDiff = (fromDate: Date, toDate: Date, expectedDiff: number) => {
            // act
            let actualDiff = monthRangePickerComponent.getMonthDiff(fromDate, toDate);

            // arrange
            expect(actualDiff).toEqual(expectedDiff);
        };

        it("_FromDate and ToDate in the same year -> retunrs correct difference", () => {
            let fromDate = new Date("3/1/2017");
            let toDate = new Date("5/1/2017");
            let expectedDiff = 2;

            getMonthDiff(fromDate, toDate, expectedDiff);
        });
        it("_FromDate and ToDate in the difference years -> retunrs correct difference", () => {
            let fromDate = new Date("3/1/2016");
            let toDate = new Date("5/1/2017");
            let expectedDiff = 14;

            getMonthDiff(fromDate, toDate, expectedDiff);
        });
    });

    describe("_isLeast3Month", () => {
        it("_Less than 3 months -> Returns False", () => {
            // arrange
            setFixtures(template.lessThan3Months);
            monthRangePickerComponent.render();

            // act
            let actualValue = monthRangePickerComponent.isLeast3Month();

            // assert
            expect(actualValue).toBeFalsy();
        });
        it("_Equal 3 months -> Returns True", () => {
            // arrange
            setFixtures(template.equal3Months);
            monthRangePickerComponent.render();

            // act
            let actualValue = monthRangePickerComponent.isLeast3Month();

            // assert
            expect(actualValue).toBeTruthy();
        });
        it("_More than 3 months -> Returns True", () => {
            // arrange
            setFixtures(template.moreThan3Months);
            monthRangePickerComponent.render();

            // act
            let actualValue = monthRangePickerComponent.isLeast3Month();

            // assert
            expect(actualValue).toBeTruthy();
        });
    });
});