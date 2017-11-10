using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace ArtSolution.Web.Extensions
{
    public static class HtmlExtensions
    {
        #region Html Extensions
        public static MvcHtmlString ArtLabelFor<TModel, TValue>(this HtmlHelper<TModel> helper,
        Expression<Func<TModel, TValue>> expression, bool displayHint = true)
        {
            return helper.LabelFor(expression, new { @class = "control-label" });
        }


        public static string FieldIdFor<T, TResult>(this HtmlHelper<T> html, Expression<Func<T, TResult>> expression)
        {
            var id = html.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));            
            return id.Replace('[', '_').Replace(']', '_');
        }

        public static MvcHtmlString ArtEditorFor<TModel, TValue>(this HtmlHelper<TModel> helper,
           Expression<Func<TModel, TValue>> expression, string placeholder = "", bool renderFormControlClass = true)
        {
            var result = new StringBuilder();

            var htmlAttributes = new
            {
                @class = renderFormControlClass ? "form-control" : "",
            };
            result.Append(helper.EditorFor(expression, new { htmlAttributes, placeholder }));

            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString ArtDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> helper,
          Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> itemList,
          object htmlAttributes = null, string optionLabel = null, bool renderFormControlClass = false, bool required = false)
        {
            var result = new StringBuilder();

            var attrs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (renderFormControlClass)
                attrs = AddFormControlClassToHtmlAttributes(attrs);

            if (required)
                result.AppendFormat("<div class=\"input-group input-group-required\">{0}<div class=\"input-group-btn\"><span class=\"required\">*</span></div></div>",
                    helper.DropDownListFor(expression, itemList, optionLabel, attrs).ToHtmlString());
            else
                result.Append(helper.DropDownListFor(expression, itemList, optionLabel,attrs).ToHtmlString());
            
            return MvcHtmlString.Create(result.ToString());
        }


        public static RouteValueDictionary AddFormControlClassToHtmlAttributes(RouteValueDictionary htmlAttributes)
        {
            //TODO test new implementation
            if (!htmlAttributes.ContainsKey("class"))
                htmlAttributes.Add("class", null);
            if (htmlAttributes["class"] == null || string.IsNullOrEmpty(htmlAttributes["class"].ToString()))
                htmlAttributes["class"] = "form-control";
            else
                if (!htmlAttributes["class"].ToString().Contains("form-control"))
                htmlAttributes["class"] += " form-control";

            return htmlAttributes;
        }

        #endregion

    }
}