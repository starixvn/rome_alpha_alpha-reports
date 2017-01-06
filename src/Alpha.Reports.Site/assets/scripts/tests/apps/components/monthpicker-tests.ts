describe("MonthPickerComponent_", () => {
    let monthPickerComponent: MonthPickerComponent;

    beforeEach(() => {
        monthPickerComponent = new MonthPickerComponent($);
    });

    describe("render", () => {
        it("Invalid Date -> Log Warning Message", () => {
            // arrange
            setFixtures(`<body>
                            <input id="from-date" value="" />
                            <input id="to-date" value="" />
                            <div id="dropdown-month"  
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-dateinmonth="0/1/2017"
                                data-fromdate="1"
                                data-todate="3"
                                data-currentdate="3/1/2017"></div>
                         </body>`);
            spyOn(console, "warn");

            // act
            monthPickerComponent.render();

            // assert
            expect(console.warn).toHaveBeenCalledWith("Oops! \"Invalid Date\" is an invalid Date");
        });
    });
    describe("onChange ", () => {
        it("Always -> Update textbox FromDate/ToDate", () => {
            // arrange
            setFixtures(`<body>
                            <input id="from-date" value="" />
                            <input id="to-date" value="" />
                            <div id="dropdown-month"  
                                data-longmonths-inyear="Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                                data-mindate="01/01/2013"
                                data-dateinmonth="1/1/2017"
                                data-fromdate="1"
                                data-todate="3"
                                data-currentdate="3/1/2017"></div>
                         </body>`);
            var dropdownMonth = monthPickerComponent.render();

            // act
            dropdownMonth.monPickr("setFromDay", 1);
            dropdownMonth.monPickr("setToDay", 2);

            // assert
            expect($("#from-date").val()).toEqual("01/01/2017");
            expect($("#to-date").val()).toEqual("01/02/2017");
        });
    });
});