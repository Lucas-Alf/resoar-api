using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Models;
using Infrastructure.Interfaces.Repositories.Domain;

namespace Application.Services.Domain
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public ResponseMessageModel GetById(int id)
        {
            try
            {
                var user = _repository
                    .Query(x => x.Id == id)
                    .Select(x => new UserViewModel
                    {
                        Id = x.Id,
                        Email = x.Email,
                        Name = x.Name,
                        Image = x.ImagePath
                    })
                    .FirstOrDefault();

                if (user == null)
                    throw new NotFoundException();

                return new ResponseMessageModel(user);
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public PaginationModel<UserViewModel> GetPaged(int page, int pageSize)
        {
            var data = _repository.GetPagedAnonymous<UserViewModel>(
                page: page,
                pageSize: pageSize,
                orderBy: x => x.Name!,
                selector: x => new UserViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Image = x.ImagePath
                }
            );

            return data;
        }

        public ResponseMessageModel Update(int userId, UserUpdateModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Name))
                    throw new BusinessException("Nome é obrigatório");

                if (String.IsNullOrEmpty(model.Email))
                    throw new BusinessException("Email é obrigatório");

                var domain = _repository.GetById(userId);
                if (domain == null)
                    throw new BusinessException("Usuário não encontrado.");

                if (model.Email != domain.Email)
                {
                    var emailExists = _repository.Query(x => x.Email == model.Email && x.Id != userId).Any();
                    if (emailExists)
                        throw new BusinessException("Já existe um usuário cadastrado com este e-mail.");
                }

                // !TODO: implementar armazenamento de imagem de perfil
                // if (model.ProfileImage != null)
                // domain.ImagePath = 

                domain.Id = userId;
                domain.Name = model.Name;
                domain.Email = model.Email;

                _repository.Update(domain);

                return new ResponseMessageModel("Registro adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public ResponseMessageModel Remove(int userId)
        {
            try
            {
                var domain = _repository.GetById(userId);
                if (domain == null)
                    throw new BusinessException("Usuário não encontrado.");

                _repository.Remove(domain);
                return new ResponseMessageModel("Registro removido com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }
    }
}
