const events: QueryEventModel[] = [
    {
        "LeagueId": 8,
        "SportType": 1,
        "BetType": 7,
        "Hdp1": 0.1,
        "Hdp2": 0.2,
        "HomeId": 1,
        "AwayId": 2,
        "BetTeam": "a",
        "GlobalShowTime": "2020-02-02T16:05:00",
        "IsLive": false,
        "LiveHomeScore": 1,
        "LiveAwayScore": 1,
        "IsRunning": false,
        "Users": [
            {
                "CustId": 1,
                "Username": "NK1",
                "Stake": 23.0,
                "MemberWinLoss": 3.0,
                "SuperWinLoss": 3.0,
                "Company": 0.0,
                "IsRunning": false,
                "Tickets": [
                    {
                        "LeagueId": 8,
                        "GlobalShowTime": "2020-02-02T16:05:00",
                        "SportType": 1,
                        "MatchId": 1,
                        "CustId": 1,
                        "Username": "NK1",
                        "BetType": 7,
                        "BetTeam": "a",
                        "Hdp1": 0.1,
                        "Hdp2": 0.2,
                        "IP": "127.0.0.1",
                        "Odds": 0.1,
                        "DisplayOdds": "0.1",
                        "DisplayOddsType": "",
                        "TransDate": "2020-02-02T16:26:45.597",
                        "TransId": 1,
                        "Stake": 11.0,
                        "MemberWinLoss": 1.0,
                        "SuperWinLoss": 1.0,
                        "Company": 0.0,
                        "HomeId": 1,
                        "AwayId": 2,
                        "IsLive": false,
                        "LiveHomeScore": 1,
                        "LiveAwayScore": 1,
                        "IsRunning": false
                    },
                    {
                        "LeagueId": 8,
                        "GlobalShowTime": "2020-02-02T16:05:00",
                        "SportType": 1,
                        "MatchId": 1,
                        "CustId": 1,
                        "Username": "NK1",
                        "BetType": 7,
                        "BetTeam": "a",
                        "Hdp1": 0.1,
                        "Hdp2": 0.2,
                        "IP": "127.0.0.1",
                        "Odds": -0.1,
                        "DisplayOdds": "-0.1",
                        "DisplayOddsType": "OddsType1",
                        "TransDate": "2020-02-02T16:27:45.597",
                        "TransId": 2,
                        "Stake": 12.0,
                        "MemberWinLoss": 2.0,
                        "SuperWinLoss": 2.0,
                        "Company": 0.0,
                        "HomeId": 1,
                        "AwayId": 2,
                        "IsLive": false,
                        "LiveHomeScore": 1,
                        "LiveAwayScore": 1,
                        "IsRunning": false
                    }
                ]
            },
            {
                "CustId": 2,
                "Username": "NK2",
                "Stake": 13.0,
                "MemberWinLoss": 4.0,
                "SuperWinLoss": 4.0,
                "Company": 0.0,
                "IsRunning": false,
                "Tickets": [
                    {
                        "LeagueId": 8,
                        "GlobalShowTime": "2020-02-02T16:05:00",
                        "SportType": 1,
                        "MatchId": 1,
                        "CustId": 2,
                        "Username": "NK2",
                        "BetType": 7,
                        "BetTeam": "a",
                        "Hdp1": 0.1,
                        "Hdp2": 0.2,
                        "IP": "127.0.0.2",
                        "Odds": 1234.5678,
                        "DisplayOdds": "1,234.568",
                        "DisplayOddsType": "OddsType2",
                        "TransDate": "2020-02-02T16:28:45.597",
                        "TransId": 3,
                        "Stake": 13.0,
                        "MemberWinLoss": 4.0,
                        "SuperWinLoss": 4.0,
                        "Company": 0.0,
                        "HomeId": 1,
                        "AwayId": 2,
                        "IsLive": false,
                        "LiveHomeScore": 1,
                        "LiveAwayScore": 1,
                        "IsRunning": false
                    }
                ]
            }
        ]
    },
    {
        "LeagueId": 8,
        "SportType": 1,
        "BetType": 8,
        "Hdp1": 0.1,
        "Hdp2": 0.2,
        "HomeId": 1,
        "AwayId": 2,
        "BetTeam": "a",
        "GlobalShowTime": "2020-02-02T17:05:00",
        "IsLive": true,
        "LiveHomeScore": 1,
        "LiveAwayScore": 1,
        "IsRunning": true,
        "Users": [
            {
                "CustId": 2,
                "Username": "NK2",
                "Stake": 14.0,
                "MemberWinLoss": 0.0,
                "SuperWinLoss": 0.0,
                "Company": 0.0,
                "IsRunning": true,
                "Tickets": [
                    {
                        "LeagueId": 8,
                        "GlobalShowTime": "2020-02-02T17:05:00",
                        "SportType": 1,
                        "MatchId": 1,
                        "CustId": 2,
                        "Username": "NK2",
                        "BetType": 8,
                        "BetTeam": "a",
                        "Hdp1": 0.1,
                        "Hdp2": 0.2,
                        "IP": "127.0.0.2",
                        "Odds": 123456.7800,
                        "DisplayOdds": "123,456.78",
                        "DisplayOddsType": "OddsType5",
                        "TransDate": "2020-02-02T16:28:45.597",
                        "TransId": 4,
                        "Stake": 14.0,
                        "MemberWinLoss": 0.0,
                        "SuperWinLoss": 0.0,
                        "Company": 0.0,
                        "HomeId": 1,
                        "AwayId": 2,
                        "IsLive": true,
                        "LiveHomeScore": 1,
                        "LiveAwayScore": 1,
                        "IsRunning": true
                    }
                ]
            }
        ]
    }
];

const teams: KeyValueModel[] = [
    {
        Key: 1,
        Value: 'Team 1'
    },
    {
        Key: 2,
        Value: 'Team 2'
    }
];

const leagues: KeyValueModel[] = [
    {
        Key: 8,
        Value: 'League 8'
    }
];

const sportTypes: KeyValueModel[] = [
    {
        Key: 1,
        Value: 'SportType 1'
    }
];

const betTypes: KeyValueModel[] = [
    {
        Key: 1,
        Value: 'BetType 1'
    },
    {
        Key: 3,
        Value: 'BetType 3'
    },
    {
        Key: 5,
        Value: 'BetType 5'
    },
    {
        Key: 7,
        Value: 'BetType 7'
    },
    {
        Key: 8,
        Value: 'BetType 8'
    },
    {
        Key: 15,
        Value: 'BetType 15'
    }
];

const ips: QueryIP[] = [
    {
        "CustId": 1,
        "Username": "NK1",
        "IP": "127.0.0.1"
    },
    {
        "CustId": 2,
        "Username": "NK2",
        "IP": "127.0.0.1"
    },
    {
        "CustId": 3,
        "Username": "NK3",
        "IP": "127.0.0.2"
    }
];

function deepClone(obj) {
    return JSON.parse(JSON.stringify(obj));
}

describe("BetChoiceMatching", () => {
    const jQueryFake = $;
    let betChoiceMatching: BetChoiceMatching;

    function ajax_response(response) {
        var deferred = $.Deferred().resolve(response);
        return deferred.promise();
    }

    beforeEach(() => {
        betChoiceMatching = new BetChoiceMatching(jQueryFake);
        spyOn(betChoiceMatching, "cacheElements").and.callThrough();
        spyOn(betChoiceMatching, "registerEvents").and.callThrough();
        spyOn(betChoiceMatching, "submitOnLoad").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        setFixtures(`<body>
                        <input id="Today" name="Today" type="hidden" value="3/11/2020 12:00:00 AM" />
<input data-val="true" data-val-date="The field MinDate must be a date." data-val-required="The MinDate field is required." id="MinDate" name="MinDate" type="hidden" value="1/29/2020 12:00:00 AM" />
<form class="form-inline filter" role="form" id="report-form" action="/site-reports/(S(4nsoovmoqwsehypd115bjn12))/BetChoiceMatching/Query">
    <input name="__RequestVerificationToken" type="hidden" value="4RMVxjga7MUADvwSlW0x1Atpl4kXSK3CbcklO3-3KxrTSzoQwjnlv5YPpuggXMoreSUCd-C-8uEnhBZdwwbVJLxo7741" />
    <div class="form-group">
        <label class="txt">Event Date</label>
        <div class="control date">
            <div class="input-group date" id="dpkContainer">
                <input class="form-control text-box single-line" data-val="true" data-val-date="The field WinlostDate must be a date." data-val-required="The WinlostDate field is required." id="WinlostDate" name="WinlostDate" readonly="readonly" type="datetime" value="3/11/2020 12:00:00 AM" />
                <span class="input-group-addon">
                    <span class="icon-calendar"></span>
                </span>
            </div>
        </div>
    </div>
    <div class="form-group">
        <label for="BetCount">Matching Bets</label>
        <select id="BetCount" name="BetCount">
                <option value="2" selected=&quot;selected&quot;> >= 2</option>
                <option value="3" > >= 3</option>
        </select>
    </div>
    <div class="form-group">
        <input type="submit" value="Submit" class="btn-submit" />
    </div>
    <div class="form-group">
        <span id="icon-excel" class="icon-excel" title="Export to Excel"></span>
    </div>
</form>
<form class="form-inline hide" role="form" id="export-excel-form" action="/site-reports/(S(4nsoovmoqwsehypd115bjn12))/BetChoiceMatching/Excel">
    <input type="hidden" name="WinlostDate" id="ExcelWinlostDate" />
    <input type="hidden" name="BetCount" id="ExcelBetCount" />
    <input type="hidden" name="Username" id="ExcelUsername" />
    <input type="hidden" name="Stake" id="ExcelStake" />
    <input type="hidden" name="MemberWinLossOperator" id="ExcelMemberWinLossOperator" />
    <input type="hidden" name="MemberWinLossValue" id="ExcelMemberWinLossValue" />
</form>
<form role="form" id="uplines-form" action="/site-reports/(S(4nsoovmoqwsehypd115bjn12))/BetChoiceMatching/Uplines">
    <input name="__RequestVerificationToken" type="hidden" value="5r1LmsI2dcJOu8xKaWpEcok11uXvTXmaC0eT_bk6FWqwub4uLUXcQ-_BsZ9-v2epDU27sjtMGdjdECsnFJTwzZ4Jjl81" />
    <input id="SearchAccountId" name="SearchAccountId" type="hidden" value="" />
</form>
<table class="tblRpt tblRpt-bordered tblRpt-hover width-100per hide" id="tbl-report">
    <thead class="tableFloatingHeaderOriginal">
        <tr>
            <th class="col-username">Username <span class="icon-table-filter"></span></th>
            <th class="col-transtime">Trans.Time</th>
            <th class="col-refno">Ref No</th>
            <th class="col-odds">Odds</th>
            <th class="col-ip">IP</th>
            <th class="col-stake">Stake <span class="icon-table-filter"></span></th>
            <th class="col-memberwinloss">Member Win Loss <span class="icon-table-filter"></span></th>
            <th class="col-superwinloss">Super Win Loss</th>
            <th class="col-company">Company</th>
        </tr>
    </thead>
    <tbody>
        <tr id="noinfo-row">
            <td colspan="9" align="center">No information is available</td>
        </tr>
    </tbody>
    <tfoot>
    </tfoot>
</table>
                    </body>`);
    });

    const assertEventRow = ($row: JQuery, colBetChoice: string, colEvent: string, colGlobalShowTime: string) => {
        const $cols = $row.find('td');
        let colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(colBetChoice);
        expect(trimText($($cols[colId++]).text())).toEqual(colEvent);
        expect(trimText($($cols[colId++]).text())).toEqual(colGlobalShowTime);
    }
    const assertTicketTotalRow = ($row: JQuery, username: string, ip: string, stake: string, memberWinLoss: string, superWinLoss: string, company: string) => {
        const $cols = $row.find('td');
        let colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(username);
        expect(trimText($($cols[colId++]).text())).toEqual(ip);
        expect(trimText($($cols[colId++]).text())).toEqual(stake);
        expect(trimText($($cols[colId++]).text())).toEqual(memberWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(superWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(company);
    }
    const assertTicketRow = ($row: JQuery, transTime: string, refNo: string, odds: string, ip: string, stake: string, memberWinLoss: string, superWinLoss: string, company: string) => {
        const $cols = $row.find('td');
        let colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(transTime);
        expect(trimText($($cols[colId++]).text())).toEqual(refNo);
        expect(trimText($($cols[colId++]).text())).toEqual(odds);
        expect(trimText($($cols[colId++]).text())).toEqual(ip);
        expect(trimText($($cols[colId++]).text())).toEqual(stake);
        expect(trimText($($cols[colId++]).text())).toEqual(memberWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(superWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(company);
    }
    const assertFooterRow = ($row: JQuery, ip: string, stake: string, memberWinLoss: string, superWinLoss: string, company: string) => {
        const $cols = $row.find('td');
        let colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(ip);
        expect(trimText($($cols[colId++]).text())).toEqual(stake);
        expect(trimText($($cols[colId++]).text())).toEqual(memberWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(superWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(company);
    }

    const trimText = (value: string): string => {
        return value.trim().split("\n").join("|").replace(/  +/g, ' ');;
    }

    describe("_init", () => {
        beforeEach(() => {
            // act
            const fakeData: QueryModel = {
                Events: [],
                Teams: [],
                Leagues: [],
                SportTypes: [],
                BetTypes: [],
                IPs: []
            };
            spyOn(jQueryFake, "post").and.returnValue(ajax_response(fakeData));
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

        it("_Calls submitOnLoad", () => {
            // assert
            expect(betChoiceMatching.$table.hasClass('hide')).toBeFalsy();
            expect(jQueryFake.post).toHaveBeenCalled();
        });

        it("_Show table with no data", () => {
            // act
            const bodyText = betChoiceMatching.$tableTbody.text();

            // assert
            const expectedText = `No information is available`;
            expect(bodyText.trim()).toEqual(expectedText.trim());
        });

        it("_Invoke export Excel when click icon Excel", () => {
            // arrange
            spyOn(betChoiceMatching.$exportExcelForm, "submit").and.callFake(() => { });

            // act
            betChoiceMatching.$excelIcon.click();

            // assert
            expect(betChoiceMatching.$excelWinlostDate.val()).toEqual("03/11/2020");
            expect(betChoiceMatching.$excelBetCount.val()).toEqual("2");
            expect(betChoiceMatching.$excelUsername.val()).toEqual("");
            expect(betChoiceMatching.$excelStake.val()).toEqual("");
            expect(betChoiceMatching.$excelMemberWinLossOperator.val()).toEqual("0");
            expect(betChoiceMatching.$excelMemberWinLossValue.val()).toEqual("");
            expect(betChoiceMatching.$exportExcelForm.submit).toHaveBeenCalled();
        });
    });

    describe("_table", () => {
        beforeEach(() => {
            // act
            const fakeData: QueryModel = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            spyOn(jQueryFake, "post").and.returnValue(ajax_response(fakeData));
            betChoiceMatching.init();
        });

        it("_Render table", () => {
            // act
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            const $footerRow = betChoiceMatching.$tableTfoot.find('tr');

            // assert
            expect($rows.length).toBe(9);
            let rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");
            assertTicketTotalRow($($rows[rowIndex++]), "NK2", "Total", "13", "4.00", "4.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:28:45", "3", "(OddsType2) 1,234.568", "127.0.0.2", "13", "4.00", "4.00", "0.00");
            assertEventRow($($rows[rowIndex++]), "Under| -0.2| [1-1]| | BetType 8", "Team 1VersusTeam 2| | SportType 1| | League 8", "02/02/2020 05:05 PM");
            assertTicketTotalRow($($rows[rowIndex++]), "NK2", "Total", "14", "-", "-", "-");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:28:45", "4", "(OddsType5) 123,456.78", "127.0.0.2", "14", "-", "-", "-");

            assertFooterRow($footerRow, "Total", "50", "7.00", "7.00", "0.00");
        });
    });

    describe("_filter", () => {
        beforeEach(() => {
            // act
            BaseFilterForm.tooltips = [];
            const fakeData: QueryModel = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            spyOn(jQueryFake, "post").and.returnValue(ajax_response(fakeData));
            betChoiceMatching.init();
        });

        it('_filter toggle to show/hide', () => {
            const $filterIcons = betChoiceMatching.$table.find('.icon-table-filter');
            expect($('.username-filter').is(":hidden")).toBeTruthy();
            $($filterIcons[0]).click();
            expect($('.username-filter').is(":hidden")).toBeFalsy();

            expect($('.stake-filter').is(":hidden")).toBeTruthy();
            $($filterIcons[1]).click();
            expect($('.stake-filter').is(":hidden")).toBeFalsy();

            expect($('.memberwinloss-filter').is(":hidden")).toBeTruthy();
            $($filterIcons[2]).click();
            expect($('.memberwinloss-filter').is(":hidden")).toBeFalsy();
        });
    });

    describe("_filter by username", () => {
        beforeEach(() => {
            // act
            BaseFilterForm.tooltips = [];
            const fakeData: QueryModel = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            spyOn(jQueryFake, "post").and.returnValue(ajax_response(fakeData));
            betChoiceMatching.init();
        });

        it('_filter by user with data matched', () => {
            // act
            betChoiceMatching.usernameFilterForm.$textareaUsername.val('nk1');
            const event: any = {
                preventDefault: () => { }
            };
            betChoiceMatching.usernameFilterForm.onSubmit(event);

            // assert
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            expect($rows.length).toBe(4);
            let rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");

            const $footerRow = betChoiceMatching.$tableTfoot.find('tr');
            assertFooterRow($footerRow, "Total", "23", "3.00", "3.00", "0.00");
        });

        it('_filter by user without data matched', () => {
            // arrange
            betChoiceMatching.usernameFilterForm.$textareaUsername.val('nk11');
            const event: any = {
                preventDefault: () => { }
            };

            // act
            betChoiceMatching.usernameFilterForm.onSubmit(event);
            expect(1).toBe(1);
            const bodyText = betChoiceMatching.$tableTbody.text();

            // assert
            const expectedText = `No information is available`;
            expect(bodyText.trim()).toEqual(expectedText.trim());
        });

        it('_remove special characters with colon', () => {
            // arrange
            betChoiceMatching.usernameFilterForm.$textareaUsername.val('nk11 \n nk12 ,  nk13 ');

            // act
            betChoiceMatching.usernameFilterForm.$textareaUsername.focusout();

            // assert
            expect(betChoiceMatching.usernameFilterForm.$textareaUsername.val()).toEqual('nk11,nk12,nk13');
        });
    });

    describe("_filter by total stake", () => {
        beforeEach(() => {
            // act
            BaseFilterForm.tooltips = [];
            const fakeData: QueryModel = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            spyOn(jQueryFake, "post").and.returnValue(ajax_response(fakeData));
            betChoiceMatching.init();
        });

        it('_filter by stake with data matched', () => {
            // act
            betChoiceMatching.stakeFilterForm.$inputTotalStake.data('cleave').setRawValue(14);
            const event: any = {
                preventDefault: () => { }
            };
            betChoiceMatching.stakeFilterForm.onSubmit(event);
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            const $footerRow = betChoiceMatching.$tableTfoot.find('tr');

            // assert
            expect($rows.length).toBe(7);
            let rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");
            assertEventRow($($rows[rowIndex++]), "Under| -0.2| [1-1]| | BetType 8", "Team 1VersusTeam 2| | SportType 1| | League 8", "02/02/2020 05:05 PM");
            assertTicketTotalRow($($rows[rowIndex++]), "NK2", "Total", "14", "-", "-", "-");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:28:45", "4", "(OddsType5) 123,456.78", "127.0.0.2", "14", "-", "-", "-");

            assertFooterRow($footerRow, "Total", "37", "3.00", "3.00", "0.00");
        });
    });

    describe("_filter by total member win loss", () => {
        beforeEach(() => {
            // act
            BaseFilterForm.tooltips = [];
            const fakeData: QueryModel = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            spyOn(jQueryFake, "post").and.returnValue(ajax_response(fakeData));
            betChoiceMatching.init();
        });

        it('_filter with operator Equal', () => {
            // act
            betChoiceMatching.memberWinLossFilterForm.$selectOperator.val("1");
            betChoiceMatching.memberWinLossFilterForm.$inputMemberWinLoss.data('cleave').setRawValue(4);
            const event: any = {
                preventDefault: () => { }
            };
            betChoiceMatching.memberWinLossFilterForm.onSubmit(event);
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            const $footerRow = betChoiceMatching.$tableTfoot.find('tr');

            // assert
            expect($rows.length).toBe(3);
            let rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK2", "Total", "13", "4.00", "4.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:28:45", "3", "(OddsType2) 1,234.568", "127.0.0.2", "13", "4.00", "4.00", "0.00");

            assertFooterRow($footerRow, "Total", "13", "4.00", "4.00", "0.00");
        });

        it('_filter with operator GreatorOrEqual', () => {
            // act
            betChoiceMatching.memberWinLossFilterForm.$selectOperator.val("0");
            betChoiceMatching.memberWinLossFilterForm.$inputMemberWinLoss.data('cleave').setRawValue(3);
            const event: any = {
                preventDefault: () => { }
            };
            betChoiceMatching.memberWinLossFilterForm.onSubmit(event);
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            const $footerRow = betChoiceMatching.$tableTfoot.find('tr');

            // assert
            expect($rows.length).toBe(6);
            let rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");
            assertTicketTotalRow($($rows[rowIndex++]), "NK2", "Total", "13", "4.00", "4.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:28:45", "3", "(OddsType2) 1,234.568", "127.0.0.2", "13", "4.00", "4.00", "0.00");

            assertFooterRow($footerRow, "Total", "36", "7.00", "7.00", "0.00");
        });

        it('_filter with operator LessOrEqual', () => {
            // act
            betChoiceMatching.memberWinLossFilterForm.$selectOperator.val("2");
            betChoiceMatching.memberWinLossFilterForm.$inputMemberWinLoss.data('cleave').setRawValue(3);
            const event: any = {
                preventDefault: () => { }
            };
            betChoiceMatching.memberWinLossFilterForm.onSubmit(event);
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            const $footerRow = betChoiceMatching.$tableTfoot.find('tr');

            // assert
            expect($rows.length).toBe(4);
            let rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");

            assertFooterRow($footerRow, "Total", "23", "3.00", "3.00", "0.00");
        });
    });

    describe("_IPs", () => {
        beforeEach(() => {
            // act
            BaseFilterForm.tooltips = [];
            const fakeData: QueryModel = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            spyOn(jQueryFake, "post").and.returnValue(ajax_response(fakeData));
            betChoiceMatching.init();
        });

        it('_should show shared IP', () => {
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            const $ip = $($rows[2]).find('.icon-ip-share');
            $ip.click();
            const $popup = $ip.parent().find('.simple-tooltip');
            expect($popup.length).toBe(1);
            expect(trimText($popup.text())).toEqual("IP_used_by| | 1: NK2");
        });
    });

    describe("_upline", () => {
        beforeEach(() => {
            // act
            BaseFilterForm.tooltips = [];
            const fakeData: QueryModel = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            const fakeUplines: UplinesModel = {
                AgentId: 1,
                AgentName: 'Agent 1',
                MasterId: 2,
                MasterName: 'Master 2'
            };
            spyOn(jQueryFake, "post").and.callFake((queryUrl: string) => {
                if (queryUrl.indexOf('Uplines') !== -1) {
                    return ajax_response(fakeUplines);
                } else {
                    return ajax_response(fakeData);
                }
            });
            betChoiceMatching.init();
        });

        it('_should open uplines popup', () => {
            const $rows = betChoiceMatching.$tableTbody.find('tr');
            const $usernameText = $($rows[1]).find('.text-username');
            $usernameText.click();
            const $popup = $usernameText.parent().find('.simple-tooltip');
            expect($popup.length).toBe(1);
            expect(trimText($popup.text())).toEqual("Upline| | | Master 2| Agent 1");
        });
    });
});