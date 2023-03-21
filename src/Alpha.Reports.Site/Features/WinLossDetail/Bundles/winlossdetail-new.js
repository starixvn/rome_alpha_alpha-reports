"use strict";
var AlphaReports = /** @class */ (function () {
    function AlphaReports($, store) {
        var _this = this;
        this.format = {
            monthFormat: "MMMM - yyyy",
            shortDateWithSlash_DatePicker: "MM/DD/YYYY",
            shortDateWithSlash_MonthPicker: "MM/dd/yyyy"
        };
        this.sort = {
            none: 0,
            ascending: 1,
            descending: -1
        };
        this.keyCode = {
            enter: 13
        };
        this.numberDisplayedProductsOnFilter = 1;
        this.maxTimesToShowTooltip = 5;
        this.screenSmallThan768 = function () {
            var currentWindowWidth = screen.width || _this.$(window).width();
            return currentWindowWidth < 768;
        };
        this.screenLandscapeMode = function () { return screen.width > screen.height || _this.$(window).width() > _this.$(window).height(); };
        this.splitCell = function (firstCell, cellClass, lastCellStyle) {
            if (cellClass === void 0) { cellClass = ""; }
            if (lastCellStyle === void 0) { lastCellStyle = "bold"; }
            var this$ = _this.$;
            var combinedCells = _this.$("." + firstCell + "-combined");
            combinedCells.each(function (index) {
                var oldValues = this$(this).find("li");
                var cellsLength = oldValues.length;
                var oldCells = "";
                for (var i = 0, lastCellIndex = cellsLength - 1; i < cellsLength; i++) {
                    oldCells += "<td class=\"hidden-stack-mode " + cellClass +
                        (i === lastCellIndex ? " " + lastCellStyle : "") + "\">" +
                        this$(oldValues[i]).html() +
                        "</td>";
                }
                this$(combinedCells[index]).before(oldCells);
            });
        };
        this.combineConsecutiveCells = function (firstCells, numberOfCombinedCells, combinedCellClass, combinedCellColSpan) {
            if (combinedCellColSpan === void 0) { combinedCellColSpan = 1; }
            if (numberOfCombinedCells <= 1) {
                return;
            }
            var this$ = _this.$;
            firstCells.each(function (index) {
                var newCell = "<td class=\"visible-stack-mode combinedcell " +
                    combinedCellClass + "\" colspan=\"" + combinedCellColSpan + "\">\
                    <ul>";
                var currentCell = this$(firstCells[index]);
                var isLastCell = false;
                for (var i = 0; i < numberOfCombinedCells; i++) {
                    newCell += "<li>" + currentCell.html() + "</li>";
                    if (currentCell.next().length) {
                        currentCell = currentCell.next();
                    }
                    else {
                        isLastCell = true;
                    }
                }
                newCell += "</ul>\
                        </td>";
                if (isLastCell) {
                    currentCell.after(newCell);
                }
                else {
                    currentCell.prev().after(newCell);
                }
            });
        };
        this.localStorage = {
            get: function (key) {
                return _this.store.enabled ? _this.store.get(key) : _this.$.cookie(key);
            },
            set: function (key, value) {
                _this.store.enabled ? _this.store.set(key, value) : _this.$.cookie(key, value, _this.localStorage.getCookieExpireOptions());
            },
            getCookieExpireOptions: function () {
                var cookieExpire = { expires: 365, path: window.location.pathname };
                if (_this.isIe()) {
                    cookieExpire = { expires: 365 };
                }
                return cookieExpire;
            }
        };
        this.isIe = function () {
            var ua = window.navigator.userAgent;
            var msie = ua.indexOf("MSIE ");
            return msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./);
        };
        this.replaceQueryStringValue = function (queryString, paramName, paramValue) {
            if (typeof queryString === "undefined"
                || queryString === null) {
                return "";
            }
            var regex = new RegExp("([?&])" + paramName + "=[^&]*", "i");
            var newQueryString = regex.test(queryString)
                ? queryString.replace(regex, "$1" + paramName + "=" + paramValue)
                : queryString + (queryString.indexOf("?") >= 0 ? "&" : "?") + paramName + "=" + paramValue;
            return newQueryString;
        };
        this.correctEvenRowClass = function ($tableReport) {
            var this$ = _this.$;
            var isEvenRow = false;
            $tableReport.find("tbody > tr").each(function () {
                var $row = this$(this);
                if ($row.hasClass("even-row") && !isEvenRow) {
                    $row.removeClass("even-row");
                }
                else if (!$row.hasClass("even-row") && isEvenRow) {
                    $row.addClass("even-row");
                }
                isEvenRow = !isEvenRow;
            });
        };
        this.convertAttributeToBool = function (attrValue) {
            return !!attrValue && attrValue.toString().toLowerCase() === "true";
        };
        this.formatNumber = function (value) { return value.toLocaleString("en-US"); };
        this.formatCurrency = function (value) { return value.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,"); };
        this.$ = $;
        this.store = store;
    }
    AlphaReports.prototype.getFormData = function ($form) {
        var unindexed_array = $form.serializeArray();
        var indexed_array = {};
        $.map(unindexed_array, function (n, i) {
            indexed_array[n['name']] = n['value'];
        });
        return indexed_array;
    };
    return AlphaReports;
}());
var alphaReports = new AlphaReports($, store);
//# sourceMappingURL=core.js.map
"use strict";
var DateRangePickerNewComponent = /** @class */ (function () {
    function DateRangePickerNewComponent($) {
        this.$ = $;
    }
    DateRangePickerNewComponent.prototype.render = function () {
        var _this = this;
        var dateRangePicker = this.$("#daterange-picker");
        var localeFormat = "MM/DD/YYYY";
        this.fromDate = dateRangePicker.data("fromdate");
        this.toDate = dateRangePicker.data("todate");
        return $("#daterange-picker").daterangepicker({
            autoApply: true,
            ranges: this.getRangeDates(dateRangePicker),
            locale: {
                format: localeFormat,
                customRangeLabel: dateRangePicker.data("label-specificdate"),
            },
            linkedCalendars: false,
            alwaysShowCalendars: true,
            startDate: this.fromDate,
            endDate: this.toDate,
            minDate: dateRangePicker.data("mindate"),
            maxDate: dateRangePicker.data("maxdate"),
        }, function (start, end, label) {
            _this.fromDate = start.format(localeFormat);
            _this.toDate = end.format(localeFormat);
            console.log("New date range selected: " + start.format(localeFormat) + " to " + end.format(localeFormat) + " (predefined range: " + label + ")");
        });
    };
    DateRangePickerNewComponent.prototype.getRangeDates = function (dateRangePicker) {
        var today = moment(dateRangePicker.data("currentdate"));
        var yesterday = moment(today).subtract(1, "d");
        var thisWeekStartOfDay = moment(today).startOf("isoWeek");
        var lastMonthStartOfDay = moment(today).subtract(1, "M").startOf("M");
        var ranges = {};
        ranges[dateRangePicker.data("label-today")] = [today, today];
        ranges[dateRangePicker.data("label-yesterday")] = [yesterday, yesterday];
        ranges[dateRangePicker.data("label-thisweek")] = [thisWeekStartOfDay, today];
        ranges[dateRangePicker.data("label-lastweek")] = [moment(thisWeekStartOfDay).subtract(1, "w"), moment(thisWeekStartOfDay).subtract(1, "d")];
        ranges[dateRangePicker.data("label-thismonth")] = [moment(today).startOf("M"), today];
        ranges[dateRangePicker.data("label-lastmonth")] = [lastMonthStartOfDay, moment(lastMonthStartOfDay).endOf("M")];
        ranges[dateRangePicker.data("label-sincelastmonth")] = [lastMonthStartOfDay, today];
        return ranges;
    };
    DateRangePickerNewComponent.prototype.getDateRangeValues = function () {
        return { fromDate: this.fromDate, toDate: this.toDate };
    };
    return DateRangePickerNewComponent;
}());
//# sourceMappingURL=daterangepicker-new.js.map
"use strict";
var WeekRangeComponent = /** @class */ (function () {
    function WeekRangeComponent($, moment) {
        this.$ = $;
        this.moment = moment;
    }
    WeekRangeComponent.prototype.render = function (fromDate, toDate, onClick) {
        var _this = this;
        var weekRange = [];
        var TextDateFormat = "MMM YYYYY";
        var ValueDateFormat = "MM/DD/YYYYY";
        var fromDateStartOfWeek = this.moment(fromDate).startOf("isoWeek");
        var toDateStartOfWeek = this.moment(toDate).startOf("isoWeek");
        if (!fromDateStartOfWeek.isSame(toDateStartOfWeek)) {
            var startDate = moment(fromDate);
            var endDate = moment(toDate);
            while (endDate.diff(startDate, "d") >= 0) {
                var endDateStartOfWeek = moment(endDate).startOf("isoWeek");
                var startDateOrEndDateStartOfWeek = startDate.isBefore(endDateStartOfWeek) ? endDateStartOfWeek : startDate;
                var fromDateValue = startDateOrEndDateStartOfWeek.format(ValueDateFormat);
                var toDateValue = endDate.format(ValueDateFormat);
                var label = startDateOrEndDateStartOfWeek.format(TextDateFormat) + " - " + endDate.format(TextDateFormat);
                weekRange.push("<span class=\"label label-default label-week-range\" data-fromdate=\"" + fromDateValue + "\" data-todate=\"" + toDateValue + "\">" + label + "</span>");
                endDate = moment(endDateStartOfWeek).subtract(1, "d");
            }
        }
        this.$("#labelWeekRange")
            .html(weekRange.join(""))
            .find(".label-week-range")
            .off()
            .click(function (e) {
            var $this = _this.$(e.target);
            onClick && onClick($this.data("fromdate"), $this.data("todate"));
        });
    };
    return WeekRangeComponent;
}());
//# sourceMappingURL=week-range.js.map
"use strict";
var ProductAndProductDetailDropdownComponent = /** @class */ (function () {
    function ProductAndProductDetailDropdownComponent($) {
        var _this = this;
        this.optionClass = function (element) {
            if (_this.$(element).data("isnested") === 1) {
                return "multiselect-nested-item";
            }
        };
        this.onChangeProductDetail = function (option, checked) {
            var productId = _this.$(option.context).data("productid");
            if (checked) {
                if (!_this.$dropdownProduct.find("option[value=\"" + productId + "\"]:selected").length) {
                    _this.$dropdownProduct.multiselect("select", productId);
                }
            }
            else {
                if (!_this.$dropdownProductDetail.find("option[data-productid=\"" + productId + "\"]:selected").length) {
                    _this.$dropdownProduct.multiselect("deselect", productId);
                }
            }
            _this.validateProduct();
        };
        this.onSelectAllProductDetail = function (e) {
            var productIds = [];
            _this.$dropdownProductDetail.find("option").each(function (_, item) { return productIds.push(_this.$(item).data("productid")); });
            var uniqueProductIds = productIds.filter(function (value, index) { return productIds.indexOf(value) === index; });
            var checked = _this.$(e.target).is(":checked");
            _this.$dropdownProduct.multiselect(checked ? "select" : "deselect", uniqueProductIds);
            _this.validateProduct();
        };
        this.$ = $;
    }
    ProductAndProductDetailDropdownComponent.prototype.render = function () {
        this.renderProduct();
        this.renderProductDetail();
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProduct = function () {
        var _this = this;
        this.$dropdownProduct = this.$("#dropdown-product");
        this.$dropdownProduct.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProduct.data("label-select-products"),
            nSelectedText: " " + this.$dropdownProduct.data("label-selected"),
            allSelectedText: this.$dropdownProduct.data("label-all-products"),
            selectAllText: this.$dropdownProduct.data("label-select-all"),
            includeSelectAllOption: true,
            onChange: function (option, checked) { return _this.checkOrUncheckProductDetail(checked, option.context.value); },
            enableCollapsibleOptGroups: true,
            optionClass: this.optionClass
        });
        this.$dropdownProduct.next().find(".multiselect-container li").removeClass("hidden");
        this.$dropdownProductContainer = this.$dropdownProduct.parent().find(".multiselect.btn");
        // Cannot use build in function 'onSelectAll'.
        // Must register CheckAll checkbox event manually.
        this.$("#product-multiselect-container .multiselect-all input[type=checkbox]").change(function (e) { return _this.checkOrUncheckProductDetail(_this.$(e.target).is(":checked")); });
    };
    ProductAndProductDetailDropdownComponent.prototype.checkOrUncheckProductDetail = function (checked, productId) {
        if (productId === void 0) { productId = undefined; }
        this.validateProduct();
        var optionElements = productId
            ? this.$dropdownProductDetail.find("option[data-productid=\"" + productId + "\"]")
            : this.$dropdownProductDetail.find("option");
        var selectedProductIds = [];
        optionElements.each(function (_, item) { return selectedProductIds.push(item["value"]); });
        this.$dropdownProductDetail.multiselect(checked ? "select" : "deselect", selectedProductIds);
    };
    ProductAndProductDetailDropdownComponent.prototype.validateProduct = function () {
        if (this.$dropdownProduct.find("option:selected").length) {
            this.$dropdownProductContainer.removeClass("error");
        }
        else {
            this.$dropdownProductContainer.addClass("error");
        }
    };
    ProductAndProductDetailDropdownComponent.prototype.renderProductDetail = function () {
        this.$dropdownProductDetail = this.$("#dropdown-product-detail");
        var selectAllText = this.$dropdownProductDetail.data("label-all-product-detail");
        this.$dropdownProductDetail.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaReports.numberDisplayedProductsOnFilter,
            nonSelectedText: this.$dropdownProductDetail.data("label-select-product-detail"),
            nSelectedText: " " + this.$dropdownProductDetail.data("label-selected"),
            allSelectedText: selectAllText,
            selectAllText: selectAllText,
            includeSelectAllOption: true,
            onChange: this.onChangeProductDetail
        });
        // Cannot use build in function 'onSelectAll'.
        // Must register CheckAll checkbox event manually.
        this.$("#product-detail-multiselect-container .multiselect-all input[type=checkbox]").change(this.onSelectAllProductDetail);
    };
    ProductAndProductDetailDropdownComponent.prototype.getProductSelectedItems = function () {
        var selectedProductIds = [];
        this.$dropdownProduct.find("option:selected").each(function (_, item) { return selectedProductIds.push(item["value"]); });
        return selectedProductIds.join();
    };
    ProductAndProductDetailDropdownComponent.prototype.getProductDetailSelectedItems = function () {
        var selectedProductDetailIds = [];
        this.$dropdownProductDetail.find("option:selected").each(function (_, item) { return selectedProductDetailIds.push(item["value"]); });
        return selectedProductDetailIds.join();
    };
    return ProductAndProductDetailDropdownComponent;
}());
//# sourceMappingURL=product-productdetail-dropdown.js.map
"use strict";
var WinLossDetailViewNew = /** @class */ (function () {
    function WinLossDetailViewNew($, dateRangePickerComponent, weekRangeComponent, productAndProductDetailDropdownComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.weekRangeComponent = weekRangeComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
    }
    WinLossDetailViewNew.prototype.openFullscreen = function () {
        var element = document.documentElement;
        if (element.requestFullscreen) {
            element.requestFullscreen();
        }
        else if (element.webkitRequestFullscreen) { /* Safari */
            element.webkitRequestFullscreen();
        }
        else if (element.msRequestFullscreen) { /* IE11 */
            element.msRequestFullscreen();
        }
    };
    WinLossDetailViewNew.prototype.closeFullscreen = function () {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        }
        else if (document.webkitExitFullscreen) { /* Safari */
            document.webkitExitFullscreen();
        }
        else if (document.msExitFullscreen) { /* IE11 */
            document.msExitFullscreen();
        }
    };
    WinLossDetailViewNew.prototype.init = function () {
        this.dateRangePickerComponent.render();
        this.productAndProductDetailDropdownComponent.render();
        this.cacheDOMElements();
        this.bindEvents();
    };
    WinLossDetailViewNew.prototype.cacheDOMElements = function () {
        this.hiddenCustId = this.$("#hiddenCustId");
        this.hiddenCustLevel = +this.$("#hiddenCustLevel").val();
    };
    WinLossDetailViewNew.prototype.bindEvents = function () {
        var _this = this;
        var reportForm = this.$("#report-form");
        var dataTable = $("#tbl-report").DataTable(this.buildDataTableOptions(reportForm.attr("action")));
        reportForm.submit(function (e) {
            e.preventDefault();
            dataTable.ajax.reload();
            var formData = dataTable.ajax.param();
            _this.weekRangeComponent.render(formData.fromDate, formData.toDate, function (fromDate, toDate) { });
        });
        this.$(".icon-filter").click(this.openFullscreen);
    };
    WinLossDetailViewNew.prototype.buildDataTableOptions = function (winlossDetailUrl) {
        //const sortDefinitions = {
        //    CreatedDate: 1,
        //    WinningLosingAmount: 2,
        //    WinningThreshold: 3,
        //    LosingThreshold: 4
        //};
        var isMasterOrSuperLevel = this.isMasterOrSuperLevel();
        var isSuperLevel = this.isSuperLevel();
        var self = this;
        return {
            serverSide: true,
            ajax: {
                type: "POST",
                url: winlossDetailUrl,
                data: function (formData) {
                    var dateRange = self.dateRangePickerComponent.getDateRangeValues();
                    var productIds = self.productAndProductDetailDropdownComponent.getProductSelectedItems();
                    var resourceIds = self.productAndProductDetailDropdownComponent.getProductDetailSelectedItems();
                    formData.fromDate = dateRange.fromDate;
                    formData.toDate = dateRange.toDate;
                    formData.userSelectedProductIds = productIds;
                    formData.userSelectedResourceIds = resourceIds;
                },
                dataType: "json",
                dataSrc: "WinLossDetail.DataSource"
            },
            //ajax: (data, callback) => {
            //    //const columnName = data.columns[data.order[0].column].name;
            //    //const sortType = sortDefinitions[columnName];
            //    this.$.get(
            //        `${winlossDetailUrl}?${this.$.param(this.getFormData())}`,
            //        jsonData => callback({ data: jsonData.WinLossDetail.DataSource }));
            //},
            columns: this.buildDataTableColumns(isMasterOrSuperLevel, isSuperLevel),
            ordering: true,
            order: [0, "asc"],
            paging: false,
            searching: false,
            bInfo: false,
            dom: 'rt<"bottom"iflp<"clear">>',
            fixedHeader: true,
            processing: true,
            language: {
                zeroRecords: this.$("#hiddenNoInformation").val()
            },
            initComplete: function (_, json) {
                //self.updateTotalFooter(json, this.api());
                //if (json.WinLossDetail.TotalRecords) {
                //    const formatDecimal = $.fn.dataTable.render.number(",", ".", 2, "");
                //} else {
                //    console.dir(this.api().table().footer());
                //}
            },
            footerCallback: function (row, data, start, end, display) {
                console.log('footerCallback');
                self.updateTotalFooter(row, this.api(), isMasterOrSuperLevel, isSuperLevel);
            }
            //drawCallback: _ => tableReport.removeClass("hidden")
        };
    };
    WinLossDetailViewNew.prototype.buildDataTableColumns = function (isMasterOrSuperLevel, isSuperLevel) {
        var _this = this;
        var formatInt = $.fn.dataTable.render.number(",", ".", 0, "").display;
        var formatDecimal = $.fn.dataTable.render.number(",", ".", 2, "").display;
        return [
            { data: "CustName" },
            { data: "BetCount", render: formatInt },
            { data: "Turnover", render: formatDecimal },
            { data: "NetTurnover", render: formatDecimal },
            { data: "GrossComm", render: formatDecimal },
            {
                data: null,
                render: function (data, type, row) { return type === "display"
                    ? _this.renderGroupColumn(row.Turnover, row.NetTurnover, row.GrossComm, formatDecimal)
                    : data; },
                visible: false
            },
            { data: "MemberNetWinLoss.WinLoss", render: formatDecimal },
            { data: "MemberNetWinLoss.Comm", render: formatDecimal },
            { data: "MemberNetWinLoss.Total", render: formatDecimal },
            {
                data: null,
                render: function (data, type, row) { return type === "display"
                    ? _this.renderGroupColumn(row.MemberNetWinLoss.WinLoss, row.MemberNetWinLoss.Comm, row.MemberNetWinLoss.Total, formatDecimal)
                    : data; },
                visible: false
            },
            { data: "AgentNetWinLoss.WinLoss", render: formatDecimal, className: "altercol" },
            { data: "AgentNetWinLoss.Comm", render: formatDecimal, className: "altercol" },
            { data: "AgentNetWinLoss.Total", render: formatDecimal, className: "altercol" },
            {
                data: null,
                render: function (data, type, row) { return type === "display"
                    ? _this.renderGroupColumn(row.AgentNetWinLoss.WinLoss, row.AgentNetWinLoss.Comm, row.AgentNetWinLoss.Total, formatDecimal)
                    : data; },
                className: "altercol",
                visible: false
            },
            { data: "MasterNetWinLoss.WinLoss", render: formatDecimal, visible: isMasterOrSuperLevel },
            { data: "MasterNetWinLoss.Comm", render: formatDecimal, visible: isMasterOrSuperLevel },
            { data: "MasterNetWinLoss.Total", render: formatDecimal, visible: isMasterOrSuperLevel },
            {
                data: null,
                render: function (data, type, row) { return type === "display"
                    ? _this.renderGroupColumn(row.MasterNetWinLoss.WinLoss, row.MasterNetWinLoss.Comm, row.MasterNetWinLoss.Total, formatDecimal)
                    : data; },
                visible: false
            },
            { data: "SuperNetWinLoss.WinLoss", render: formatDecimal, className: "altercol", visible: isSuperLevel },
            { data: "SuperNetWinLoss.Comm", render: formatDecimal, className: "altercol", visible: isSuperLevel },
            { data: "SuperNetWinLoss.Total", render: formatDecimal, className: "altercol", visible: isSuperLevel },
            {
                data: null,
                render: function (data, type, row) { return type === "display"
                    ? _this.renderGroupColumn(row.SuperNetWinLoss.WinLoss, row.SuperNetWinLoss.Comm, row.SuperNetWinLoss.Total, formatDecimal)
                    : data; },
                className: "altercol",
                visible: false
            },
            { data: "Company", render: formatDecimal },
        ];
    };
    WinLossDetailViewNew.prototype.updateTotalFooter = function (row, api, isMasterOrSuperLevel, isSuperLevel) {
        var winLossDetail = api.ajax.json().WinLossDetail;
        if (winLossDetail.TotalRecords) {
            var formatInt = $.fn.dataTable.render.number(",", ".", 0, "").display;
            var formatDecimal = $.fn.dataTable.render.number(",", ".", 2, "").display;
            var total = winLossDetail.TotalData;
            this.$(api.column(1).footer()).html(formatInt(total.BetCount));
            this.$(api.column(2).footer()).html(formatDecimal(total.Turnover));
            this.$(api.column(3).footer()).html(formatDecimal(total.NetTurnover));
            this.$(api.column(4).footer()).html(formatDecimal(total.GrossComm));
            this.$(api.column(5).footer()).html(this.renderGroupColumn(total.Turnover, total.NetTurnover, total.GrossComm, formatDecimal));
            this.$(api.column(6).footer()).html(formatDecimal(total.MemberNetWinLoss.WinLoss));
            this.$(api.column(7).footer()).html(formatDecimal(total.MemberNetWinLoss.Comm));
            this.$(api.column(8).footer()).html(formatDecimal(total.MemberNetWinLoss.Total));
            this.$(api.column(9).footer()).html(this.renderGroupColumn(total.MemberNetWinLoss.WinLoss, total.MemberNetWinLoss.Comm, total.MemberNetWinLoss.Total, formatDecimal));
            this.$(api.column(10).footer()).html(formatDecimal(total.AgentNetWinLoss.WinLoss));
            this.$(api.column(11).footer()).html(formatDecimal(total.AgentNetWinLoss.Comm));
            this.$(api.column(12).footer()).html(formatDecimal(total.AgentNetWinLoss.Total));
            this.$(api.column(13).footer()).html(this.renderGroupColumn(total.AgentNetWinLoss.WinLoss, total.AgentNetWinLoss.Comm, total.AgentNetWinLoss.Total, formatDecimal));
            if (isMasterOrSuperLevel) {
                this.$(api.column(14).footer()).html(formatDecimal(total.MasterNetWinLoss.WinLoss));
                this.$(api.column(15).footer()).html(formatDecimal(total.MasterNetWinLoss.Comm));
                this.$(api.column(16).footer()).html(formatDecimal(total.MasterNetWinLoss.Total));
                this.$(api.column(17).footer()).html(this.renderGroupColumn(total.MasterNetWinLoss.WinLoss, total.MasterNetWinLoss.Comm, total.MasterNetWinLoss.Total, formatDecimal));
            }
            if (isSuperLevel) {
                this.$(api.column(18).footer()).html(formatDecimal(total.SuperNetWinLoss.WinLoss));
                this.$(api.column(19).footer()).html(formatDecimal(total.SuperNetWinLoss.Comm));
                this.$(api.column(20).footer()).html(formatDecimal(total.SuperNetWinLoss.Total));
                this.$(api.column(21).footer()).html(this.renderGroupColumn(total.SuperNetWinLoss.WinLoss, total.SuperNetWinLoss.Comm, total.SuperNetWinLoss.Total, formatDecimal));
            }
            this.$(api.column(22).footer()).html(formatDecimal(total.Company));
        }
    };
    WinLossDetailViewNew.prototype.renderGroupColumn = function (number1, number2, number3, formatDecimal) {
        return "<div>" + formatDecimal(number1) + "</div><div>" + formatDecimal(number2) + "</div><div>" + formatDecimal(number3) + "</div>";
    };
    WinLossDetailViewNew.prototype.isMasterOrSuperLevel = function () {
        return this.hiddenCustLevel >= 3;
    };
    WinLossDetailViewNew.prototype.isSuperLevel = function () {
        return this.hiddenCustLevel === 4;
    };
    return WinLossDetailViewNew;
}());
$(function () {
    new WinLossDetailViewNew($, new DateRangePickerNewComponent($), new WeekRangeComponent($, moment), new ProductAndProductDetailDropdownComponent($)).init();
});
//# sourceMappingURL=winlossdetail-new.js.map