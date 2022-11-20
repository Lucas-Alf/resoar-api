using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Domain
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly ICurrentUserService _currentUserService;

        public UserService(
            IUserRepository repository,
            ICurrentUserService currentUserService
        )
        {
            _repository = repository;
            _currentUserService = currentUserService;
        }

        public IQueryable<User> Query(FilterBy<User> filter) => _repository.Query(filter);

        public PaginationModel<UserViewModel> GetPaged(int page, int pageSize, string? name)
        {
            var filter = new FilterBy<User>();
            if (!String.IsNullOrEmpty(name))
                filter.Add(x => EF.Functions.ILike(x.Name!, $"%{name.Trim()}%"));

            var data = _repository.GetPagedAnonymous<UserViewModel>(
                page: page,
                pageSize: pageSize,
                filter: filter,
                orderBy: x => x.Name!,
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

        public ResponseMessageModel GetById(int id)
        {
            try
            {
                var user = _repository
                    .Query(new FilterBy<User>(x => x.Id == id))
                    .Select(x => new UserViewModel
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Name = x.Name,
                        ImagePath = x.ImagePath
                    })
                    .FirstOrDefault();

                if (user == null)
                    throw new NotFoundException();

                return new ResponseMessageModel(user);
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public ResponseMessageModel Update(UserUpdateModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Name))
                    throw new BusinessException("Nome é obrigatório");

                if (String.IsNullOrEmpty(model.Email))
                    throw new BusinessException("Email é obrigatório");

                var currentUserId = _currentUserService.GetId();
                var domain = _repository.GetById(currentUserId);
                if (domain == null)
                    throw new BusinessException("Usuário não encontrado.");

                if (model.Email != domain.Email)
                {
                    var emailExists = _repository.Query(new FilterBy<User>(x => x.Email == model.Email && x.Id != currentUserId)).Any();
                    if (emailExists)
                        throw new BusinessException("Já existe um usuário cadastrado com este e-mail.");
                }

                // !TODO: implementar armazenamento de imagem de perfil
                // if (model.ProfileImage != null)
                // domain.ImagePath = 

                domain.Id = currentUserId;
                domain.Name = model.Name;
                domain.Email = model.Email;

                _repository.Update(domain);

                return new ResponseMessageModel("Registro adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }

        public ResponseMessageModel Remove()
        {
            try
            {
                var currentUserId = _currentUserService.GetId();
                var domain = _repository.GetById(currentUserId);
                if (domain == null)
                    throw new BusinessException("Usuário não encontrado.");

                _repository.Delete(domain);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ErrorMessageModel(ex);
            }
        }
    }
}
