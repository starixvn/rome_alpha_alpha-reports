/// <reference path="../core.ts" />
"use strict";
var DualAxisChartOption = /** @class */ (function () {
    function DualAxisChartOption(isDarkMode, selectorId) {
        if (isDarkMode === void 0) { isDarkMode = false; }
        if (selectorId === void 0) { selectorId = "winloss-chart"; }
        this.isDarkMode = isDarkMode;
        this.selectorId = selectorId;
    }
    return DualAxisChartOption;
}());
var ChartStyles = /** @class */ (function () {
    function ChartStyles(barColor, textColor, lineColor, fontFamily, defaultFontSize, defaultFontStyle) {
        if (lineColor === void 0) { lineColor = "#fcb441"; }
        if (fontFamily === void 0) { fontFamily = "Trebuchet MS"; }
        if (defaultFontSize === void 0) { defaultFontSize = 12; }
        if (defaultFontStyle === void 0) { defaultFontStyle = "bold"; }
        this.barColor = barColor;
        this.textColor = textColor;
        this.lineColor = lineColor;
        this.fontFamily = fontFamily;
        this.defaultFontSize = defaultFontSize;
        this.defaultFontStyle = defaultFontStyle;
    }
    return ChartStyles;
}());
var DualAxisChartComponent = /** @class */ (function () {
    function DualAxisChartComponent($, chart) {
        var _this = this;
        this.options = new DualAxisChartOption();
        this.render = function (options) {
            if (options === void 0) { options = new DualAxisChartOption; }
            var chartOptions = _this.$.extend({}, _this.options, options);
            var chartStyles = new ChartStyles("#418cf0", "#000"), xAxesGridLines = {}, yAxesGridLines = { drawOnChartArea: false };
            if (chartOptions.isDarkMode) {
                chartStyles = new ChartStyles("#41a4ff", "#f2f2f2");
                xAxesGridLines = { color: "#444", zeroLineColor: "#555" };
                yAxesGridLines = { color: "#444", drawOnChartArea: false };
            }
            _this.chart.defaults.global.defaultFontFamily = chartStyles.fontFamily;
            _this.chart.defaults.global.defaultFontColor = chartStyles.textColor;
            _this.chart.defaults.global.defaultFontSize = chartStyles.defaultFontSize;
            _this.chart.defaults.global.defaultFontStyle = chartStyles.defaultFontStyle;
            var chartContainer = _this.$("#" + chartOptions.selectorId);
            var chartContext = chartContainer[0].getContext("2d");
            var labels = chartContainer.data("labels").split(",");
            _this.currencyName = chartContainer.data("currency-name");
            var chartData = {
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
            _this.chart.Bar(chartContext, {
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
                            label: _this.formatTooltipLabel
                        }
                    }
                }
            });
        };
        this.formatTooltipLabel = function (tooltipItem, data) {
            var datesetLabel = data.datasets[tooltipItem.datasetIndex].label;
            return datesetLabel + " = " + alphaReports.formatCurrency(Number(tooltipItem.yLabel)) + " " + _this.currencyName;
        };
        this.$ = $;
        this.chart = chart;
    }
    return DualAxisChartComponent;
}());
//# sourceMappingURL=dual-axis-chart.js.map