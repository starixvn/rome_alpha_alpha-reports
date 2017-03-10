describe("BaseReport", () => {
    const jQueryFake = $;
    const linkBuilderComponent = new LinkBuilderComponent($);
    let baseReport: BaseReport;
    let template = {
        shownFilter: `<body>
                        <div class="page-title"></div>
                        <form id="report-form"></form>
                        <div id="tbl-report"></div>
                        <div class="icon-filter"></div>
                        <input id="IsFilterVisible" type="hidden" />
                        <form id="export-excel-form"></form>
                     </body>`,
        hiddenFilter: `<body>
                        <div class="page-title"></div>
                        <form id="report-form" class="hide"></form>
                        <div id="tbl-report"></div>
                        <div class="icon-filter"></div>
                        <input id="IsFilterVisible" type="hidden" />
                        <form id="export-excel-form"></form>
                     </body>`
    };

    beforeEach(() => {
        baseReport = new BaseReport($, linkBuilderComponent);
        spyOn(baseReport, "onClickIconFilter").and.callThrough();
    });

    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // act
            setFixtures(template.shownFilter);
            baseReport.cacheElements();

            // assert
            expect(baseReport.$reportForm).toBeDefined();
            expect(baseReport.$pageTitle).toBeDefined();
            expect(baseReport.$tableReport).toBeDefined();
            expect(baseReport.$isFilterVisible).toBeDefined();
            expect(baseReport.$exportExcelForm).toBeDefined();
        });
    });
    describe("_registerEvents", () => {
        it("_Always_Bind onClickIconFilter", () => {
            // arrange
            setFixtures(template.shownFilter);
            baseReport.cacheElements();
            baseReport.registerEvents();

            // act
            $(".icon-filter").trigger("click");

            // assert
            expect(baseReport.onClickIconFilter).toHaveBeenCalled();
        });
    });
    describe("_onClickIconFilter", () => {
        describe("_Filter is currently shown", () => {
            beforeEach(() => {
                // arrange
                setFixtures(template.shownFilter);
                baseReport.cacheElements();
                spyOn(baseReport, "replaceHrefParam");

                // act
                baseReport.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(baseReport.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(baseReport.$reportForm.hasClass("hide")).toBeTruthy();
            });
            it("_Update Href Param to False", () => {
                // assert
                expect(baseReport.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
            });
        });
        describe("_Filter is currently hidden", () => {
            beforeEach(() => {
                // arrange
                setFixtures(template.hiddenFilter);
                baseReport.cacheElements();
                spyOn(baseReport, "replaceHrefParam");

                // act
                baseReport.onClickIconFilter();
            });
            it("_Hides Filter", () => {
                // assert
                expect(baseReport.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(baseReport.$reportForm.hasClass("hide")).toBeFalsy();
            });
            it("_Update Href Param to True", () => {
                // assert
                expect(baseReport.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
            });
        });
    });
    it("_replaceHrefParam_Always_Call linkBuilderComponent update param", () => {
        // arrange
        let paramName = "paramName";
        let paramValue = "paramValue";
        spyOn(linkBuilderComponent, "updateParam");

        // act
        baseReport.replaceHrefParam(paramName, paramValue);

        // assert
        expect(linkBuilderComponent.updateParam).toHaveBeenCalledWith("paramName", "paramValue");
    });
});