﻿using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class ResearchKeyWordRepository : DomainRepository<ResearchKeyWord>, IResearchKeyWordRepository
    {
        public ResearchKeyWordRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}