"use strict";
class UsernameAutoSuggestionOption {
    constructor(
        public url: string,
        public custId: number,
        public directDownlines = true,
        public initialUsername = "",
        public onBlur = Function(),
        public selectorId = "txt-username") {
        this.url = url;
        this.custId = custId;
        this.directDownlines = directDownlines;
        this.onBlur = onBlur;
        this.selectorId = selectorId;
    }
}

class UsernameAutoSuggestionComponent {
    private usernameInput: JQuery;

    init(options: UsernameAutoSuggestionOption) {
        this.usernameInput = $("#" + options.selectorId);
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
            afterSelect: (item) => this.usernameInput.val(item.UserName),
            delay: 200
        };

        this.usernameInput
            .typeahead(typeaheadOptions)
            .blur(() => options.onBlur($.trim(this.usernameInput.val())))
            .val(options.initialUsername);
    }

    getUsername() {
        return $.trim(this.usernameInput.val());
    }

    resetUsername() {
        this.usernameInput.val("");
    }
}
