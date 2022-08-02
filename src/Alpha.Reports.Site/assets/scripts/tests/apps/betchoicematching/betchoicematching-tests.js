var events = [
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
var teams = [
    {
        Key: 1,
        Value: 'Team 1'
    },
    {
        Key: 2,
        Value: 'Team 2'
    }
];
var leagues = [
    {
        Key: 8,
        Value: 'League 8'
    }
];
var sportTypes = [
    {
        Key: 1,
        Value: 'SportType 1'
    }
];
var betTypes = [
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
var ips = [
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
describe("BetChoiceMatching", function () {
    var jQueryFake = $;
    var betChoiceMatching;
    function ajax_response(response) {
        var deferred = $.Deferred().resolve(response);
        return deferred.promise();
    }
    beforeEach(function () {
        betChoiceMatching = new BetChoiceMatching(jQueryFake);
        spyOn(betChoiceMatching, "cacheElements").and.callThrough();
        spyOn(betChoiceMatching, "registerEvents").and.callThrough();
        spyOn(betChoiceMatching, "submitOnLoad").and.callThrough();
        spyOn(jQueryFake, "blockUI");
        spyOn(jQueryFake, "unblockUI");
        setFixtures("<body>\n                        <input id=\"Today\" name=\"Today\" type=\"hidden\" value=\"3/11/2020 12:00:00 AM\" />\n<input data-val=\"true\" data-val-date=\"The field MinDate must be a date.\" data-val-required=\"The MinDate field is required.\" id=\"MinDate\" name=\"MinDate\" type=\"hidden\" value=\"1/29/2020 12:00:00 AM\" />\n<form class=\"form-inline filter\" role=\"form\" id=\"report-form\" action=\"/site-reports/(S(4nsoovmoqwsehypd115bjn12))/BetChoiceMatching/Query\">\n    <input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"4RMVxjga7MUADvwSlW0x1Atpl4kXSK3CbcklO3-3KxrTSzoQwjnlv5YPpuggXMoreSUCd-C-8uEnhBZdwwbVJLxo7741\" />\n    <div class=\"form-group\">\n        <label class=\"txt\">Event Date</label>\n        <div class=\"control date\">\n            <div class=\"input-group date\" id=\"dpkContainer\">\n                <input class=\"form-control text-box single-line\" data-val=\"true\" data-val-date=\"The field WinlostDate must be a date.\" data-val-required=\"The WinlostDate field is required.\" id=\"WinlostDate\" name=\"WinlostDate\" readonly=\"readonly\" type=\"datetime\" value=\"3/11/2020 12:00:00 AM\" />\n                <span class=\"input-group-addon\">\n                    <span class=\"icon-calendar\"></span>\n                </span>\n            </div>\n        </div>\n    </div>\n    <div class=\"form-group\">\n        <label for=\"BetCount\">Matching Bets</label>\n        <select id=\"BetCount\" name=\"BetCount\">\n                <option value=\"2\" selected=&quot;selected&quot;> >= 2</option>\n                <option value=\"3\" > >= 3</option>\n        </select>\n    </div>\n    <div class=\"form-group\">\n        <input type=\"submit\" value=\"Submit\" class=\"btn-submit\" />\n    </div>\n    <div class=\"form-group\">\n        <span id=\"icon-excel\" class=\"icon-excel\" title=\"Export to Excel\"></span>\n    </div>\n</form>\n<form class=\"form-inline hide\" role=\"form\" id=\"export-excel-form\" action=\"/site-reports/(S(4nsoovmoqwsehypd115bjn12))/BetChoiceMatching/Excel\">\n    <input type=\"hidden\" name=\"WinlostDate\" id=\"ExcelWinlostDate\" />\n    <input type=\"hidden\" name=\"BetCount\" id=\"ExcelBetCount\" />\n    <input type=\"hidden\" name=\"Username\" id=\"ExcelUsername\" />\n    <input type=\"hidden\" name=\"Stake\" id=\"ExcelStake\" />\n    <input type=\"hidden\" name=\"MemberWinLossOperator\" id=\"ExcelMemberWinLossOperator\" />\n    <input type=\"hidden\" name=\"MemberWinLossValue\" id=\"ExcelMemberWinLossValue\" />\n</form>\n<form role=\"form\" id=\"uplines-form\" action=\"/site-reports/(S(4nsoovmoqwsehypd115bjn12))/BetChoiceMatching/Uplines\">\n    <input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"5r1LmsI2dcJOu8xKaWpEcok11uXvTXmaC0eT_bk6FWqwub4uLUXcQ-_BsZ9-v2epDU27sjtMGdjdECsnFJTwzZ4Jjl81\" />\n    <input id=\"SearchAccountId\" name=\"SearchAccountId\" type=\"hidden\" value=\"\" />\n</form>\n<table class=\"tblRpt tblRpt-bordered tblRpt-hover width-100per hide\" id=\"tbl-report\">\n    <thead class=\"tableFloatingHeaderOriginal\">\n        <tr>\n            <th class=\"col-username\">Username <span class=\"icon-table-filter\"></span></th>\n            <th class=\"col-transtime\">Trans.Time</th>\n            <th class=\"col-refno\">Ref No</th>\n            <th class=\"col-odds\">Odds</th>\n            <th class=\"col-ip\">IP</th>\n            <th class=\"col-stake\">Stake <span class=\"icon-table-filter\"></span></th>\n            <th class=\"col-memberwinloss\">Member Win Loss <span class=\"icon-table-filter\"></span></th>\n            <th class=\"col-superwinloss\">Super Win Loss</th>\n            <th class=\"col-company\">Company</th>\n        </tr>\n    </thead>\n    <tbody>\n        <tr id=\"noinfo-row\">\n            <td colspan=\"9\" align=\"center\">No information is available</td>\n        </tr>\n    </tbody>\n    <tfoot>\n    </tfoot>\n</table>\n                    </body>");
    });
    var assertEventRow = function ($row, colBetChoice, colEvent, colGlobalShowTime) {
        var $cols = $row.find('td');
        var colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(colBetChoice);
        expect(trimText($($cols[colId++]).text())).toEqual(colEvent);
        expect(trimText($($cols[colId++]).text())).toEqual(colGlobalShowTime);
    };
    var assertTicketTotalRow = function ($row, username, ip, stake, memberWinLoss, superWinLoss, company) {
        var $cols = $row.find('td');
        var colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(username);
        expect(trimText($($cols[colId++]).text())).toEqual(ip);
        expect(trimText($($cols[colId++]).text())).toEqual(stake);
        expect(trimText($($cols[colId++]).text())).toEqual(memberWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(superWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(company);
    };
    var assertTicketRow = function ($row, transTime, refNo, odds, ip, stake, memberWinLoss, superWinLoss, company) {
        var $cols = $row.find('td');
        var colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(transTime);
        expect(trimText($($cols[colId++]).text())).toEqual(refNo);
        expect(trimText($($cols[colId++]).text())).toEqual(odds);
        expect(trimText($($cols[colId++]).text())).toEqual(ip);
        expect(trimText($($cols[colId++]).text())).toEqual(stake);
        expect(trimText($($cols[colId++]).text())).toEqual(memberWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(superWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(company);
    };
    var assertFooterRow = function ($row, ip, stake, memberWinLoss, superWinLoss, company) {
        var $cols = $row.find('td');
        var colId = 0;
        expect(trimText($($cols[colId++]).text())).toEqual(ip);
        expect(trimText($($cols[colId++]).text())).toEqual(stake);
        expect(trimText($($cols[colId++]).text())).toEqual(memberWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(superWinLoss);
        expect(trimText($($cols[colId++]).text())).toEqual(company);
    };
    var trimText = function (value) {
        return value.trim().split("\n").join("|").replace(/  +/g, ' ');
        ;
    };
    describe("_init", function () {
        beforeEach(function () {
            // act
            var fakeData = {
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
        it("_Calls cacheElements", function () {
            // assert
            expect(betChoiceMatching.cacheElements).toHaveBeenCalled();
        });
        it("_Calls registerEvents", function () {
            // assert
            expect(betChoiceMatching.registerEvents).toHaveBeenCalled();
        });
        it("_Calls blockUI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Calls submitOnLoad", function () {
            // assert
            expect(betChoiceMatching.$table.hasClass('hide')).toBeFalsy();
            expect(jQueryFake.post).toHaveBeenCalled();
        });
        it("_Show table with no data", function () {
            // act
            var bodyText = betChoiceMatching.$tableTbody.text();
            // assert
            var expectedText = "No information is available";
            expect(bodyText.trim()).toEqual(expectedText.trim());
        });
        it("_Invoke export Excel when click icon Excel", function () {
            // arrange
            spyOn(betChoiceMatching.$exportExcelForm, "submit").and.callFake(function () { });
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
    describe("_table", function () {
        beforeEach(function () {
            // act
            var fakeData = {
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
        it("_Render table", function () {
            // act
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            var $footerRow = betChoiceMatching.$tableTfoot.find('tr');
            // assert
            expect($rows.length).toBe(9);
            var rowIndex = 0;
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
    describe("_filter", function () {
        beforeEach(function () {
            // act
            BaseFilterForm.tooltips = [];
            var fakeData = {
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
        it('_filter toggle to show/hide', function () {
            var $filterIcons = betChoiceMatching.$table.find('.icon-table-filter');
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
    describe("_filter by username", function () {
        beforeEach(function () {
            // act
            BaseFilterForm.tooltips = [];
            var fakeData = {
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
        it('_filter by user with data matched', function () {
            // act
            betChoiceMatching.usernameFilterForm.$textareaUsername.val('nk1');
            var event = {
                preventDefault: function () { }
            };
            betChoiceMatching.usernameFilterForm.onSubmit(event);
            // assert
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            expect($rows.length).toBe(4);
            var rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");
            var $footerRow = betChoiceMatching.$tableTfoot.find('tr');
            assertFooterRow($footerRow, "Total", "23", "3.00", "3.00", "0.00");
        });
        it('_filter by user without data matched', function () {
            // arrange
            betChoiceMatching.usernameFilterForm.$textareaUsername.val('nk11');
            var event = {
                preventDefault: function () { }
            };
            // act
            betChoiceMatching.usernameFilterForm.onSubmit(event);
            expect(1).toBe(1);
            var bodyText = betChoiceMatching.$tableTbody.text();
            // assert
            var expectedText = "No information is available";
            expect(bodyText.trim()).toEqual(expectedText.trim());
        });
        it('_remove special characters with colon', function () {
            // arrange
            betChoiceMatching.usernameFilterForm.$textareaUsername.val('nk11 \n nk12 ,  nk13 ');
            // act
            betChoiceMatching.usernameFilterForm.$textareaUsername.focusout();
            // assert
            expect(betChoiceMatching.usernameFilterForm.$textareaUsername.val()).toEqual('nk11,nk12,nk13');
        });
    });
    describe("_filter by total stake", function () {
        beforeEach(function () {
            // act
            BaseFilterForm.tooltips = [];
            var fakeData = {
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
        it('_filter by stake with data matched', function () {
            // act
            betChoiceMatching.stakeFilterForm.$inputTotalStake.data('cleave').setRawValue(14);
            var event = {
                preventDefault: function () { }
            };
            betChoiceMatching.stakeFilterForm.onSubmit(event);
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            var $footerRow = betChoiceMatching.$tableTfoot.find('tr');
            // assert
            expect($rows.length).toBe(7);
            var rowIndex = 0;
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
    describe("_filter by total member win loss", function () {
        beforeEach(function () {
            // act
            BaseFilterForm.tooltips = [];
            var fakeData = {
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
        it('_filter with operator Equal', function () {
            // act
            betChoiceMatching.memberWinLossFilterForm.$selectOperator.val("1");
            betChoiceMatching.memberWinLossFilterForm.$inputMemberWinLoss.data('cleave').setRawValue(4);
            var event = {
                preventDefault: function () { }
            };
            betChoiceMatching.memberWinLossFilterForm.onSubmit(event);
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            var $footerRow = betChoiceMatching.$tableTfoot.find('tr');
            // assert
            expect($rows.length).toBe(3);
            var rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK2", "Total", "13", "4.00", "4.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:28:45", "3", "(OddsType2) 1,234.568", "127.0.0.2", "13", "4.00", "4.00", "0.00");
            assertFooterRow($footerRow, "Total", "13", "4.00", "4.00", "0.00");
        });
        it('_filter with operator GreatorOrEqual', function () {
            // act
            betChoiceMatching.memberWinLossFilterForm.$selectOperator.val("0");
            betChoiceMatching.memberWinLossFilterForm.$inputMemberWinLoss.data('cleave').setRawValue(3);
            var event = {
                preventDefault: function () { }
            };
            betChoiceMatching.memberWinLossFilterForm.onSubmit(event);
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            var $footerRow = betChoiceMatching.$tableTfoot.find('tr');
            // assert
            expect($rows.length).toBe(6);
            var rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");
            assertTicketTotalRow($($rows[rowIndex++]), "NK2", "Total", "13", "4.00", "4.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:28:45", "3", "(OddsType2) 1,234.568", "127.0.0.2", "13", "4.00", "4.00", "0.00");
            assertFooterRow($footerRow, "Total", "36", "7.00", "7.00", "0.00");
        });
        it('_filter with operator LessOrEqual', function () {
            // act
            betChoiceMatching.memberWinLossFilterForm.$selectOperator.val("2");
            betChoiceMatching.memberWinLossFilterForm.$inputMemberWinLoss.data('cleave').setRawValue(3);
            var event = {
                preventDefault: function () { }
            };
            betChoiceMatching.memberWinLossFilterForm.onSubmit(event);
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            var $footerRow = betChoiceMatching.$tableTfoot.find('tr');
            // assert
            expect($rows.length).toBe(4);
            var rowIndex = 0;
            assertEventRow($($rows[rowIndex++]), "Team 2| -0.2| | | BetType 7", "Team 1VersusTeam 2| | SportType 1| | League 8", '02/02/2020 04:05 PM');
            assertTicketTotalRow($($rows[rowIndex++]), "NK1", "Total", "23", "3.00", "3.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:26:45", "1", "0.1", "127.0.0.1", "11", "1.00", "1.00", "0.00");
            assertTicketRow($($rows[rowIndex++]), "02/02 16:27:45", "2", "(OddsType1) -0.1", "127.0.0.1", "12", "2.00", "2.00", "0.00");
            assertFooterRow($footerRow, "Total", "23", "3.00", "3.00", "0.00");
        });
    });
    describe("_IPs", function () {
        beforeEach(function () {
            // act
            BaseFilterForm.tooltips = [];
            var fakeData = {
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
        it('_should show shared IP', function () {
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            var $ip = $($rows[2]).find('.icon-ip-share');
            $ip.click();
            var $popup = $ip.parent().find('.simple-tooltip');
            expect($popup.length).toBe(1);
            expect(trimText($popup.text())).toEqual("IP_used_by| | 1: NK2");
        });
    });
    describe("_upline", function () {
        beforeEach(function () {
            // act
            BaseFilterForm.tooltips = [];
            var fakeData = {
                Events: deepClone(events),
                Teams: teams,
                Leagues: leagues,
                SportTypes: sportTypes,
                BetTypes: betTypes,
                IPs: ips
            };
            var fakeUplines = {
                AgentId: 1,
                AgentName: 'Agent 1',
                MasterId: 2,
                MasterName: 'Master 2'
            };
            spyOn(jQueryFake, "post").and.callFake(function (queryUrl) {
                if (queryUrl.indexOf('Uplines') !== -1) {
                    return ajax_response(fakeUplines);
                }
                else {
                    return ajax_response(fakeData);
                }
            });
            betChoiceMatching.init();
        });
        it('_should open uplines popup', function () {
            var $rows = betChoiceMatching.$tableTbody.find('tr');
            var $usernameText = $($rows[1]).find('.text-username');
            $usernameText.click();
            var $popup = $usernameText.parent().find('.simple-tooltip');
            expect($popup.length).toBe(1);
            expect(trimText($popup.text())).toEqual("Upline| | | Master 2| Agent 1");
        });
    });
});
//# sourceMappingURL=betchoicematching-tests.js.map