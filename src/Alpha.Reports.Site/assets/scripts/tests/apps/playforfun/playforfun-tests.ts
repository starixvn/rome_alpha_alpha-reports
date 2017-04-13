describe("PlayForFun", () => {
    const jQueryFake = $;
    const dateRangePickerComponent = new DateRangePickerComponent($);
    const linkBuilderComponent = new LinkBuilderComponent($);
    const sortComponent = new SortComponent($, linkBuilderComponent);
    const statusSelectionComponent = new SingleSelectionDropdownComponent($);
    const productSelectionComponent = new SingleSelectionDropdownComponent($);
    let playForFunView: PlayForFunView;
    let template = {
        report: `<body>
                    <div class="page-title"></div>
                    <form id="report-form"></form>
                    <div id="tbl-report" data-totalrecords="0"
                        data-descendingsort="False"
                        data-sortedcolumn="Turnover"></div>
                    <div class="icon-filter"></div>
                    <div id="icon-excel"></div>
                    <input id="IsFilterVisible" type="hidden" />
                    <input id="UserSelectedProductId" type="hidden" />
                    <input id="UserSelectedStatusId" type="hidden" />
                    <input id="IsDescendingSort" type="hidden" value="False"/>
                    <input id="SortPropertyName" type="hidden" value="Turnover" />
                    <form id="export-excel-form"></form>
                 </body>`,
        hiddenFilter: `<body>
                    <div class="page-title"></div>
                        <form id="report-form" class="hide"></form>
                        <div id="tbl-report" data-totalrecords="0"
                            data-descendingsort="False"
                            data-sortedcolumn="Turnover"></div>
                        <div class="icon-filter"></div>
                        <div id="icon-excel"></div>
                        <input id="IsFilterVisible" type="hidden" />
                        <input id="UserSelectedProductId" type="hidden" />
                        <input id="UserSelectedStatusId" type="hidden" />
                        <input id="IsDescendingSort" type="hidden" value="False"/>
                        <input id="SortPropertyName" type="hidden" value="Turnover" />
                        <form id="export-excel-form"></form>
                     </body>`
    };

    beforeEach(() => {
        playForFunView = new PlayForFunView(
            jQueryFake,
            dateRangePickerComponent,
            sortComponent,
            productSelectionComponent,
            statusSelectionComponent,
            linkBuilderComponent);
    });

    it("Inherit BaseReport_Always_PlayForFunView is BaseReport", () => {
        // assert
        expect(playForFunView instanceof BaseReport).toBeTruthy();
    });

    describe("_init_Always", () => {
        beforeEach(() => {
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

        it("_Calls cacheElements", () => {
            // assert
            expect(playForFunView.cacheElements).toHaveBeenCalled();
        });
        it("_Calls blockUI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Calls unblockUI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_Renders Dropdown Products", () => {
            // assert
            expect(productSelectionComponent.render)
                .toHaveBeenCalledWith(new SingleSelectionDropdownOption("dropdown-product", "UserSelectedProductId"));
        });
        it("_Renders Dropdown Statuses", () => {
            // assert
            expect(statusSelectionComponent.render)
                .toHaveBeenCalledWith(new SingleSelectionDropdownOption("dropdown-statuses", "UserSelectedStatusId"));
        });
        it("_Renders Date Range Picker", () => {
            // assert
            expect(dateRangePickerComponent.render).toHaveBeenCalled();
        });
        it("_Init table Report", () => {
            // assert
            expect(playForFunView.initTableReport).toHaveBeenCalled();
        });;
        it("_Calls linkBuilderComponent.render", () => {
            // assert
            expect(linkBuilderComponent.render).toHaveBeenCalled();
        });
        it("_Calls registerEvents", () => {
            // assert
            expect(playForFunView.registerEvents).toHaveBeenCalled();
        });
    });
    describe("_cacheElements_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.report);
            spyOn(PlayForFunView.prototype, "cacheElements").and.callThrough();

            // act
            playForFunView.cacheElements();
        });

        it("_Calls super.cacheElements", () => {
            expect(PlayForFunView.prototype.cacheElements).toHaveBeenCalled();
        });
        it("_All neccessary elements have been cached", () => {
            // assert
            expect(playForFunView.$isDescendingSort).toBeDefined();
            expect(playForFunView.$sortPropertyName).toBeDefined();
        });
    });
    describe("_getSorterHeader", () => {
        it("_Always_Returns Correct Sorter Header", () => {
            // arrange
            let expectedSorterHeader = { 0: { sorter: "text" } };

            // act
            let sorterHeader = playForFunView.getSorterHeader();

            // assert
            expect(sorterHeader).toEqual(expectedSorterHeader);
        });
    });
    describe("_initTableReport_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.report);
            playForFunView.cacheElements();

            spyOn(jQueryFake.fn, "stickyTableHeaders");
            spyOn(sortComponent, "render");

            // act
            playForFunView.initTableReport();
        });

        it("_Bind stickyTableHeaders", () => {
            // assert
            expect(jQueryFake.fn.stickyTableHeaders).toHaveBeenCalled();
        });

        it("_Renders sorter", () => {
            // assert
            expect(sortComponent.render).toHaveBeenCalledWith(new SortOption(
                playForFunView.$tableReport,
                false,
                true,
                sortComponent.afterSort,
                { 0: { sorter: "text" } },
                playForFunView.$reportForm));
        });
    });
    describe("_onSubmitReport", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.report);
            playForFunView.init();

            spyOn(jQueryFake, "blockUI");

            // act
            playForFunView.onSubmitReport();
        });
        it("_Always_Block UI", () => {
            // assert
            expect(jQueryFake.blockUI).toHaveBeenCalled();
        });
        it("_Screen smaller than 768_Sets Correct new values to hidden fields", () => {
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
        it("_Screen not smaller than 768_Sets Correct new values to hidden fields", () => {
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
    describe("_getParams", () => {
        it("_Shown Filter_Returns Correct Params", () => {
            // arrange
            setFixtures(template.report);
            playForFunView.init();
            let expectedParams = {
                IsFilterVisible: true,
                SortPropertyName: "Turnover",
                IsDescendingSort: false
            };

            // act
            let actualParams = playForFunView.getParams();

            // assert
            expect(expectedParams).toEqual(actualParams);
        });
        it("_Hidden Filter_Returns Correct Params", () => {
            // arrange
            setFixtures(template.hiddenFilter);
            playForFunView.init();
            let expectedParams = {
                IsFilterVisible: false,
                SortPropertyName: "Turnover",
                IsDescendingSort: false
            };

            // act
            let actualParams = playForFunView.getParams();

            // assert
            expect(expectedParams).toEqual(actualParams);
        });
    });
    describe("_onExportExcel_Always", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.report);
            playForFunView.cacheElements();
            spyOn(playForFunView.$exportExcelForm, "submit");
            spyOn(jQueryFake, "unblockUI");

            // act
            playForFunView.onExportExcel();
        });
        it("_Submits Export Excel Form", () => {
            // assert
            expect(playForFunView.$exportExcelForm.submit).toHaveBeenCalled();
        });
        it("_Unblocks UI", () => {
            // assert
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
    });
});
