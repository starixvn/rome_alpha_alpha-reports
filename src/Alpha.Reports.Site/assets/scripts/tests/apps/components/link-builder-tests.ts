describe("LinkBuilder", () => {
    let linkBuilderComponent: LinkBuilderComponent;

    beforeEach(() => {
        linkBuilderComponent = new LinkBuilderComponent($);
    });

    describe("_updateParam ", () => {
        it("_Always -> Insert new param to href attribute for downline link", () => {
            // arrange
            setFixtures(`<a data-cust-id="1176086"
                            data-cust-name="agent01"
                            data-cust-level-id="2"
                            href="/site-reports/winlossdetail/master?IsFilterVisible=true"
                            class="downline-link">
                            agent01
                        </a>`);

            // act
            linkBuilderComponent.updateParam("param", "paramvalue");

            // assert
            expect($(".downline-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue");
        });

        it("_Always -> Update param value to href attribute for downline link", () => {
            // arrange
            setFixtures(`<a data-cust-id="1176086"
                            data-cust-name="agent01"
                            data-cust-level-id="2"
                            href="/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue1"
                            class="downline-link">
                            agent01
                        </a>`);

            // act
            linkBuilderComponent.updateParam("param", "paramvalue");

            // assert
            expect($(".downline-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue");
        });

        it("_Always -> Insert new param to href attribute for bread crumb link", () => {
            // arrange
            setFixtures(`<a data-cust-id="1176070"
                            data-cust-name="SUPERTEST1"
                            data-cust-level-id="4"
                            class="breadcrumb-link"
                            href="/site-reports/winlossdetail/master?IsFilterVisible=false">
                            SUPERTEST1
                        </a>`);

            // act
            linkBuilderComponent.updateParam("param", "paramvalue");

            // assert
            expect($(".breadcrumb-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue");
        });

        it("_Always -> Update param to href attribute for bread crumb link", () => {
            // arrange
            setFixtures(`<a data-cust-id="1176070"
                            data-cust-name="SUPERTEST1"
                            data-cust-level-id="4"
                            class="breadcrumb-link"
                            href="/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue1">
                            SUPERTEST1
                        </a>`);

            // act
            linkBuilderComponent.updateParam("param", "paramvalue");

            // assert
            expect($(".breadcrumb-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue");
        });
    });

    describe("_render", () => {
        it("_Always -> Assign new options", () => {
            // arrange
            let option = new LinkBuilderOption([".action-link"]);
            let expectOption = {
                linkClasses: [".action-link"]
            };

            // act
            linkBuilderComponent.render(option);

            // assert
            expect(linkBuilderComponent.options).toEqual(expectOption);
        });
    });
});