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
    public class RentingDetailService : BaseService<RentingDetail>, IRentingDetailService
    {
        private IRepository<RentingDetail> rentingDetailRepository;
        private IRepository<RentingTransaction> rentingTransRepository;
        public RentingDetailService(IRepository<RentingDetail> repository, IRepository<RentingDetail> rentingDetailRepository, IRepository<RentingTransaction> rentingTransRepository) : base(repository)
        {
            this.rentingDetailRepository = rentingDetailRepository;
            this.rentingTransRepository = rentingTransRepository;      
        }
        public RentingDetailService() : base(new Repository<RentingDetail>(new FucarRentingManagementContext())) { }
        public IEnumerable<RentingDetail> GetRentingDetailsByTransactionId(int transactionId)
        {
            var trans = rentingTransRepository.GetById(transactionId);
            if(trans is not null)
            {
            return rentingDetailRepository.GetAll().Where(detail => detail.RentingTransactionId == transactionId);
            }
            else
            {
                throw new Exception("Transaction does not exist no more");
            }
        }
    }
}
