/// <reference path="../core.ts" />
/// <reference path="../base-report.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/sort.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/link-builder.ts" />
/// <reference path="playforfun.ts" />
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var PlayForFunMemberView = /** @class */ (function (_super) {
    __extends(PlayForFunMemberView, _super);
    function PlayForFunMemberView() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    PlayForFunMemberView.prototype.getSorterHeader = function () {
        return { 0: { sorter: "text" }, 1: { sorter: "text" } };
    };
    return PlayForFunMemberView;
}(PlayForFunView));
$(function () {
    var linkBuilderComponent = new LinkBuilderComponent($);
    var playForFunMemberView = new PlayForFunMemberView($, new DateRangePickerComponent($), new SortComponent($, linkBuilderComponent), new SingleSelectionDropdownComponent($), new SingleSelectionDropdownComponent($), linkBuilderComponent);
    playForFunMemberView.init();
});
//# sourceMappingURL=playforfun-member.js.map