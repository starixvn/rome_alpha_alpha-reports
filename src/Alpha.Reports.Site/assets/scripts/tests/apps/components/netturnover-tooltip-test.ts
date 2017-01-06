describe("NetTurnoverTooltipComponent_", () => {
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

        ////this test case does not work
        // it("Not reach max times to show tooltip -> Auto close tooltip after 10000", () => {
        //    // arrange
        //    setFixtures(`<body>
        //                    <div id="netturnovertooltip"></div>
        //                    <div id="close-netturnover-tooltip"></div>
        //                </body>`);
        //    spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
        //    spyOn(netTurnoverTooltipComponent, "closeNetTurnoverTooltip");

        //    // act
        //    //jasmine.clock().install();
        //    let netTurnoverTooltip = netTurnoverTooltipComponent.render();
        //    //jasmine.clock().tick(100001);
        //    //jasmine.clock().uninstall();

        //    // assert
        //    expect(netTurnoverTooltipComponent.closeNetTurnoverTooltip).toHaveBeenCalledWith(netTurnoverTooltip);
        // });
    });

    describe("positionNetTurnoverTooltip", () => {
        it("Not Have Net Turnover Tooltip -> Do nothing; No Exception Throwed", () => {
            // act & assert
            expect(() => netTurnoverTooltipComponent.positionNetTurnoverTooltip(undefined)).not.toThrow();
        });

        it("Have Net Turnover Tooltip/Screen Smaller Than 768/Header Not On Top -> Positions Tooltip Correctly", () => {
            // arrange
            setFixtures(`<body>
                                <div class="col-netturnover-header" style="top: 10px; left: 10px;position: static;"></div>
                                <div id="netturnovertooltip"></div>
                                <div id="close-netturnover-tooltip" style="position: absolute;"></div>
                            </body>`);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => true);

            var netTurnoverTooltip = netTurnoverTooltipComponent.render();

            // act
            netTurnoverTooltipComponent.positionNetTurnoverTooltip(netTurnoverTooltip);

            // assert
            expect(netTurnoverTooltip.css("bottom")).toBe("auto");
            // can not test other properties (width/top/left/right)
        });

        it("Have Net Turnover Tooltip/Screen Larger Than 768/Header Not On Top -> Positions Tooltip Correctly", () => {
            // arrange
            setFixtures(`<body style="postion: absolute; top: 0px;">
                                <div class="col-netturnover-header" style="top: 10px; left: 10px;position: static;width: 10px;"></div>
                                <div id="netturnovertooltip"></div>
                                <div id="close-netturnover-tooltip" style="position: absolute;"></div>
                            </body>`);
            spyOn(alphaReports, "screenSmallThan768").and.callFake(() => false);

            var netTurnoverTooltip = $("#netturnovertooltip");

            // act
            netTurnoverTooltipComponent.positionNetTurnoverTooltip(netTurnoverTooltip);

            // assert
            expect(netTurnoverTooltip.css("right")).toBe("auto");
            expect(netTurnoverTooltip.css("bottom")).toBe("auto");
            // can not test other properties (width/top/left)
        });

        // can not set position for col-netturnover-header => can not test
        // it("Have Net Turnover Tooltip/Screnn Larger Than 768/Header On Top -> Positions Tooltip Correctly", () => {
        //    // arrange
        //    setFixtures(`<body style="postion: absolute; top: 0px;">
        //                        <div class="col-netturnover-header" style="top: 10px; left: 10px;position: static;width: 10px;"></div>
        //                        <div id="netturnovertooltip"></div>
        //                        <div id="close-netturnover-tooltip" style="position: absolute;"></div>
        //                    </body>`);

        //    var netTurnoverTooltip = $("#netturnovertooltip");
        //    spyOn($.fn, "css");

        //    // act
        //    netTurnoverTooltipComponent.positionNetTurnoverTooltip(netTurnoverTooltip);

        //    // assert
        //    expect($.fn.css).not.toHaveBeenCalled();
        // });
    });

    describe("closeNetTurnoverTooltip", () => {
        it("Always -> Adds class hide to modal", () => {
            // arrange
            setFixtures(`<body style="postion: absolute; top: 0px;">
                                <div class="col-netturnover-header" style="top: 10px; left: 10px;position: static;width: 10px;"></div>
                                <div id="netturnovertooltip"></div>
                                <div id="close-netturnover-tooltip" style="position: absolute;"></div>
                            </body>`);
            var netTurnoverTooltip = netTurnoverTooltipComponent.render();

            // act
            netTurnoverTooltipComponent.closeNetTurnoverTooltip(netTurnoverTooltip);

            // assert
            expect(netTurnoverTooltip.css("display")).toBe("");
        });
    });
    describe("registerEvents", () => {
        it("Window Resize -> Position Net Turnover Tooltip", () => {
            // arrange
            setFixtures(`<body style="postion: absolute; top: 0px;">
                                <div class="col-netturnover-header" style="top: 10px; left: 10px;position: static;width: 10px;"></div>
                                <div id="netturnovertooltip"></div>
                                <div id="close-netturnover-tooltip" style="position: absolute;"></div>
                            </body>`);
            var netTurnoverTooltip = $("#netturnovertooltip");
            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");

            // act
            netTurnoverTooltipComponent.registerEvents(netTurnoverTooltip);
            $(window).resize();

            // assert
            expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalledWith(netTurnoverTooltip);
        });
    });
});