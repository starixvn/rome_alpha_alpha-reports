var _this = this;
describe("DualAxisChartComponent", function () {
    var dualAxisChartComponent;
    var chart = {
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
    var template = {
        dualAxisChart: "<body>\n                                <div class=\"chart-container\">\n                                    <canvas id=\"winloss-chart\"\n                                        data-title=\"Monthly Win Loss Chart From 03/01/2017 To 05/31/2017\"\n                                        data-labels=\"Oct-15,Nov-15,Dec-15,Jan-16\"\n                                        data-y-axis-1=\"1214.29,-2257.96,1944.90,-1065.65\"\n                                        data-y-axis-2=\"58630,63840,236403.50,260958.70\"\n                                        data-y-axis-1-label=\"Agent Win Loss\"\n                                        data-y-axis-2-label=\"Turnover\"\n                                        data-currency-name=\"UUS\"></canvas>\n                                </div>\n                            </body>",
    };
    beforeEach(function () {
        dualAxisChartComponent = new DualAxisChartComponent($, chart);
    });
    describe("_render", function () {
        it("_Always -> bind bar chart with correct options", function () {
            // arrange
            setFixtures(template.dualAxisChart);
            spyOn(chart, "Bar");
            var chartContainer = _this.$("#winloss-chart");
            var chartContext = chartContainer[0].getContext("2d");
            var chartData = {
                labels: ["Oct-15", "Nov-15", "Dec-15", "Jan-16"],
                datasets: [{
                        label: "Agent Win Loss",
                        type: "line",
                        borderColor: "#fcb441",
                        backgroundColor: "#fcb441",
                        fill: false,
                        lineTension: 0,
                        data: ["1214.29", "-2257.96", "1944.90", "-1065.65"],
                        yAxisID: "y-axis-1"
                    }, {
                        label: "Turnover",
                        type: "bar",
                        borderColor: "#418cf0",
                        backgroundColor: "#418cf0",
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
                                },
                                gridLines: {}
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
                                    beginAtZero: true,
                                    callback: alphaReports.formatNumber
                                },
                                gridLines: {}
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
                                    beginAtZero: true,
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
    describe("_formatTooltipLabel", function () {
        var formatTooltipLabelTest = function (tooltipItem, data, expectedValue) {
            // arranger
            spyOn(alphaReports, "formatNumber").and.callFake(function (value) { return value.toString(); });
            dualAxisChartComponent.currencyName = "UUS";
            // act
            var actualResult = dualAxisChartComponent.formatTooltipLabel(tooltipItem, data);
            // assert
            expect(actualResult).toEqual(expectedValue);
        };
        it("_Always -> Returns correct tooltip label", function () {
            var tooltipItem = {
                datasetIndex: 0,
                yLabel: "100000"
            };
            var dataset = {
                label: "Jan-17"
            };
            var data = {
                datasets: [dataset]
            };
            formatTooltipLabelTest(tooltipItem, data, "Jan-17 = 100,000.00 UUS");
        });
    });
});
//# sourceMappingURL=dual-axis-chart-tests.js.map