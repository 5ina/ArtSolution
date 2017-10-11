using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.News;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Models.News
{
    [AutoMap(typeof(Loan))]
    public class LoanModel:EntityDto
    {
        public LoanModel()
        {
            this.AvailableCyclies = new List<SelectListItem>();
            this.AvailableQuotas = new List<SelectListItem>();
        }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required, MaxLength(10)]
        [DisplayName("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Required, MaxLength(11)]
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DisplayName("性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 学校名称
        /// </summary>
        [Required, MaxLength(30)]
        [DisplayName("所在院校")]
        public string SchoolName { get; set; }

        /// <summary>
        /// 定位位置
        /// </summary>
        [MaxLength(200)]
        [DisplayName("当前位置")]
        public string Location { get; set; }

        /// <summary>
        /// 身份证正面
        /// </summary>
        [DisplayName("身份证-正面")]
        [UIHint("Camera")]
        public string IdCardPositive { get; set; }

        [DisplayName("身份证-反面")]

        [UIHint("Camera")]
        public string IdCardSide { get; set; }

        /// <summary>
        /// 额度
        /// </summary>
        [DisplayName("分期额度")]
        public int Quota { get; set; }
        public IList<SelectListItem> AvailableQuotas { get; set; }

        [DisplayName("分期时间")]
        public int Cycle { get; set; }

        public IList<SelectListItem> AvailableCyclies { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        public int Audit { get; set; }

        /// <summary>
        /// 未通过原因
        /// </summary>
        [MaxLength(200)]
        public string AuditReason { get; set; }

        public long? CreatorUserId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}