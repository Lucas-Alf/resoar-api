using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces.Repositories.Domain;

namespace Application.Services.Domain
{
    public class UserService : ServiceBase<User>, IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public PaginationModel<object> GetPaged(int page, int pageSize)
        {
            var data = _repository.GetPagedAnonymous(
                page: page,
                pageSize: pageSize,
                orderBy: x => x.Name,
                selector: x => new UserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    ImagePath = x.ImagePath
                }
            );

            return data;
        }
    }
}
