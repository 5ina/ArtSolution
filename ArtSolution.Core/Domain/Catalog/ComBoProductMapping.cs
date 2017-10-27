using Abp.Domain.Entities;

namespace ArtSolution.Domain.Catalog
{
    /// <summary>
    /// 组合套餐商品关联表
    /// </summary>
    public class ComBoProductMapping : Entity
    {
        /// <summary>
        /// 所属套餐
        /// </summary>
        public int ComBoId { get; set; }

        /// <summary>
        /// 所属商品
        /// </summary>
        public int ProductId { get; set; }

    }
}
