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
    public class CarService: BaseService<CarInformation>,ICarService
    {
        public CarService(IRepository<CarInformation> repository) : base(repository) { }
        public CarService() :base(new Repository<CarInformation>(new FucarRentingManagementContext())) { }
    }
}
