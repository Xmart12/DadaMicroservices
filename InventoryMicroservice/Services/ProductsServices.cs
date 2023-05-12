using DadaRepositories.Contexts;
using DadaRepositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Services
{
    public class ProductsServices
    {
        private readonly DadaDbContext _context;

        public ProductsServices(DadaDbContext context)
        {
            _context = context;
        }


        public async Task<List<Product>> GetProducts() => await _context.Products.AsQueryable()
            .ToListAsync();


        public async Task<Product> GetProduct(string id) => await _context.Products.AsQueryable()
            .FirstOrDefaultAsync(p => p.Id == id);


        public async Task<(Product, string)> CreateProduct(Product product)
        {
            string message = null;

            if ((await GetProduct(product.Id)) != null)
            {
                message = "Product already exists";
                return (null, message);
            }

            var res = await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            if (res.State != EntityState.Added)
            {
                message = "Unable to create product";
                return (null, message);
            }

            return (res.Entity, message);
        }


        public async Task<(Product, string)> UpdateProduct(Product product)
        {
            string message = null;

            Product pr = await GetProduct(product.Id);

            if (pr is null)
            {
                message = "Product not found";
                return (null, message);
            }

            pr.Description = product.Description;
            pr.Cost = product.Cost;
            _context.Entry(pr).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return (pr, message);
        }
    }
}
