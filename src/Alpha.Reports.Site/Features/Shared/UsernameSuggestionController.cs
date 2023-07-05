namespace Alpha.Reports.Site.Features.Shared
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Controllers;
    using Alpha.Reports.Site.Features.Shared.Requests;
    using Alpha.Web.Filters;
    using MediatR;

    [AllowProtectedMode]
    public class UsernameSuggestionController : ReportBaseController
    {
        public UsernameSuggestionController(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ActionResult> Get(UsernameSuggestionQuery message)
        {
            var usernamesSuggestion = message.IsValid()
                ? await mediator.SendAsync(message)
                : Enumerable.Empty<UsernameSuggestion>();

            return Json(usernamesSuggestion, JsonRequestBehavior.AllowGet);
        }
    }
}