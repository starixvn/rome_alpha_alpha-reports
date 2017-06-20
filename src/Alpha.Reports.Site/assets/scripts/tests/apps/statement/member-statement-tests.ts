describe("MemberStatement", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const pagerComponent = new ClientPagerComponent($);
    const reportTypeComponent = new SingleSelectionDropdownComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const topWindow = {
        popupManager: {
            open: jasmine.any(Function)
        }
    };
    const windowLocation = {
        href: ""
    };

    let statementView: MemberStatementView;
    let template = {
        normal: `<body>
                    <div id="icon-excel"></div>
                    <table id="tbl-report"> 
                        <tr class="col-void-amount"></tr>
                        <tr class="col-outstanding"></tr>
                        <tr class="col-win-loss"></tr>
                        <tr class="col-grand-prize"></tr>
                    </table>
                    <div id="report-form"></div>
                    <div class="page-title"></div>
                    <div id="export-excel-form"></div>
                    <input id="StatementDetailsURL" value="StatementDetailsURL" />
                    <input id="CustId" value="1" />
                    <input id="CustName" value="member01" />
                    <input id="BetListUrl" value="BetListUrl" />
                    <input id="BetListTitle" value="BetListTitle" />
                    <div class="icon-filter"></div>
                    <div class="column-transfer-amount" data-transfer-date="4/11/2017"></div>
                    <select id="report-type">
                        <option data-url="NewPage" selected="selected"></option>
                    </select>
                    <div id="view-betlist-element" data-transfer-date="4/11/2017"></div>                   
                 </body>`
    };

    beforeEach(() => {
        statementView = new MemberStatementView(
            jQueryFake,
            dateRangePickerComponent,
            pagerComponent,
            topWindow,
            reportTypeComponent,
            windowLocation,
            linkBuilderComponent);
    });

    it("Inherit StatementView_Always_MemberStatementView is StatementView", () => {
        // assert
        expect(statementView instanceof StatementView).toBeTruthy();
    });
    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(StatementView.prototype, "init");
            spyOn(reportTypeComponent, "render");

            // act
            statementView.init();
        });
        it("_Calls blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Calls super.init", () => {
            // assert
            expect(StatementView.prototype.init).toHaveBeenCalled();
        });
        it("_Calls reportTypeComponent.render", () => {
            // assert
            expect(reportTypeComponent.render).toHaveBeenCalledWith(
                new SingleSelectionDropdownOption("dropdown-report-type", "ReportType", statementView.changeReportType));
        });
        it("_Stores cust Id", () => {
            // assert
            expect(statementView.custId).toBe("1");
        });
        it("_Stores cust name", () => {
            // assert
            expect(statementView.custName).toBe("member01");
        });
        it("_Stores betListUrl", () => {
            // assert
            expect(statementView.betListUrl).toBe("BetListUrl");
        });
        it("_Stores betListTitle", () => {
            // assert
            expect(statementView.betListTitle).toBe("BetListTitle");
        });
    });
    describe("_viewBetList", () => {
        it("_open popup bet list", () => {
            // arrange
            setFixtures(template.normal);
            statementView.init();
            spyOn(topWindow.popupManager, "open");
            let transactionType = 1;

            // act
            statementView.viewBetList($("#view-betlist-element"), transactionType);

            // assert
            expect(topWindow.popupManager.open).toHaveBeenCalledWith(
                "BetListUrl?custid=1&winlossdate=4%2F11%2F2017&transtype=1&custname=member01", "BetListTitle", 900);
        });
    });
    describe("_changeReportType", () => {
        it("_assign location.href url", () => {
            // arrange
            setFixtures(template.normal);
            statementView.init();
            statementView.reportTypeComponent.$dropdown = $("#report-type");

            // act
            statementView.changeReportType();

            // assert
            expect(windowLocation.href).toBe("NewPage");
        });
    });
    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(StatementView.prototype, "registerEvents");
            spyOn(statementView, "viewBetList");
            statementView.cacheElements();

            // act
            statementView.registerEvents();
        });
        it("_Call super.registerEvents", () => {
            // assert
            expect(StatementView.prototype.registerEvents).toHaveBeenCalled();
        });
        it("_Bind view void-amount", () => {
            // act
            statementView.$(".col-void-amount").trigger("click");

            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 2);
        });
        it("_Bind view outstanding", () => {
            // act
            statementView.$(".col-outstanding").trigger("click");

            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 3);
        });
        it("_Bind view win-loss", () => {
            // act
            statementView.$(".col-win-loss").trigger("click");

            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 1);
        });
        it("_Bind view grand-prize", () => {
            // act
            statementView.$(".col-grand-prize").trigger("click");

            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 4);
        });
    });
});
