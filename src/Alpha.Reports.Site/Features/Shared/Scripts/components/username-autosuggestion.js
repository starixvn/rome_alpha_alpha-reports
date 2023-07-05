"use strict";
var UsernameAutoSuggestionOption = /** @class */ (function () {
    function UsernameAutoSuggestionOption(url, custId, directDownlines, isAccInfo, initialUsername, onBlur, selectorId) {
        if (directDownlines === void 0) { directDownlines = false; }
        if (isAccInfo === void 0) { isAccInfo = true; }
        if (initialUsername === void 0) { initialUsername = ""; }
        if (onBlur === void 0) { onBlur = Function(); }
        if (selectorId === void 0) { selectorId = "txt-username"; }
        this.url = url;
        this.custId = custId;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
        this.initialUsername = initialUsername;
        this.onBlur = onBlur;
        this.selectorId = selectorId;
        this.url = url;
        this.custId = custId;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
        this.onBlur = onBlur;
        this.selectorId = selectorId;
    }
    return UsernameAutoSuggestionOption;
}());
var UsernameAutoSuggestionComponent = /** @class */ (function () {
    function UsernameAutoSuggestionComponent() {
    }
    UsernameAutoSuggestionComponent.prototype.init = function (options) {
        var _this = this;
        this.usernameInput = $("#" + options.selectorId);
        var queryString = this.buildQueryString(options);
        var urlAndQueryString = options.url + "?" + queryString + (queryString.length ? "&" : "");
        var typeaheadOptions = {
            source: function (query, process) {
                return $.get(urlAndQueryString + "suggestionquery=" + query, process);
            },
            items: 10,
            minLength: 2,
            highlighter: function (displayText) {
                var parts = displayText.split("#");
                return parts[0] + " <br/> " + parts[1];
            },
            displayText: function (item) {
                var nickname = item.UserName2 ? " (" + item.UserName2 + ")" : "";
                return "" + item.UserName + nickname + "#" + item.FirstName + " " + item.LastName;
            },
            autoSelect: false,
            afterSelect: function (item) { return _this.usernameInput.val(item.UserName); },
            delay: 200
        };
        this.usernameInput
            .typeahead(typeaheadOptions)
            .blur(function () {
            var visible = _this.usernameInput.next(".typeahead").is(":visible");
            !visible && options.onBlur($.trim(_this.usernameInput.val()));
        })
            .val(options.initialUsername);
    };
    UsernameAutoSuggestionComponent.prototype.buildQueryString = function (options) {
        var queryString = [];
        if (options.custId) {
            queryString.push("custid=" + options.custId);
        }
        if (options.directDownlines) {
            queryString.push("directdownlines=" + options.directDownlines);
        }
        if (!options.isAccInfo) {
            queryString.push("isaccinfo=" + options.isAccInfo);
        }
        return queryString.join('&');
    };
    UsernameAutoSuggestionComponent.prototype.getUsername = function () {
        return $.trim(this.usernameInput.val());
    };
    UsernameAutoSuggestionComponent.prototype.resetUsername = function () {
        this.usernameInput.val("");
    };
    return UsernameAutoSuggestionComponent;
}());
//# sourceMappingURL=username-autosuggestion.js.map