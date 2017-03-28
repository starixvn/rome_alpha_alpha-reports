describe("SortComponent", () => {
    let sortComponent: SortComponent;
    let linkBuilderComponent: LinkBuilderComponent;

    beforeEach(() => {
        linkBuilderComponent = new LinkBuilderComponent($);
        sortComponent = new SortComponent($, linkBuilderComponent);
    });

    describe("_render", () => {
        it("_Cannot sort -> Remove icon sorting from sorting column", () => {
            // arrange
            setFixtures(`<body>
                            <table id="table-report">
                                <th class="sorting" data-sortedname="Column1">Column 1</th>
                                <th class="sorting" data-sortedname="Column2">Column 2</th>
                            </table>
                        </body>`);

            // act
            sortComponent.render(new SortOption($("#table-report"), false));

            // assert
            expect($(".sorting").length).toEqual(0);
        });

        describe("_Can sort/Have all data on client", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <table id="table-report">
                                <th class="sorting" data-sortedname="Column1">Column 1</th>
                                <th class="sorting" data-sortedname="Column2">Column 2</th>
                            </table>
                        </body>`);
                spyOn(sortComponent, "bindTableSorter");
                spyOn(sortComponent, "onSort");

                // act
                sortComponent.render(new SortOption($("#table-report"), true, true));
            });

            it("_Not Remove icon sorting from sorting column", () => {
                // assert
                expect($(".sorting").length).not.toEqual(0);
            });
            it("_Bind Table Sorter", () => {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });
            it("Not bind on sort event", () => {
                // assert
                expect(sortComponent.onSort).not.toHaveBeenCalled();
            });
        });

        describe("_Can sort/Not Have all data on client/Descending Sort", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                                <table id="table-report" data-descendingsort="True" data-sortedcolumn="Column1">
                                    <th class="sorting" data-sortedname="Column1">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2">Column 2</th>
                                </table>
                            </body>`);
                spyOn(sortComponent, "bindTableSorter");
                spyOn(sortComponent, "onSort");

                // act
                sortComponent.render(new SortOption($("#table-report"), true, false));
            });
            it("_Not Remove icon sorting from sorting column", () => {
                // assert
                expect($(".sorting").length).not.toEqual(0);
            });
            it("_Bind Table Sorter", () => {
                // assert
                expect($("[data-sortedname=\"Column1\"]").hasClass("sorting-desc")).toBeTruthy();
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });
            it("_Bind on sort event", () => {
                // act
                $("[data-sortedname=\"Column2\"]").trigger("click");

                // assert
                expect(sortComponent.onSort).toHaveBeenCalled();
            });
        });

        describe("_Can sort/Not Have all data on client/Ascending Sort", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                                <table id="table-report" data-descendingsort="False" data-sortedcolumn="Column1">
                                    <th class="sorting" data-sortedname="Column1">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2">Column 2</th>
                                </table>
                            </body>`);
                spyOn(sortComponent, "bindTableSorter");
                spyOn(sortComponent, "onSort");

                // act
                sortComponent.render(new SortOption($("#table-report"), true, false));
            });
            it("_Not Remove icon sorting from sorting column", () => {
                // assert
                expect($(".sorting").length).not.toEqual(0);
            });
            it("_Bind Table Sorter", () => {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();

                expect($("[data-sortedname=\"Column1\"]").hasClass("sorting-asc")).toBeTruthy();
            });
            it("_Bind on sort event", () => {
                // act
                $("[data-sortedname=\"Column2\"]").trigger("click");

                // assert
                expect(sortComponent.onSort).toHaveBeenCalled();
            });
        });
    });
    describe("_onSort", () => {
        describe("_Switch Sort Column", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                                <table id="table-report" data-descendingsort="False" data-sortedcolumn="Column1">
                                    <th class="sorting" data-sortedname="Column1">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2">Column 2</th>
                                </table>
                            </body>`);
                sortComponent.render(new SortOption($("#table-report"), true, false, () => null));
                spyOn(sortComponent.options, "afterSortFunction");

                // act
                $("[data-sortedname=\"Column2\"]").trigger("click");
            });
            it("_Set Descending Sort", () => {
                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("1");
            });
            it("_Update Sort Column Name", () => {
                // assert
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column2");
            });
            it("_Call After Sort Function", () => {
                // assert
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        });
        describe("_Not Switch Sort Column/ Currently Ascending Sort", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                                <table id="table-report" data-descendingsort="False" data-sortedcolumn="Column1">
                                    <th class="sorting" data-sortedname="Column1">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2">Column 2</th>
                                </table>
                            </body>`);
                sortComponent.render(new SortOption($("#table-report"), true, false, () => null));
                spyOn(sortComponent.options, "afterSortFunction");

                // act
                $("[data-sortedname=\"Column1\"]").trigger("click");
            });
            it("_Set Descending Sort", () => {
                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("1");
            });
            it("_Not Change Sort Column Name", () => {
                // assert
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column1");
            });
            it("_Call After Sort Function", () => {
                // assert
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        });
        describe("_Not Switch Sort Column/Currently Descending Sort", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                                <table id="table-report" data-descendingsort="True" data-sortedcolumn="Column1">
                                    <th class="sorting" data-sortedname="Column1">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2">Column 2</th>
                                </table>
                            </body>`);
                sortComponent.render(new SortOption($("#table-report"), true, false, () => null));
                spyOn(sortComponent.options, "afterSortFunction");

                // act
                $("[data-sortedname=\"Column1\"]").trigger("click");
            });
            it("_Set Ascending Sort", () => {
                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("0");
            });
            it("_Not Change Sort Column Name", () => {
                // assert
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column1");
            });
            it("_Call After Sort Function", () => {
                // assert
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        });
    });
    describe("_getSortInfo", () => {
        it("_Descending Sort -> Returns Sort Info from table data", () => {
            // arrange
            setFixtures(`<body>
                            <table id="table-report" data-descendingsort="1" data-sortedcolumn="Column1">
                                <th class="sorting" data-sortedname="Column1">Column 1</th>
                                <th class="sorting" data-sortedname="Column2">Column 2</th>
                            </table>
                        </body>`);
            sortComponent.$sortContainer = $("#table-report");

            // act
            var actalSortInfo = sortComponent.getSortInfo();

            // assert
            expect(actalSortInfo).toEqual({
                sortPropertyName: "Column1",
                isDescendingSort: true
            });
        });

        it("_Ascending Sort -> Returns Sort Info from table data", () => {
            // arrange
            setFixtures(`<body>
                            <table id="table-report" data-descendingsort="0" data-sortedcolumn="Column1">
                                <th class="sorting" data-sortedname="Column1">Column 1</th>
                                <th class="sorting" data-sortedname="Column2">Column 2</th>
                            </table>
                        </body>`);
            sortComponent.$sortContainer = $("#table-report");

            // act
            var actalSortInfo = sortComponent.getSortInfo();

            // assert
            expect(actalSortInfo).toEqual({
                sortPropertyName: "Column1",
                isDescendingSort: false
            });
        });
    });
    describe("_bindTableSorter", () => {
        describe("_Cannot Sort", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <table id="table-report" data-descendingsort="0" data-sortedcolumn="Column1">
                                <th class="sorting" data-sortedname="Column1">Column 1</th>
                                <th class="sorting" data-sortedname="Column2">Column 2</th>
                            </table>
                        </body>`);

                sortComponent.options.canSort = false;
                sortComponent.$sortContainer = $("#table-report");
                sortComponent.options.afterSortFunction = () => null;
                spyOn($.fn, "tablesorter");
                spyOn(sortComponent.options, "afterSortFunction");

                // act
                sortComponent.bindTableSorter("Column1");
            });
            it("_Don't bind tablesorter", () => {
                // assert
                expect($.fn.tablesorter).not.toHaveBeenCalled();
            });
            it("_Don't bind event sort end", () => {
                // assert
                sortComponent.$sortContainer.trigger("sortEnd");
                expect(sortComponent.options.afterSortFunction).not.toHaveBeenCalled();
            });
        });
        describe("_Don't have all data on client", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <table id="table-report" data-descendingsort="0" data-sortedcolumn="Column1">
                                <th class="sorting" data-sortedname="Column1">Column 1</th>
                                <th class="sorting" data-sortedname="Column2">Column 2</th>
                            </table>
                        </body>`);

                sortComponent.options.canSort = true;
                sortComponent.options.hasAllDataOnClient = false;
                sortComponent.$sortContainer = $("#table-report");
                sortComponent.options.afterSortFunction = () => null;
                spyOn($.fn, "tablesorter");
                spyOn(sortComponent.options, "afterSortFunction");

                // act
                sortComponent.bindTableSorter("Column1");
            });
            it("_Don't bind tablesorter", () => {
                // assert
                expect($.fn.tablesorter).not.toHaveBeenCalled();
            });
            it("_Don't bind event sort end", () => {
                // assert
                sortComponent.$sortContainer.trigger("sortEnd");
                expect(sortComponent.options.afterSortFunction).not.toHaveBeenCalled();
            });
        });
        describe("_Can sort;Have all data on client", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <table id="table-report" data-descendingsort="0" data-sortedcolumn="Column1">
                                <th class="sorting" data-sortedname="Column1" data-colindex="0">Column 1</th>
                                <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                            </table>
                        </body>`);

                sortComponent.render(new SortOption($("#table-report"), true, true, () => null, {}));
                spyOn($.fn, "tablesorter");
                spyOn(sortComponent.options, "afterSortFunction");

                // act
                sortComponent.bindTableSorter("Column1");
            });
            it("_Bind tablesorter", () => {
                // assert
                expect($.fn.tablesorter).toHaveBeenCalledWith({
                    defaultParser: 1,
                    headers: { 1: { sorter: false } },
                    sortList: [[0, 0]],
                    textExtraction: jasmine.any(Function)
                });
            });
        });

        it("_Incorrect data", () => {
            // arrange
            setFixtures(`<body>
                            <table id="table-report" data-descendingsort="1" data-sortedcolumn="Column1">
                                <thead class="tableFloatingHeaderOriginal">
                                    <th class="sorting sorting-desc" data-sortedname="Column1" data-colindex="0">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                                </thead>
                            </table>
                        </body>`);

            sortComponent.render(new SortOption($("#table-report"), true, true, () => null, {}));
            spyOn(sortComponent, "onSort");
            spyOn(console, "log");

            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");

            // assert
            expect(console.log).toHaveBeenCalled();
        });
    });
    describe("_Had Data On Client", () => {
        it("_Table sorter sorted descending column on normal mode => onSort was called with descending sort", () => {
            // arrange
            setFixtures(`<body>
                            <table id="table-report" data-descendingsort="1" data-sortedcolumn="Column1">
                                <thead class="tableFloatingHeaderOriginal">
                                    <th class="sorting sorting-desc" data-sortedname="Column1" data-colindex="0">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                                </thead>
                                <tbody>
                                    <tr><td>Row 1 - Column 1</td><td>Row 1 - Column 2</td></tr>
                                    <tr><td>Row 2 - Column 1</td><td>Row 2 - Column 2</td></tr>
                                </tbody>
                            </table>
                        </body>`);
            sortComponent.render(new SortOption($("#table-report"), true, true, () => null, {}));
            spyOn(sortComponent, "onSort");

            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");

            // assert
            let isDescendingSort = true;
            let sortColumnName = "Column1";
            expect(sortComponent.onSort).toHaveBeenCalledWith(sortColumnName, isDescendingSort);
        });
        it("_Table sorter sorted ascending column on normal mode => onSort was called with ascending sort", () => {
            // arrange
            setFixtures(`<body>
                            <table id="table-report" data-descendingsort="0" data-sortedcolumn="Column1">
                                <thead class="tableFloatingHeaderOriginal">
                                    <th class="sorting sorting-asc" data-sortedname="Column1" data-colindex="0">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                                </thead>
                                <tbody>
                                    <tr><td>Row 1 - Column 1</td><td>Row 1 - Column 2</td></tr>
                                    <tr><td>Row 2 - Column 1</td><td>Row 2 - Column 2</td></tr>
                                </tbody>
                            </table>
                        </body>`);
            sortComponent.render(new SortOption($("#table-report"), true, true, () => null, {}));
            spyOn(sortComponent, "onSort");

            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");

            // assert
            let isDescendingSort = false;
            let sortColumnName = "Column1";
            expect(sortComponent.onSort).toHaveBeenCalledWith(sortColumnName, isDescendingSort);
        });
        it("_Table sorter sorted ascending column on stack mode => onSort was called with ascending sort", () => {
            // arrange
            setFixtures(`<body>
                            <table id="table-report2" class="stack-mode" data-descendingsort="False" data-sortedcolumn="Column1">
                                <thead class="tableFloatingHeaderOriginal">
                                    <th class="sorting sorting-asc visible-stack-mode" 
                                        data-sortedname="Column1" data-colindex="0">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                                </thead>
                                <tbody>
                                    <tr><td>Row 1 - Column 1</td><td>Row 1 - Column 2</td></tr>
                                    <tr><td>Row 2 - Column 1</td><td>Row 2 - Column 2</td></tr>
                                </tbody>
                            </table>
                        </body>`);
            sortComponent.render(new SortOption($("#table-report2"), true, true, () => null, {}));
            spyOn(sortComponent, "onSort");
            spyOn($.fn, "tablesorter").and.callFake(() => null);

            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");

            // assert
            let isDescendingSort = false;
            let sortColumnName = "Column1";
            expect(sortComponent.onSort).toHaveBeenCalledWith(sortColumnName, isDescendingSort);
        });
    });
    describe("_textExtraction", () => {
        it("_Link Cell -> Returns HRef Data", () => {
            // arrange
            setFixtures(`<body>
                            <div id="extracted-element" data-islink="true">
                                <a>Cust Name</a>
                            </div>
                        </body>`);

            // act
            var actualData = sortComponent.textExtraction($("#extracted-element"));

            // assert
            expect(actualData).toBe("Cust Name");
        });
        it("_Combined Cell -> Returns First Child Data (Sort Combined Column mean Sort By Win Loss)", () => {
            // arrange
            setFixtures(`<body>
                            <div id="extracted-element" class="combinedcell">
                                <ul>
                                    <li>Win Loss</li>
                                    <li>Comm</li>
                                    <li>Total</li>
                                </ul>
                            </div>
                        </body>`);

            // act
            var actualData = sortComponent.textExtraction($("#extracted-element"));

            // assert
            expect(actualData).toBe("Win Loss");
        });
        it("_Not Combined Cell/Not Link Cell -> Returns Text in Node", () => {
            // arrange
            setFixtures(`<body>
                            <div id="extracted-element">Node Data</div>
                        </body>`);

            // act
            var actualData = sortComponent.textExtraction($("#extracted-element"));

            // assert
            expect(actualData).toBe("Node Data");
        });
    });

    describe("_afterSort", () => {
        it("_Not Have All Data on Client -> Submits Report", () => {
            // arrange
            setFixtures(`<body><form id="report-form" role="form"></form></body>`);
            sortComponent.options.hasAllDataOnClient = false;
            sortComponent.$reportForm = $("#report-form");

            spyOn(sortComponent.$reportForm, "trigger");

            // act
            sortComponent.afterSort();

            // assert
            expect(sortComponent.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });

        let haveAllDataOnClientTests = (currentHtml: string, expectedIsDescendingSort: string, expectedTableBody: string) => {
            // arrange
            setFixtures(currentHtml);
            sortComponent.options.hasAllDataOnClient = true;
            sortComponent.options.sortContainer = $("#tbl-report");
            sortComponent.$reportForm = $("#report-form");
            spyOn(sortComponent.$reportForm, "trigger");
            let replaceHrefParamSpy = spyOn(linkBuilderComponent, "updateParam");

            // act
            sortComponent.afterSort();
            let actualTableBody = $.trim(sortComponent.options.sortContainer.find("tbody").html());

            // assert
            expect(sortComponent.$reportForm.trigger).not.toHaveBeenCalledWith("submit");
            expect(replaceHrefParamSpy.calls.allArgs()).toEqual([
                ["SortPropertyName", "WinLoss"],
                ["IsDescendingSort", expectedIsDescendingSort]]);

            expect(actualTableBody).toEqual(expectedTableBody);
        };

        it(`_Have All Data on Client and Currently Sorts ASC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(sortTemplate.sortASCOnNormalMode, "false", `<tr class=""><td>Row 1</td></tr>
                                    <tr class="even-row"><td>Row 2</td></tr>
                                    <tr><td>Row 3</td></tr>
                                    <tr class="even-row"><td>Row 4</td></tr>`);
            });

        it(`_Have All Data on Client and Currently Sorts DESC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(sortTemplate.sortDESCOnNormalMode, "true", "");
            });
        it(`_Have All Data on Client and Currently Sorts DESC Combined Column on Stack Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(sortTemplate.sortDESCCombinedColumnOnStackMode, "true", "");
            });
    });
    describe("_mapSortedColumnToStackMode", () => {
        it("_Sorted Column in Stack Group_Returns First Column of Stack Group", () => {
            // arrange
            let stackColumnGroups = [["Group 1 - 1", "Group 1 - 2", "Group 1 -3"]];
            let sortedColumn = "Group 1 - 2";
            let expectValue = "Group 1 - 1";

            // act
            let actualValue = sortComponent.mapSortedColumnToStackMode(sortedColumn, stackColumnGroups);

            // assert
            expect(actualValue).toBe(expectValue);
        });
        it("_Sorted Column Not in Stack Group_Returns Sorted Column", () => {
            // arrange
            let stackColumnGroups = [["Group 1 - 1", "Group 1 - 2", "Group 1 -3"]];
            let sortedColumn = "Group 1 - 4";
            let expectValue = sortedColumn;

            // act
            let actualValue = sortComponent.mapSortedColumnToStackMode(sortedColumn, stackColumnGroups);

            // assert
            expect(actualValue).toBe(expectValue);
        });
    });

    describe("_afterSort", () => {
        it("_Not Have All Data on Client -> Submits Report", () => {
            // arrange
            setFixtures(`<body><form id="report-form" role="form"></form></body>`);
            sortComponent.options.hasAllDataOnClient = false;
            sortComponent.$reportForm = $("#report-form");

            spyOn(sortComponent.$reportForm, "trigger");

            // act
            sortComponent.afterSort();

            // assert
            expect(sortComponent.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });

        let haveAllDataOnClientTests = (currentHtml: string, expectedIsDescendingSort: string, expectedTableBody: string) => {
            // arrange
            setFixtures(currentHtml);
            sortComponent.options.hasAllDataOnClient = true;
            sortComponent.options.sortContainer = $("#tbl-report");
            sortComponent.$reportForm = $("#report-form");
            spyOn(sortComponent.$reportForm, "trigger");
            let replaceHrefParamSpy = spyOn(linkBuilderComponent, "updateParam");

            // act
            sortComponent.afterSort();
            let actualTableBody = $.trim(sortComponent.options.sortContainer.find("tbody").html());

            // assert
            expect(sortComponent.$reportForm.trigger).not.toHaveBeenCalledWith("submit");
            expect(replaceHrefParamSpy.calls.allArgs()).toEqual([
                ["SortPropertyName", "WinLoss"],
                ["IsDescendingSort", expectedIsDescendingSort]]);

            expect(actualTableBody).toEqual(expectedTableBody);
        };

        it(`_Have All Data on Client and Currently Sorts ASC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(sortTemplate.sortASCOnNormalMode, "false", `<tr class=""><td>Row 1</td></tr>
                                    <tr class="even-row"><td>Row 2</td></tr>
                                    <tr><td>Row 3</td></tr>
                                    <tr class="even-row"><td>Row 4</td></tr>`);
            });

        it(`_Have All Data on Client and Currently Sorts DESC on Normal Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(sortTemplate.sortDESCOnNormalMode, "true", "");
            });
        it(`_Have All Data on Client and Currently Sorts DESC Combined Column on Stack Mode ->
            Updates Href Param;
            Not Submit Report;
            Updates even-row class for tbody`, () => {
                haveAllDataOnClientTests(sortTemplate.sortDESCCombinedColumnOnStackMode, "true", "");
            });
    });
});