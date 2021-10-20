using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace EnazaWebApi.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login(string login, string password)
        {
            var token = await _service.GetToken(login, password);
            return Ok(token);
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
    }
}
