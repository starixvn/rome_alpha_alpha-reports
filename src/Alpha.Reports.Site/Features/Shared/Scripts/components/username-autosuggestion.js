"use strict";
var UsernameAutoSuggestionOption = /** @class */ (function () {
    function UsernameAutoSuggestionOption(url, custId, directDownlines, onEnter, selectorId) {
        if (directDownlines === void 0) { directDownlines = true; }
        if (onEnter === void 0) { onEnter = Function(); }
        if (selectorId === void 0) { selectorId = "txt-username"; }
        this.url = url;
        this.custId = custId;
        this.directDownlines = directDownlines;
        this.onEnter = onEnter;
        this.selectorId = selectorId;
        this.url = url;
        this.custId = custId;
        this.directDownlines = directDownlines;
        this.onEnter = onEnter;
        this.selectorId = selectorId;
    }
    return UsernameAutoSuggestionOption;
}());
var UsernameAutoSuggestionComponent = /** @class */ (function () {
    function UsernameAutoSuggestionComponent() {
    }
    UsernameAutoSuggestionComponent.prototype.init = function (options) {
        var usernameInput = $("#" + options.selectorId);
        var urlAndQueryString = options.url + "?custid=" + options.custId + "&directdownlines=" + options.directDownlines + "&isaccinfo=false";
        var typeaheadOptions = {
            source: function (query, process) {
                return $.get(urlAndQueryString + "&suggestionquery=" + query, process);
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
            afterSelect: function (item) {
                usernameInput.val(item.UserName);
            },
            delay: 200
        };
        usernameInput
            .typeahead(typeaheadOptions)
            .on("keyup", function (e) {
            e.keyCode === 13 && options.onEnter($.trim(usernameInput.val()));
        });
    };
    return UsernameAutoSuggestionComponent;
}());
//# sourceMappingURL=username-autosuggestion.js.map