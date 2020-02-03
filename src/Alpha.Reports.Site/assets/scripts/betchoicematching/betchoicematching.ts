/// <reference path="../core.ts" />

"use strict";

interface JQuery {
    datepicker(option: any): void;
}

function getValueByKey(data: KeyValueModel[], key: any): any {
    for (let i = 0; i < data.length; i++) {
        const item = data[i];
        if (item.Key === key)
            return item.Value;
    }

    return null;
}

enum BetTypes {
    HandicapFT = 1,
    OverUnderFT = 3,
    OneXTwoFT = 5,
    Handicap1H = 7,
    OverUnder1H = 8,
    OneXTwo1H = 15
}

enum BetTeams {
    H = 'h', // for HDP (FT + 1H) & OU (FT + 1H)
    A = 'a', // for HDP (FT + 1H) & OU (FT + 1H)
    One = '1', // for 1X2
    Two = '2', // for 1X2
    X = 'x' // for 1X2
}

const BetTeamClassNames = {
    Underdog: "underdog",
    Favorite: "favorite"
};

class QueryModelUserGroup {
    Username: string;
    MemberTurnover: number;
    MemberWinLoss: number;
    SuperWinLoss: number;
    Company: number;
    Items: QueryModelItem[];

    constructor(item: QueryModelItem) {
        this.Username = item.Username;
        this.MemberTurnover = item.MemberTurnover;
        this.MemberWinLoss = item.MemberWinLoss;
        this.SuperWinLoss = item.SuperWinLoss;
        this.Company = item.Company;
        this.Items = [item];
    }

    addItem(item: QueryModelItem) {
        this.Items.push(item);
        this.MemberTurnover = this.MemberTurnover + item.MemberTurnover;
        this.MemberWinLoss = this.MemberWinLoss + item.MemberWinLoss;
        this.SuperWinLoss = this.SuperWinLoss + item.SuperWinLoss;
        this.Company = this.Company + item.Company;
    }
}

interface ChoiceTicket {
    LeagueId: number;
    SportType: number;
    BetType: number;
    Hdp1: number;
    Hdp2: number;
    HomeId: number;
    AwayId: number;
    BetTeam: string;
    GlobalShowTime: string;
    LiveHomeScore: number;
    LiveAwayScore: number;
}

abstract class ChoiceBuilder {
    constructor(protected ticket: ChoiceTicket) { }

    get IsBetHomeTeam(): boolean {
        return this.ticket.BetTeam && this.ticket.BetTeam.toLowerCase().indexOf(BetTeams.H) >= 0;
    }

    build(data: QueryModel): string {
        return `<span class="text-bet-team">${this.generateBetTeam(data)}</span>
                <span class="text-bet-team">${this.generateHandicap()}</span>
                <span class="text-live-score">[${this.ticket.LiveHomeScore}-${this.ticket.LiveAwayScore}]</span>
                <br />
                <span class="text-bet-type">${getValueByKey(data.BetTypes, this.ticket.BetType)}</span>`;
    }

    abstract generateBetTeam(data: QueryModel): string;

    abstract generateHandicap(): string;
}

class HandicapChoiceBuilder extends ChoiceBuilder {
    generateBetTeam(data: QueryModel): string {
        const betTeamId = this.IsBetHomeTeam ? this.ticket.HomeId : this.ticket.AwayId;
        return getValueByKey(data.Teams, betTeamId);
    }

    generateHandicap(): string {
        let betTeamClassName = '';
        if (this.IsBetHomeTeam) {
            betTeamClassName = this.ticket.Hdp2 >= this.ticket.Hdp1 ? BetTeamClassNames.Underdog : BetTeamClassNames.Favorite;
        } else {
            betTeamClassName = this.ticket.Hdp2 <= this.ticket.Hdp1 ? BetTeamClassNames.Underdog : BetTeamClassNames.Favorite;
        }

        let handicap = Math.max(this.ticket.Hdp1, this.ticket.Hdp2);
        handicap = betTeamClassName === BetTeamClassNames.Favorite ? -handicap : handicap;
        return numeral(handicap).format('0.[00]');
    }
}

class OverUnderChoiceBuilder extends HandicapChoiceBuilder {
    generateBetTeam(data: QueryModel): string {
        return this.IsBetHomeTeam ? data.Resource.Over : data.Resource.Under;
    }
}

class OneXTwoChoiceBuilder extends ChoiceBuilder {
    generateBetTeam(): string {
        const prefix = this.ticket.BetType === BetTypes.OneXTwoFT ? 'FT' : 'HT';
        return `${prefix}.${this.ticket.BetTeam ? this.ticket.BetTeam.toUpperCase() : ''}`;
    }

    generateHandicap(): string {
        return '';
    }
}

class QueryModelEventGroup implements ChoiceTicket {
    Key: string;
    LeagueId: number;
    SportType: number;
    BetType: number;
    Hdp1: number;
    Hdp2: number;
    HomeId: number;
    AwayId: number;
    BetTeam: string;
    GlobalShowTime: string;
    LiveHomeScore: number;
    LiveAwayScore: number;
    UserGroups: QueryModelUserGroup[];

    constructor(item: QueryModelItem) {
        this.LeagueId = item.LeagueId;
        this.SportType = item.SportType;
        this.BetType = item.BetType;
        this.GlobalShowTime = item.GlobalShowTime;
        this.Hdp1 = item.Hdp1;
        this.Hdp2 = item.Hdp2;
        this.BetTeam = item.BetTeam;
        this.LiveHomeScore = item.LiveHomeScore;
        this.LiveAwayScore = item.LiveAwayScore;
        this.HomeId = item.HomeId;
        this.AwayId = item.AwayId;
        this.Key = `${item.MatchId}_${item.BetType}_${item.Hdp1}_${item.Hdp2}_${item.BetTeam}`;
        this.UserGroups = [new QueryModelUserGroup(item)];
    }

    addItem(item: QueryModelItem) {
        const filterUserGroups = this.UserGroups.filter(g => g.Username === item.Username);
        if (filterUserGroups.length === 0) {
            this.UserGroups.push(new QueryModelUserGroup(item));
        } else {
            filterUserGroups[0].addItem(item);
        }
    }

    generateHtml(data: QueryModel): string {
        const choiceBuilder = this.getChoiceBuilder();
        return `<tr class="row-bet-choice">
                    <td class="text-left col-bet-choice">${choiceBuilder.build(data)}</td>
                    <td colspan="6" class="text-left col-league-sport">${this.generateEvent(data)}</td>
                    <td colspan="2" class="text-center col-global-time">${moment(this.GlobalShowTime).format("MM/DD/YYYY HH:mm A")}</td>
                </tr>`;
    }

    private getChoiceBuilder(): ChoiceBuilder {
        switch (this.BetType) {
            case BetTypes.HandicapFT:
            case BetTypes.Handicap1H:
                return new HandicapChoiceBuilder(this);
            case BetTypes.OverUnderFT:
            case BetTypes.OverUnder1H:
                return new OverUnderChoiceBuilder(this);
            case BetTypes.OneXTwoFT:
            case BetTypes.OneXTwo1H:
                return new OneXTwoChoiceBuilder(this);
        }

        throw new Error(`ChoiceBuilder is not implemented for bettype ${this.BetType}`);
    }

    private generateEvent(data: QueryModel): string {
        return `<span class="text-event">${getValueByKey(data.Teams, this.HomeId)}${data.Resource.Versus}${getValueByKey(data.Teams, this.AwayId)}</span>
                <br />
                <span class="text-sport-type">${getValueByKey(data.SportTypes, this.SportType)}</span>
                ${' '}
                <span class="text-league">${getValueByKey(data.Leagues, this.LeagueId)}</span>`;
    }
}

interface QueryModelItem {
    LeagueId: number;
    SportType: number;
    BetType: number;
    Username: string;
    TransDate: string;
    GlobalShowTime: string;
    TransId: number;
    Odds: number;
    IP: string;
    MemberTurnover: number;
    MemberWinLoss: number;
    SuperWinLoss: number;
    Company: number;
    MatchId: number;
    HomeId: number;
    AwayId: number;
    LiveHomeScore: number;
    LiveAwayScore: number;
    Hdp1: number;
    Hdp2: number;
    BetTeam: string;
}

interface KeyValueModel {
    Key: any;
    Value: any;
}

interface QueryResource {
    Total: string;
    Versus: string;
    Over: string;
    Under: string;
}

interface QueryModel {
    Items: QueryModelItem[];
    Teams: KeyValueModel[];
    Leagues: KeyValueModel[];
    SportTypes: KeyValueModel[];
    BetTypes: KeyValueModel[];
    Resource: QueryResource;
}

class BetChoiceMatching {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $datePicker: JQuery;
    $datePickerInput: JQuery;
    $betCountDropdown: JQuery;
    $table: JQuery;
    $tableTbody: JQuery;
    $tableTfoot: JQuery;
    noinfoRowHtml: string;
    data: QueryModel;

    constructor(
        $: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.$.blockUI();
        this.cacheElements();
        this.registerEvents();
        this.$table.hide();
        this.$table.removeClass('hide');
        this.$.unblockUI();
    };

    cacheElements = () => {
        this.$pageTitle = this.$(".page-title");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#WinlostDate");
        this.$betCountDropdown = this.$("#betCount");
        this.$table = this.$("#tbl-report");
        this.$tableTbody = this.$table.find("tbody");
        this.$tableTfoot = this.$table.find("tfoot");
        const $noinfoRow = this.$table.find("#noinfo-row");
        this.noinfoRowHtml = $noinfoRow.html();
    };

    registerEvents = () => {
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$datePicker.datepicker({
            allowInputToggle: true,
            minDate: new Date(this.$('#MinDate').val()),
            maxDate: new Date(this.$('#Today').val()),
            format: alphaReports.format.shortDateWithSlash_DatePicker
        });
    };

    onFilterEvent = () => {
        let currentHideState = this.$reportForm.hasClass("hide");
        currentHideState ? this.showFilter() : this.hideFilter();
    };

    showFilter = () => {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.removeClass("hide");
    };

    hideFilter = () => {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    };

    onSubmitReport = (e: any) => {
        e.preventDefault();
        let this$ = this.$;
        this$.blockUI();
        this.$table.hide();
        const action = this.$reportForm.prop('action');
        const params = this.getParams();
        this$.post(action, params)
            .done((data: QueryModel) => {
                this.data = data;
                const filterItems = this.filterItems(data.Items);
                const eventGroups = this.groupItems(filterItems);
                this.renderTable(eventGroups);
            })
            .fail(() => {
                alert('error');
            })
            .always(() => {
                this.$table.show();
                this$.unblockUI();
            });
    };

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };

    getParams = () => {
        return {
            winlostDate: this.$datePickerInput.val(),
            betCount: this.$betCountDropdown.val()
        };
    };

    groupItems = (items: QueryModelItem[]) => {
        const itemGroups: QueryModelEventGroup[] = [];
        items.forEach(item => {
            let itemGroup = new QueryModelEventGroup(item);
            const filterItemGroups = itemGroups.filter(g => g.Key === itemGroup.Key);
            if (filterItemGroups.length === 0) {
                itemGroups.push(itemGroup);
            } else {
                filterItemGroups[0].addItem(item);
            }
        });

        return itemGroups;
    }

    filterItems = (items: QueryModelItem[]) => {
        return items;
    }

    formatNumber = (value: number) => {
        if (!value) {
            return '-';
        }

        return numeral(value).format('0,0.00');
    }

    renderTable = (eventGroups: QueryModelEventGroup[]) => {
        if (eventGroups.length === 0) {
            this.$tableTbody.html(this.noinfoRowHtml);
            this.$tableTfoot.hide();
            return;
        }

        let totalMemberTurnover = null,
            totalMemberWinLoss = null,
            totalSuperWinLoss = null,
            totalCompany = null;
        const html = [];
        eventGroups.forEach(eventGroup => {
            html.push(eventGroup.generateHtml(this.data));
            eventGroup.UserGroups.forEach((userGroup, index) => {
                totalMemberTurnover += userGroup.MemberTurnover;
                totalMemberWinLoss += userGroup.MemberWinLoss;
                totalSuperWinLoss += userGroup.SuperWinLoss;
                totalCompany += userGroup.Company;

                html.push(`<tr class="${index % 2 === 1 ? "row-even row-total" : "row-total"}">
                    <td class="text-left text-username" rowspan="${userGroup.Items.length + 1}">${userGroup.Username}</td>
                    <td class="text-right" colspan="4">${this.data.Resource.Total}</td>
                    <td>${this.formatNumber(userGroup.MemberTurnover)}</td>
                    <td>${this.formatNumber(userGroup.MemberWinLoss)}</td>
                    <td class="col-superwinloss">${this.formatNumber(userGroup.SuperWinLoss)}</td>
                    <td>${this.formatNumber(userGroup.Company)}</td>
                </tr>`);
                userGroup.Items.forEach(item => {
                    html.push(`<tr class="${index % 2 === 1 ? "row-even" : ""}">
                        <td class="text-center">${moment(item.TransDate).format("MM/DD HH:mm:ss")}</td>
                        <td class="text-center">${item.TransId}</td>
                        <td class="text-center">${item.Odds}</td>
                        <td class="text-center">${item.IP}</td>
                        <td>${this.formatNumber(item.MemberTurnover)}</td>
                        <td>${this.formatNumber(item.MemberWinLoss)}</td>
                        <td class="col-superwinloss">${this.formatNumber(item.SuperWinLoss)}</td>
                        <td>${this.formatNumber(item.Company)}</td>
                    </tr>`);
                });
            });
        });
        this.$tableTbody.html(html.join(''));

        this.$tableTfoot.html(`
            <tr class="tbl-footer">
                <td colspan="5">${this.data.Resource.Total}</td>
                <td>${this.formatNumber(totalMemberTurnover)}</td>
                <td>${this.formatNumber(totalMemberWinLoss)}</td>
                <td class="col-superwinloss">${this.formatNumber(totalSuperWinLoss)}</td>
                <td>${this.formatNumber(totalCompany)}</td>
            </tr>
        `);
        this.$tableTfoot.show();
    };
}

$(() => {
    const betChoiceMatching = new BetChoiceMatching(
        $);

    betChoiceMatching.init();
});
