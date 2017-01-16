describe("LinkBuilder", () => {
    let linkBuilderComponent: LinkBuilderComponent;

    beforeEach(() => {
        linkBuilderComponent = new LinkBuilderComponent($);
    });

    describe("_render", () => {
        beforeEach(() => {
            // arrange
            setFixtures(`<a data-cust-id="1176086"
                            data-cust-name="agent01"
                            data-cust-level-id="2"
                            href="/site-reports/winlossdetail/master?IsFilterVisible=true"
                            class="downline-link">
                            agent01
                        </a>
                        <a data-cust-id="1176070"
                            data-cust-name="SUPERTEST1"
                            data-cust-level-id="4"
                            class="breadcrumb-link"
                            href="/site-reports/winlossdetail/master?IsFilterVisible=false">
                            SUPERTEST1
                        </a>
                        <input id="BreadcrumbLevelsJson" name="BreadcrumbLevelsJson" type="hidden" value="[{'CustId':1176070,'CustName':'SUPERTEST1','CustLevelId':4},{'CustId':1176083,'CustName':'master1','CustLevelId':3}]">`);
        });

        it("_Always -> Insert CustId, CustName, LevelId, and Breadcrumb Level Json to href attribute for downline link", () => {
            // act
            linkBuilderComponent.render();

            // assert
            expect($(".downline-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=true&CustId=1176086&CustName=agent01&CustLevelId=2&BreadcrumbLevelsJson=%5B%7B'CustId':1176070,'CustName':'SUPERTEST1','CustLevelId':4%7D,%7B'CustId':1176083,'CustName':'master1','CustLevelId':3%7D%5D")
        });

        it("_Always -> Insert CustId, CustName, LevelId, and Breadcrumb Level Json to href attribute for bread crumb link", () => {
            // act
            linkBuilderComponent.render();

            // assert
            expect($(".breadcrumb-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=false&CustId=1176070&CustName=SUPERTEST1&CustLevelId=4&BreadcrumbLevelsJson=%5B%7B'CustId':1176070,'CustName':'SUPERTEST1','CustLevelId':4%7D,%7B'CustId':1176083,'CustName':'master1','CustLevelId':3%7D%5D")
        });
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
            expect($(".downline-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue")
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
            expect($(".downline-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=true&param=paramvalue")
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
            expect($(".breadcrumb-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue")
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
            expect($(".breadcrumb-link").attr("href")).toEqual("/site-reports/winlossdetail/master?IsFilterVisible=false&param=paramvalue")
        });
    });
});