"use strict";
class UsernameAutoSuggestionOption {
    constructor(
        public url: string,
        public custId?: number,
        public directDownlines = false,
        public isAccInfo = true,
        public initialUsername = "",
        public onBlur = Function(),
        public selectorId = "txt-username") {
        this.url = url;
        this.custId = custId;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
        this.onBlur = onBlur;
        this.selectorId = selectorId;
    }
}

class UsernameAutoSuggestionComponent {
    private usernameInput: JQuery;

    init(options: UsernameAutoSuggestionOption) {
        this.usernameInput = $("#" + options.selectorId);
        const queryString = this.buildQueryString(options);
        const urlAndQueryString = `${options.url}?${queryString}${queryString.length ? "&" : ""}`;

        const typeaheadOptions = {
            source: function (query: string, process) {
                return $.get(`${urlAndQueryString}suggestionquery=${query}`, process);
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
            .blur(() => {
                const visible = this.usernameInput.next(".typeahead").is(":visible");
                !visible && options.onBlur($.trim(this.usernameInput.val()));
            })
            .val(options.initialUsername);
    }

    private buildQueryString(options: UsernameAutoSuggestionOption) {
        const queryString = [];
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
    }

    getUsername() {
        return $.trim(this.usernameInput.val());
    }

    resetUsername() {
        this.usernameInput.val("");
    }
}
