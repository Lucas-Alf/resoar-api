﻿using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class KeyWordRepository : DomainRepository<KeyWord>, IKeyWordRepository
    {
        public KeyWordRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}