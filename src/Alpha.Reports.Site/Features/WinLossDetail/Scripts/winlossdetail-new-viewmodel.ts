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
    NonEnteredText: string;
    EnteredText: string;
    ExportToExcelText: string;

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
        this.NonEnteredText = this.$("#hiddenNonEnteredText").val();
        this.EnteredText = this.$("#hiddenEnteredText").val();
        this.ExportToExcelText = this.$("#hiddenExportToExcelText").val();
    }
}

enum CustomerLevel {
    Super = 4,
    Master = 3,
    Agent = 2
}