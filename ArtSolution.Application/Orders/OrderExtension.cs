using ArtSolution.Common;
using ArtSolution.Customers;
using ArtSolution.Domain.Customers;
using ArtSolution.Domain.Orders;
using ArtSolution.Domain.Settings;
using ArtSolution.Names;
using System;
using System.Linq;

namespace ArtSolution.Orders
{
    /// <summary>
    /// 订单扩展服务
    /// </summary>
    public static class OrderExtension
    {

        /// <summary>
        /// 订单返佣
        /// </summary>
        /// <param name="order"></param>
        public static decimal OrderCommission(this Order order,Customer customer, IOrderService orderService)
        {
            decimal amount = 0;
            var settings = GetSettings();
            if (settings.Enabled)//启用推广返佣模式
            {
                //如果有推广人
                if (customer.Promoter > 0)
                {                   
                    var orders = orderService.GetAllOrders(customerId: customer.Id);
                    if (JudgmentCom(customer, settings)) //判定该订单是否应该奖励
                    {
                        var commission = new Commission {
                            CreationTime = DateTime.Now,
                            CustomerId  = customer.Promoter,
                            OrderId = order.Id,
                            OrderTotal = order.Subtotal,
                            Rate = settings.RewardRate,
                            ReturnAmount = order.Subtotal * settings.RewardRate,                            
                        };

                        var commissionService = Abp.Dependency.IocManager.Instance.Resolve<ICommissionService>();
                        commissionService.InsertCommission(commission);
                        //剩下的就是打款的信息的内容了

                        var commiss = customer.GetCustomerAttributeValue<decimal>(CustomerAttributeNames.Commission);

                        commiss = commiss + commission.ReturnAmount;
                        customer.SaveCustomerAttribute(CustomerAttributeNames.Commission, commiss);
                        amount = commission.ReturnAmount;
                    }                   
                }
            }
            return amount;
        }
        //判定该订单是否应该返佣
        private static bool JudgmentCom(Customer customer, PromotersSetting settings)
        {
            var orderService = Abp.Dependency.IocManager.Instance.Resolve<IOrderService>();
            var result = false;
            switch (settings.RewardMode)
            {
                case RewardMode.Date:
                    result = customer.CreationTime.AddDays(settings.ModeValue) < DateTime.Now;
                    break;
                case RewardMode.OrderCount:
                    var orders = orderService.GetAllOrders(customerId: customer.Id);
                    var summary = orders.Items.Where(o => o.OrderStatusId == (int)OrderStatus.Paid
                                        || o.OrderStatusId == (int)OrderStatus.Delivered
                                        || o.OrderStatusId == (int)OrderStatus.Complete).Count();
                    result = summary < settings.ModeValue;
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        private static PromotersSetting GetSettings()
        {
            var settingService = Abp.Dependency.IocManager.Instance.Resolve<ISettingService>();
            return new PromotersSetting
            {
                Enabled = settingService.GetSettingByKey<bool>(PromotersSettings.Enabled),
                ModeValue = settingService.GetSettingByKey<int>(PromotersSettings.ModeValue),
                RewardMode = settingService.GetSettingByKey<RewardMode>(PromotersSettings.RewardMode),
                RewardRate = settingService.GetSettingByKey<decimal>(PromotersSettings.RewardRate),
            };
        }
    }


    public class PromotersSetting
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 奖励模式
        /// </summary>
        public RewardMode RewardMode { get; set; }
        /// <summary>
        /// 奖励值
        /// </summary>
        public int ModeValue { get; set; }
        /// <summary>
        /// 奖励比率
        /// </summary>
        public decimal RewardRate { get; set; }       

    }
}
