using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EnazaWebApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        private readonly int _waitCreateSeconds = 5;
        public UserController(IUserService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns users list</response>   
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserShowDto>>> Get()
        {
            return Ok(await _service.GetUsers());
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserShowDto>> Get(int id)
        {
            return Ok(await _service.GetUser(id));
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
            if(taskAdd.Wait(_waitCreateSeconds * 1000))
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
