using Api.Common;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Contacts;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactRepository _repository;

        public ContactController(ContactRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repository.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Contact contact)
        {
            await _repository.Add(contact);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            await _repository.Delete(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(Contact contact)
        {
            await _repository.Update(contact);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repository.GetById(id);
            return Ok(result);
        }

        [HttpGet("find")]
        public async Task<IActionResult> Find([FromQuery] string name, [FromQuery] string phone,
            [FromQuery] string address, [FromQuery] int pageNumber, [FromQuery] int pageSize,
            [FromQuery] string orderBy, [FromQuery] string sort)
        {
            var result = await _repository.Find(name, phone, address, pageNumber, pageSize, orderBy, sort);
            Response.AddPagination(result.Pagination);

            return Ok(result);
        }
    }
}
