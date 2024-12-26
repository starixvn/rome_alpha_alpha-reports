"use strict";
class UsernameAutoSuggestionOption {
    constructor(
        public url: string,
        public directDownlines = false,
        public isAccInfo = true,
        public onBlur = Function(),
        public selectorId = "txt-username") {
        this.url = url;
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
        if (!this.usernameInput.length) {
            return;
        }

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
                const nickname = item.Username2 ? ` (${item.Username2})` : "";
                return `${item.Username}${nickname}#${item.FirstName} ${item.LastName}`;
            },
            autoSelect: false,
            afterSelect: (item) => this.usernameInput.val(item.Username),
            delay: 200
        };

        let hitEnterTextboxUsername = false;
        this.usernameInput.typeahead(typeaheadOptions);
        this.usernameInput.keypress((e) => {
            const key = e.which || e.keyCode;
            if (key === 13) {
                hitEnterTextboxUsername = true;
                options.onBlur();
            }
        }).blur(() => {
            if (hitEnterTextboxUsername) {
                hitEnterTextboxUsername = false;
            } else {
                const visible = this.usernameInput.next(".typeahead").is(":visible");
                !visible && options.onBlur();
            }
        });
    }

    private buildQueryString(options: UsernameAutoSuggestionOption) {
        const queryString = [];

        if (options.directDownlines) {
            queryString.push("directdownlines=" + options.directDownlines);
        }

        if (!options.isAccInfo) {
            queryString.push("isaccinfo=" + options.isAccInfo);
        }

        return queryString.join('&');
    }

    getUsername() {
        if (this.usernameInput.length) {
            return $.trim(this.usernameInput.val());
        }
    }

    resetUsername() {
        this.usernameInput.val("");
    }
}
