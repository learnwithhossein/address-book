using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Contacts;
using System.Collections.Generic;
using Service.Common;

namespace Api.Controllers
{
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
        public IActionResult GetAll()
        {
            var result = _repository.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            _repository.Add(contact);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] int id)
        {
            _repository.Delete(id);
            return Ok();
        }

        [HttpPut]
        public IActionResult Update(Contact contact)
        {
            _repository.Update(contact);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = _repository.GetById(id);
                return Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("find")]
        public IActionResult Find([FromQuery] string name, [FromQuery] string phone, [FromQuery] string address)
        {
            var result = _repository.Find(name, phone, address);
            return Ok(result);
        }
    }
}
