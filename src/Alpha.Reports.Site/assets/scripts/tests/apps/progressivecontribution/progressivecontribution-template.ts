const progressiveContributionTemplate = {
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

                                    <div class="icon-filter"></div>
                                    <div class="icon-switchmode"></div>
                                    <div id="icon-excel"></div>
                                </body>`,
    reportWithStackMode: `<body>
                                    <div class="page-title"></div>
                                    <div id="tbl-report" data-totalrecords="0"> </div>
                                    <input id="IsFilterVisible" value="True" type="hidden" />
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