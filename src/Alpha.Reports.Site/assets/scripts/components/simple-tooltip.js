var SimpleTooltip = /** @class */ (function () {
    function SimpleTooltip($, $target, option) {
        if (option === void 0) { option = defaultOption; }
        var _this = this;
        this.$ = $;
        this.$target = $target;
        this.render = function (html) {
            if (!_this.$tooltip) {
                _this.$tooltip = _this.$("<div class=\"simple-tooltip " + _this.option.tooltipClass + "\">\n                    <div class=\"tooltip-header\">\n                        <button type=\"button\" class=\"icon icon-close close\"></button>\n                        <div class=\"tooltip-title\">" + _this.option.title + "</div>\n                    </div>\n                    <div class=\"tooltip-body\">" + html + "</div>\n                </div>");
                _this.$target.after(_this.$tooltip);
                _this.$tooltipTitle = _this.$tooltip.find('.tooltip-title');
                _this.$tooltipBody = _this.$tooltip.find('.tooltip-body');
                var closeButton = _this.$tooltip.find('.icon-close');
                closeButton.click(_this.hide);
            }
            else {
                _this.$tooltipBody.html(html);
            }
            if (_this.option.direction === 'right') {
                var targetLeft = _this.$target.offset().left;
                _this.$tooltip.offset({ top: _this.$tooltip.offset().top, left: targetLeft - _this.$tooltip.width() });
            }
        };
        this.changeTitle = function (html) {
            _this.$tooltipTitle.html(html);
        };
        this.hide = function () {
            _this.$tooltip.hide();
            if (_this.hideCallback) {
                _this.hideCallback();
            }
        };
        this.show = function () {
            _this.$tooltip.show();
        };
        this.onHide = function (callback) {
            _this.hideCallback = callback;
        };
        this.option = this.$.extend({}, defaultOption, option);
    }
    return SimpleTooltip;
}());
//# sourceMappingURL=simple-tooltip.js.map