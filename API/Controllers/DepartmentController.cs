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
    public class DepartmentController : BasesController<Department, DepartmentRepository>
    {
        private readonly DepartmentRepository _repository;
        public DepartmentController(DepartmentRepository departmentRepository) : base (departmentRepository)
        {
            this._repository = departmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Department>> Get()
        {
            var get = await _repository.Get();
            return Ok(new { data = get });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> Get(int Id)
        {
            var get = await _repository.Get(Id);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Department>> Put(int id, Department entity)
        {
            var put = await _repository.Get(id);
            if (put == null)
            {
                return NotFound();
            }
            put.Name = entity.Name;
            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Update Successfully");
        }

        [HttpPost]
        public async Task<ActionResult<Department>> Post(Department entity)
        {
            await _repository.Post(entity);
            return CreatedAtAction("Get", new { id = entity.Id }, entity);
        }

        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Department>> Delete(int id)
        //{
        //    var delete = await _repository.Delete(id);
        //    if (delete == null)
        //    {
        //        return NotFound();
        //    }
        //    return delete;
        //}
    }
}