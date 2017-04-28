/// <reference path="../core.ts" />

"use strict";
declare let Chart: any;

interface JQuery {
    Chart(option: any): void;
}

class DualAxisChartOption {
    constructor(public selectorId: string = "winloss-chart") {
    }
}

class DualAxisChartComponent {
    $: JQueryStatic;

    options = new DualAxisChartOption();
    currencyName: string;

    chartStyles = {
        barColor: "#418cf0",
        lineColor: "#fcb441",
        textColor: "#000",
        fontFamily: "Trebuchet MS",
        defaultFontSize: 12,
        defaultFontStyle: "bold"
    };

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: DualAxisChartOption = new DualAxisChartOption) => {
        let chartOptions = <DualAxisChartOption>this.$.extend({}, this.options, options);
        let chartStyles = this.chartStyles;

        Chart.defaults.global.defaultFontFamily = chartStyles.fontFamily;
        Chart.defaults.global.defaultFontColor = chartStyles.textColor;
        Chart.defaults.global.defaultFontSize = chartStyles.defaultFontSize;
        Chart.defaults.global.defaultFontStyle = chartStyles.defaultFontStyle;

        let chartContainer: any = this.$("#" + chartOptions.selectorId);
        let chartContext = chartContainer[0].getContext("2d");

        let labels = chartContainer.data("labels").split(",");
        this.currencyName = chartContainer.data("currency-name");

        let chartData = {
            labels: labels,
            datasets: [{
                label: chartContainer.data("y-axis-1-label"),
                type: "line",
                borderColor: chartStyles.lineColor,
                backgroundColor: chartStyles.lineColor,
                fill: false,
                lineTension: 0,
                data: chartContainer.data("y-axis-1").split(","),
                yAxisID: "y-axis-1"
            }, {
                label: chartContainer.data("y-axis-2-label"),
                type: "bar",
                borderColor: chartStyles.barColor,
                backgroundColor: chartStyles.barColor,
                fill: false,
                data: chartContainer.data("y-axis-2").split(","),
                yAxisID: "y-axis-2"
            }]
        };

        Chart.Bar(chartContext, {
            data: chartData,
            options: {
                responsive: true,
                hoverMode: "index",
                stacked: false,
                title: {
                    display: true,
                    text: chartContainer.data("title"),
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
                            labelString: chartContainer.data("y-axis-1-label")
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
                            labelString: chartContainer.data("y-axis-2-label")
                        },
                        ticks: {
                            callback: alphaReports.formatNumber
                        },

                        gridLines: {
                            drawOnChartArea: false
                        },
                    }],
                },
                legend: {
                    display: true,
                    position: 'bottom'
                },
                tooltips: {
                    callbacks: {
                        label: this.formatTooltipLabel
                    }
                }
            }
        });
    };

    formatTooltipLabel = (tooltipItem: any, data: any) => {
        let datesetLabel = data.datasets[tooltipItem.datasetIndex].label;
        return datesetLabel + ' = ' + alphaReports.formatNumber(Number(tooltipItem.yLabel)) + " " + this.currencyName;
    };
}