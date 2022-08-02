describe("LeagueMultiSelectionComponent", function () {
    var leagueMultiSelectionComponent;
    beforeEach(function () {
        leagueMultiSelectionComponent = new LeagueMultiSelectionComponent($);
    });
    describe("_validate", function () {
        it("_Always -> Return Selected Items", function () {
            // arrange
            setFixtures("<body>\n                        <select id=\"dropdown-league\">\n                            <option value=\"1\" selected=\"selected\"></option>\n                            <option value=\"2\"></option>\n                            <option value=\"3\" selected=\"selected\"></option>\n                        </select>\n                    </body>");
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            // act
            var actualSelectedItems = leagueMultiSelectionComponent.getSelectedItems();
            // assert
            expect(actualSelectedItems).toEqual(actualSelectedItems);
        });
    });
    describe("_getSelectedItems", function () {
        it("_Validate Before Submit; Reselect Items after Validate Error -> Remove Class Error from dropdown", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-league\">\n                                <option value=\"1\" selected=\"selected\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            leagueMultiSelectionComponent.validate();
            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Not Validate Before Submit; Select Items after Validate Error -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-league\">\n                                <option value=\"1\" selected=\"selected\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = false;
            // act
            leagueMultiSelectionComponent.validate();
            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Not Select Items after Validate Error -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-league\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </div>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            leagueMultiSelectionComponent.validate();
            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Select Items after Validate Success -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-league\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </div>\n                            <div class=\"multiselect btn\"></div>\n                        </body>");
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            leagueMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            leagueMultiSelectionComponent.validate();
            // assert
            expect(leagueMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Validate Before Submit; Select Items after Validate Error;Don't have button select -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-league\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </div>\n                        </body>");
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
    describe("_onChange_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"league-ids\"></div>\n                            <input id=\"IsUserSelectsAllLeagues\" type=\"hidden\" />\n                        </body>");
            spyOn(leagueMultiSelectionComponent, "validate");
            spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [1, 2, 3]; });
            leagueMultiSelectionComponent.$hiddenInput = $("#league-ids");
            leagueMultiSelectionComponent.$isUserSelectsAllLeagues = leagueMultiSelectionComponent.$("#IsUserSelectsAllLeagues");
            // act
            leagueMultiSelectionComponent.onChange();
        });
        it("_Validate", function () {
            // assert
            expect(leagueMultiSelectionComponent.validate).toHaveBeenCalled();
        });
        it("_Update hidden input", function () {
            // assert
            expect(leagueMultiSelectionComponent.$hiddenInput.val()).toBe("1,2,3");
            expect(leagueMultiSelectionComponent.$isUserSelectsAllLeagues.val()).toBe("false");
        });
    });
    describe("_onSelectAll", function () {
        beforeEach(function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-league\">\n                                <option id=\"1\">Option 1</option>\n                                <option id=\"2\">Option 2</option>\n                                <option id=\"3\">Option 3</option>\n                            </select>\n                            <div id=\"league-ids\"></div>\n                            <input id=\"IsUserSelectsAllLeagues\" type=\"hidden\" />\n                        </body>");
            spyOn(leagueMultiSelectionComponent, "validate");
            leagueMultiSelectionComponent.$hiddenInput = $("#league-ids");
            leagueMultiSelectionComponent.$dropdownLeague = $("#dropdown-league");
            leagueMultiSelectionComponent.$isUserSelectsAllLeagues = leagueMultiSelectionComponent.$("#IsUserSelectsAllLeagues");
        });
        describe("_Select All", function () {
            beforeEach(function () {
                // arrange
                spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [1, 2, 3]; });
                // act
                leagueMultiSelectionComponent.onSelectAll();
            });
            it("_Validate", function () {
                // assert
                expect(leagueMultiSelectionComponent.validate).toHaveBeenCalled();
            });
            it("_Update hidden input", function () {
                // assert
                expect(leagueMultiSelectionComponent.$hiddenInput.val()).toBe("");
                expect(leagueMultiSelectionComponent.$isUserSelectsAllLeagues.val()).toBe("true");
            });
        });
        describe("_Deselect All", function () {
            beforeEach(function () {
                // arrange
                spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return []; });
                // act
                leagueMultiSelectionComponent.onSelectAll();
            });
            it("_Validate", function () {
                // assert
                expect(leagueMultiSelectionComponent.validate).toHaveBeenCalled();
            });
            it("_Update hidden input", function () {
                // assert
                expect(leagueMultiSelectionComponent.$hiddenInput.val()).toBe("-1");
                expect(leagueMultiSelectionComponent.$isUserSelectsAllLeagues.val()).toBe("false");
            });
        });
        describe("_Remain some selected", function () {
            beforeEach(function () {
                // arrange
                spyOn(leagueMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [1, 2]; });
                // act
                leagueMultiSelectionComponent.onSelectAll();
            });
            it("_Validate", function () {
                // assert
                expect(leagueMultiSelectionComponent.validate).toHaveBeenCalled();
            });
            it("_Update hidden input", function () {
                // assert
                expect(leagueMultiSelectionComponent.$hiddenInput.val()).toBe("1,2");
                expect(leagueMultiSelectionComponent.$isUserSelectsAllLeagues.val()).toBe("false");
            });
        });
    });
    describe("_render", function () {
        describe("_Has Leagues", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <select id=\"dropdown-league\"\n                                    data-has-any-league=\"true\"\n                                    data-label-select-leagues=\"Select League\"\n                                    data-label-selected=\"# selected\"\n                                    data-label-all-leagues=\"All selected\"\n                                    data-language=\"en\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn\"></div>\n                            <div id=\"league-ids\"></div>\n                        </body>");
                spyOn($.fn, "multiselect");
                // act
                leagueMultiSelectionComponent.render();
            });
            it("_Bind multi selection", function () {
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
            it("_Cache element;", function () {
                // assert
                expect(leagueMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
        describe("_Not Have League", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <select id=\"dropdown-league\"\n                                    data-has-any-league=\"false\"\n                                    data-label-there-is-no-league=\"No League Found\"\n                                    data-label-selected=\"# selected\"\n                                    data-label-all-leagues=\"All selected\"\n                                    data-language=\"en\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn\"></div>\n                            <div id=\"league-ids\"></div>\n                        </body>");
                spyOn($.fn, "multiselect");
                // act
                leagueMultiSelectionComponent.render();
            });
            it("_Bind multi selection", function () {
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
            it("_Cache element;", function () {
                // assert
                expect(leagueMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
    });
});
//# sourceMappingURL=league-multiselection-tests.js.map