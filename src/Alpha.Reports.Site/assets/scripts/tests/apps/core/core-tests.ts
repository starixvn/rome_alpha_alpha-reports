describe("AlphaReports_", () => {
    let alphaReports: AlphaReports;
    const jQueryFake = $;
    const storeFake = store;

    beforeEach(() => {
        alphaReports = new AlphaReports(jQueryFake, storeFake);
    });
    describe("format", () => {
        it("Always -> Uses correct format", () => {
            // arrange
            var expectedFormat = {
                monthFormat: "MMMM - yyyy",
                shortDateWithSlash_DatePicker: "MM/DD/YYYY",
                shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
            };

            // act
            var actualFormat = alphaReports.format;

            // assert
            expect(actualFormat).toEqual(expectedFormat);
        });
    });

    // screenSmallThan768: can't be tested
    // screen.width: IE, can't be spied
    // browser used to run test is IE => not call to this.$(window).width()

    describe("splitCell", () => {
        it("Always -> Splits corresponding columns; Not Split other columns; Appends columns before assigned columns", () => {
            // arrange
            setFixtures(coreTestsTemplate.tableWithCombinedColumns);
            let expectedSplittedTableBody = $(coreTestsTemplate.tableWithSplittedColumns).html().replace(/[ \r\n]/g, "");

            // act
            alphaReports.splitCell("column-a");
            let actualSplittedTableBody = $('table').html().replace(/[ \r\n]/g, "");

            // assert
            expect(actualSplittedTableBody).toEqual(expectedSplittedTableBody);
        });
    });
    describe("combineConsecutiveCells", () => {
        it("Combines 1 column -> Does nothing", () => {
            // arrange
            setFixtures(coreTestsTemplate.tableNeedCombinedColumns);
            let expectedTableBody = $(coreTestsTemplate.tableNeedCombinedColumns).html().replace(/[ \r\n]/g, "");

            // act
            alphaReports.combineConsecutiveCells("column-a", 1, "");
            let actualTableBody = $('table').html().replace(/[ \r\n]/g, "");

            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
        it("Combines 2+ columns -> Appends combined column to last cell", () => {
            console.log("Combines 2+ columns");
            // arrange
            setFixtures(coreTestsTemplate.tableNeedCombinedColumns);
            let expectedTableBody = $(coreTestsTemplate.tableAfterCombinedColumns).html().replace(/[ \r\n]/g, "");
            let combinedColumn = $('table .column-1');

            // act
            alphaReports.combineConsecutiveCells(combinedColumn, 3, "");
            let actualTableBody = $('table').html().replace(/[ \r\n]/g, "");

            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
    });
    describe("localStorage.get", () => {
        it("Enable store -> Gets data from store", () => {
            // arrange
            alphaReports.store.enable = true;
            alphaReports.localStorage.set("SomeKey", "SomeValue");
            spyOn(jQueryFake, "cookie");

            // act
            var actualValue = alphaReports.localStorage.get("SomeKey");

            // assert
            expect(actualValue).toEqual("SomeValue");
            expect(jQueryFake.cookie).not.toHaveBeenCalled();
        });
        it("Disable store -> Gets data from cookie", () => {
            // arrange
            alphaReports.store.enable = false;
            alphaReports.localStorage.set("SomeKey", "SomeValue");
            spyOn(jQueryFake, "cookie").and.callThrough();

            // act
            var actualValue = alphaReports.localStorage.get("SomeKey");

            // assert
            expect(alphaReports.$.cookie).toHaveBeenCalled();
            expect(actualValue).toEqual("SomeValue");
        });
    });
});