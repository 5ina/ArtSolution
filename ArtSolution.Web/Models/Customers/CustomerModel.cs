using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using System;
using System.ComponentModel;

namespace ArtSolution.Web.Models.Customers
{
    [AutoMap(typeof(Customer))]
    public class CustomerModel : EntityDto
    {
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        [DisplayName("微信昵称")]
        public string NickName { get; set; }


        [DisplayName("OPENID")]
        public string OpenId { get; set; }
                

        /// <summary>
        /// 创建时间（不需要处理）
        /// </summary>
        public DateTime CreationTime { get; set; }

        public string CustomerAvatar { get; set; }


    }
}