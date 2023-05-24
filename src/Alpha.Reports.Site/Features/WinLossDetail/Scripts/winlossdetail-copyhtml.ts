"use strict";
declare var Clipboard: any;

class WinLossDetailCopyHtml {
    private clipboard: any;

    constructor(clipboard: any) {
        this.clipboard = clipboard;
    }

    init() {
        $("#icon-copy").tooltip();
        const clipboard = new this.clipboard("#icon-copy", {
            text: (trigger) => this._copyTableHtml($("#tbl-report"))
        });

        if (window.document.queryCommandSupported && window.document.queryCommandSupported("copy")) {
            clipboard.on("success", function (e) {
                const $this = $(e.trigger);
                $this.attr("data-original-title", $("#hiddenCopiedText").val());
                $this.tooltip("show");
                setTimeout(() => {
                    $this.tooltip("hide");
                    $this.attr("data-original-title", $("#hiddenCopyText").val());
                }, 3000);
            }).on("error", function (e) {
                $(e.trigger).remove();
                console.error("Action:", e.action);
                console.error("Trigger:", e.trigger);
            });
        }
    }

    _copyTableHtml(tableReport: JQuery) {
        const html = ['<table>'];
        html.push(this._copyTableHeader(tableReport));
        html.push(this._copyTableBody(tableReport));
        html.push(this._copyTableFooter(tableReport));
        html.push('</table>');

        return html.join("");
    }

    _copyTableHeader(tableReport: JQuery) {
        const html = ['<thead><tr>'];
        tableReport.find("thead tr:first th:not(.hide)").each(function () {
            const $this = $(this);
            html.push('<th');

            const rowspan = $this.attr("rowspan");
            if (rowspan) {
                html.push(' rowspan="', rowspan, '"');
            }

            const colspan = $this.attr("colspan");
            if (colspan) {
                html.push(' colspan="', colspan, '"');
            }

            html.push('>', this.innerText, '</th>');
        });

        html.push('</tr><tr>');
        tableReport.find("thead tr:last th:not(.hide)").each(function () {
            html.push('<th>', this.innerText, '</th>');
        });

        html.push('</tr></thead>');
        return html.join("");
    }

    _copyTableBody(tableReport: JQuery) {
        const getColumnText = function (td) {
            let title;
            const $td = $(td);
            if ($td.data("colname") === "fullname") {
                title = $td.find("div:first").data("original-title");
            }

            return title || td.innerText;
        };
        const html = ['<tbody>'];

        tableReport.find("tbody tr").each(function () {
            html.push('<tr>');
            $(this).find("td:not(.hide)").each(function () {
                html.push('<td>', getColumnText(this), '</td>');
            });
            html.push('</tr>');
        });

        html.push('</tbody>');
        return html.join("");
    }

    _copyTableFooter(tableReport: JQuery) {
        const html = ['<tfoot><tr>'];
        tableReport.find("tfoot tr td:not(.hide)").each(function () {
            html.push('<td>', this.innerText, '</td>');
        });

        html.push('</tr></tfoot>');
        return html.join("");
    }
}