describe("BaseReport", function () {
    var jQueryFake = $;
    var linkBuilderComponent = new LinkBuilderComponent($);
    var baseReport;
    var template = {
        normal: "<body>\n                    <div id=\"icon-information\"></div>\n                    <input type=\"hidden\" id=\"LoadLegendURL\" value=\"ReportLegend/Index\" />\n                    <div class=\"page-title\"></div>\n                    <form id=\"report-form\"></form>\n                    <div id=\"tbl-report\"></div>\n                    <div class=\"icon-filter\"></div>\n                    <input id=\"IsFilterVisible\" type=\"hidden\" />\n                    <form id=\"export-excel-form\"></form>\n                </body>",
        hiddenFilter: "<body>\n                        <div class=\"page-title\"></div>\n                        <form id=\"report-form\" class=\"hide\"></form>\n                        <div id=\"tbl-report\"></div>\n                        <div class=\"icon-filter\"></div>\n                        <input id=\"IsFilterVisible\" type=\"hidden\" />\n                        <form id=\"export-excel-form\"></form>\n                     </body>"
    };
    beforeEach(function () {
        baseReport = new BaseReport(jQueryFake, linkBuilderComponent);
        spyOn(baseReport, "onClickIconFilter").and.callThrough();
        spyOn($, "ajax");
    });
    describe("_initHelp", function () {
        it("_Always -> Bind Click to show legend event", function () {
            // arrange
            spyOn(baseReport, "showLegend");
            setFixtures(template.normal);
            // act
            baseReport.initHelp();
            baseReport.$iconInformation.trigger("click");
            // assert
            expect(baseReport.showLegend).toHaveBeenCalled();
        });
        it("_Always -> load Legend", function () {
            // arrange
            spyOn(baseReport, "loadLegend");
            setFixtures(template.normal);
            // act
            baseReport.initHelp();
            // assert
            expect(baseReport.loadLegend).toHaveBeenCalled();
        });
    });
    describe("_loadLegend", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            baseReport.initHelp();
            spyOn($, "blockUI");
            // act
            baseReport.loadLegend();
        });
        it("_Always -> call ajax", function () {
            expect($.ajax).toHaveBeenCalledWith({
                method: "GET",
                url: "ReportLegend/Index",
                cache: false,
                data: {},
                success: baseReport.renderLegend,
                complete: $.unblockUI
            });
        });
    });
    describe("_renderLegend", function () {
        describe("_Not Load Legend Before", function () {
            var newLegend = "<div id=\"report-legend\">New Legend</div>";
            beforeEach(function () {
                // arrange
                setFixtures(template.normal);
                baseReport.initHelp();
                spyOn($.fn, "modal");
                spyOn($.fn, "css");
                spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });
                // act
                baseReport.renderLegend(newLegend);
            });
            it("_Append New Legend to Body", function () {
                // assert
                expect($("#report-legend").length).toBe(1);
            });
            it("_Show icon information", function () {
                // assert
                expect(baseReport.$iconInformationContainer.hasClass("hide")).toBeFalsy();
            });
        });
        describe("_Already Load Legend Before", function () {
            var newLegend = "<div id=\"report-legend\">New Legend</div>";
            beforeEach(function () {
                // arrange
                setFixtures(template.normal);
                baseReport.initHelp();
                spyOn($.fn, "modal");
                spyOn($.fn, "css");
                spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });
                baseReport.renderLegend("<div id=\"report-legend\" class=\"\"old-legend\">Old Legend</div>");
                // act
                baseReport.renderLegend(newLegend);
            });
            it("_Remove Old Legend and Append New Legend to Body", function () {
                // assert
                expect($("#report-legend.old-legend").length).toBe(0);
                expect($("#report-legend").length).toBe(1);
            });
            it("_Show icon information", function () {
                // assert
                expect(baseReport.$iconInformationContainer.hasClass("hide")).toBeFalsy();
            });
        });
        describe("_Not Load Legend Before/New Legend is Empty", function () {
            var newLegend = " ";
            beforeEach(function () {
                // arrange
                setFixtures(template.normal);
                baseReport.initHelp();
                spyOn($.fn, "modal");
                spyOn($.fn, "css");
                spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });
                // act
                baseReport.renderLegend(newLegend);
            });
            it("_Remove icon information", function () {
                // assert
                expect($("#icon-information-container").length).toBe(0);
            });
        });
    });
    describe("_showLegend", function () {
        var newLegend = "<div id=\"report-legend\">New Legend</div>";
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            baseReport.initHelp();
            spyOn($.fn, "modal");
            spyOn($.fn, "css");
            spyOn($.fn, "position").and.returnValue({ left: 40, top: 40 });
            baseReport.renderLegend(newLegend);
            // act
            baseReport.showLegend();
        });
        it("_Always_Show Popup", function () {
            // assert
            expect($.fn.modal).toHaveBeenCalledWith({ show: true, backdrop: false });
        });
        it("_Always_Position Legend Popup by icon help", function () {
            // assert
            expect($.fn.css).toHaveBeenCalledWith({
                bottom: "auto",
                top: 100
            });
        });
    });
    describe("_cacheElements", function () {
        it("_Always -> All neccessary elements have been cached", function () {
            // act
            setFixtures(template.normal);
            baseReport.cacheElements();
            // assert
            expect(baseReport.$reportForm).toBeDefined();
            expect(baseReport.$pageTitle).toBeDefined();
            expect(baseReport.$tableReport).toBeDefined();
            expect(baseReport.$isFilterVisible).toBeDefined();
            expect(baseReport.$exportExcelForm).toBeDefined();
        });
    });
    describe("_registerEvents", function () {
        it("_Always_Bind onClickIconFilter", function () {
            // arrange
            setFixtures(template.normal);
            baseReport.cacheElements();
            baseReport.registerEvents();
            // act
            $(".icon-filter").trigger("click");
            // assert
            expect(baseReport.onClickIconFilter).toHaveBeenCalled();
        });
    });
    describe("_onClickIconFilter", function () {
        describe("_Filter is currently shown", function () {
            beforeEach(function () {
                // arrange
                setFixtures(template.normal);
                baseReport.cacheElements();
                spyOn(baseReport, "replaceHrefParam");
                // act
                baseReport.onClickIconFilter();
            });
            it("_Hides Filter", function () {
                // assert
                expect(baseReport.$pageTitle.hasClass("hide-filter")).toBeTruthy();
                expect(baseReport.$reportForm.hasClass("hide")).toBeTruthy();
            });
            it("_Update Href Param to False", function () {
                // assert
                expect(baseReport.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", false);
            });
        });
        describe("_Filter is currently hidden", function () {
            beforeEach(function () {
                // arrange
                setFixtures(template.hiddenFilter);
                baseReport.cacheElements();
                spyOn(baseReport, "replaceHrefParam");
                // act
                baseReport.onClickIconFilter();
            });
            it("_Hides Filter", function () {
                // assert
                expect(baseReport.$pageTitle.hasClass("hide-filter")).toBeFalsy();
                expect(baseReport.$reportForm.hasClass("hide")).toBeFalsy();
            });
            it("_Update Href Param to True", function () {
                // assert
                expect(baseReport.replaceHrefParam).toHaveBeenCalledWith("IsFilterVisible", true);
            });
        });
    });
    it("_replaceHrefParam_Always_Call linkBuilderComponent update param", function () {
        // arrange
        var paramName = "paramName";
        var paramValue = "paramValue";
        spyOn(linkBuilderComponent, "updateParam");
        // act
        baseReport.replaceHrefParam(paramName, paramValue);
        // assert
        expect(linkBuilderComponent.updateParam).toHaveBeenCalledWith("paramName", "paramValue");
    });
});
//# sourceMappingURL=base-report-tests.js.map