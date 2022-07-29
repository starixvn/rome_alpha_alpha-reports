describe("ClientPager", function () {
    var jQueryFake = $;
    var clientPagerComponent;
    var template = {
        normal: "<body>\n                    <div id=\"report-pager\"\n                        data-totalrecords=\"10\"\n                        data-pagesize=\"20\"\n                        data-pageindex=\"1\"\n                        data-lablepage=\"Page\"\n                        data-labelof=\"of\"\n                        data-labeldisplayitems=\"Display items\">\n                    </div>\n                 </body>",
        noData: "<body>\n                    <div id=\"report-pager\"\n                        data-totalrecords=\"0\"\n                        data-pagesize=\"20\"\n                        data-pageindex=\"1\"\n                        data-lablepage=\"Page\"\n                        data-labelof=\"of\"\n                        data-labeldisplayitems=\"Display items\">\n                    </div>\n                 </body>",
        hasData: "<body>\n                    <div id=\"report-pager\"\n                        data-totalrecords=\"3\"\n                        data-pagesize=\"2\"\n                        data-pageindex=\"1\"\n                        data-lablepage=\"Page\"\n                        data-labelof=\"of\"\n                        data-labeldisplayitems=\"Display items\">\n                    </div>\n                    <table id=\"tbl-report\">\n                        <tbody>\n                            <tr></tr>\n                            <tr></tr>\n                            <tr></tr>\n                        </tbody>\n                    </table>\n                 </body>"
    };
    beforeEach(function () {
        clientPagerComponent = new ClientPagerComponent(jQueryFake);
    });
    describe("_Render", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake.fn, "pagination");
            spyOn(clientPagerComponent, "showBodyRows");
            // act
            clientPagerComponent.render();
        });
        it("_Init total records", function () {
            // assert
            expect(clientPagerComponent.totalRecords).toBe(10);
        });
        it("_Init page size", function () {
            // assert
            expect(clientPagerComponent.pageSize).toBe(20);
        });
        it("_Init page index", function () {
            // assert
            expect(clientPagerComponent.pageIndex).toBe(1);
        });
        it("_Bind Pager Plugin", function () {
            // assert
            expect(jQueryFake.fn.pagination).toHaveBeenCalledWith({
                pageList: [1000, 2000, 3000, 5000],
                total: 10,
                pageSize: 20,
                pageNumber: 1,
                showRefresh: false,
                onSelectPage: clientPagerComponent.onSelectPage,
                beforePageText: "Page",
                afterPageText: "of {pages}",
                displayMsg: "Display items"
            });
        });
        it("_show Body Rows", function () {
            // assert
            expect(clientPagerComponent.showBodyRows).toHaveBeenCalled();
        });
        it("_show pager", function () {
            // arrange
            var pager = $("#report-pager");
            // assert
            expect(pager.hasClass("hide")).toBeFalsy();
        });
    });
    describe("_onSelectPage", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            spyOn(clientPagerComponent, "updateHiddenFields");
            spyOn(clientPagerComponent, "showBodyRows");
            var pageNumber = 2;
            var pageSize = 50;
            // act
            clientPagerComponent.onSelectPage(pageNumber, pageSize);
        });
        it("_update Hidden Fields", function () {
            // assert
            expect(clientPagerComponent.updateHiddenFields).toHaveBeenCalled();
        });
        it("_show Body Rows", function () {
            // assert
            expect(clientPagerComponent.showBodyRows).toHaveBeenCalled();
        });
    });
    describe("_show Body Rows", function () {
        it("_No Data_Don't add class hide to body rows", function () {
            // arrange
            setFixtures(template.noData);
            spyOn(jQueryFake.fn, "addClass");
            spyOn(jQueryFake.fn, "pagination");
            clientPagerComponent.render();
            // act
            clientPagerComponent.showBodyRows();
            // assert
            expect(jQueryFake.fn.addClass).not.toHaveBeenCalledWith("hide");
        });
        it("_Has Data_Shows and Hide correct Rows", function () {
            // arrange
            setFixtures(template.hasData);
            spyOn(jQueryFake.fn, "pagination");
            clientPagerComponent.render();
            // act
            clientPagerComponent.showBodyRows();
            // assert
            expect(!$(clientPagerComponent.$bodyRows[0]).hasClass("hide")
                && !$(clientPagerComponent.$bodyRows[1]).hasClass("hide")
                && $(clientPagerComponent.$bodyRows[2]).hasClass("hide")).toBeTruthy();
        });
    });
    describe("_update Hidden Fields ", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake.fn, "pagination");
            spyOn(clientPagerComponent, "showBodyRows");
            clientPagerComponent.render();
            var pageNumber = 1;
            var pageSize = 10;
            // act
            clientPagerComponent.updateHiddenFields(pageNumber, pageSize);
        });
        it("_Cache Data", function () {
            // assert
            expect(clientPagerComponent.pageIndex).toBe(1);
            expect(clientPagerComponent.pageSize).toBe(10);
        });
        it("_Store data to hidden fields", function () {
            // assert
            expect(clientPagerComponent.$pageIndex.val()).toBe("1");
            expect(clientPagerComponent.$pageSize.val()).toBe("10");
        });
    });
});
//# sourceMappingURL=client-pager-tests.js.map