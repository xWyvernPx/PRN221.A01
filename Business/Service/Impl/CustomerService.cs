using Business.Service.Interface;
using Domain.Models;
using Infra.Implement;
using Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Impl
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        //private readonly IRepository<Customer> _repository;
        public CustomerService(IRepository<Customer> repository) : base(repository)
        {
        }
        public CustomerService() : base(new Repository<Customer>(new FucarRentingManagementContext()))
        {
            // Additional initialization code for CustomerService if needed
        }
        public int getNextId()
        {
            return _repository.GetAll().MaxBy(cus => cus.CustomerId)?.CustomerId + 1 ?? -1;
        }
        public List<Customer> GetCustomers()
        {
            return _repository.GetAll().ToList();
        }

        public Customer? GetCustomerByEmail(string email)
        {
            return _repository.GetAll()
                .FirstOrDefault(customer => customer.Email.Equals(email));
        }

        public Customer? CheckAuth(string email, string password)
        {
            var user = this.GetCustomerByEmail(email);
            if (user != null)
            {
                return password.Equals(user.Password) ? user : null;
            }
            return user;
        }

        public override void Update(Customer customer)
        {
            _repository.Update(c => c.Email == customer.Email,
                        setter => setter.SetProperty(c => c.CustomerBirthday, customer.CustomerBirthday)
                                .SetProperty(c => c.CustomerName, customer.CustomerName)
                                .SetProperty(c => c.CustomerStatus, customer.CustomerStatus)
                                .SetProperty(c => c.Password, customer.Password)
                                .SetProperty(c => c.Telephone, customer.Telephone));
            _repository.SaveChanges();
        }
    }
}
