'use strict';
var SettlementColumns = /** @class */ (function () {
    function SettlementColumns(visibleColumnDropdown) {
        this.localStorageKey = "wlsettlement_" + top.$('#userId').val();
        this.visibleColumnDropdown = visibleColumnDropdown;
    }
    SettlementColumns.prototype.init = function () {
        var _this = this;
        this.visibleColumnDropdown.render(new VisibleColumnDropdownOption(alphaReports.localStorage.get(this.localStorageKey) || '', columnList, function (columns) {
            _this.showHideColumns(columns);
            var selectedColumnsValue = [];
            Object.entries(columns).forEach(function (_a) {
                var key = _a[0], value = _a[1];
                return value && selectedColumnsValue.push(key);
            });
            alphaReports.localStorage.set(_this.localStorageKey, selectedColumnsValue.join(','));
        }, undefined, 260));
    };
    SettlementColumns.prototype.showHideColumns = function (visibleColumn) {
        if (visibleColumn === void 0) { visibleColumn = undefined; }
        var $tblReport = $('#tbl-report');
        visibleColumn = visibleColumn || this.visibleColumnDropdown.getColumns();
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupTotal'), ['month', 'whitelabel', 'agent', 'master']);
        this.showHideSpecificColumn($tblReport, visibleColumn, 'losscarryforward');
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupMember'), ['memberwinloss', 'memberpromotion']);
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupWhiteLabel'), ['wlnetwinloss', 'wlincome', 'wlfee', 'wlpromotionshare', 'wlpromotionsettlement', 'wltotal']);
        this.showHideSpecificColumn($tblReport, visibleColumn, 'masterearning');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'superearning');
        this.showHideSpecificColumn($tblReport, visibleColumn, 'headlineearning');
        this.showHideGroupColumn($tblReport, visibleColumn, $tblReport.find('#groupCompany'), ['companyincome', 'companypromotionshare', 'companytotal']);
    };
    SettlementColumns.prototype.showHideGroupColumn = function ($tblReport, visibleColumn, $groupColumn, columnNames) {
        var _this = this;
        var colspan = 0;
        columnNames.forEach(function (item) {
            if (_this.showHideSpecificColumn($tblReport, visibleColumn, item)) {
                colspan++;
            }
        });
        if ($groupColumn.length) {
            colspan ? $groupColumn.attr('colspan', colspan).show() : $groupColumn.hide();
        }
    };
    SettlementColumns.prototype.showHideSpecificColumn = function ($tblReport, visibleColumn, key) {
        var visible = visibleColumn[key];
        $tblReport.find("[data-colname=" + key + "]").toggle(visible);
        return visible;
    };
    return SettlementColumns;
}());
//# sourceMappingURL=settlement-columns.js.map