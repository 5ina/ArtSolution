using ArtSolution.Common;
using ArtSolution.Layout;
using ArtSolution.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolution.Web.Framework
{
    /// <summary>
    /// 布局扩展
    /// </summary>
    public static class LayoutExtensions
    {
        private static string _title;
        private static string _keywords;
        private static string _description;

        #region title
        /// <summary>
        /// 增加标题
        /// </summary>
        /// <param name="html"></param>
        /// <param name="part"></param>
        public static void AddTitleParts(this HtmlHelper html,string part)
        {
            _title = part;
        }
        
        /// <summary>
        /// 自定义Title
        /// </summary>
        /// <param name="html"></param>
        /// <param name="addDefaultTitle"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public static MvcHtmlString CustomTitle(this HtmlHelper html)
        {
            var pageHeadBuilder = Abp.Dependency.IocManager.Instance.Resolve<IPageHeadBuilder>();
            if (!string.IsNullOrWhiteSpace(_title))
            {
                var title = string.Join(" | ", _title, pageHeadBuilder.GenerateTitle());
                _title = "";
                return new MvcHtmlString(title);
            }
            else
            {
                return new MvcHtmlString(pageHeadBuilder.GenerateTitle());
            }
        }

        #endregion

        #region keywords


        /// <summary>
        /// Add meta description element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Meta description part</param>
        public static void AddMetaDescriptionParts(this HtmlHelper html,string part)
        {
            _description = part;
        }
        /// <summary>
        /// Generate all description parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Meta description part</param>
        /// <returns>Generated string</returns>
        public static MvcHtmlString CustomMetaDescription(this HtmlHelper html)
        {
            var pageHeadBuilder = Abp.Dependency.IocManager.Instance.Resolve<IPageHeadBuilder>();

            if (!String.IsNullOrWhiteSpace(_description))
            {
                var description = string.Join(",", _description, pageHeadBuilder.GenerateMetaDescription());
                _description = "";
                return new MvcHtmlString(html.Encode(description));

            }
            else
            {
                return new MvcHtmlString(html.Encode(pageHeadBuilder.GenerateMetaDescription()));
            }
        }


        /// <summary>
        /// Add meta keyword element to the <![CDATA[<head>]]>
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Meta keyword part</param>
        public static void AddMetaKeywordParts(this HtmlHelper html, string part)
        {
            _keywords = part;
        }
        /// <summary>
        /// Generate all keyword parts
        /// </summary>
        /// <param name="html">HTML helper</param>
        /// <param name="part">Meta keyword part</param>
        /// <returns>Generated string</returns>
        public static MvcHtmlString CustomMetaKeywords(this HtmlHelper html)
        {

            var pageHeadBuilder = Abp.Dependency.IocManager.Instance.Resolve<IPageHeadBuilder>();

            if (!String.IsNullOrWhiteSpace(_keywords))
            {
                var keywords = string.Join(",", _keywords, pageHeadBuilder.GenerateMetaKeywords());
                _keywords = "";
                return new MvcHtmlString(html.Encode(keywords));

            }
            else
            {
                return new MvcHtmlString(html.Encode(pageHeadBuilder.GenerateMetaKeywords()));
            }
        }
        #endregion
    }
}