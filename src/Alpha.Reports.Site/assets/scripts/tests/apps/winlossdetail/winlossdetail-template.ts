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
                        <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                        <div id="report-switch" data-is-historic-report="true"></div>
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
                        <div id="netturnovertooltip"></div>
                        <div id="tbl-report" data-totalrecords="0">
                            <div class="col-netturnover-header"></div>
                        </div>
                        <input id="IsHistoricReport" value="True" type="hidden" />
                        <input id="IsFilterVisible" value="False" type="hidden" />
                        <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                    </body>`,
    currentReportWithNormalMode: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report" data-totalrecords="0" data-sortedcolumn="WinLoss" data-descendingsort="false">
                                        <div class="col-netturnover-header"></div>
                                    </div>
                                    <div class="downline-link"></div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="True" type="hidden" />
                                    <input id="IsDescendingSort" value="True" type="hidden" />
                                    <input id="SortPropertyName" value="" type="hidden" />

                                    <form id="report-form" role="form"></form>
                                    <form id="export-excel-form"></form>
                                    <input id="UserSelectedDateRangeOption" value="" type="hidden" />
                                    <div>
                                        <select id="dropdown-product">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>

                                    <div id="report-switch" data-is-historic-report="false"></div>
                                    <div id="from-date"></div>
                                    <div id="to-date"></div>
                                    <div id="icon-normal-column"></div>
                                    <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                                    <input class="breadcrumb-link" />

                                    <div id="netturnovertooltip"></div>
                                    <input id="IsHistoricReport" value="False" type="hidden" />

                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
    currentReportWithStackMode: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report" data-totalrecords="0" class="stack-mode">
                                        <div class="col-netturnover-header"></div>
                                    </div>
                                    <div class="downline-link"></div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="True" type="hidden" />
                                    <input id="IsDescendingSort" value="True" type="hidden" />
                                    <input id="SortPropertyName" value="" type="hidden" />

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
                                    <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                                    <input class="breadcrumb-link" />

                                    <div id="netturnovertooltip"></div>
                                    <input id="IsHistoricReport" value="False" type="hidden" />

                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
    reportOfTopLevel: `<body>
                            <div id="dropdown-product"></div>
                            <div id="netturnovertooltip"></div>
                            <div id="tbl-report" data-totalrecords="0">
                                <div class="col-netturnover-header"></div>
                            </div>
                            <input id="IsHistoricReport" value="False" type="hidden" />
                            <input id="IsFilterVisible" value="True" type="hidden" />
                            <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                            <input id="CustId" value="1" type="hidden" />
                        </body>`,

    sortASCOnNormalMode: `<body>
                            <div class="page-title"></div>
                            <table id="tbl-report" data-totalrecords="0">
                                <thead>
                                    <th class="col-netturnover-header"></th>
                                    <th class="sorting-asc" data-sortedname="WinLoss"></th>
                                </thead>
                                <tbody>
                                    <tr class="even-row"><td>Row 1</td></tr>
                                    <tr><td>Row 2</td></tr>
                                    <tr><td>Row 3</td></tr>
                                    <tr class="even-row"><td>Row 4</td></tr>
                                </tbody>
                            </table>
                            <div class="downline-link"></div>
                            <input id="IsFilterVisible" value="True" type="hidden" />
                            <input id="IsStackMode" value="True" type="hidden" />
                            <input id="IsDescendingSort" value="True" type="hidden" />
                            <input id="SortPropertyName" value="" type="hidden" />

                            <form id="report-form" role="form"></form>
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
                            <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                            <input class="breadcrumb-link" />

                            <div id="netturnovertooltip"></div>
                            <input id="IsHistoricReport" value="False" type="hidden" />

                            <div class="icon-filter"></div>
                            <div class="icon-switchmode"></div>
                            <div id="icon-excel"></div>
                        </body>`,
    sortDESCOnNormalMode: `<body>
                            <div class="page-title"></div>
                            <div id="tbl-report" data-totalrecords="0">
                                <div class="col-netturnover-header"></div>
                                <div class="sorting-desc" data-sortedname="WinLoss"></div>
                            </div>
                            <div class="downline-link"></div>
                            <input id="IsFilterVisible" value="True" type="hidden" />
                            <input id="IsStackMode" value="True" type="hidden" />
                            <input id="IsDescendingSort" value="True" type="hidden" />
                            <input id="SortPropertyName" value="" type="hidden" />

                            <form id="report-form" role="form"></form>
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
                            <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                            <input class="breadcrumb-link" />

                            <div id="netturnovertooltip"></div>
                            <input id="IsHistoricReport" value="False" type="hidden" />

                            <div class="icon-filter"></div>
                            <div class="icon-switchmode"></div>
                            <div id="icon-excel"></div>
                        </body>`,
    sortDESCCombinedColumnOnStackMode: `<body>
                            <div class="page-title"></div>
                            <div id="tbl-report" data-totalrecords="0">
                                <div class="col-netturnover-header"></div>
                                <div class="sorting-desc" data-sortedname="WinLoss"></div>
                            </div>
                            <div class="downline-link"></div>
                            <input id="IsFilterVisible" value="True" type="hidden" />
                            <input id="IsStackMode" value="True" type="hidden" />
                            <input id="IsDescendingSort" value="True" type="hidden" />
                            <input id="SortPropertyName" value="" type="hidden" />

                            <form id="report-form" role="form"></form>
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
                            <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                            <input class="breadcrumb-link" />

                            <div id="netturnovertooltip"></div>
                            <input id="IsHistoricReport" value="False" type="hidden" />

                            <div class="icon-filter"></div>
                            <div class="icon-switchmode"></div>
                            <div id="icon-excel"></div>
                        </body>`
};