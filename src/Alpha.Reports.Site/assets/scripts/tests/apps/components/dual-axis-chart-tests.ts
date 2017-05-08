describe("DualAxisChartComponent", () => {
    let dualAxisChartComponent: DualAxisChartComponent;
    let chart = {
        Bar: jasmine.any(Function),
        defaults: {
            global: {
                defaultFontFamily: jasmine.any,
                defaultFontColor: jasmine.any,
                defaultFontSize: jasmine.any,
                defaultFontStyle: jasmine.any
            }
        }
    };

    let template = {
        dualAxisChart: `<body>
                                <div class="chart-container">
                                    <canvas id="winloss-chart"
                                        data-title="Monthly Win Loss Chart From 03/01/2017 To 05/31/2017"
                                        data-labels="Oct-15,Nov-15,Dec-15,Jan-16"
                                        data-y-axis-1="1214.29,-2257.96,1944.90,-1065.65"
                                        data-y-axis-2="58630,63840,236403.50,260958.70"
                                        data-y-axis-1-label="Agent Win Loss"
                                        data-y-axis-2-label="Turnover"
                                        data-currency-name="UUS"></canvas>
                                </div>
                            </body>`,
    };

    beforeEach(() => {
        dualAxisChartComponent = new DualAxisChartComponent($, chart);
    });

    describe("_render", () => {
        it("_Always -> bind bar chart with correct options", () => {
            // arrange
            setFixtures(template.dualAxisChart);
            spyOn(chart, "Bar");
            let chartStyles = {
                barColor: "#418cf0",
                lineColor: "#fcb441",
                textColor: "#000",
                fontFamily: "Trebuchet MS",
                defaultFontSize: 12,
                defaultFontStyle: "bold"
            };

            let chartContainer: any = this.$("#winloss-chart");
            let chartContext = chartContainer[0].getContext("2d");

            let chartData = {
                labels: ["Oct-15", "Nov-15", "Dec-15", "Jan-16"],
                datasets: [{
                    label: "Agent Win Loss",
                    type: "line",
                    borderColor: chartStyles.lineColor,
                    backgroundColor: chartStyles.lineColor,
                    fill: false,
                    lineTension: 0,
                    data: ["1214.29", "-2257.96", "1944.90", "-1065.65"],
                    yAxisID: "y-axis-1"
                }, {
                    label: "Turnover",
                    type: "bar",
                    borderColor: chartStyles.barColor,
                    backgroundColor: chartStyles.barColor,
                    fill: false,
                    data: ["58630", "63840", "236403.50", "260958.70"],
                    yAxisID: "y-axis-2"
                }]
            };

            // act
            dualAxisChartComponent.render();

            // assert
            expect(chart.Bar).toHaveBeenCalledWith(chartContext, {
                data: chartData,
                options: {
                    responsive: true,
                    hoverMode: "index",
                    stacked: false,
                    title: {
                        display: true,
                        text: "Monthly Win Loss Chart From 03/01/2017 To 05/31/2017",
                        fontSize: 18
                    },
                    scales: {
                        xAxes: [{
                            barThickness: 30,
                            ticks: {
                                autoSkip: false,
                                maxRotation: 50,
                                minRotation: 50
                            }
                        }],
                        yAxes: [{
                            type: "linear",
                            display: true,
                            position: "left",
                            id: "y-axis-1",
                            scaleLabel: {
                                display: true,
                                labelString: "Agent Win Loss"
                            },
                            ticks: {
                                callback: alphaReports.formatNumber
                            }
                        }, {
                            type: "linear",
                            display: true,
                            position: "right",
                            id: "y-axis-2",
                            scaleLabel: {
                                display: true,
                                labelString: "Turnover"
                            },
                            ticks: {
                                callback: alphaReports.formatNumber
                            },

                            gridLines: {
                                drawOnChartArea: false
                            }
                        }]
                    },
                    legend: {
                        display: true,
                        position: "bottom"
                    },
                    tooltips: {
                        callbacks: {
                            label: dualAxisChartComponent.formatTooltipLabel
                        }
                    }
                }
            });
        });
    });

    describe("_formatTooltipLabel", () => {
        let formatTooltipLabelTest = (tooltipItem: any, data: any, expectedValue: string) => {
            // arranger
            spyOn(alphaReports, "formatNumber").and.callFake((value: number) => value.toString());
            dualAxisChartComponent.currencyName = "UUS";

            // act
            var actualResult = dualAxisChartComponent.formatTooltipLabel(tooltipItem, data);

            // assert
            expect(actualResult).toEqual(expectedValue);
        };
        it("_Always -> Returns correct tooltip label", () => {
            let tooltipItem = {
                datasetIndex: 0,
                yLabel: "100000"
            };
            let dataset = {
                label: "Jan-17"
            };
            let data = {
                datasets: [dataset]
            };

            formatTooltipLabelTest(tooltipItem, data, "Jan-17 = 100000 UUS");
        });
    });
});