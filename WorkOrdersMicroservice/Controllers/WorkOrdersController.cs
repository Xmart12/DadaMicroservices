using DadaRepositories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkOrdersMicroservice.Services;

namespace WorkOrdersMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        private readonly WorkOrdersService _service;

        public WorkOrdersController(IConfiguration configuration)
        {
            _service = new WorkOrdersService(configuration);
        }

        // GET: api/workorders
        [HttpGet]
        public async Task<ActionResult<List<WorkOrder>>> Get()
        {
            return Ok(await _service.GetWorkOrders());
        }

        // GET api/workorders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovement>> Get(string id)
        {
            return Ok(await _service.GetWorkOrder(id));
        }

        // POST api/workorders/entry
        [HttpPost]
        public async Task<ActionResult<InventoryMovement>> Post([FromBody] WorkOrder work)
        {
            WorkOrder added = await _service.CreateWorkOrder(work);

            if (added is null)
            {
                return BadRequest();
            }

            return Ok(added);
        }

        // PUT api/workorders/{id}
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/workorders/{id}
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
