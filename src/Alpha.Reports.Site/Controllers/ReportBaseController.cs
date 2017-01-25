namespace Alpha.Reports.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using Requests;
    using ViewModels;
    using Web;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class ReportBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected ReportBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                //// TODO: this is pseudo code
                return RedirectToAction("InvalidReport");
            }

            return View(mediator.Send(message));
        }

        protected FileContentResult ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                //// TODO: this is pseudo code
                //// return RedirectToAction("EmptyFileContent");
            }

            return GetExcelContent(mediator.Send(message));
        }
    }
}