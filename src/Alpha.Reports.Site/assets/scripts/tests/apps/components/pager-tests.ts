describe("NetTurnoverTooltipComponent_", () => {
    let pagerComponent: PagerComponent;

    beforeEach(() => {
        pagerComponent = new PagerComponent($);
    });

    describe("hasAllDataOnClient", () => {
        it("Total Records Not Greater Than Page Size => Returns true", () => {
            // arrange
            pagerComponent.totalRecords = 1;
            pagerComponent.pageSize = 2;

            // act
            var actualHasAllDataOnClient = pagerComponent.hasAllDataOnClient();

            // assert
            expect(actualHasAllDataOnClient).toBeTruthy();
        });
        it("Total Records  Greater Than Page Size => Returns false", () => {
            // arrange
            pagerComponent.totalRecords = 3;
            pagerComponent.pageSize = 2;

            // act
            var actualHasAllDataOnClient = pagerComponent.hasAllDataOnClient();

            // assert
            expect(actualHasAllDataOnClient).toBeFalsy();
        });
    });
    describe("isShow", () => {
        it("Total Records Less Than Min Page Size => Returns false", () => {
            // arrange
            pagerComponent.totalRecords = 1;
            pagerComponent.pageList = [10, 20, 30, 50];

            // act
            var actualIsSHow = pagerComponent.isShow();

            // assert
            expect(actualIsSHow).toBeFalsy();
        });
        it("Total Records Not Less Than Min Page Size => Returns true", () => {
            // arrange
            pagerComponent.totalRecords = 100;
            pagerComponent.pageList = [10, 20, 30, 50];

            // act
            var actualIsShow = pagerComponent.isShow();

            // assert
            expect(actualIsShow).toBeTruthy();
        });
    });
    describe("updateHiddenFields", () => {
        it("Always -> Set values to hidden fields", () => {
            // arrange
            setFixtures(`<body>
                            <input id="page-index" type="hidden"/>
                            <input id="page-size" type="hidden"/>
                         </body>`);
            pagerComponent.$pageIndex = $("#page-index");
            pagerComponent.$pageSize = $("#page-size");

            // act
            pagerComponent.updateHiddenFields(1, 100);

            // assert
            expect($("#page-index").val()).toBe("1");
            expect($("#page-size").val()).toBe("100");
        });
    });
});