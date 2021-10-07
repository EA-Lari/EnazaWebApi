using EnazaWebApi.Logic;
using EnazaWebApi.Logic.Dto;
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

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserShowDto>>> Get()
        {
            return await _service.GetUsers();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserShowDto>> Get(int id)
        {
            return await _service.GetUser(id);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] UserEditDto user)
        {
            await _service.Add(user);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UserEditDto user)
        {
            await _service.Edit(user);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok();
        }
    }
}
