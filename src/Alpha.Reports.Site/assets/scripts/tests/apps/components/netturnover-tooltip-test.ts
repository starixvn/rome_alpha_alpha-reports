describe("MonthPickerComponent_", () => {
    let netTurnoverTooltipComponent: NetTurnoverTooltipComponent;
    let cookieNetTurnoverTooltip = "NetTurnoverTooltipShowCount";
    let maxTimesToShowTooltip = 5;

    beforeEach(() => {
        netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);
    });

    describe("render", () => {
        it("Reach max times to show tooltip -> Removes popup", () => {
            // arrange
            setFixtures(`<body><div id="netturnovertooltip"></div></body>`);

            alphaReports.localStorage.set(cookieNetTurnoverTooltip, maxTimesToShowTooltip);

            // act
            netTurnoverTooltipComponent.render();

            // assert
            expect($("#netturnovertooltip").length).toBe(0);
        });
        it(`Not reach max times to show tooltip -> 
                Positions Net Turnover Tooltip;
                Shows Net Turnover Tooltip Modal;
                Updates Show Count;
                Registers Events;`, () => {
                // arrange
                setFixtures(`<body>
                                <div id="netturnovertooltip"></div>
                                <div id="close-netturnover-tooltip"></div>
                            </body>`);

                spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
                spyOn($.fn, "modal");
                alphaReports.localStorage.set(cookieNetTurnoverTooltip, 0);
                spyOn(netTurnoverTooltipComponent, "registerEvents");

                // act
                netTurnoverTooltipComponent.render();

                // assert
                expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalled();
                expect($.fn.modal).toHaveBeenCalled();
                expect(alphaReports.localStorage.get(cookieNetTurnoverTooltip)).toBe(1);
                expect(netTurnoverTooltipComponent.registerEvents).toHaveBeenCalled();
            });
        it("Not reach max times to show tooltip -> Binds Event to Close Net Turnover Tooltip Button", () => {
            // arrange
            setFixtures(`<body>
                            <div id="netturnovertooltip"></div>
                            <div id="close-netturnover-tooltip"></div>
                        </body>`);
            alphaReports.localStorage.set(cookieNetTurnoverTooltip, 0);
            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
            spyOn(netTurnoverTooltipComponent, "closeNetTurnoverTooltip");
            let netTurnoverTooltip = netTurnoverTooltipComponent.render();

            // act
            $("#close-netturnover-tooltip").trigger("click");

            // assert
            expect(alphaReports.localStorage.get(cookieNetTurnoverTooltip)).toBe(maxTimesToShowTooltip);
            expect(netTurnoverTooltipComponent.closeNetTurnoverTooltip).toHaveBeenCalledWith(netTurnoverTooltip);
        });

        // this test case does not work
        //it("Not reach max times to show tooltip -> Auto close tooltip after 10000", () => {
        //    // arrange
        //    setFixtures(`<body>
        //                    <div id="netturnovertooltip"></div>
        //                    <div id="close-netturnover-tooltip"></div>
        //                </body>`);
        //    spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
        //    spyOn(netTurnoverTooltipComponent, "closeNetTurnoverTooltip");

        //    // act
        //    jasmine.clock().install();
        //    let netTurnoverTooltip = netTurnoverTooltipComponent.render();
        //    jasmine.clock().tick(100001);
        //    jasmine.clock().uninstall();

        //    // assert
        //    expect(netTurnoverTooltipComponent.closeNetTurnoverTooltip).toHaveBeenCalledWith(netTurnoverTooltip);
        //});
    });
});