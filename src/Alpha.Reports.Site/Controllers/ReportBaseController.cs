namespace Alpha.Reports.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Helpers;
    using MediatR;
    using Requests;
    using ViewModels;
    using Web;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class ReportBaseController : BaseController
    {
        private const string InvalidReportViewPath = "~/Views/Shared/InvalidReport.cshtml";
        private const string InvalidReportExcelFileName = "InvalidReport";

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
                return View(InvalidReportViewPath);
            }

            return View(mediator.Send(message));
        }

        protected ActionResult View<TResponse>(string viewName, IRequest<TResponse> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return View(InvalidReportViewPath);
            }

            return View(viewName, mediator.Send(message));
        }

        protected async Task<ActionResult> ViewAsync<TResponse>(string viewName, IAsyncRequest<TResponse> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return View(InvalidReportViewPath);
            }

            return View(viewName, await mediator.SendAsync(message));
        }

        protected FileContentResult ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidReportExcelFileName, new byte[] { }));
            }

            return GetExcelContent(mediator.Send(message));
        }

        protected ActionResult Json<TResponse>(IRequest<TResponse> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return Json(Resources.NoInformationIsAvailable);
            }

            return new JsonNetResult(mediator.Send(message));
        }

        protected async Task<ActionResult> JsonAsync<TResponse>(IAsyncRequest<TResponse> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return Json(new Result((int)HttpStatusCode.Forbidden, Resources.NotAuthorized));
            }

            return new JsonNetResult(await mediator.SendAsync(message));
        }
    }
}