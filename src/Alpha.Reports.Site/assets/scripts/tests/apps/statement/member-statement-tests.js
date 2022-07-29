describe("MemberStatement", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var pagerComponent = new ClientPagerComponent($);
    var reportTypeComponent = new SingleSelectionDropdownComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var topWindow = {
        popupManager: {
            open: jasmine.any(Function)
        }
    };
    var windowLocation = {
        href: ""
    };
    var statementView;
    var template = {
        normal: "<body>\n                    <div id=\"icon-excel\"></div>\n                    <table id=\"tbl-report\"> \n                        <tr class=\"col-void-amount\"></tr>\n                        <tr class=\"col-outstanding\"></tr>\n                        <tr class=\"col-win-loss\"></tr>\n                        <tr class=\"col-grand-prize\"></tr>\n                    </table>\n                    <div id=\"report-form\"></div>\n                    <div class=\"page-title\"></div>\n                    <div id=\"export-excel-form\"></div>\n                    <input id=\"StatementDetailsURL\" value=\"StatementDetailsURL\" />\n                    <input id=\"CustId\" value=\"1\" />\n                    <input id=\"CustName\" value=\"member01\" />\n                    <input id=\"BetListUrl\" value=\"BetListUrl\" />\n                    <input id=\"BetListTitle\" value=\"BetListTitle\" />\n                    <div class=\"icon-filter\"></div>\n                    <div class=\"column-transfer-amount\" data-transfer-date=\"4/11/2017\"></div>\n                    <select id=\"report-type\">\n                        <option data-url=\"NewPage\" selected=\"selected\"></option>\n                    </select>\n                    <div id=\"view-betlist-element\" data-transfer-date=\"4/11/2017\"></div>                   \n                 </body>"
    };
    beforeEach(function () {
        statementView = new MemberStatementView(jQueryFake, dateRangePickerComponent, pagerComponent, topWindow, reportTypeComponent, windowLocation, linkBuilderComponent);
    });
    it("Inherit StatementView_Always_MemberStatementView is StatementView", function () {
        // assert
        expect(statementView instanceof StatementView).toBeTruthy();
    });
    describe("_init", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(StatementView.prototype, "init");
            spyOn(reportTypeComponent, "render");
            // act
            statementView.init();
        });
        it("_Calls blockUI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Calls super.init", function () {
            // assert
            expect(StatementView.prototype.init).toHaveBeenCalled();
        });
        it("_Calls reportTypeComponent.render", function () {
            // assert
            expect(reportTypeComponent.render).toHaveBeenCalledWith(new SingleSelectionDropdownOption("dropdown-report-type", "ReportType", statementView.changeReportType));
        });
        it("_Stores cust Id", function () {
            // assert
            expect(statementView.custId).toBe("1");
        });
        it("_Stores cust name", function () {
            // assert
            expect(statementView.custName).toBe("member01");
        });
        it("_Stores betListUrl", function () {
            // assert
            expect(statementView.betListUrl).toBe("BetListUrl");
        });
        it("_Stores betListTitle", function () {
            // assert
            expect(statementView.betListTitle).toBe("BetListTitle");
        });
    });
    describe("_viewBetList", function () {
        it("_open popup bet list", function () {
            // arrange
            setFixtures(template.normal);
            statementView.init();
            spyOn(topWindow.popupManager, "open");
            var transactionType = 1;
            // act
            statementView.viewBetList($("#view-betlist-element"), transactionType);
            // assert
            expect(topWindow.popupManager.open).toHaveBeenCalledWith("BetListUrl?custid=1&winlossdate=4%2F11%2F2017&transtype=1&custname=member01", "BetListTitle", 900);
        });
    });
    describe("_changeReportType", function () {
        it("_assign location.href url", function () {
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
    describe("_registerEvents", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            spyOn(StatementView.prototype, "registerEvents");
            spyOn(statementView, "viewBetList");
            statementView.cacheElements();
            // act
            statementView.registerEvents();
        });
        it("_Call super.registerEvents", function () {
            // assert
            expect(StatementView.prototype.registerEvents).toHaveBeenCalled();
        });
        it("_Bind view void-amount", function () {
            // act
            statementView.$(".col-void-amount").trigger("click");
            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 2);
        });
        it("_Bind view outstanding", function () {
            // act
            statementView.$(".col-outstanding").trigger("click");
            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 3);
        });
        it("_Bind view win-loss", function () {
            // act
            statementView.$(".col-win-loss").trigger("click");
            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 1);
        });
        it("_Bind view grand-prize", function () {
            // act
            statementView.$(".col-grand-prize").trigger("click");
            // assert
            expect(statementView.viewBetList).toHaveBeenCalledWith(jasmine.any(Object), 4);
        });
    });
});
//# sourceMappingURL=member-statement-tests.js.map