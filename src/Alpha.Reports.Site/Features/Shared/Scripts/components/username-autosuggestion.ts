"use strict";
class UsernameAutoSuggestionOption {
    constructor(
        public url: string,
        public custId: number,
        public directDownlines = true,
        public onEnter = Function(),
        public selectorId = "txt-username") {
        this.url = url;
        this.custId = custId;
        this.directDownlines = directDownlines;
        this.onEnter = onEnter;
        this.selectorId = selectorId;
    }
}

class UsernameAutoSuggestionComponent {
    init(options: UsernameAutoSuggestionOption) {
        const usernameInput = $("#" + options.selectorId);
        const urlAndQueryString = `${options.url}?custid=${options.custId}&directdownlines=${options.directDownlines}&isaccinfo=false`;

        const typeaheadOptions = {
            source: function (query: string, process) {
                return $.get(`${urlAndQueryString}&suggestionquery=${query}`, process);
            },
            items: 10,
            minLength: 2,
            highlighter: function (displayText: string) {
                const parts = displayText.split("#");
                return `${parts[0]} <br/> ${parts[1]}`;
            },
            displayText: function (item) {
                const nickname = item.UserName2 ? ` (${item.UserName2})` : "";
                return `${item.UserName}${nickname}#${item.FirstName} ${item.LastName}`;
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
    }
}
