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
        IRepository<RentingDetail> rentingDetailRepository;
        public CarService(IRepository<CarInformation> repository, IRepository<RentingDetail> rentingDetailRepository) : base(repository) {
            this.rentingDetailRepository = rentingDetailRepository;
        }
        public CarService() :base(new Repository<CarInformation>(new FucarRentingManagementContext())) { }

        public override void Delete(CarInformation car)
        {
            bool isUsed = rentingDetailRepository.GetAll().ToList().Any(renting => renting.CarId == car.CarId);
            
            if (isUsed)
            {
                car.CarStatus = 0;
                this.Update(car);
            }
            else { 
                base.Delete(car);
            }
        }
    }
}
