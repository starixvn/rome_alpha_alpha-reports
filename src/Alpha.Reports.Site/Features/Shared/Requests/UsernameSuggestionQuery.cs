namespace Alpha.Reports.Site.Features.Shared.Requests
{
    using System.Collections.Generic;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Requests;
    using MediatR;

    public class UsernameSuggestionQuery : ReportBaseQuery, IAsyncRequest<IEnumerable<UsernameSuggestion>>
    {
        public UsernameSuggestionQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
        }

        public string SuggestionQuery { get; set; }

        public bool DirectDownlines { get; set; }

        public bool IsAccInfo { get; set; } = true;

        public bool IsInternal { get; }

        public override bool IsValid()
            => base.IsValid() && !string.IsNullOrEmpty(SuggestionQuery);
    }
}