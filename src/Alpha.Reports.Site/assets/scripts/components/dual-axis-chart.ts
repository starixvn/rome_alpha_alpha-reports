/// <reference path="../core.ts" />

"use strict";

interface JQuery {
    Chart(option: any): void;
}

class DualAxisChartOption {
    constructor(
        public isDarkMode: boolean = false,
        public selectorId: string = "winloss-chart") {
    }
}

class ChartStyles {
    constructor(
        public barColor: string,
        public textColor: string,
        public lineColor: string = "#fcb441",
        public fontFamily: string = "Trebuchet MS",
        public defaultFontSize: number = 12,
        public defaultFontStyle: string = "bold") {
    }
}

class DualAxisChartComponent {
    $: JQueryStatic;
    chart: any;

    options = new DualAxisChartOption();
    currencyName: string;

    constructor($: JQueryStatic, chart: any) {
        this.$ = $;
        this.chart = chart;
    }

    render = (options: DualAxisChartOption = new DualAxisChartOption) => {
        let chartOptions = <DualAxisChartOption>this.$.extend({}, this.options, options);
        let chartStyles: ChartStyles = new ChartStyles("#418cf0", "#000"),
            xAxesGridLines: any = {},
            yAxesGridLines: any = { drawOnChartArea: false };

        if (chartOptions.isDarkMode) {
            chartStyles = new ChartStyles("#41a4ff", "#f2f2f2");
            xAxesGridLines = { color: "#444", zeroLineColor: "#555" };
            yAxesGridLines = { color: "#444", drawOnChartArea: false };
        }

        this.chart.defaults.global.defaultFontFamily = chartStyles.fontFamily;
        this.chart.defaults.global.defaultFontColor = chartStyles.textColor;
        this.chart.defaults.global.defaultFontSize = chartStyles.defaultFontSize;
        this.chart.defaults.global.defaultFontStyle = chartStyles.defaultFontStyle;

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
                data: chartContainer.attr("data-y-axis-1").split(","),
                yAxisID: "y-axis-1"
            }, {
                label: chartContainer.data("y-axis-2-label"),
                type: "bar",
                borderColor: chartStyles.barColor,
                backgroundColor: chartStyles.barColor,
                fill: false,
                data: chartContainer.attr("data-y-axis-2").split(","),
                yAxisID: "y-axis-2"
            }]
        };

        this.chart.Bar(chartContext, {
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
                        },
                        gridLines: xAxesGridLines
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
                            beginAtZero: true,
                            callback: alphaReports.formatNumber
                        },
                        gridLines: xAxesGridLines
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
                            beginAtZero: true,
                            callback: alphaReports.formatNumber
                        },
                        gridLines: yAxesGridLines
                    }]
                },
                legend: {
                    display: true,
                    position: "bottom"
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
        return datesetLabel + " = " + alphaReports.formatCurrency(Number(tooltipItem.yLabel)) + " " + this.currencyName;
    };
}
