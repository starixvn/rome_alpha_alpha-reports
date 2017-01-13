describe("ReportTooltip", () => {
    let reportTooltip: ReportTooltip;

    let tooltipCookieId = "TooltipShowCount";

    beforeEach(() => {
        reportTooltip = new ReportTooltip($);
    });

    describe("_show", () => {
        it("_Reach max times to show tooltip -> Do nothing", () => {
            // arrange
            alphaReports.localStorage.set(tooltipCookieId + "-tooltip-0", 5);
            spyOn($.fn, "tooltip");

            // act
            reportTooltip.show();

            // assert
            expect($.fn.tooltip).not.toHaveBeenCalled();
        });
        it("_Not reach max times to show tooltip -> Bind tooltip plugin", () => {
            // arrange
            alphaReports.localStorage.set(tooltipCookieId + "-tooltip-0", undefined);
            var tooltipSpy = spyOn($.fn, "tooltip");

            // act
            reportTooltip.show();

            // assert
            expect(tooltipSpy.calls.allArgs()).toEqual([
                [{ placement: "bottom" }],
                ["show"]
            ]);
        });
        it("_Not reach max times to show tooltip -> Bind tooltip click event", () => {
            // arrange
            setFixtures(`<body><div id="tooltip"></div></body>`);
            alphaReports.localStorage.set(tooltipCookieId + "-tooltip-0", undefined);
            var tooltipSpy = spyOn($.fn, "tooltip");
            reportTooltip.show();

            // act
            $("#tooltip").trigger("click");

            // assert
            expect(tooltipSpy.calls.allArgs()).toEqual([
                [{ placement: "bottom" }],
                ["show"],
                ["hide"]
            ]);
        });
        it("_Not reach max times to show tooltip -> Auto Close tooltip after 5s", () => {
            // arrange
            setFixtures(`<body><div id="tooltip"></div></body>`);
            alphaReports.localStorage.set(tooltipCookieId + "-tooltip-0", undefined);
            var tooltipSpy = spyOn($.fn, "tooltip");
            jasmine.clock().install();

            // act
            reportTooltip.show();
            jasmine.clock().tick(5100);

            // assert
            expect(tooltipSpy.calls.allArgs()).toEqual([
                [{ placement: "bottom" }],
                ["show"],
                ["hide"]
            ]);
            jasmine.clock().uninstall();
        });
        it("_Not reach max times to show tooltip/Click on tooltip -> Call click event", () => {
            // arrange
            setFixtures(`<body><div id="tooltip"></div></body>`);
            alphaReports.localStorage.set(tooltipCookieId + "-tooltip-0", undefined);
            var tooltipSpy = spyOn($.fn, "tooltip");
            spyOn($.fn, "off").and.callThrough();

            // act
            reportTooltip.show();

            // assert
            $("#tooltip").trigger("click");
            expect(tooltipSpy.calls.allArgs()).toEqual([
                [{ placement: "bottom" }],
                ["show"],
                ["hide"]
            ]);
            expect($.fn.off).toHaveBeenCalledWith("click.tooltip");
        });
    });
});