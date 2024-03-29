﻿using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Domain
{
    public class KeyWordService : ServiceBase<KeyWord>, IKeyWordService
    {
        private readonly IKeyWordRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public KeyWordService(
            IKeyWordRepository repository,
            ICurrentUserService currentUserService
        ) : base(repository)
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public PaginationModel<object> GetPaged(int page, int pageSize, string? description)
        {
            var filter = new FilterBy<KeyWord>();

            if (!String.IsNullOrEmpty(description))
                filter.Add(x => EF.Functions.ILike(x.Description!, $"%{description.Trim()}%"));

            return GetPagedAnonymous<object>(
                page: page,
                pageSize: pageSize,
                orderBy: x => x.Description!,
                filter: filter,
                selector: x => new
                {
                    x.Id,
                    x.Description
                }
            );
        }

        public ResponseMessageModel Add(KeyWordNewModel model)
        {
            try
            {
                var domain = new KeyWord
                {
                    Description = model.Description,
                    CreatedById = _currentUserService.GetId()
                };

                return base.Add(domain);
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public ResponseMessageModel Update(KeyWordUpdateModel model)
        {
            try
            {
                _repository.UpdateSomeFields(
                    new KeyWord
                    {
                        Id = model.Id!.Value,
                        Description = model.Description,
                        ModifiedById = _currentUserService.GetId()
                    },
                    x => x.Description!,
                    x => x.ModifiedById!
                );

                return new ResponseMessageModel("Registro atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }
    }
}
