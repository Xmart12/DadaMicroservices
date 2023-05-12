using DadaRepositories.Contexts;
using DadaRepositories.Models;
using InvoicesMicroservice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoicesService _service;

        public InvoicesController(DadaDbContext context)
        {
            _service = new InvoicesService(context);
        }

        // GET: api/invoices
        [HttpGet]
        public async Task<ActionResult<List<Invoice>>> Get()
        {
            return Ok(await _service.GetInvoices());
        }

        // GET api/invoices/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> Get(int id)
        {
            return Ok(await _service.GetInvoice(id));
        }

        // POST api/invoices
        [HttpPost]
        public async Task<ActionResult<Invoice>> Post([FromBody] Invoice invoice)
        {
            (Invoice added, string message) = await _service.CreateInvoice(invoice);

            if (added is null)
            {
                return BadRequest(message);
            }

            return Ok(added);
        }
    }
}
