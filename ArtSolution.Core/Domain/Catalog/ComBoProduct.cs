using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Catalog
{
    /// <summary>
    /// 商品组合
    /// </summary>
    public class ComBoProduct : Entity
    {
        /// <summary>
        /// 组合名称
        /// </summary>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 组合图片
        /// </summary>
        [MaxLength(500)]
        public string ComBoProductImage { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int StockQuantity { get; set; }

       /// <summary>
       /// 销售价格
       /// </summary>
        public decimal Price { get; set; }


        /// <summary>
        /// 原价
        /// </summary>
        public decimal Market { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool Published { get; set; }
    }
}
