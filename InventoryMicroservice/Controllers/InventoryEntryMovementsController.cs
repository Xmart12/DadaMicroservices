using DadaRepositories.Contexts;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using InventoryMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InventoryMicroservice.Controllers
{
    [Route("api/inventory/movements/entry")]
    [ApiController]
    public class InventoryEntryMovementsController : ControllerBase
    {
        private readonly InventoryMovementsService _service;

        public InventoryEntryMovementsController(DadaDbContext context)
        {
            _service = new InventoryMovementsService(context, InventoryMovementType.Entry);
        }

        // GET: api/inventory/movements/entry
        [HttpGet]
        public async Task<ActionResult<List<InventoryMovement>>> Get()
        {
            return Ok(await _service.GetInventoyMovements());
        }

        // GET api/inventory/movements/entry/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovement>> Get(int id)
        {
            return Ok(await _service.GetInventoyMovement(id));
        }

        // POST api/inventory/movements/entry
        [HttpPost]
        public async Task<ActionResult<InventoryMovement>> Post([FromBody] InventoryMovement movement)
        {
            (InventoryMovement added, string message) = await _service.CreateInventoryMovement(movement);

            if (added is null)
            {
                return BadRequest(message);
            }

            return Ok(added);
        }

        // PUT api/<InventoryEntryMovementController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<InventoryMovement>> Put(int id, [FromBody] InventoryMovement movement)
        {
            (InventoryMovement updated, string message) = await _service.UpdateInventoryMovement(movement);

            if (updated is null)
            {
                return BadRequest(message);
            }

            return Ok(updated);
        }
    }
}
