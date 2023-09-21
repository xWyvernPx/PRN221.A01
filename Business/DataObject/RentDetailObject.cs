using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DataObject {
    public class RentDetailObject {
        public int RentingTransactionId { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal? Price { get; set; }

        public int CarId { get; set; } 
        public string CarName { get; set; }

    }
}
