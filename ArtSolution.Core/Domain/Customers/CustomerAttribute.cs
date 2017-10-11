using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Customers
{
    /// <summary>
    /// 用户属性
    /// </summary>
    public class CustomerAttribute : Entity
    {
        /// <summary>
        /// 所属用户Id
        /// </summary>
        public int CustomerId { get; set; }

        [Required, MaxLength(200)]
        public string Key { get; set; }

        public string Value { get; set; }

    }
}
