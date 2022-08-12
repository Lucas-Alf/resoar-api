using Application.Exceptions;
using Application.Interfaces.Services.Domain;
using Domain.Auth;
using Domain.Extensions;
using Domain.Models;
using Domain.Utils;
using Infrastructure.Interfaces.Repositories.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Domain
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _repository;

        public LoginService(IUserRepository repository)
        {
            _repository = repository;
        }

        public ResultMessageModel GetToken(LoginModel model)
        {
            try
            {
                var modelValidation = model.GetValidationErrorMessages();
                if (!String.IsNullOrEmpty(modelValidation))
                    throw new BusinessException(modelValidation);

                var user = _repository
                    .Query(x => x.Email == model.Email)
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

                return new ResultMessageModel(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });

            }
            catch (Exception ex)
            {
                return new ResultMessageModel(ex);
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
    }
}
