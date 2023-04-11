using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SalesMicroservice.Models;
using SalesMicroservice.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMicroservice.Controllers
{
    /// <summary>
    /// Sales Report Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SalesReportController : ControllerBase
    {
        /// <summary>
        /// Sales Report Repository
        /// </summary>
        private readonly SalesReportRepository _repository;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration interface</param>
        public SalesReportController(IConfiguration configuration)
        {
            _repository = new SalesReportRepository(configuration);
        }


        // GET: api/salesreport
        /// <summary>
        /// Get all sales report records
        /// </summary>
        /// <returns>List of Sales reports</returns>
        [HttpGet]
        public async Task<ActionResult<List<SalesReport>>> Get()
        {
            return Ok(await _repository.GetAllAsync());
        }


        // GET api/salesreport/{id}
        /// <summary>
        /// Get sales report by Id
        /// </summary>
        /// <param name="id">Id record to find</param>
        /// <returns>Sales report record</returns>
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

        // POST api/salesreport
        /// <summary>
        /// Create sales report
        /// </summary>
        /// <param name="sales">Sales Report Model</param>
        /// <returns>Sales Report Model</returns>
        [HttpPost]
        public async Task<ActionResult<SalesReport>> Post([FromBody] SalesReport sales)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());
            }

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
