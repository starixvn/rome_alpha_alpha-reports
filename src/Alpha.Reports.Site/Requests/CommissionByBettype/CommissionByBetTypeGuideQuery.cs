namespace Alpha.Reports.Site.Requests.CommissionByBettype
{
    using System.Collections.Generic;
    using MediatR;

    public class CommissionByBetTypeGuideQuery : IRequest<IEnumerable<string>>
    {
    }
}