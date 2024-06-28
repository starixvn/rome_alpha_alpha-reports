namespace Alpha.Reports.Site.Requests.WinLossDetail
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.Shared.ViewModels.Components;
    using MediatR;

    public class WinLossDetailWeeksQuery : WinLossDetailGetDataWeekQuery, IAsyncRequest<IEnumerable<WeekSelectionViewModel>>
    {
        public WinLossDetailWeeksQuery(IUser user) : base(user)
        {
        }
    }
}