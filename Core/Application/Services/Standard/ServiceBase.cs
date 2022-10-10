using System.Linq.Expressions;
using Application.Exceptions;
using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Extensions;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Standard;

namespace Application.Services.Standard
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class, IIdentityEntity
    {
        protected readonly IRepositoryBase<TEntity> repository;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            this.repository = repository;
        }

        public virtual IQueryable<TEntity> Query(FilterBy<TEntity>? filter = null)
        {
            return repository.Query(filter);
        }

        public virtual ResponseMessageModel Add(TEntity obj)
        {
            try
            {
                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new BusinessException(validation);

                repository.Add(obj);
                return new ResponseMessageModel("Registro adicionado com sucesso", new { Id = obj.Id });
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new BusinessException(validation);
                }

                repository.AddRange(entities);
                return new ResponseMessageModel("Registros adicionados com sucesso", new { Ids = entities.Select(x => x.Id).ToList() });
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual IEnumerable<TEntity> GetAll(FilterBy<TEntity>? filter = null)
        {
            return repository.GetAll(filter);
        }

        public virtual PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            return repository.GetPaged(page, pageSize, orderBy, filter);
        }

        public virtual PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            return repository.GetPagedAnonymous(page, pageSize, selector, orderBy, filter);
        }

        public virtual ResponseMessageModel GetById(int id)
        {
            try
            {
                var data = repository.GetById(id);
                if (data == null)
                    throw new NotFoundException();

                return new ResponseMessageModel(data);
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel Delete(int id)
        {
            try
            {
                var exits = repository.Query(new FilterBy<TEntity>(x => x.Id == id)).Any();
                if (!exits)
                    throw new NotFoundException();

                repository.Delete(id);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel Delete(TEntity obj)
        {
            try
            {
                var exits = repository.Query(new FilterBy<TEntity>(x => x.Id == obj.Id)).Any();
                if (!exits)
                    throw new NotFoundException();

                repository.Delete(obj);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel DeleteRange(IEnumerable<TEntity> entities)
        {
            try
            {
                repository.DeleteRange(entities);
                return new ResponseMessageModel("Registros removidos com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel Update(TEntity obj)
        {
            try
            {
                var exits = repository.Query(new FilterBy<TEntity>(x => x.Id == obj.Id)).Any();
                if (!exits)
                    throw new NotFoundException();

                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new BusinessException(validation);

                repository.Update(obj);
                return new ResponseMessageModel("Registro atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new BusinessException(validation);
                }

                repository.UpdateRange(entities);
                return new ResponseMessageModel("Registros atualizados com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> AddAsync(TEntity obj)
        {
            try
            {
                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new BusinessException(validation);

                await repository.AddAsync(obj);
                return new ResponseMessageModel("Registro adicionado com sucesso", new { Id = obj.Id });
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new BusinessException(validation);
                }

                await repository.AddRangeAsync(entities);
                return new ResponseMessageModel("Registros adicionados com sucesso", new { Ids = entities.Select(x => x.Id).ToList() });
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(FilterBy<TEntity>? filter = null)
        {
            return await repository.GetAllAsync(filter);
        }

        public virtual async Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            return await repository.GetPagedAsync(page, pageSize, orderBy, filter);
        }

        public virtual async Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, FilterBy<TEntity>? filter = null)
        {
            return await repository.GetPagedAnonymousAsync(page, pageSize, selector, orderBy, filter);
        }

        public virtual async Task<ResponseMessageModel> GetByIdAsync(int id)
        {
            try
            {
                var data = await repository.GetByIdAsync(id);
                if (data == null)
                    throw new NotFoundException();

                return new ResponseMessageModel(data);
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> DeleteAsync(int id)
        {
            try
            {
                var exits = repository.Query(new FilterBy<TEntity>(x => x.Id == id)).Any();
                if (!exits)
                    throw new NotFoundException();

                await repository.DeleteAsync(id);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> DeleteAsync(TEntity obj)
        {
            try
            {
                var exits = repository.Query(new FilterBy<TEntity>(x => x.Id == obj.Id)).Any();
                if (!exits)
                    throw new NotFoundException();

                await repository.DeleteAsync(obj);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await repository.DeleteRangeAsync(entities);
                return new ResponseMessageModel("Registros removidos com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> UpdateAsync(TEntity obj)
        {
            try
            {
                var exits = repository.Query(new FilterBy<TEntity>(x => x.Id == obj.Id)).Any();
                if (!exits)
                    throw new NotFoundException();

                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new BusinessException(validation);

                await repository.UpdateAsync(obj);
                return new ResponseMessageModel("Registro atualizado com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new BusinessException(validation);
                }

                await repository.UpdateRangeAsync(entities);
                return new ResponseMessageModel("Registros atualizados com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }
    }
}
