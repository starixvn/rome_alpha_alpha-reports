const matchWinLossDetailTemplate = {
    currentReportWithNormalMode: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report">
                                    </div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="False" type="hidden" />

                                    <form id="report-form" role="form"></form>
                                    <form id="export-excel-form"></form>
                                    <input id="UserSelectedDateRangeOption" value="" type="hidden" />
                                    <div>
                                        <select id="dropdown-product">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>
                                    <div id="dropdown-league-container">
                                        <select id="dropdown-league">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                        <div id="league-ids" value=""></div>
                                    </div>

                                    <div id="from-date"></div>
                                    <div id="to-date"></div>
                                    <div id="icon-normal-column"></div>
                                    <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                                    <input id="CustId" value="1" type="hidden" />
                                    <input id="LoadLeagueURL" value="League/LeaguesByCustomer" type="hidden" />
                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
    currentReportWithNormalModeAndHiddenFilter: `<body>
                                    <div class="page-title hide-filter"></div>
                                    <div id="tbl-report">
                                    </div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="False" type="hidden" />

                                    <form id="report-form" role="form" class="hide"></form>
                                    <form id="export-excel-form"></form>
                                    <input id="UserSelectedDateRangeOption" value="" type="hidden" />
                                    <div>
                                        <select id="dropdown-product">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>
                                    <div>
                                        <select id="dropdown-league">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>

                                    <div id="from-date"></div>
                                    <div id="to-date"></div>
                                    <div id="icon-normal-column"></div>
                                    <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                                    <input id="CustId" value="1" type="hidden" />
                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
    currentReportWithStackMode: `<body>
                                    <div class="page-title stack-mode"></div>
                                    <div id="tbl-report" class="stack-mode">
                                    </div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="False" type="hidden" />

                                    <form id="report-form" role="form"></form>
                                    <form id="export-excel-form"></form>
                                    <input id="UserSelectedDateRangeOption" value="" type="hidden" />
                                    <div>
                                        <select id="dropdown-product">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>
                                    <div>
                                        <select id="dropdown-league">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>

                                    <div id="from-date"></div>
                                    <div id="to-date"></div>
                                    <div id="icon-normal-column"></div>
                                    <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                                    <input id="CustId" value="1" type="hidden" />
                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
    currentReportWithNormalModeAndLeagueIsShown: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report">
                                    </div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="False" type="hidden" />

                                    <form id="report-form" role="form"></form>
                                    <form id="export-excel-form"></form>
                                    <input id="UserSelectedDateRangeOption" value="" type="hidden" />
                                    <div>
                                        <select id="dropdown-product">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn"></button>
                                    </div>
                                    <div>
                                        <select id="dropdown-league">
                                            <option value="1" selected="selected"></option>
                                        </select>
                                        <button class="multiselect btn" aria-expanded="true"></button>
                                    </div>

                                    <div id="from-date"></div>
                                    <div id="to-date"></div>
                                    <div id="icon-normal-column"></div>
                                    <input id="IsSetDefaultStackMode" value="True" type="hidden" />
                                    <input id="CustId" value="1" type="hidden" />
                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`
};