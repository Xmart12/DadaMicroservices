using DadaRepositories;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoicesMicroservice.Services
{
    public class InvoicesService
    {
        private readonly FirestoreRepository _invoice;
        private readonly FirestoreRepository _customers;
        private readonly FirestoreRepository _conf;

        public InvoicesService(IConfiguration configuration)
        {
            string filepath = configuration.GetValue<string>("Settings:FirebaseSettings:FilePath");
            string projectid = configuration.GetValue<string>("Settings:FirebaseSettings:ProjectId");
            _invoice = new FirestoreRepository(filepath, projectid, Collection.Invoices);
            _customers = new FirestoreRepository(filepath, projectid, Collection.Customers);
            _conf = new FirestoreRepository(filepath, projectid, Collection.Configurations);
        }


        public async Task<List<Invoice>> GetInvoices() => await _invoice.GetAllAsync<Invoice>();


        public async Task<Invoice> GetInvoice(string id) => await _invoice.GetAsync<Invoice>(id);


        public async Task<Invoice> CreateInvoice(Invoice invoice)
        {
            Configuration invConf = await _conf.GetAsync<Configuration>("Invoice");

            int.TryParse(invConf.LastId, out int correlative);
            correlative++;

            invoice.Id = correlative.ToString();
            int detailId = 1;
            invoice.Details.ForEach(f =>
            {
                f.WorkOrderId = correlative.ToString();
                f.Id = detailId.ToString();
                detailId++;
            });

            invoice = await _invoice.AddAsync(invoice);

            invConf.LastId = correlative.ToString();
            await _conf.UpdateAsync(invConf);

            return invoice;

        }
    }
}
