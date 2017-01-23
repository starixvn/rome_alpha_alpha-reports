namespace Alpha.Reports.Site.ViewModels.RacingCommission
{
    using System;
    using System.Collections.Generic;
    using Components;
    using Reports.Models.RacingCommission;
    using Requests.WinLoss;

    public class RacingCommissionViewModel
    {
        public RacingCommissionViewModel(RacingCommissionQuery query, IEnumerable<RacingCommissionModel> racingCommission, DateTime balanceUpDate)
        {
            DateRangePickerViewModel dateRangePickerViewModel = new DateRangePickerViewModel(
                balanceUpDate,
                DateTime.MaxValue,
                query.FromDate,
                query.ToDate,
                query.UserSelectedDateRangeOption);

            RacingCommissionFilter = new RacingCommissionFilterViewModel(dateRangePickerViewModel, balanceUpDate);
            RacingCommissions = racingCommission;
        }

        public string Title
        {
            get
            {
                return "Racing Commission";
            }
        }

        public RacingCommissionFilterViewModel RacingCommissionFilter { get; }

        public IEnumerable<RacingCommissionModel> RacingCommissions { get; }

        public RacingCommissionModel Total => RacingCommissionModel.GetAccumulatedCommission(RacingCommissions);
    }
}