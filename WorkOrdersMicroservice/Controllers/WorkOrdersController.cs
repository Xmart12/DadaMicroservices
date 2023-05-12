using DadaRepositories.Contexts;
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

        public WorkOrdersController(DadaDbContext context)
        {
            _service = new WorkOrdersService(context);
        }

        // GET: api/workorders
        [HttpGet]
        public async Task<ActionResult<List<WorkOrder>>> Get()
        {
            return Ok(await _service.GetWorkOrders());
        }

        // GET api/workorders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkOrder>> Get(int id)
        {
            return Ok(await _service.GetWorkOrder(id));
        }

        // POST api/workorders
        [HttpPost]
        public async Task<ActionResult<WorkOrder>> Post([FromBody] WorkOrder work)
        {
            (WorkOrder added, string message) = await _service.CreateWorkOrder(work);

            if (added is null)
            {
                return BadRequest(message);
            }

            return Ok(added);
        }

    }
}
