using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArtSolution.Authentication.Dto
{

    [AutoMap(typeof(Customer))]
    public class CustomerDto : EntityDto, IUser<int>
    {
        
        public string Mobile { get; set; }
        
        public string Password { get; set; }

        public string NickName { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public int CustomerRoleId { get; set; }

        public string OpenId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [NotMapped]
        public CustomerRole CustomerRole
        {
            get { return (CustomerRole)CustomerRoleId; }
            set { this.CustomerRoleId = (int)value; }
        }
        
        public string PasswordSalt { get; set; }

        
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 创建时间（不需要处理）
        /// </summary>
        public DateTime CreationTime { get; set; }
        
        public string VerificationCode { get; set; }

        public int Promoter { get; set; }

        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsSubscribe { get; set; }
        public string UserName
        {
            get
            {
                if (String.IsNullOrWhiteSpace(NickName))
                    return this.Mobile;
                else
                    return NickName;
            }

            set
            {
                if (String.IsNullOrWhiteSpace(NickName))
                    this.NickName = value;
                else
                    this.Mobile = value;
            }
        }
    }
}
