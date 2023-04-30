using DadaRepositories.Contexts;
using DadaRepositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PurchasesMicroservice.Services
{
    public class PurchasesService
    {
        private readonly DadaDbContext _context;

        public PurchasesService(DadaDbContext context)
        {
            _context = context;
        }


        public async Task<List<Purchase>> GetPurchases() => await _context.Purchases
            .Include(i => i.Details).ThenInclude(i => i.Product)
            .ToListAsync();


        public async Task<Purchase> GetPurchase(int id) => await _context.Purchases
            .Include(i => i.Details).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(f => f.Id == id);


        public async Task<Purchase> CreatePurchase(Purchase purchase)
        {
            var result = await _context.Purchases.AddAsync(purchase);
            await _context.SaveChangesAsync();

            if (result.State == EntityState.Added)
            {
                purchase.Details.ForEach(f =>
                {
                    f.PurchaseId = result.Entity.Id;

                    _context.PurchaseDetails.Add(f);
                });

                await _context.SaveChangesAsync();
            }

            return await GetPurchase(result.Entity.Id);
        }


        public async Task<Purchase> UpdateSalesReport(Purchase purchase)
        {
            Purchase pr = await GetPurchase(purchase.Id);

            if (pr is null)
            {
                throw new Exception();
            }

            pr.Description = purchase.Description;
            _context.Entry(pr).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            pr.Details.ForEach(f =>
            {
                _context.PurchaseDetails.Remove(f);
            });
            purchase.Details.ForEach(f =>
            {
                _context.PurchaseDetails.Add(f);
            });
            await _context.SaveChangesAsync();

            return purchase;
        }

    }
}
