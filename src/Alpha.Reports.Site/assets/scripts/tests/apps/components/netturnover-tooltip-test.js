describe("NetTurnoverTooltipComponent", function () {
    var netTurnoverTooltipComponent;
    var cookieNetTurnoverTooltip = "NetTurnoverTooltipShowCount";
    var maxTimesToShowTooltip = 5;
    beforeEach(function () {
        netTurnoverTooltipComponent = new NetTurnoverTooltipComponent($);
    });
    describe("_render", function () {
        it("_Reach max times to show tooltip -> Removes popup", function () {
            // arrange
            setFixtures("<body><div id=\"netturnovertooltip\"></div></body>");
            alphaReports.localStorage.set(cookieNetTurnoverTooltip, maxTimesToShowTooltip);
            // act
            netTurnoverTooltipComponent.render();
            // assert
            expect($("#netturnovertooltip").length).toBe(0);
        });
        describe("_Not reach max times to show tooltip", function () {
            var netTurnoverTooltip;
            beforeEach(function () {
                // arrange
                setFixtures("<body>\n                                <div id=\"netturnovertooltip\"></div>\n                                <div id=\"close-netturnover-tooltip\"></div>\n                            </body>");
                spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
                spyOn($.fn, "modal");
                alphaReports.localStorage.set(cookieNetTurnoverTooltip, 0);
                spyOn(netTurnoverTooltipComponent, "registerEvents");
                // act
                netTurnoverTooltip = netTurnoverTooltipComponent.render();
            });
            it("_Positions Net Turnover Tooltip;", function () {
                // assert
                expect(netTurnoverTooltipComponent.positionNetTurnoverTooltip).toHaveBeenCalled();
            });
            it("_Shows Net Turnover Tooltip Modal;", function () {
                // assert
                expect($.fn.modal).toHaveBeenCalled();
            });
            it("_Updates Show Count;", function () {
                // assert
                expect(alphaReports.localStorage.get(cookieNetTurnoverTooltip)).toBe(1);
            });
            it("_Registers Events;", function () {
                // assert
                expect(netTurnoverTooltipComponent.registerEvents).toHaveBeenCalled();
            });
            it("_Binds Event to Close Net Turnover Tooltip Button", function () {
                // arrange 
                spyOn(netTurnoverTooltipComponent, "closeNetTurnoverTooltip");
                // act
                $("#close-netturnover-tooltip").trigger("click");
                // assert
                expect(alphaReports.localStorage.get(cookieNetTurnoverTooltip)).toBe(maxTimesToShowTooltip);
                expect(netTurnoverTooltipComponent.closeNetTurnoverTooltip).toHaveBeenCalledWith(netTurnoverTooltip);
            });
        });
        it("_Not reach max times to show tooltip -> Auto close tooltip after 10000", function () {
            // arrange
            jasmine.clock().install();
            setFixtures("<body>\n                            <div id=\"netturnovertooltip\"></div>\n                            <div id=\"close-netturnover-tooltip\"></div>\n                        </body>");
            spyOn(netTurnoverTooltipComponent, "positionNetTurnoverTooltip");
            spyOn(netTurnoverTooltipComponent, "closeNetTurnoverTooltip");
            alphaReports.localStorage.set(cookieNetTurnoverTooltip, 0);
            // act
            var netTurnoverTooltip = netTurnoverTooltipComponent.render();
            jasmine.clock().tick(10001);
            // assert
            expect(netTurnoverTooltipComponent.closeNetTurnoverTooltip).toHaveBeenCalledWith(netTurnoverTooltip);
            jasmine.clock().uninstall();
        });
    });
    describe("_positionNetTurnoverTooltip", function () {
        it("_Not Have Net Turnover Tooltip -> Do nothing; No Exception Throwed", function () {
            // act & assert
            expect(function () { return netTurnoverTooltipComponent.positionNetTurnoverTooltip(undefined); }).not.toThrow();
        });
        it("_Have Net Turnover Tooltip/Screen Smaller Than 768/Header Not On Top -> Positions Tooltip Correctly", function () {
            // arrange
            setFixtures("<body>\n                                <div class=\"col-netturnover-header\" style=\"top: 10px; left: 10px;position: static;\"></div>\n                                <div id=\"netturnovertooltip\"></div>\n                                <div id=\"close-netturnover-tooltip\" style=\"position: absolute;\"></div>\n                            </body>");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return true; });
            var netTurnoverTooltip = netTurnoverTooltipComponent.render();
            // act
            netTurnoverTooltipComponent.positionNetTurnoverTooltip(netTurnoverTooltip);
            // assert
            expect(netTurnoverTooltip.css("bottom")).toBe("auto");
            // can not test other properties (width/top/left/right)
        });
        it("_Have Net Turnover Tooltip/Screen Larger Than 768/Header Not On Top -> Positions Tooltip Correctly", function () {
            // arrange
            setFixtures("<body style=\"postion: absolute; top: 0px;\">\n                                <div class=\"col-netturnover-header\" style=\"top: 10px; left: 10px;position: static;width: 10px;\"></div>\n                                <div id=\"netturnovertooltip\"></div>\n                                <div id=\"close-netturnover-tooltip\" style=\"position: absolute;\"></div>\n                            </body>");
            spyOn(alphaReports, "screenSmallThan768").and.callFake(function () { return false; });
            var netTurnoverTooltip = $("#netturnovertooltip");
            // act
            netTurnoverTooltipComponent.positionNetTurnoverTooltip(netTurnoverTooltip);
            // assert
            expect(netTurnoverTooltip.css("right")).toBe("auto");
            expect(netTurnoverTooltip.css("bottom")).toBe("auto");
            // can not test other properties (width/top/left)
        });
        // can not set position for col-netturnover-header => can not test
        // it("_Have Net Turnover Tooltip/Screnn Larger Than 768/Header On Top -> Positions Tooltip Correctly", () => {
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
    describe("_closeNetTurnoverTooltip", function () {
        it("_Always -> Adds class hide to modal", function () {
            // arrange
            setFixtures("<body style=\"postion: absolute; top: 0px;\">\n                                <div class=\"col-netturnover-header\" style=\"top: 10px; left: 10px;position: static;width: 10px;\"></div>\n                                <div id=\"netturnovertooltip\"></div>\n                                <div id=\"close-netturnover-tooltip\" style=\"position: absolute;\"></div>\n                            </body>");
            var netTurnoverTooltip = netTurnoverTooltipComponent.render();
            // act
            netTurnoverTooltipComponent.closeNetTurnoverTooltip(netTurnoverTooltip);
            // assert
            expect(netTurnoverTooltip.css("display")).toBe("none");
        });
    });
    describe("_registerEvents", function () {
        it("_Window Resize -> Position Net Turnover Tooltip", function () {
            // arrange
            setFixtures("<body style=\"postion: absolute; top: 0px;\">\n                                <div class=\"col-netturnover-header\" style=\"top: 10px; left: 10px;position: static;width: 10px;\"></div>\n                                <div id=\"netturnovertooltip\"></div>\n                                <div id=\"close-netturnover-tooltip\" style=\"position: absolute;\"></div>\n                            </body>");
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
//# sourceMappingURL=netturnover-tooltip-test.js.map