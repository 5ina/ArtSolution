using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Web.Extensions
{
    /// <summary>
    /// 商品属性转换为字符串
    /// </summary>
    public static class ProductAttributeExtensions
    {
        public static string ToAttributeString(this string specifications)
        {
            var attributeStringValue = string.Empty;
            var productAttributeList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductAttribute>>(specifications);
            foreach (var attr in productAttributeList)
            {
                attributeStringValue += string.Format("<p>{0}:{1}</p>", attr.AttributeName, attr.ValueName);
            }

            return attributeStringValue;
        }
    }
}