using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Orders;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Web.Models.Orders
{

    [AutoMap(typeof(ReturnOrder))]
    public class ReturnOrderModel : EntityDto
    {
        /// <summary>
        /// 所属订单
        /// </summary>
        public int OrderId { get; set; }        

        /// <summary>
        /// 所属用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 退货原因
        /// </summary>
        [DisplayName("退单原因")]
        [Required, MaxLength(200)]
        public string ReasonForReturn { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        public int AuditStatus { get; set; }

        /// <summary>
        /// 拒绝原因
        /// </summary>
        [MaxLength(50)]
        public string AuditReason { get; set; }
    }
}