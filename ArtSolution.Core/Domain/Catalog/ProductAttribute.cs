using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Catalog
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class ProductAttribute : Entity
    {
        public int ProductId { get; set; }
        
        [Required, MaxLength(50)]
        public string ValueName { get; set; }
        /// <summary>
        /// 规格的价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }
    }
}
