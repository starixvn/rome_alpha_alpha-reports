describe("AlphaReports", function () {
    var alphaReports;
    var jQueryFake = $;
    var storeFake = store;
    beforeEach(function () {
        alphaReports = new AlphaReports(jQueryFake, storeFake);
    });
    describe("_format", function () {
        it("_Always -> Uses correct format", function () {
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
    describe("_splitCell", function () {
        it("_Always -> Splits corresponding columns; Not Split other columns; Appends columns before assigned columns", function () {
            // arrange
            setFixtures(coreTestsTemplate.tableWithCombinedColumns);
            var expectedSplittedTableBody = $(coreTestsTemplate.tableWithSplittedColumns).html().replace(/[ \r\n]/g, "");
            // act
            alphaReports.splitCell("column-a");
            var actualSplittedTableBody = $("table").html().replace(/[ \r\n]/g, "");
            // assert
            expect(actualSplittedTableBody).toEqual(expectedSplittedTableBody);
        });
    });
    describe("_combineConsecutiveCells", function () {
        it("_Combines 1 column -> Does nothing", function () {
            // arrange
            setFixtures(coreTestsTemplate.tableNeedCombinedColumns);
            var expectedTableBody = $(coreTestsTemplate.tableNeedCombinedColumns).html().replace(/[ \r\n]/g, "");
            // act
            alphaReports.combineConsecutiveCells("column-a", 1, "");
            var actualTableBody = $("table").html().replace(/[ \r\n]/g, "");
            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
        it("_Combines 2+ columns -> Appends combined column to last cell", function () {
            // arrange
            setFixtures(coreTestsTemplate.tableNeedCombinedColumns);
            var expectedTableBody = $(coreTestsTemplate.tableAfterCombinedColumns).html().replace(/[ \r\n]/g, "");
            var combinedColumn = $("table .column-1");
            // act
            alphaReports.combineConsecutiveCells(combinedColumn, 3, "");
            var actualTableBody = $("table").html().replace(/[ \r\n]/g, "");
            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
    });
    describe("_localStorage.get", function () {
        it("_Enable store -> Gets data from store", function () {
            // arrange
            alphaReports.store.enabled = true;
            alphaReports.localStorage.set("SomeKey", "SomeValue");
            spyOn(jQueryFake, "cookie");
            // act
            var actualValue = alphaReports.localStorage.get("SomeKey");
            // assert
            expect(actualValue).toEqual("SomeValue");
            expect(jQueryFake.cookie).not.toHaveBeenCalled();
        });
        it("_Disable store -> Gets data from cookie", function () {
            // arrange
            alphaReports.store.enabled = false;
            alphaReports.localStorage.set("SomeKey", "SomeValue");
            spyOn(jQueryFake, "cookie").and.callFake(function () { return "SomeValue"; });
            // act
            var actualValue = alphaReports.localStorage.get("SomeKey");
            // assert
            expect(jQueryFake.cookie).toHaveBeenCalled();
            expect(actualValue).toEqual("SomeValue");
        });
    });
    describe("_localStorage.set", function () {
        it("_Enable store -> Sets data to store", function () {
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
        it("_Disable store -> Sets data to cookie", function () {
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
    describe("_localStorage.getCookieExpireOptions", function () {
        it("_IE -> Returns expire days", function () {
            // arrange
            spyOn(alphaReports, "isIe").and.callFake(function () { return true; });
            var expectedCookieExpireOptions = { expires: 365 };
            // act
            var actualCookieExpireOptions = alphaReports.localStorage.getCookieExpireOptions();
            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
        it("_Not IE -> Returns expire days and path", function () {
            // arrange
            spyOn(alphaReports, "isIe").and.callFake(function () { return false; });
            var expectedCookieExpireOptions = { expires: 365, path: window.location.pathname };
            // act
            var actualCookieExpireOptions = alphaReports.localStorage.getCookieExpireOptions();
            // assert
            expect(actualCookieExpireOptions).toEqual(expectedCookieExpireOptions);
        });
    });
    // isIE: can't be tested
    // window.navigator.userAgent: can't be spied
    describe("_replaceQueryStringValue", function () {
        it("_Url Already Had Param -> Replaces Value", function () {
            // arrange
            var queryString = "http://demo?ParamName1=ParamValue1&ParamName=ParamValue";
            var expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName=NewParamValue";
            // act
            var actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName", "NewParamValue");
            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });
        it("_Url Did Not Have Param -> Appends New Param", function () {
            // arrange
            var queryString = "http://demo?ParamName1=ParamValue1";
            var expectNewQueryString = "http://demo?ParamName1=ParamValue1&ParamName2=ParamValue2";
            // act
            var actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });
        it("_Url Did Not Have Query String -> Appends New Param With Question Mark", function () {
            // arrange
            var queryString = "http://demo";
            var expectNewQueryString = "http://demo?ParamName2=ParamValue2";
            // act
            var actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual(expectNewQueryString);
        });
        it("_Url Is Null -> Return String empty", function () {
            // arrange
            var queryString = null;
            // act
            var actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual("");
        });
        it("_Url Is Undifined -> Return String empty", function () {
            // arrange
            var queryString;
            // act
            var actualNewQueryString = alphaReports.replaceQueryStringValue(queryString, "ParamName2", "ParamValue2");
            // assert
            expect(actualNewQueryString).toEqual("");
        });
    });
    describe("_isIe", function () {
        var setUserAgent = function (window, userAgent) {
            if (window.navigator.userAgent !== userAgent) {
                var userAgentProp = { get: function () { return userAgent; } };
                try {
                    Object.defineProperty(window.navigator, "userAgent", userAgentProp);
                }
                catch (e) {
                    window.navigator = Object.create(navigator, {
                        userAgent: userAgentProp
                    });
                }
            }
        };
        it("_MSIE -> Returns true", function () {
            // arrange
            setUserAgent(window, "Browser MSIE ");
            // act
            var actualIsIE = alphaReports.isIe();
            // assert
            expect(actualIsIE).toBeTruthy();
        });
        it("_Trident -> Returns true", function () {
            // arrange
            setUserAgent(window, "Browser Trident.*rv:11.");
            // act
            var actualIsIE = alphaReports.isIe();
            // assert
            expect(actualIsIE).toBeTruthy();
        });
    });
    describe("_correctEvenRowClass", function () {
        it("_Always -> Correct table rows", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"tbl-report\">\n                                <tbody>\n                                    <tr class=\"even-row\"><td>Row 1</td></tr>\n                                    <tr><td>Row 2</td></tr>\n                                    <tr><td>Row 3</td></tr>\n                                    <tr class=\"even-row\"><td>Row 4</td></tr>\n                                </tbody>\n                            </table>\n                        </body>");
            var $tableReport = $("#tbl-report");
            var expectedTableBody = "<tr class=\"\"><td>Row 1</td></tr>\n                                    <tr class=\"even-row\"><td>Row 2</td></tr>\n                                    <tr><td>Row 3</td></tr>\n                                    <tr class=\"even-row\"><td>Row 4</td></tr>";
            // act
            alphaReports.correctEvenRowClass($tableReport);
            var actualTableBody = $.trim($tableReport.find("tbody").html());
            // assert
            expect(actualTableBody).toEqual(expectedTableBody);
        });
    });
    describe("_convertAttributeToBool", function () {
        var convertAttributeToBoolTestCase = function (attributeValue, expectedValue) {
            // act
            var actualValue = alphaReports.convertAttributeToBool(attributeValue);
            // assert
            expect(actualValue).toEqual(expectedValue);
        };
        it("_Null Attribute -> Returns False", function () {
            convertAttributeToBoolTestCase(null, false);
        });
        it("_Empty Value -> Returns False", function () {
            convertAttributeToBoolTestCase("", false);
        });
        it("_Not True Value -> Returns False", function () {
            convertAttributeToBoolTestCase("False", false);
            convertAttributeToBoolTestCase(" ", false);
            convertAttributeToBoolTestCase("  ", false);
        });
        it("_True Value -> Returns True", function () {
            convertAttributeToBoolTestCase("true", true);
            convertAttributeToBoolTestCase("True", true);
        });
    });
});
//# sourceMappingURL=core-tests.js.map