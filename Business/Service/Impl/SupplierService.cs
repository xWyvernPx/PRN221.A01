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
    public class SupplierService: BaseService<Supplier>, ISupplierService
    {
        public SupplierService(IRepository<Supplier> repository) : base(repository) { }
        public SupplierService() : base(new Repository<Supplier>(new FucarRentingManagementContext())) { }
    }
}
