using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Base;
using API.Models;
using API.Repository.Data;
using API.RepositoryContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BasesController<Employee, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;

        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<EmployeeVM>> Get()
        {
            var get = await _repository.GetAll();
            return Ok(new { data = get });
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<EmployeeVM>> Get(string email)
        {
            var get = await _repository.Get(email);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }

        [HttpPut("{email}")]
        public async Task<ActionResult<EmployeeVM>> Put(string email, Employee entity)
        {
            var put = await _repository.Get(email);
            if (put == null)
            {
                return NotFound();
            }
            put.FirstName = entity.FirstName;
            put.LastName = entity.LastName;
            put.Email = entity.Email;
            put.BirthDate = entity.BirthDate;
            put.PhoneNumber = entity.PhoneNumber;
            put.Address = entity.Address;
            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Update Successfully");
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeVM>> Post(Employee entity)
        {
            await _repository.Post(entity);
            return CreatedAtAction("Get", new { Email = entity.Email}, entity);
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult<EmployeeVM>> Delete(string email)
        {
            var del = await _repository.Delete(email);
            if (del == null)
            {
                return NotFound();
            }
            return Ok(del);
        }
    }
}