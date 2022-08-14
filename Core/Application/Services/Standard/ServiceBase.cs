using Application.Exceptions;
using Application.Interfaces.Services.Standard;
using Domain.Entities;
using Domain.Extensions;
using Domain.Models;
using Infrastructure.Interfaces.Repositories.Standard;
using System.Linq.Expressions;

namespace Application.Services.Standard
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class, IIdentityEntity
    {
        protected readonly IRepositoryBase<TEntity> repository;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            this.repository = repository;
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>>? filter = null)
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
                return new ResponseMessageModel("Registro adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
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
                return new ResponseMessageModel("Registros adicionados com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            return repository.GetAll(filter);
        }

        public virtual PaginationModel<TEntity> GetPaged(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return repository.GetPaged(page, pageSize, orderBy, filter);
        }

        public virtual PaginationModel<T> GetPagedAnonymous<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return repository.GetPagedAnonymous(page, pageSize, selector, orderBy, filter);
        }

        public virtual TEntity? GetById(int id)
        {
            return repository.GetById(id);
        }

        public virtual ResponseMessageModel Remove(int id)
        {
            try
            {
                var exits = repository.Query(x => x.Id == id).Any();
                if (!exits)
                    throw new NotFoundException();

                repository.Remove(id);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel Remove(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
                if (!exits)
                    throw new NotFoundException();

                repository.Remove(obj);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel RemoveRange(IEnumerable<TEntity> entities)
        {
            try
            {
                repository.RemoveRange(entities);
                return new ResponseMessageModel("Registros removidos com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual ResponseMessageModel Update(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
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
                return new ResponseMessageModel(ex);
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
                return new ResponseMessageModel(ex);
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
                return new ResponseMessageModel("Registro adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
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
                return new ResponseMessageModel("Registros adicionados com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return await repository.GetAllAsync(filter);
        }

        public virtual async Task<PaginationModel<TEntity>> GetPagedAsync(int page, int pageSize, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return await repository.GetPagedAsync(page, pageSize, orderBy, filter);
        }

        public virtual async Task<PaginationModel<T>> GetPagedAnonymousAsync<T>(int page, int pageSize, Expression<Func<TEntity, T>> selector, Expression<Func<TEntity, object>>? orderBy = null, Expression<Func<TEntity, bool>>? filter = null)
        {
            return await repository.GetPagedAnonymousAsync(page, pageSize, selector, orderBy, filter);
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public virtual async Task<ResponseMessageModel> RemoveAsync(int id)
        {
            try
            {
                var exits = repository.Query(x => x.Id == id).Any();
                if (!exits)
                    throw new NotFoundException();

                await repository.RemoveAsync(id);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> RemoveAsync(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
                if (!exits)
                    throw new NotFoundException();

                await repository.RemoveAsync(obj);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await repository.RemoveRangeAsync(entities);
                return new ResponseMessageModel("Registros removidos com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public virtual async Task<ResponseMessageModel> UpdateAsync(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
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
                return new ResponseMessageModel(ex);
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
                return new ResponseMessageModel(ex);
            }
        }
    }
}
