using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DataObject {
    public class RentingTransObject {
        public int RentingTransationId { get; set; }

        public DateTime? RentingDate { get; set; }

        public decimal? TotalPrice { get; set; }

        public byte? RentingStatus { get; set; }

        public  int CustomerId { get; set; }
        public string CustomerName { get; set; }

    }
}
