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
    public class RentingTransactionService : BaseService<RentingTransaction>, IRentingTransactionService
    {
        public RentingTransactionService(IRepository<RentingTransaction> repository) : base(repository)
        {
        }
        public RentingTransactionService() : base(new Repository<RentingTransaction>(new()))
        {
        }
    }
}
