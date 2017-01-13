describe("SortComponent", () => {
    let sortComponent: SortComponent;

    beforeEach(() => {
        sortComponent = new SortComponent($);
    });

    describe("render", () => {
        it("Can not sort -> Remove icon sorting from sorting column", () => {
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
        it(`Can sort/Have all data on client ->
                Remove icon sorting from sorting column;
                Bind Table Sorter;
                Not bind on sort event;`, () => {
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

                // assert
                expect($(".sorting").length).not.toEqual(0);
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();

                $("[data-sortedname=\"Column2\"]").trigger("click");
                expect(sortComponent.onSort).not.toHaveBeenCalled();
            });
        it(`Can sort/Not Have all data on client/Descending Sort ->
                Remove icon sorting from sorting column;
                Bind Table Sorter;
                Bind on sort event;`, () => {
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

                // assert
                expect($(".sorting").length).not.toEqual(0);
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();

                expect($("[data-sortedname=\"Column1\"]").hasClass("sorting-desc")).toBeTruthy();
                $("[data-sortedname=\"Column2\"]").trigger("click");
                expect(sortComponent.onSort).toHaveBeenCalled();
            });
        it(`Can sort/Not Have all data on client/Ascending Sort ->
                Remove icon sorting from sorting column;
                Bind Table Sorter;
                Bind on sort event;`, () => {
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

                // assert
                expect($(".sorting").length).not.toEqual(0);
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();

                expect($("[data-sortedname=\"Column1\"]").hasClass("sorting-asc")).toBeTruthy();
                $("[data-sortedname=\"Column2\"]").trigger("click");
                expect(sortComponent.onSort).toHaveBeenCalled();
            });
    });

    describe("onSort", () => {
        it(`Switch Sort Column ->
                Set Descending Sort;
                Update Sort Column Name;
                Call After Sort Function`, () => {
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

                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("1");
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column2");
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        it(`Not Switch Sort Column/Currently Ascending Sort ->
                Set Descending Sort;
                Not Change Sort Column Name;
                Call After Sort Function`, () => {
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

                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("1");
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column1");
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        it(`Not Switch Sort Column/Currently Ascending Sort ->
                Set Descending Sort;
                Not Change Sort Column Name;
                Call After Sort Function`, () => {
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

                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("1");
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column1");
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        it(`Not Switch Sort Column/Currently Descending Sort ->
                Set Ascending Sort;
                Not Change Sort Column Name;
                Call After Sort Function`, () => {
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

                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("0");
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column1");
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
    });
    describe("getSortInfo", () => {
        it("Descending Sort -> Returns Sort Info from table data", () => {
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
            expect(actalSortInfo.sortPropertyName).toBe("Column1");
            expect(actalSortInfo.isDescendingSort).toBeTruthy();
        });

        it("Ascending Sort -> Returns Sort Info from table data", () => {
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
            expect(actalSortInfo.sortPropertyName).toBe("Column1");
            expect(actalSortInfo.isDescendingSort).toBeFalsy();
        });
    });
    describe("bindTableSorter", () => {
        it("Cannot Sort -> Don't bind tablesorter and event sort end", () => {
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

            // assert
            expect($.fn.tablesorter).not.toHaveBeenCalled();
            sortComponent.$sortContainer.trigger("sortEnd");
            expect(sortComponent.options.afterSortFunction).not.toHaveBeenCalled();
        });
        it("Don't have all data on client -> Don't bind tablesorter and event sort end", () => {
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

            // assert
            expect($.fn.tablesorter).not.toHaveBeenCalled();
            sortComponent.$sortContainer.trigger("sortEnd");
            expect(sortComponent.options.afterSortFunction).not.toHaveBeenCalled();
        });
        it("Can sort;Have all data on client -> Bind tablesorter and event sort end", () => {
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

            // assert
            expect($.fn.tablesorter).toHaveBeenCalledWith({
                defaultParser: 1,
                headers: { 1: { sorter: false } },
                sortList: [[0, 0]],
                textExtraction: jasmine.any(Function)
            });
        });

        describe("Had Data On Client", () => {
            beforeEach(() => {
            });

            it("Table sorter sorted descending column on normal mode => onSort was called", () => {
                // arrange
                setFixtures(`<body>
                            <table id="table-report" data-descendingsort="0" data-sortedcolumn="Column1">
                                <thead class="tableFloatingHeaderOriginal">
                                    <th class="sorting sorting-desc" data-sortedname="Column1" data-colindex="0">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                                </thead>
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

            it("Table sorter sorted ascending column on normal mode => onSort was called", () => {
                // arrange
                setFixtures(`<body>
                            <table id="table-report" data-descendingsort="0" data-sortedcolumn="Column1">
                                <thead class="tableFloatingHeaderOriginal">
                                    <th class="sorting sorting-asc" data-sortedname="Column1" data-colindex="0">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                                </thead>
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

            it("Table sorter sorted ascending column on stack mode => onSort was called", () => {
                // arrange
                setFixtures(`<body>
                            <table id="table-report2" class="stack-mode" data-descendingsort="0" data-sortedcolumn="Column1">
                                <thead class="tableFloatingHeaderOriginal">
                                    <th class="sorting sorting-asc visible-stack-mode" data-sortedname="Column1" data-colindex="0">Column 1</th>
                                    <th class="sorting" data-sortedname="Column2" data-disabledsort="true">Column 2</th>
                                </thead>
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
    });

    describe("textExtraction", () => {
        it("Combined Cell -> Returns First Child Data (Sort Combined Column mean Sort By Win Loss)", () => {
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
        it("Link Cell -> Returns HRef Data", () => {
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
        it("Not Combined Cell/Not Link Cell -> Returns Text in Node", () => {
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
});