describe("ProductMultiSelectionComponent", () => {
    let productMultiSelectionComponent: ProductMultiSelectionComponent;

    beforeEach(() => {
        productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    });

    describe("_validate", () => {
        it("_Always -> Return Selected Items", () => {
            // arrange
            setFixtures(`<body>
                        <select id="dropdown-product">
                            <option value="1" selected="selected"></option>
                            <option value="2"></option>
                            <option value="3" selected="selected"></option>
                        </select>
                    </body>`);
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");

            // act
            var actualSelectedItems = productMultiSelectionComponent.getSelectedItems();

            // assert
            expect(actualSelectedItems).toEqual(actualSelectedItems);
        });
    });
    describe("_getSelectedItems", () => {
        it("_Validate Before Submit; Reselect Items after Validate Error -> Remove Class Error from dropdown", () => {
            // arrange
            setFixtures(`<body>
                            <select id="dropdown-product">
                                <option value="1" selected="selected"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn error"></div>
                        </body>`);
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            productMultiSelectionComponent.validate();

            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Not Validate Before Submit; Select Items after Validate Error -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <select id="dropdown-product">
                                <option value="1" selected="selected"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn error"></div>
                        </body>`);
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = false;

            // act
            productMultiSelectionComponent.validate();

            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Not Select Items after Validate Error -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-product">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </div>
                            <div class="multiselect btn error"></div>
                        </body>`);
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            productMultiSelectionComponent.validate();

            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Select Items after Validate Success -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-product">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </div>
                            <div class="multiselect btn"></div>
                        </body>`);
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;

            // act
            productMultiSelectionComponent.validate();

            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Validate Before Submit; Select Items after Validate Error;Don't have button select -> Do nothing", () => {
            // arrange
            setFixtures(`<body>
                            <div id="dropdown-product">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </div>
                        </body>`);
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;
            spyOn($.fn, "removeClass");

            // act
            productMultiSelectionComponent.validate();

            // assert
            expect($.fn.removeClass).not.toHaveBeenCalledWith("error");
        });
    });
    describe("_onChange_Always", () => {

        beforeEach(() => {
            // arrange
            setFixtures(`<body><div id="product-ids"></div></body>`);
            spyOn(productMultiSelectionComponent, "validate");
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(() => [1, 2, 3]);
            productMultiSelectionComponent.$hiddenInput = $("#product-ids");

            // act
            productMultiSelectionComponent.onChange(null, false);
        });

        it("_Validate", () => {
            // assert
            expect(productMultiSelectionComponent.validate).toHaveBeenCalled();
        });

        it("_Update hidden input", () => {
            // assert
            expect(productMultiSelectionComponent.$hiddenInput.val()).toBe("1,2,3");
        });
        it("_Calls Onchange Event", () => {
            // arrange
            let onChange = {
                event: () => {
                    console.log(1);
                }
            };

            spyOn(onChange, "event");

            productMultiSelectionComponent.options =
                new ProductMultiSelectionOption("dropdown-product", true, "product-ids", onChange.event);
            productMultiSelectionComponent.onChange(null, false);

            // assert
            expect(productMultiSelectionComponent.options.onChange).toHaveBeenCalled();
        });
    });
    describe("_render", () => {
        describe("_Has Product", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <select id="dropdown-product"
                                    data-has-any-product="true"
                                    data-label-select-products="Select Product"
                                    data-label-selected="# selected"
                                    data-label-all-products="All selected"
                                    data-language="en">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn"></div>
                            <div id="product-ids"></div>
                        </body>`);
                spyOn($.fn, "multiselect");

                // act
                productMultiSelectionComponent.render();
            });

            it("_Bind multi selection", () => {
                // assert
                expect($.fn.multiselect).toHaveBeenCalledWith({
                    maxHeight: 300,
                    numberDisplayed: 1,
                    nonSelectedText: "Select Product",
                    nSelectedText: " # selected",
                    allSelectedText: "All selected",
                    includeSelectAllOption: true,
                    onChange: productMultiSelectionComponent.onChange,
                    onSelectAll: productMultiSelectionComponent.onChange,
                    language: "en"
                });
            });

            it("_Cache element;", () => {
                // assert
                expect(productMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });

        describe("_Not Have Product", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <select id="dropdown-product"
                                    data-has-any-product="false"
                                    data-label-there-is-no-product="No Product Found"
                                    data-label-selected="# selected"
                                    data-label-all-products="All selected"
                                    data-language="en">
                                <option value="1"></option>
                                <option value="2"></option>
                                <option value="3"></option>
                            </select>
                            <div class="multiselect btn"></div>
                            <div id="product-ids"></div>
                        </body>`);
                spyOn($.fn, "multiselect");

                // act
                productMultiSelectionComponent.render();
            });
            it("_Bind multi selection", () => {
                // assert
                expect($.fn.multiselect).toHaveBeenCalledWith({
                    maxHeight: 300,
                    numberDisplayed: 1,
                    nonSelectedText: "No Product Found",
                    nSelectedText: " # selected",
                    allSelectedText: "All selected",
                    includeSelectAllOption: false,
                    onChange: productMultiSelectionComponent.onChange,
                    onSelectAll: productMultiSelectionComponent.onChange,
                    language: "en"
                });
            });
            it("_Cache element;", () => {
                // assert
                expect(productMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
    });
});