describe("Currency Selection", () => {
    let currencySelectionComponent: CurrencySelectionComponent;
    beforeEach(() => {
        currencySelectionComponent = new CurrencySelectionComponent($);

        setFixtures(`<body>
                        <div id="dropdown-currency-container">
                            <select id="dropdown-currencies">
                                <option id="1" value="1" selected="selected"></option>
                                <option id="2" value="2"></option>
                            </select>
                            <input type="hidden" value="" id="UserSelectedCurrencyId"/>
                        </div>
                     </body>`);
    });
    describe("_render", () => {
        it("_Always -> Bind onChange Event to dropdown", () => {
            // act
            currencySelectionComponent.render();
            $("#dropdown-currency-container .dropdown-menu input[value=\"2\"]").click();

            // assert
            expect(currencySelectionComponent.selectedValue).toBe("2");
        });
    });
    describe("_updateSelectedValue", () => {
        it("_Always -> Bind onChange Event to dropdown", () => {
            // arrange
            currencySelectionComponent.render();
            currencySelectionComponent.selectedValue = "2";
            let expectValue = "2";

            // act
            currencySelectionComponent.updateSelectedValue();

            // assert
            expect(currencySelectionComponent.$hiddenInput.val()).toBe(expectValue);
        });
    });
    describe("_getSelectedItems", () => {
        it("_Always_Returns Correct Selected Item", () => {
            // arrange
            currencySelectionComponent.render();

            // act
            let actualSelectedItem = currencySelectionComponent.getSelectedItems();

            // assert
            expect(actualSelectedItem).toBe("1");
        });
    });
});





















