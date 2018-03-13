const progressiveContributionTemplate = {
    historicReportWithShownFilter: `<body>
                        <form id="report-form">
                            <div id="dropdown-product"></div>
                            <div id="dropdown-month" data-longmonths-inyear=""></div>
                        </form>
                        <div id="tbl-report" data-totalrecords="0"> </div>
                        <input id="IsHistoricReport" value="True" type="hidden" />
                        <input id="IsFilterVisible" value="True" type="hidden" />
                        <div id="report-switch" data-is-historic-report="true"></div>
                        <input id="IsSetDefaultStackMode" value="true" type="hidden" />
                    </body>`,
    historicReportWithHiddenFilter: `<body>
                        <form id="report-form" class="hide">
                            <div id="dropdown-product"></div>
                            <div id="dropdown-month"
                                data-longmonths-inyear="['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov', 'Dec']"
                                data-mindate="01/01/2013"
                                data-dateinmonth="1/1/2017"
                                data-fromdate="1"
                                data-todate="3"
                                data-currentdate="3/1/2017"></div>
                        </form>
                        <div id="tbl-report" data-totalrecords="0"> </div>
                        <input id="IsHistoricReport" value="True" type="hidden" />
                        <input id="IsFilterVisible" value="False" type="hidden" />
                       <input id="IsSetDefaultStackMode" value="true" type="hidden" />
                    </body>`,
    reportWithNormalMode: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report" data-totalrecords="0">
                                        <div class="product-row" data-product-id="1"></div>
                                    </div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="True" type="hidden" />
                                    <input id="IsAgentLevel" value="False" type="hidden">
                                    <input id="IsDescendingSort" value="True" type="hidden" />
                                    <input id="SortPropertyName" value="" type="hidden" />
                                    <input id="IsSetDefaultStackMode" value="true" type="hidden" />
                                    <input id="CustId" value="1" type="hidden" />
                                    <form id="report-form" role="form"></form>
                                    <form id="export-excel-form"></form>
                                    <input id="UserSelectedDateRangeOption" value="" type="hidden" />
                                    <div>
                                        <select id="dropdown-product">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>

                                    <div id="from-date"></div>
                                    <div id="to-date"></div>
                                    <div id="icon-normal-column"></div>
                                    <input id="IsHistoricReport" value="False" type="hidden" />

                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
    reportWithStackMode: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report" data-totalrecords="0"> </div>
                                    <input id="IsFilterVisible" value="False" type="hidden" />
                                    <input id="IsStackMode" value="True" type="hidden" />
                                    <input id="IsAgentLevel" value="True" type="hidden">
                                    <input id="IsDescendingSort" value="True" type="hidden" />
                                    <input id="SortPropertyName" value="" type="hidden" />
                                    <input id="IsSetDefaultStackMode" value="true" type="hidden" />

                                    <form id="report-form"></form>
                                    <form id="export-excel-form"></form>
                                    <input id="UserSelectedDateRangeOption" value="" type="hidden" />
                                    <div>
                                        <div id="dropdown-product"></div>
                                        <button class="multiselect btn"></button>
                                    </div>

                                    <div id="report-switch" data-is-historic-report="false"></div>
                                    <div id="from-date"></div>
                                    <div id="to-date"></div>
                                    <div id="icon-normal-column"></div>
                                    <input id="IsHistoricReport" value="False" type="hidden" />

                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,

    clickProduct: `<body>
                        <table>
                            <tbody>
                                <tr class="product-row" data-product-id="1" data-is-hide="false">
                                    <td class="col-username active"></td>
                                </tr>
                                <tr class="bet-type-row bet-type-of-product-1 bet-type-of-sport-1"></tr>
                                <tr class="product-row" data-product-id="2" data-is-hide="false">
                                    <td class="col-username active"></td>
                                </tr>
                                <tr class="bet-type-row bet-type-of-product-2"></tr>
                                <tr class="product-row" data-product-id="3" data-is-hide="true">
                                    <td class="col-username"></td>
                                </tr>
                                <tr class="bet-type-row bet-type-of-product-4"></tr>
                            </tbody>
                        </table>
                   </body>`
};