using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Catalog
{
    /// <summary>
    /// 商品的销售标签
    /// </summary>
    public class ProductTag: Entity
    {
        [Required, MaxLength(10)]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool Enabled { get; set; }

        [Required, MaxLength(500)]
        public string TagImage { get; set; }
    }
}
