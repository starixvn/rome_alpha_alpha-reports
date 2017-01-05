const enteredResultTemplate = {
    reportContent: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report" data-totalrecords="0" data-sortedcolumn="WinLoss">
                                    </div>
                                    <div class="downline-link"></div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
                                    <input id="IsStackMode" value="True" type="hidden" />

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
                                    <input class="breadcrumb-link" />
                                    <input id="IsHistoricReport" value="False" type="hidden" />

                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
};