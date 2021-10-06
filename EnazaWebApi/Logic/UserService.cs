using AutoMapper;
using EnazaWebApi.Data;
using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnazaWebApi.Logic
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;
        private readonly IValidate _validate;
        private readonly IMapper _mapper;

        public async Task Add(UserEditDto userDto)
        {
            Check(userDto);
            var user = _mapper.Map<User>(userDto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task SetStateBlocked(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            user.UserStateId = await _context.States
                .Where(x => x.Code == Data.Enums.UserStateCodeEnum.Blocked)
                .Select(x => x.UserStateId)
                .FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
        }

        public async Task Edit(UserEditDto userDto)
        {
            if (!_context.Users.Any(x => x.UserId == userDto.UserId))
                throw new Exception("Невозможно отредактировать пользователя. Пользователь не найден.");
            Check(userDto);
            var user = _context.Users
                .Include(x => x.Group)
                .FirstOrDefault(x => x.UserId == userDto.UserId);
            _mapper.Map(userDto, user);
            await _context.SaveChangesAsync();
        }

        private void Check(UserEditDto userDto)
        {
            _validate.CheckForCompletenessTest(userDto);
            _validate.CheckLogin(_context.Users, userDto.Login);
            _validate.CheckState(_context.Users, userDto);
        }

        public async Task<UserShowDto> GetUser(int userId)
        {
            var user = await _context.Users
                .Include(x => x.Group)
                .Include(x => x.State)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            var userDto = _mapper.Map<UserShowDto>(user);
            return userDto;
        }

        public async Task<List<UserShowDto>> GetUsers()
        {
            var users = await _context.Users
               .Include(x => x.Group)
               .Include(x => x.State)
               .ToListAsync();
            var userList = users
                .Select(x => _mapper.Map<UserShowDto>(x))
                .ToList();
            return userList;
        }

        private int GetStateActive()
        {

        }
    }
}
