describe("Currency Selection", function () {
    var currencySelectionComponent;
    beforeEach(function () {
        currencySelectionComponent = new CurrencySelectionComponent($);
        setFixtures("<body>\n                        <div id=\"dropdown-currency-container\">\n                            <select id=\"dropdown-currencies\">\n                                <option id=\"1\" value=\"1\" selected=\"selected\"></option>\n                                <option id=\"2\" value=\"2\"></option>\n                            </select>\n                            <input type=\"hidden\" value=\"\" id=\"UserSelectedCurrencyId\"/>\n                        </div>\n                     </body>");
    });
    describe("_render", function () {
        it("_Always -> Bind onChange Event to dropdown", function () {
            // act
            currencySelectionComponent.render();
            $("#dropdown-currency-container .dropdown-menu input[value=\"2\"]").click();
            // assert
            expect(currencySelectionComponent.selectedValue).toBe("2");
        });
    });
    describe("_updateSelectedValue", function () {
        it("_Always -> Bind onChange Event to dropdown", function () {
            // arrange
            currencySelectionComponent.render();
            currencySelectionComponent.selectedValue = "2";
            var expectValue = "2";
            // act
            currencySelectionComponent.updateSelectedValue();
            // assert
            expect(currencySelectionComponent.$hiddenInput.val()).toBe(expectValue);
        });
    });
    describe("_getSelectedItems", function () {
        it("_Always_Returns Correct Selected Item", function () {
            // arrange
            currencySelectionComponent.render();
            // act
            var actualSelectedItem = currencySelectionComponent.getSelectedItems();
            // assert
            expect(actualSelectedItem).toBe("1");
        });
    });
});
//# sourceMappingURL=currency-selection-tests.js.map