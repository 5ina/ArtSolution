using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using System.ComponentModel;

namespace ArtSolution.Web.Models.Customers
{

    [AutoMap(typeof(ApplyCash))]
    public class ApplyCashModel : EntityDto
    {
        public int CustomerId { get; set; }
        /// <summary>
        /// 提现金额
        /// </summary>
        [DisplayName("提现金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal Allowance { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Audit { get; set; }
    }
}