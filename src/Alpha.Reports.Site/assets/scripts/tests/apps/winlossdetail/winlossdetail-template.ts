const winLossDetailTemplate = {
    historicReportWithShownFilter: `<body>
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
    historicReportWithHiddenFilter: `<body>
                        <form id="report-form">
                            <div id="dropdown-product"></div>
                            <div id="dropdown-month" 
                                data-longmonths-inyear="['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov', 'Dec']"
                                data-mindate="01/01/2013"
                                data-dateinmonth="1/1/2017"
                                data-fromdate="1"
                                data-todate="3"
                                data-currentdate="3/1/2017"></div>
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