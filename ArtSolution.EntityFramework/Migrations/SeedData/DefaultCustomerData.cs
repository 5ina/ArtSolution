using ArtSolution.Domain.Customers;
using ArtSolution.EntityFramework;
using System;
using System.Linq;

namespace ArtSolution.Migrations.SeedData
{
    public class DefaultCustomerData
    {
        private readonly ArtSolutionDbContext _context;

        public DefaultCustomerData(ArtSolutionDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //CreateCustomer();
        }

        private void CreateCustomer()
        {
            var admin = _context.Customer.FirstOrDefault(e => e.Mobile == "18503223172");
            if (admin == null)
            {
                admin = new Domain.Customers.Customer
                {
                    CustomerRoleId = (int)CustomerRole.System,
                    Mobile = "18503223172",
                    PasswordSalt = "WPIUEZ",
                    Password = "D7CB286678A76D14D69AD990709D74E51B293DC4",
                    CreationTime = DateTime.Now,
                    LastModificationTime = DateTime.Now,
                    OpenId = "",
                    NickName = "",
                    Promoter = 0,                    
                };
            }

            _context.Customer.Add(admin);
            _context.SaveChanges();

        }

    }
}
