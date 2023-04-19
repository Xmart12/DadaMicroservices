using DadaRepositories.Models;
using InventoryMicroservice.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsServices _service;

        public ProductsController(IConfiguration configuration)
        {
            _service = new ProductsServices(configuration);
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            return Ok(await _service.GetProducts());
        }


        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(string id)
        {
            Product product = await _service.GetProduct(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] Product product)
        {
            Product added = await _service.CreateProduct(product);

            if (added is null)
            {
                return BadRequest();
            }

            return Ok(added);
        }

    }
}
