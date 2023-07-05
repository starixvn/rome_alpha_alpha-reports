namespace Alpha.Reports.Site.Features.Shared
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Reports.Models;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.Features.Shared.Requests;
    using MediatR;

    public class UsernameSuggestionHandler : IAsyncRequestHandler<UsernameSuggestionQuery, IEnumerable<UsernameSuggestion>>
    {
        private readonly CustomerService customerService;

        public UsernameSuggestionHandler(CustomerService customerService)
        {
            this.customerService = customerService;
        }

        public Task<IEnumerable<UsernameSuggestion>> Handle(UsernameSuggestionQuery message)
            => customerService.GetUsernamesSuggestion(
                message.ViewerId,
                message.SuggestionQuery,
                message.IsInternal,
                message.DirectDownlines,
                message.IsAccInfo);
    }
}