using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Business.Service.Interface
{
    public interface ICustomerService:IBaseService<Customer>
    {
        List<Customer> GetCustomers();
        Customer GetCustomerByEmail(string email);
        Customer CheckAuth(string email, string password);
        int getNextId();
        IEnumerable<Customer> SearchCustomer(string type, string keyword);
    }
}
