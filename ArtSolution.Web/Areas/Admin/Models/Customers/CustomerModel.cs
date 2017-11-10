using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtSolution.Web.Areas.Admin.Models.Customers
{

    [AutoMap(typeof(Customer))]
    public class CustomerModel:EntityDto
    {
        public CustomerModel()
        {
            this.AvailableCustomerRoles = new List<SelectListItem>();
        }
        [DisplayName("手机号")]
        public string Mobile { get; set; }

        [DisplayName("密码")]
        [DataType(DataType.Password)]
        [AllowHtml]
        public string Password { get; set; }
        [DisplayName("确认密码")]
        [AllowHtml]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }

        [DisplayName("用户角色")]
        /// <summary>
        /// 用户角色
        /// </summary>
        public int CustomerRoleId { get; set; }

        
        /// <summary>
        /// 是否关注
        /// </summary>
        [DisplayName("是否关注")]
        public bool IsSubscribe { get; set; }
        [DisplayName("是否推广人")]
        public bool IsPromoter { get; set; }

        public List<SelectListItem> AvailableCustomerRoles { get; set; }

    }
}