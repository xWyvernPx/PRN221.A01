using Business.DataObject;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.MapperExtensions.Manual
{
    public static class ManualMapperExtensions
    {
        public static RentDetailObject ToRentDetailObject(this RentingDetail rentingDetail)
        => new RentDetailObject
            {
                RentingTransactionId = rentingDetail.RentingTransactionId,
                StartDate = rentingDetail.StartDate,
                EndDate = rentingDetail.EndDate,
                Price = rentingDetail.Price,
                CarId = rentingDetail.CarId,
                CarName = rentingDetail.Car?.CarName
            };
        public static RentingTransObject ToRentingTransObject(this RentingTransaction rentingTrans)
       => new RentingTransObject
       {
           RentingTransationId = rentingTrans.RentingTransationId,
           RentingDate = rentingTrans.RentingDate,
           TotalPrice = rentingTrans.TotalPrice,
           RentingStatus = rentingTrans.RentingStatus,
           CustomerId = rentingTrans.CustomerId,
           CustomerName = rentingTrans.Customer?.CustomerName
       };
        public static RentingTransaction ToRentingTransaction(this RentingTransObject transObj)
        {
            return new RentingTransaction
            {
                RentingTransationId = transObj.RentingTransationId,
                RentingDate = transObj.RentingDate,
                TotalPrice = transObj.TotalPrice,
                RentingStatus = transObj.RentingStatus,
                CustomerId = transObj.CustomerId
            };
        }
    }
        
}
