namespace Alpha.Reports.Site.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using MediatR;
    using ViewModels;
    using Web;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1304:NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed")]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public abstract class ReportControllerBase : BaseController
    {
        protected readonly IMediator mediator;

        protected ReportControllerBase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [NonAction]
        public FileContentResult ExcelFile(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }   
}   