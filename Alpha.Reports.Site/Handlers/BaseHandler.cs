namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Newtonsoft.Json;
    using Requests;
    using ViewModels;

    public class BaseHandler
    {
        internal IEnumerable<SelectListItem> BuildProductSelectListItems(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products)
        {
            if (products == null
                || !products.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return products.Select(product =>
            {
                var productSelectListItem = new SelectListItem
                {
                    Text = product.DisplayName,
                    Value = product.Id.ToString(),
                    Selected = userSelectedProductIds.Contains(product.Id)
                };

                return productSelectListItem;
            });
        }

        internal IEnumerable<ActionLinkViewModel> BuildBreadcrumbLevels<T>(
            BaseReportQuery message,
            Func<CustInfo, BaseReportQuery, string, T> buildActionLinkViewModelFunc) where T : class
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (buildActionLinkViewModelFunc == null)
            {
                throw new ArgumentNullException(nameof(buildActionLinkViewModelFunc));
            }

            var drilldownLevels = new List<ActionLinkViewModel>();
            var breadcrumbLevels = new List<CustInfo>();

            if (!string.IsNullOrWhiteSpace(message.BreadcrumbLevels))
            {
                var messageBreadcrumbLevels = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(message.BreadcrumbLevels.Replace("'", "\""));

                foreach (var custInfo in messageBreadcrumbLevels)
                {
                    if (custInfo.UserLevelId == message.UserLevelId)
                    {
                        break;
                    }

                    breadcrumbLevels.Add(custInfo);
                    var actionLinkViewModel = new ActionLinkViewModel(
                        controllerName: "WinLossDetail",
                        actionName: "Index",
                        linkText: custInfo.CustName,
                        routeValues: buildActionLinkViewModelFunc(custInfo, message, ConvertBreadcrumbLevelsToJson(breadcrumbLevels)));
                    drilldownLevels.Add(actionLinkViewModel);
                }
            }

            breadcrumbLevels.Add(new CustInfo(message.CustId, message.CustName, message.UserLevelId));
            var routeValues = buildActionLinkViewModelFunc(
                new CustInfo(message.CustId, message.CustName, message.UserLevelId),
                message,
                ConvertBreadcrumbLevelsToJson(breadcrumbLevels));

            drilldownLevels.Add(new ActionLinkViewModel(
                controllerName: "WinLossDetail",
                actionName: "Index",
                linkText: message.CustName,
                routeValues: routeValues));

            return drilldownLevels;
        }

        private string ConvertBreadcrumbLevelsToJson(IEnumerable<CustInfo> breadcrumbLevels)
        {
            return JsonConvert.SerializeObject(breadcrumbLevels).Replace("\"", "'");
        }
    }
}