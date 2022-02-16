describe("Statement", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const pagerComponent = new ClientPagerComponent($);
    const topWindow = {
        popupManager: {
            open: jasmine.any(Function)
        }
    };
    const linkBuilderComponent = new LinkBuilderComponent($);

    let statementView: StatementView;
    let template = {
        normal: `<body>
                    <div id="icon-excel"></div>
                    <table id="tbl-report"></table>
                    <div id="report-form"></div>
                    <div class="page-title"></div>
                    <div id="export-excel-form"></div>
                    <input id="StatementDetailsURL" value="StatementDetailsURL" />
                    <input id="CustId" value="1" />
                    <input id="CustName" value="member01" />
                    <div class="icon-filter"></div>
                    <div class="column-transfer-amount" data-transfer-date="4/11/2017"></div>
                 </body>`,
        shownFilter: `<body>
                        <div id="icon-excel"></div>
                        <table id="tbl-report"></table>
                        <div id="report-form"></div>
                        <div class="page-title"></div>
                        <div id="export-excel-form"></div>
                        <div id="StatementDetailsURL"></div>
                        <div id="CustId"></div>
                        <div id="CustName"></div>
                     </body>`,
        hiddenFilter: `<body>
                            <div id="icon-excel"></div>
                            <table id="tbl-report"></table>
                            <div id="report-form" class="hide"></div>
                            <div class="page-title"></div>
                            <div id="export-excel-form"></div>
                            <div id="StatementDetailsURL"></div>
                            <div id="CustId"></div>
                            <div id="CustName"></div>
                         </body>`,
    };

    beforeEach(() => {
        statementView = new StatementView(jQueryFake, dateRangePickerComponent, pagerComponent, topWindow, linkBuilderComponent);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(statementView, "cacheElements").and.callThrough();
            spyOn(statementView, "registerEvents");
            spyOn(statementView, "initHelp");
            spyOn(jQueryFake.fn, "stickyTableHeaders");
            spyOn(dateRangePickerComponent, "render");
            spyOn(pagerComponent, "render");

            // act
            statementView.init();
        });
        it("_Calls cacheElements", () => {
            // assert
            expect(statementView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls dateRangePickerComponent.render", () => {
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Calls pagerComponent.render with small page size list", () => {
            // assert
            expect(pagerComponent.pageList).toEqual([10, 50, 100, 500]);
            expect(pagerComponent.render).toHaveBeenCalledWith();
        });
        it("_Calls stickyTableHeaders", () => {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
        it("_Calls registerEvents", () => {
            // assert
            expect(statementView.registerEvents).toHaveBeenCalled();
        });
        it("_Calls initHelp", () => {
            // assert
            expect(statementView.initHelp).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", () => {
        it("_Always -> All neccessary elements have been cached", () => {
            // arrange
            setFixtures(template.normal);

            // act
            statementView.cacheElements();

            // assert
            expect(statementView.$tableReport).toBeDefined();
            expect(statementView.$reportForm).toBeDefined();
            expect(statementView.$pageTitle).toBeDefined();
            expect(statementView.$exportExcelForm).toBeDefined();
            expect(statementView.$statementDetailsURL).toBeDefined();
            expect(statementView.custId).toEqual("1");
            expect(statementView.custName).toEqual("member01");
        });
    });
    describe("_onClickIconFilter", () => {
        it("_Filter is currently shown_Hides Filter", () => {
            // arrange
            setFixtures(template.shownFilter);
            statementView.cacheElements();

            // act
            statementView.onClickIconFilter();

            // assert
            expect(statementView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(statementView.$reportForm.hasClass("hide")).toBeTruthy();
        });
        it("_Filter is currently hidden_Hides Filter", () => {
            // arrange
            setFixtures(template.hiddenFilter);
            statementView.cacheElements();

            // act
            statementView.onClickIconFilter();

            // assert
            expect(statementView.$pageTitle.hasClass("hide-filter")).toBeFalsy();
            expect(statementView.$reportForm.hasClass("hide")).toBeFalsy();
        });
    });
    describe("_onExportExcel", () => {
        it("_Bind onExportExcel", () => {
            // arrange
            setFixtures(template.normal);
            statementView.cacheElements();
            spyOn(jQueryFake.fn, "submit");

            // act
            statementView.onExportExcel();

            // assert
            expect(jQueryFake.fn.submit).toHaveBeenCalled();
        });
    });
    describe("_onSubmitReport", () => {
        it("_block UI", () => {
            // arrange
            setFixtures(template.normal);
            statementView.cacheElements();
            spyOn(jQueryFake, "blockUI");

            // act
            statementView.onSubmitReport();

            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
    });
    //describe("_loadStatementDetail", () => {
    //    it("_open popup detail", () => {
    //        // arrange
    //        setFixtures(template.normal);
    //        statementView.cacheElements();
    //        spyOn(topWindow.popupManager, "open");
    //        let transferDate = "04/11/2017";

    //        // act
    //        statementView.loadStatementDetail(transferDate);

    //        // assert
    //        expect(topWindow.popupManager.open).toHaveBeenCalledWith(
    //            "StatementDetailsURL?TransferDate=04%2F11%2F2017&CustId=1&CustName=member01", null, 800);
    //    });
    //});
    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            statementView.cacheElements();
            spyOn(statementView, "onClickIconFilter");
            spyOn(statementView, "onExportExcel");
            spyOn(statementView, "onSubmitReport");
            spyOn(statementView, "loadStatementDetail");

            // act
            statementView.registerEvents();
        });
        it("_Bind onSwitchReport", () => {
            // act
            statementView.$(".icon-filter").trigger("click");

            // assert
            expect(statementView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", () => {
            // act
            statementView.$("#icon-excel").trigger("click");

            // assert
            expect(statementView.onExportExcel).toHaveBeenCalled();
        });
        it("_Bind loadStatementDetail", () => {
            // act
            statementView.$(".column-transfer-amount").trigger("click");

            // assert
            expect(statementView.loadStatementDetail).toHaveBeenCalledWith("4/11/2017");
        });
        it("_Bind onSubmitReport", () => {
            // act
            statementView.$reportForm.submit();

            // assert
            expect(statementView.onSubmitReport).toHaveBeenCalled();
        });
    });
});