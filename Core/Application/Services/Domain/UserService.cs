using System.Net;
using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Application.Services.Standard;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Newtonsoft.Json;

namespace Application.Services.Domain
{
    public class UserService : ServiceBase<User>, IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public new ResultMessageModel GetById(int id)
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

                return new ResultMessageModel(user);
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
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

        public ResultMessageModel Add(UserCreateModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Token))
                    throw new BusinessException("reCAPTCHA é obrigatório");

                if (!ValidateReCaptcha(model.Token))
                    throw new BusinessException("reCAPTCHA inválido");

                var emailExists = _repository.Query(x => x.Email == model.Email).Any();
                if (emailExists)
                    throw new BusinessException("Já existe um usuário cadastrado com este e-mail.");

                if (String.IsNullOrEmpty(model.Password))
                    throw new BusinessException("O campo senha é obrigatório.");

                if (model.Password.Length < 6)
                    throw new BusinessException("A senha deve ter no mínimo 6 caracteres.");

                var domain = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };

                return base.Add(domain);
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        public ResultMessageModel Update(int userId, UserUpdateModel model)
        {
            try
            {
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

                return base.Update(domain);
            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
            }
        }

        private bool ValidateReCaptcha(string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var secret = EnvironmentManager.GetReCaptchaSecret();
                    var request = client.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={token}", null);
                    request.Wait();

                    var response = request.Result.Content.ReadAsStringAsync();
                    response.Wait();

                    var result = JsonConvert.DeserializeObject<ReCaptchaResponseModel>(response.Result);
                    if (result != null && result.Success)
                        return true;

                    return false;
                }
            }
            catch
            {
                throw new BusinessException("Ocorreu um erro ao validar o reCAPTCHA.");
            }
        }
    }
}
