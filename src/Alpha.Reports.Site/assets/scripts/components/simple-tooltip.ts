class SimpleTooltip {
    private option: SimpleTooltipOption;
    $tooltip: JQuery;
    private $tooltipTitle: JQuery;
    private $tooltipBody: JQuery;
    private hideCallback: () => void;

    constructor(private $: JQueryStatic, private $target: JQuery, option = defaultOption) {
        this.option = this.$.extend({}, defaultOption, option);
    }

    render = (html: string) => {
        if (!this.$tooltip) {
            this.$tooltip = this.$(
                `<div class="simple-tooltip ${this.option.tooltipClass}">
                    <div class="tooltip-header">
                        <button type="button" class="icon icon-close close"></button>
                        <div class="tooltip-title">${this.option.title}</div>
                    </div>
                    <div class="tooltip-body">${html}</div>
                </div>`
            );
            this.$target.after(this.$tooltip);
            this.$tooltipTitle = this.$tooltip.find('.tooltip-title');
            this.$tooltipBody = this.$tooltip.find('.tooltip-body');
            const closeButton = this.$tooltip.find('.icon-close');
            closeButton.click(this.hide);
        } else {
            this.$tooltipBody.html(html);
        }

        if (this.option.direction === 'right') {
            const targetLeft = this.$target.offset().left;
            this.$tooltip.offset({ top: this.$tooltip.offset().top, left: targetLeft - this.$tooltip.width() });
        }
    };

    changeTitle = (html: string) => {
        this.$tooltipTitle.html(html);
    };

    hide = () => {
        this.$tooltip.hide();
        if (this.hideCallback) {
            this.hideCallback();
        }
    };

    show = () => {
        this.$tooltip.show();
    };

    onHide = (callback: () => void) => {
        this.hideCallback = callback;
    }
}