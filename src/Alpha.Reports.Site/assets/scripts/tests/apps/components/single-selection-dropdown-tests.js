describe("SingleSelectionDropdownComponent", function () {
    var singleSelectionDropdownComponent;
    var template = {
        hasOptionsSelection: "<body>\n                                <div id=\"dropdown-container\">\n                                    <select id=\"dropdown-single\">\n                                        <option value=\"Option 1\">Option 1</option>\n                                        <option value=\"Option 2\" selected=\"selected\">Option 2</option>\n                                        <option value=\"Option 3\">Option 3</option>\n                                        <option value=\"Option 4\">Option 4</option>\n                                        <option value=\"Option 5\">Option 5</option>\n                                    </select>\n                                    <input type=\"hidden\" id=\"UserSelectedId\" value=\"UserSelectedId\" />\n                                </div>\n                            </body>"
    };
    beforeEach(function () {
        singleSelectionDropdownComponent = new SingleSelectionDropdownComponent($);
    });
    describe("_render_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.hasOptionsSelection);
            spyOn($.fn, "multiselect");
            // act
            singleSelectionDropdownComponent.render();
        });
        it("_Init Correct Selected Value", function () {
            // arrange
            var expectedValue = "Option 2";
            // assert
            expect(singleSelectionDropdownComponent.selectedValue).toBe(expectedValue);
        });
        it("_Bind multi selection", function () {
            // assert
            expect($.fn.multiselect).toHaveBeenCalledWith({
                multiple: false,
                maxHeight: 300,
                numberDisplayed: 1,
                includeSelectAllOption: false,
                onChange: singleSelectionDropdownComponent.onchange
            });
        });
    });
    describe("_getSelectedItems", function () {
        it("_Not Have Option_Returns Empty", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-single\">\n                            </select>\n                            <input type=\"hidden\" id=\"UserSelectedId\" value=\"UserSelectedId\" />\n                        </body>");
            singleSelectionDropdownComponent.render();
            // act
            var actualSelectedItem = singleSelectionDropdownComponent.getSelectedItem();
            // assert
            expect(actualSelectedItem).toBe("");
        });
        it("_Have Options_Returns Correct Value", function () {
            // arrange
            setFixtures(template.hasOptionsSelection);
            singleSelectionDropdownComponent.render();
            var expectedValue = "Option 2";
            // act
            var actualSelectedItem = singleSelectionDropdownComponent.getSelectedItem();
            // assert
            expect(actualSelectedItem).toBe(expectedValue);
        });
    });
    describe("_updateSelectedValue", function () {
        it("_Always -> Sets New Value to Hidden Field", function () {
            // arrange
            setFixtures(template.hasOptionsSelection);
            singleSelectionDropdownComponent.render();
            singleSelectionDropdownComponent.selectedValue = "Option 3";
            var expectValue = "Option 3";
            // act
            singleSelectionDropdownComponent.updateSelectedValue();
            // assert
            expect(singleSelectionDropdownComponent.$hiddenInput.val()).toBe(expectValue);
        });
    });
    describe("_onchange", function () {
        it("_Always -> Stores Changed Value;Calls option.onchange", function () {
            // arrange
            setFixtures(template.hasOptionsSelection);
            singleSelectionDropdownComponent.render();
            var expectValue = "Option 3";
            singleSelectionDropdownComponent.options.onChange = jasmine.any(Function);
            spyOn(singleSelectionDropdownComponent.options, "onChange");
            // act
            $("#dropdown-container .dropdown-menu input[value=\"Option 3\"]").click();
            // assert
            expect(singleSelectionDropdownComponent.selectedValue).toBe(expectValue);
            expect(singleSelectionDropdownComponent.options.onChange).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=single-selection-dropdown-tests.js.map