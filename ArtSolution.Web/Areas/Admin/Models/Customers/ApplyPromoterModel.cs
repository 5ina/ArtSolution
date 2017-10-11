using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using System;
using System.ComponentModel;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{
    [AutoMap(typeof(ApplyPromoter))]
    public class ApplyPromoterModel : EntityDto
    {
        [DisplayName("申请时间")]
        public DateTime CreationTime { get; set; }

        public int CustomerId { get; set; }

        [DisplayName("用户昵称")]
        public string NickName { get; set; }

        [DisplayName("手机号")]
        public string Mobile { get; set; }

        [DisplayName("审核状态")]
        public bool Audit { get; set; }

        /// <summary>
        /// 未通过原因
        /// </summary>
        [DisplayName("原因")]
        public string AuditReason { get; set; }

    }
}