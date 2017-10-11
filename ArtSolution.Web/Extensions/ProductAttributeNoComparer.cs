using ArtSolution.Domain.Catalog;
using System.Collections.Generic;

namespace ArtSolution.Web.Extensions
{
    /// <summary>
    /// 实体比较
    /// </summary>
    public class ProductAttributeNoComparer : IEqualityComparer<ProductAttribute>
    {
        public bool Equals(ProductAttribute p1, ProductAttribute p2)
        {
            if (p1 == null)
                return p2 == null;
            return p1.AttributeId == p2.AttributeId;
        }

        public int GetHashCode(ProductAttribute p)
        {
            if (p == null)
                return 0;
            return p.AttributeId.GetHashCode();
        }
                
    }
}