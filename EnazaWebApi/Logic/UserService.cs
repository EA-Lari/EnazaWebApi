using EnazaWebApi.Logic.Dto;
using EnazaWebApi.Logic.Interfaces;
using System;
using System.Collections.Generic;
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
    }
}
