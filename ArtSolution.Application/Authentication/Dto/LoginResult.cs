using ArtSolution.Domain.Customers;
using System.Security.Claims;

namespace ArtSolution.Authentication.Dto
{

    public class LoginResult<TCustomer>
    {
        public LoginResult(LoginResults result, Customer customer = null)
        {
            this.Result = result;
            this.Customer = customer;
        }

        public LoginResult(Customer customer, ClaimsIdentity identity) : this(LoginResults.Successful, customer)
        {
            this.Customer = customer;
            this.Identity = identity;
        }

        // Properties
        public ClaimsIdentity Identity { get; private set; }

        public LoginResults Result { get; private set; }

        public Customer Customer { get; private set; }


    }
}
