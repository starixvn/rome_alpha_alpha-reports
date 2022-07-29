describe("Statement", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var pagerComponent = new ClientPagerComponent($);
    var topWindow = {
        popupManager: {
            open: jasmine.any(Function)
        }
    };
    var linkBuilderComponent = new LinkBuilderComponent($);
    var statementView;
    var template = {
        normal: "<body>\n                    <div id=\"icon-excel\"></div>\n                    <table id=\"tbl-report\"></table>\n                    <div id=\"report-form\"></div>\n                    <div class=\"page-title\"></div>\n                    <div id=\"export-excel-form\"></div>\n                    <input id=\"StatementDetailsURL\" value=\"StatementDetailsURL\" />\n                    <input id=\"CustId\" value=\"1\" />\n                    <input id=\"CustName\" value=\"member01\" />\n                    <div class=\"icon-filter\"></div>\n                    <div class=\"column-transfer-amount\" data-transfer-date=\"4/11/2017\"></div>\n                 </body>",
        shownFilter: "<body>\n                        <div id=\"icon-excel\"></div>\n                        <table id=\"tbl-report\"></table>\n                        <div id=\"report-form\"></div>\n                        <div class=\"page-title\"></div>\n                        <div id=\"export-excel-form\"></div>\n                        <div id=\"StatementDetailsURL\"></div>\n                        <div id=\"CustId\"></div>\n                        <div id=\"CustName\"></div>\n                     </body>",
        hiddenFilter: "<body>\n                            <div id=\"icon-excel\"></div>\n                            <table id=\"tbl-report\"></table>\n                            <div id=\"report-form\" class=\"hide\"></div>\n                            <div class=\"page-title\"></div>\n                            <div id=\"export-excel-form\"></div>\n                            <div id=\"StatementDetailsURL\"></div>\n                            <div id=\"CustId\"></div>\n                            <div id=\"CustName\"></div>\n                         </body>",
    };
    beforeEach(function () {
        statementView = new StatementView(jQueryFake, dateRangePickerComponent, pagerComponent, topWindow, linkBuilderComponent);
    });
    describe("_init", function () {
        beforeEach(function () {
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
        it("_Calls cacheElements", function () {
            // assert
            expect(statementView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls dateRangePickerComponent.render", function () {
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Calls pagerComponent.render with small page size list", function () {
            // assert
            expect(pagerComponent.pageList).toEqual([10, 50, 100, 500]);
            expect(pagerComponent.render).toHaveBeenCalledWith();
        });
        it("_Calls stickyTableHeaders", function () {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
        it("_Calls registerEvents", function () {
            // assert
            expect(statementView.registerEvents).toHaveBeenCalled();
        });
        it("_Calls initHelp", function () {
            // assert
            expect(statementView.initHelp).toHaveBeenCalled();
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
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
    describe("_onClickIconFilter", function () {
        it("_Filter is currently shown_Hides Filter", function () {
            // arrange
            setFixtures(template.shownFilter);
            statementView.cacheElements();
            // act
            statementView.onClickIconFilter();
            // assert
            expect(statementView.$pageTitle.hasClass("hide-filter")).toBeTruthy();
            expect(statementView.$reportForm.hasClass("hide")).toBeTruthy();
        });
        it("_Filter is currently hidden_Hides Filter", function () {
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
    describe("_onExportExcel", function () {
        it("_Bind onExportExcel", function () {
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
    describe("_onSubmitReport", function () {
        it("_block UI", function () {
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
    describe("_registerEvents", function () {
        beforeEach(function () {
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
        it("_Bind onSwitchReport", function () {
            // act
            statementView.$(".icon-filter").trigger("click");
            // assert
            expect(statementView.onClickIconFilter).toHaveBeenCalled();
        });
        it("_Bind onExportExcel", function () {
            // act
            statementView.$("#icon-excel").trigger("click");
            // assert
            expect(statementView.onExportExcel).toHaveBeenCalled();
        });
        it("_Bind loadStatementDetail", function () {
            // act
            statementView.$(".column-transfer-amount").trigger("click");
            // assert
            expect(statementView.loadStatementDetail).toHaveBeenCalledWith("4/11/2017");
        });
        it("_Bind onSubmitReport", function () {
            // act
            statementView.$reportForm.submit();
            // assert
            expect(statementView.onSubmitReport).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=statement-tests.js.map