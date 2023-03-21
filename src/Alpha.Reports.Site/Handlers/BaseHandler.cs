namespace Alpha.Reports.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Alpha.Reports.Site.ViewModels.Components;
    using App_GlobalResources;
    using Reports.Constants;
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

        protected internal static IEnumerable<GroupProductViewModel> BuildProductSelectListItems(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products)
            => products?.Any() == true
                ? products
                    .GroupBy(product => new { product.Type, product.TypeName })
                    .Select(grp => new GroupProductViewModel(
                        new SelectListGroup { Name = grp.Key.TypeName },
                        grp.Select(product => new SelectListItem
                        {
                            Text = product.DisplayName,
                            Value = product.Id.ToString(),
                            Selected = userSelectedProductIds.Contains(product.Id)
                        })))
                : Enumerable.Empty<GroupProductViewModel>();

        protected internal static IEnumerable<GroupProductViewModel> BuildProductSelectListItems(IEnumerable<Product> products)
            => products?.Any() == true
                ? products
                    .GroupBy(product => new { product.Type, product.TypeName })
                    .Select(grp => new GroupProductViewModel(
                        new SelectListGroup { Name = grp.Key.TypeName },
                        grp.Select(product => new SelectListItem
                        {
                            Text = product.DisplayName,
                            Value = product.Id.ToString(),
                            Selected = true
                        })))
                : Enumerable.Empty<GroupProductViewModel>();

        protected internal static IEnumerable<ProductDetailViewModel> BuildProductDetailViewModels(
            IEnumerable<int> userSelectedResourceIds,
            IEnumerable<ProductDetail> productDetails)
            => productDetails.Any()
                ? productDetails.Select(p =>
                    new ProductDetailViewModel(p.ProductId, p.ResourceId, p.DisplayName, userSelectedResourceIds.Contains(p.ResourceId)))
                : Enumerable.Empty<ProductDetailViewModel>();

        protected internal static IEnumerable<ProductDetailViewModel> BuildProductDetailViewModels(IEnumerable<ProductDetail> productDetails)
            => productDetails.Any()
                ? productDetails.Select(p => new ProductDetailViewModel(p.ProductId, p.ResourceId, p.DisplayName, true))
                : Enumerable.Empty<ProductDetailViewModel>();

        protected internal IEnumerable<SelectListItem> BuildLeagueSelectListItems(
            IEnumerable<string> userSelectedLeagueIds,
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
                    Value = league.Id,
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

        protected internal IEnumerable<SelectListItem> BuildCurrencySelectListItems(
            IEnumerable<CurrencyModel> currencies,
            int userSelectedCurrency)
        {
            var result = new List<SelectListItem>();

            result.Add(new SelectListItem
            {
                Text = Resources.AllCurrencies,
                Value = DefaultVariables.AllCurrencies.ToString(),
                Selected = userSelectedCurrency == DefaultVariables.AllCurrencies
            });

            foreach (var currency in currencies)
            {
                result.Add(new SelectListItem
                {
                    Text = currency.Name,
                    Value = currency.Id.ToString(),
                    Selected = userSelectedCurrency == currency.Id
                });
            }

            return result;
        }

        protected internal IEnumerable<string> GetGrossCommNotes(UserLevel viewerLevel)
        {
            IList<string> grossCommNotes;

            if (viewerLevel.IsSuperLevel())
            {
                grossCommNotes = GetSuperGrossCommNotes();
            }
            else if (viewerLevel.IsMasterLevel())
            {
                grossCommNotes = GetMasterGrossCommNotes();
            }
            else if (viewerLevel.IsAgentLevel())
            {
                grossCommNotes = GetAgentGrossCommNotes();
            }
            else
            {
                grossCommNotes = new List<string>();
            }

            grossCommNotes.Add(Resources.GrossCommNoteForCasino);

            if (viewerLevel.IsNotAgentLevel())
            {
                grossCommNotes.Add(Resources.GrossCommNoteForRoundingProblem);
            }

            return grossCommNotes;
        }

        private IList<string> GetSuperGrossCommNotes()
            => new List<string>
            {
                Resources.GrossCommNoteForMaster,
                Resources.GrossCommNoteForAgent,
                Resources.GrossCommNoteForMember
            };

        private IList<string> GetMasterGrossCommNotes()
            => new List<string>
            {
                Resources.GrossCommNoteForAgent,
                Resources.GrossCommNoteForMember
            };

        private IList<string> GetAgentGrossCommNotes()
            => new List<string>
            {
                Resources.GrossCommNoteForMember
            };
    }
}