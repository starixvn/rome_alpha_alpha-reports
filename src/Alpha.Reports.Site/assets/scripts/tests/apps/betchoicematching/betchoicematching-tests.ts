describe("BetChoiceMatching", () => {
    const jQueryFake = $;
    let betChoiceMatching: BetChoiceMatching;
    const betMatchingResources: Resources = {
        Total: "Total",
        Versus: "Versus",
        Over: "Over",
        Under: "Under",
        Upline: "Upline",
        IP_used_by: "IP_used_by",
        BetMatching_Username_Title: "BetMatching_Username_Title",
        BetMatching_IP_Title: "BetMatching_IP_Title",
        FilterByUsername: "FilterByUsername",
        FilterByStake: "FilterByStake",
        FilterByMemberWinLoss: "FilterByMemberWinLoss",
        FilterByUsername_Placeholder: "FilterByUsername_Placeholder",
        TotalStake: "TotalStake",
        Filter: "Filter",
        TotalMemberWL: "TotalMemberWL"
    };

    beforeEach(() => {
        betChoiceMatching = new BetChoiceMatching(jQueryFake);
        spyOn(betChoiceMatching, "cacheElements").and.callThrough();
        spyOn(betChoiceMatching, "registerEvents").and.callThrough();
        spyOn(betChoiceMatching, "submitOnLoad").and.callThrough();
        spyOn(window, "betMatchingResources").and.returnValue(betMatchingResources);
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        setFixtures(`<body>
                        <form id="report-form" role="form">
                            <input id="IsDescendingSort" value="False" type="hidden" />
                            <input id="SortPropertyName" value="EventInfo" type="hidden" />
                        </form>
                        <span class="icon-filter" title="Hide filters"></span>
                        <span id="icon-excel" class="icon-excel" title="@Resources.ExportToExcel"></span>
                        <div class="page-title"></div>
                        <table id="tbl-report" data-cansort="False"
                            data-sortedcolumn="Event Info"
                            data-descendingsort="False">
                        </table>
                    </body>`);
    });

    describe("_init", () => {
        beforeEach(() => {
            // act
            betChoiceMatching.init();
        });

        it("_Calls cacheElements", () => {
            // assert
            expect(betChoiceMatching.cacheElements).toHaveBeenCalled();
        });
        it("_Calls registerEvents", () => {
            // assert
            expect(betChoiceMatching.registerEvents).toHaveBeenCalled();
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
});