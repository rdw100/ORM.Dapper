using Microsoft.AspNetCore.Mvc;
using ORM.Dapper.Application.Interfaces;
using ORM.Dapper.Core.Models;
using System.Threading.Tasks;

namespace ORM.Dapper.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        public ShipperController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: api/<ShipperController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await unitOfWork.Shippers.GetAllAsync();
            return Ok(data);
        }

        // GET api/<ShipperController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await unitOfWork.Shippers.GetByIdAsync(id);
            if (data == null) return Ok();
            return Ok(data);
        }

        // POST api/<ShipperController>
        [HttpPost]
        public async Task<IActionResult> Add(Shipper shipper)
        {
            var data = await unitOfWork.Shippers.AddAsync(shipper);
            return Ok(data);
        }

        // PUT api/<ShipperController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Shipper shipper)
        {
            var data = await unitOfWork.Shippers.UpdateAsync(shipper);
            return Ok(data);
        }

        // DELETE api/<ShipperController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await unitOfWork.Shippers.DeleteAsync(id);
            return Ok(data);
        }
    }
}
