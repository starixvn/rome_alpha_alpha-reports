describe("DateRangePicker_", () => {
    let dateRangePickerComponent: DateRangePickerComponent;

    beforeEach(() => {
        dateRangePickerComponent = new DateRangePickerComponent($);
    });

    describe("render", () => {
        it("Always -> Binds nexDatePicker to date range element; Binds quick choose option;", () => {
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

            expect($.fn.multiselect).toHaveBeenCalledWith({
                maxHeight: 300
            });
        });
    });
});