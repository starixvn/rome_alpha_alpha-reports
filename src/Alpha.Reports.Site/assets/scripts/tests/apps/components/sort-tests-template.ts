const sortTemplate = {
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