using DadaRepositories;
using DadaRepositories.Models;
using DadaRepositories.Utilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PurchasesMicroservice.Services
{
    public class PurchasesService
    {
        private readonly FirestoreRepository _purchases;
        private readonly FirestoreRepository _conf;

        public PurchasesService(IConfiguration configuration)
        {
            string filepath = configuration.GetValue<string>("Settings:FirebaseSettings:FilePath");
            string projectid = configuration.GetValue<string>("Settings:FirebaseSettings:ProjectId");
            _purchases = new FirestoreRepository(filepath, projectid, Collection.Purchases);
            _conf = new FirestoreRepository(filepath, projectid, Collection.Configurations);
        }


        public async Task<List<Purchase>> GetPurchases() => await _purchases.GetAllAsync<Purchase>();

        public async Task<Purchase> GetPurchase(string id) => await _purchases.GetAsync<Purchase>(id);

        public async Task<Purchase> CreatePurchase(Purchase purchase)
        {
            Configuration purchasesConf = await _conf.GetAsync<Configuration>("Purchases");

            int.TryParse(purchasesConf.LastId, out int correlative);
            correlative++;

            purchase.Id = correlative.ToString();
            int detailId = 1;
            purchase.Details.ForEach(f =>
            {
                f.PurchaseId = correlative;
                f.Id = detailId;
                f.Line = detailId;
                detailId++;
            });

            purchase = await _purchases.AddAsync(purchase);

            purchasesConf.LastId = correlative.ToString();
            await _conf.UpdateAsync(purchasesConf);

            return purchase;
        }

        public async Task<Purchase> UpdateSalesReport(Purchase purchase)
        {
            Purchase sr = await _purchases.GetAsync<Purchase>(purchase.Id);

            if (sr is null)
            {
                throw new Exception();
            }

            sr.Description = purchase.Description;
            sr.Details = purchase.Details;

            return await _purchases.UpdateAsync(sr);
        }
    }
}
