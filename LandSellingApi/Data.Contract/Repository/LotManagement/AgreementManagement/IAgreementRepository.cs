﻿using Data.Identity.Repository.Base;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Contract.Repository.LotManagement
{
    public interface IAgreementRepository : IEntityRepository<Agreement>
    {
        Task<IEnumerable<Agreement>> GetByOwnerId(Guid ownerId);
        Task<IEnumerable<Agreement>> GetByMangerId(Guid managerId);
        Task<IEnumerable<Agreement>> GetByCustomerId(Guid customerId);
        Task<IEnumerable<Agreement>> GetByDate(DateTime date);
    }
}
