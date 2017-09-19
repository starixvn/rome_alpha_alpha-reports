namespace Alpha.Reports.Site.ViewModels.Statement
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Alpha.Reports.Models.Statement;
    using Alpha.Reports.Site.Requests.Statement;

    public class SMAStatementViewModel : StatementViewModel
    {
        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public SMAStatementViewModel(
            SMAStatementQuery message,
            IEnumerable<StatementModel> statements)
            : base(message, statements, message.MinDate, message.FromDate, message.ToDate, message.UserSelectedDateRangeOption, message.IsFilterVisible, message.PageSize, message.PageIndex)
        {
        }
    }
}