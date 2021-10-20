using EnazaWebApi.Auth;
using EnazaWebApi.Logic.Dto;
using EnazaWebApi.Logic.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EnazaWebApi.Logic
{
    public class UserService : IUserService
    {
        private readonly IRepositoryUsers _repository;

        public UserService(IRepositoryUsers repository)
        {
            _repository = repository;
        }

        public async Task Add(UserEditDto userDto)
        {
            Check(userDto);
            await _repository.Add(userDto);
        }

        public async Task Delete(int userId)
        {
            await _repository.SetBlockState(userId);
        }

        public async Task Edit(UserEditDto userDto)
        {
            if(!userDto.UserId.HasValue)
                throw new ArgumentNullException("Отсутствует идентификатор");
            if (!_repository.Any(x => x.UserId == userDto.UserId))
                throw new ArgumentNullException("Невозможно отредактировать пользователя. Пользователь не найден.");
            Check(userDto);
            await _repository.Update(userDto);
        }

        private void Check(UserEditDto userDto)
        {
            CheckForCompletenessTest(userDto);
            CheckLogin(userDto);
            CheckGroup(userDto);
        }

        private void CheckForCompletenessTest(UserEditDto userDto)
        {
            if (userDto.UserId.HasValue)
                return;
            if (string.IsNullOrEmpty(userDto.Login) || string.IsNullOrEmpty(userDto.Password))
                throw new ArgumentException("Указаны не все обязательные поля");
        }

        private void CheckLogin(UserEditDto userDto)
        {
            if(_repository.Any(x => x.Login == userDto.Login && x.UserId != userDto.UserId))
                throw new ArgumentException($"Логин {userDto.Login} занят, необходимо выбрать другой");
        }

        private void CheckGroup(UserEditDto userDto)
        {
            if (userDto.Group != Data.Enums.UserGroupCodeEnum.Admin)
                return;
            if(_repository.Any(x => x.Group.Code == Data.Enums.UserGroupCodeEnum.Admin 
                && x.UserId != userDto.UserId 
                && x.State.Code == Data.Enums.UserStateCodeEnum.Active))
                    throw new ArgumentException($"Не может быть более одного активного администратора");
        }

        public async Task<UserShowDto> GetUser(int userId)
            => await _repository.GetUser(userId);

        public async Task<List<UserShowDto>> GetUsers()
            =>await _repository.GetUsers();

        public async Task<string> GetToken(string login, string password)
        {
            var identity = await GetIdentity(login, password);
            if (identity == null)
            {
                throw new Exception( "Invalid username or password." );
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            var person = await _repository.CheckLogin(login, password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Group.Code.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
