namespace Alpha.Reports.Site.ViewModels.WinLossByGameType
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Params;
    using Reports.Models;
    using Reports.Models.WinlossByGameType;
    using Requests.WinLossByGameType;
    using WinLossByBetType;

    public class WinLossByGameTypeViewModel
    {
        private readonly WinLossByGameTypeQuery message;
        private readonly WinLossByGameTypeParam winLossByGameTypeParam;
        public static readonly string NetWinLossWinLoss = $"{nameof(WinLossByGameTypeModel.AgentNetWinLoss)}.{nameof(NetWinLoss.WinLoss)}";
        public static readonly string NetWinLossComm = $"{nameof(WinLossByGameTypeModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Comm)}";
        public static readonly string NetWinLossTotal = $"{nameof(WinLossByGameTypeModel.AgentNetWinLoss)}.{nameof(NetWinLoss.Total)}";

        public WinLossByGameTypeViewModel(
            IEnumerable<WinLossByGameTypeModel> reportData,
            WinLossByGameTypeModel grandTotal,
            WinLossByGameTypeQuery message,
            WinLossByGameTypeParam winLossByGameTypeParam)
        { 
            this.message = message;
            this.winLossByGameTypeParam = winLossByGameTypeParam;
            ReportData = reportData;
            GrandTotal = grandTotal;

            BuildResources();
        }

        public int CustId => message.CustId;

        public int ProductId => message.ProductId;

        public UserLevel CustLevel => message.CustLevel;

        public DateTime MinDate => DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => winLossByGameTypeParam.FromDate;

        public DateTime ToDate => winLossByGameTypeParam.ToDate;

        public bool IsSetDefaultStackMode => message.IsSetDefaultStackMode;

        public string Language => message.Language;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public IEnumerable<WinLossByGameTypeModel> ReportData { get; }

        public WinLossByGameTypeModel GrandTotal { get; }

        public bool IsStackMode => message.IsStackMode;

        public bool IsDescendingSort => message.IsDescendingSort;

        public bool IsFilterVisible => message.IsFilterVisible;

        public string Title => message.Title;

        public string CustLevelName { get; private set; }

        public string DownLineLevelName { get; private set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public WinLossByBetTypeFilterViewModel FilterViewModel =>
            new WinLossByBetTypeFilterViewModel(
                CustId,
                CustLevel,
                DateRangePickerViewModel,
                IsFilterVisible,
                MinDate);

        private void BuildResources()
        {
            if (CustLevel.IsSuperLevel())
            {
                CustLevelName = Resources.Super;
                DownLineLevelName = Resources.Master;
            }
            else if (CustLevel.IsMasterLevel())
            {
                CustLevelName = Resources.Master;
                DownLineLevelName = Resources.Agent;
            }
            else
            {
                CustLevelName = Resources.Agent;
                DownLineLevelName = Resources.Member;
            }
        }
    }
}