describe("PagerComponent", () => {
    let pagerComponent: PagerComponent;

    beforeEach(() => {
        pagerComponent = new PagerComponent($);
    });

    describe("_hasAllDataOnClient", () => {
        it("_Total Records Not Greater Than Page Size => Returns true", () => {
            // arrange
            pagerComponent.totalRecords = 1;
            pagerComponent.pageSize = 2;

            // act
            var actualHasAllDataOnClient = pagerComponent.hasAllDataOnClient();

            // assert
            expect(actualHasAllDataOnClient).toBeTruthy();
        });
        it("_Total Records  Greater Than Page Size => Returns false", () => {
            // arrange
            pagerComponent.totalRecords = 3;
            pagerComponent.pageSize = 2;

            // act
            var actualHasAllDataOnClient = pagerComponent.hasAllDataOnClient();

            // assert
            expect(actualHasAllDataOnClient).toBeFalsy();
        });
    });
    describe("_isShow", () => {
        it("_Total Records Less Than Min Page Size => Returns false", () => {
            // arrange
            pagerComponent.totalRecords = 1;
            pagerComponent.pageList = [10, 20, 30, 50];

            // act
            var actualIsSHow = pagerComponent.isShow();

            // assert
            expect(actualIsSHow).toBeFalsy();
        });
        it("_Total Records Not Less Than Min Page Size => Returns true", () => {
            // arrange
            pagerComponent.totalRecords = 100;
            pagerComponent.pageList = [10, 20, 30, 50];

            // act
            var actualIsShow = pagerComponent.isShow();

            // assert
            expect(actualIsShow).toBeTruthy();
        });
    });
    describe("_updateHiddenFields", () => {
        beforeEach(() => {
            // arrange
            setFixtures(`<body>
                            <input id="page-index" type="hidden"/>
                            <input id="page-size" type="hidden"/>
                         </body>`);
            pagerComponent.$pageIndex = $("#page-index");
            pagerComponent.$pageSize = $("#page-size");

            // act
            pagerComponent.updateHiddenFields(1, 100);
        });

        it("_Always -> Set values to Page Index", () => {
            // assert
            expect($("#page-index").val()).toBe("1");
        });
        it("_Always -> Set values to Page Size", () => {
            // assert
            expect($("#page-size").val()).toBe("100");
        });
    });
    describe("_onChange", () => {
        it("_Always -> Submit Report", () => {
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
    describe("_render", () => {
        describe("_Not Show Pager", () => {
            let pager: JQuery;
            beforeEach(() => {
                // arrange
                setFixtures(`<body>
                            <div id="report-pager"
                                 data-totalrecords="0"
                                 data-pagesize="100"
                                 data-pageindex="1"></div>
                            <form id="report-form" type="hidden"></form>
                         </body>`);
                pager = $("#report-pager");
                spyOn($.fn, "pagination");

                // act
                pagerComponent.render();
            });

            it("_Add Class Hide to pager", () => {
                // assert
                expect(pager.hasClass("hide")).toBeTruthy();
            });

            it("_Not bind pagination to pager", () => {
                // assert
                expect($.fn.pagination).not.toHaveBeenCalled();
            });
        });

        describe("_Show Pager", () => {
            let pager: JQuery;
            beforeEach(() => {
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
                pager = $("#report-pager");
                spyOn($.fn, "pagination");

                // act
                pagerComponent.render();
            });

            it("_Remove Class Hide from pager", () => {
                // assert
                expect(pager.hasClass("hide")).toBeFalsy();
            });
            it("_Bind pagination to pager", () => {
                // assert
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
    });
    describe("_onSelectPage_Always", () => {
        beforeEach(() => {
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
        });

        it("_Update Hidden Fieldst", () => {
            // assert
            expect(pagerComponent.updateHiddenFields).toHaveBeenCalledWith(2, 1000);
        });

        it("_Call On Change Event", () => {
            // assert
            expect(pagerComponent.onChange).toHaveBeenCalled();
        });
    });
});