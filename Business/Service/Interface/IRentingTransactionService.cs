﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Service.Interface
{
    public interface IRentingTransactionService : IBaseService<RentingTransaction>
    {
    public int getNextId();
        void DeleteById(int id);
    }
}
