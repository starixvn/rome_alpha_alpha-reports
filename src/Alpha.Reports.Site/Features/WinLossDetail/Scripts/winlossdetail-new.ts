"use strict";

interface Document {
    webkitExitFullscreen(): void;
    msExitFullscreen(): void;
}

interface HTMLElement {
    webkitRequestFullscreen(): void;
    msRequestFullscreen(): void
}

class WinLossDetailViewNew {
    private $: JQueryStatic;
    private dateRangePickerComponent: DateRangePickerNewComponent;
    private productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerNewComponent,
        productAndProductDetailDropdownComponent: ProductAndProductDetailDropdownComponent) {
        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.productAndProductDetailDropdownComponent = productAndProductDetailDropdownComponent;
    }

    private openFullscreen() {
        const element = document.documentElement;

        if (element.requestFullscreen) {
            element.requestFullscreen();
        } else if (element.webkitRequestFullscreen) { /* Safari */
            element.webkitRequestFullscreen();
        } else if (element.msRequestFullscreen) { /* IE11 */
            element.msRequestFullscreen();
        }
    }

    private closeFullscreen() {
        if (document.exitFullscreen) {
            document.exitFullscreen();
        } else if (document.webkitExitFullscreen) { /* Safari */
            document.webkitExitFullscreen();
        } else if (document.msExitFullscreen) { /* IE11 */
            document.msExitFullscreen();
        }
    }

    init() {
        this.dateRangePickerComponent.render();
        this.productAndProductDetailDropdownComponent.render();
        this.$("#report-form").submit((e) => {
            e.preventDefault();
            


            this.$("#tbl-report-container").html("");
        });

        this.$(".icon-filter").click(this.openFullscreen);
    }
}

$(function () {
    new WinLossDetailViewNew(
        $,
        new DateRangePickerNewComponent($),
        new ProductAndProductDetailDropdownComponent($)).init();
});