using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SalesMicroservice.Models;
using SalesMicroservice.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SalesMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReportController : ControllerBase
    {

        private readonly IConfiguration _configuration;

        private readonly SalesReportRepository _repository;

        public SalesReportController(IConfiguration configuration)
        {
            _configuration = configuration;
            _repository = new SalesReportRepository(configuration);
        }

        // GET: api/<SalesReportController>
        [HttpGet]
        public async Task<ActionResult<List<SalesReport>>> Get()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET api/<SalesReportController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesReport>> Get(string id)
        {
            SalesReport sales = await _repository.GetAsync(id);

            if (sales is null)
            {
                return NotFound();
            }

            return Ok(sales);
        }

        // POST api/<SalesReportController>
        [HttpPost]
        public async Task<ActionResult<SalesReport>> Post([FromBody] SalesReport sales)
        {
            SalesReport added = await _repository.AddAsync(sales);

            if (added is null)
            {
                return BadRequest();
            }

            return Ok(added);
        }

        // PUT api/<SalesReportController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SalesReportController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
