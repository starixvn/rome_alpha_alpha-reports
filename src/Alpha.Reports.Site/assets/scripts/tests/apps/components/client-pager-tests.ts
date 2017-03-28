describe("ClientPager", () => {
    const jQueryFake = $;

    let clientPagerComponent: ClientPagerComponent;
    let template = {
        normal: `<body>
                    <div id="report-pager"
                        data-totalrecords="10"
                        data-pagesize="20"
                        data-pageindex="1"
                        data-lablepage="Page"
                        data-labelof="of"
                        data-labeldisplayitems="Display items">
                    </div>
                 </body>`,
        noData: `<body>
                    <div id="report-pager"
                        data-totalrecords="0"
                        data-pagesize="20"
                        data-pageindex="1"
                        data-lablepage="Page"
                        data-labelof="of"
                        data-labeldisplayitems="Display items">
                    </div>
                 </body>`,
        hasData: `<body>
                    <div id="report-pager"
                        data-totalrecords="3"
                        data-pagesize="2"
                        data-pageindex="1"
                        data-lablepage="Page"
                        data-labelof="of"
                        data-labeldisplayitems="Display items">
                    </div>
                    <table id="tbl-report">
                        <tbody>
                            <tr></tr>
                            <tr></tr>
                            <tr></tr>
                        </tbody>
                    </table>
                 </body>`
    };

    beforeEach(() => {
        clientPagerComponent = new ClientPagerComponent(jQueryFake);
    });

    describe("_Render", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake.fn, "pagination");
            spyOn(clientPagerComponent, "showBodyRows");

            // act
            clientPagerComponent.render();
        });

        it("_Init total records", () => {
            // assert
            expect(clientPagerComponent.totalRecords).toBe(10);
        });
        it("_Init page size", () => {
            // assert
            expect(clientPagerComponent.pageSize).toBe(20);
        });
        it("_Init page index", () => {
            // assert
            expect(clientPagerComponent.pageIndex).toBe(1);
        });
        it("_Bind Pager Plugin", () => {
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
        it("_show Body Rows", () => {
            // assert
            expect(clientPagerComponent.showBodyRows).toHaveBeenCalled();
        });
        it("_show pager", () => {
            // arrange
            let pager = $("#report-pager");

            // assert
            expect(pager.hasClass("hide")).toBeFalsy();
        });
    });
    describe("_onSelectPage", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(clientPagerComponent, "updateHiddenFields");
            spyOn(clientPagerComponent, "showBodyRows");
            let pageNumber = 2;
            let pageSize = 50;

            // act
            clientPagerComponent.onSelectPage(pageNumber, pageSize);
        });

        it("_update Hidden Fields", () => {
            // assert
            expect(clientPagerComponent.updateHiddenFields).toHaveBeenCalled();
        });
        it("_show Body Rows", () => {
            // assert
            expect(clientPagerComponent.showBodyRows).toHaveBeenCalled();
        });
    });
    describe("_show Body Rows", () => {
        it("_No Data_Don't add class hide to body rows", () => {
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
        it("_Has Data_Shows and Hide correct Rows", () => {
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
    describe("_update Hidden Fields ", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake.fn, "pagination");
            spyOn(clientPagerComponent, "showBodyRows");
            clientPagerComponent.render();
            let pageNumber = 1;
            let pageSize = 10;

            // act
            clientPagerComponent.updateHiddenFields(pageNumber, pageSize);
        });

        it("_Cache Data", () => {
            // assert
            expect(clientPagerComponent.pageIndex).toBe(1);
            expect(clientPagerComponent.pageSize).toBe(10);
        });
        it("_Store data to hidden fields", () => {
            // assert
            expect(clientPagerComponent.$pageIndex.val()).toBe("1");
            expect(clientPagerComponent.$pageSize.val()).toBe("10");
        });
    });
});