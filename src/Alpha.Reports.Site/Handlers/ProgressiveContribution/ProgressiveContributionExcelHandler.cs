namespace Alpha.Reports.Site.Handlers.ProgressiveContribution
{
    using System;
    using MediatR;
    using Requests.ProgressiveContribution;
    using Services;
    using ViewModels;

    public class ProgressiveContributionExcelHandler :
        ProgressiveContributionBaseHandler,
        IRequestHandler<ProgressiveContributionExcelQuery, ExcelFileViewModel>
    {
        public ProgressiveContributionExcelHandler(ProductService productService) : base(productService)
        {
        }

        public ExcelFileViewModel Handle(ProgressiveContributionExcelQuery message)
        {
            throw new NotImplementedException();
        }
    }
}