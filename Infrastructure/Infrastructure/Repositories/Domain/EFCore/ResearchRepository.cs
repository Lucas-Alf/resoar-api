﻿using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class ResearchRepository : DomainRepository<Research>, IResearchRepository
    {
        public ResearchRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}