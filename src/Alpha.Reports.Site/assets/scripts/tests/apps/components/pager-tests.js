describe("PagerComponent", function () {
    var pagerComponent;
    beforeEach(function () {
        pagerComponent = new PagerComponent($);
    });
    describe("_hasAllDataOnClient", function () {
        it("_Total Records Not Greater Than Page Size => Returns true", function () {
            // arrange
            pagerComponent.totalRecords = 1;
            pagerComponent.pageSize = 2;
            // act
            var actualHasAllDataOnClient = pagerComponent.hasAllDataOnClient();
            // assert
            expect(actualHasAllDataOnClient).toBeTruthy();
        });
        it("_Total Records  Greater Than Page Size => Returns false", function () {
            // arrange
            pagerComponent.totalRecords = 3;
            pagerComponent.pageSize = 2;
            // act
            var actualHasAllDataOnClient = pagerComponent.hasAllDataOnClient();
            // assert
            expect(actualHasAllDataOnClient).toBeFalsy();
        });
    });
    describe("_isShow", function () {
        it("_Total Records Less Than Min Page Size => Returns false", function () {
            // arrange
            pagerComponent.totalRecords = 1;
            pagerComponent.pageList = [10, 20, 30, 50];
            // act
            var actualIsSHow = pagerComponent.isShow();
            // assert
            expect(actualIsSHow).toBeFalsy();
        });
        it("_Total Records Not Less Than Min Page Size => Returns true", function () {
            // arrange
            pagerComponent.totalRecords = 100;
            pagerComponent.pageList = [10, 20, 30, 50];
            // act
            var actualIsShow = pagerComponent.isShow();
            // assert
            expect(actualIsShow).toBeTruthy();
        });
    });
    describe("_updateHiddenFields", function () {
        beforeEach(function () {
            // arrange
            setFixtures("<body>\n                            <input id=\"page-index\" type=\"hidden\"/>\n                            <input id=\"page-size\" type=\"hidden\"/>\n                         </body>");
            pagerComponent.$pageIndex = $("#page-index");
            pagerComponent.$pageSize = $("#page-size");
            // act
            pagerComponent.updateHiddenFields(1, 100);
        });
        it("_Always -> Set values to Page Index", function () {
            // assert
            expect($("#page-index").val()).toBe("1");
        });
        it("_Always -> Set values to Page Size", function () {
            // assert
            expect($("#page-size").val()).toBe("100");
        });
    });
    describe("_onChange", function () {
        it("_Always -> Submit Report", function () {
            // arrange
            setFixtures("<body>\n                            <form id=\"report-form\" type=\"hidden\"></form>\n                         </body>");
            pagerComponent.$reportForm = $("#report-form");
            spyOn(pagerComponent.$reportForm, "trigger");
            // act
            pagerComponent.onChange();
            // assert
            expect(pagerComponent.$reportForm.trigger).toHaveBeenCalledWith("submit");
        });
    });
    describe("_render", function () {
        describe("_Not Show Pager", function () {
            var pager;
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <div id=\"report-pager\"\n                                 data-totalrecords=\"0\"\n                                 data-pagesize=\"100\"\n                                 data-pageindex=\"1\"></div>\n                            <form id=\"report-form\" type=\"hidden\"></form>\n                         </body>");
                pager = $("#report-pager");
                spyOn($.fn, "pagination");
                // act
                pagerComponent.render();
            });
            it("_Add Class Hide to pager", function () {
                // assert
                expect(pager.hasClass("hide")).toBeTruthy();
            });
            it("_Not bind pagination to pager", function () {
                // assert
                expect($.fn.pagination).not.toHaveBeenCalled();
            });
        });
        describe("_Show Pager", function () {
            var pager;
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                            <div id=\"report-pager\"\n                                 data-totalrecords=\"2000\"\n                                 data-pagesize=\"1000\"\n                                 data-pageindex=\"1\"\n                                 data-lablepage=\"Page\"\n                                 data-labelof=\"of\"\n                                 data-labeldisplayitems=\"Displays\"\n                                 class=\"hide\"></div>\n                            <form id=\"report-form\" type=\"hidden\"></form>\n                         </body>");
                pager = $("#report-pager");
                spyOn($.fn, "pagination");
                // act
                pagerComponent.render();
            });
            it("_Remove Class Hide from pager", function () {
                // assert
                expect(pager.hasClass("hide")).toBeFalsy();
            });
            it("_Bind pagination to pager", function () {
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
    describe("_onSelectPage_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures("<body>\n                            <div id=\"report-pager\"\n                                 data-totalrecords=\"4000\"\n                                 data-pagesize=\"1000\"\n                                 data-pageindex=\"1\"\n                                 data-lablepage=\"Page\"\n                                 data-labelof=\"of\"\n                                 data-labeldisplayitems=\"Displays\"\n                                 class=\"hide\"></div>\n                            <form id=\"report-form\" type=\"hidden\"></form>\n                         </body>");
            pagerComponent.$reportForm = $("#report-form");
            spyOn(pagerComponent, "updateHiddenFields");
            spyOn(pagerComponent, "onChange");
            pagerComponent.render();
            // act
            $(".pagination-next").trigger("click");
        });
        it("_Update Hidden Fieldst", function () {
            // assert
            expect(pagerComponent.updateHiddenFields).toHaveBeenCalledWith(2, 1000);
        });
        it("_Call On Change Event", function () {
            // assert
            expect(pagerComponent.onChange).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=pager-tests.js.map