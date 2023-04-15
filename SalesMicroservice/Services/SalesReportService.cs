using DadaRepositories;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMicroservice.Services
{
    public class SalesReportService
    {
        private readonly FirestoreRepository _sales;
        private readonly FirestoreRepository _customers;
        private readonly FirestoreRepository _conf;

        public SalesReportService(IConfiguration configuration)
        {
            string filepath = configuration.GetValue<string>("Settings:FirebaseSettings:FilePath");
            string projectid = configuration.GetValue<string>("Settings:FirebaseSettings:ProjectId");
            _sales = new FirestoreRepository(filepath, projectid, Collection.SalesReports);
            _customers = new FirestoreRepository(filepath, projectid, Collection.Customers);
            _conf = new FirestoreRepository(filepath, projectid, Collection.Configurations);
        }


        public async Task<List<SalesReport>> GetSalesReports() => await _sales.GetAllAsync<SalesReport>();

        public async Task<SalesReport> GetSalesReport(string id) => await _sales.GetAsync<SalesReport>(id);

        public async Task<SalesReport> CreateSalesReport(SalesReport sales)
        {
            if (sales.CustomerDocument is null)
            {
                if (sales.Customer is null)
                {
                    throw new Exception();
                }

                if ((await _customers.GetAsync<Customer>(sales.Customer.Document)) is null)
                {
                    sales.Customer.Id = sales.Customer.Document;
                    sales.CustomerDocument = sales.Customer.Document;
                    await _customers.AddAsync(sales.Customer);
                }
            }

            if (sales.Customer is null)
            {
                sales.Customer = await _customers.GetAsync<Customer>(sales.CustomerDocument);

                if (sales.Customer is null)
                {
                    throw new Exception();
                }
            }

            Configuration salesReportConf = await _conf.GetAsync<Configuration>("SalesReport");

            int.TryParse(salesReportConf.LastId, out int correlative);
            correlative++;

            sales.Id = correlative.ToString();
            int detailId = 1;
            sales.Details.ForEach(f =>
            {
                f.SalesReportId = correlative;
                f.Id = detailId;
                detailId++;
            });    
            
            sales = await _sales.AddAsync(sales);

            salesReportConf.LastId = correlative.ToString();
            await _conf.UpdateAsync(salesReportConf);

            return sales;
        }

        public async Task<SalesReport> UpdateSalesReport(SalesReport sales)
        {
            SalesReport sr = await _sales.GetAsync<SalesReport>(sales.Id);

            if (sr is null)
            {
                throw new Exception();
            }

            sr.Description = sales.Description;
            sr.Details = sales.Details;

            return await _sales.UpdateAsync(sr);
        }
    }
}
