
using DadaRepositories.Contexts;
using DadaRepositories.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PurchasesMicroservice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PurchasesMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchasesService _service;

        public PurchasesController(DadaDbContext context)
        {
            _service = new PurchasesService(context);
        }


        // GET: api/purchases
        /// <summary>
        /// Get all purchases records
        /// </summary>
        /// <returns>List of Purchases reports</returns>
        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> Get()
        {
            return Ok(await _service.GetPurchases());
        }


        // GET api/purchases/{id}
        /// <summary>
        /// Get purchases report by Id
        /// </summary>
        /// <param name="id">Id record to find</param>
        /// <returns>Purchases record</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> Get(int id)
        {
            Purchase sales = await _service.GetPurchase(id);

            if (sales is null)
            {
                return NotFound();
            }

            return Ok(sales);
        }

        // POST api/purchases
        /// <summary>
        /// Create purchase
        /// </summary>
        /// <param name="sales">Purchase Model</param>
        /// <returns>Purchase Model</returns>
        [HttpPost]
        public async Task<ActionResult<Purchase>> Post([FromBody] Purchase sales)
        {
            Purchase added = await _service.CreatePurchase(sales);

            if (added is null)
            {
                return BadRequest();
            }

            return Ok(added);
        }
    }
}
