using Application.Interfaces.Services.Standard;
using Domain.Entities;
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

        public virtual ResultMessageModel Add(TEntity obj)
        {
            try
            {
                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new Exception(validation);

                repository.Add(obj);
                return new ResultMessageModel("Registro adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual ResultMessageModel AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new Exception(validation);
                }

                repository.AddRange(entities);
                return new ResultMessageModel("Registros adicionados com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
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

        public virtual TEntity GetById(int id)
        {
            return repository.GetById(id);
        }

        public virtual ResultMessageModel Remove(int id)
        {
            try
            {
                var exits = repository.Query(x => x.Id == id).Any();
                if (!exits)
                    throw new Exception("Registro não encontrado.");

                repository.Remove(id);
                return new ResultMessageModel("Registro removido com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual ResultMessageModel Remove(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
                if (!exits)
                    throw new Exception("Registro não encontrado.");

                repository.Remove(obj);
                return new ResultMessageModel("Registro removido com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual ResultMessageModel RemoveRange(IEnumerable<TEntity> entities)
        {
            try
            {
                repository.RemoveRange(entities);
                return new ResultMessageModel("Registros removidos com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual ResultMessageModel Update(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
                if (!exits)
                    throw new Exception("Registro não encontrado.");

                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new Exception(validation);

                repository.Update(obj);
                return new ResultMessageModel("Registro atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual ResultMessageModel UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new Exception(validation);
                }

                repository.UpdateRange(entities);
                return new ResultMessageModel("Registros atualizados com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual async Task<ResultMessageModel> AddAsync(TEntity obj)
        {
            try
            {
                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new Exception(validation);

                await repository.AddAsync(obj);
                return new ResultMessageModel("Registro adicionado com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual async Task<ResultMessageModel> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new Exception(validation);
                }

                await repository.AddRangeAsync(entities);
                return new ResultMessageModel("Registros adicionados com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
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

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await repository.GetByIdAsync(id);
        }

        public virtual async Task<ResultMessageModel> RemoveAsync(int id)
        {
            try
            {
                var exits = repository.Query(x => x.Id == id).Any();
                if (!exits)
                    throw new Exception("Registro não encontrado.");

                await repository.RemoveAsync(id);
                return new ResultMessageModel("Registro removido com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual async Task<ResultMessageModel> RemoveAsync(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
                if (!exits)
                    throw new Exception("Registro não encontrado.");

                await repository.RemoveAsync(obj);
                return new ResultMessageModel("Registro removido com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual async Task<ResultMessageModel> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await repository.RemoveRangeAsync(entities);
                return new ResultMessageModel("Registros removidos com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual async Task<ResultMessageModel> UpdateAsync(TEntity obj)
        {
            try
            {
                var exits = repository.Query(x => x.Id == obj.Id).Any();
                if (!exits)
                    throw new Exception("Registro não encontrado.");

                var validation = obj.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(validation))
                    throw new Exception(validation);

                await repository.UpdateAsync(obj);
                return new ResultMessageModel("Registro atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public virtual async Task<ResultMessageModel> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var obj in entities)
                {
                    var validation = obj.GetValidationErrorMessages();
                    if (!String.IsNullOrEmpty(validation))
                        throw new Exception(validation);
                }

                await repository.UpdateRangeAsync(entities);
                return new ResultMessageModel("Registros atualizados com sucesso.");
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }
    }
}
