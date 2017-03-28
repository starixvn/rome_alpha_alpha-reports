describe("StatementDetail", () => {
    const jQueryFake = $;
    let statementDetailView: StatementDetailView;
    let template = {
        normal: `<body>
                    <div id="icon-excel"></div>
                 </body>`
    };

    beforeEach(() => {
        statementDetailView = new StatementDetailView(jQueryFake);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(statementDetailView, "cacheElements");
            spyOn(statementDetailView, "registerEvents");

            // act
            statementDetailView.init();
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(statementDetailView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls registerEvents", () => {
            // assert
            expect(statementDetailView.registerEvents).toHaveBeenCalled();
        });
        it("_Calls blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(template.normal);

            // act
            statementDetailView.cacheElements();

            // assert
            expect(statementDetailView.$exportExcelForm).toBeDefined();
        });
    });
    describe("_registerEvents", () => {
        it("_Bind onExportExcel", () => {
            // arrange
            setFixtures(template.normal);
            statementDetailView.cacheElements();
            spyOn(statementDetailView, "onExportExcel");
            statementDetailView.registerEvents();

            // act
            statementDetailView.$("#icon-excel").trigger("click");

            // assert
            expect(statementDetailView.onExportExcel).toHaveBeenCalled();
        });
    });
    describe("_onExportExcel", () => {
        it("_Bind onExportExcel", () => {
            // arrange
            setFixtures(template.normal);
            statementDetailView.cacheElements();
            spyOn(jQueryFake.fn, "submit");

            // act
            statementDetailView.onExportExcel();

            // assert
            expect(jQueryFake.fn.submit).toHaveBeenCalled();
        });
    });
});