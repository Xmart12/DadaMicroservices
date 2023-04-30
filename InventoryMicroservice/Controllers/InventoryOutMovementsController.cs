using DadaRepositories.Contexts;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using InventoryMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMicroservice.Controllers
{
    [Route("api/inventory/movements/out")]
    [ApiController]
    public class InventoryOutMovementsController : ControllerBase
    {
        private readonly InventoryMovementsService _service;

        public InventoryOutMovementsController(DadaDbContext context)
        {
            _service = new InventoryMovementsService(context, InventoryMovementType.Out);
        }

        // GET: api/inventory/movements/out
        [HttpGet]
        public async Task<ActionResult<List<InventoryMovement>>> Get()
        {
            return Ok(await _service.GetInventoyMovements());
        }

        // GET api/inventory/movements/out/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovement>> Get(int id)
        {
            return Ok(await _service.GetInventoyMovement(id));
        }

        // POST api/inventory/movements/out
        [HttpPost]
        public async Task<ActionResult<InventoryMovement>> Post([FromBody] InventoryMovement movement)
        {
            InventoryMovement added = await _service.CreateInventoryMovement(movement);

            if (added is null)
            {
                return BadRequest();
            }

            return Ok(added);
        }

        // PUT api/<InventoryOutMovementController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InventoryOutMovementController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
