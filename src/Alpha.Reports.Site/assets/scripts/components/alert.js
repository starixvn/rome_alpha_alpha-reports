"use strict";
var Alert = /** @class */ (function () {
    function Alert($) {
        this.$ = $;
        this.cacheDOMElements();
    }
    Alert.prototype.cacheDOMElements = function () {
        this.divAlert = this.$("#alert-message");
        this.divAlertMessage = this.$("#alert-message-content");
    };
    Alert.prototype.showSuccess = function (message) {
        this.divAlertMessage
            .addClass("msg msg-success")
            .html(message);
        this.divAlert.removeClass("hidden");
    };
    Alert.prototype.showError = function (error) {
        this.divAlertMessage
            .addClass("msg msg-error")
            .html(error);
        this.divAlert.removeClass("hidden");
    };
    Alert.prototype.show = function (code, message) {
        this.divAlertMessage
            .addClass("msg " + (code ? "msg-error" : "msg-success"))
            .html(message);
        this.divAlert.removeClass("hidden");
    };
    Alert.prototype.hide = function () {
        this.divAlert.addClass("hidden");
        this.divAlertMessage.removeClass().html("");
    };
    return Alert;
}());
//# sourceMappingURL=alert.js.map