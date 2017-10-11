using System.Collections.Generic;
using Abp.Localization;

namespace ArtSolution.Web.Areas.Admin.Models.Layout
{
    public class LanguageSelectionViewModel
    {
        public LanguageInfo CurrentLanguage { get; set; }

        public IReadOnlyList<LanguageInfo> Languages { get; set; }
    }
}