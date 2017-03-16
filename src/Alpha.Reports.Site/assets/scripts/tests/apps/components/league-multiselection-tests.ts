describe("LeagueMultiSelectionComponent", () => {
    let leagueMultiSelectionComponent: LeagueMultiSelectionComponent;

    beforeEach(() => {
        leagueMultiSelectionComponent = new LeagueMultiSelectionComponent($);
    });

    describe("_validate", () => {
        it("_Always -> Return Selected Items", () => {
            // arrange
            setFixtures(`<body>
                        <select id="dropdown-league">
                            <option value="1" selected="selected"></option>
                            <option value="2"></option>
                            <option value="3" selected="selected"></option>
                        </select>
                    </body>`);
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");

            // act
            var actualSelectedItems = leagueMultiSelectionComponent.getSelectedItems();

            // assert
            expect(actualSelectedItems).toEqual(actualSelectedItems);
        });
    });
    describe("_getSelectedItems", () => {
        it("_Validate Before Submit; Reselect Items after Validate Error -> Remove Class Error from dropdown", () => {
            // arrange
            setFixtures(`<body>
                            <select id="dropdown-league">
                                <option value="1" selected="selected"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn error"></div>
                        </body>`);
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            leagueMultiSelectionComponent.validate();

            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Not Validate Before Submit; Select Items after Validate Error -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <select id="dropdown-league">
                                <option value="1" selected="selected"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn error"></div>
                        </body>`);
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = false;

            // act
            leagueMultiSelectionComponent.validate();

            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Not Select Items after Validate Error -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-league">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </div>
                            <div class="multiselect btn error"></div>
                        </body>`);
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            leagueMultiSelectionComponent.validate();

            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Select Items after Validate Success -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-league">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </div>
                            <div class="multiselect btn"></div>
                        </body>`);
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            leagueMultiSelectionComponent.validate();

            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Validate Before Submit; Select Items after Validate Error;Don't have button select -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-league">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </div>
                        </body>`);
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = true;
            spyOn($.fn, "removeClass");

            // act
            leagueMultiSelectionComponent.validate();

            // assert
            expect($.fn.removeClass).not.toHaveBeenCalledWith("error");
        });
    });
    describe("_onChange_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(`<body><div id="league-ids"></div></body>`);
            spyOn(leagueMultiSelectionComponent, "validate");
            spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1, 2, 3]);
            leagueMultiSelectionComponent.$hiddenInput = $("#league-ids");

            // act
            leagueMultiSelectionComponent.onChange();
        });

        it("_Validate", () => {
            // assert
            expect(leagueMultiSelectionComponent.validate).toHaveBeenCalled();
        });

        it("_Update hidden input", () => {
            // assert
            expect(leagueMultiSelectionComponent.$hiddenInput.val()).toBe("1,2,3");
        });
    });
    describe("_onSelectAll_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(`<body><div id="league-ids"></div></body>`);
            spyOn(leagueMultiSelectionComponent, "validate");
            spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1, 2, 3]);
            leagueMultiSelectionComponent.$hiddenInput = $("#league-ids");
        });

        describe("_Select All", () => {
            beforeEach(() => {
                // act
                leagueMultiSelectionComponent.onSelectAll(true);
            });

            it("_Validate", () => {
                // assert
                expect(leagueMultiSelectionComponent.validate).toHaveBeenCalled();
            });

            it("_Update hidden input", () => {
                // assert
                expect(leagueMultiSelectionComponent.$hiddenInput.val()).toBe("");
            });
        });
        describe("_Deselect All", () => {
            beforeEach(() => {
                // act
                leagueMultiSelectionComponent.onSelectAll(false);
            });

            it("_Validate", () => {
                // assert
                expect(leagueMultiSelectionComponent.validate).toHaveBeenCalled();
            });

            it("_Update hidden input", () => {
                // assert
                expect(leagueMultiSelectionComponent.$hiddenInput.val()).toBe("-1");
            });
        });
    });
    describe("_render", () => {
        describe("_Has Leagues", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <select id="dropdown-league"
                                    data-has-any-league="true"
                                    data-label-select-leagues="Select League"
                                    data-label-selected="# selected"
                                    data-label-all-leagues="All selected"
                                    data-language="en">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn"></div>
                            <div id="league-ids"></div>
                        </body>`);
                spyOn($.fn, "multiselect");

                // act
                leagueMultiSelectionComponent.render();
            });
            it("_Bind multi selection", () => {
                // assert
                expect($.fn.multiselect).toHaveBeenCalledWith({
                    maxHeight: 300,
                    numberDisplayed: 1,
                    nonSelectedText: "Select League",
                    nSelectedText: " # selected",
                    allSelectedText: "All selected",
                    includeSelectAllOption: true,
                    onChange: leagueMultiSelectionComponent.onChange,
                    onSelectAll: leagueMultiSelectionComponent.onSelectAll,
                    language: "en",
                    enableCaseInsensitiveFiltering: true
                });
            });
            it("_Cache element;", () => {
                // assert
                expect(leagueMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
        describe("_Not Have League", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <select id="dropdown-league"
                                    data-has-any-league="false"
                                    data-label-there-is-no-league="No League Found"
                                    data-label-selected="# selected"
                                    data-label-all-leagues="All selected"
                                    data-language="en">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn"></div>
                            <div id="league-ids"></div>
                        </body>`);
                spyOn($.fn, "multiselect");

                // act
                leagueMultiSelectionComponent.render();
            });
            it("_Bind multi selection", () => {
                // assert
                expect($.fn.multiselect).toHaveBeenCalledWith({
                    maxHeight: 300,
                    numberDisplayed: 1,
                    nonSelectedText: "No League Found",
                    nSelectedText: " # selected",
                    allSelectedText: "All selected",
                    includeSelectAllOption: false,
                    onChange: leagueMultiSelectionComponent.onChange,
                    onSelectAll: leagueMultiSelectionComponent.onSelectAll,
                    language: "en",
                    enableCaseInsensitiveFiltering: true
                });
            });
            it("_Cache element;", () => {
                // assert
                expect(leagueMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
    });
});