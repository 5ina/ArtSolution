using ArtSolution.Common;
using ArtSolution.CommonSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtSolution.Layout
{
    public class PageHeadBuilder : ArtSolutionAppServiceBase, IPageHeadBuilder
    {

        #region Fields

        //private static readonly object s_lock = new object();
        
        //private readonly List<string> _titleParts = new List<string>();
        //private readonly List<string> _metaDescriptionParts;
        //private readonly List<string> _metaKeywordParts;
        private readonly SystemSetting _setting;

        #endregion

        #region Ctor

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="seoSettings">SEO settings</param>
        public PageHeadBuilder(ISettingService settingService)
        {
            this._setting = settingService.GetSystemSettings();
            //this._metaDescriptionParts = new List<string>();
            //this._metaKeywordParts = new List<string>();
        }

        #endregion

        #region method
        public string GenerateMetaDescription()
        {
            return _setting.Description;
        }

        public string GenerateMetaKeywords()
        {
            return _setting.Keywords;
        }

        public string GenerateTitle()
        {
            return _setting.Title;
        }

        #endregion
    }
}
