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

        protected internal Func<string, string> MapPath
        {
            get { return mapPath ?? HttpContext.Current.Server.MapPath; }
            set { mapPath = value; }
        }

        internal Func<string, bool> IsFileExist { get; set; } = File.Exists;

        protected internal IEnumerable<SelectListItem> BuildProductSelectListItems(
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

        protected internal IEnumerable<ActionLinkViewModel> BuildBreadcrumbLevels<T>(
            IEnumerable<CustInfo> drillDownInfo,
            ReportBaseQuery query,
            string controllerName,
            string actionName,
            Func<CustInfo, ReportBaseQuery, string, T> buildRouteValues) where T : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (buildRouteValues == null)
            {
                throw new ArgumentNullException(nameof(buildRouteValues));
            }

            var breadcrumbLevels = new List<ActionLinkViewModel>();
            var drillDonwInfoByLevels = new List<CustInfo>();

            foreach (var custInfo in drillDownInfo)
            {
                drillDonwInfoByLevels.Add(custInfo);

                var actionLinkViewModel = new ActionLinkViewModel(
                    controllerName: controllerName,
                    actionName: actionName,
                    linkText: custInfo.CustName,
                    routeValues: buildRouteValues(custInfo, query, ToJson(drillDonwInfoByLevels)));

                breadcrumbLevels.Add(actionLinkViewModel);
            }

            return breadcrumbLevels;
        }

        protected internal IEnumerable<CustInfo> BuildDrillDownInfo(ReportBaseQuery query)
        {
            var drillDownInfo = new List<CustInfo>();

            if (!string.IsNullOrWhiteSpace(query.DrillDownInfoJson))
            {
                var previousDrillDownInfo = JsonConvert.DeserializeObject<IEnumerable<CustInfo>>(query.DrillDownInfoJson.Replace("'", "\""));

                foreach (var custInfo in previousDrillDownInfo)
                {
                    if (custInfo.CustLevelId == query.CustLevelId)
                    {
                        break;
                    }

                    drillDownInfo.Add(custInfo);
                }
            }

            drillDownInfo.Add(new CustInfo(query.CustId, query.CustName, query.CustLevelId));

            return drillDownInfo;
        }

        protected internal string ToJson(object data)
        {
            return JsonConvert.SerializeObject(data).Replace("\"", "'");
        }

        protected internal virtual string GetExcelTemplatePath(string templateName, string fileExtension = "xls")
        {
            if (string.IsNullOrWhiteSpace(templateName))
            {
                throw new ArgumentNullException(nameof(templateName), "Template Name cannot be null or empty");
            }

            var relativePath = string.Format(ExcelTemplateDir, GetExcelTemplateFolder(), templateName, fileExtension);

            var absolutePath = MapPath(relativePath);

            if (IsFileExist(absolutePath))
            {
                return absolutePath;
            }

            throw new FileNotFoundException($"Template {absolutePath} does not exist");
        }

        protected internal string GetExcelTemplateFolder()
        {
            var reportNamespace = GetType().Namespace;

            var lastIndexOfDot = reportNamespace.LastIndexOf(".", StringComparison.Ordinal) + 1;

            if (lastIndexOfDot <= 0)
            {
                throw new ArgumentException("Namespace does not contain any dot character, wrong report convention.");
            }

            var templateFolderName = reportNamespace.Substring(lastIndexOfDot);

            return templateFolderName;
        }
    }
}