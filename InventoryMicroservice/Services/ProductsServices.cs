using DadaRepositories;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryMicroservice.Services
{
    public class ProductsServices
    {
        private readonly FirestoreRepository _product;

        public ProductsServices(IConfiguration configuration)
        {
            string filepath = configuration.GetValue<string>("Settings:FirebaseSettings:FilePath");
            string projectid = configuration.GetValue<string>("Settings:FirebaseSettings:ProjectId");
            _product = new FirestoreRepository(filepath, projectid, Collection.Products);
        }


        public async Task<List<Product>> GetProducts() => await _product.GetAllAsync<Product>();

        public async Task<Product> GetProduct(string id) => await _product.GetAsync<Product>(id);

        public async Task<Product> CreateProduct(Product product)
        {
            product.Id = product.Code;

            if ((await GetProduct(product.Id)) != null)
            {
                throw new Exception();
            }

            return await _product.AddAsync(product);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            Product sr = await _product.GetAsync<Product>(product.Id);

            if (sr is null)
            {
                throw new Exception();
            }

            sr.Description = product.Description;
            sr.Cost = product.Cost;

            return await _product.UpdateAsync(sr);
        }
    }
}
