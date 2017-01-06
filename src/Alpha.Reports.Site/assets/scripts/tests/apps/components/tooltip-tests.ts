describe("ReportTooltip", () => {
    let reportTooltip: ReportTooltip;

    let tooltipCookieId = "TooltipShowCount";

    beforeEach(() => {
        reportTooltip = new ReportTooltip($);
    });

    describe("show", () => {
        it("Reach max times to show tooltip -> Do nothing", () => {
            // arrange
            alphaReports.localStorage.set(tooltipCookieId + "-tooltip-0", 5);
            spyOn($.fn, "tooltip");

            // act
            reportTooltip.show();

            // assert
            expect($.fn.tooltip).not.toHaveBeenCalled();
        });
        it("Not reach max times to show tooltip -> Bind tooltip plugin", () => {
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
        it("Not reach max times to show tooltip -> Bind tooltip click event", () => {
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
        it("Test Clock", () => {
            // arrange
            setFixtures(`<body><div id="tooltip"></div></body>`);
            alphaReports.localStorage.set(tooltipCookieId + "-tooltip-0", undefined);
            var tooltipSpy = spyOn($.fn, "tooltip");
            spyOn($.fn, "off").and.callThrough();
            jasmine.clock().install();

            // act
            reportTooltip.show();
            jasmine.clock().tick(5100);

            // assert
            $("#tooltip").trigger("click");
            expect(tooltipSpy.calls.allArgs()).toEqual([
                [{ placement: "bottom" }],
                ["show"],
                ["hide"]
            ]);
            expect($.fn.off).toHaveBeenCalledWith("click.tooltip");
            jasmine.clock().uninstall();
        });
    });
});