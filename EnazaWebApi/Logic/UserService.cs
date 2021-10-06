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
        private readonly IValidate _validate;

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
                throw new Exception("Невозможно отредактировать пользователя. Пользователь не найден.");
            Check(userDto);
            await _repository.Update(userDto);
        }

        private void Check(UserEditDto userDto)
        {
            _validate.CheckForCompletenessTest(userDto);
            _validate.CheckLogin(_repository.Users, userDto.Login);
            _validate.CheckState(_repository.Users, userDto);
        }

        public async Task<UserShowDto> GetUser(int userId)
            => await _repository.GetUser(userId);

        public async Task<List<UserShowDto>> GetUsers()
            =>await _repository.GetUsers();
    }
}
