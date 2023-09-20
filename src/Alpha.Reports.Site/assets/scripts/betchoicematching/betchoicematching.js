/// <reference path="../core.ts" />
/// <reference path="../components/simple-tooltip.ts" />
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
function getValueByKey(data, key) {
    for (var i = 0; i < data.length; i++) {
        var item = data[i];
        if (item.Key === key)
            return item.Value;
    }
    return null;
}
var BetTypes;
(function (BetTypes) {
    BetTypes[BetTypes["HandicapFT"] = 1] = "HandicapFT";
    BetTypes[BetTypes["OverUnderFT"] = 3] = "OverUnderFT";
    BetTypes[BetTypes["OneXTwoFT"] = 5] = "OneXTwoFT";
    BetTypes[BetTypes["Handicap1H"] = 7] = "Handicap1H";
    BetTypes[BetTypes["OverUnder1H"] = 8] = "OverUnder1H";
    BetTypes[BetTypes["OneXTwo1H"] = 15] = "OneXTwo1H";
})(BetTypes || (BetTypes = {}));
var BetTeams;
(function (BetTeams) {
    BetTeams["H"] = "h";
    BetTeams["A"] = "a";
    BetTeams["One"] = "1";
    BetTeams["Two"] = "2";
    BetTeams["X"] = "x"; // for 1X2
})(BetTeams || (BetTeams = {}));
var MemberWinLossOperators;
(function (MemberWinLossOperators) {
    MemberWinLossOperators[MemberWinLossOperators["GreaterOrEqual"] = 0] = "GreaterOrEqual";
    MemberWinLossOperators[MemberWinLossOperators["Equal"] = 1] = "Equal";
    MemberWinLossOperators[MemberWinLossOperators["LessOrEqual"] = 2] = "LessOrEqual";
})(MemberWinLossOperators || (MemberWinLossOperators = {}));
var BetTeamClassNames = {
    Underdog: "underdog",
    Favorite: "favorite"
};
var ChoiceBuilder = /** @class */ (function () {
    function ChoiceBuilder(ticket) {
        this.ticket = ticket;
    }
    Object.defineProperty(ChoiceBuilder.prototype, "IsBetHomeTeam", {
        get: function () {
            return this.ticket.BetTeam && this.ticket.BetTeam.toLowerCase().indexOf(BetTeams.H) >= 0;
        },
        enumerable: true,
        configurable: true
    });
    ChoiceBuilder.prototype.build = function (data) {
        return "<span class=\"" + this.BetTeamClassName + "\">\n                    " + this.generateBetTeam(data) + "\n                    <span class=\"text-handicap\">" + this.generateHandicap() + "</span>\n                    " + this.buildLiveScore() + "\n                    <br />\n                    <span class=\"text-bet-type\">" + getValueByKey(data.BetTypes, this.ticket.BetType) + "</span>\n                </span>";
    };
    ChoiceBuilder.prototype.buildLiveScore = function () {
        if (!this.ticket.IsLive) {
            return '';
        }
        return "<span class=\"text-live-score\">[" + this.ticket.LiveHomeScore + "-" + this.ticket.LiveAwayScore + "]</span>";
    };
    return ChoiceBuilder;
}());
var HandicapChoiceBuilder = /** @class */ (function (_super) {
    __extends(HandicapChoiceBuilder, _super);
    function HandicapChoiceBuilder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(HandicapChoiceBuilder.prototype, "BetTeamClassName", {
        get: function () {
            if (this.IsBetHomeTeam) {
                return this.ticket.Hdp2 >= this.ticket.Hdp1 ? BetTeamClassNames.Underdog : BetTeamClassNames.Favorite;
            }
            else {
                return this.ticket.Hdp2 <= this.ticket.Hdp1 ? BetTeamClassNames.Underdog : BetTeamClassNames.Favorite;
            }
        },
        enumerable: true,
        configurable: true
    });
    HandicapChoiceBuilder.prototype.generateBetTeam = function (data) {
        var betTeamId = this.IsBetHomeTeam ? this.ticket.HomeId : this.ticket.AwayId;
        return getValueByKey(data.Teams, betTeamId);
    };
    HandicapChoiceBuilder.prototype.generateHandicap = function () {
        var handicap = Math.max(this.ticket.Hdp1, this.ticket.Hdp2);
        handicap = this.BetTeamClassName === BetTeamClassNames.Favorite ? -handicap : handicap;
        return handicap.toLocaleString('en-US', { maximumFractionDigits: 2 });
    };
    return HandicapChoiceBuilder;
}(ChoiceBuilder));
var OverUnderChoiceBuilder = /** @class */ (function (_super) {
    __extends(OverUnderChoiceBuilder, _super);
    function OverUnderChoiceBuilder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    OverUnderChoiceBuilder.prototype.generateBetTeam = function () {
        return this.IsBetHomeTeam ? betMatchingResources.Over : betMatchingResources.Under;
    };
    return OverUnderChoiceBuilder;
}(HandicapChoiceBuilder));
var OneXTwoChoiceBuilder = /** @class */ (function (_super) {
    __extends(OneXTwoChoiceBuilder, _super);
    function OneXTwoChoiceBuilder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(OneXTwoChoiceBuilder.prototype, "BetTeamClassName", {
        get: function () {
            return BetTeamClassNames.Favorite;
        },
        enumerable: true,
        configurable: true
    });
    OneXTwoChoiceBuilder.prototype.generateBetTeam = function () {
        var prefix = this.ticket.BetType === BetTypes.OneXTwoFT ? betMatchingResources.FT : betMatchingResources.HT;
        return prefix + "." + (this.ticket.BetTeam ? this.ticket.BetTeam.toUpperCase() : '');
    };
    OneXTwoChoiceBuilder.prototype.generateHandicap = function () {
        return '';
    };
    return OneXTwoChoiceBuilder;
}(ChoiceBuilder));
var BetChoiceUplines = /** @class */ (function () {
    function BetChoiceUplines($) {
        var _this = this;
        this.uplinesCache = {};
        this.query = function (custId, callback) {
            if (_this.uplinesCache[custId]) {
                callback(_this.uplinesCache[custId]);
                return;
            }
            _this.$searchAccountId.val(custId);
            var queryUrl = _this.$uplinesForm.attr('action');
            var params = alphaReports.getFormData(_this.$uplinesForm);
            _this.$.post(queryUrl, params)
                .done(function (data) {
                _this.uplinesCache[custId] = data;
                callback(data);
            })
                .fail(function (jqXHR) {
                var jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    location.href = top["alpha"].urls.mainRootPath
                        + "PartialUM/Index?umfrom=" + jsonData.UMFrom
                        + "&umto=" + jsonData.UMTo;
                }
                else {
                    callback(null);
                }
            });
        };
        this.$ = $;
        this.$uplinesForm = this.$("#uplines-form");
        this.$searchAccountId = this.$("#SearchAccountId");
    }
    return BetChoiceUplines;
}());
var BetChoiceMatching = /** @class */ (function () {
    function BetChoiceMatching($) {
        var _this = this;
        this.queryIPGroup = {};
        this.init = function () {
            _this.$.blockUI();
            _this.cacheElements();
            _this.registerEvents();
            _this.$.unblockUI();
            _this.submitOnLoad();
        };
        this.cacheElements = function () {
            _this.$pageTitle = _this.$(".page-title");
            _this.$reportForm = _this.$("#report-form");
            _this.$exportExcelForm = _this.$("#export-excel-form");
            _this.$excelWinlostDate = _this.$("#ExcelWinlostDate");
            _this.$excelBetCount = _this.$("#ExcelBetCount");
            _this.$excelUsername = _this.$("#ExcelUsername");
            _this.$excelStake = _this.$("#ExcelStake");
            _this.$excelMemberWinLossOperator = _this.$("#ExcelMemberWinLossOperator");
            _this.$excelMemberWinLossValue = _this.$("#ExcelMemberWinLossValue");
            _this.$filterIcon = _this.$(".icon-filter");
            _this.$excelIcon = _this.$("#icon-excel");
            _this.$betCount = _this.$("#BetCount");
            _this.$datePicker = _this.$("#dpkContainer");
            _this.$datePickerInput = _this.$("#WinlostDate");
            _this.$table = _this.$("#tbl-report");
            _this.$tableTbody = _this.$table.find("tbody");
            _this.$tableTfoot = _this.$table.find("tfoot");
            var $noinfoRow = _this.$table.find("#noinfo-row");
            _this.noinfoRowHtml = $noinfoRow.prop("outerHTML");
            _this.betChoiceUplines = new BetChoiceUplines(_this.$);
            _this.usernameFilterForm = new UsernameFilterForm(_this.$);
            _this.stakeFilterForm = new StakeFilterForm(_this.$);
            _this.memberWinLossFilterForm = new MemberWinLossFilterForm(_this.$);
        };
        this.registerEvents = function () {
            _this.$filterIcon.on("click", _this.onFilterEvent);
            _this.$excelIcon.on("click", _this.onExportExcel);
            _this.$reportForm.submit(_this.onSubmitReport);
            var options = {
                allowInputToggle: true,
                minDate: new Date(_this.$('#MinDate').val()),
                maxDate: new Date(_this.$('#Today').val()),
                format: alphaReports.format.shortDateWithSlash_DatePicker
            };
            _this.$datePicker.datepicker(options);
            _this.usernameFilterForm.subscribeSubmit(_this.filterAndRender);
            _this.stakeFilterForm.subscribeSubmit(_this.filterAndRender);
            _this.memberWinLossFilterForm.subscribeSubmit(_this.filterAndRender);
        };
        this.submitOnLoad = function () {
            _this.$table.hide();
            _this.$table.removeClass('hide');
            _this.$reportForm.submit();
        };
        this.onFilterEvent = function () {
            var currentHideState = _this.$reportForm.hasClass("hide");
            currentHideState ? _this.showFilter() : _this.hideFilter();
        };
        this.showFilter = function () {
            _this.$pageTitle.removeClass("hide-filter");
            _this.$reportForm.removeClass("hide");
        };
        this.hideFilter = function () {
            _this.$pageTitle.addClass("hide-filter");
            _this.$reportForm.addClass("hide");
        };
        this.onSubmitReport = function (e) {
            e.preventDefault();
            _this.$.blockUI();
            _this.removeFilterCriteria();
            _this.hideColumnFilterIcons();
            _this.hideColumnFilterTooltips();
            _this.$table.hide();
            _this.queryParam = alphaReports.getFormData(_this.$reportForm);
            var queryUrl = _this.$reportForm.attr('action');
            _this.$.post(queryUrl, _this.queryParam)
                .done(function (data) {
                _this.data = data;
                _this.groupIPs(data.IPs);
                _this.filterAndRender();
            })
                .fail(function (jqXHR) {
                var jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    var cookielessSiteMainUrl = _this.$("#CookielessSiteMainUrl");
                    var siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : top["alpha"].urls.mainRootPath;
                    location.href = siteMainUrl + "partialum/index?" + _this.$.param(jsonData);
                }
            })
                .always(function () {
                _this.$table.show();
                _this.$.unblockUI();
            });
        };
        this.filterAndRender = function () {
            var filterEvents = [];
            _this.data.Events.forEach(function (event) {
                var filterUsers = event.Users;
                filterUsers = _this.usernameFilterForm.filter(filterUsers);
                filterUsers = _this.stakeFilterForm.filter(filterUsers);
                filterUsers = _this.memberWinLossFilterForm.filter(filterUsers);
                if (filterUsers.length > 0) {
                    event.Users = filterUsers;
                    filterEvents.push(event);
                }
            });
            _this.renderTable(filterEvents);
        };
        this.onExportExcel = function () {
            _this.$excelWinlostDate.val(_this.queryParam.WinlostDate);
            _this.$excelBetCount.val(_this.queryParam.BetCount);
            _this.$excelUsername.val(_this.usernameFilterForm.getCriteria().Username);
            _this.$excelStake.val(_this.stakeFilterForm.getCriteria().TotalStake);
            _this.$excelMemberWinLossOperator.val(_this.memberWinLossFilterForm.getCriteria().Operator);
            _this.$excelMemberWinLossValue.val(_this.memberWinLossFilterForm.getCriteria().MemberWinLoss);
            _this.$exportExcelForm.submit();
        };
        this.groupIPs = function (ips) {
            _this.queryIPGroup = {};
            ips.forEach(function (ip) {
                if (_this.queryIPGroup[ip.IP]) {
                    _this.queryIPGroup[ip.IP].push(ip);
                }
                else {
                    _this.queryIPGroup[ip.IP] = [ip];
                }
            });
        };
        this.formatNumber = function (value, isRunning) {
            if (isRunning) {
                return '-';
            }
            return alphaReports.formatCurrency(value);
        };
        this.renderTable = function (events) {
            if (events.length === 0) {
                _this.$tableTbody.html(_this.noinfoRowHtml);
                _this.$tableTfoot.hide();
                return;
            }
            _this.showColumnFilterIcons();
            var totalStake = null, totalMemberWinLoss = null, totalSuperWinLoss = null, totalCompany = null, isRunning = true;
            var html = [];
            events.forEach(function (event) {
                html.push(_this.generateBetChoice(event));
                event.Users.forEach(function (user, index) {
                    totalStake += user.Stake;
                    if (!user.IsRunning) {
                        totalMemberWinLoss += user.MemberWinLoss;
                        totalSuperWinLoss += user.SuperWinLoss;
                        totalCompany += user.Company;
                        isRunning = false;
                    }
                    html.push("<tr class=\"" + (index % 2 === 1 ? "row-even row-total" : "row-total") + "\">\n                    <td class=\"text-left\" rowspan=\"" + (user.Tickets.length + 1) + "\">\n                        <span data-custid=\"" + user.CustId + "\" class=\"text-username\" title=\"" + betMatchingResources.BetMatching_Username_Title + "\">" + user.Username + "</span>\n                    </td>\n                    <td class=\"text-right\" colspan=\"4\">" + betMatchingResources.Total + "</td>\n                    <td>" + alphaReports.formatNumber(user.Stake) + "</td>\n                    <td>" + _this.formatNumber(user.MemberWinLoss, user.IsRunning) + "</td>\n                    <td class=\"col-superwinloss\">" + _this.formatNumber(user.SuperWinLoss, user.IsRunning) + "</td>\n                    <td>" + _this.formatNumber(user.Company, user.IsRunning) + "</td>\n                </tr>");
                    user.Tickets.forEach(function (ticket) {
                        var ipHtml = '';
                        if (_this.queryIPGroup[ticket.IP]) {
                            ipHtml = " <span class=\"icon-ip-share\" data-custid=\"" + ticket.CustId + "\" data-ip=\"" + ticket.IP + "\" title=\"" + betMatchingResources.BetMatching_IP_Title + "\"></span>";
                        }
                        html.push("<tr class=\"" + (index % 2 === 1 ? "row-even" : "") + "\">\n                        <td class=\"text-center\">" + moment(ticket.TransDate).format("MM/DD HH:mm:ss") + "</td>\n                        <td class=\"text-center\">" + ticket.TransIdString + "</td>\n                        <td class=\"text-center\">" + _this.buildOdds(ticket) + "</td>\n                        <td class=\"text-center\">" + (ticket.IP ? ticket.IP : '') + ipHtml + "</td>\n                        <td>" + alphaReports.formatNumber(ticket.Stake) + "</td>\n                        <td>" + _this.formatNumber(ticket.MemberWinLoss, ticket.IsRunning) + "</td>\n                        <td class=\"col-superwinloss\">" + _this.formatNumber(ticket.SuperWinLoss, ticket.IsRunning) + "</td>\n                        <td>" + _this.formatNumber(ticket.Company, ticket.IsRunning) + "</td>\n                    </tr>");
                    });
                });
            });
            _this.$tableTbody.html(html.join(''));
            _this.$tableTfoot.html("\n            <tr class=\"tbl-footer\">\n                <td colspan=\"5\">" + betMatchingResources.Total + "</td>\n                <td>" + alphaReports.formatNumber(totalStake) + "</td>\n                <td>" + _this.formatNumber(totalMemberWinLoss, isRunning) + "</td>\n                <td class=\"col-superwinloss\">" + _this.formatNumber(totalSuperWinLoss, isRunning) + "</td>\n                <td>" + _this.formatNumber(totalCompany, isRunning) + "</td>\n            </tr>\n        ");
            _this.$tableTfoot.show();
            _this.$tableTbody.find('.text-username').click(_this.onUsernameClick);
            _this.$tableTbody.find('.icon-ip-share').click(_this.onIPClick);
        };
        this.onUsernameClick = function (e) {
            if (_this.activeUplinesTooltip) {
                _this.activeUplinesTooltip.hide();
            }
            var tooltipDataKey = 'simpleTooltip';
            var $target = _this.$(e.target);
            var uplinesTooltip = $target.data(tooltipDataKey);
            if (!uplinesTooltip) {
                uplinesTooltip = new SimpleTooltip(_this.$, $target, {
                    title: betMatchingResources.Upline
                });
                $target.data(tooltipDataKey, uplinesTooltip);
                uplinesTooltip.render("<div class=\"loading-icon\"></div>");
                var custId = $target.data('custid');
                _this.betChoiceUplines.query(custId, function (data) {
                    uplinesTooltip.render("\n                <div>" + data.MasterName + "</div>\n                <div>" + data.AgentName + "</div>\n            ");
                });
            }
            _this.activeUplinesTooltip = uplinesTooltip;
            _this.activeUplinesTooltip.show();
        };
        this.onIPClick = function (e) {
            if (_this.activeIPTooltip) {
                _this.activeIPTooltip.hide();
            }
            var tooltipDataKey = 'simpleTooltip';
            var $target = _this.$(e.target);
            var ipTooltip = $target.data(tooltipDataKey);
            if (!ipTooltip) {
                ipTooltip = new SimpleTooltip(_this.$, $target, {
                    title: betMatchingResources.IP_used_by,
                    direction: 'right',
                    tooltipClass: 'ip-tooltip'
                });
                $target.data(tooltipDataKey, ipTooltip);
                var ip = $target.data('ip');
                var custId_1 = $target.data('custid');
                var data = _this.queryIPGroup[ip];
                var html_1 = [];
                var index_1 = 0;
                data.forEach(function (item) {
                    if (item.CustId !== custId_1) {
                        index_1++;
                        html_1.push("<div>" + index_1 + ": " + item.Username + "</div>");
                    }
                });
                ipTooltip.render(html_1.join(''));
            }
            _this.activeIPTooltip = ipTooltip;
            _this.activeIPTooltip.show();
        };
        this.showColumnFilterIcons = function () {
            _this.usernameFilterForm.showIcon();
            _this.stakeFilterForm.showIcon();
            _this.memberWinLossFilterForm.showIcon();
        };
        this.hideColumnFilterIcons = function () {
            _this.usernameFilterForm.hideIcon();
            _this.stakeFilterForm.hideIcon();
            _this.memberWinLossFilterForm.hideIcon();
        };
        this.hideColumnFilterTooltips = function () {
            _this.usernameFilterForm.hideTooltip();
            _this.stakeFilterForm.hideTooltip();
            _this.memberWinLossFilterForm.hideTooltip();
        };
        this.removeFilterCriteria = function () {
            _this.usernameFilterForm.removeCriteria();
            _this.stakeFilterForm.removeCriteria();
            _this.memberWinLossFilterForm.removeCriteria();
        };
        this.$ = $;
    }
    BetChoiceMatching.prototype.buildOdds = function (ticket) {
        if (ticket.Odds == 0) {
            return ticket.DisplayOdds;
        }
        var formatOdds = ticket.Odds < 0
            ? "<font color='#B50000'>" + ticket.DisplayOdds + "</font>"
            : ticket.DisplayOdds;
        if (ticket.DisplayOddsType) {
            return "(" + ticket.DisplayOddsType + ") " + formatOdds;
        }
        else {
            return formatOdds;
        }
    };
    BetChoiceMatching.prototype.generateBetChoice = function (event) {
        var getChoiceBuilder = function () {
            switch (event.BetType) {
                case BetTypes.HandicapFT:
                case BetTypes.Handicap1H:
                    return new HandicapChoiceBuilder(event);
                case BetTypes.OverUnderFT:
                case BetTypes.OverUnder1H:
                    return new OverUnderChoiceBuilder(event);
                case BetTypes.OneXTwoFT:
                case BetTypes.OneXTwo1H:
                    return new OneXTwoChoiceBuilder(event);
            }
            throw new Error("ChoiceBuilder is not implemented for bettype " + event.BetType);
        };
        var choiceBuilder = getChoiceBuilder();
        return "<tr class=\"row-bet-choice\">\n                    <td class=\"text-left col-bet-choice\">" + choiceBuilder.build(this.data) + "</td>\n                    <td colspan=\"6\" class=\"text-left col-event\">\n                        <span class=\"text-event\">" + getValueByKey(this.data.Teams, event.HomeId) + betMatchingResources.Versus + getValueByKey(this.data.Teams, event.AwayId) + "</span>\n                        <br />\n                        <span class=\"text-sport-type\">" + getValueByKey(this.data.SportTypes, event.SportType) + "</span>\n                        " + ' ' + "\n                        <span class=\"text-league\">" + getValueByKey(this.data.Leagues, event.LeagueId) + "</span>\n                    </td>\n                    <td colspan=\"2\" class=\"text-center col-global-time\">" + moment(event.GlobalShowTime).format("MM/DD/YYYY hh:mm A") + "</td>\n                </tr>";
    };
    return BetChoiceMatching;
}());
var defaultOption = {
    tooltipClass: '',
    direction: 'left',
    title: ''
};
var BaseFilterForm = /** @class */ (function () {
    function BaseFilterForm($, iconFilterClassname, tooltipOption, tooltipContent) {
        var _this = this;
        this.$ = $;
        this.onHideTooltip = function () {
            if (_this.filterCriteria) {
                var filterCriteria = _this.getCriteria();
                if (JSON.stringify(filterCriteria) !== JSON.stringify(_this.filterCriteria)) {
                    _this.restoreCriteria(_this.filterCriteria);
                }
            }
            else {
                _this.filterCriteria = _this.getCriteria();
            }
        };
        this.hideIcon = function () {
            _this.$filterIcon.hide();
        };
        this.showIcon = function () {
            _this.$filterIcon.show();
        };
        this.hideTooltip = function () {
            _this.simpleTooltip.hide();
        };
        this.showTooltip = function () {
            _this.simpleTooltip.show();
        };
        this.onSubmit = function (e) {
            e.preventDefault();
            _this.filterCriteria = _this.getCriteria();
            _this.$filterIcon.removeClass('has-filter');
            if (_this.hasFilter()) {
                _this.$filterIcon.addClass('has-filter');
            }
            _this.listener && _this.listener();
        };
        this.removeCriteria = function () {
            _this.filterCriteria = undefined;
            _this.$filterIcon.removeClass('has-filter');
            _this.resetCriteria();
        };
        this.$filterIcon = $(iconFilterClassname);
        this.simpleTooltip = new SimpleTooltip($, this.$filterIcon, tooltipOption);
        this.simpleTooltip.render(tooltipContent);
        this.simpleTooltip.hide();
        BaseFilterForm.tooltips.push(this.simpleTooltip);
        this.$filterIcon.on("click", function () {
            BaseFilterForm.tooltips.forEach(function (tooltip) { return tooltip.hide(); });
            _this.simpleTooltip.show();
        });
        this.$form = this.simpleTooltip.$tooltip.find('form');
        this.$form.submit(this.onSubmit);
        this.simpleTooltip.onHide(this.onHideTooltip);
    }
    BaseFilterForm.prototype.subscribeSubmit = function (listener) {
        this.listener = listener;
    };
    BaseFilterForm.prototype.filter = function (users) {
        var _this = this;
        if (!this.hasFilter()) {
            return users;
        }
        var filterUsers = [];
        users.forEach(function (user) {
            if (_this.isMatch(user, _this.filterCriteria)) {
                filterUsers.push(user);
            }
        });
        return filterUsers;
    };
    BaseFilterForm.prototype.isMatch = function (user, filterCriteria) {
        return false;
    };
    BaseFilterForm.tooltips = [];
    return BaseFilterForm;
}());
var UsernameFilterForm = /** @class */ (function (_super) {
    __extends(UsernameFilterForm, _super);
    function UsernameFilterForm($) {
        var _this = _super.call(this, $, UsernameFilterForm.iconFilterClassname, UsernameFilterForm.tooltipOption, UsernameFilterForm.tooltipContent) || this;
        _this.textareaLostFocus = function () {
            var username = _this.$.trim(_this.$textareaUsername.val()).replace(/(?:\r\n|\r|\n)/g, ',');
            var usernames = username.split(',');
            var newUsernames = [];
            usernames.forEach(function (x) {
                var trimmed = _this.$.trim(x);
                if (trimmed) {
                    newUsernames.push(trimmed);
                }
            });
            username = newUsernames.join(',');
            _this.$textareaUsername.val(username);
        };
        _this.restoreCriteria = function (filterCriteria) {
            _this.$textareaUsername.val(filterCriteria.Username);
        };
        _this.$textareaUsername = _this.$form.find('textarea');
        _this.$textareaUsername.focusout(_this.textareaLostFocus);
        return _this;
    }
    UsernameFilterForm.prototype.getCriteria = function () {
        return {
            Username: this.$.trim(this.$textareaUsername.val())
        };
    };
    UsernameFilterForm.prototype.hasFilter = function () {
        var criteria = this.getCriteria();
        return !!criteria.Username;
    };
    UsernameFilterForm.prototype.resetCriteria = function () {
        this.$textareaUsername.val('');
    };
    UsernameFilterForm.prototype.filter = function (users) {
        if (!this.hasFilter()) {
            return users;
        }
        var username = this.filterCriteria.Username.toLowerCase();
        var usernames = username.split(',').map(function (x) { return $.trim(x).toLowerCase(); });
        var filterUsers = [];
        users.forEach(function (user) {
            if (usernames.indexOf(user.Username.toLowerCase()) >= 0) {
                filterUsers.push(user);
            }
        });
        return filterUsers;
    };
    UsernameFilterForm.iconFilterClassname = "thead .col-username .icon-table-filter";
    UsernameFilterForm.tooltipOption = {
        title: betMatchingResources.FilterByUsername,
        tooltipClass: 'username-filter'
    };
    UsernameFilterForm.tooltipContent = "\n        <form>\n            <textarea class=\"form-control\" name=\"Username\" autocomplete=\"off\" rows=\"4\" cols=\"50\" placeholder=\"" + betMatchingResources.FilterByUsername_Placeholder + "\"></textarea>\n            <div class=\"text-right\">\n                <button class=\"btn-submit btn-filter\" type=\"submit\">" + betMatchingResources.Filter + "</button>\n            </div>\n        </form>\n    ";
    return UsernameFilterForm;
}(BaseFilterForm));
var StakeFilterForm = /** @class */ (function (_super) {
    __extends(StakeFilterForm, _super);
    function StakeFilterForm($) {
        var _this = _super.call(this, $, StakeFilterForm.iconFilterClassname, StakeFilterForm.tooltipOption, StakeFilterForm.tooltipContent) || this;
        _this.restoreCriteria = function (filterCriteria) {
            _this.$inputTotalStake.data('cleave').setRawValue(filterCriteria.TotalStake);
        };
        _this.$inputTotalStake = _this.$form.find('input');
        _this.$inputTotalStake.cleave({
            numeral: true,
            numeralIntegerScale: 15,
            numeralDecimalScale: 0,
            numeralThousandsGroupStyle: 'thousand',
            autoUnmask: true
        });
        return _this;
    }
    StakeFilterForm.prototype.getCriteria = function () {
        var rawValue = parseInt(this.$inputTotalStake.data('cleave').getRawValue());
        return {
            TotalStake: isNaN(rawValue) ? null : rawValue
        };
    };
    StakeFilterForm.prototype.hasFilter = function () {
        var criteria = this.getCriteria();
        return criteria.TotalStake !== null;
    };
    StakeFilterForm.prototype.resetCriteria = function () {
        this.$inputTotalStake.val('');
    };
    StakeFilterForm.prototype.isMatch = function (user, filterCriteria) {
        var totalStake = filterCriteria.TotalStake;
        return user.Stake >= totalStake;
    };
    StakeFilterForm.iconFilterClassname = "thead .col-stake .icon-table-filter";
    StakeFilterForm.tooltipOption = {
        title: betMatchingResources.FilterByStake,
        tooltipClass: 'stake-filter'
    };
    StakeFilterForm.tooltipContent = "\n        <form class=\"form-inline\">\n            <div>\n                <span class=\"form-label\">" + betMatchingResources.TotalStake + " &gt;=</span>\n                <input name=\"TotalStake\" class=\"form-control input-sm\" />\n            </div>\n            <div class=\"text-right\">\n                <button class=\"btn-submit btn-filter\">" + betMatchingResources.Filter + "</button>\n            </div>\n        </form>\n    ";
    return StakeFilterForm;
}(BaseFilterForm));
var MemberWinLossFilterForm = /** @class */ (function (_super) {
    __extends(MemberWinLossFilterForm, _super);
    function MemberWinLossFilterForm($) {
        var _this = _super.call(this, $, MemberWinLossFilterForm.iconFilterClassname, MemberWinLossFilterForm.tooltipOption, MemberWinLossFilterForm.tooltipContent) || this;
        _this.restoreCriteria = function (filterCriteria) {
            _this.$selectOperator.val(filterCriteria.Operator);
            _this.$inputMemberWinLoss.data('cleave').setRawValue(filterCriteria.MemberWinLoss);
        };
        _this.$selectOperator = _this.$form.find('select');
        _this.$inputMemberWinLoss = _this.$form.find('input');
        _this.$inputMemberWinLoss.cleave({
            numeral: true,
            numeralIntegerScale: 15,
            numeralThousandsGroupStyle: 'thousand',
            autoUnmask: true
        });
        return _this;
    }
    MemberWinLossFilterForm.prototype.getCriteria = function () {
        var rawValue = parseFloat(this.$inputMemberWinLoss.data('cleave').getRawValue());
        return {
            Operator: parseInt(this.$selectOperator.val()),
            MemberWinLoss: isNaN(rawValue) ? null : rawValue
        };
    };
    MemberWinLossFilterForm.prototype.hasFilter = function () {
        var criteria = this.getCriteria();
        return criteria.MemberWinLoss !== null;
    };
    MemberWinLossFilterForm.prototype.resetCriteria = function () {
        this.$selectOperator.val(MemberWinLossOperators.GreaterOrEqual);
        this.$inputMemberWinLoss.val('');
    };
    MemberWinLossFilterForm.prototype.isMatch = function (user, filterCriteria) {
        var memberWinLoss = filterCriteria.MemberWinLoss;
        var operator = filterCriteria.Operator;
        switch (operator) {
            case MemberWinLossOperators.GreaterOrEqual:
                return !user.IsRunning && user.MemberWinLoss >= memberWinLoss;
            case MemberWinLossOperators.Equal:
                return !user.IsRunning && user.MemberWinLoss === memberWinLoss;
            case MemberWinLossOperators.LessOrEqual:
                return !user.IsRunning && user.MemberWinLoss <= memberWinLoss;
        }
    };
    MemberWinLossFilterForm.iconFilterClassname = "thead .col-memberwinloss .icon-table-filter";
    MemberWinLossFilterForm.tooltipOption = {
        title: betMatchingResources.FilterByMemberWinLoss,
        tooltipClass: 'memberwinloss-filter'
    };
    MemberWinLossFilterForm.tooltipContent = "\n        <form class=\"form-inline\">\n            <div>\n                <span class=\"form-label\">" + betMatchingResources.TotalMemberWL + "</span>\n                <select name=\"Operator\" class=\"form-control input-sm\">\n                    <option value=\"" + MemberWinLossOperators.GreaterOrEqual + "\" selected>&gt;=</option>\n                    <option value=\"" + MemberWinLossOperators.Equal + "\">=</option>\n                    <option value=\"" + MemberWinLossOperators.LessOrEqual + "\">&lt;=</option>\n                </select>\n                <input name=\"MemberWinLoss\" class=\"form-control input-sm\" />\n            </div>\n            <div class=\"text-right\">\n                <button class=\"btn-submit btn-filter\">" + betMatchingResources.Filter + "</button>\n            </div>\n        </form>\n    ";
    return MemberWinLossFilterForm;
}(BaseFilterForm));
$(function () {
    var betChoiceMatching = new BetChoiceMatching($);
    betChoiceMatching.init();
});
//# sourceMappingURL=betchoicematching.js.map