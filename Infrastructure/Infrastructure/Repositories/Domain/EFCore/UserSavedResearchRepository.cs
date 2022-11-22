﻿using Domain.Entities;
using Infrastructure.DBConfiguration.EFCore;
using Infrastructure.Interfaces.Repositories.Domain;
using Infrastructure.Repositories.Standard.EFCore;

namespace Infrastructure.Repositories.Domain.EFCore
{
    public class UserSavedResearchRepository : DomainRepository<UserSavedResearch>, IUserSavedResearchRepository
    {
        public UserSavedResearchRepository(ApplicationContext dbContext) : base(dbContext)
        {

        }
    }
}