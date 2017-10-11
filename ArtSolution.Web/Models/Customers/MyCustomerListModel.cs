using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ArtSolution.Domain.Customers;
using System;
using System.Collections.Generic;

namespace ArtSolution.Web.Models.Customers
{
    public class MyCustomerListModel
    {
        public MyCustomerListModel()
        {
            this.Customers = new List<CustomerModel>();
        }

        /// <summary>
        /// 推广总量
        /// </summary>
        public int TotalCount { get; set; }

        public int WeekTotal { get; set; }

        public int MonthTotal { get; set; }

        public IList<CustomerModel> Customers { get; set; }

        [AutoMap(typeof(Customer))]
        public class CustomerModel:EntityDto
        {
            public DateTime CreationTime { get; set; }
            public string NickName { get; set; }
        }
    }
}