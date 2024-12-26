"use strict";
var UsernameAutoSuggestionOption = /** @class */ (function () {
    function UsernameAutoSuggestionOption(url, directDownlines, isAccInfo, onBlur, selectorId) {
        if (directDownlines === void 0) { directDownlines = false; }
        if (isAccInfo === void 0) { isAccInfo = true; }
        if (onBlur === void 0) { onBlur = Function(); }
        if (selectorId === void 0) { selectorId = "txt-username"; }
        this.url = url;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
        this.onBlur = onBlur;
        this.selectorId = selectorId;
        this.url = url;
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
        if (!this.usernameInput.length) {
            return;
        }
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
                var nickname = item.Username2 ? " (" + item.Username2 + ")" : "";
                return "" + item.Username + nickname + "#" + item.FirstName + " " + item.LastName;
            },
            autoSelect: false,
            afterSelect: function (item) { return _this.usernameInput.val(item.Username); },
            delay: 200
        };
        var hitEnterTextboxUsername = false;
        this.usernameInput.typeahead(typeaheadOptions);
        this.usernameInput.keypress(function (e) {
            var key = e.which || e.keyCode;
            if (key === 13) {
                hitEnterTextboxUsername = true;
                options.onBlur();
            }
        }).blur(function () {
            if (hitEnterTextboxUsername) {
                hitEnterTextboxUsername = false;
            }
            else {
                var visible = _this.usernameInput.next(".typeahead").is(":visible");
                !visible && options.onBlur();
            }
        });
    };
    UsernameAutoSuggestionComponent.prototype.buildQueryString = function (options) {
        var queryString = [];
        if (options.directDownlines) {
            queryString.push("directdownlines=" + options.directDownlines);
        }
        if (!options.isAccInfo) {
            queryString.push("isaccinfo=" + options.isAccInfo);
        }
        return queryString.join('&');
    };
    UsernameAutoSuggestionComponent.prototype.getUsername = function () {
        if (this.usernameInput.length) {
            return $.trim(this.usernameInput.val());
        }
    };
    UsernameAutoSuggestionComponent.prototype.resetUsername = function () {
        this.usernameInput.val("");
    };
    return UsernameAutoSuggestionComponent;
}());
//# sourceMappingURL=username-autosuggestion.js.map