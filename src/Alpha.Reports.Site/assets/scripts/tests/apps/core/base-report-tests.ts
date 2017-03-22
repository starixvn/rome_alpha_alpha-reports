describe("BaseReport", () => {
    let baseReport: BaseReport;

    let template = {
        normal: `<body>
                    <div id="icon-information"></div>
                    <input type="hidden" id="LoadLegendURL" value="ReportLegend/Index" />
                </body>`
    };

    beforeEach(() => {
        baseReport = new BaseReport();
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
            spyOn($, "ajax");

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
});