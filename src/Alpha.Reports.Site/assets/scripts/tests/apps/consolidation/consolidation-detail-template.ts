const consolidationDetailTemplate = {
    normalModeReport: `<body>
                            <table id="tbl-report" data-totalrecords="0" data-sortedcolumn="WinLoss" data-descendingsort="false"></table>
                            <form id="report-form">
                                <div id="dropdown-product"></div>
                                <div class="multiselect btn"></div>
                                <div class="icon-filter"></div>
                                <div class="icon-switchmode"></div>
                                <div id="icon-excel"></div>
                                <div name="report-type" action="report-type-action"></div>
                            </form>
                            <form id="export-excel-form"></form>
                            <div class="page-title"></div>
                            <div class="btn-submit"></div>
                            <input id="IsSetDefaultStackMode" value="" />
                            <input id="IsFilterVisible" value="" />
                            <input id="IsStackMode" value="" />
                        </body>`,
    shownFilterReport: `<body>
                            <table id="tbl-report"></table>
                            <form id="report-form"></form>
                            <form id="export-excel-form"></form>
                            <div class="page-title"></div>
                            <input id="IsSetDefaultStackMode" value="" />
                            <input id="IsFilterVisible" value="" />
                            <input id="IsStackMode" value="" />
                        </body>`,
    hiddenFilterReport: `<body>
                            <table id="tbl-report"></table>
                            <form id="report-form" class="hide"></form>
                            <form id="export-excel-form"></form>
                            <div class="page-title"></div>
                            <input id="IsSetDefaultStackMode" value="" />
                            <input id="IsFilterVisible" value="" />
                            <input id="IsStackMode" value="" />
                        </body>`,
    stackModeReport: `<body>
                            <table id="tbl-report" class="stack-mode" data-totalrecords="0" 
                                data-sortedcolumn="WinLoss" data-descendingsort="false"></table>
                            <form id="report-form"></form>
                            <form id="export-excel-form"></form>
                            <div class="page-title"></div>
                            <input id="IsSetDefaultStackMode" value="" />
                            <input id="IsFilterVisible" value="" />
                            <input id="IsStackMode" value="" />
                        </body>`
};