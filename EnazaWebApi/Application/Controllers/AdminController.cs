using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnazaWebApi.Application.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUserService _service;

        private readonly int _waitCreateSeconds = 5;

        public AdminController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Add new active User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] UserEditDto user)
        {
            var taskAdd = _service.Add(user);
            if (taskAdd.Wait(_waitCreateSeconds * 1000))
                return Ok();
            return Problem("Long waiting time create User");
        }

        /// <summary>
        /// Update info about User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UserEditDto user)
        {
            await _service.Edit(user);
            return Ok();
        }

        /// <summary>
        /// Blocked user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
