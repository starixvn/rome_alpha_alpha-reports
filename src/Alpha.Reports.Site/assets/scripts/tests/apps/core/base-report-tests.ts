describe("BaseReport", () => {
    const jQueryFake = $;
    const linkBuilderComponent = new LinkBuilderComponent($);
    let baseReport: BaseReport;

    let template = {
        normal: `<body>
                    <div id="icon-information"></div>
                    <input type="hidden" id="LoadLegendURL" value="ReportLegend/Index" />
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
        baseReport = new BaseReport(jQueryFake, linkBuilderComponent);
        spyOn(baseReport, "onClickIconFilter").and.callThrough();
        spyOn($, "ajax");
    });

    describe("_initHelp", () => {
        it("_Always -> Bind Click to show legend event", () => {
            // arrange
            spyOn(baseReport, "showLegend");
            setFixtures(template.normal);

            // act
            baseReport.initHelp();
            baseReport.$iconInformation.trigger("click");

            // assert
            expect(baseReport.showLegend).toHaveBeenCalled();
        });
        it("_Always -> load Legend", () => {
            // arrange
            spyOn(baseReport, "loadLegend");
            setFixtures(template.normal);

            // act
            baseReport.initHelp();

            // assert
            expect(baseReport.loadLegend).toHaveBeenCalled();
        });
    });

    describe("_loadLegend", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            baseReport.initHelp();
            spyOn($, "blockUI");

            // act
            baseReport.loadLegend();
        });

        it("_Always -> call ajax", () => { // assert
            expect($.ajax).toHaveBeenCalledWith({
                method: "GET",
                url: "ReportLegend/Index",
                cache: false,
                data: {},
                success: baseReport.renderLegend,
                complete: $.unblockUI
            });
        });
    });

    describe("_renderLegend", () => {
        describe("_Not Load Legend Before", () => {
            let newLegend = `<div id="report-legend">New Legend</div>`;

            beforeEach(() => {
                // arrange
                setFixtures(template.normal);
                baseReport.initHelp();
                spyOn($.fn, "modal");
                spyOn($.fn, "css");
                spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });

                // act
                baseReport.renderLegend(newLegend);
            });

            it("_Append New Legend to Body", () => {
                // assert
                expect($("#report-legend").length).toBe(1);
            });
            it("_Show icon information", () => {
                // assert
                expect(baseReport.$iconInformationContainer.hasClass("hide")).toBeFalsy();
            });
        });
        describe("_Already Load Legend Before", () => {
            let newLegend = `<div id="report-legend">New Legend</div>`;

            beforeEach(() => {
                // arrange
                setFixtures(template.normal);
                baseReport.initHelp();
                spyOn($.fn, "modal");
                spyOn($.fn, "css");
                spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });
                baseReport.renderLegend(`<div id="report-legend" class=""old-legend">Old Legend</div>`);

                // act
                baseReport.renderLegend(newLegend);
            });

            it("_Remove Old Legend and Append New Legend to Body", () => {
                // assert
                expect($("#report-legend.old-legend").length).toBe(0);
                expect($("#report-legend").length).toBe(1);
            });
            it("_Show icon information", () => {
                // assert
                expect(baseReport.$iconInformationContainer.hasClass("hide")).toBeFalsy();
            });
        });
        describe("_Not Load Legend Before/New Legend is Empty", () => {
            let newLegend = ` `;

            beforeEach(() => {
                // arrange
                setFixtures(template.normal);
                baseReport.initHelp();
                spyOn($.fn, "modal");
                spyOn($.fn, "css");
                spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });

                // act
                baseReport.renderLegend(newLegend);
            });

            it("_Remove icon information", () => {
                // assert
                expect($("#icon-information-container").length).toBe(0);
            });
        });
    });
    describe("_showLegend", () => {
        let newLegend = `<div id="report-legend">New Legend</div>`;

        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            baseReport.initHelp();
            spyOn($.fn, "modal");
            spyOn($.fn, "css");
            spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });
            baseReport.renderLegend(newLegend);

            // act
            baseReport.showLegend();
        });

        it("_Always_Show Popup", () => {
            // assert
            expect($.fn.modal).toHaveBeenCalledWith({ show: true, backdrop: false });
        });
        it("_Always_Position Legend Popup by icon help", () => {
            // assert
            expect($.fn.css).toHaveBeenCalledWith({
                left: 20,
                right: "auto",
                bottom: "auto",
                top: 100,
                width: "auto"
            });
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // act
            setFixtures(template.normal);
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
            setFixtures(template.normal);
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
                setFixtures(template.normal);
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