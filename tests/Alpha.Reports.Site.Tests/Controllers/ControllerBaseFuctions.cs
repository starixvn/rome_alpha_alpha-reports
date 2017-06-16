namespace Alpha.Reports.Site.Tests.Controllers
{
    using MediatR;
    using NSubstitute;
    using Site.ViewModels;

    public static class ControllerBaseFuctions
    {
        public static void ArrangeExportExcel<TQuery>(IMediator mediator)
            where TQuery : IRequest<ExcelFileViewModel>
        {
            mediator.Send(Arg.Any<TQuery>()).Returns(new ExcelFileViewModel("Excel File", new byte[0]));
        }
    }
}
