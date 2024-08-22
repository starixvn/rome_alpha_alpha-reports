'use strict';
declare var columnList: any;

class SettlementColumns {
    private visibleColumnDropdown: VisibleColumnDropdownComponent;
    private localStorageKey = `wlsettlement_${top.$('#userId').val()}`;

    constructor(visibleColumnDropdown: VisibleColumnDropdownComponent) {
        this.visibleColumnDropdown = visibleColumnDropdown;
    }

    init() {
        this.visibleColumnDropdown.render(new VisibleColumnDropdownOption(
            alphaReports.localStorage.get(this.localStorageKey) || '',
            columnList,
            (columns) => {
                this.showHideColumns(columns);
                const selectedColumnsValue = [];
                Object.entries(columns).forEach(([key, value]) => value && selectedColumnsValue.push(key));
                alphaReports.localStorage.set(this.localStorageKey, selectedColumnsValue.join(','));
            },
            undefined,
            260));
    }

    showHideColumns(visibleColumn = undefined) {
        const $tblReport = $('#tbl-report');
        visibleColumn = visibleColumn || this.visibleColumnDropdown.getColumns();
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupTotal'), ['month', 'whitelabel', 'agent', 'master']);
        this.showHideSpecificColumn($tblReport, visibleColumn, 'losscarryforward');
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupMember'), ['memberwinloss', 'memberpromotion']);
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupWhiteLabel'), ['wlnetwinloss', 'wlincome', 'wlfee', 'wlpromotionshare', 'wlpromotionsettlement', 'wltotal']);
        this.showHideSpecificColumn($tblReport, visibleColumn, 'masterearning');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'superearning');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'headlineearning');
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupCompany'), ['companyincome', 'companypromotionshare', 'companytotal']);
    }

    private showHideGroupColumn($tblReport: JQuery, visibleColumn, $groupColumn: JQuery, columnNames: string[]) {
        let colspan = 0;
        columnNames.forEach((item) => {
            if (this.showHideSpecificColumn($tblReport, visibleColumn, item)) {
                colspan++;
            }
        });

        if ($groupColumn.length) {
            colspan ? $groupColumn.attr('colspan', colspan).show() : $groupColumn.hide();
        }
    }

    private showHideSpecificColumn($tblReport: JQuery, visibleColumn, key: string) {
        const visible = visibleColumn[key];
        $tblReport.find(`[data-colname=${key}]`).toggle(visible);

        return visible;
    }
}