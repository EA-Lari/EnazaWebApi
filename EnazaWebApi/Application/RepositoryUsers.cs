using AutoMapper;
using EnazaWebApi.Data;
using EnazaWebApi.Data.Enums;
using EnazaWebApi.Data.Models;
using EnazaWebApi.Logic.Dto;
using EnazaWebApi.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EnazaWebApi.Application
{
    public class RepositoryUsers : IRepositoryUsers
    {
        private readonly IMapper _mapper;
        private readonly UserContext _context;

        public async Task Add(UserEditDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.UserStateId = await GetStateByCode(UserStateCodeEnum.Active);
            user.UserGroupId = await GetGroupByCode(userDto.Group);
            user.CreateDate = DateTime.Now;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public bool Any(Expression<Func<User, bool>> predicate)
            => _context.Users.Any(predicate);

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

        public async Task SetBlockState(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            user.UserStateId = await GetStateByCode(UserStateCodeEnum.Blocked);
            await _context.SaveChangesAsync();
        }

        public async Task Update(UserEditDto userDto)
        {
            var user = _context.Users
               .FirstOrDefault(x => x.UserId == userDto.UserId);
            _mapper.Map(userDto, user);
            user.UserGroupId = await GetGroupByCode(userDto.Group);
            await _context.SaveChangesAsync();
        }

        private async Task<int> GetStateByCode(UserStateCodeEnum code)
            => await _context.States
                .Where(x => x.Code == code)
                .Select(x => x.UserStateId)
                .FirstOrDefaultAsync();

        private async Task<int> GetGroupByCode(UserGroupCodeEnum code)
            => await _context.Groups
                .Where(x => x.Code == code)
                .Select(x => x.UserGroupId)
                .FirstOrDefaultAsync();
    }
}
