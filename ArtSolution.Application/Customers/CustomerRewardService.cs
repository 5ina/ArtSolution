using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using ArtSolution.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ArtSolution.Customers
{
    public class CustomerRewardService: ArtSolutionAppServiceBase ,ICustomerRewardService
    {

        #region Ctor && Field
        /// <summary>
        /// 用户属性{0} 用户主键
        /// </summary>
        private const string CUSTOMER_REWARD_HISTORY = "store.reward.history.by-customerid-{0}";

        private readonly IRepository<CustomerReward> _rewardRepository;
        private readonly ICacheManager _cacheManager;

        public CustomerRewardService(IRepository<CustomerReward> rewardRepository,
            ICacheManager cacheManager)
        {
            this._rewardRepository = rewardRepository;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Method

        public void InsertRewardHistory(CustomerReward history)
        {
            if (history == null)
                throw new Exception("reward-history");

            var key = string.Format(CUSTOMER_REWARD_HISTORY, history.CustomerId);
            _cacheManager.GetCache(key).Remove(key);
            _rewardRepository.Insert(history);
            
        }

        public void UpdateRewardHistory(CustomerReward history)
        {
            if (history == null)
                throw new Exception("reward-history");

            var key = string.Format(CUSTOMER_REWARD_HISTORY, history.CustomerId);
            _cacheManager.GetCache(key).Remove(key);
            _rewardRepository.Update(history);
        }

        public void DeleteRewardHistoryById(int historyId)
        {
            if (historyId < 0)
                throw new Exception("reward-history");
            var reward = _rewardRepository.Get(historyId);
            var key = string.Format(CUSTOMER_REWARD_HISTORY, reward.CustomerId);
            _cacheManager.GetCache(key).Remove(key);
            _rewardRepository.Delete(historyId);
        }

        public void DeleteRewardHistoryByCustomerId(int customerId)
        {
            if (customerId < 0)
                throw new Exception("reward-history"); 
            var key = string.Format(CUSTOMER_REWARD_HISTORY, customerId);
            _cacheManager.GetCache(key).Remove(key);
            _rewardRepository.Delete(r => r.CustomerId == customerId);
        }

        public CustomerReward GetCustomerRewardHistoryById(int historyId)
        {
            return _rewardRepository.Get(historyId);
        }

        public IPagedResult<CustomerReward> GetAllCustomerRewards(int customer, string keywords = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {

            var query = _rewardRepository.GetAll();
            if (customer > 0)
                query = query.Where(r => r.CustomerId == customer);

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(r => r.Message.Contains(keywords));

            query = query.OrderByDescending(r => r.CreationTime);
            return new PagedResult<CustomerReward>(query, pageIndex, pageSize);
        }

        #endregion
    }
}
