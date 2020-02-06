/// <reference path="../core.ts" />

"use strict";

declare const betMatchingResources: Resources;

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
    CustId: number;
    Username: string;
    MemberTurnover: number;
    MemberWinLoss: number;
    SuperWinLoss: number;
    Company: number;
    IsRunning: boolean;
    Items: QueryModelItem[];

    constructor(item: QueryModelItem) {
        this.CustId = item.CustId;
        this.Username = item.Username;
        this.MemberTurnover = item.MemberTurnover;
        this.MemberWinLoss = item.MemberWinLoss;
        this.SuperWinLoss = item.SuperWinLoss;
        this.Company = item.Company;
        this.IsRunning = item.IsRunning;
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
        return `<span class="${this.BetTeamClassName}">
                    ${this.generateBetTeam(data)}
                    <span class="text-handicap">${this.generateHandicap()}</span>
                    <span class="text-live-score">[${this.ticket.LiveHomeScore}-${this.ticket.LiveAwayScore}]</span>
                    <br />
                    <span class="text-bet-type">${getValueByKey(data.BetTypes, this.ticket.BetType)}</span>
                </span>`;
    }

    abstract get BetTeamClassName(): string;

    abstract generateBetTeam(data: QueryModel): string;

    abstract generateHandicap(): string;
}

class HandicapChoiceBuilder extends ChoiceBuilder {
    get BetTeamClassName(): string {
        if (this.IsBetHomeTeam) {
            return this.ticket.Hdp2 >= this.ticket.Hdp1 ? BetTeamClassNames.Underdog : BetTeamClassNames.Favorite;
        } else {
            return this.ticket.Hdp2 <= this.ticket.Hdp1 ? BetTeamClassNames.Underdog : BetTeamClassNames.Favorite;
        }
    }

    generateBetTeam(data: QueryModel): string {
        const betTeamId = this.IsBetHomeTeam ? this.ticket.HomeId : this.ticket.AwayId;
        return getValueByKey(data.Teams, betTeamId);
    }

    generateHandicap(): string {
        let handicap = Math.max(this.ticket.Hdp1, this.ticket.Hdp2);
        handicap = this.BetTeamClassName === BetTeamClassNames.Favorite ? -handicap : handicap;
        return handicap.toLocaleString('en-US', { maximumFractionDigits: 2 });
    }
}

class OverUnderChoiceBuilder extends HandicapChoiceBuilder {
    generateBetTeam(data: QueryModel): string {
        return this.IsBetHomeTeam ? betMatchingResources.Over : betMatchingResources.Under;
    }
}

class OneXTwoChoiceBuilder extends ChoiceBuilder {
    get BetTeamClassName(): string {
        return BetTeamClassNames.Favorite;
    }

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
    IsRunning: boolean;
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
        this.IsRunning = item.IsRunning;
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
                    <td colspan="6" class="text-left col-event">${this.generateEvent(data)}</td>
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
        return `<span class="text-event">${getValueByKey(data.Teams, this.HomeId)}${betMatchingResources.Versus}${getValueByKey(data.Teams, this.AwayId)}</span>
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
    CustId: number;
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
    IsRunning: boolean;
}

interface KeyValueModel {
    Key: any;
    Value: any;
}

interface Resources {
    Total: string;
    Versus: string;
    Over: string;
    Under: string;
    Upline: string;
    IP_used_by: string;
    BetMatching_Username_Title: string;
    BetMatching_IP_Title: string;
    FilterByUsername: string;
    FilterByStake: string;
    FilterByMemberWinLoss: string;
    FilterByUsername_Placeholder: string;
    TotalStake: string;
    Filter: string;
    TotalMemberWL: string;
}

interface QueryModel {
    Items: QueryModelItem[];
    Teams: KeyValueModel[];
    Leagues: KeyValueModel[];
    SportTypes: KeyValueModel[];
    BetTypes: KeyValueModel[];
    IPs: QueryIP[];
}

interface QueryIP {
    IP: string;
    CustId: number;
    Username: string;
}

interface QueryIPGroup {
    [IP: string]: QueryIP[];
}

interface UplinesModel {
    AgentId: number;
    AgentName: string;
    MasterId: number;
    MasterName: string;
}

interface UplinesCache {
    [custId: number]: UplinesModel;
}

class BetChoiceUplines {
    $: JQueryStatic;
    $uplinesForm: JQuery;
    $searchAccountId: JQuery;
    uplinesCache: UplinesCache = {};

    constructor(
        $: JQueryStatic
    ) {
        this.$ = $;
        this.$uplinesForm = this.$("#uplines-form");
        this.$searchAccountId = this.$("#SearchAccountId");
    }

    query = (custId: number, callback: (data: UplinesModel) => void) => {
        if (this.uplinesCache[custId]) {
            callback(this.uplinesCache[custId]);
            return;
        }

        this.$searchAccountId.val(custId);
        const queryUrl = this.$uplinesForm.attr('action');
        const params = alphaReports.getFormData(this.$uplinesForm);
        this.$.post(queryUrl, params)
            .done((data: UplinesModel) => {
                this.uplinesCache[custId] = data;
                callback(data);
            })
            .fail(() => {
                callback(null);
            });
    };
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
    $table: JQuery;
    $tableTbody: JQuery;
    $tableTfoot: JQuery;
    $usernameFilterIcon: JQuery;
    $stakeFilterIcon: JQuery;
    $memberWinLossFilterIcon: JQuery;
    noinfoRowHtml: string;
    data: QueryModel;
    betChoiceUplines: BetChoiceUplines;
    usernameFilterTooltip: SimpleTooltip;
    stakeFilterTooltip: SimpleTooltip;
    memberWinLossFilterTooltip: SimpleTooltip;
    activeUplinesTooltip: SimpleTooltip;
    activeIPTooltip: SimpleTooltip;
    queryIPGroup: QueryIPGroup = {};

    constructor(
        $: JQueryStatic
    ) {
        this.$ = $;
    }

    init = () => {
        this.$.blockUI();
        this.cacheElements();
        this.registerEvents();
        this.$table.hide();
        this.$table.removeClass('hide');
        this.$.unblockUI();
        this.$reportForm.submit();
    };

    cacheElements = () => {
        this.$pageTitle = this.$(".page-title");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#WinlostDate");
        this.$table = this.$("#tbl-report");
        const $tableThead = this.$table.find("thead");
        this.$usernameFilterIcon = $tableThead.find(".col-username .icon-column-filter");
        this.usernameFilterTooltip = new SimpleTooltip(this.$, this.$usernameFilterIcon, {
            title: betMatchingResources.FilterByUsername,
            tooltipClass: 'username-filter'
        });
        this.$stakeFilterIcon = $tableThead.find(".col-stake .icon-column-filter");
        this.stakeFilterTooltip = new SimpleTooltip(this.$, this.$stakeFilterIcon, {
            title: betMatchingResources.FilterByStake,
            tooltipClass: 'stake-filter'
        });
        this.$memberWinLossFilterIcon = $tableThead.find(".col-memberwinloss .icon-column-filter");
        this.memberWinLossFilterTooltip = new SimpleTooltip(this.$, this.$memberWinLossFilterIcon, {
            title: betMatchingResources.FilterByMemberWinLoss,
            tooltipClass: 'memberwinloss-filter'
        });
        this.$tableTbody = this.$table.find("tbody");
        this.$tableTfoot = this.$table.find("tfoot");
        const $noinfoRow = this.$table.find("#noinfo-row");
        this.noinfoRowHtml = $noinfoRow.html();
        this.betChoiceUplines = new BetChoiceUplines(this.$);
    };

    registerEvents = () => {
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$datePicker.datepicker({
            allowInputToggle: true,
            //minDate: new Date(this.$('#MinDate').val()),
            maxDate: new Date(this.$('#Today').val()),
            format: alphaReports.format.shortDateWithSlash_DatePicker
        });

        this.usernameFilterTooltip.render(`
            <textarea class="form-control" autocomplete="off" rows="4" cols="50" placeholder="${betMatchingResources.FilterByUsername_Placeholder}"></textarea>
            <div class="filter-row">
                <button class="btn-submit">${betMatchingResources.Filter}</button>
            </div>
        `);
        this.usernameFilterTooltip.hide();
        this.$usernameFilterIcon.on("click", () => {
            this.hideColumnFilterTooltips();
            this.usernameFilterTooltip.show();
        });

        this.stakeFilterTooltip.render(`
            <div class="form-inline">
                <span class="form-label">${betMatchingResources.TotalStake} &gt;=</span>
                <input id="inputTotalStake" class="form-control input-sm" />
            </div>
            <div class="filter-row">
                <button class="btn-submit">${betMatchingResources.Filter}</button>
            </div>
        `);
        this.stakeFilterTooltip.hide();
        this.$stakeFilterIcon.on("click", () => {
            this.hideColumnFilterTooltips();
            this.stakeFilterTooltip.show();
        });

        this.memberWinLossFilterTooltip.render(`
            <div class="form-inline">
                <span class="form-label">${betMatchingResources.TotalMemberWL}</span>
                <select id="selectMemberWL" class="form-control input-sm">
                    <option value="0">&gt;=</option>
                    <option value="1">=</option>
                    <option value="2">&lt;=</option>
                </select>
                <input id="inputMemberWL" class="form-control input-sm" />
            </div>
            <div class="filter-row">
                <button class="btn-submit">${betMatchingResources.Filter}</button>
            </div>
        `);
        this.memberWinLossFilterTooltip.hide();
        this.$memberWinLossFilterIcon.on("click", () => {
            this.hideColumnFilterTooltips();
            this.memberWinLossFilterTooltip.show();
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
        this.$.blockUI();
        this.$table.hide();
        const params = alphaReports.getFormData(this.$reportForm);
        const queryUrl = this.$reportForm.attr('action');
        this.$.post(queryUrl, params)
            .done((data: QueryModel) => {
                this.data = data;
                this.groupIPs(data.IPs);
                const filterItems = this.filterItems(data.Items);
                const eventGroups = this.groupItems(filterItems);
                this.renderTable(eventGroups);
            })
            .fail(() => {
                alert('error');
            })
            .always(() => {
                this.$table.show();
                this.$.unblockUI();
            });
    };

    onExportExcel = () => {
        this.$exportExcelForm.submit();
    };

    groupIPs = (ips: QueryIP[]) => {
        this.queryIPGroup = {};
        ips.forEach(ip => {
            if (this.queryIPGroup[ip.IP]) {
                this.queryIPGroup[ip.IP].push(ip);
            } else {
                this.queryIPGroup[ip.IP] = [ip];
            }
        });
    }

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
    };

    filterItems = (items: QueryModelItem[]) => {
        return items;
    };

    formatNumber = (value: number, isRunning: boolean) => {
        if (isRunning) {
            return '-';
        }

        return alphaReports.formatCurrency(value);
    };

    renderTable = (eventGroups: QueryModelEventGroup[]) => {
        if (eventGroups.length === 0) {
            this.$tableTbody.html(this.noinfoRowHtml);
            this.$tableTfoot.hide();
            this.hideColumnFilterIcons();
            this.hideColumnFilterTooltips();
            return;
        }

        let totalMemberTurnover = null,
            totalMemberWinLoss = null,
            totalSuperWinLoss = null,
            totalCompany = null,
            isRunning = true;
        const html = [];
        eventGroups.forEach(eventGroup => {
            html.push(eventGroup.generateHtml(this.data));
            eventGroup.UserGroups.forEach((userGroup, index) => {
                totalMemberTurnover += userGroup.MemberTurnover;
                if (!userGroup.IsRunning) {
                    totalMemberWinLoss += userGroup.MemberWinLoss;
                    totalSuperWinLoss += userGroup.SuperWinLoss;
                    totalCompany += userGroup.Company;
                    isRunning = false;
                }

                html.push(`<tr class="${index % 2 === 1 ? "row-even row-total" : "row-total"}">
                    <td class="text-left" rowspan="${userGroup.Items.length + 1}"><span data-custid="${userGroup.CustId}" class="text-username" title="${betMatchingResources.BetMatching_Username_Title}">${userGroup.Username}</span></td>
                    <td class="text-right" colspan="4">${betMatchingResources.Total}</td>
                    <td>${this.formatNumber(userGroup.MemberTurnover, false)}</td>
                    <td>${this.formatNumber(userGroup.MemberWinLoss, userGroup.IsRunning)}</td>
                    <td class="col-superwinloss">${this.formatNumber(userGroup.SuperWinLoss, userGroup.IsRunning)}</td>
                    <td>${this.formatNumber(userGroup.Company, userGroup.IsRunning)}</td>
                </tr>`);
                userGroup.Items.forEach(item => {
                    let ipHtml = '';
                    if (this.queryIPGroup[item.IP]) {
                        ipHtml = ` <span class="icon-ip" data-ip="${item.IP}" title="${betMatchingResources.BetMatching_IP_Title}"></span>`;
                    }

                    html.push(`<tr class="${index % 2 === 1 ? "row-even" : ""}">
                        <td class="text-center">${moment(item.TransDate).format("MM/DD HH:mm:ss")}</td>
                        <td class="text-center">${item.TransId}</td>
                        <td class="text-center">${item.Odds}</td>
                        <td class="text-center">${item.IP ? item.IP : ''}${ipHtml}</td>
                        <td>${this.formatNumber(item.MemberTurnover, false)}</td>
                        <td>${this.formatNumber(item.MemberWinLoss, item.IsRunning)}</td>
                        <td class="col-superwinloss">${this.formatNumber(item.SuperWinLoss, item.IsRunning)}</td>
                        <td>${this.formatNumber(item.Company, item.IsRunning)}</td>
                    </tr>`);
                });
            });
        });
        this.$tableTbody.html(html.join(''));

        this.$tableTfoot.html(`
            <tr class="tbl-footer">
                <td colspan="5">${betMatchingResources.Total}</td>
                <td>${this.formatNumber(totalMemberTurnover, false)}</td>
                <td>${this.formatNumber(totalMemberWinLoss, isRunning)}</td>
                <td class="col-superwinloss">${this.formatNumber(totalSuperWinLoss, isRunning)}</td>
                <td>${this.formatNumber(totalCompany, isRunning)}</td>
            </tr>
        `);
        this.$tableTfoot.show();
        this.showColumnFilterIcons();

        this.$tableTbody.find('.text-username').click(this.onUsernameClick);
        this.$tableTbody.find('.icon-ip').click(this.onIPClick);
    };

    onUsernameClick = (e: MouseEvent) => {
        if (this.activeUplinesTooltip) {
            this.activeUplinesTooltip.hide();
        }

        const tooltipDataKey = 'simpleTooltip';
        const $target = this.$(e.target);
        let uplinesTooltip: SimpleTooltip = $target.data(tooltipDataKey);
        if (!uplinesTooltip) {
            uplinesTooltip = new SimpleTooltip(this.$, $target, {
                title: betMatchingResources.Upline
            });
            $target.data(tooltipDataKey, uplinesTooltip);
            uplinesTooltip.render(`<div class="loading-icon"></div>`);
            const custId = $target.data('custid');
            this.betChoiceUplines.query(custId, (data: UplinesModel) => {
                uplinesTooltip.render(`
                <div>${data.MasterName}</div>
                <div>${data.AgentName}</div>
            `);
            });
        }

        this.activeUplinesTooltip = uplinesTooltip;
        this.activeUplinesTooltip.show();
    };

    onIPClick = (e: MouseEvent) => {
        if (this.activeIPTooltip) {
            this.activeIPTooltip.hide();
        }

        const tooltipDataKey = 'simpleTooltip';
        const $target = this.$(e.target);
        let ipTooltip: SimpleTooltip = $target.data(tooltipDataKey);
        if (!ipTooltip) {
            ipTooltip = new SimpleTooltip(this.$, $target, {
                title: betMatchingResources.IP_used_by,
                direction: 'right'
            });
            $target.data(tooltipDataKey, ipTooltip);
            const ip = this.$(e.target).data('ip');
            const data = this.queryIPGroup[ip];
            const html = data.map((item: QueryIP, index: number) => `<div>${index + 1}: ${item.Username}</div>`).join('');
            ipTooltip.render(html);
        }

        this.activeIPTooltip = ipTooltip;
        this.activeIPTooltip.show();
    };

    showColumnFilterIcons = () => {
        this.$usernameFilterIcon.show();
        this.$stakeFilterIcon.show();
        this.$memberWinLossFilterIcon.show();
    };

    hideColumnFilterTooltips = () => {
        this.usernameFilterTooltip.hide();
        this.stakeFilterTooltip.hide();
        this.memberWinLossFilterTooltip.hide();
    };

    hideColumnFilterIcons = () => {
        this.$usernameFilterIcon.hide();
        this.$stakeFilterIcon.hide();
        this.$memberWinLossFilterIcon.hide();
    }
}

$(() => {
    const betChoiceMatching = new BetChoiceMatching(
        $);

    betChoiceMatching.init();
});

interface SimpleTooltipOption {
    tooltipClass?: string;
    direction?: string;
    title?: string;
}

const defaultOption: SimpleTooltipOption = {
    tooltipClass: '',
    direction: 'left',
    title: ''
};

class SimpleTooltip {
    private option: SimpleTooltipOption;
    private $tooltip: JQuery;
    private $tooltipTitle: JQuery;
    private $tooltipBody: JQuery;

    constructor(private $: JQueryStatic, private $target: JQuery, option = defaultOption) {
        this.option = this.$.extend({}, defaultOption, option);
    }

    render = (html: string) => {
        if (!this.$tooltip) {
            this.$tooltip = this.$(
                `<div class="simple-tooltip ${this.option.tooltipClass}">
                    <div class="tooltip-header">
                        <button type="button" class="icon icon-close close"></button>
                        <div class="tooltip-title">${this.option.title}</div>
                    </div>
                    <div class="tooltip-body">${html}</div>
                </div>`
            );
            this.$target.after(this.$tooltip);
            this.$tooltipTitle = this.$tooltip.find('.tooltip-title');
            this.$tooltipBody = this.$tooltip.find('.tooltip-body');
            const closeButton = this.$tooltip.find('.icon-close');
            closeButton.click(this.hide);
        } else {
            this.$tooltipBody.html(html);
        }

        if (this.option.direction === 'right') {
            const targetLeft = this.$target.offset().left;
            this.$tooltip.offset({ top: this.$tooltip.offset().top, left: targetLeft - this.$tooltip.width() });
        }
    };

    changeTitle = (html: string) => {
        this.$tooltipTitle.html(html);
    };

    hide = () => {
        this.$tooltip.hide();
    };

    show = () => {
        this.$tooltip.show();
    };
}

class FilterTooltip extends SimpleTooltip {
    $filterButton: JQuery;

    constructor($: JQueryStatic, $target: JQuery, option = defaultOption) {
        super($, $target, option);
    }
}