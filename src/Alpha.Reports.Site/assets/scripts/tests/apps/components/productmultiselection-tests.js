describe("ProductMultiSelectionComponent", function () {
    var productMultiSelectionComponent;
    beforeEach(function () {
        productMultiSelectionComponent = new ProductMultiSelectionComponent($);
    });
    describe("_getSelectedItems", function () {
        it("_Always -> Return Selected Items", function () {
            // arrange
            setFixtures("<body>\n                        <select id=\"dropdown-product\">\n                            <option value=\"1\" selected=\"selected\"></option>\n                            <option value=\"2\"></option>\n                            <option value=\"3\" selected=\"selected\"></option>\n                        </select>\n                    </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$excludeProducts = [];
            // act
            var actualSelectedItems = productMultiSelectionComponent.getSelectedItems();
            // assert
            expect(actualSelectedItems).toEqual(actualSelectedItems);
        });
    });
    describe("_validate", function () {
        it("_Validate Before Submit; Reselect Items after Validate Error -> Remove Class Error from dropdown", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-product\">\n                                <option value=\"1\" selected=\"selected\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;
            productMultiSelectionComponent.$excludeProducts = [];
            // act
            productMultiSelectionComponent.validate();
            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Not Validate Before Submit; Select Items after Validate Error -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-product\">\n                                <option value=\"1\" selected=\"selected\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = false;
            // act
            productMultiSelectionComponent.validate();
            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Not Select Items after Validate Error -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-product\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </div>\n                            <div class=\"multiselect btn error\"></div>\n                        </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            productMultiSelectionComponent.validate();
            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Select Items after Validate Success -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-product\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </div>\n                            <div class=\"multiselect btn\"></div>\n                        </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;
            // act
            productMultiSelectionComponent.validate();
            // assert
            expect(productMultiSelectionComponent.$multiSelectButton.hasClass("error")).toBeFalsy();
        });
        it("_Validate Before Submit; Select Items after Validate Error;Don't have button select -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-product\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </div>\n                        </body>");
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
    describe("_onChange_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures("<body><div id=\"product-ids\"></div></body>");
            spyOn(productMultiSelectionComponent, "validate");
            spyOn(productMultiSelectionComponent, "getSelectedItems").and.callFake(function () { return [1, 2, 3]; });
            productMultiSelectionComponent.$hiddenInput = $("#product-ids");
            // act
            productMultiSelectionComponent.onChange(null, false);
        });
        it("_Validate", function () {
            // assert
            expect(productMultiSelectionComponent.validate).toHaveBeenCalled();
        });
        it("_Update hidden input", function () {
            // assert
            expect(productMultiSelectionComponent.$hiddenInput.val()).toBe("1,2,3");
        });
        it("_Calls Onchange Event", function () {
            // arrange
            var onChange = {
                event: function () {
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
    describe("_render", function () {
        describe("_Has Product", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <select id=\"dropdown-product\"\n                                    data-has-any-product=\"true\"\n                                    data-label-select-products=\"Select Product\"\n                                    data-label-selected=\"# selected\"\n                                    data-label-all-products=\"All selected\"\n                                    data-language=\"en\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn\"></div>\n                            <div id=\"product-ids\"></div>\n                        </body>");
                spyOn($.fn, "multiselect");
                // act
                productMultiSelectionComponent.render();
            });
            it("_Bind multi selection", function () {
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
                    language: "en",
                    enableCollapsibleOptGroups: true,
                    optionClass: productMultiSelectionComponent.optionClass
                });
            });
            it("_Cache element;", function () {
                // assert
                expect(productMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
        describe("_Not Have Product", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <select id=\"dropdown-product\"\n                                    data-has-any-product=\"false\"\n                                    data-label-there-is-no-product=\"No Product Found\"\n                                    data-label-selected=\"# selected\"\n                                    data-label-all-products=\"All selected\"\n                                    data-language=\"en\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn\"></div>\n                            <div id=\"product-ids\"></div>\n                        </body>");
                spyOn($.fn, "multiselect");
                // act
                productMultiSelectionComponent.render();
            });
            it("_Bind multi selection", function () {
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
                    language: "en",
                    enableCollapsibleOptGroups: true,
                    optionClass: productMultiSelectionComponent.optionClass
                });
            });
            it("_Cache element;", function () {
                // assert
                expect(productMultiSelectionComponent.$multiSelectButton).toBeDefined();
            });
        });
        describe("_Has Exclude Product", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <label><input type=\"checkbox\" class=\"exclude-product\" value=\"33\"></label>\n                            <input type=\"hidden\" name=\"UserSelectedExcludeProductIds\" id=\"exclude-product-ids\" value=\"\">\n                        </body>");
                spyOn(productMultiSelectionComponent, "excludeProductOnChange");
                // act
                productMultiSelectionComponent.render();
            });
            it("_Cache element;", function () {
                // assert
                expect(productMultiSelectionComponent.$excludeHiddenInput).toBeDefined();
            });
            it("_Bind Exclude Product OnChange Event", function () {
                // act
                $(".exclude-product").change();
                // assert
                expect(productMultiSelectionComponent.excludeProductOnChange).toHaveBeenCalled();
            });
        });
    });
    describe("_getExcludeSelectedItems", function () {
        it("_Always -> Return Exclude Selected Items", function () {
            // arrange
            setFixtures("<body>\n                        <select id=\"dropdown-product\">\n                            <option value=\"1\" selected=\"selected\"></option>\n                            <option value=\"2\"></option>\n                            <option value=\"3\" selected=\"selected\"></option>\n                        </select>\n                        <label><input type=\"checkbox\" class=\"exclude-product\" value=\"33\" checked></label>\n                    </body>");
            productMultiSelectionComponent.$excludeProducts = $(".exclude-product");
            // act
            var actualExcludeSelectedItems = productMultiSelectionComponent.getExcludeSelectedItems();
            // assert
            expect(actualExcludeSelectedItems).toEqual(["33"]);
        });
    });
    describe("_addClassToExcludeProductLabels", function () {
        it("_Always -> Return Class Name Successfully", function () {
            // arrange
            setFixtures("<body>\n                        <label><input type=\"checkbox\" class=\"exclude-product\" value=\"33\"></label>\n                    </body>");
            productMultiSelectionComponent.$excludeProducts = $(".exclude-product");
            // act
            productMultiSelectionComponent.addClassToExcludeProductLabels("error");
            // assert
            expect(productMultiSelectionComponent.$excludeProducts.closest("label").hasClass("error")).toBeTruthy();
        });
    });
    describe("_removeErrorClassFromExcludeProductLabels", function () {
        it("_Always -> Remove .error Class Name Successfully", function () {
            // arrange
            setFixtures("<body>\n                        <label class=\"error\"><input type=\"checkbox\" class=\"exclude-product\" value=\"33\"></label>\n                    </body>");
            productMultiSelectionComponent.$excludeProducts = $(".exclude-product");
            // act
            productMultiSelectionComponent.removeErrorClassFromExcludeProductLabels();
            // assert
            expect(productMultiSelectionComponent.$excludeProducts.closest("label").hasClass("error")).toBeFalsy();
        });
    });
    describe("_validateExcludeProductOnChange", function () {
        it("_Validate Before Submit; Reselect Items after Validate Error -> Remove Class Error from dropdown", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-product\">\n                                <option value=\"1\" selected=\"selected\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                            <label class=\"error\"><input type=\"checkbox\" class=\"exclude-product\" value=\"33\"></label>\n                        </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;
            productMultiSelectionComponent.$excludeProducts = $(".exclude-product");
            // act
            productMultiSelectionComponent.validateExcludeProductOnChange();
            // assert
            expect(productMultiSelectionComponent.$excludeProducts.closest("label").hasClass("error")).toBeFalsy();
        });
        it("_Not Validate Before Submit; Select Items after Validate Error -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-product\">\n                                <option value=\"1\" selected=\"selected\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </select>\n                            <div class=\"multiselect btn error\"></div>\n                            <label class=\"error\"><input type=\"checkbox\" class=\"exclude-product\" value=\"33\"></label>\n                        </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = false;
            productMultiSelectionComponent.$excludeProducts = $(".exclude-product");
            // act
            productMultiSelectionComponent.validateExcludeProductOnChange();
            // assert
            expect(productMultiSelectionComponent.$excludeProducts.closest("label").hasClass("error")).toBeTruthy();
        });
        it("_Validate Before Submit; Select Items after Validate Success -> Do nothing", function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"dropdown-product\">\n                                <option value=\"1\"></option>\n                                <option value=\"2\"></option>\n                                <option value=\"3\"></option>\n                            </div>\n                            <div class=\"multiselect btn\"></div>\n                            <label><input type=\"checkbox\" class=\"exclude-product\" value=\"33\"></label>\n                        </body>");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$multiSelectButton = $(".multiselect.btn");
            productMultiSelectionComponent.options.validateBeforeSubmit = true;
            productMultiSelectionComponent.$excludeProducts = $(".exclude-product");
            // act
            productMultiSelectionComponent.validateExcludeProductOnChange();
            // assert
            expect(productMultiSelectionComponent.$excludeProducts.closest("label").hasClass("error")).toBeFalsy();
        });
    });
    describe("_onChange_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures("<body>\n                            <select id=\"dropdown-product\">\n                                <option value=\"1\"></option>\n                                <option value=\"3\"></option>\n                                <option value=\"5\" selected=\"selected\"></option>\n                            </select>\n                            <input type=\"hidden\" name=\"UserSelectedProductIds\" id=\"product-ids\">\n                            <label class=\"error\"><input type=\"checkbox\" class=\"exclude-product\" value=\"33\" checked></label>\n                            <input type=\"hidden\" name=\"UserSelectedExcludeProductIds\" id=\"exclude-product-ids\">\n                        </body>");
            spyOn(productMultiSelectionComponent, "validateExcludeProductOnChange");
            productMultiSelectionComponent.$dropdownProduct = $("#dropdown-product");
            productMultiSelectionComponent.$hiddenInput = $("#product-ids");
            productMultiSelectionComponent.$excludeProducts = $(".exclude-product");
            productMultiSelectionComponent.$excludeHiddenInput = $("#exclude-product-ids");
            // act
            productMultiSelectionComponent.excludeProductOnChange();
        });
        it("_validateExcludeProductOnChange", function () {
            // assert
            expect(productMultiSelectionComponent.validateExcludeProductOnChange).toHaveBeenCalled();
        });
        it("_Update hidden input", function () {
            // assert
            expect(productMultiSelectionComponent.$hiddenInput.val()).toBe("5,33");
        });
        it("_Update exclude hidden input", function () {
            // assert
            expect(productMultiSelectionComponent.$excludeHiddenInput.val()).toBe("33");
        });
    });
});
//# sourceMappingURL=productmultiselection-tests.js.map