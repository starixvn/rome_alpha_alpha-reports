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
    describe("onChange", () => {
        it("Always -> Submit Report", () => {
            // arrange
            setFixtures(`<body>
                            <form id="report-form" type="hidden"></form>
                         </body>`);
            pagerComponent.$reportForm = $("#report-form");
            spyOn(pagerComponent.$reportForm, "trigger");

            // act
            pagerComponent.onChange();

            // assert
            expect(pagerComponent.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
    });
    describe("render", () => {
        it("Not Show Pager -> Add Class Hide to pager; Not bind pagination to pager", () => {
            // arrange
            setFixtures(`<body>
                            <div id="report-pager"
                                 data-totalrecords="0"
                                 data-pagesize="100"
                                 data-pageindex="1"></div>
                            <form id="report-form" type="hidden"></form>
                         </body>`);
            let pager = $("#report-pager");
            spyOn($.fn, "pagination");

            // act
            pagerComponent.render();

            // assert
            expect(pager.hasClass("hide")).toBeTruthy();
            expect($.fn.pagination).not.toHaveBeenCalled();
        });
        it("Show Pager -> Remove Class Hide from pager; Bind pagination to pager", () => {
            // arrange
            setFixtures(`<body>
                            <div id="report-pager"
                                 data-totalrecords="2000"
                                 data-pagesize="1000"
                                 data-pageindex="1"
                                 data-lablepage="Page"
                                 data-labelof="of"
                                 data-labeldisplayitems="Displays"
                                 class="hide"></div>
                            <form id="report-form" type="hidden"></form>
                         </body>`);
            let pager = $("#report-pager");
            spyOn($.fn, "pagination");

            // act
            pagerComponent.render();

            // assert
            expect(pager.hasClass("hide")).toBeFalsy();
            expect($.fn.pagination).toHaveBeenCalledWith({
                pageList: [1000, 2000, 3000, 5000],
                total: 2000,
                pageSize: 1000,
                pageNumber: 1,

                showRefresh: false,
                onSelectPage: jasmine.any(Function),
                beforePageText: "Page",
                afterPageText: "of {pages}",
                displayMsg: "Displays"
            });
        });
    });
    describe("onSelectPage", () => {
        it("Always -> Update Hidden Fields and Call On Change Event", () => {
            // arrange
            setFixtures(`<body>
                            <div id="report-pager"
                                 data-totalrecords="4000"
                                 data-pagesize="1000"
                                 data-pageindex="1"
                                 data-lablepage="Page"
                                 data-labelof="of"
                                 data-labeldisplayitems="Displays"
                                 class="hide"></div>
                            <form id="report-form" type="hidden"></form>
                         </body>`);
            pagerComponent.$reportForm = $("#report-form");
            spyOn(pagerComponent, "updateHiddenFields");
            spyOn(pagerComponent, "onChange");
            pagerComponent.render();

            // act
            $(".pagination-next").trigger("click");

            // assert
            expect(pagerComponent.updateHiddenFields).toHaveBeenCalledWith(2, 1000);
            expect(pagerComponent.onChange).toHaveBeenCalled();
        });
    });
});