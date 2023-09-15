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
    public class ManufacturerService:BaseService<Manufacturer> , IManufacturerService

    {
        public ManufacturerService(IRepository<Manufacturer> repository): base(repository) { }
        public ManufacturerService() : base(new Repository<Manufacturer>(new FucarRentingManagementContext()))
        {
            
        }
    }
}
