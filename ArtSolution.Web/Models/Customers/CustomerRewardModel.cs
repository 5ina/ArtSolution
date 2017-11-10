using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using System;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Customers
{
    public class CustomerRewardModel
    {
        public CustomerRewardModel() {

            this.Histories = new List<RewardHistoryModel>();
        }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 积分额
        /// </summary>
        public int Reward { get; set; }

        public bool IsSign { get; set; }

        public IList<RewardHistoryModel> Histories { get; set; }

    }


    [AutoMap(typeof(CustomerReward))]
    public class RewardHistoryModel : EntityDto
    {
        /// <summary>
        /// 所属用户
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// 增加/删除的积分（操作的）
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// 总积分（操作前的）
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 积分说明
        /// </summary>
        public string Message { get; set; }

        public DateTime CreationTime { get; set; }
    }
}