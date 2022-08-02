describe("LinkBuilder", function () {
    var linkBuilderComponent;
    beforeEach(function () {
        linkBuilderComponent = new LinkBuilderComponent($);
    });
    describe("_updateParam ", function () {
        it("_Always -> Insert new param to href attribute for downline link", function () {
            // arrange
            setFixtures("<a data-cust-id=\"1176086\"\n                            data-cust-name=\"agent01\"\n                            data-cust-level-id=\"2\"\n                            href=\"/site-reports/winlossdetail/master?IsFilterVisible=true\"\n                            class=\"downline-link\">\n                            agent01\n                        </a>");
            // act
            linkBuilderComponent.updateParam("param", "paramvalue");
            // assert
            expect($(".downline-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue");
        });
        it("_Always -> Update param value to href attribute for downline link", function () {
            // arrange
            setFixtures("<a data-cust-id=\"1176086\"\n                            data-cust-name=\"agent01\"\n                            data-cust-level-id=\"2\"\n                            href=\"/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue1\"\n                            class=\"downline-link\">\n                            agent01\n                        </a>");
            // act
            linkBuilderComponent.updateParam("param", "paramvalue");
            // assert
            expect($(".downline-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue");
        });
        it("_Always -> Insert new param to href attribute for bread crumb link", function () {
            // arrange
            setFixtures("<a data-cust-id=\"1176070\"\n                            data-cust-name=\"SUPERTEST1\"\n                            data-cust-level-id=\"4\"\n                            class=\"breadcrumb-link\"\n                            href=\"/site-reports/winlossdetail/master?IsFilterVisible=false\">\n                            SUPERTEST1\n                        </a>");
            // act
            linkBuilderComponent.updateParam("param", "paramvalue");
            // assert
            expect($(".breadcrumb-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue");
        });
        it("_Always -> Update param to href attribute for bread crumb link", function () {
            // arrange
            setFixtures("<a data-cust-id=\"1176070\"\n                            data-cust-name=\"SUPERTEST1\"\n                            data-cust-level-id=\"4\"\n                            class=\"breadcrumb-link\"\n                            href=\"/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue1\">\n                            SUPERTEST1\n                        </a>");
            // act
            linkBuilderComponent.updateParam("param", "paramvalue");
            // assert
            expect($(".breadcrumb-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue");
        });
    });
    describe("_render", function () {
        it("_Always -> Assign new options", function () {
            // arrange
            var option = new LinkBuilderOption([".action-link"]);
            var expectOption = {
                linkClasses: [".action-link"]
            };
            // act
            linkBuilderComponent.render(option);
            // assert
            expect(linkBuilderComponent.options).toEqual(expectOption);
        });
    });
});
//# sourceMappingURL=link-builder-tests.js.map