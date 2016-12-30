const winLossDetailTemplate = {
    historicReportHasShownFilter: `<body>
                        <form id="report-form">
                            <div id="dropdown-product"></div>
                            <div id="dropdown-month" data-longmonths-inyear=""></div>
                        </form>
                        <div id="netturnovertooltip"></div>
                        <div id="tbl-report" data-totalrecords="0">
                            <div class="col-netturnover-header"></div>
                        </div>
                        <input id="IsHistoricReport" value="True" type="hidden" />
                        <input id="IsFilterVisible" value="True" type="hidden" />
                        <input id="HasSubmittedParams" value="False" type="hidden" />
                    </body>`,
    historicReportHasHiddenFilter: `<body>
                        <form id="report-form">
                            <div id="dropdown-product"></div>
                            <div id="dropdown-month" data-longmonths-inyear=""></div>
                        </form>
                        <div id="netturnovertooltip"></div>
                        <div id="tbl-report" data-totalrecords="0">
                            <div class="col-netturnover-header"></div>
                        </div>
                        <input id="IsHistoricReport" value="True" type="hidden" />
                        <input id="IsFilterVisible" value="False" type="hidden" />
                        <input id="HasSubmittedParams" value="False" type="hidden" />
                    </body>`,
    currentReport: `<body>
                        <div id="dropdown-product"></div>
                        <div id="netturnovertooltip"></div>
                        <div id="tbl-report" data-totalrecords="0">
                            <div class="col-netturnover-header"></div>
                        </div>
                        <input id="IsHistoricReport" value="False" type="hidden" />
                        <input id="IsFilterVisible" value="True" type="hidden" />
                        <input id="HasSubmittedParams" value="False" type="hidden" />
                    </body>`
};