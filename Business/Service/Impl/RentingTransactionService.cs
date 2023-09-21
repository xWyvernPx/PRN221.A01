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
        private readonly IRepository<RentingDetail> rentingDetailRepo;
        private readonly ICustomerService customerService;
        public RentingTransactionService(IRepository<RentingTransaction> repository, IRepository<RentingDetail> rentingDetailRepo, ICustomerService customerService) : base(repository)
        {
            this.rentingDetailRepo = rentingDetailRepo;
            this.customerService = customerService;
        }
        public RentingTransactionService() : base(new Repository<RentingTransaction>(new()))
        {
        }
        public int getNextId() {
            return _repository.GetAll().Max(t => t.RentingTransationId) + 1;
        }
        public void DeleteById(int id)
        {
            var transDetails = rentingDetailRepo.GetAll().Where(rd => rd.RentingTransactionId == id);
            //TODO Test
            //foreach (var transDetail in transDetails)
            //{
            //    rentingDetailRepo.Remove(transDetail);
            //}
            rentingDetailRepo.RemoveRange(transDetails);
            var trans = this.GetById(id);
            this.Delete(trans);
        }

        public override IEnumerable<RentingTransaction> GetAll()
        {
            return base.GetAll().Select(rt =>
            {
                var customer = customerService.GetById(rt.CustomerId);
                rt.Customer = customer;
                return rt;
            });
        }

        public override void Update(RentingTransaction rt)
        {
            _repository.Update(c => c.RentingTransationId == rt.RentingTransationId,
                        setter => setter.SetProperty(c => c.TotalPrice, rt.TotalPrice)
                                .SetProperty(c => c.CustomerId, rt.CustomerId)
                                .SetProperty(c => c.RentingStatus, rt.RentingStatus));
            _repository.SaveChanges();
        }
    }
}
