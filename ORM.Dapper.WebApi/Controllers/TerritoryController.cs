using Microsoft.AspNetCore.Mvc;
using ORM.Dapper.Application.Interfaces;
using ORM.Dapper.Core.Models;
using System.Threading.Tasks;

namespace ORM.Dapper.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoryController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public TerritoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/<TerritoryController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await unitOfWork.Territories.GetAllAsync();
            return Ok(data);
        }

        // GET api/<TerritoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.Territories.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }

        // POST api/<TerritoryController>
        [HttpPost]
        public async Task<IActionResult> Add(Territory territory)
        {
            var data = await unitOfWork.Territories.AddAsync(territory);
            return Ok(data);
        }

        // PUT api/<TerritoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Territory territory)
        {
            var data = await unitOfWork.Territories.UpdateAsync(territory);
            return Ok(data);
        }

        // DELETE api/<TerritoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Territories.DeleteAsync(id);
            return Ok(data);
        }
    }
}
