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
    public class CustomerService :BaseService<Customer>, ICustomerService
    {
        //private readonly IRepository<Customer> _repository;
        public CustomerService(IRepository<Customer> repository) : base(repository)
        {
        }
        public CustomerService() : base(new Repository<Customer>(new FucarRentingManagementContext()))
        {
            // Additional initialization code for CustomerService if needed
        }
        public List<Customer> GetCustomers()
        {
            return _repository.GetAll().ToList();
        }
    }
}
