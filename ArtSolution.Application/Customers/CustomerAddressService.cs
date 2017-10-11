using Abp.Domain.Repositories;
using ArtSolution.Domain.Customers;
using System;
using System.Linq;
using Abp.Application.Services.Dto;

namespace ArtSolution.Customers
{
    public class CustomerAddressService: ArtSolutionAppServiceBase, ICustomerAddressService
    {

        #region Ctor && Field
        
        private readonly IRepository<CustomerAddress> _addressRepository;
        public CustomerAddressService(IRepository<CustomerAddress> addressRepository)
        {
            this._addressRepository = addressRepository;
        }


        #endregion

        #region Method

        public CustomerAddress GetAddressById(int addressId)
        {
            if (addressId > 0)
                return _addressRepository.Get(addressId);

            return null;
        }

        public IPagedResult<CustomerAddress> GetAllAddress(string keywords = "",
            int customerId = 0,
            string province = "", string city = "", string country = "",
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _addressRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(keywords))
                query = query.Where(a =>
                 a.TelNumber.Contains(keywords) ||
                 a.UserName.Contains(keywords) ||
                 a.DetailInfo.Contains(keywords));

            if (!String.IsNullOrWhiteSpace(province))
                query = query.Where(a => a.ProvinceName.Contains(province));

            if (!String.IsNullOrWhiteSpace(city))
                query = query.Where(a => a.CityName.Contains(city));

            if (!String.IsNullOrWhiteSpace(country))
                query = query.Where(a => a.CountryName.Contains(country));

            if (customerId > 0)
                query = query.Where(a => a.CustomerId == customerId);

            query = query.OrderByDescending(a => a.Id);
            return new PagedResult<CustomerAddress>(query, pageIndex, pageSize);
        }

        public int InsertAddress(CustomerAddress address)
        {
            if (address != null)
                return _addressRepository.InsertAndGetId(address);

            return 0;
        }
        #endregion
    }
}