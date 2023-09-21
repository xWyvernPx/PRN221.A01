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
    public class CarService : BaseService<CarInformation>, ICarService
    {
        IRepository<RentingDetail> rentingDetailRepository;
        IManufacturerService manufacturerService;
        ISupplierService supplierService;
        public CarService(IRepository<CarInformation> repository, IRepository<RentingDetail> rentingDetailRepository, IManufacturerService manufacturerService, ISupplierService supplierService) : base(repository) {
            this.rentingDetailRepository = rentingDetailRepository;
            this.manufacturerService = manufacturerService;
            this.supplierService = supplierService;
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

        public override IEnumerable<CarInformation> GetAll()
        {
            return _repository.GetAll().Select(s =>
            {
                var manu = manufacturerService.GetById(s.ManufacturerId);
                var supplier = supplierService.GetById(s.SupplierId); 
                s.Supplier = supplier;
                s.Manufacturer = manu;
                return s;
            });
        }

        public IEnumerable<CarInformation> SearchCar(string keywordCarName)
        {
           return this._repository.GetAll().Select(s =>
           {
               var manu = manufacturerService.GetById(s.ManufacturerId);
               var supplier = supplierService.GetById(s.SupplierId);
               s.Supplier = supplier;
               s.Manufacturer = manu;
               return s;
           }).Where(car => car.CarName.Contains(keywordCarName));
        }
        public override void Update(CarInformation car)
        {
            _repository.Update(c => c.CarId == car.CarId,
                        setter => setter.SetProperty(c => c.CarName, car.CarName)
                                .SetProperty(c => c.CarDescription, car.CarDescription)
                                .SetProperty(c => c.NumberOfDoors, car.NumberOfDoors)
                                .SetProperty(c => c.SeatingCapacity, car.SeatingCapacity)
                                .SetProperty(c => c.FuelType, car.FuelType)
                                .SetProperty(c => c.Year, car.Year)
                                .SetProperty(c => c.ManufacturerId, car.ManufacturerId)
                                .SetProperty(c => c.SupplierId, car.SupplierId)
                                .SetProperty(c => c.CarStatus, car.CarStatus)
                                .SetProperty(c => c.CarRentingPricePerDay, car.CarRentingPricePerDay));
            _repository.SaveChanges();
        }
    }
}
