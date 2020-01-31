/// <reference path="../core.ts" />

"use strict";

interface JQuery {
    datepicker(option: any): void;
}

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

class QueryModelEventGroup {
    Key: string;
    LeagueId: number;
    SportType: number;
    BetType: number;
    GlobalShowTime: string;
    UserGroups: QueryModelUserGroup[];

    constructor(item: QueryModelItem) {
        this.LeagueId = item.LeagueId;
        this.SportType = item.SportType;
        this.BetType = item.BetType;
        this.GlobalShowTime = item.GlobalShowTime;
        this.Key = `${item.LeagueId}_${item.SportType}_${item.BetType}_${item.GlobalShowTime}`;
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
}

interface KeyValueModel {
    Key: any;
    Value: any;
}

interface QueryResource {
    Total: string;
}

interface QueryModel {
    Items: QueryModelItem[];
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

    getValueByKey(data: KeyValueModel[], key: any): any {
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            if (item.Key === key)
                return item.Value;
        }

        return null;
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
            html.push(`<tr class="row-bet-choice"> \
                    <td class="text-left col-bet-choice">
                        <span class="text-bet-type">${this.getValueByKey(this.data.BetTypes, eventGroup.BetType)}</span>
                    </td>
                    <td colspan="6" class="text-left col-league-sport">
                        <span class="text-league">${this.getValueByKey(this.data.Leagues, eventGroup.LeagueId)}</span>
                        <br />
                        <span class="text-sport-type">${this.getValueByKey(this.data.SportTypes, eventGroup.SportType)}</span>
                    </td>
                    <td colspan="2" class="text-center col-global-time">${moment(eventGroup.GlobalShowTime).format("MM/DD/YYYY HH:mm A")}</td>
                </tr>`);
            eventGroup.UserGroups.forEach((userGroup, index) => {
                totalMemberTurnover += userGroup.MemberTurnover;
                totalMemberWinLoss += userGroup.MemberWinLoss;
                totalSuperWinLoss += userGroup.SuperWinLoss;
                totalCompany += userGroup.Company;

                html.push(`<tr class="${index % 2 === 0 ? "even-row bold" : "bold"}"> \
                    <td class="text-left text-username" rowspan="${userGroup.Items.length + 1}">${userGroup.Username}</td>
                    <td class="text-right" colspan="4">${this.data.Resource.Total}</td>
                    <td>${this.formatNumber(userGroup.MemberTurnover)}</td>
                    <td>${this.formatNumber(userGroup.MemberWinLoss)}</td>
                    <td>${this.formatNumber(userGroup.SuperWinLoss)}</td>
                    <td>${this.formatNumber(userGroup.Company)}</td>
                </tr>`);
                userGroup.Items.forEach(item => {
                    html.push(`<tr> \
                        <td class="text-center">${moment(item.TransDate).format("MM/DD HH:mm:ss")}</td>
                        <td class="text-center">${item.TransId}</td>
                        <td class="text-center">${item.Odds}</td>
                        <td class="text-center">${item.IP}</td>
                        <td>${this.formatNumber(item.MemberTurnover)}</td>
                        <td>${this.formatNumber(item.MemberWinLoss)}</td>
                        <td>${this.formatNumber(item.SuperWinLoss)}</td>
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
                <td>${this.formatNumber(totalSuperWinLoss)}</td>
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
