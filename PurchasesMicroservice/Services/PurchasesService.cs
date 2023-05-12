using DadaRepositories.Contexts;
using DadaRepositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<(Purchase, string)> CreatePurchase(Purchase purchase)
        {
            string message = null;

            if (purchase.Date > DateTime.Today)
            {
                message = "Date not valid";
                return (null, message);
            }

            purchase.Details.ForEach(f =>
            {
                Product prod = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);

                if (prod == null)
                {
                    message = $"The item {f.ProductId} is not found";
                    return;
                }
            });

            if (message != null)
            {
                return (null, message);
            }

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

            return (await GetPurchase(result.Entity.Id), null);
        }


        public async Task<(Purchase, string)> UpdateSalesReport(Purchase purchase)
        {
            string message = null;

            Purchase pr = await GetPurchase(purchase.Id);

            if (pr is null)
            {
                message = "Purchase not found";
                return (null, message);
            }

            purchase.Details.ForEach(f =>
            {
                Product prod = _context.Products.FirstOrDefault(p => p.Id == f.ProductId);

                if (prod == null)
                {
                    message = $"The item {f.ProductId} is not found";
                    return;
                }
            });

            if (message != null)
            {
                return (null, message);
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

            return (purchase, null);
        }

    }
}
