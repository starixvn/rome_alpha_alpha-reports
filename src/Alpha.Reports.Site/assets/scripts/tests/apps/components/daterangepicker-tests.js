var _this = this;
describe("DateRangePicker", function () {
    var dateRangePickerComponent;
    var template = {
        dateRange: "<body>\n                            <div id=\"daterange-picker\"\n                                    data-currentdate=\"01/05/2016\"\n                                    data-mindate=\"01/05/2013\"\n                                    data-maxdate=\"\"\n                                    data-selecteddaterange=\"0\">\n                                 <div class=\"ui_nexdatepicker_edit_field daterange-container\">\n                                    <select id=\"dropdown-daterange\"\n                                            name=\"UserSelectedDateRangeOption\"\n                                            class=\"dropdown-daterange\">\n                                            <option value=\"0\" selected=\"selected\">Today</option>\n                                            <option value=\"1\">Yesterday</option>\n                                            <option value=\"2\">This week</option>\n                                            <option value=\"3\">Last Week</option>\n                                            <option value=\"4\">This Month</option>\n                                            <option value=\"5\">Last Month</option>\n                                            <option value=\"6\">Since Last Month</option>\n                                            <option value=\"7\">Yesterday</option>\n                                    </select>\n                                </div>\n                                 <div id=\"from-date-container\"\n                                        class=\"input-group date ui_nexdatepicker_edit_field css fromdate\">\n                                    <input type=\"text\"\n                                            id=\"from-date\"\n                                            name=\"FromDate\"\n                                            readonly=\"readonly\"\n                                            class=\"ui_nexdatepicker_datetextbox ui_nexdatepicker_fromdate form-control\"\n                                            value=\"01/17/2017\" />\n                                    <span class=\"input-group-addon\">\n                                        <span class=\"icon-calendar\"></span>\n                                    </span>\n                                </div>\n                                <div class=\"ui_nexdatepicker_lbl_field separator\">-</div>\n                                <div id=\"to-date-container\"\n                                        class=\"input-group date ui_nexdatepicker_edit_field  todate\">\n                                    <input type=\"text\"\n                                            id=\"to-date\"\n                                            name=\"ToDate\"\n                                            readonly=\"readonly\"\n                                            class=\"ui_nexdatepicker_datetextbox ui_nexdatepicker_todate form-control\"\n                                            value=\"01/17/2017\" />\n                                    <span class=\"input-group-addon\">\n                                        <span class=\"icon-calendar\"></span>\n                                    </span>\n                                </div>\n                             </div>\n                            <form id=\"report-form\"></form>\n                        </body>"
    };
    beforeEach(function () {
        dateRangePickerComponent = new DateRangePickerComponent($);
    });
    describe("_render", function () {
        describe("_Always", function () {
            beforeEach(function () {
                // arrange
                setFixtures("<div id=\"daterange-picker\"\n                                data-currentdate=\"01/05/2016\"\n                                data-mindate=\"01/05/2013\"\n                                data-maxdate=\"\"\n                                data-selecteddaterange=\"0\">\n                         </div>");
                spyOn($.fn, "nexDatePicker");
                spyOn($.fn, "multiselect");
                // act
                dateRangePickerComponent.render();
            });
            it("_Binds nexDatePicker to date range element", function () {
                // assert
                expect($.fn.nexDatePicker).toHaveBeenCalledWith({
                    datePickerFormat: {
                        dateFormat: "MM/DD/YYYY"
                    },
                    selectedItem: 0,
                    minDate: new Date("01/05/2013"),
                    maxDate: null,
                    currentDate: new Date("01/05/2016"),
                    onSelectDatePicker: jasmine.any(Function),
                    onQuickChooseChange: jasmine.any(Function)
                });
            });
        });
    });
    describe("_onSelectDatePicker", function () {
        it("_Always_Calls Change Event", function () {
            // arrange
            setFixtures(template.dateRange);
            var onChange = {
                event: new Function()
            };
            spyOn(onChange, "event");
            dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", onChange.event));
            // act
            _this.$("#dropdown-daterange").val("7").change();
            // assert
            expect(onChange.event).toHaveBeenCalled();
        });
    });
    describe("_onQuickChooseChange", function () {
        var $reportForm;
        beforeEach(function () {
            // arrange
            setFixtures(template.dateRange);
            dateRangePickerComponent.render();
            $reportForm = dateRangePickerComponent.$reportForm;
            spyOn($reportForm, "submit");
        });
        it("_NotCompleteRender;NotChooseSpecificDates => Not Call Submit", function () {
            // arrange
            dateRangePickerComponent.isCompleteInitDateRangePicker = false;
            // act
            _this.$("#dropdown-daterange").val("1").change();
            // assert
            expect($reportForm.submit).not.toHaveBeenCalled();
        });
        it("_CompleteRender;NotChooseSpecificDates => Call Submit", function () {
            // act
            _this.$("#dropdown-daterange").val("1").change();
            // assert
            expect($reportForm.submit).toHaveBeenCalled();
        });
        it("_CompleteRender_ChooseSpecificDates => Not Call Submit", function () {
            // act
            _this.$("#dropdown-daterange").val("7").change();
            // assert
            expect($reportForm.submit).not.toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=daterangepicker-tests.js.map