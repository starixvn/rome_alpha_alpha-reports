describe("AlphaReports", () => {
    let alphaReports: AlphaReports;
    const jQueryFake = $;
    const storeFake = store;

    beforeEach(() => {
        alphaReports = new AlphaReports(jQueryFake, storeFake);
    });
    describe("_format", () => {
        it("_Always -> Uses correct format", () => {
            // arrange
            let expectedFormat = {
                monthFormat: "MMMM - yyyy",
                shortDateWithSlash_DatePicker: "MM/DD/YYYY",
                shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
            };

            // act
            let actualFormat = alphaReports.format;

            // assert
            expect(actualFormat).toEqual(expectedFormat);
        });
    });

    // screenSmallThan768: can't be tested
    // screen.width: IE, can't be spied
    // browser used to run test is IE => not call to this.$(window).width()

    describe("_splitCell", () => {
        it("_Always -> Splits corresponding columns; Not Split other columns; Appends columns before assigned columns", () => {
            // arrange
            setFixtures(coreTestsTemplate.tableWithCombinedColumns);
            let expectedSplittedTableBody = $(coreTestsTemplate.tableWithSplittedColumns).html().replace(/[ \r\n]/g, "");

            // act
            alphaReports.splitCell("column-a");
            let actualSplittedTableBody = $("table").html().replace(/[ \r\n]/g, "");

            // assert
            expect(actualSplittedTableBody).toEqual(expectedSplittedTableBody);
        });
    });
    describe("_combineConsecutiveCells", () => {
        it("_Combines 1 column -> Does nothing", () => {
            // arrange
            setFixtures(coreTestsTemplate.tableNeedCombinedColumns);
            let expectedTableBody = $(coreTestsTemplate.tableNeedCombinedColumns).html().replace(/[ \r\n]/g, "");

            // act
            alphaReports.combineConsecutiveCells("column-a", 1, "");
            let actualTableBody = $("table").html().replace(/[ \r\n]/g, "");

            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
        it("_Combines 2+ columns -> Appends combined column to last cell", () => {
            // arrange
            setFixtures(coreTestsTemplate.tableNeedCombinedColumns);
            let expectedTableBody = $(coreTestsTemplate.tableAfterCombinedColumns).html().replace(/[ \r\n]/g, "");
            let combinedColumn = $("table .column-1");

            // act
            alphaReports.combineConsecutiveCells(combinedColumn, 3, "");
            let actualTableBody = $("table").html().replace(/[ \r\n]/g, "");

            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
    });
    describe("_localStorage.get", () => {
        it("_Enable store -> Gets data from store", () => {
            // arrange
            alphaReports.store.enabled = true;
            alphaReports.localStorage.set("SomeKey", "SomeValue");
            spyOn(jQueryFake, "cookie");

            // act
            let actualValue = alphaReports.localStorage.get("SomeKey");

            // assert
            expect(actualValue).toEqual("SomeValue");
            expect(jQueryFake.cookie).not.toHaveBeenCalled();
        });
        it("_Disable store -> Gets data from cookie", () => {
            // arrange
            alphaReports.store.enabled = false;
            alphaReports.localStorage.set("SomeKey", "SomeValue");
            spyOn(jQueryFake, "cookie").and.callFake(() => "SomeValue");

            // act
            let actualValue = alphaReports.localStorage.get("SomeKey");

            // assert
            expect(jQueryFake.cookie).toHaveBeenCalled();
            expect(actualValue).toEqual("SomeValue");
        });
    });
    describe("_localStorage.set", () => {
        it("_Enable store -> Sets data to store", () => {
            // arrange
            alphaReports.store.enabled = true;
            spyOn(storeFake, "set");
            spyOn(jQueryFake, "cookie");

            // act
            alphaReports.localStorage.set("SomeKey", "SomeValue");

            // assert
            expect(storeFake.set).toHaveBeenCalled();
            expect(jQueryFake.cookie).not.toHaveBeenCalled();
        });
        it("_Disable store -> Sets data to cookie", () => {
            // arrange
            alphaReports.store.enabled = false;
            spyOn(storeFake, "set");
            spyOn(jQueryFake, "cookie");

            // act
            alphaReports.localStorage.set("SomeKey", "SomeValue");

            // assert
            expect(jQueryFake.cookie).toHaveBeenCalled();
            expect(storeFake.set).not.toHaveBeenCalled();
        });
    });
    describe("_localStorage.getCookieExpireOptions", () => {
        it("_IE -> Returns expire days", () => {
            // arrange
            spyOn(alphaReports, "isIe").and.callFake(() => true);
            let expectedCookieExpireOptions = { expires: 365 };

            // act
            let actualCookieExpireOptions = alphaReports.localStorage.getCookieExpireOptions();

            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
        it("_Not IE -> Returns expire days and path", () => {
            // arrange
            spyOn(alphaReports, "isIe").and.callFake(() => false);
            let expectedCookieExpireOptions = { expires: 365, path: window.location.pathname };

            // act
            let actualCookieExpireOptions = alphaReports.localStorage.getCookieExpireOptions();

            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
    });

    // isIE: can't be tested
    // window.navigator.userAgent: can't be spied
    describe("_replaceQueryStringValue", () => {
        it("_Url Already Had Param -> Replaces Value", () => {
            // arrange
            let queryString = "http://demo?ParamName1=ParamValue1&ParamName=ParamValue";
            let expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName=NewParamValue";

            // act
            let actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName", "NewParamValue");

            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });

        it("_Url Did Not Have Param -> Appends New Param", () => {
            // arrange
            let queryString = "http://demo?ParamName1=ParamValue1";
            let expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName2=ParamValue2";

            // act
            let actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });

        it("_Url Did Not Have Query String -> Appends New Param With Question Mark", () => {
            // arrange
            let queryString = "http://demo";
            let expectNewQueryString = "http://demo?ParamName2=ParamValue2";

            // act
            let actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });

        it("_Url Is Null -> Return String empty", () => {
            // arrange
            let queryString = null;

            // act
            let actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual("");
        });

        it("_Url Is Undifined -> Return String empty", () => {
            // arrange
            let queryString;

            // act
            let actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");

            // assert
            expect(actualNewQueryString).toEqual("");
        });
    });

    describe("_isIe", () => {
        let setUserAgent = (window: any, userAgent: string) => {
            if (window.navigator.userAgent !== userAgent) {
                let userAgentProp = { get: function () { return userAgent; } };
                try {
                    Object.defineProperty(window.navigator, "userAgent", userAgentProp);
                } catch (e) {
                    window.navigator = Object.create(navigator, {
                        userAgent: userAgentProp
                    });
                }
            }
        };

        it("_MSIE -> Returns true", () => {
            // arrange
            setUserAgent(window, "Browser MSIE ");

            // act
            let actualIsIE = alphaReports.isIe();

            // assert
            expect(actualIsIE).toBeTruthy();
        });
        it("_Trident -> Returns true", () => {
            // arrange
            setUserAgent(window, "Browser Trident.*rv:11.");

            // act
            let actualIsIE = alphaReports.isIe();

            // assert
            expect(actualIsIE).toBeTruthy();
        });
    });

    describe("_correctEvenRowClass", () => {
        it("_Always -> Correct table rows", () => {
            // arrange
            setFixtures(`<body>
                            <table id="tbl-report">
                                <tbody>
                                    <tr class="even-row"><td>Row 1</td></tr>
                                    <tr><td>Row 2</td></tr>
                                    <tr><td>Row 3</td></tr>
                                    <tr class="even-row"><td>Row 4</td></tr>
                                </tbody>
                            </table>
                        </body>`);
            let $tableReport = $("#tbl-report");
            let expectedTableBody = `<tr class=""><td>Row 1</td></tr>
                                    <tr class="even-row"><td>Row 2</td></tr>
                                    <tr><td>Row 3</td></tr>
                                    <tr class="even-row"><td>Row 4</td></tr>`;
            // act
            alphaReports.correctEvenRowClass($tableReport);
            let actualTableBody = $.trim($tableReport.find("tbody").html());

            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
    });
});