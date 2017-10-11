using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ArtSolution.Common;
using ArtSolution.Domain.Customers;
using ArtSolution.Names;
using System;
using System.Data.Objects;
using System.Linq;

namespace ArtSolution.Customers
{
    /// <summary>
    /// 签到服务接口实现类
    /// </summary>
    public class SignLogService : ArtSolutionAppServiceBase, ISignLogService
    {
        #region Ctor && Field

        private readonly IRepository<SignLog> _logRepository;
        private readonly IRepository<CustomerReward> _rewardRepository;
        private readonly ISettingService _settingService;

        private readonly ICustomerService _customerService;
        private readonly IUnitOfWorkManager _unitOfWorkManage;

        public SignLogService(IRepository<SignLog> logRepository,
            IRepository<CustomerReward> rewardRepository,
            ISettingService settingService,
            ICustomerService customerService,
            IUnitOfWorkManager unitOfWorkManage)
        {
            this._logRepository = logRepository;
            this._rewardRepository = rewardRepository;
            this._settingService = settingService;
            this._customerService = customerService;
            this._unitOfWorkManage = unitOfWorkManage;
        }


        #endregion
        #region

        public void Clear(int customerId = 0)
        {
            if (customerId == 0)
            {
                _logRepository.Delete(e => e.CustomerId == customerId);
            }
            else
            {
                
            }
        }

        public void SignIn(int customerId)
        {
            //是否开启登录奖励
            var enabled = _settingService.GetSettingByKey<bool>(RewardSettingNames.SignRewardEnabled);
            if (enabled)
            {
                var lastLog = _logRepository.GetAll()
                                .OrderByDescending(l => l.CreationTime)
                                .Where(l => l.CustomerId == customerId)
                                .FirstOrDefault();
                
                var customer = _customerService.GetCustomerId(customerId);
                var total = customer.GetCustomerAttributeValue<int>(CustomerAttributeNames.Reward);
                if (lastLog == null) //首次登录
                {
                    using (var unitOfWork = _unitOfWorkManage.Begin())
                    {
                        var firstReward = _settingService.GetSettingByKey<int>(RewardSettingNames.FirstRewardPoint);
                        //签到表
                        _logRepository.Insert(new SignLog
                        {
                            CreationTime = DateTime.Now,
                            CustomerId = customerId,
                            NumberEntries = 1,
                            Reward = firstReward
                        });
                        //新增积分日志表
                        _rewardRepository.Insert(new CustomerReward
                        {
                            CreationTime = DateTime.Now,
                            CustomerId = customerId,
                            Message = "连续签到赠送积分",
                            Points = firstReward,
                            Total = total + firstReward
                        });
                        //存储积分
                        customer.SaveCustomerAttribute<int>(CustomerAttributeNames.Reward, total + firstReward);
                        unitOfWork.Complete();
                    }

                }
                else//连续登录
                {
                    if (IsSign(customerId))
                    {
                        return; //如果是当天则跳过
                    }
                    var reward = lastLog.Reward;
                    var additionalReward = _settingService.GetSettingByKey<int>(RewardSettingNames.AdditionalReward);
                    var maxReward = _settingService.GetSettingByKey<int>(RewardSettingNames.MaxRewardPoint);
                    reward = reward + additionalReward > maxReward ? maxReward : reward + additionalReward;

                    using (var unitOfWork = _unitOfWorkManage.Begin())
                    {                        
                        //签到表
                        _logRepository.Insert(new SignLog
                        {
                            CreationTime = DateTime.Now,
                            CustomerId = customerId,
                            NumberEntries = lastLog.NumberEntries +1,
                            Reward = reward
                        });
                        //新增积分日志表
                        _rewardRepository.Insert(new CustomerReward
                        {
                            CreationTime = DateTime.Now,
                            CustomerId = customerId,
                            Message = "连续签到赠送积分",
                            Points = reward,
                            Total = total + reward
                        });
                        //存储积分
                        customer.SaveCustomerAttribute<int>(CustomerAttributeNames.Reward, total + reward);
                        unitOfWork.Complete();
                    }
                }
            }
        }

        public bool IsSign(int customerId)
        {

            var query = _logRepository.GetAll();
            query = query.Where(l => l.CustomerId == customerId);


            DateTime sdt = DateTime.Now.Date;
            DateTime dt = DateTime.Now.Date.AddDays(1);

            var result = query.FirstOrDefault(l => l.CreationTime >= sdt && l.CreationTime <= dt);
            return result != null;
        }
        #endregion
    }
}
