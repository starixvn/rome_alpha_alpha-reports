namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using Reports.Models;
    using Requests;
    using ViewModels;

    public class BaseHandler
    {
        private const string ExcelTemplateDir = "~/App_Data/ExcelTemplates/{0}/{1}.{2}";

        private Func<string, string> mapPath;

        internal Func<string, string> MapPath
        {
            get { return mapPath ?? HttpContext.Current.Server.MapPath; }
            set { mapPath = value; }
        }

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

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
            ReportBaseQuery message,
            string controllerName,
            string actionName,
            Func<CustInfo, ReportBaseQuery, string, T> buildActionLinkViewModelFunc) where T : class
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
                        controllerName: controllerName,
                        actionName: actionName,
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
                controllerName: controllerName,
                actionName: actionName,
                linkText: message.CustName,
                routeValues: routeValues));

            return drilldownLevels;
        }

        private string ConvertBreadcrumbLevelsToJson(IEnumerable<CustInfo> breadcrumbLevels)
        {
            return JsonConvert.SerializeObject(breadcrumbLevels).Replace("\"", "'");
        }

        internal virtual string GetExcelTemplatePath(string reportName, string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(reportName))
            {
                throw new ArgumentNullException(nameof(reportName), "Report Name cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var relativePath = string.Format(ExcelTemplateDir, reportName, templateName, fileExtension);

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }
    }
}