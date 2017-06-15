namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Reports.Models;
    using Requests;

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

            var selectedProduct = products.FirstOrDefault(product => userSelectedProductIds.Contains(product.Id));

            return products.Select(product =>
            {
                var productSelectListItem = new SelectListItem
                {
                    Text = product.DisplayName,
                    Value = product.Id.ToString(),
                    Selected = selectedProduct == null || userSelectedProductIds.Contains(product.Id)
                };

                return productSelectListItem;
            });
        }

        protected internal IEnumerable<SelectListItem> BuildLeagueSelectListItems(
            IEnumerable<int> userSelectedLeagueIds,
            IEnumerable<League> leagues)
        {
            if (leagues == null
                || !leagues.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return leagues.Select(league =>
            {
                var leagueSelectListItem = new SelectListItem
                {
                    Text = league.DisplayName,
                    Value = league.Id.ToString(),
                    Selected = userSelectedLeagueIds.Contains(league.Id)
                };

                return leagueSelectListItem;
            });
        }

        protected internal IEnumerable<CustInfo> BuildBreadcrumbLevel(ReportBaseQuery query)
        {
            var breadcrumbLevel = new List<CustInfo>();

            if (query.BreadcrumbLevel != null
                && query.BreadcrumbLevel.Any())
            {
                foreach (var custInfo in query.BreadcrumbLevel)
                {
                    if (custInfo.CustLevelId == query.CustLevelId)
                    {
                        break;
                    }

                    breadcrumbLevel.Add(custInfo);
                }
            }

            breadcrumbLevel.Add(new CustInfo(query.CustId, query.CustName, query.CustLevelId));

            return breadcrumbLevel;
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