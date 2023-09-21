using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Interface
{
    public interface IRentingDetailService : IBaseService<RentingDetail>
    {
        IEnumerable<RentingDetail> GetRentingDetailsByTransactionId(int transactionId);
    }
}
