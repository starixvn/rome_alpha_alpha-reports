describe("DateRangePicker", () => {
    let dateRangePickerComponent: DateRangePickerComponent;

    beforeEach(() => {
        dateRangePickerComponent = new DateRangePickerComponent($);
    });

    describe("_render", () => {
        describe("_Always", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<div id="daterange-picker" 
                                data-currentdate="01/05/2016"
                                data-mindate="01/05/2013" 
                                data-maxdate=""
                                data-selecteddaterange="0">
                         </div>`);
                spyOn($.fn, "nexDatePicker");
                spyOn($.fn, "multiselect");

                // act
                dateRangePickerComponent.render();
            });

            it("_Binds nexDatePicker to date range element", () => {
                // assert
                expect($.fn.nexDatePicker).toHaveBeenCalledWith({
                    datePickerFormat: {
                        dateFormat: "MM/DD/YYYY"
                    },
                    selectedItem: 0,
                    minDate: new Date("01/05/2013"),
                    maxDate: null,
                    currentDate: new Date("01/05/2016")
                });
            });
            it("_Binds quick choose option", () => {
                // assert
                expect($.fn.multiselect).toHaveBeenCalledWith({
                    maxHeight: 300
                });
            });
        });
    });
});