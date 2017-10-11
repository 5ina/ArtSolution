using Abp.Application.Services;
using ArtSolution.Authentication.Dto;
using Microsoft.AspNet.Identity;

namespace ArtSolution.Authentication
{

    public class CustomerManager : UserManager<CustomerDto, int>, IApplicationService
    {
        public CustomerManager(CustomerStore store) : base(store)
        {
        }
    }
}
