using ArtSolution.Api.Models.Customers;
using ArtSolution.Domain.Customers;
using System.Collections.Generic;
using System.Web.Http;

namespace ArtSolution.Api.Customers
{
    public interface ICustomerApiService:IApiService
    {
        [HttpGet]
        IList<Customer> GetAllCustomers();
    }
}
