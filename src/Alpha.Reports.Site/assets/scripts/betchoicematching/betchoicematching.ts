/// <reference path="../core.ts" />
/// <reference path="../components/simple-tooltip.ts" />

"use strict";

interface DatepickerOptions {
    allowInputToggle: boolean;
    minDate: Date;
    maxDate: Date;
    format: string;
}

interface JQuery {
    inputFilter(option: any): void;
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

enum MemberWinLossOperators {
    GreaterOrEqual = 0,
    Equal = 1,
    LessOrEqual = 2
}

const BetTeamClassNames = {
    Underdog: "underdog",
    Favorite: "favorite"
};

interface QueryModel {
    Events: QueryEventModel[];
    Teams: KeyValueModel[];
    Leagues: KeyValueModel[];
    SportTypes: KeyValueModel[];
    BetTypes: KeyValueModel[];
    IPs: QueryIP[];
}

interface QueryUserModel {
    CustId: number;
    Username: string;
    Stake: number;
    MemberWinLoss: number;
    SuperWinLoss: number;
    Company: number;
    IsRunning: boolean;
    Tickets: QueryTicketModel[];
}

interface QueryEventModel {
    LeagueId: number;
    SportType: number;
    BetType: number;
    Hdp1: number;
    Hdp2: number;
    HomeId: number;
    AwayId: number;
    BetTeam: string;
    GlobalShowTime: string;
    IsLive: boolean;
    LiveHomeScore: number;
    LiveAwayScore: number;
    IsRunning: boolean;
    Users: QueryUserModel[];
}

interface QueryTicketModel {
    LeagueId: number;
    SportType: number;
    BetType: number;
    CustId: number;
    Username: string;
    TransDate: string;
    GlobalShowTime: string;
    TransId: number;
    Odds: number;
    DisplayOdds: string;
    DisplayOddsType: string;
    IP: string;
    Stake: number;
    MemberWinLoss: number;
    SuperWinLoss: number;
    Company: number;
    MatchId: number;
    HomeId: number;
    AwayId: number;
    IsLive: boolean;
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
    FT: string;
    HT: string;
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
    IsLive: boolean;
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
                    ${this.buildLiveScore()}
                    <br />
                    <span class="text-bet-type">${getValueByKey(data.BetTypes, this.ticket.BetType)}</span>
                </span>`;
    }

    private buildLiveScore(): string {
        if (!this.ticket.IsLive) {
            return '';
        }

        return `<span class="text-live-score">[${this.ticket.LiveHomeScore}-${this.ticket.LiveAwayScore}]</span>`;
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
    generateBetTeam(): string {
        return this.IsBetHomeTeam ? betMatchingResources.Over : betMatchingResources.Under;
    }
}

class OneXTwoChoiceBuilder extends ChoiceBuilder {
    get BetTeamClassName(): string {
        return BetTeamClassNames.Favorite;
    }

    generateBetTeam(): string {
        const prefix = this.ticket.BetType === BetTypes.OneXTwoFT ? betMatchingResources.FT : betMatchingResources.HT;
        return `${prefix}.${this.ticket.BetTeam ? this.ticket.BetTeam.toUpperCase() : ''}`;
    }

    generateHandicap(): string {
        return '';
    }
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

class BetChoiceUplines {
    $: JQueryStatic;
    $uplinesForm: JQuery;
    $searchAccountId: JQuery;
    uplinesCache: { [custId: number]: UplinesModel } = {};

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
            .fail((jqXHR) => {
                const jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = top["alpha"].urls.mainRootPath
                        + "PartialUM/Index?umfrom=" + jsonData.UMFrom
                        + "&umto=" + jsonData.UMTo;
                } else {
                    callback(null);
                }
            });
    };
}

interface QueryParam {
    WinlostDate: string;
    BetCount: string;
}

class BetChoiceMatching {
    $: JQueryStatic;
    $pageTitle: JQuery;
    $reportForm: JQuery;
    $exportExcelForm: JQuery;
    $filterIcon: JQuery;
    $excelIcon: JQuery;
    $betCount: JQuery;
    $datePicker: JQuery;
    $datePickerInput: JQuery;
    $table: JQuery;
    $tableTbody: JQuery;
    $tableTfoot: JQuery;
    $excelWinlostDate: JQuery;
    $excelBetCount: JQuery;
    $excelUsername: JQuery;
    $excelStake: JQuery;
    $excelMemberWinLossOperator: JQuery;
    $excelMemberWinLossValue: JQuery;
    noinfoRowHtml: string;
    queryParam: QueryParam;
    data: QueryModel;
    betChoiceUplines: BetChoiceUplines;
    usernameFilterForm: UsernameFilterForm;
    stakeFilterForm: StakeFilterForm;
    memberWinLossFilterForm: MemberWinLossFilterForm;
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
        this.$.unblockUI();
        this.submitOnLoad();
    };

    cacheElements = () => {
        this.$pageTitle = this.$(".page-title");
        this.$reportForm = this.$("#report-form");
        this.$exportExcelForm = this.$("#export-excel-form");
        this.$excelWinlostDate = this.$("#ExcelWinlostDate");
        this.$excelBetCount = this.$("#ExcelBetCount");
        this.$excelUsername = this.$("#ExcelUsername");
        this.$excelStake = this.$("#ExcelStake");
        this.$excelMemberWinLossOperator = this.$("#ExcelMemberWinLossOperator");
        this.$excelMemberWinLossValue = this.$("#ExcelMemberWinLossValue");
        this.$filterIcon = this.$(".icon-filter");
        this.$excelIcon = this.$("#icon-excel");
        this.$betCount = this.$("#BetCount");
        this.$datePicker = this.$("#dpkContainer");
        this.$datePickerInput = this.$("#WinlostDate");
        this.$table = this.$("#tbl-report");
        this.$tableTbody = this.$table.find("tbody");
        this.$tableTfoot = this.$table.find("tfoot");
        const $noinfoRow = this.$table.find("#noinfo-row");
        this.noinfoRowHtml = $noinfoRow.prop("outerHTML");
        this.betChoiceUplines = new BetChoiceUplines(this.$);
        this.usernameFilterForm = new UsernameFilterForm(this.$);
        this.stakeFilterForm = new StakeFilterForm(this.$);
        this.memberWinLossFilterForm = new MemberWinLossFilterForm(this.$);
    };

    registerEvents = () => {
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        const options: DatepickerOptions = {
            allowInputToggle: true,
            minDate: new Date(this.$('#MinDate').val()),
            maxDate: new Date(this.$('#Today').val()),
            format: alphaReports.format.shortDateWithSlash_DatePicker
        }
        this.$datePicker.datepicker(options);
        this.usernameFilterForm.subscribeSubmit(this.filterAndRender);
        this.stakeFilterForm.subscribeSubmit(this.filterAndRender);
        this.memberWinLossFilterForm.subscribeSubmit(this.filterAndRender);
    };

    submitOnLoad = () => {
        this.$table.hide();
        this.$table.removeClass('hide');
        this.$reportForm.submit();
    }

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

    onSubmitReport = (e: JQueryEventObject) => {
        e.preventDefault();
        this.$.blockUI();
        this.removeFilterCriteria();
        this.hideColumnFilterIcons();
        this.hideColumnFilterTooltips();
        this.$table.hide();
        this.queryParam = alphaReports.getFormData(this.$reportForm);
        const queryUrl = this.$reportForm.attr('action');
        this.$.post(queryUrl, this.queryParam)
            .done((data: QueryModel) => {
                this.data = data;
                this.groupIPs(data.IPs);
                this.filterAndRender();
            })
            .fail((jqXHR) => {
                const jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = top["alpha"].urls.mainRootPath
                        + "PartialUM/Index?umfrom=" + jsonData.UMFrom
                        + "&umto=" + jsonData.UMTo;
                }
            })
            .always(() => {
                this.$table.show();
                this.$.unblockUI();
            });
    };

    filterAndRender = () => {
        const filterEvents: QueryEventModel[] = [];
        this.data.Events.forEach(event => {
            let filterUsers = event.Users;
            filterUsers = this.usernameFilterForm.filter(filterUsers);
            filterUsers = this.stakeFilterForm.filter(filterUsers);
            filterUsers = this.memberWinLossFilterForm.filter(filterUsers);
            if (filterUsers.length > 0) {
                event.Users = filterUsers;
                filterEvents.push(event);
            }
        })

        this.renderTable(filterEvents);
    }

    onExportExcel = () => {
        this.$excelWinlostDate.val(this.queryParam.WinlostDate);
        this.$excelBetCount.val(this.queryParam.BetCount);
        this.$excelUsername.val(this.usernameFilterForm.getCriteria().Username);
        this.$excelStake.val(this.stakeFilterForm.getCriteria().TotalStake);
        this.$excelMemberWinLossOperator.val(this.memberWinLossFilterForm.getCriteria().Operator);
        this.$excelMemberWinLossValue.val(this.memberWinLossFilterForm.getCriteria().MemberWinLoss);
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

    formatNumber = (value: number, isRunning: boolean) => {
        if (isRunning) {
            return '-';
        }

        return alphaReports.formatCurrency(value);
    };

    renderTable = (events: QueryEventModel[]) => {
        if (events.length === 0) {
            this.$tableTbody.html(this.noinfoRowHtml);
            this.$tableTfoot.hide();
            return;
        }

        this.showColumnFilterIcons();
        let totalStake = null,
            totalMemberWinLoss = null,
            totalSuperWinLoss = null,
            totalCompany = null,
            isRunning = true;
        const html = [];
        events.forEach(event => {
            html.push(this.generateBetChoice(event));
            event.Users.forEach((user, index) => {
                totalStake += user.Stake;
                if (!user.IsRunning) {
                    totalMemberWinLoss += user.MemberWinLoss;
                    totalSuperWinLoss += user.SuperWinLoss;
                    totalCompany += user.Company;
                    isRunning = false;
                }

                html.push(`<tr class="${index % 2 === 1 ? "row-even row-total" : "row-total"}">
                    <td class="text-left" rowspan="${user.Tickets.length + 1}">
                        <span data-custid="${user.CustId}" class="text-username" title="${betMatchingResources.BetMatching_Username_Title}">${user.Username}</span>
                    </td>
                    <td class="text-right" colspan="4">${betMatchingResources.Total}</td>
                    <td>${alphaReports.formatNumber(user.Stake)}</td>
                    <td>${this.formatNumber(user.MemberWinLoss, user.IsRunning)}</td>
                    <td class="col-superwinloss">${this.formatNumber(user.SuperWinLoss, user.IsRunning)}</td>
                    <td>${this.formatNumber(user.Company, user.IsRunning)}</td>
                </tr>`);
                user.Tickets.forEach(ticket => {
                    let ipHtml = '';
                    if (this.queryIPGroup[ticket.IP]) {
                        ipHtml = ` <span class="icon-ip-share" data-custid="${ticket.CustId}" data-ip="${ticket.IP}" title="${betMatchingResources.BetMatching_IP_Title}"></span>`;
                    }

                    html.push(`<tr class="${index % 2 === 1 ? "row-even" : ""}">
                        <td class="text-center">${moment(ticket.TransDate).format("MM/DD HH:mm:ss")}</td>
                        <td class="text-center">${ticket.TransId}</td>
                        <td class="text-center">${this.buildOdds(ticket)}</td>
                        <td class="text-center">${ticket.IP ? ticket.IP : ''}${ipHtml}</td>
                        <td>${alphaReports.formatNumber(ticket.Stake)}</td>
                        <td>${this.formatNumber(ticket.MemberWinLoss, ticket.IsRunning)}</td>
                        <td class="col-superwinloss">${this.formatNumber(ticket.SuperWinLoss, ticket.IsRunning)}</td>
                        <td>${this.formatNumber(ticket.Company, ticket.IsRunning)}</td>
                    </tr>`);
                });
            });
        });
        this.$tableTbody.html(html.join(''));

        this.$tableTfoot.html(`
            <tr class="tbl-footer">
                <td colspan="5">${betMatchingResources.Total}</td>
                <td>${alphaReports.formatNumber(totalStake)}</td>
                <td>${this.formatNumber(totalMemberWinLoss, isRunning)}</td>
                <td class="col-superwinloss">${this.formatNumber(totalSuperWinLoss, isRunning)}</td>
                <td>${this.formatNumber(totalCompany, isRunning)}</td>
            </tr>
        `);
        this.$tableTfoot.show();
        this.$tableTbody.find('.text-username').click(this.onUsernameClick);
        this.$tableTbody.find('.icon-ip-share').click(this.onIPClick);
    };

    buildOdds(ticket: QueryTicketModel) {
        if (ticket.Odds == 0) {
            return ticket.DisplayOdds;
        }

        const formatOdds = ticket.Odds < 0
            ? `<font color='#B50000'>${ticket.DisplayOdds}</font>`
            : ticket.DisplayOdds;

        if (ticket.DisplayOddsType) {
            return `(${ticket.DisplayOddsType}) ${formatOdds}`;
        } else {
            return formatOdds;
        }
    }

    generateBetChoice(event: QueryEventModel) {
        const getChoiceBuilder = (): ChoiceBuilder => {
            switch (event.BetType) {
                case BetTypes.HandicapFT:
                case BetTypes.Handicap1H:
                    return new HandicapChoiceBuilder(event);
                case BetTypes.OverUnderFT:
                case BetTypes.OverUnder1H:
                    return new OverUnderChoiceBuilder(event);
                case BetTypes.OneXTwoFT:
                case BetTypes.OneXTwo1H:
                    return new OneXTwoChoiceBuilder(event);
            }

            throw new Error(`ChoiceBuilder is not implemented for bettype ${event.BetType}`);
        }

        const choiceBuilder = getChoiceBuilder();
        return `<tr class="row-bet-choice">
                    <td class="text-left col-bet-choice">${choiceBuilder.build(this.data)}</td>
                    <td colspan="6" class="text-left col-event">
                        <span class="text-event">${getValueByKey(this.data.Teams, event.HomeId)}${betMatchingResources.Versus}${getValueByKey(this.data.Teams, event.AwayId)}</span>
                        <br />
                        <span class="text-sport-type">${getValueByKey(this.data.SportTypes, event.SportType)}</span>
                        ${' '}
                        <span class="text-league">${getValueByKey(this.data.Leagues, event.LeagueId)}</span>
                    </td>
                    <td colspan="2" class="text-center col-global-time">${moment(event.GlobalShowTime).format("MM/DD/YYYY hh:mm A")}</td>
                </tr>`;
    }

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
                direction: 'right',
                tooltipClass: 'ip-tooltip'
            });
            $target.data(tooltipDataKey, ipTooltip);
            const ip = $target.data('ip');
            const custId = $target.data('custid');
            const data = this.queryIPGroup[ip];
            const html = [];
            let index = 0;
            data.forEach(item => {
                if (item.CustId !== custId) {
                    index++;
                    html.push(`<div>${index}: ${item.Username}</div>`);
                }
            });
            ipTooltip.render(html.join(''));
        }

        this.activeIPTooltip = ipTooltip;
        this.activeIPTooltip.show();
    };

    showColumnFilterIcons = () => {
        this.usernameFilterForm.showIcon();
        this.stakeFilterForm.showIcon();
        this.memberWinLossFilterForm.showIcon();
    };

    hideColumnFilterIcons = () => {
        this.usernameFilterForm.hideIcon();
        this.stakeFilterForm.hideIcon();
        this.memberWinLossFilterForm.hideIcon();
    };

    hideColumnFilterTooltips = () => {
        this.usernameFilterForm.hideTooltip();
        this.stakeFilterForm.hideTooltip();
        this.memberWinLossFilterForm.hideTooltip();
    };

    removeFilterCriteria = () => {
        this.usernameFilterForm.removeCriteria();
        this.stakeFilterForm.removeCriteria();
        this.memberWinLossFilterForm.removeCriteria();
    }
}

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

abstract class BaseFilterForm<T> {
    protected simpleTooltip: SimpleTooltip;
    protected $filterIcon: JQuery;
    protected $form: JQuery;
    protected filterCriteria: T;
    private listener: () => void;
    static tooltips: SimpleTooltip[] = [];

    constructor(protected $: JQueryStatic, iconFilterClassname: string, tooltipOption: SimpleTooltipOption, tooltipContent: string) {
        this.$filterIcon = $(iconFilterClassname);
        this.simpleTooltip = new SimpleTooltip($, this.$filterIcon, tooltipOption);
        this.simpleTooltip.render(tooltipContent);
        this.simpleTooltip.hide();
        BaseFilterForm.tooltips.push(this.simpleTooltip);
        this.$filterIcon.on("click", () => {
            BaseFilterForm.tooltips.forEach(tooltip => tooltip.hide());
            this.simpleTooltip.show();
        });
        this.$form = this.simpleTooltip.$tooltip.find('form');
        this.$form.submit(this.onSubmit);
        this.simpleTooltip.onHide(this.onHideTooltip);
    }

    onHideTooltip = () => {
        if (this.filterCriteria) {
            const filterCriteria = this.getCriteria();
            if (JSON.stringify(filterCriteria) !== JSON.stringify(this.filterCriteria)) {
                this.restoreCriteria(this.filterCriteria);
            }
        } else {
            this.filterCriteria = this.getCriteria();
        }
    }

    hideIcon = () => {
        this.$filterIcon.hide();
    }

    showIcon = () => {
        this.$filterIcon.show();
    }

    hideTooltip = () => {
        this.simpleTooltip.hide();
    }

    showTooltip = () => {
        this.simpleTooltip.show();
    }

    onSubmit = (e: JQueryEventObject) => {
        e.preventDefault();
        this.filterCriteria = this.getCriteria();
        this.$filterIcon.removeClass('has-filter');
        if (this.hasFilter()) {
            this.$filterIcon.addClass('has-filter');
        }

        this.listener && this.listener();
    }

    subscribeSubmit(listener: () => void) {
        this.listener = listener;
    }

    removeCriteria = () => {
        this.filterCriteria = undefined;
        this.$filterIcon.removeClass('has-filter');
        this.resetCriteria();
    }

    abstract hasFilter(): boolean;

    abstract getCriteria(): T;

    protected abstract resetCriteria(): void;

    abstract restoreCriteria(filterCriteria: T): void;

    filter(users: QueryUserModel[]): QueryUserModel[] {
        if (!this.hasFilter()) {
            return users;
        }

        const filterUsers = [];
        users.forEach(user => {
            if (this.isMatch(user, this.filterCriteria)) {
                filterUsers.push(user);
            }
        });

        return filterUsers;
    }

    isMatch(user: QueryUserModel, filterCriteria: T): boolean {
        return false;
    }
}

interface UsernameFilterCriteria {
    Username: string;
}

class UsernameFilterForm extends BaseFilterForm<UsernameFilterCriteria> {
    static iconFilterClassname = "thead .col-username .icon-table-filter";
    static tooltipOption = {
        title: betMatchingResources.FilterByUsername,
        tooltipClass: 'username-filter'
    }
    static tooltipContent = `
        <form>
            <textarea class="form-control" name="Username" autocomplete="off" rows="4" cols="50" placeholder="${betMatchingResources.FilterByUsername_Placeholder}"></textarea>
            <div class="text-right">
                <button class="btn-submit btn-filter" type="submit">${betMatchingResources.Filter}</button>
            </div>
        </form>
    `;

    $textareaUsername: JQuery;

    constructor($: JQueryStatic) {
        super($, UsernameFilterForm.iconFilterClassname, UsernameFilterForm.tooltipOption, UsernameFilterForm.tooltipContent);
        this.$textareaUsername = this.$form.find('textarea');
        this.$textareaUsername.focusout(this.textareaLostFocus);
    }

    textareaLostFocus = () => {
        let username = this.$.trim(this.$textareaUsername.val()).replace(/(?:\r\n|\r|\n)/g, ',');
        const usernames = username.split(',');
        const newUsernames = [];
        usernames.forEach(x => {
            const trimmed = this.$.trim(x);
            if (trimmed) {
                newUsernames.push(trimmed);
            }
        })
        username = newUsernames.join(',');
        this.$textareaUsername.val(username);
    }

    getCriteria(): UsernameFilterCriteria {
        return {
            Username: this.$.trim(this.$textareaUsername.val())
        };
    }

    hasFilter(): boolean {
        const criteria = this.getCriteria();
        return !!criteria.Username;
    }

    protected resetCriteria() {
        this.$textareaUsername.val('');
    }

    restoreCriteria = (filterCriteria: UsernameFilterCriteria) => {
        this.$textareaUsername.val(filterCriteria.Username);
    }

    filter(users: QueryUserModel[]): QueryUserModel[] {
        if (!this.hasFilter()) {
            return users;
        }

        const username = this.filterCriteria.Username.toLowerCase();
        const usernames = username.split(',').map(x => $.trim(x).toLowerCase());
        const filterUsers = [];
        users.forEach(user => {
            if (usernames.indexOf(user.Username.toLowerCase()) >= 0) {
                filterUsers.push(user);
            }
        });

        return filterUsers;
    }
}

interface StakeFilterCriteria {
    TotalStake: number;
}

class StakeFilterForm extends BaseFilterForm<StakeFilterCriteria> {
    static iconFilterClassname = "thead .col-stake .icon-table-filter";
    static tooltipOption = {
        title: betMatchingResources.FilterByStake,
        tooltipClass: 'stake-filter'
    }
    static tooltipContent = `
        <form class="form-inline">
            <div>
                <span class="form-label">${betMatchingResources.TotalStake} &gt;=</span>
                <input name="TotalStake" class="form-control input-sm" />
            </div>
            <div class="text-right">
                <button class="btn-submit btn-filter">${betMatchingResources.Filter}</button>
            </div>
        </form>
    `;

    $inputTotalStake: JQuery;

    constructor($: JQueryStatic) {
        super($, StakeFilterForm.iconFilterClassname, StakeFilterForm.tooltipOption, StakeFilterForm.tooltipContent);
        this.$inputTotalStake = this.$form.find('input');
        this.$inputTotalStake.cleave({
            numeral: true,
            numeralIntegerScale: 15,
            numeralDecimalScale: 0,
            numeralThousandsGroupStyle: 'thousand',
            autoUnmask: true
        });
    }

    getCriteria(): StakeFilterCriteria {
        const rawValue = parseInt(this.$inputTotalStake.data('cleave').getRawValue());
        return {
            TotalStake: isNaN(rawValue) ? null : rawValue
        };
    }

    hasFilter(): boolean {
        const criteria = this.getCriteria();
        return criteria.TotalStake !== null;
    }

    protected resetCriteria() {
        this.$inputTotalStake.val('');
    }

    restoreCriteria = (filterCriteria: StakeFilterCriteria) => {
        this.$inputTotalStake.data('cleave').setRawValue(filterCriteria.TotalStake);
    }

    isMatch(user: QueryUserModel, filterCriteria: StakeFilterCriteria): boolean {
        const totalStake = filterCriteria.TotalStake;
        return user.Stake >= totalStake;
    }
}

interface MemberWinLossFilterCriteria {
    Operator: number;
    MemberWinLoss: number;
}

class MemberWinLossFilterForm extends BaseFilterForm<MemberWinLossFilterCriteria> {
    static iconFilterClassname = "thead .col-memberwinloss .icon-table-filter";
    static tooltipOption = {
        title: betMatchingResources.FilterByMemberWinLoss,
        tooltipClass: 'memberwinloss-filter'
    }
    static tooltipContent = `
        <form class="form-inline">
            <div>
                <span class="form-label">${betMatchingResources.TotalMemberWL}</span>
                <select name="Operator" class="form-control input-sm">
                    <option value="${MemberWinLossOperators.GreaterOrEqual}" selected>&gt;=</option>
                    <option value="${MemberWinLossOperators.Equal}">=</option>
                    <option value="${MemberWinLossOperators.LessOrEqual}">&lt;=</option>
                </select>
                <input name="MemberWinLoss" class="form-control input-sm" />
            </div>
            <div class="text-right">
                <button class="btn-submit btn-filter">${betMatchingResources.Filter}</button>
            </div>
        </form>
    `;

    $selectOperator: JQuery;
    $inputMemberWinLoss: JQuery;

    constructor($: JQueryStatic) {
        super($, MemberWinLossFilterForm.iconFilterClassname, MemberWinLossFilterForm.tooltipOption, MemberWinLossFilterForm.tooltipContent);
        this.$selectOperator = this.$form.find('select');
        this.$inputMemberWinLoss = this.$form.find('input');
        this.$inputMemberWinLoss.cleave({
            numeral: true,
            numeralIntegerScale: 15,
            numeralThousandsGroupStyle: 'thousand',
            autoUnmask: true
        });
    }

    getCriteria(): MemberWinLossFilterCriteria {
        const rawValue = parseFloat(this.$inputMemberWinLoss.data('cleave').getRawValue());
        return {
            Operator: parseInt(this.$selectOperator.val()),
            MemberWinLoss: isNaN(rawValue) ? null : rawValue
        };
    }

    hasFilter(): boolean {
        const criteria = this.getCriteria();
        return criteria.MemberWinLoss !== null;
    }

    protected resetCriteria() {
        this.$selectOperator.val(MemberWinLossOperators.GreaterOrEqual);
        this.$inputMemberWinLoss.val('');
    }

    restoreCriteria = (filterCriteria: MemberWinLossFilterCriteria) => {
        this.$selectOperator.val(filterCriteria.Operator);
        this.$inputMemberWinLoss.data('cleave').setRawValue(filterCriteria.MemberWinLoss);
    }

    isMatch(user: QueryUserModel, filterCriteria: MemberWinLossFilterCriteria): boolean {
        const memberWinLoss = filterCriteria.MemberWinLoss;
        const operator = filterCriteria.Operator;
        switch (operator) {
            case MemberWinLossOperators.GreaterOrEqual:
                return !user.IsRunning && user.MemberWinLoss >= memberWinLoss;
            case MemberWinLossOperators.Equal:
                return !user.IsRunning && user.MemberWinLoss === memberWinLoss;
            case MemberWinLossOperators.LessOrEqual:
                return !user.IsRunning && user.MemberWinLoss <= memberWinLoss;
        }
    }
}

$(() => {
    const betChoiceMatching = new BetChoiceMatching(
        $);

    betChoiceMatching.init();
});