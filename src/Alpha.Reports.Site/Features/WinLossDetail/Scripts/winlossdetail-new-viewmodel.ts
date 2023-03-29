"use strict";

class WinLossDetailNewViewModel {
    private $: JQueryStatic;
    ViewerId: number;
    ViewerName: string;
    ViewerLevel: number;
    ViewerLevelIsMasterOrSuper: boolean;
    ViewerLevelIsSuper: boolean;
    SubAccId: number;
    LegendUrl: string;
    WinLossDetailUrl: string;
    CanViewBetLists: boolean;
    BetListsUrl: string;
    NoInformationText: string;
    ExportToExcelText: string;
    MomentLocale: string;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init() {
        this.ViewerId = +this.$("#hiddenViewerId").val();
        this.ViewerName = this.$("#hiddenViewerName").val();
        this.ViewerLevel = +this.$("#hiddenViewerLevel").val();
        this.ViewerLevelIsMasterOrSuper = this.ViewerLevel >= CustomerLevel.Master;
        this.ViewerLevelIsSuper = this.ViewerLevel === CustomerLevel.Super;
        this.SubAccId = +this.$("#hiddenSubAccId").val();
        this.LegendUrl = this.$("#hiddenLegendUrl").val();
        this.WinLossDetailUrl = this.$("#hiddenWinLossDetailUrl").val();
        this.CanViewBetLists = this.$("#hiddenCanViewBetLists").val() === "1";
        this.BetListsUrl = this.$("#hiddenBetListsUrl").val();
        this.NoInformationText = this.$("#hiddenNoInformationText").val();
        this.ExportToExcelText = this.$("#hiddenExportToExcelText").val();
        const locale = {
            "th-TH": "th",
            "vi-VN": "vi",
            "zh-CN": "zh-cn",
            "zh-TW": "zh-tw",
            "default": "en"
        };
        this.MomentLocale = locale[window.top.$("#language").data("currentlanguage")] || locale.default;
    }
}
