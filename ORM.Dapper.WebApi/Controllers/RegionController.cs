using Microsoft.AspNetCore.Mvc;
using ORM.Dapper.Application.Interfaces;
using ORM.Dapper.Core.Models;
using System.Threading.Tasks;

namespace ORM.Dapper.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public RegionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/<RegionController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await unitOfWork.Regions.GetAllAsync();
            return Ok(data);
        }

        // GET: api/<RegionController>/5/terr
        [HttpGet("{id}/terr")]
        public async Task<IActionResult> GetTerritoriesByRegion(int id)
        {
            var data = await unitOfWork.Regions.GetTerritoriesByRegion(id);
            if (data == null) return Ok();
            return Ok(data);
        }

        // GET api/<RegionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.Regions.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }

        // POST api/<RegionController>
        [HttpPost]
        public async Task<IActionResult> Add(Region region)
        {
            var data = await unitOfWork.Regions.AddAsync(region);
            return Ok(data);
        }

        // PUT api/<RegionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Region region)
        {
            var data = await unitOfWork.Regions.UpdateAsync(region);
            return Ok(data);
        }

        // DELETE api/<RegionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Regions.DeleteAsync(id);
            return Ok(data);
        }
    }
}
