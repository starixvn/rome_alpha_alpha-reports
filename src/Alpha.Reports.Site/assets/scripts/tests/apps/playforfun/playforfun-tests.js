describe("PlayForFun", function () {
    var jQueryFake = $;
    var dateRangePickerComponent = new DateRangePickerComponent($);
    var linkBuilderComponent = new LinkBuilderComponent($);
    var sortComponent = new SortComponent($, linkBuilderComponent);
    var statusSelectionComponent = new SingleSelectionDropdownComponent($);
    var productSelectionComponent = new SingleSelectionDropdownComponent($);
    var playForFunView;
    var template = {
        report: "<body>\n                    <div class=\"page-title\"></div>\n                    <form id=\"report-form\"></form>\n                    <div id=\"tbl-report\" data-totalrecords=\"0\"\n                        data-descendingsort=\"False\"\n                        data-sortedcolumn=\"Turnover\"></div>\n                    <div class=\"icon-filter\"></div>\n                    <div id=\"icon-excel\"></div>\n                    <input id=\"IsFilterVisible\" type=\"hidden\" />\n                    <input id=\"UserSelectedProductId\" type=\"hidden\" />\n                    <input id=\"UserSelectedStatusId\" type=\"hidden\" />\n                    <input id=\"IsDescendingSort\" type=\"hidden\" value=\"False\"/>\n                    <input id=\"SortPropertyName\" type=\"hidden\" value=\"Turnover\" />\n                    <form id=\"export-excel-form\"></form>\n                 </body>",
        hiddenFilter: "<body>\n                    <div class=\"page-title\"></div>\n                        <form id=\"report-form\" class=\"hide\"></form>\n                        <div id=\"tbl-report\" data-totalrecords=\"0\"\n                            data-descendingsort=\"False\"\n                            data-sortedcolumn=\"Turnover\"></div>\n                        <div class=\"icon-filter\"></div>\n                        <div id=\"icon-excel\"></div>\n                        <input id=\"IsFilterVisible\" type=\"hidden\" />\n                        <input id=\"UserSelectedProductId\" type=\"hidden\" />\n                        <input id=\"UserSelectedStatusId\" type=\"hidden\" />\n                        <input id=\"IsDescendingSort\" type=\"hidden\" value=\"False\"/>\n                        <input id=\"SortPropertyName\" type=\"hidden\" value=\"Turnover\" />\n                        <form id=\"export-excel-form\"></form>\n                     </body>"
    };
    beforeEach(function () {
        playForFunView = new PlayForFunView(jQueryFake, dateRangePickerComponent, sortComponent, productSelectionComponent, statusSelectionComponent, linkBuilderComponent);
    });
    it("Inherit BaseReport_Always_PlayForFunView is BaseReport", function () {
        // assert
        expect(playForFunView instanceof BaseReport).toBeTruthy();
    });
    describe("_init_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.report);
            spyOn(jQueryFake, "blockUI");
            spyOn(jQueryFake, "unblockUI");
            spyOn(playForFunView, "cacheElements").and.callThrough();
            spyOn(playForFunView, "initTableReport").and.callThrough();
            spyOn(playForFunView, "registerEvents").and.callThrough();
            spyOn(productSelectionComponent, "render");
            spyOn(statusSelectionComponent, "render");
            spyOn(dateRangePickerComponent, "render");
            spyOn(linkBuilderComponent, "render");
            // act
            playForFunView.init();
        });
        it("_Calls cacheElements", function () {
            // assert
            expect(playForFunView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls blockUI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Renders Dropdown Products", function () {
            // assert
            expect(productSelectionComponent.render)
                .toHaveBeenCalledWith(new SingleSelectionDropdownOption("dropdown-product", "UserSelectedProductId"));
        });
        it("_Renders Dropdown Statuses", function () {
            // assert
            expect(statusSelectionComponent.render)
                .toHaveBeenCalledWith(new SingleSelectionDropdownOption("dropdown-statuses", "UserSelectedStatusId"));
        });
        it("_Renders Date Range Picker", function () {
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Init table Report", function () {
            // assert
            expect(playForFunView.initTableReport).toHaveBeenCalled();
        });
        ;
        it("_Calls linkBuilderComponent.render", function () {
            // assert
            expect(linkBuilderComponent.render).toHaveBeenCalled();
        });
        it("_Calls registerEvents", function () {
            // assert
            expect(playForFunView.registerEvents).toHaveBeenCalled();
        });
    });
    describe("_cacheElements_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.report);
            spyOn(PlayForFunView.prototype, "cacheElements").and.callThrough();
            // act
            playForFunView.cacheElements();
        });
        it("_Calls super.cacheElements", function () {
            expect(PlayForFunView.prototype.cacheElements).toHaveBeenCalled();
        });
        it("_All neccessary elements have been cached", function () {
            // assert
            expect(playForFunView.$isDescendingSort).toBeDefined();
            expect(playForFunView.$sortPropertyName).toBeDefined();
        });
    });
    describe("_getSorterHeader", function () {
        it("_Always_Returns Correct Sorter Header", function () {
            // arrange
            var expectedSorterHeader = { 0: { sorter: "text" } };
            // act
            var sorterHeader = playForFunView.getSorterHeader();
            // assert
            expect(sorterHeader).toEqual(expectedSorterHeader);
        });
    });
    describe("_initTableReport_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.report);
            playForFunView.cacheElements();
            spyOn(jQueryFake.fn, "stickyTableHeaders");
            spyOn(sortComponent, "render");
            // act
            playForFunView.initTableReport();
        });
        it("_Bind stickyTableHeaders", function () {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });
        it("_Renders sorter", function () {
            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(playForFunView.$tableReport, false, true, sortComponent.afterSort, { 0: { sorter: "text" } }, playForFunView.$reportForm));
        });
    });
    describe("_onSubmitReport", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.report);
            playForFunView.init();
            spyOn(jQueryFake, "blockUI");
            // act
            playForFunView.onSubmitReport();
        });
        it("_Always_Block UI", function () {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Screen smaller than 768_Sets Correct new values to hidden fields", function () {
            // arrange
            spyOn(alphaReports, "screenSmallThan768").and.returnValue(true);
            spyOn(productSelectionComponent, "render").and.callThrough();
            spyOn(statusSelectionComponent, "render").and.callThrough();
            // assert
            expect(playForFunView.$isFilterVisible.val()).toBe("true");
            expect(playForFunView.$sortPropertyName.val()).toBe("Turnover");
            expect(productSelectionComponent.$hiddenInput.val()).toBe("");
            expect(statusSelectionComponent.$hiddenInput.val()).toBe("");
            expect(playForFunView.$isDescendingSort.val()).toBe("false");
        });
        it("_Screen not smaller than 768_Sets Correct new values to hidden fields", function () {
            // arrange
            spyOn(alphaReports, "screenSmallThan768").and.returnValue(false);
            spyOn(productSelectionComponent, "render").and.callThrough();
            spyOn(statusSelectionComponent, "render").and.callThrough();
            // assert
            expect(playForFunView.$isFilterVisible.val()).toBe("true");
            expect(playForFunView.$sortPropertyName.val()).toBe("Turnover");
            expect(productSelectionComponent.$hiddenInput.val()).toBe("");
            expect(statusSelectionComponent.$hiddenInput.val()).toBe("");
            expect(playForFunView.$isDescendingSort.val()).toBe("false");
        });
    });
    describe("_getParams", function () {
        it("_Shown Filter_Returns Correct Params", function () {
            // arrange
            setFixtures(template.report);
            playForFunView.init();
            var expectedParams = {
                IsFilterVisible: true,
                SortPropertyName: "Turnover",
                IsDescendingSort: false
            };
            // act
            var actualParams = playForFunView.getParams();
            // assert
            expect(expectedParams).toEqual(actualParams);
        });
        it("_Hidden Filter_Returns Correct Params", function () {
            // arrange
            setFixtures(template.hiddenFilter);
            playForFunView.init();
            var expectedParams = {
                IsFilterVisible: false,
                SortPropertyName: "Turnover",
                IsDescendingSort: false
            };
            // act
            var actualParams = playForFunView.getParams();
            // assert
            expect(expectedParams).toEqual(actualParams);
        });
    });
    describe("_onExportExcel_Always", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.report);
            playForFunView.cacheElements();
            spyOn(playForFunView.$exportExcelForm, "submit");
            spyOn(jQueryFake, "unblockUI");
            // act
            playForFunView.onExportExcel();
        });
        it("_Submits Export Excel Form", function () {
            // assert
            expect(playForFunView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", function () {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=playforfun-tests.js.map