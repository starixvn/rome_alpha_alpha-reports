describe("SortComponent", function () {
    var sortComponent;
    var linkBuilderComponent;
    beforeEach(function () {
        linkBuilderComponent = new LinkBuilderComponent($);
        sortComponent = new SortComponent($, linkBuilderComponent);
    });
    describe("_render", function () {
        it("_Cannot sort -> Remove icon sorting from sorting column", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"table-report\">\n                                <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                            </table>\n                        </body>");
            // act
            sortComponent.render(new SortOption($("#table-report"), false));
            // assert
            expect($(".sorting").length).toEqual(0);
        });
        describe("_Can sort/Have all data on client", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <table id=\"table-report\">\n                                <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                            </table>\n                        </body>");
                spyOn(sortComponent, "bindTableSorter");
                spyOn(sortComponent, "onSort");
                // act
                sortComponent.render(new SortOption($("#table-report"), true, true));
            });
            it("_Not Remove icon sorting from sorting column", function () {
                // assert
                expect($(".sorting").length).not.toEqual(0);
            });
            it("_Bind Table Sorter", function () {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });
            it("Not bind on sort event", function () {
                // assert
                expect(sortComponent.onSort).not.toHaveBeenCalled();
            });
        });
        describe("_Can sort/Not Have all data on client/Descending Sort", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                                <table id=\"table-report\" data-descendingsort=\"True\" data-sortedcolumn=\"Column1\">\n                                    <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                                </table>\n                            </body>");
                spyOn(sortComponent, "bindTableSorter");
                spyOn(sortComponent, "onSort");
                // act
                sortComponent.render(new SortOption($("#table-report"), true, false));
            });
            it("_Not Remove icon sorting from sorting column", function () {
                // assert
                expect($(".sorting").length).not.toEqual(0);
            });
            it("_Bind Table Sorter", function () {
                // assert
                expect($("[data-sortedname=\"Column1\"]").hasClass("sorting-desc")).toBeTruthy();
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
            });
            it("_Bind on sort event", function () {
                // act
                $("[data-sortedname=\"Column2\"]").trigger("click");
                // assert
                expect(sortComponent.onSort).toHaveBeenCalled();
            });
        });
        describe("_Can sort/Not Have all data on client/Ascending Sort", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                                <table id=\"table-report\" data-descendingsort=\"False\" data-sortedcolumn=\"Column1\">\n                                    <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                                </table>\n                            </body>");
                spyOn(sortComponent, "bindTableSorter");
                spyOn(sortComponent, "onSort");
                // act
                sortComponent.render(new SortOption($("#table-report"), true, false));
            });
            it("_Not Remove icon sorting from sorting column", function () {
                // assert
                expect($(".sorting").length).not.toEqual(0);
            });
            it("_Bind Table Sorter", function () {
                // assert
                expect(sortComponent.bindTableSorter).toHaveBeenCalled();
                expect($("[data-sortedname=\"Column1\"]").hasClass("sorting-asc")).toBeTruthy();
            });
            it("_Bind on sort event", function () {
                // act
                $("[data-sortedname=\"Column2\"]").trigger("click");
                // assert
                expect(sortComponent.onSort).toHaveBeenCalled();
            });
        });
    });
    describe("_onSort", function () {
        describe("_Switch Sort Column", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                                <table id=\"table-report\" data-descendingsort=\"False\" data-sortedcolumn=\"Column1\">\n                                    <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                                </table>\n                            </body>");
                sortComponent.render(new SortOption($("#table-report"), true, false, function () { return null; }));
                spyOn(sortComponent.options, "afterSortFunction");
                // act
                $("[data-sortedname=\"Column2\"]").trigger("click");
            });
            it("_Set Descending Sort", function () {
                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("1");
            });
            it("_Update Sort Column Name", function () {
                // assert
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column2");
            });
            it("_Call After Sort Function", function () {
                // assert
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        });
        describe("_Not Switch Sort Column/ Currently Ascending Sort", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                                <table id=\"table-report\" data-descendingsort=\"False\" data-sortedcolumn=\"Column1\">\n                                    <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                                </table>\n                            </body>");
                sortComponent.render(new SortOption($("#table-report"), true, false, function () { return null; }));
                spyOn(sortComponent.options, "afterSortFunction");
                // act
                $("[data-sortedname=\"Column1\"]").trigger("click");
            });
            it("_Set Descending Sort", function () {
                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("1");
            });
            it("_Not Change Sort Column Name", function () {
                // assert
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column1");
            });
            it("_Call After Sort Function", function () {
                // assert
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        });
        describe("_Not Switch Sort Column/Currently Descending Sort", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                                <table id=\"table-report\" data-descendingsort=\"True\" data-sortedcolumn=\"Column1\">\n                                    <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                                </table>\n                            </body>");
                sortComponent.render(new SortOption($("#table-report"), true, false, function () { return null; }));
                spyOn(sortComponent.options, "afterSortFunction");
                // act
                $("[data-sortedname=\"Column1\"]").trigger("click");
            });
            it("_Set Ascending Sort", function () {
                // assert
                expect(sortComponent.$sortContainer.data("descendingsort")).toEqual("0");
            });
            it("_Not Change Sort Column Name", function () {
                // assert
                expect(sortComponent.$sortContainer.data("sortedcolumn")).toEqual("Column1");
            });
            it("_Call After Sort Function", function () {
                // assert
                expect(sortComponent.options.afterSortFunction).toHaveBeenCalled();
            });
        });
    });
    describe("_getSortInfo", function () {
        it("_Descending Sort -> Returns Sort Info from table data", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"1\" data-sortedcolumn=\"Column1\">\n                                <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                            </table>\n                        </body>");
            sortComponent.$sortContainer = $("#table-report");
            // act
            var actalSortInfo = sortComponent.getSortInfo();
            // assert
            expect(actalSortInfo).toEqual({
                sortPropertyName: "Column1",
                isDescendingSort: true
            });
        });
        it("_Ascending Sort -> Returns Sort Info from table data", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"0\" data-sortedcolumn=\"Column1\">\n                                <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                            </table>\n                        </body>");
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
    describe("_bindTableSorter", function () {
        describe("_Cannot Sort", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"0\" data-sortedcolumn=\"Column1\">\n                                <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                            </table>\n                        </body>");
                sortComponent.options.canSort = false;
                sortComponent.$sortContainer = $("#table-report");
                sortComponent.options.afterSortFunction = function () { return null; };
                spyOn($.fn, "tablesorter");
                spyOn(sortComponent.options, "afterSortFunction");
                // act
                sortComponent.bindTableSorter("Column1");
            });
            it("_Don't bind tablesorter", function () {
                // assert
                expect($.fn.tablesorter).not.toHaveBeenCalled();
            });
            it("_Don't bind event sort end", function () {
                // assert
                sortComponent.$sortContainer.trigger("sortEnd");
                expect(sortComponent.options.afterSortFunction).not.toHaveBeenCalled();
            });
        });
        describe("_Don't have all data on client", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"0\" data-sortedcolumn=\"Column1\">\n                                <th class=\"sorting\" data-sortedname=\"Column1\">Column 1</th>\n                                <th class=\"sorting\" data-sortedname=\"Column2\">Column 2</th>\n                            </table>\n                        </body>");
                sortComponent.options.canSort = true;
                sortComponent.options.hasAllDataOnClient = false;
                sortComponent.$sortContainer = $("#table-report");
                sortComponent.options.afterSortFunction = function () { return null; };
                spyOn($.fn, "tablesorter");
                spyOn(sortComponent.options, "afterSortFunction");
                // act
                sortComponent.bindTableSorter("Column1");
            });
            it("_Don't bind tablesorter", function () {
                // assert
                expect($.fn.tablesorter).not.toHaveBeenCalled();
            });
            it("_Don't bind event sort end", function () {
                // assert
                sortComponent.$sortContainer.trigger("sortEnd");
                expect(sortComponent.options.afterSortFunction).not.toHaveBeenCalled();
            });
        });
        describe("_Can sort;Have all data on client", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"0\" data-sortedcolumn=\"Column1\">\n                                <th class=\"sorting\" data-sortedname=\"Column1\" data-colindex=\"0\">Column 1</th>\n                                <th class=\"sorting\" data-sortedname=\"Column2\" data-disabledsort=\"true\">Column 2</th>\n                            </table>\n                        </body>");
                sortComponent.render(new SortOption($("#table-report"), true, true, function () { return null; }, {}));
                spyOn($.fn, "tablesorter");
                spyOn(sortComponent.options, "afterSortFunction");
                // act
                sortComponent.bindTableSorter("Column1");
            });
            it("_Bind tablesorter", function () {
                // assert
                expect($.fn.tablesorter).toHaveBeenCalledWith({
                    defaultParser: 1,
                    headers: { 1: { sorter: false } },
                    sortList: [[0, 0]],
                    textExtraction: jasmine.any(Function)
                });
            });
        });
        it("_Incorrect data", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"1\" data-sortedcolumn=\"Column1\">\n                                <thead class=\"tableFloatingHeaderOriginal\">\n                                    <th class=\"sorting sorting-desc\" data-sortedname=\"Column1\" data-colindex=\"0\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\" data-disabledsort=\"true\">Column 2</th>\n                                </thead>\n                            </table>\n                        </body>");
            sortComponent.render(new SortOption($("#table-report"), true, true, function () { return null; }, {}));
            spyOn(sortComponent, "onSort");
            spyOn(console, "log");
            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");
            // assert
            expect(console.log).toHaveBeenCalled();
        });
    });
    describe("_Had Data On Client", function () {
        it("_Table sorter sorted descending column on normal mode => onSort was called with descending sort", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"1\" data-sortedcolumn=\"Column1\">\n                                <thead class=\"tableFloatingHeaderOriginal\">\n                                    <th class=\"sorting sorting-desc\" data-sortedname=\"Column1\" data-colindex=\"0\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\" data-disabledsort=\"true\">Column 2</th>\n                                </thead>\n                                <tbody>\n                                    <tr><td>Row 1 - Column 1</td><td>Row 1 - Column 2</td></tr>\n                                    <tr><td>Row 2 - Column 1</td><td>Row 2 - Column 2</td></tr>\n                                </tbody>\n                            </table>\n                        </body>");
            sortComponent.render(new SortOption($("#table-report"), true, true, function () { return null; }, {}));
            spyOn(sortComponent, "onSort");
            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");
            // assert
            var isDescendingSort = true;
            var sortColumnName = "Column1";
            expect(sortComponent.onSort).toHaveBeenCalledWith(sortColumnName, isDescendingSort);
        });
        it("_Table sorter sorted ascending column on normal mode => onSort was called with ascending sort", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"table-report\" data-descendingsort=\"0\" data-sortedcolumn=\"Column1\">\n                                <thead class=\"tableFloatingHeaderOriginal\">\n                                    <th class=\"sorting sorting-asc\" data-sortedname=\"Column1\" data-colindex=\"0\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\" data-disabledsort=\"true\">Column 2</th>\n                                </thead>\n                                <tbody>\n                                    <tr><td>Row 1 - Column 1</td><td>Row 1 - Column 2</td></tr>\n                                    <tr><td>Row 2 - Column 1</td><td>Row 2 - Column 2</td></tr>\n                                </tbody>\n                            </table>\n                        </body>");
            sortComponent.render(new SortOption($("#table-report"), true, true, function () { return null; }, {}));
            spyOn(sortComponent, "onSort");
            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");
            // assert
            var isDescendingSort = false;
            var sortColumnName = "Column1";
            expect(sortComponent.onSort).toHaveBeenCalledWith(sortColumnName, isDescendingSort);
        });
        it("_Table sorter sorted ascending column on stack mode => onSort was called with ascending sort", function () {
            // arrange
            setFixtures("<body>\n                            <table id=\"table-report2\" class=\"stack-mode\" data-descendingsort=\"False\" data-sortedcolumn=\"Column1\">\n                                <thead class=\"tableFloatingHeaderOriginal\">\n                                    <th class=\"sorting sorting-asc visible-stack-mode\" \n                                        data-sortedname=\"Column1\" data-colindex=\"0\">Column 1</th>\n                                    <th class=\"sorting\" data-sortedname=\"Column2\" data-disabledsort=\"true\">Column 2</th>\n                                </thead>\n                                <tbody>\n                                    <tr><td>Row 1 - Column 1</td><td>Row 1 - Column 2</td></tr>\n                                    <tr><td>Row 2 - Column 1</td><td>Row 2 - Column 2</td></tr>\n                                </tbody>\n                            </table>\n                        </body>");
            sortComponent.render(new SortOption($("#table-report2"), true, true, function () { return null; }, {}));
            spyOn(sortComponent, "onSort");
            spyOn($.fn, "tablesorter").and.callFake(function () { return null; });
            // act
            sortComponent.bindTableSorter("Column1");
            sortComponent.$sortContainer.trigger("sortEnd");
            // assert
            var isDescendingSort = false;
            var sortColumnName = "Column1";
            expect(sortComponent.onSort).toHaveBeenCalledWith(sortColumnName, isDescendingSort);
        });
    });
    describe("_textExtraction", function () {
        it("_Link Cell -> Returns HRef Data", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"extracted-element\" data-islink=\"true\">\n                                <a>Cust Name</a>\n                            </div>\n                        </body>");
            // act
            var actualData = sortComponent.textExtraction($("#extracted-element"));
            // assert
            expect(actualData).toBe("Cust Name");
        });
        it("_Combined Cell -> Returns First Child Data (Sort Combined Column mean Sort By Win Loss)", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"extracted-element\" class=\"combinedcell\">\n                                <ul>\n                                    <li>Win Loss</li>\n                                    <li>Comm</li>\n                                    <li>Total</li>\n                                </ul>\n                            </div>\n                        </body>");
            // act
            var actualData = sortComponent.textExtraction($("#extracted-element"));
            // assert
            expect(actualData).toBe("Win Loss");
        });
        it("_Not Combined Cell/Not Link Cell -> Returns Text in Node", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"extracted-element\">Node Data</div>\n                        </body>");
            // act
            var actualData = sortComponent.textExtraction($("#extracted-element"));
            // assert
            expect(actualData).toBe("Node Data");
        });
    });
    describe("_afterSort", function () {
        it("_Not Have All Data on Client -> Submits Report", function () {
            // arrange
            setFixtures("<body><form id=\"report-form\" role=\"form\"></form></body>");
            sortComponent.options.hasAllDataOnClient = false;
            sortComponent.$reportForm = $("#report-form");
            spyOn(sortComponent.$reportForm, "trigger");
            // act
            sortComponent.afterSort();
            // assert
            expect(sortComponent.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
        var haveAllDataOnClientTests = function (currentHtml, expectedIsDescendingSort, expectedTableBody) {
            // arrange
            setFixtures(currentHtml);
            sortComponent.options.hasAllDataOnClient = true;
            sortComponent.options.sortContainer = $("#tbl-report");
            sortComponent.$reportForm = $("#report-form");
            spyOn(sortComponent.$reportForm, "trigger");
            var replaceHrefParamSpy = spyOn(linkBuilderComponent, "updateParam");
            // act
            sortComponent.afterSort();
            var actualTableBody = $.trim(sortComponent.options.sortContainer.find("tbody").html());
            // assert
            expect(sortComponent.$reportForm.trigger).not.toHaveBeenCalledWith("submit");
            expect(replaceHrefParamSpy.calls.allArgs()).toEqual([
                ["SortPropertyName", "WinLoss"],
                ["IsDescendingSort", expectedIsDescendingSort]
            ]);
            expect(actualTableBody).toEqual(expectedTableBody);
        };
        it("_Have All Data on Client and Currently Sorts ASC on Normal Mode ->\n            Updates Href Param;\n            Not Submit Report;\n            Updates even-row class for tbody", function () {
            haveAllDataOnClientTests(sortTemplate.sortASCOnNormalMode, "false", "<tr class=\"\"><td>Row 1</td></tr>\n                                    <tr class=\"even-row\"><td>Row 2</td></tr>\n                                    <tr><td>Row 3</td></tr>\n                                    <tr class=\"even-row\"><td>Row 4</td></tr>");
        });
        it("_Have All Data on Client and Currently Sorts DESC on Normal Mode ->\n            Updates Href Param;\n            Not Submit Report;\n            Updates even-row class for tbody", function () {
            haveAllDataOnClientTests(sortTemplate.sortDESCOnNormalMode, "true", "");
        });
        it("_Have All Data on Client and Currently Sorts DESC Combined Column on Stack Mode ->\n            Updates Href Param;\n            Not Submit Report;\n            Updates even-row class for tbody", function () {
            haveAllDataOnClientTests(sortTemplate.sortDESCCombinedColumnOnStackMode, "true", "");
        });
    });
    describe("_mapSortedColumnToStackMode", function () {
        it("_Sorted Column in Stack Group_Returns First Column of Stack Group", function () {
            // arrange
            var stackColumnGroups = [["Group 1 - 1", "Group 1 - 2", "Group 1 -3"]];
            var sortedColumn = "Group 1 - 2";
            var expectValue = "Group 1 - 1";
            // act
            var actualValue = sortComponent.mapSortedColumnToStackMode(sortedColumn, stackColumnGroups);
            // assert
            expect(actualValue).toBe(expectValue);
        });
        it("_Sorted Column Not in Stack Group_Returns Sorted Column", function () {
            // arrange
            var stackColumnGroups = [["Group 1 - 1", "Group 1 - 2", "Group 1 -3"]];
            var sortedColumn = "Group 1 - 4";
            var expectValue = sortedColumn;
            // act
            var actualValue = sortComponent.mapSortedColumnToStackMode(sortedColumn, stackColumnGroups);
            // assert
            expect(actualValue).toBe(expectValue);
        });
    });
    describe("_afterSort", function () {
        it("_Not Have All Data on Client -> Submits Report", function () {
            // arrange
            setFixtures("<body><form id=\"report-form\" role=\"form\"></form></body>");
            sortComponent.options.hasAllDataOnClient = false;
            sortComponent.$reportForm = $("#report-form");
            spyOn(sortComponent.$reportForm, "trigger");
            // act
            sortComponent.afterSort();
            // assert
            expect(sortComponent.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
        var haveAllDataOnClientTests = function (currentHtml, expectedIsDescendingSort, expectedTableBody) {
            // arrange
            setFixtures(currentHtml);
            sortComponent.options.hasAllDataOnClient = true;
            sortComponent.options.sortContainer = $("#tbl-report");
            sortComponent.$reportForm = $("#report-form");
            spyOn(sortComponent.$reportForm, "trigger");
            var replaceHrefParamSpy = spyOn(linkBuilderComponent, "updateParam");
            // act
            sortComponent.afterSort();
            var actualTableBody = $.trim(sortComponent.options.sortContainer.find("tbody").html());
            // assert
            expect(sortComponent.$reportForm.trigger).not.toHaveBeenCalledWith("submit");
            expect(replaceHrefParamSpy.calls.allArgs()).toEqual([
                ["SortPropertyName", "WinLoss"],
                ["IsDescendingSort", expectedIsDescendingSort]
            ]);
            expect(actualTableBody).toEqual(expectedTableBody);
        };
        it("_Have All Data on Client and Currently Sorts ASC on Normal Mode ->\n            Updates Href Param;\n            Not Submit Report;\n            Updates even-row class for tbody", function () {
            haveAllDataOnClientTests(sortTemplate.sortASCOnNormalMode, "false", "<tr class=\"\"><td>Row 1</td></tr>\n                                    <tr class=\"even-row\"><td>Row 2</td></tr>\n                                    <tr><td>Row 3</td></tr>\n                                    <tr class=\"even-row\"><td>Row 4</td></tr>");
        });
        it("_Have All Data on Client and Currently Sorts DESC on Normal Mode ->\n            Updates Href Param;\n            Not Submit Report;\n            Updates even-row class for tbody", function () {
            haveAllDataOnClientTests(sortTemplate.sortDESCOnNormalMode, "true", "");
        });
        it("_Have All Data on Client and Currently Sorts DESC Combined Column on Stack Mode ->\n            Updates Href Param;\n            Not Submit Report;\n            Updates even-row class for tbody", function () {
            haveAllDataOnClientTests(sortTemplate.sortDESCCombinedColumnOnStackMode, "true", "");
        });
    });
});
//# sourceMappingURL=sort-tests.js.map