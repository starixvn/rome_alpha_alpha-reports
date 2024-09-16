"use strict";
declare var Clipboard: any;

class WinLossDetailColumns {
    private visibleColumnDropdownComponent: VisibleColumnDropdownComponent;
    private columnList: any;

    constructor(
        visibleColumnDropdownComponent: VisibleColumnDropdownComponent,
        columnList: any) {
        this.visibleColumnDropdownComponent = visibleColumnDropdownComponent;
        this.columnList = columnList;
    }

    init(visibleColumns, onDropdownHidden) {
        this.visibleColumnDropdownComponent.render(new VisibleColumnDropdownOption(
            visibleColumns,
            this.columnList,
            (columns) => {
                this.showHideColumns(columns);
                const selectedColumnsValue = [];
                Object.entries(columns).forEach(([key, value]) => value && selectedColumnsValue.push(key));
                onDropdownHidden(selectedColumnsValue.join());
            }));
    }

    showHideColumns(visibleColumn = undefined) {
        const tableReport = $("#tbl-report");
        visibleColumn = visibleColumn || this.visibleColumnDropdownComponent.getColumns();
        this._showHideSpecificColumn(tableReport, visibleColumn, "fullname");
        this._showHideSpecificColumn(tableReport, visibleColumn, "betcount");
        this._showHideSpecificColumn(tableReport, visibleColumn, "turnover");
        this._showHideSpecificColumn(tableReport, visibleColumn, "netturnover");
        this._showHideSpecificColumn(tableReport, visibleColumn, "grosscomm");
        this._showHideGroupColumn(tableReport, visibleColumn, "p", "groupColumnMember");
        this._showHideGroupColumn(tableReport, visibleColumn, "a", "groupColumnAgent");
        this._showHideGroupColumn(tableReport, visibleColumn, "m", "groupColumnMaster");
        this._showHideGroupColumn(tableReport, visibleColumn, "s", "groupColumnSuper");
        this._showHideSpecificColumn(tableReport, visibleColumn, "company");
        this._showHideSpecificColumn(tableReport, visibleColumn, "reward");
    }

    private _showHideGroupColumn(tableReport: JQuery, visibleColumn, prefix: string, groupColumnId: string) {
        const groupColumn = tableReport.find("#" + groupColumnId);
        if (groupColumn.length) {
            let colspan = 0;
            if (this._showHideSpecificColumn(tableReport, visibleColumn, prefix + "winloss")) {
                colspan++;
            }
            if (this._showHideSpecificColumn(tableReport, visibleColumn, prefix + "comm")) {
                colspan++;
            }
            if (this._showHideSpecificColumn(tableReport, visibleColumn, prefix + "total")) {
                colspan++;
            }

            colspan ? groupColumn.attr("colspan", colspan).removeClass("hide") : groupColumn.addClass("hide");
        }
    }

    private _showHideSpecificColumn(tableReport: JQuery, visibleColumn, key: string) {
        const matchedColumns = tableReport.find(`[data-colname=${key}]`);
        const visible = visibleColumn[key];
        visible ? matchedColumns.removeClass("hide") : matchedColumns.addClass("hide");

        return visible;
    }

    getShowColumns() {
        return this.visibleColumnDropdownComponent.getShowColumns();
    }

    getHideColumns() {
        return this.visibleColumnDropdownComponent.getHideColumns();
    }
}