using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Auth;
using Domain.Entities;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Application.Services.Domain
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public LoginService(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public ResponseMessageModel GetToken(LoginModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Email))
                    throw new BusinessException("Email é obrigatório");

                if (String.IsNullOrEmpty(model.Password))
                    throw new BusinessException("Senha é obrigatório");

#if RELEASE
                if (String.IsNullOrEmpty(model.Token))
                    throw new BusinessException("Token hCaptcha obrigatório");

                if (!ValidateCaptcha(model.Token))
                    throw new BusinessException("Token hCaptcha inválido");
#endif

                var user = _userRepository
                    .Query(new FilterBy<User>(x => x.Email == model.Email))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Password
                    })
                    .FirstOrDefault();

                if (user == null)
                    throw new BusinessException("Usuário não encontrado");

                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                    throw new BusinessException("Usuário ou Senha inválida");

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.User));

                var token = GenerateToken(authClaims);

                return new ResponseMessageModel(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });

            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public ResponseMessageModel Register(UserCreateModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Name))
                    throw new BusinessException("Nome é obrigatório");

                if (String.IsNullOrEmpty(model.Email))
                    throw new BusinessException("Email é obrigatório");

                if (String.IsNullOrEmpty(model.Password))
                    throw new BusinessException("Senha é obrigatório");

                if (!ValidateCaptcha(model.Token))
                    throw new BusinessException("reCAPTCHA inválido");

                var emailExists = _userRepository.Query(new FilterBy<User>(x => x.Email == model.Email)).Any();
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

                _userRepository.Add(domain);

                return new ResponseMessageModel("Registro adicionado com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        public ResponseMessageModel RecoverPassword(RecoverPasswordModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Email))
                    throw new BusinessException("Email é obrigatório");

                if (!ValidateCaptcha(model.Token))
                    throw new BusinessException("reCAPTCHA inválido");

                var user = _userRepository
                    .Query(new FilterBy<User>(x => x.Email == model.Email))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                    .FirstOrDefault();

                if (user == null)
                    throw new BusinessException("Usuário não encontrado");

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.Recovery)
                };

                var token = GenerateToken(authClaims);

                try
                {
                    _emailService.Send(
                        to: model.Email,
                        subject: "Recuperação de senha",
                        html: GetRecoveryPasswordHTML(
                            userName: user.Name,
                            token: new JwtSecurityTokenHandler().WriteToken(token),
                            validTo: token.ValidTo
                        )
                    );
                }
                catch
                {
                    throw new Exception("Não foi possível enviar o e-mail de recuperação de senha");
                }

                return new ResponseMessageModel("Email de recuperação enviado com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        private string GetRecoveryPasswordHTML(string? userName, string token, DateTime validTo)
        {
            var url = EnvironmentManager.GetJwtAudience();
            var recoveryLink = $"{url}/reset-password?token={token}";
            return $@"
            <html>
              <head>
                <link rel=""noopener"" target=""_blank"" href=""https://fonts.googleapis.com/css2?family=Roboto&display=swap"" rel=""stylesheet"">
                <style>
                  .text {{
                    font-family: ""Roboto"",""Helvetica"",""Arial"",sans-serif;
                  }}
                </style>
              </head>
              <body>
                <img width=""240px"" src=""{url}/src/assets/img/resoar/colorfull/fullname.png"">
                <h2 class=""text"">Recuperação de senha</h2>
                <p class=""text"">
                    Olá {userName}!</br>
                    Você está recebendo este email porque solicitou a recuperação da senha do seu usuário.</br>
                    Caso você não tenha solicitado ignore este email.</br>
                    Clique <a href=""{recoveryLink}"">aqui</a> ou acesse o endereço a baixo para criar uma nova senha de acesso.</br>
                    <a href=""{recoveryLink}"">{recoveryLink}</a></br></br>
                    Este endereço será valido até {validTo.ToString("dd/MM/yyyy hh:mm")}.
                </p>
              </body>
            </html>";
        }

        public ResponseMessageModel ResetPassword(int userId, ResetPasswordModel model)
        {
            try
            {
                if (String.IsNullOrEmpty(model.Password))
                    throw new BusinessException("Senha é obrigatório");

                if (String.IsNullOrEmpty(model.ConfirmPassword))
                    throw new BusinessException("Senha é obrigatório");

                if (model.Password.Length < 6)
                    throw new BusinessException("A senha deve ter no mínimo 6 caracteres");

                if (model.Password != model.ConfirmPassword)
                    throw new BusinessException("As senhas não conferem");

                var userExists = _userRepository.Query(new FilterBy<User>(x => x.Id == userId)).Any();
                if (!userExists)
                    throw new BusinessException("Usuário não encontrado");

                _userRepository.UpdateSomeFields(
                    new User
                    {
                        Id = userId,
                        Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
                    },
                    x => x.Password!
                );

                return new ResponseMessageModel("Senha alterada com sucesso");
            }
            catch (Exception ex)
            {
                return new ResponseMessageModel(ex);
            }
        }

        private static JwtSecurityToken GenerateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvironmentManager.GetJwtSecret()));

            var token = new JwtSecurityToken(
                issuer: EnvironmentManager.GetJwtIssuer(),
                audience: EnvironmentManager.GetJwtAudience(),
                expires: DateTime.Now.AddHours(3),
                notBefore: DateTime.Now,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        private bool ValidateCaptcha(string? token)
        {
            if (String.IsNullOrEmpty(token))
                return false;

            try
            {
                using (var client = new HttpClient())
                {
                    var secret = EnvironmentManager.GetCaptchaSecret();
                    var siteKey = EnvironmentManager.GetCaptchaSiteKey();
                    var request = client.PostAsync($"https://hcaptcha.com/siteverify?response={token}&secret={secret}&sitekey={siteKey}", null);
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
                throw new BusinessException("Ocorreu um erro ao validar o hCAPTCHA.");
            }
        }
    }
}
