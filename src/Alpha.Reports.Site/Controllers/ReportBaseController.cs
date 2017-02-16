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

        protected FileContentResult ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var baseQuery = message as ReportBaseQuery;
            if (baseQuery != null && !baseQuery.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidReportExcelFileName, new byte[] { }));
            }

            return GetExcelContent(mediator.Send(message));
        }
    }
}