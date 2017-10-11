using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;

namespace ArtSolution.Domain.Customers
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class Customer : Entity, IHasCreationTime, IHasModificationTime
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(15)]
        public string Mobile { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [MaxLength(30)]
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required, MaxLength(60)]
        public string Password { get; set; }

        /// <summary>
        /// 用于存储微信的Id
        /// </summary>
        [MaxLength(60)]
        public string OpenId { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public int CustomerRoleId { get; set; }
        
        /// <summary>
        /// 秘钥
        /// </summary>
        [Required, MaxLength(6)]
        public string PasswordSalt { get; set; }
        

        /// <summary>
        /// 最后修改时间（不需要处理）
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 创建时间（不需要处理）
        /// </summary>
        public DateTime CreationTime { get; set; }

        [MaxLength(200)]
        public string VerificationCode { get; set; }
        
        /// <summary>
        /// 推广人
        /// </summary>
        public int Promoter { get; set; }

        /// <summary>
        /// 是否关注
        /// </summary>
        public bool IsSubscribe { get; set; }
    }
}
