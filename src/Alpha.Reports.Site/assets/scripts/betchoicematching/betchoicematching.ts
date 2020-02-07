/// <reference path="../core.ts" />

"use strict";

(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                this.value = "";
            }
        });
    };
}(jQuery));

declare const betMatchingResources: Resources;

interface JQuery {
    datepicker(option: any): void;
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
    noinfoRowHtml: string;
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
        this.$tableTbody = this.$table.find("tbody");
        this.$tableTfoot = this.$table.find("tfoot");
        const $noinfoRow = this.$table.find("#noinfo-row");
        this.noinfoRowHtml = $noinfoRow.html();
        this.betChoiceUplines = new BetChoiceUplines(this.$);
        this.usernameFilterForm = new UsernameFilterForm(this.$);
        this.stakeFilterForm = new StakeFilterForm(this.$);
        this.memberWinLossFilterForm = new MemberWinLossFilterForm(this.$);
    };

    registerEvents = () => {
        this.$filterIcon.on("click", this.onFilterEvent);
        this.$excelIcon.on("click", this.onExportExcel);
        this.$reportForm.submit(this.onSubmitReport);
        this.$datePicker.datepicker({
            allowInputToggle: true,
            // TODO
            //minDate: new Date(this.$('#MinDate').val()),
            maxDate: new Date(this.$('#Today').val()),
            format: alphaReports.format.shortDateWithSlash_DatePicker
        });
        this.usernameFilterForm.subscribeSubmit(this.filterAndRender);
        this.stakeFilterForm.subscribeSubmit(this.filterAndRender);
        this.memberWinLossFilterForm.subscribeSubmit(this.filterAndRender);
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

    onSubmitReport = (e: JQueryEventObject) => {
        e.preventDefault();
        this.$.blockUI();
        this.$table.hide();
        const params = alphaReports.getFormData(this.$reportForm);
        const queryUrl = this.$reportForm.attr('action');
        this.$.post(queryUrl, params)
            .done((data: QueryModel) => {
                this.data = data;
                this.groupIPs(data.IPs);
                this.filterAndRender();
            })
            .fail(() => {
                alert('error');
            })
            .always(() => {
                this.$table.show();
                this.$.unblockUI();
            });
    };

    filterAndRender = () => {
        const eventGroups = this.groupItems(this.data.Items);
        const filterEventGroups: QueryModelEventGroup[] = [];
        eventGroups.forEach(eventGroup => {
            this.usernameFilterForm.filter(eventGroup);
            this.stakeFilterForm.filter(eventGroup);
            this.memberWinLossFilterForm.filter(eventGroup);
            if (eventGroup.UserGroups.length > 0) {
                filterEventGroups.push(eventGroup);
            }
        })

        this.renderTable(filterEventGroups);
    }

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

    groupItems = (items: QueryModelItem[]): QueryModelEventGroup[] => {
        const eventGroups: QueryModelEventGroup[] = [];
        items.forEach(item => {
            let eventGroup = new QueryModelEventGroup(item);
            const filterEventGroups = eventGroups.filter(g => g.Key === eventGroup.Key);
            if (filterEventGroups.length === 0) {
                eventGroups.push(eventGroup);
            } else {
                filterEventGroups[0].addItem(item);
            }
        });

        return eventGroups;
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
            if (this.data.Items.length === 0) {
                this.hideColumnFilterIcons();
                this.hideColumnFilterTooltips();
            }

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
                    <td class="text-left" rowspan="${userGroup.Items.length + 1}">
                        <span data-custid="${userGroup.CustId}" class="text-username" title="${betMatchingResources.BetMatching_Username_Title}">${userGroup.Username}</span>
                    </td>
                    <td class="text-right" colspan="4">${betMatchingResources.Total}</td>
                    <td>${this.formatNumber(userGroup.MemberTurnover, false)}</td>
                    <td>${this.formatNumber(userGroup.MemberWinLoss, userGroup.IsRunning)}</td>
                    <td class="col-superwinloss">${this.formatNumber(userGroup.SuperWinLoss, userGroup.IsRunning)}</td>
                    <td>${this.formatNumber(userGroup.Company, userGroup.IsRunning)}</td>
                </tr>`);
                userGroup.Items.forEach(item => {
                    let ipHtml = '';
                    if (this.queryIPGroup[item.IP]) {
                        ipHtml = ` <span class="icon-ip" data-custid="${item.CustId}" data-ip="${item.IP}" title="${betMatchingResources.BetMatching_IP_Title}"></span>`;
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
    $tooltip: JQuery;
    private $tooltipTitle: JQuery;
    private $tooltipBody: JQuery;
    private hideCallback: () => void;

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
        if (this.hideCallback) {
            this.hideCallback();
        }
    };

    show = () => {
        this.$tooltip.show();
    };

    onHide = (callback: () => void) => {
        this.hideCallback = callback;
    }
}

abstract class BaseFilterForm {
    protected simpleTooltip: SimpleTooltip;
    protected $filterIcon: JQuery;
    protected $form: JQuery;
    protected filterCriteria: JQuerySerializeArrayElement[];
    private listener: () => void;
    static tooltips: SimpleTooltip[] = [];

    constructor($: JQueryStatic, iconFilterClassname: string, tooltipOption: SimpleTooltipOption, tooltipContent: string) {
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
        this.filterCriteria = this.$form.serializeArray();
        this.simpleTooltip.onHide(this.onHideTooltip);
    }

    onHideTooltip = () => {
        const filterCriteria = this.$form.serializeArray();
        if (JSON.stringify(filterCriteria) !== JSON.stringify(this.filterCriteria)) {
            this.restoreCriteria();
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
        this.filterCriteria = this.$form.serializeArray();
        this.$filterIcon.removeClass('has-filter');
        if (this.hasFilter()) {
            this.$filterIcon.addClass('has-filter');
        }

        this.listener && this.listener();
    }

    subscribeSubmit(listener: () => void) {
        this.listener = listener;
    }

    abstract hasFilter(): boolean;

    abstract restoreCriteria();

    abstract filter(eventGroup: QueryModelEventGroup);
}

class UsernameFilterForm extends BaseFilterForm {
    static iconFilterClassname = "thead .col-username .icon-column-filter";
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

    constructor(private $: JQueryStatic) {
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

    getCriteria() {
        return {
            Username: this.$.trim(this.filterCriteria[0].value)
        };
    }

    hasFilter(): boolean {
        const criteria = this.getCriteria();
        return !!criteria.Username;
    }

    restoreCriteria = () => {
        this.$textareaUsername.val(this.getCriteria().Username);
    }

    filter(eventGroup: QueryModelEventGroup) {
        if (!this.hasFilter()) {
            return;
        }

        const username = this.getCriteria().Username;
        const usernames = username.split(',').map(x => $.trim(x));
        const filterUserGroups = [];
        eventGroup.UserGroups.forEach(userGroup => {
            if (usernames.indexOf(userGroup.Username) >= 0) {
                filterUserGroups.push(userGroup);
            }
        });
        eventGroup.UserGroups = filterUserGroups;
    }
}

class StakeFilterForm extends BaseFilterForm {
    static iconFilterClassname = "thead .col-stake .icon-column-filter";
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

    constructor(private $: JQueryStatic) {
        super($, StakeFilterForm.iconFilterClassname, StakeFilterForm.tooltipOption, StakeFilterForm.tooltipContent);
        this.$inputTotalStake = this.$form.find('input');
        this.$inputTotalStake.inputFilter(value => /^-?\d*[.,]?\d{0,2}$/.test(value));
    }

    getCriteria() {
        return {
            TotalStake: this.$.trim(this.filterCriteria[0].value)
        };
    }

    hasFilter(): boolean {
        const criteria = this.getCriteria();
        return !!criteria.TotalStake;
    }

    restoreCriteria = () => {
        this.$inputTotalStake.val(this.getCriteria().TotalStake);
    }

    filter(eventGroup: QueryModelEventGroup) {
        if (!this.hasFilter()) {
            return;
        }

        const totalStake = parseFloat(this.getCriteria().TotalStake);
        const filterUserGroups = [];
        eventGroup.UserGroups.forEach(userGroup => {
            if (userGroup.MemberTurnover >= totalStake) {
                filterUserGroups.push(userGroup);
            }
        });

        eventGroup.UserGroups = filterUserGroups;
    }
}

class MemberWinLossFilterForm extends BaseFilterForm {
    static iconFilterClassname = "thead .col-memberwinloss .icon-column-filter";
    static tooltipOption = {
        title: betMatchingResources.FilterByMemberWinLoss,
        tooltipClass: 'memberwinloss-filter'
    }
    static tooltipContent = `
        <form class="form-inline">
            <div>
                <span class="form-label">${betMatchingResources.TotalMemberWL}</span>
                <select name="Operator" class="form-control input-sm">
                    <option value="0">&gt;=</option>
                    <option value="1">=</option>
                    <option value="2">&lt;=</option>
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

    constructor(private $: JQueryStatic) {
        super($, MemberWinLossFilterForm.iconFilterClassname, MemberWinLossFilterForm.tooltipOption, MemberWinLossFilterForm.tooltipContent);
        this.$selectOperator = this.$form.find('select');
        this.$inputMemberWinLoss = this.$form.find('input');
        this.$inputMemberWinLoss.inputFilter(value => /^-?\d*[.,]?\d{0,2}$/.test(value));
    }

    getCriteria() {
        return {
            Operator: this.$.trim(this.filterCriteria[0].value),
            MemberWinLoss: this.$.trim(this.filterCriteria[1].value)
        };
    }

    hasFilter(): boolean {
        const criteria = this.getCriteria();
        return !!criteria.MemberWinLoss;
    }

    restoreCriteria = () => {
        const criteria = this.getCriteria();
        this.$selectOperator.val(criteria.Operator);
        this.$inputMemberWinLoss.val(criteria.MemberWinLoss);
    }

    filter(eventGroup: QueryModelEventGroup) {
        if (!this.hasFilter()) {
            return;
        }

        const criteria = this.getCriteria();
        const memberWinLoss = parseFloat(criteria.MemberWinLoss);
        const operator = parseInt(criteria.Operator);
        const filterUserGroups = [];
        eventGroup.UserGroups.forEach(userGroup => {
            switch (operator) {
                case 0:
                    if (userGroup.MemberWinLoss >= memberWinLoss) {
                        filterUserGroups.push(userGroup);
                    }
                    break;
                case 1:
                    if (userGroup.MemberWinLoss === memberWinLoss) {
                        filterUserGroups.push(userGroup);
                    }
                    break;
                case 2:
                    if (userGroup.MemberWinLoss <= memberWinLoss) {
                        filterUserGroups.push(userGroup);
                    }
                    break;
            }
        });

        eventGroup.UserGroups = filterUserGroups;
    }
}