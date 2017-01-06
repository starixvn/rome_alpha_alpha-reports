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
                                <table id="table-report" data-descendingsort="1" data-sortedcolumn="Column1">
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
});